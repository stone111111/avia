using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SP.StudioCore.Model;
using System;

namespace SP.StudioCore.Services
{

    [Obsolete("已被Mvc.Startup.MvcStartupBase替代")]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 容器初始化
        /// 1、注册MessageResult，用于跨程序的消息传递
        /// 2、注册HttpContext对象，全局可以使用静态变量 SP.StudioCore.Web.Context.Current
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection Initialize(this IServiceCollection services)
        {
            //作用域 仅在当前请求上下文中复用
            services.AddScoped<MessageResult>();

            //单一  整个应用生命周期中复用
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
