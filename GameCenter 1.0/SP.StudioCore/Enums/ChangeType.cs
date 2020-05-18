using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SP.StudioCore.Enums
{
    /// <summary>
    /// 信用额度变化类型
    /// </summary>
    public enum ChangeType : byte
    {
        /// <summary>
        /// 买分
        /// </summary>
        [Description("买分")]
        Buy = 1,
        /// <summary>
        /// 投注
        /// </summary>
        [Description("投注")]
        Bet = 2
    }
}
