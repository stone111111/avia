using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace GM.Agent
{
    public static class ServiceCollection
    {
        public static ServiceProvider Provider;
        public static void AddService(this IServiceCollection services)
        {
            RegisterServices(services);
        }
        public static void RegisterServices(IServiceCollection services)
        {
            //基础服务注册
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            Provider = services.BuildServiceProvider();
        }
    }
}
