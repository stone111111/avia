using System;
using System.Collections.Generic;
using System.Text;
using GM.Common.Models;
using SP.StudioCore.Data.Extension;

namespace GM.Agent.Users
{
    /// <summary>
    /// 转账订单管理
    /// </summary>
    public sealed class TransferAgent : AgentBase<TransferAgent>
    {
        /// <summary>
        /// 获取转账订单
        /// </summary>
        /// <param name="sourceId">商户的订单号</param>
        /// <returns></returns>
        public UserTransfer GetUserTransfer(int siteId, int gameId, string sourceId)
        {
            return this.ReadDB.ReadInfo<UserTransfer>(t => t.SourceID == sourceId && t.SiteID == siteId && t.GameID == gameId);
        }
    }
}
