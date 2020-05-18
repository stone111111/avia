using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BW.Agent.Base;
using BW.Common;
using BW.Common.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SP.StudioCore.Data;
using SP.StudioCore.Mvc.Startups;
using SP.StudioCore.Web;
using Web.System.Utils;

namespace Web.System
{
    public sealed class Startup : MvcStartupBase
    {
        protected override void Configure(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                opt.Filters.Add<SysFilterAttribute>();
            });
            services.AddDbContext<BizDataContext>(ServiceLifetime.Scoped);
            services.AddScoped<ReadDbExecutor>();
            services.AddScoped<WriteDbExecutor>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        protected override void UseMiddleware(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddle>();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

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
