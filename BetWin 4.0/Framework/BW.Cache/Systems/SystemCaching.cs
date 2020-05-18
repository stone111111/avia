using BW.Common.Systems;
using SP.StudioCore.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Cache.Systems
{
    /// <summary>
    /// 系统设置缓存 #1
    /// </summary>
    public sealed class SystemCaching : CacheBase<SystemCaching>
    {
        protected override int DB_INDEX => 1;

        /// <summary>
        /// 系统配置
        /// </summary>
        private const string SETTING = "SETTING:";

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetSetting(SystemSetting.SettingType type)
        {
            string key = $"{SETTING}{type}";
            return this.NewExecutor().StringGet(key).GetRedisValue<string>();
        }

        /// <summary>
        /// 保存设置信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void SaveSetting(SystemSetting.SettingType type, string value)
        {
            string key = $"{SETTING}{type}";
            this.NewExecutor().StringSet(key, value.GetRedisValue());
        }
    }
}
