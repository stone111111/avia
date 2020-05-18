using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// 返回的统一状态
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>
        /// 未知原因的错误
        /// </summary>
        Faild = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 发生异常
        /// </summary>
        Exception = 199,
        /// <summary>
        /// 未指定游戏
        /// </summary>
        NoGame = 198,
        /// <summary>
        /// 密钥错误
        /// </summary>
        SecretKey = 197,

        #region =======  登录错误代码  ========

        /// <summary>
        /// 用户名不存在
        /// </summary>
        NoUser = 10,
        /// <summary>
        /// 会员被锁定
        /// </summary>
        UserLock = 11,

        #endregion

        #region ========  注册错误代码  ========

        /// <summary>
        /// 用户名已经存在
        /// </summary>
        ExistsUser = 20,
        /// <summary>
        /// 用户名规则错误
        /// </summary>
        BadUserName = 21,
        /// <summary>
        /// 密码规则错误
        /// </summary>
        BadPassword = 22,

        #endregion

        #region ========  转账错误代码  ========

        /// <summary>
        /// 订单号已经存在
        /// </summary>
        ExistsOrder = 30,
        /// <summary>
        /// 订单号格式错误
        /// </summary>
        OrderIDFormat = 31,
        /// <summary>
        /// 余额不足
        /// </summary>
        NoBalance = 32,
        /// <summary>
        /// 金额错误
        /// </summary>
        BadMoney = 33,

        #endregion

        #region ========  转账查询  ========

        /// <summary>
        /// 订单不存在
        /// </summary>
        NoOrder = 40,
        /// <summary>
        /// 转账失败
        /// </summary>
        OrderFaild = 41,
        /// <summary>
        /// 订单支付中
        /// </summary>
        OrderPaying = 42,

        #endregion

        #region ========  免转接口  ========

        /// <summary>
        /// 内容为空
        /// </summary>
        Empty = 50,
        /// <summary>
        /// 超时
        /// </summary>
        Timeout = 51

        #endregion
    }
}
