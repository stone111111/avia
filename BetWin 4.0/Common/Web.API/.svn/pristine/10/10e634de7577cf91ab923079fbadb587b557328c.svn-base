using Newtonsoft.Json;
using SP.StudioCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Web.API.Agent.Translates
{
    public struct TranslateModel
    {
        /// <summary>
        /// 设定当前要支持的语种
        /// </summary>
        /// <param name="language"></param>
        public TranslateModel(string language) : this()
        {
            this.Languages = new Dictionary<Language, string>();
            if (string.IsNullOrEmpty(language))
            {
                foreach (Language lang in Enum.GetValues(typeof(Language)))
                {
                    this.Languages.Add(lang, lang.GetDescription());
                }
            }
            else
            {
                foreach (string lang in language.Split(','))
                {
                    if (Enum.IsDefined(typeof(Language), lang))
                    {
                        Language value = lang.ToEnum<Language>();
                        this.Languages.Add(value, value.GetDescription());
                    }
                }
            }
        }

        private static readonly Regex regex = new Regex(@"^~(?<Value>.+)~$");

        /// <summary>
        /// 当前系统支持的语种
        /// </summary>
        public Dictionary<Language, string> Languages;

        public Dictionary<string, Dictionary<Language, string>> Content;

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="keyword"></param>
        public bool AddNew(string keyword)
        {
            if (!regex.IsMatch(keyword)) return false;
            Match match = regex.Match(keyword);
            if (this.Content == null) this.Content = new Dictionary<string, Dictionary<Language, string>>();
            string key = keyword.GetKey();
            if (!this.Content.ContainsKey(key))
            {
                this.Content.Add(key, new Dictionary<Language, string>()
                    {
                        { Language.CHN, match.Groups["Value"].Value },
                        { Language.THN, match.Groups["Value"].Value }
                    });
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static implicit operator TranslateModel(string content)
        {
            if (content == null) content = "{}";
            return JsonConvert.DeserializeObject<TranslateModel>(content);
        }

        /// <summary>
        /// 转化成为xml的字符串
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            XElement root = new XElement("root");
            XElement language = new XElement("Languages");
            XElement content = new XElement("Content");
            foreach (KeyValuePair<Language, string> item in Languages)
            {
                language.Add(new XElement(item.Key.ToString())
                {
                    Value = item.Value
                });
            }
            if (this.Content != null)
            {
                foreach (var item in this.Content)
                {
                    XElement entry = new XElement("Item");
                    entry.SetAttributeValue("Key", item.Key);
                    foreach (var t in item.Value)
                    {
                        entry.SetAttributeValue(t.Key.ToString(), t.Value);
                    }
                    content.Add(entry);
                }
            }
            root.Add(language);
            root.Add(content);
            return root.ToString();
        }
    }
}
