using Newtonsoft.Json.Linq;
using SP.Provider.Game.Models;
using SP.StudioCore.Array;
using SP.StudioCore.Enums;
using SP.StudioCore.Http;
using SP.StudioCore.Json;
using SP.StudioCore.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SP.Provider.Game.Clients
{
    /// <summary>
    /// 游戏数据中心
    /// </summary>
    [Description("贝盈数据中心")]
    public sealed class BetWin : IGameProvider
    {
        /// <summary>
        /// API网关
        /// </summary>
        [Description("API网关")]
        public string Gateway { get; set; } = "https://api.betwin.ph";

        [Description("商户号")]
        public string Merchant { get; set; }

        [Description("密钥")]
        public string SecretKey { get; set; }

        public BetWin(string queryString) : base(queryString)
        {
        }

        public override LoginResult Login(LoginUser user)
        {
            ResultStatus status = this.POST("/login", new Dictionary<string, object>()
            {
                {"UserName", user.UserName },
                {"Password", user.Password },
                {"Game", user.Game },
                {"Category", user.Category },
                {"Code", user.Code }
            }, out JObject info);
            if (status != ResultStatus.Success) return new LoginResult(status);

            HttpMethod method = info["Method"] != null && info["Method"].Value<string>() == "Post" ? HttpMethod.Post : HttpMethod.Get;
            Dictionary<string, object> data = info.Get<Dictionary<string, object>>("Data");
            return new LoginResult(info["Url"].Value<string>(), method, data);
        }

        /// <summary>
        /// 试玩/游客
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override LoginResult Guest(LoginUser user)
        {
            ResultStatus status = this.POST("/guest", new Dictionary<string, object>()
            {
                {"Game", user.Game },
                {"Category", user.Category },
                {"Code", user.Code }
            }, out JObject info);
            if (status != ResultStatus.Success) return new LoginResult(status);

            HttpMethod method = info["Method"] != null && info["Method"].Value<string>() == "Post" ? HttpMethod.Post : HttpMethod.Get;
            Dictionary<string, object> data = info.Get<Dictionary<string, object>>("Data");
            return new LoginResult(info["Url"].Value<string>(), method, data);
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override TransferResult Transfer(TransferInfo info)
        {
            throw new NotImplementedException();
        }

        public override QueryTransferResult QueryTransfer(QueryTransferInfo info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override RegisterResult Register(RegisterUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 余额
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override BalanceResult Balance(BalanceInfo balanceInfo)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<OrderModel> GetOrderLog(OrderTaskModel task)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送数据到网关
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private ResultStatus POST(string method, Dictionary<string, object> data, out JObject info)
        {
            string url = $"{Gateway}{method}";
            ResultStatus status = base.POST(url, new Dictionary<string, string>()
                {
                    {"Authorization", $"{Merchant}:{SecretKey}".ToBasicAuth() }
                }, data, out string result);

            if (status == ResultStatus.Exception)
            {
                info = null;
                return status;
            }
            info = JObject.Parse(result)["info"].Value<JObject>();
            return status;
        }

        /// <summary>
        /// 获取错误的状态码
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override ResultStatus GetStatus(string result)
        {
            try
            {
                JObject info = JObject.Parse(result);
                if (info["success"].Value<int>() == 1) return ResultStatus.Success;
                return info["msg"].Value<string>().ToEnum<ResultStatus>();
            }
            catch
            {
                return ResultStatus.Exception;
            }
        }

    }
}
