using Newtonsoft.Json.Linq;
using SP.Provider.Game.Models;
using SP.StudioCore.Array;
using SP.StudioCore.Http;
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
            JObject info = this.POST("/login", new Dictionary<string, object>()
            {
                {"UserName", user.UserName },
                {"Password", user.Password },
                {"Game", user.Game },
                {"Category", user.Category },
                {"Code", user.Code }
            });
            if (info == null) return new LoginResult(LoginStatus.Exception);

            int success = info["success"].Value<int>();
            if (success == 0)
            {
                // 发生了已知错误

            }

            info = info["info"].Value<JObject>();
            HttpMethod method = info["Method"] != null && info["Method"].Value<string>() == "Post" ? HttpMethod.Post : HttpMethod.Get;
            Dictionary<string, object> data = null;
            if (info["Data"] != null) data = info["Data"].Value<Dictionary<string, object>>();
            return new LoginResult(info["Url"].Value<string>(), method, data);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool Register(LoginUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送数据到网关
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private JObject POST(string method, Dictionary<string, object> data)
        {
            string url = $"{Gateway}{method}";
            string resultData = string.Empty;
            string postData = data.ToQueryString();
            bool success = false;
            try
            {
                resultData = NetAgent.UploadData(url, postData, Encoding.UTF8, null, new Dictionary<string, string>()
                {
                    {"Authorization", $"{Merchant}:{SecretKey}".ToBasicAuth() }
                });
                JObject info = JObject.Parse(resultData);
                success = info["success"].Value<int>() == 1;
                return info;
            }
            catch (Exception ex)
            {
                resultData = ex.Message + resultData;
                return null;
            }
            finally
            {
                this.SaveLog(url, postData, resultData, success);
            }
        }
    }
}
