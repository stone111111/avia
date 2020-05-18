using GM.Agent.Games;
using GM.Common;
using GM.Common.Database;
using GM.Service;
using Microsoft.Extensions.DependencyInjection;
using SP.Provider.Game;
using SP.StudioCore.Ioc;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IocCollection.AddService(new ServiceCollection()
                .AddDbContext<BizDataContext>(ServiceLifetime.Scoped)
                .AddScoped<ReadDbExecutor>()
                .AddScoped<WriteDbExecutor>()
                .AddSingleton<IGameDelegate, GameDelegate>());
            //
            ServiceAgent.Instance().Build();

            ServiceAgent.Instance().Execute(out string msg);
        }
    }
}
