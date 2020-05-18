using GM.Cache.Games;
using GM.Common.Games;
using System;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Data.Extension;

namespace GM.Agent.Games
{
    /// <summary>
    /// 游戏设置相关
    /// </summary>
    public sealed class GameAgent : AgentBase<GameAgent>
    {
        /// <summary>
        /// 通过游戏厂商代码获取游戏配置信息
        /// </summary>
        /// <returns></returns>
        public GameSetting GetGameSetting(string gameCode)
        {
            int gameId = GameCaching.Instance().GetGameID(gameCode);
            if (gameId == 0)
            {
                gameId = this.ReadDB.ReadInfo<GameSetting, int>(t => t.ID, t => t.Code == gameCode);
                if (gameId != 0) GameCaching.Instance().SaveGameID(gameCode, gameId);
            }
            if (gameId == 0) return null;
            return this.GetGameSetting(gameId);
        }

        public GameSetting GetGameSetting(int gameId)
        {
            GameSetting setting = GameCaching.Instance().GetGameSetting(gameId);
            if (setting == null)
            {
                setting = this.ReadDB.ReadInfo<GameSetting>(t => t.ID == gameId);
                if (setting != null) GameCaching.Instance().SaveGameSetting(setting);
            }
            return setting;
        }


    }
}
