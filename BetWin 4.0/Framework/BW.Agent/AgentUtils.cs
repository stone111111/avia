using BW.Cache.Systems;
using System;
using System.Collections.Generic;
using System.Text;
using static BW.Common.Systems.SystemSetting;

namespace BW.Agent
{
    /// <summary>
    /// 代理层的全局公共方法
    /// </summary>
    public static class AgentUtils
    {
        /// <summary>
        /// 获取系统配置值（缓存直接读取）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSetting(this SettingType type)
        {
            return SystemCaching.Instance().GetSetting(type);
        }
    }
}
