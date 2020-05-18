using Newtonsoft.Json.Linq;
using SP.Provider.Game.Models;
using SP.StudioCore.Enums;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Linq;
using SP.StudioCore.Array;
using SP.StudioCore.Json;
using SP.StudioCore.Types;

namespace SP.Provider.Game.Clients
{
    [Description("泛亚电竞")]
    public sealed class AVIA : IGameProvider
    {
        public AVIA()
        {
        }

        public AVIA(string queryString) : base(queryString)
        {
        }

        #region ========  配置参数  ========

        [Description("网关")]
        public string Gateway { get; set; }

        [Description("商户")]
        public int SiteID { get; set; }

        [Description("密钥")]
        public string SecretKey { get; set; }

        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override LoginResult Login(LoginUser user)
        {
            ResultStatus status = this.POST("/api/user/login", new Dictionary<string, object>()
            {
                {"UserName",user.UserName }
            }, out JObject info);
            if (status != ResultStatus.Success) return new LoginResult(status);

            string url = info["Url"].Value<string>();

            return new LoginResult(url, HttpMethod.Post);
        }

        /// <summary>
        /// 游客登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override LoginResult Guest(LoginUser user)
        {
            ResultStatus status = this.POST("/api/user/guest", new Dictionary<string, object>()
            {
                {"Language","" }
            }, out JObject info);
            if (status != ResultStatus.Success) return new LoginResult(status);

            string url = info["Url"].Value<string>();

            return new LoginResult(url, HttpMethod.Post);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override RegisterResult Register(RegisterUser user)
        {
            ResultStatus status = ResultStatus.Exception;
            // 用户名重复的问题重试5次
            for (int i = 0; i < 5; i++)
            {
                //处理用户名前缀+UserName+4位随机数
                string username = this.GetPlayerName(user.Prefix, user.UserName, i);
                string password = Guid.NewGuid().ToString("N").Substring(0, 8);
                status = this.POST("/api/user/register", new Dictionary<string, object>()
                {
                    {"UserName",username },
                    {"Password",password }
                }, out JObject _);
                if (status == ResultStatus.ExistsUser) continue;
                if (status != ResultStatus.Success) return new RegisterResult(status);
                return new RegisterResult(username, password);
            }
            return new RegisterResult(status);
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override TransferResult Transfer(TransferInfo transferInfo)
        {
            string systemId = this.GetSystemID(transferInfo.Prefix);
            ResultStatus status = this.POST("/api/user/Transfer", new Dictionary<string, object>()
            {
                {"UserName",transferInfo.UserName },
                {"Type",transferInfo.Action },
                {"Currency",transferInfo.Currency },
                {"Money",transferInfo.Money },
                {"ID",systemId },
            }, out JObject info);

            if (status != ResultStatus.Success) return new TransferResult(status);

            decimal balance = info["Balance"].Value<decimal>();
            decimal credit = info["Credit"].Value<decimal>();
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"Credit",credit }
            };

            return new TransferResult(systemId, balance, data);
        }

        /// <summary>
        /// 转账查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override QueryTransferResult QueryTransfer(QueryTransferInfo queryTransferInfo)
        {
            ResultStatus status = this.POST("/api/user/transferinfo", new Dictionary<string, object>()
            {
                {"ID",queryTransferInfo.OrderID },
                {"Currency",queryTransferInfo.Currency },
            }, out JObject _);
            return status;
        }

        /// <summary>
        /// 余额
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override BalanceResult Balance(BalanceInfo balanceInfo)
        {
            ResultStatus status = this.POST("/api/user/balance", new Dictionary<string, object>()
            {
                {"ID",balanceInfo.UserName },
                {"Currency",balanceInfo.Currency },
            }, out JObject info);

            if (status != ResultStatus.Success) return new BalanceResult(status);

            decimal balance = info.Get<decimal>("Money");
            decimal credit = info.Get<decimal>("Credit");

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"Credit",credit }
            };

