using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.API.Action;

namespace Web.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Contains("cmd"))
            {
                ActionFactory.Invote(args);
                return;
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
           .UseUrls("http://*:5000", "http://*:80")
           .UseKestrel()
           .UseStartup<Startup>();
    }
}
