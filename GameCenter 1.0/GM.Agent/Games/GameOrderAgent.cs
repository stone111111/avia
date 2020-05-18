using GM.Cache.Games;
using GM.Common.Games;
using System;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Data.Extension;
using SP.Provider.Game.Models;
using System.Threading;
using GM.Common.Models;
using System.Linq;

namespace GM.Agent.Games
{
    /// <summary>
    /// 游戏订单相关
    /// </summary>
    public sealed class GameOrderAgent : AgentBase<GameOrderAgent>
    {
        /// <summary>
        /// 获取当前订单的HashCode值，用于判断是否重复保存
        /// </summary>
        /// <param name="gameId">游戏代码</param>
        /// <param name="sourceId">来源于游戏厂商的订单ID</param>
        /// <returns></returns>
        public long GetOrderHashCode(int gameId, string sourceId)
        {
            long hashCode = GameOrderCaching.Instance().GetHashCode(gameId, sourceId);
            if (hashCode == 0)
            {
                hashCode = this.ReadDB.ReadInfo<GameOrder, long>(t => t.HashCode, t => t.GameID == gameId && t.SourceID == sourceId);
                if (hashCode != 0) GameOrderCaching.Instance().SaveHashCode(gameId, sourceId, hashCode);
            }
            return hashCode;
        }

        [Obsolete("存在问题：没有分页、没有条件搜索，大字段做列表内容返回")]
        public List<GameOrder> GetGameOrderList(int siteID, GameType gameType, int gameID,
            string playerName, DateTime startAt, DateTime endAt)
        {
            if (startAt == DateTime.MinValue)
            {
                startAt = DateTime.Now.AddDays(-1);
                endAt = DateTime.Now;
            }

            List<GameOrder> list = BDC.GameOrder.Where(t => t.SiteID == siteID && t.Type == gameType &&
                t.GameID == gameID && t.PlayerName == playerName && t.CreateAt >= startAt && t.CreateAt < endAt).ToList();

            return list;
        }
    }
}
