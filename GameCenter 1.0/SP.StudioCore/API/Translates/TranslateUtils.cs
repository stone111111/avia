using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Enums;
using SP.StudioCore.Json;
using SP.StudioCore.Net;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace SP.StudioCore.API.Translates
{
    /// <summary>
    /// 翻译工具
    /// </summary>
    public static class TranslateUtils
    {
        /// <summary>
        /// 远程接口地址
        /// </summary>
        private static readonly string APIURL;

        private const string KEY = "TRANSLATE";

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// 本地的更新时间
        /// </summary>
        private static long UPDATETIME = 0;

        static TranslateUtils()
        {
            APIURL = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build()["studio:language"];

            if (string.IsNullOrEmpty(APIURL)) return;

            Console.WriteLine("触发翻译器静态构造");

            Timer timer = new Timer(60 * 1000)
            {
                Enabled = true
            };
            timer.Elapsed += (sender, e) =>
            {
                if (UPDATETIME != GetDataTime())
                {
                    Console.WriteLine("语言包存在更新");
                    MemoryUtils.Set(KEY, GetAPIData(), TimeSpan.FromDays(7));
                }
            };
            timer.Start();
        }


        /// <summary>
        /// 提交要保存的单个单词（异步）
        /// </summary>
        private static async Task PostWordAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return;
            Uri url = new Uri(APIURL + "&action=SaveWord");
            string postData = string.Concat("[\"", HttpUtility.JavaScriptStringEncode(keyword), "\"]");
            await client.PostAsync(url, new StringContent(postData)).ConfigureAwait(false);
        }

        #region ========  对外抛出的公共工具方法  ========

        /// <summary>
        /// 加载远程数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<long, Dictionary<Language, string>> GetAPIData()
        {
            if (string.IsNullOrEmpty(APIURL))
            {
                return null;
            }
            string result = NetAgent.UploadData(APIURL, "action=GetData");
            try
            {
                TranslateModel model = JsonConvert.DeserializeObject<TranslateModel>(result);
                UPDATETIME = model.msg;
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(result);
                return null;
            }
        }

        /// <summary>
        /// 获取频道的更新时间
        /// </summary>
        /// <returns></returns>
        public static long GetDataTime()
        {
            if (string.IsNullOrEmpty(APIURL)) return 0;
            string result = NetAgent.UploadData(APIURL, "action=GetData&Time=1");
            try
            {
                return JObject.Parse(result).Get<long>("msg");
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取远程接口的数据（本地缓存)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<long, Dictionary<Language, string>> GetData()
        {
            if (string.IsNullOrEmpty(APIURL)) return null;
            return MemoryUtils.Get(KEY, TimeSpan.FromDays(7), () =>
               {
                   return GetAPIData();
               });
        }

        /// <summary>
        /// 返回翻译之后的语言包
        /// </summary>
        /// <param name="input">简体中文内容</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Get(this string input, Language language)
        {
            Dictionary<long, Dictionary<Language, string>> data = GetData();
            if (data == null || string.IsNullOrEmpty(input)) return input;
            string keyword = $"~{input}~";
            long hashCode = keyword.GetLongHashCode();
            if (!data.ContainsKey(hashCode))
            {
                PostWordAsync(keyword).ConfigureAwait(false);
                return input;
            }
            if (!data[hashCode].ContainsKey(language)) return input;
            return string.IsNullOrEmpty(data[hashCode][language]) ? input : data[hashCode][language];
        }

        public static Dictionary<string, Dictionary<Language, string>> Get(string[] v, string translateUrl, Language[] languages)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
