using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Common.Procedure
{
    /// <summary>
    /// 保存用户日报表
    /// </summary>
    public class rpt_SaveUserDate : IProcedureModel
    {
        public rpt_SaveUserDate(int userId, DateTime date, int gameId, int siteId, decimal betMoney, decimal betAmount, decimal money, int orderCount = 1)
        {
            this.UserID = userId;
            this.Date = date;
            this.GameID = gameId;
            this.SiteID = siteId;
            this.BetMoney = betMoney;
            this.BetAmount = betAmount;
            this.Money = money;
            this.OrderCount = orderCount;
        }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 报表日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 所属游戏
        /// </summary>
        public int GameID { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        public int SiteID { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public decimal BetMoney { get; set; }

        /// <summary>
        /// 有效投注
        /// </summary>
        public decimal BetAmount { get; set; }

        /// <summary>
        /// 盈亏
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; } = 1;
    }
}
