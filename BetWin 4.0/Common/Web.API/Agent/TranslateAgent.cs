using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SP.StudioCore.Array;
using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Data;
using SP.StudioCore.Enums;
using SP.StudioCore.Net;
using SP.StudioCore.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.API.Agent.Translates;

namespace Web.API.Agent
{
    /// <summary>
    /// 翻译的数据库操作类
    /// </summary>
    public sealed class TranslateAgent : AgentBase<TranslateAgent>
    {


        public TranslateAgent() : base(TranslateUtils._connectionString)
        {
        }

        /// <summary>
        /// 在线翻译（本地读取数据库，如果没有的话再读取远程接口）
        /// </summary>
        /// <returns></returns>
        public bool OnlineTranslate(Language source, Language target, string content, out string message)
        {
            if (source == target)
            {
                message = "源语言与目标语言相同";
                return false;
            }
            if (string.IsNullOrEmpty(content))
            {
                message = "需要翻译的文本为空";
                return false;
            }

            string result = NetAgent.UploadData(TranslateUtils._api, new Dictionary<string, string>()
                {
                    {"d",target.GetAttribute<ISO6391Attribute>() },
                    {"q",content },
                    {"s",source.GetAttribute<ISO6391Attribute>() }
                }.ToQueryString(), Encoding.UTF8, null, new Dictionary<string, string>()
            {
                {"Authorization",TranslateUtils._auth }
            });
            try
            {
                JObject json = JObject.Parse(result);
                if (json["status"].Value<int>() != 200)
                {
                    message = json["msg"].Value<string>();
                    return false;
                }
                message = json["msg"].Value<string>();
                return true;
            }
            catch (Exception ex)
            {
                message = result + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 新建词条
        /// </summary>
        public bool AddKeyword(string keyword, string channelName)
        {
            if (string.IsNullOrEmpty(channelName)) return this.FaildMessage("频道为空");
            string keyId = keyword.GetKey();
            Regex regex = new Regex(@"^~(?<Content>.+)~$");
            if (regex.IsMatch(keyword)) keyword = regex.Match(keyword).Groups["Content"].Value;
            using (DbExecutor db = NewExecutor())
            {
                db.ExecuteNonQuery(CommandType.StoredProcedure, "tran_AddKeyword", new
                {
                    KeyID = keyId,
                    Content = keyword,
                    Channel = channelName
                });
                MemoryUtils.Remove(channelName);
            }
            return true;
        }

        /// <summary>
        /// 保存内容
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="language"></param>
        /// <param name="content"></param>
        public void SaveContent(string keyId, Language language, string content, string channelName)
        {
            using (DbExecutor db = NewExecutor())
            {
                db.ExecuteNonQuery(CommandType.StoredProcedure, "tran_SaveContent", new
                {
                    KeyID = keyId,
                    Language = (byte)language,
                    Content = content,
                    Token = channelName
                });
                MemoryUtils.Remove(channelName);
            }
        }

        /// <summary>
        /// 获取语言包（内存中缓存）
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<Language, string>> GetTranslate(string channelName, bool fromDatabase = false)
        {
            int channelId = this.GetChannelID(channelName);
            if (channelId == 0) return new Dictionary<string, Dictionary<Language, string>>();

            Func<Dictionary<string, Dictionary<Language, string>>> action = () =>
            {
                Dictionary<string, Dictionary<Language, string>> data = new Dictionary<string, Dictionary<Language, string>>();
                using (DbExecutor db = NewExecutor())
                {
                    IDataReader reader = db.ReadData(CommandType.Text, "SELECT * FROM tran_Content WHERE EXISTS(SELECT 0 FROM tran_Token WHERE ChannelID = @ChannelID AND tran_Token.KeyID = tran_Content.KeyID)", new
                    {
                        ChannelID = channelId
                    });
                    while (reader.Read())
                    {
                        string keyId = (string)reader["KeyID"];
                        Language language = (Language)reader["Language"];
                        string content = (string)reader["Content"];

                        if (!data.ContainsKey(keyId)) data.Add(keyId, new Dictionary<Language, string>());
                        if (!data[keyId].ContainsKey(language)) data[keyId].Add(language, content);
                    }
                }
                return data;
            };

            if (fromDatabase) return action.Invoke();

            return MemoryUtils.Get(channelName, TimeSpan.FromHours(1), action);
        }

        /// <summary>
        /// 获取频道ID
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public int GetChannelID(string channelName)
        {
            using (DbExecutor db = NewExecutor())
            {
                TranslateChannel channel = new TranslateChannel()
                {
                    Name = channelName
                }.Info(db, t => t.Name);
                return channel == null ? 0 : channel.ID;
            }
        }

        /// <summary>
        /// 获取单个词条的翻译
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="languages"></param>
        /// <returns></returns>
        public Dictionary<Language, string> GetTranslate(string keyword, params Language[] languages)
        {
            string keyId = keyword.GetKey();
            Dictionary<Language, string> dic = new Dictionary<Language, string>();
            using (DbExecutor db = NewExecutor())
            {
                DataSet ds = db.GetDataSet($"SELECT [Language],Content FROM tran_Content WHERE KeyID = '{keyId}' AND [Language] IN ({ string.Join(",", languages.Select(t => (byte)t)) })");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dic.Add((Language)dr["Language"], (string)dr["Content"]);
                }
            }
            return dic;
        }
    }
}
