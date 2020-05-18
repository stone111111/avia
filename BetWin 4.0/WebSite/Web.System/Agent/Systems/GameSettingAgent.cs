using BW.Agent.Systems;
using BW.Cache.Systems;
using BW.Common.Games;
using BW.Common.Systems;
using SP.StudioCore.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static BW.Common.Systems.SystemAdminLog;

namespace Web.System.Agent.Systems
{
    /// <summary>
    /// 系统配置
    /// </summary>
    internal sealed class GameSettingAgent : IGameSettingAgent<GameSettingAgent>
    {

        /// <summary>
        /// 获取所以游戏配置信息列表
        /// </summary>
        /// <returns></returns>
        public new List<GameSetting> GetGameSettingList()
        {
            return base.GetGameSettingList();
        }

        /// <summary>
        /// 根据游戏设置ID获取游戏设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new GameSetting GetGameSettingInfo(int id)
        {
            return base.GetGameSettingInfo(id);
        }

        /// <summary>
        /// 新增或修改游戏设置信息
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public bool SaveSetting(GameSetting setting)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (setting.Exists(db))
                {
                    setting.Update(db);
                }
                else
                {
                    setting.Add(db);
                }

                db.Commit();
            }
            return this.AccountInfo.Log(LogType.Setting, $"保存游戏设置:{setting.Name}");
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public bool UpdateIsOpen(GameSetting setting)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                setting.Update(db, t => t.IsOpen);
                db.Commit();
            }
            return this.AccountInfo.Log(LogType.Setting, $"修改开启状态:{setting.Name}");
        }

        /// <summary>
        /// 删除游戏设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteGameSettingInfo(int id)
        {
            GameSetting setting = this.GetGameSettingInfo(id);
            if (setting == null) return this.FaildMessage("编号错误");
            return this.WriteDB.Delete(setting) &&
                 AccountInfo.Log(LogType.Set, string.Format("删除游戏设置{0} 名称{1}", setting.ID, setting.Name));
        }
    }
}
