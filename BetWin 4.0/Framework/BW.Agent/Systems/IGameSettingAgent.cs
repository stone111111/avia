using BW.Cache.Systems;
using BW.Common.Games;
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
    public abstract class IGameSettingAgent<T> : AgentBase<T> where T : class, new()
    {
        /// <summary>
        /// 获取所有游戏设置信息
        /// </summary>
        /// <returns></returns>
        protected List<GameSetting> GetGameSettingList()
        {
            // 此处表达式存在BUG，布尔型的判断一定要有等于号
            return this.ReadDB.ReadList<GameSetting>();
        }

        /// <summary>
        /// 根据游戏设置ID获取游戏设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected GameSetting GetGameSettingInfo(int id)
        {
            return this.ReadDB.ReadInfo<GameSetting>(t => t.ID == id);
        }

    }
}
