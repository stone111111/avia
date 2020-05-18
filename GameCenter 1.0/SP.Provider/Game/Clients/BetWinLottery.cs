using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SP.Provider.Game.Models;
using SP.Provider.Game.Receives;
using SP.StudioCore.Array;
using SP.StudioCore.Http;
using SP.StudioCore.Json;
using SP.StudioCore.Model;
using SP.StudioCore.Net;
using SP.StudioCore.Security;
using SP.StudioCore.Types;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace SP.Provider.Game.Clients
{
    /// <summary>
    /// 贝盈彩票
    /// </summary>
    [Description("贝盈彩票")]
    public sealed class BetWinLottery : IGameProvider, IGameReceive
    {
        public BetWinLottery()
        {
        }

        public BetWinLottery(string queryString) : base(queryString)
        {
        }

        #region ========  配置参数  ========

        [Description("网关")]
        public string Gateway { get; set; }

        [Description("商户号")]
        public int SiteID { get; set; }

        [Description("密钥")]
        public string SecretKey { get; set; }

        #endregion

        protected override long GetStartMark(OrderTaskModel task, byte mark = 0)
        {
            return this.GameDelegate.GetMarkTime(task, mark);
        }

        public override BalanceResult Balance(BalanceInfo info)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<OrderModel> GetOrderLog(OrderTaskModel task)
        {
            long time = this.GetStartMark(task);

            SortedDictionary<string, object> data = new SortedDictionary<string, object>()
            {
                {"SiteID",this.SiteID },
                {"Time", WebAgent.GetTimestamp() },
                {"Update",time },
                {"Count",100 }
            };
            ResultStatus resultStatus = this.POST("Orders", data, out JToken info);
            if (resultStatus != ResultStatus.Success) yield break;

            foreach (JObject item in info.Value<JArray>())
            {
                OrderStatus status = OrderStatus.Wait;
                switch (item["Status"].Value<string>())
                {
                    case "None":
                        status = OrderStatus.Wait;
                        break;
                    case "Revoke":
                        status = OrderStatus.Return;
                        break;
                    case "Win":
                        status = OrderStatus.Win;
                        break;
                    case "Lose":
                        status = OrderStatus.Lose;
                        break;
                }

                long updateTime = WebAgent.GetTimestamp(item.Get<DateTime>("CreateAt")
                    .Max(item.Get<DateTime>("ResultAt"))
                    .Max(item.Get<DateTime>("RewardAt")));

                yield return new OrderModel()
                {
                    Provider = this.GetType().Name,
                    Code = item.Get<string>("Code"),
                    UserName = item.Get<string>("UserName"),
                    Category = GameCategory.Lottery,
                    CreateAt = item.Get<DateTime>("CreateAt"),
                    ResultAt = item.Get<DateTime>("ResultAt"),
                    BetMoney = item.Get<decimal>("BetMoney"),
                    BetAmount = item.Get<decimal>("BetAmount"),
                    Content = $"{ item.Get<string>("Index") } - { item.Get<string>("PlayCode") } / { item.Get<string>("Number") } / { item.Get<string>("Result") }",
                    Money = item.Get<decimal>("Money"),
                    SourceID = item.Get<string>("ID"),
                    Status = status,
                    RawData = item.ToString()
                };
                if (updateTime > time) time = updateTime;
            }
            this.GameDelegate.SaveMarkTime(task, time);

        }

        public override LoginResult Guest(LoginUser guest)
        {
            return new LoginResult(ResultStatus.NoUser);
        }

        public override LoginResult Login(LoginUser user)
        {
            Regex regex = new Regex(@"^(?<Group>\w+?)_");
            SortedDictionary<string, object> data = new SortedDictionary<string, object>()
            {
                {"SiteID",this.SiteID },
                {"UserID", user.UserName },
                {"UserName",user.UserName },
                {"Code", user.Code },
                {"Time", WebAgent.GetTimestamp() }
            };
            if (regex.IsMatch(user.UserName)) data.Add("Group", regex.Match(user.UserName).Groups["Group"].Value);
            ResultStatus status = this.POST("Login", data, out JToken res);
            if (status == ResultStatus.Success)
            {
                return new LoginResult(((JObject)res).Get<string>("Url"), HttpMethod.Get);
            }
            return new LoginResult(status);
        }

        public override QueryTransferResult QueryTransfer(QueryTransferInfo info)
        {
            throw new NotImplementedException();
        }

        public override RegisterResult Register(RegisterUser user)
        {
            return new RegisterResult(string.Concat(user.Prefix, "_", user.UserName), user.Password);
        }

        public override TransferResult Transfer(TransferInfo info)
        {
            throw new NotImplementedException();
        }

        protected override ResultStatus GetStatus(string result)
        {
            try
            {
                JObject res = JObject.Parse(result);
                bool success = res.Get<int>("success") == 1;
                if (success) return ResultStatus.Success;
                return res.Get<string>("msg") switch
                {
                    "密钥错误" => ResultStatus.SecretKey,
                    _ => ResultStatus.Faild
                };
            }
            catch
            {
                return ResultStatus.Exception;
            }
        }


        /// <summary>
        /// 共用的消息发送方法
        /// </summary>
        /// <returns></returns>
        private ResultStatus POST(string action, SortedDictionary<string, object> data, out JToken info)
        {
            string signData = data.ToQueryString() + "&key=" + this.SecretKey;
            data.Add("Sign", Encryption.toMD5(signData));

            string url = $"{this.Gateway}/{action}";
            string postData = data.ToJson();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            this.POST(url, new Dictionary<string, string>()
                {
                    { "x-site", this.SiteID.ToString() }
                }, postData, out string result);

            ResultStatus status = this.GetStatus(result);
            if (status != ResultStatus.Success)
            {
                Console.WriteLine(result);
                info = null;
                return status;
            }
            info = JObject.Parse(result)["info"];
            return ResultStatus.Success;
        }

        #region ========  接收免转推送信息  ========

        /// <summary>
        /// 检查数据是否正确
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private ResultStatus CheckData(JObject info)
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            string sign = string.Empty;
            long time = 0;
            foreach (KeyValuePair<string, JToken> item in info)
            {
                string key = item.Key;
                switch (key)
                {
                    case "Sign":
                        sign = item.Value.ToString();
                        continue;
                    case "Time":
                        time = item.Value.ToString().GetValue<long>();
                        break;
                }
                dic.Add(item.Key, item.Value.ToString());
            }
            long now = WebAgent.GetTimestamp();
            if (time > now - 150 || time < now + 150) return ResultStatus.Timeout;
            string signStr = dic.ToQueryString() + "&key=" + this.SecretKey;
            if (sign != Encryption.toMD5(signStr)) return ResultStatus.SecretKey;

            return ResultStatus.Success;
        }

        /// <summary>
        /// 返回公共的错误信息
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private Result ShowError(ResultStatus status)
        {
            return new Result(false, status.ToString());
        }

        public Result GetBalanceReceive(HttpContext context)
        {
            string data = context.GetString();
            if (string.IsNullOrEmpty(data)) return this.ShowError(ResultStatus.Empty);

            JObject info = JObject.Parse(data);
            ResultStatus checkStatus = this.CheckData(info);
            if (checkStatus != ResultStatus.Success) return this.ShowError(checkStatus);

            string userName = info["UserID"].Value<string>();

            decimal? balance = this.GameDelegate.GetBalance(this.Provider, userName);
            if (balance == null) return this.ShowError(ResultStatus.NoUser);

            return new Result(true, userName, new
            {
                Balance = balance.Value
            });
        }

        public Result GetMoneyReceive(HttpContext context)
        {
            string data = context.GetString();
            if (string.IsNullOrEmpty(data)) return this.ShowError(ResultStatus.Empty);

            JObject info = JObject.Parse(data);
            ResultStatus checkStatus = this.CheckData(info);
            if (checkStatus != ResultStatus.Success) return this.ShowError(checkStatus);

            throw new NotImplementedException();
        }

        public IEnumerable<OrderModel> GetOrders(HttpContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
