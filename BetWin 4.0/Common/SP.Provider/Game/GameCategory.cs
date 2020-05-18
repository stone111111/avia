using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SP.Provider.Game
{
    /// <summary>
    /// 游戏类型
    /// </summary>
    [Flags]
    public enum GameCategory : byte
    {
        [Description("彩票")]
        Lottery = 1,
        [Description("电竞")]
        ESport = 2,
        [Description("电子")]
        Slot = 4,
        [Description("真人")]
        Live = 8,
        [Description("棋牌")]
        Chess = 16,
        [Description("体育")]
        Sport = 32,
        [Description("捕鱼")]
        Fish = 64
    }
}
