using BW.Agent.Systems;
using BW.Cache.Systems;
using BW.Common.Systems;
using SP.StudioCore.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Web.System.Agent.Systems
{
    /// <summary>
    /// 系统配置
    /// </summary>
    internal sealed class SettingAgent : ISettingAgent<SettingAgent>
    {
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public new string GetSetting(SystemSetting.SettingType type)
        {
            return base.GetSetting(type);
        }

        /// <summary>
        /// 保存系统参数配置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SaveSetting(SystemSetting.SettingType type, string value)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                SystemSetting setting = new SystemSetting()
                {
                    Type = type,
                    Value = value
                };
                if (setting.Exists(db))
                {
                    setting.Update(db);
                }
                else
                {
                    setting.Add(db);
                }

                db.AddCallback(() =>
                {
                    SystemCaching.Instance().SaveSetting(type, value);
                });

                db.Commit();
            }
            return true;
        }
    }
}
