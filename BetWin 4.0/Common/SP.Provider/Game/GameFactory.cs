using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Security;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SP.Provider.Game
{
    /// <summary>
    /// 游戏工厂
    /// </summary>
    public static class GameFactory
    {
        /// <summary>
        /// 获取当前系统支持的所有游戏供应商
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetGameProvider()
        {
            return MemoryUtils.Get("GAMEPROVIDER", TimeSpan.FromDays(1), () =>
            {
                return typeof(GameFactory).Assembly.GetTypes().Where(t => t.IsPublic && !t.IsAbstract && t.BaseType == typeof(IGameProvider)).ToDictionary(
                     t => t.Name,
                     t => t.GetDescription());
            });
        }

        /// <summary>
        /// 创建游戏工厂实现
        /// </summary>
        /// <param name="type"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static IGameProvider GetFactory(GameProviderType type, string setting)
        {
            string key = $"{type}{setting.GetHash(8)}";
            return MemoryUtils.Get(key, TimeSpan.FromHours(1), () =>
            {
                Type assembly = typeof(GameFactory).Assembly.GetType($"{typeof(GameFactory).Namespace}.Clients.{type}");
                if (assembly == null) return null;
                return (IGameProvider)Activator.CreateInstance(assembly, setting);
            });
        }
    }
}
