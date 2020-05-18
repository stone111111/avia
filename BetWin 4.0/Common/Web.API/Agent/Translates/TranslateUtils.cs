using Microsoft.Extensions.Configuration;
using SP.StudioCore.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.API.Agent.Translates
{
    public static class TranslateUtils
    {
        /// <summary>
        /// 在线接口地址
        /// </summary>
        public static readonly string _api;

        /// <summary>
        /// 授权码
        /// </summary>
        public static readonly string _auth;

        /// <summary>
        /// 数据库存储
        /// </summary>
        public static readonly string _connectionString;

        static TranslateUtils()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("apptranslate.json")
                .Build();

            _connectionString = config["ConnectionStrings"];
            _api = config["online:api"];
            _auth = config["online:Authorization"];
            Console.WriteLine(_api);
        }

        /// <summary>
        /// 词条密钥
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetKey(this string content)
        {
            return Encryption.toMD5(content).Substring(0, 16);
        }
    }
}
