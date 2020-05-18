using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GM.Common.Games
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    public enum GameStatus : byte
    {
        /// <summary>
        /// 开放
        /// </summary>
        [Description("开放")]
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 0,
        /// <summary>
        /// 维护中
        /// </summary>
        [Description("维护中")]
        Maintain = 2
    }
}
