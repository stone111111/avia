using Newtonsoft.Json.Linq;
using SP.StudioCore.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace SP.StudioCore.Gateway.Push
{
    public class GoEasy : IPush
    {
        /// <summary>
        /// Common key 既可以发送消息或也可以订阅channel来接收消息
        /// </summary>
        [Description("commonkey")]
        public string commonkey { get; set; } = "BC-50b33df8107a4437b9e14374cf6625d3";

        /// <summary>
        /// 推送地址
        /// </summary>
        [Description("rest")]
        public string rest { get; set; } = "http://rest-hangzhou.goeasy.io";

        /// <summary>
        /// 应用所在的区域地址: 【hangzhou.goeasy.io |singapore.goeasy.io】
        /// </summary>
        [Description("host")]
        public string host { get; set; } = "hangzhou.goeasy.io";

        /// <summary>
        /// 订阅密钥 只能用来订阅channel来接收消息
        /// </summary>
        [Description("appkey")]
        public string appkey { get; set; } = "BS-2c87f1c688f04e99902fe59eda36319d";

        public GoEasy(string queryString) : base(queryString)
        {
        }

        public override bool Send(object message, params string[] channel)
        {
            foreach (string channelName in channel)
            {
                string postDataStr = $"appkey={this.commonkey}&channel={channelName}&content={message.ToString()}";
                string result = NetAgent.UploadData($"{this.rest}/publish", Encoding.UTF8.GetBytes(postDataStr));
            }
            return true;
        }

        /// <summary>
        /// 获取客户端的配置信息
        /// </summary>
        /// <returns></returns>
        public override object Client()
        {
            return new
            {
                Type = PushType.GoEasy,
                this.appkey,
                this.host
            };
        }

        public override Dictionary<string, int> GetOnlineMember(params string[] channels)
        {
            string url = $"{this.rest}/herenow?appkey={commonkey}&{string.Join("&", channels.Select(t => $"channel={t}")) }";
            string result = NetAgent.DownloadData(url, Encoding.UTF8);
            JObject json = JObject.Parse(result);
            Dictionary<string, int> data = new Dictionary<string, int>();
            var content = json["content"]["channels"];
            foreach (var item in content)
            {
                string channel = item["channel"].Value<string>();
                int count = item["clientAmount"].Value<int>();
                data.Add(channel, count);
            }
            return data;
        }
    }
}
