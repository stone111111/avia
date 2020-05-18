using System;
using System.Collections.Generic;
using System.Text;

namespace SP.StudioCore.Enums
{
    /// <summary>
    /// 结果编码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Successed = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Failed = 2,
        /// <summary>
        /// 密码不正确
        /// </summary>
        Incorrect = 3,
        /// <summary>
        /// 旧密码不正确
        /// </summary>
        OldIncorrect = 4,
        /// <summary>
        /// 用户存在
        /// </summary>
        Exist = 5,
        /// <summary>
        /// 用户不存在
        /// </summary>
        NotExist = 6,
        /// <summary>
        /// 验证码错误
        /// </summary>
        CaptchaFailed = 7,
        /// <summary>
        /// 没有权限
        /// </summary>
        NoAccess = 8,
        /// <summary>
        /// 验证token失效
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 赔率已发生变化
        /// </summary>
        OddsChanged = 9,
        /// <summary>
        /// 用户已冻结
        /// </summary>
        UserFreeze = 10,
        /// <summary>
        /// 用户余额不足
        /// </summary>
        UserBalance = 11,
        /// <summary>
        /// 超出投注最大或最小金额
        /// </summary>
        MinMaxBet = 12,
        /// <summary>
        /// 游戏已停售
        /// </summary>
        GameState = 13,
        /// <summary>
        /// 比赛已结束或取消
        /// </summary>
        MatchState = 14,
        /// <summary>
        /// 比赛已封盘
        /// </summary>
        BetState = 15,
        /// <summary>
        /// 赔率已暂停下注
        /// </summary>
        OddsBetState = 16,
        /// <summary>
        /// 比赛不支持串关投注
        /// </summary>
        CanCombine = 17,
        /// <summary>
        /// 串关投注最少投注两关
        /// </summary>
        BetTypeMin = 18,
        /// <summary>
        /// 串关投注最多投注八关
        /// </summary>
        BetTypeMax = 19,
        /// <summary>
        /// 投注记录不匹配
        /// </summary>
        BetTypeMatch = 20,
        /// <summary>
        /// 滚盘不支持串关投注
        /// </summary>
        IsLive = 21,
        /// <summary>
        /// 投注选项有无法投注比赛，请重新确定！
        /// </summary>
        BetCountMatch = 22,
        /// <summary>
        /// 玩法已停售
        /// </summary>
        PlayState = 23,
        /// <summary>
        /// 比赛已经开始
        /// </summary>
        MatchStarted = 24,
        /// <summary>
        /// 请求无效
        /// </summary>
        InvalidRequest = 25,
        /// <summary>
        /// Application请求参数不能为空
        /// </summary>
        RequiredApplication = 26,
        /// <summary>
        /// 用户已在其它地方登录
        /// </summary>
        LoginMutiple = 27,
        /// <summary>
        /// 请求站点不存在
        /// </summary>
        InvalidSite = 28,
        /// <summary>
        /// 手机号码无效
        /// </summary>
        InvalidMobile = 29,
        /// <summary>
        /// 手机号码未绑定
        /// </summary>
        NotExistMobile = 30,
        /// <summary>
        /// 手机号码已绑定
        /// </summary>
        ExistMobile = 31,
        /// <summary>
        /// 手机验证码不正确
        /// </summary>
        InvalidAuthCode = 32,
        /// <summary>
        /// 密钥错误
        /// </summary>
        ErrorSecretKey = 33
    }
}
