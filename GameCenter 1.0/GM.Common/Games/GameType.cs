using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GM.Common.Games
{
    /// <summary>
    /// 游戏类型
    /// </summary>
    public enum GameType : byte
    {
        /// <summary>
        /// PT
        /// </summary>
        [Description("PT"), Game(CategoryType.Slot)]
        PT = 1,
        /// <summary>
        /// AG
        /// </summary>
        [Description("AG"), Game(CategoryType.Live | CategoryType.Slot | CategoryType.Fish)]
        AG = 2,
        /// <summary>
        /// BBIN
        /// </summary>
        [Description("BBIN宝盈"), Game(CategoryType.Live | CategoryType.Slot)]
        BBIN = 3,
        /// <summary>
        /// 一本体育（沙巴）
        /// </summary>
        [Description("沙巴体育"), Game(CategoryType.Sport)]
        OneBook = 40,
        [Description("MG"), Game(CategoryType.Slot)]
        MG = 5,
        [Description("OG东方"), Game(CategoryType.Live)]
        OG = 6,
        [Description("申博"), Game(CategoryType.Live)]
        SunBet = 7,
        [Description("貝盈彩票"), Game(CategoryType.Lottery)]
        BetWin = 8,
        [Description("泛亚电竞"), Game(CategoryType.ESport)]
        AVIA = 9,
        [Description("Allbet欧博"), Game(CategoryType.Live)]
        AllBet = 10,
        [Description("BG大游"), Game(CategoryType.Lottery)]
        BG = 11,
        [Description("DG"), Game(CategoryType.Live)]
        DG = 12,
        [Description("GPI"), Game(CategoryType.Live)]
        GPI = 13,
        [Description("GD"), Game(CategoryType.Live)]
        GD = 14,
        [Description("eBET"), Game(CategoryType.Live)]
        eBET = 15,
        [Description("DT"), Game(CategoryType.Slot | CategoryType.Chess)]
        DT = 16,
        [Description("Red Tiger"), Game(CategoryType.Slot)]
        RT = 17,
        [Description("TTG"), Game(CategoryType.Slot)]
        TTG = 18,
        [Description("QTech"), Game(CategoryType.Slot)]
        QTech = 19,
        [Description("IM体育"), Game(CategoryType.Sport)]
        IM = 20,
        [Description("易胜博体育"), Game(CategoryType.Sport)]
        YSB = 21,
        [Description("VR彩票"), Game(CategoryType.Lottery)]
        VR = 22,
        [Description("IG彩票"), Game(CategoryType.Lottery)]
        IG = 23,
        [Description("开元棋牌"), Game(CategoryType.Chess)]
        KY = 24,
        [Description("CMD体育"), Game(CategoryType.Sport)]
        CMD = 25,
        [Description("BTI体育"), Game(CategoryType.Sport)]
        SBTech = 26,
        /// <summary>
        /// 平博体育
        /// </summary>
        [Description("平博"), Game(CategoryType.Sport)]
        PINNACLE = 27,
        /// <summary>
        /// 电竞牛
        /// </summary>
        [Description("电竞牛"), Game(CategoryType.ESport)]
        IMOne = 28,
        /// <summary>
        /// 爱棋牌
        /// </summary>
        [Description("761棋牌"), Game(CategoryType.Chess)]
        Q761 = 29,
        /// <summary>
        /// GM棋牌
        /// </summary>
        [Description("GM棋牌"), Game(CategoryType.Chess)]
        GM = 30,
        /// <summary>
        /// JDB电子+棋牌
        /// </summary>
        [Description("JDB"), Game(CategoryType.Slot | CategoryType.Chess)]
        JDB = 31,
        /// <summary>
        /// 皇冠体育（假皇冠）
        /// </summary>
        [Description("UG体育"), Game(CategoryType.Sport)]
        UG = 32,
        /// <summary>
        /// 天成彩票
        /// </summary>
        [Description("天成彩票"), Game(CategoryType.Lottery)]
        TCG = 33,
        /// <summary>
        /// 188体育
        /// </summary>
        [Description("188体育"), Game(CategoryType.Sport)]
        Bet188 = 34,
        /// <summary>
        /// 永城
        /// </summary>
        [Description("永城"), Game(CategoryType.Live)]
        YC = 35,
        /// <summary>
        /// 沙龙
        /// </summary>
        [Description("沙龙"), Game(CategoryType.Live)]
        SA = 36,
        /// <summary>
        /// 黑桃棋牌
        /// </summary>
        [Description("黑桃棋牌"), Game(CategoryType.Chess)]
        CS = 37,
        /// <summary>
        /// 小艾电竞
        /// </summary>
        [Description("小艾电竞"), Game(CategoryType.ESport)]
        IA = 38,
        /// <summary>
        /// 天美棋牌
        /// </summary>
        [Description("天美棋牌"), Game(CategoryType.Chess)]
        TM = 39,
        /// <summary>
        /// 小金188体育
        /// </summary>
        [Description("小金体育"), Game(CategoryType.Sport)]
        Xj188 = 41,
        /// <summary>
        /// 雷竞技
        /// </summary>
        [Description("RG电竞"), Game(CategoryType.ESport)]
        RayGaming = 42,
        /// <summary>
        /// 东森电竞
        /// </summary>
        [Description("东森电竞"), Game(CategoryType.ESport)]
        Dongsen = 43,
        /// <summary>
        /// 
        /// </summary>
        [Description("CQ9"), Game(CategoryType.Slot)]
        CQ9 = 44
    }
}
