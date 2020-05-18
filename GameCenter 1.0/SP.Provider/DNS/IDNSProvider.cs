using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Model;
using SP.StudioCore.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SP.Provider.DNS
{
    /// <summary>
    /// DNS解析供应商
    /// </summary>
    public abstract class IDNSProvider : ISetting
    {
    }

    /// <summary>
    /// DNS供应商枚举
    /// </summary>
    public enum DNSProviderType : byte
    {
        /// <summary>
        /// 手动管理
        /// </summary>
        [Description("手动管理")]
        Manual,
        [Description("DNSPOD")]
        DnsPod,
        [Description("DNSDUN")]
        DnsDun
    }

    /// <summary>
    /// DNS供应商的工厂模式
    /// </summary>
    public static class DNSFactory
    {
        /// <summary>
        /// 创建DNS供应商工厂
        /// </summary>
        /// <param name="type"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static IDNSProvider GetFactory(DNSProviderType type, string setting)
        {
            /// 手动配置
            if (type == DNSProviderType.Manual) return null;
            string key = $"{type}{setting.GetHash(8)}";
            return MemoryUtils.Get(key, TimeSpan.FromHours(1), () =>
            {
                Type assembly = typeof(DNSFactory).Assembly.GetType($"{typeof(DNSFactory).Namespace}.{type}");
                if (assembly == null) return null;
                return (IDNSProvider)Activator.CreateInstance(assembly, setting);
            });
        }
    }
}
