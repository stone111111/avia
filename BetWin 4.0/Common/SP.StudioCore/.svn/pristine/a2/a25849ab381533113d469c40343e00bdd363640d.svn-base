using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SP.StudioCore.Model;
using SP.StudioCore.Services;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.StudioCore.Mvc.Startups
{
    /// <summary>
    /// Startup 的基类
    /// </summary>
    public abstract class MvcStartupBase
    {
        protected abstract void Configure(IServiceCollection services);

        /// <summary>
        /// 使用中间件
        /// </summary>
        /// <param name="app"></param>
        protected abstract void UseMiddleware(IApplicationBuilder app);

        public virtual void ConfigureServices(IServiceCollection services)
        {
            this.Configure(
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<MessageResult>()
                );
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpContext();
            this.UseMiddleware(app);
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
