using GM.Agent.Systems;
using GM.Cache.Systems;
using GM.Common.Games;
using GM.Common.Models;
using GM.Common.Sites;
using GM.Common.Systems;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static GM.Common.Systems.SystemAdminLog;

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
        public new IEnumerable<GameSetting> GetGameSettingList()
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
        [Obsolete("添加新游戏的时候给商户添加游戏需再做业务流程优化")]
        public bool SaveSetting(GameSetting setting)
        {
            if (this.ReadDB.Exists<GameSetting>(t => t.Code == setting.Code && t.ID != setting.ID)) return this.FaildMessage("代码重复");
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (db.Exists<GameSetting>(t => t.ID == setting.ID))
                {
                    db.Update(setting);
                }
                else
                {
                    db.InsertIdentity(setting);

                    //保存游戏时默认开启所有商户该游戏
                    IEnumerable<Site> list = ReadDB.ReadList<Site>();
                    foreach (Site site in list)
                    {
                        SiteGameSetting siteGameSetting = new SiteGameSetting()
                        {
                            SiteID = site.ID,
                            GameID = setting.ID,
                            Status = SiteGameSetting.SiteGameStatus.Open
                        };

                        siteGameSetting.Add(db);
                    }
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
        public bool UpdateStatus(GameSetting setting)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                setting.Update(db, t => t.Status, t => t.MaintainTime);
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
