using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game.Receives
{
    /// <summary>
    /// 解析出资金操作
    /// </summary>
    public class MoneyReceive : ReceiveBase
    {
        public MoneyReceive(string provider) : base(provider)
        {
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作的资金
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 资金操作类型
        /// </summary>
        public MoneyReceiveType Type { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 资金操作来源编号
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Description { get; set; }

    }

    /// <summary>
    /// 资金操作类型
    /// </summary>
    public enum MoneyReceiveType
    {

    }
}
