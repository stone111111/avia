using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SP.StudioCore.Array;
using SP.StudioCore.Enums;
using SP.StudioCore.Model;
using SP.StudioCore.Net;
using SP.StudioCore.Security;
using SP.StudioCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Web;
using System.Xml.Linq;

namespace SP.StudioCore.API
{
    /// <summary>
    /// 语言包扩展
    /// </summary>
    public static class Translate
    {
        /// <summary>
        /// 使用本地缓存的语言包
        /// </summary>
        public readonly static Dictionary<string, Dictionary<Language, string>> _languageBag = new Dictionary<string, Dictionary<Language, string>>();

        /// <summary>
        /// 需要更新的词汇
        /// </summary>
        public static Stack<string> keywordQueue = new Stack<string>();

        /// <summary>
        /// 接口地址（如果为空表示不启用）
        /// </summary>
        public readonly static string APIURL;

        public static Exception exception;

        public static int TimerIndex = 0;

        /// <summary>
        /// 定时器
        /// </summary>
        private static readonly Timer timer = new Timer()
        {
            Enabled = true,
            Interval = 60 * 1000
        };

        /// <summary>
        /// 初始化赋值
        /// </summary>
        static Translate()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json")
                  .Build();
            APIURL = config["studio:language"];
            if (string.IsNullOrEmpty(APIURL)) return;

            LoadLanguageBag();

            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        /// <summary>
        /// 加载远程语言包
        /// </summary>
        private static void LoadLanguageBag()
        {
            string xml = NetAgent.UploadData(APIURL, "action=data", Encoding.UTF8, null, new Dictionary<string, string>()
            {
                { "type","xml"}
            });
            Dictionary<string, Language> enums = typeof(Language).ToEnumerable<Language>().ToDictionary(t => t.ToString(), t => t);
            try
            {

                XElement root = XElement.Parse(xml);
                foreach (XElement item in root.Element("Content").Elements())
                {
                    string key = item.Attribute("Key").Value;
                    if (!_languageBag.ContainsKey(key)) _languageBag.Add(key, new Dictionary<Language, string>());
                    foreach (XAttribute att in item.Attributes())
                    {
                        string name = att.Name.ToString();
                        if (enums.ContainsKey(name))
                        {
                            Language lang = enums[name];
                            if (_languageBag[key].ContainsKey(lang) && _languageBag[key][lang] != att.Value)
                            {
                                _languageBag[key][lang] = att.Value;
                            }
                            else if (!_languageBag[key].ContainsKey(lang))
                            {
                                _languageBag[key].Add(lang, att.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                TimerIndex++;
            }
        }

        /// <summary>
        /// 从消息队列中取出新词汇，发送到接口
        /// </summary>
        private static void SaveLanguageBag()
        {
            string result;
            try
            {
                if (keywordQueue.Count == 0) return;
                string json = JsonConvert.SerializeObject(GetQueue());
                string url = $"{APIURL}&action=addnew";
                result = NetAgent.UploadData(url, Encoding.UTF8.GetBytes(json), Encoding.UTF8, null, new Dictionary<string, string>()
                {
                    { "Content-Type","application/json"}
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("保存失败，{0}", ex.Message);
            }
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SaveLanguageBag();
            LoadLanguageBag();
        }

        /// <summary>
        /// 返回翻译之后的语言包
        /// </summary>
        /// <param name="input">简体中文内容</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Get(this string input, Language language)
        {
            if (string.IsNullOrEmpty(APIURL) || _languageBag.Count == 0) return input;
            string keyword = $"~{input}~";
            string md5 = Encryption.toMD5(keyword).Substring(0, 16);
            if (!_languageBag.ContainsKey(md5))
            {
                AddQueue(keyword);
                return input;
            }
            if (!_languageBag[md5].ContainsKey(language)) return input;
            return string.IsNullOrEmpty(_languageBag[md5][language]) ? input : _languageBag[md5][language];
        }

        /// <summary>
        /// 添加词汇进入消息队列
        /// </summary>
        /// <param name="keyword"></param>
        private static void AddQueue(string keyword)
        {
            if (keywordQueue.Contains(keyword)) return;
            lock (LockHelper.GetLoker(Const.LANGUAGE))
            {
                Console.WriteLine("加入词汇:{0}", keyword);
                keywordQueue.Push(keyword);
            }
        }

        private static IEnumerable<string> GetQueue()
        {
            while (keywordQueue.Count > 0)
            {
                yield return keywordQueue.Pop();
            }
        }

        /// <summary>
        /// 远程批量获取语言包
        /// </summary>
        /// <param name="input">需要翻译的词条（携带~符号）</param>
        /// <param name="apiurl">远程接口地址</param>
        /// <param name="languages">要翻译的语种</param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<Language, string>> Get(this string[] input, string apiurl, params Language[] languages)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "Content",string.Concat("[", string.Join(",",input.Select(t=>$"\"{ HttpUtility.JavaScriptStringEncode(t) }\"")),"]") },
                { "Language",string.Join(",",languages) },
                { "Action","TranslateData" }
            };
            string result = NetAgent.UploadData(apiurl, data.ToQueryString(), Encoding.UTF8);
            return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<Language, string>>>(result);
        }

        /// <summary>
        /// 显示当前内存中的语言包内容
        /// </summary>
        /// <returns></returns>
        public static XElement Get()
        {
            XElement root = new XElement("root");
            foreach (XElement item in _languageBag.Select(t =>
             {
                 XElement item = new XElement("Item");
                 item.SetAttributeValue("Key", t.Key);
                 foreach (var p in t.Value)
                 {
                     item.SetAttributeValue(p.Key.ToString(), p.Value);
                 }
                 return item;
             }))
            {
                root.Add(item);
            }
            return root;
        }
    }
}
