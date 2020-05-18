using BW.Cache.Systems;
using BW.Common.Systems;
using SP.StudioCore.Data.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BW.Agent.Systems
{
    /// <summary>
    /// 全局参数配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ISettingAgent<T> : AgentBase<T> where T : class, new()
    {
        /// <summary>
        /// 获取系统的参数配置（Redis中读取，不存在则从数据库读取）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetSetting(SystemSetting.SettingType type)
        {
            string value = SystemCaching.Instance().GetSetting(type);
            if (string.IsNullOrEmpty(value))
            {
                value = this.ReadDB.ReadInfo<SystemSetting, string>(t => t.Value, t => t.Type == type);
                if (!string.IsNullOrEmpty(value)) SystemCaching.Instance().SaveSetting(type, value);
            }
            return value;
        }
    }
}
