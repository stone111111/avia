using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Common
{
    /// <summary>
    /// 全局的参数设定
    /// </summary>
    public static class Setting
    {
        static Setting()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DbConnection = config.GetConnectionString("DbConnection");
            ReadConnection = config.GetConnectionString("ReadConnection");
            LogDbConnection = config.GetConnectionString("LogConnection");
            RedisConnection = config.GetConnectionString("RedisConnection");
        }

        public readonly static string DbConnection;

        /// <summary>
        /// 只读数据库
        /// </summary>
        public readonly static string ReadConnection;

        public readonly static string RedisConnection;

        public readonly static string LogDbConnection;

        /// <summary>
        /// 当前的平台
        /// </summary>
        public const string PLATFORM = "x-platform";

        /// <summary>
        /// 获取请求路径的字段（Base64编码）
        /// </summary>
        public const string PATH = "x-path";

        /// <summary>
        /// 当前使用的模板
        /// </summary>
        public const string TEMPLATE = "TEMPLATE";

        /// <summary>
        /// 设备追踪
        /// </summary>
        public const string GHOST = "x-ghost";
    }
}
