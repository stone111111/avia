using GM.Cache.Games;
using GM.Common.Games;
using System;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Data;
using SP.Provider.Game.Models;
using System.Data;

namespace GM.Agent.Games
{
    /// <summary>
    /// 游戏设置相关
    /// </summary>
    public sealed class GameMarkAgent : AgentBase<GameMarkAgent>
    {
        /// <summary>
        /// 获取游戏的时间节点
        /// </summary>
        public long GetMarkTime(string gameCode, MarkType markType, byte mark)
        {
            GameSetting game = GameAgent.Instance().GetGameSetting(gameCode);
            if (game == null)
            {
                return 0;
            }

            long Time = this.ReadDB.ReadInfo<GameMark, long>(t => t.Time,
                t => t.GameID == game.ID && t.Mark == mark && t.Type == markType);
            return Time;
        }

        /// <summary>
        /// 保存游戏的时间节点
        /// </summary>
        public void SaveMarkTime(string gameCode, long time, MarkType type, byte mark)
        {
            GameSetting game = GameAgent.Instance().GetGameSetting(gameCode);
            if (game == null)
            {
                return;
            }
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (db.Exists<GameMark>(t => t.GameID == game.ID && t.Type == type && t.Mark == mark))
                {
                    db.Update<GameMark, long>(t => t.Time, time,
                        t => t.GameID == game.ID && t.Type == type && t.Mark == mark);
                }
                else
                {
                    db.Insert(new GameMark()
                    {
                        GameID = game.ID,
                        Type = type,
                        Mark = mark,
                        Time = time
                    });
                }
                db.Commit();
            }
        }
    }
}
