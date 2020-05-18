using SP.StudioCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.StudioCore.API.Translates
{
    /// <summary>
    /// 从翻译接口过来的数据
    /// </summary>
    public struct TranslateModel
    {
        public int success;

        /// <summary>
        /// 频道的数据更新时间
        /// </summary>
        public long msg;

        public List<TranslateContent> info;

        public static implicit operator Dictionary<long, Dictionary<Language, string>>(TranslateModel model)
        {
            Dictionary<long, Dictionary<Language, string>> data = new Dictionary<long, Dictionary<Language, string>>();
            foreach (TranslateContent content in model.info)
            {
                if (!data.ContainsKey(content.KeyID)) data.Add(content.KeyID, new Dictionary<Language, string>());
                if (data[content.KeyID].ContainsKey(content.Language))
                {
                    data[content.KeyID][content.Language] = content.Content;
                }
                else
                {
                    data[content.KeyID].Add(content.Language, content.Content);
                }
            }
            return data;
        }
    }

    public struct TranslateContent
    {
        public long KeyID;

        public Language Language;

        public string Content;
    }
}
