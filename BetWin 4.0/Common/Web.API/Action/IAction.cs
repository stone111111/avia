using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SP.StudioCore.API;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.API.Action
{
    /// <summary>
    /// Action 基类
    /// </summary>
    public abstract class IAction
    {
        /// <summary>
        /// OSS的参数配置
        /// </summary>
        protected readonly static OSSSetting _ossSetting;

        /// <summary>
        /// 读取配置文件
        /// </summary>
        static IAction()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .Build();

            _ossSetting = new OSSSetting(config["upload:oss"]);
        }

        protected HttpContext context;

        public IAction(HttpContext context)
        {
            this.context = context;
        }

        protected string[] args;

        public IAction(string[] args)
        {
            this.args = args;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public abstract Result Invote();

        public virtual void Execute()
        {

        }
    }
}