            return new BalanceResult(balance, data);
        }

        /// <summary>
        /// 采集订单
        /// </summary>
        /// <param name="gameCode"></param>
        /// <returns></returns>
        public override IEnumerable<OrderModel> GetOrderLog(OrderTaskModel task)
        {
            //#2 获取当前任务开始时间（-5为安全保障时间）
            long start = this.GetStartMark(task);
            DateTime startAt = WebAgent.GetTimestamps(start);
            DateTime endAt = WebAgent.GetTimestamps(this.GetEndMark(task, start));

            this.GameDelegate.SaveOrderTaskStatus(task, OrderTaskStatus.Collecting);
            int index = 1;
            while (true)
            {
                ResultStatus resultStatus = this.POST("/api/log/get", new Dictionary<string, object>()
                {
                    {"Type","UpdateAt" },
                    {"OrderType","All" },
                    { "StartAt", this.TimeRevert(startAt).ToString() },
                    { "EndAt",this.TimeRevert(endAt).ToString() },
                    { "PageIndex",index },
                    { "PageSize",1024 }
                }, out JObject info);

                if (resultStatus != ResultStatus.Success) yield break;
                foreach (JObject item in info["list"])
                {
                    yield return this.GetOrderInfo(item, task);
                }
                int pageSize = info.Get<int>("PageSize");
                int recordCount = info.Get<int>("RecordCount");
                int pageIndex = info.Get<int>("PageIndex");
                if (recordCount > pageIndex * pageSize)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }
            this.GameDelegate.SaveMarkTime(task, WebAgent.GetTimestamps(endAt));
            this.GameDelegate.SaveOrderTaskStatus(task, OrderTaskStatus.Ended);
        }

        #region ========  解析订单  ========

        private OrderModel GetOrderInfo(JObject item, string gameCode)
        {
            return item.Get<string>("Type") switch
            {
                "Single" => this.GetSingleOrder(item),
                "Combo" => this.GetComboOrder(item),
                "Smart" => this.GetSmartOrder(item),
                _ => null
            };
        }

        /// <summary>
        /// 单关订单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private OrderModel GetSingleOrder(JObject item)
        {
            return new OrderModel()
            {
                Category = GameCategory.ESport,
                Provider = this.Provider,
                SourceID = item.Get<string>("OrderID"),
                UserName = item.Get<string>("UserName"),
                Code = item.Get<string>("CateID"),
                BetMoney = item.Get<decimal>("BetMoney"),
                BetAmount = item.Get<decimal>("BetAmount"),
                Money = item.Get<decimal>("Money"),
                Status = item.Get<string>("Status") switch
                {
                    "None" => OrderStatus.Wait,
                    "Revoke" => OrderStatus.Return,
                    "Win" => OrderStatus.Win,
                    "Lose" => OrderStatus.Lose,
                    "WinHalf" => OrderStatus.Win,
                    "LoseHalf" => OrderStatus.Lose,
                    "Settlement" => OrderStatus.Wait,
                    "Cancel" => OrderStatus.Return,
                    _ => OrderStatus.Wait
                },
                CreateAt = this.TimeConvert(item.Get<DateTime>("CreateAt")),
                ResultAt = this.TimeConvert(item.Get<DateTime>("ResultAt")),
                Content = $"{ item.Get<string>("Category") } / { item.Get<string>("Match") } / { item.Get<string>("Bet") } / { item.Get<string>("Content") }",
                RawData = item.ToString()
            };
        }

        /// <summary>
        /// 串关订单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private OrderModel GetComboOrder(JObject item)
        {
            return new OrderModel()
            {
                Category = GameCategory.ESport,
                Provider = this.Provider,
                SourceID = item.Get<string>("OrderID"),
                UserName = item.Get<string>("UserName"),
                Code = "Combo",
                BetMoney = item.Get<decimal>("BetMoney"),
                BetAmount = item.Get<decimal>("BetAmount"),
                Money = item.Get<decimal>("Money"),
                Status = item.Get<string>("Status") switch
                {
                    "None" => OrderStatus.Wait,
                    "Revoke" => OrderStatus.Return,
                    "Win" => OrderStatus.Win,
                    "Lose" => OrderStatus.Lose,
                    "WinHalf" => OrderStatus.Win,
                    "LoseHalf" => OrderStatus.Lose,
                    "Settlement" => OrderStatus.Wait,
                    "Cancel" => OrderStatus.Return,
                    _ => OrderStatus.Wait
                },
                CreateAt = this.TimeConvert(item.Get<DateTime>("CreateAt")),
                ResultAt = this.TimeConvert(item.Get<DateTime>("ResultAt")),
                //Content = item.Get<string>("Content"), 需要确定显示成什么格式，在明细里
                RawData = item.ToString(),
            };
        }

        /// <summary>
        /// 小游戏订单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private OrderModel GetSmartOrder(JObject item)
        {
            return new OrderModel()
            {
                Category = GameCategory.Slot,
                Provider = this.Provider,
                SourceID = item.Get<string>("OrderID"),
                UserName = item.Get<string>("UserName"),
                Code = item.Get<string>("Code"),
                BetMoney = item.Get<decimal>("BetMoney"),
                BetAmount = item.Get<decimal>("BetAmount"),
                Money = item.Get<decimal>("Money"),
                Status = item.Get<string>("Status") switch
                {
                    "None" => OrderStatus.Wait,
                    "Revoke" => OrderStatus.Return,
                    "Win" => OrderStatus.Win,
                    "Lose" => OrderStatus.Lose,
                    "WinHalf" => OrderStatus.Win,
                    "LoseHalf" => OrderStatus.Lose,
                    "Settlement" => OrderStatus.Wait,
                    "Cancel" => OrderStatus.Return,
                    _ => OrderStatus.Wait
                },
                CreateAt = this.TimeConvert(item.Get<DateTime>("CreateAt")),
                ResultAt = this.TimeConvert(item.Get<DateTime>("RewardAt")),
                Content = item.Get<string>("Content"),
                RawData = item.ToString(),
            };
        }

        #endregion

        /// <summary>
        /// 状态码转换
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override ResultStatus GetStatus(string result)
        {
            try
            {
                JObject json = JObject.Parse(result);
                if (json["success"].Value<int>() == 1) return ResultStatus.Success;

                JObject info = json["info"].Value<JObject>();
                string status = info["Error"] == null ? string.Empty : info["Error"].Value<string>();
                return status switch
                {
                    "NOUSER" => ResultStatus.NoUser,
                    "EXISTSUSER" => ResultStatus.ExistsUser,
                    "USERLOCK" => ResultStatus.UserLock,
                    "EXISTSORDER" => ResultStatus.ExistsOrder,
                    "ORDERFORMATERROR" => ResultStatus.OrderIDFormat,
                    "NOBALANCE" => ResultStatus.NoBalance,
                    _ => ResultStatus.Exception
                };
            }
            catch
            {
                return ResultStatus.Exception;
            }
        }



        #region ========  本地工具方法  ========

        /// <summary>
        /// 封装之后的API交互
        /// </summary>
        /// <returns></returns>
        private ResultStatus POST(string method, Dictionary<string, object> data, out JObject info)
        {
            string url = $"{this.Gateway}{method}";
            ResultStatus status = this.POST(url, new Dictionary<string, string>()
            {
                {"Authorization",this.SecretKey }
            }, data, out string resultContent);
            if (status != ResultStatus.Success)
            {
                info = null;
                return status;
            }
            JObject result = JObject.Parse(resultContent);
            info = (JObject)result["info"];
            return status;
        }

        #endregion
    }
}
