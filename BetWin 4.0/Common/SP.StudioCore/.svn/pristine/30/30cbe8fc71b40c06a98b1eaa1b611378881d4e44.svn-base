using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.StudioCore.Services
{
    public static class ServiceProviderServiceExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }

        public static T GetRequiredService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetRequiredService(typeof(T));
        }
    }
}
