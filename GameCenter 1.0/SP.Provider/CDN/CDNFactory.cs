using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.CDN
{
    /// <summary>
    /// CDN供应商的工厂模式
    /// </summary>
    public static class CDNFactory
    {
        /// <summary>
        /// 创建DNS供应商工厂
        /// </summary>
        /// <param name="type"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static ICDNProvider GetFactory(CDNProviderType type, string setting)
        {
            /// 手动配置
            if (type == CDNProviderType.Manual) return null;
            string key = $"{type}{setting.GetHash(8)}";
            return MemoryUtils.Get(key, TimeSpan.FromHours(1), () =>
            {
                Type assembly = typeof(CDNFactory).Assembly.GetType($"{typeof(CDNFactory).Namespace}.{type}");
                if (assembly == null) return null;
                return (ICDNProvider)Activator.CreateInstance(assembly, setting);
            });
        }
    }
}
