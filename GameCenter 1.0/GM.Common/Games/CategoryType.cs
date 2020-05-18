using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GM.Common.Games
{
    /// <summary>
    /// 游戏种类（位枚举）
    /// </summary>
    [Flags]
    public enum CategoryType : byte
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
