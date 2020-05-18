using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// 采集到的订单信息
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// 来源ID
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 游戏类型
        /// </summary>
        public GameCategory Category { get; set; }

        /// <summary>
        /// 游戏厂商
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 玩家账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 游戏代码
        /// </summary>
        public string Code { get; set; }

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
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 投注时间（GTM+8），请注意采集过来的时间与北京时间的时区转换
        /// </summary>
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 结果产生时间，请注意采集过来的时间与北京时间的时区转换
        /// </summary>
        public DateTime ResultAt { get; set; }

        /// <summary>
        /// 订单内容（摘要信息）
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public string RawData { get; set; }

        /// <summary>
        /// 唯一值（用来判断订单是否重复)
        /// </summary>
        public long HashCode
        {
            get
            {
                return this.RawData.GetLongHashCode();
            }
        }
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus : byte
    {
        /// <summary>
        /// 等待开奖
        /// </summary>
        Wait,
        /// <summary>
        /// 赢
        /// </summary>
        Win,
        /// <summary>
        /// 输
        /// </summary>
        Lose,
        /// <summary>
        /// 退本金
        /// </summary>
        Return
    }
}
