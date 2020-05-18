using GM.Agent.Logs;
using GM.Cache.Games;
using GM.Common.Games;
using SP.Provider.Game;
using SP.Provider.Game.Models;
using SP.StudioCore.Data;
using SP.StudioCore.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SP.StudioCore.Data.Extension;

namespace GM.Agent.Games
{
    public sealed class GameDelegate : AgentBase<GameDelegate>, IGameDelegate
    {
        public long GetMarkTime(OrderTaskModel task, byte mark = 0)
        {
            int? gameId = GameAgent.Instance().GetGameSetting(task)?.ID;
            if (gameId == null) return 0;
            return this.ReadDB.ReadInfo<GameMark, long>(t => t.Time, t => t.GameID == gameId.Value && t.Mark == mark && t.Type == task.Type);
        }

        /// <summary>
        /// 存入游戏日志
        /// </summary>
        /// <param name="model"></param>
        public void SaveAPILog(APILogModel model)
        {
            APILogAgent.Instance().SaveLog(model);
        }

        public void SaveMarkTime(OrderTaskModel task, long time, byte mark = 0)
        {
            int? gameId = GameAgent.Instance().GetGameSetting(task)?.ID;
            if (gameId == null) return;
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (db.Exists<GameMark>(t => t.GameID == gameId.Value && t.Mark == mark && t.Type == task.Type))
                {
                    db.Update<GameMark, long>(t => t.Time, time, t => t.GameID == gameId.Value && t.Mark == mark && t.Type == task.Type);
                }
                else
                {
                    new GameMark()
                    {
                        GameID = gameId.Value,
                        Mark = mark,
                        Type = task.Type,
                        Time = time
                    }.Add(db);
                }
                db.Commit();
            }
        }

        public bool SaveOrderTaskStatus(OrderTaskModel task, OrderTaskStatus status)
        {
            GameOrderCaching.Instance().SetOrderTaskStatus(task, status);
            return true;
        }

        public decimal? GetBalance(string provider, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
