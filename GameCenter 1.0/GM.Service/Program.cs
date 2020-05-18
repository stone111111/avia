using GM.Agent.Games;
using GM.Agent.Logs;
using GM.Common;
using GM.Common.Database;
using Microsoft.Extensions.DependencyInjection;
using SP.Provider.Game;
using SP.StudioCore.Data.Repository;
using SP.StudioCore.Ioc;
using SP.StudioCore.Model;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GM.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            //#1 IOC容器初始化+注册服务
            IocCollection.AddService(new ServiceCollection()
                .AddDbContext<BizDataContext>(ServiceLifetime.Singleton)
                .AddTransient<IReadRepository, ReadDbExecutor>()//瞬时模式
                .AddTransient<IWriteRepository, WriteDbExecutor>()
                .AddSingleton<IGameDelegate, GameDelegate>());
            //.AddHostedService<OrderService>()); ;


            // 并行消费采集任务，线程数量应在配置文件中指定，默认为4线程
            Parallel.For(0, 4, taskIndex =>
            {
                Stopwatch sw = new Stopwatch();
                if (taskIndex == 0)
                {
                    while (true)
                    {
                        sw.Restart();
                        int taskCount = ServiceAgent.Instance().Build();
                        Console.WriteLine($"[生产]共生产{taskCount}笔任务，耗时{sw.ElapsedMilliseconds}ms");
                        Thread.Sleep(10 * 1000);
                    }
                }
                else
                {
                    while (true)
                    {
                        sw.Restart();
                        string code = string.Empty;
                        try
                        {
                            code = ServiceAgent.Instance().Execute(out string msg);
                            Console.WriteLine($"[{code}]{msg},耗时{sw.ElapsedMilliseconds}ms");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[{code}]{ex.Message},耗时{sw.ElapsedMilliseconds}ms");
                            LogAgent.Instance().SaveLog(new ErrorLogModel(ex));
                        }
                        Thread.Sleep(3000);
                    }
                }
            });
        }
    }
}
