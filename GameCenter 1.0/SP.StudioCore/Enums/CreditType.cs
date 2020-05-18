using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SP.StudioCore.Enums
{
    /// <summary>
    /// 商户状态
    /// </summary>
    public enum CreditType : byte
    {
        /// <summary>
        /// 总信用额度
        /// </summary>
        [Description("总信用额度")]
        Total = 1,
        /// <summary>
        /// 按游戏划分额度
        /// </summary>
        [Description("按游戏划分额度")]
        ByGame = 2
    }

}
