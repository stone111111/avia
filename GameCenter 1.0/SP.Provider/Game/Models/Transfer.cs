using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// 转账动作类型
    /// </summary>
    public enum TransferAction : byte
    {
        /// <summary>
        /// 转入
        /// </summary>
        [Description("转入")]
        IN,
        /// <summary>
        /// 转出
        /// </summary>
        [Description("转出")]
        OUT
    }

    /// <summary>
    /// 转账状态
    /// </summary>
    public enum TransferStatus : byte
    {
        /// <summary>
        /// 转账中
        /// </summary>
        [Description("转账中")]
        Paying = 0,
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,
        /// <summary>
        /// 异常失败（需等待查询转账接口进一步处理），
        /// 注意：查询转账订单应有一个时间间隔，不可返回异常之后马上调用查询接口
        /// </summary>
        [Description("异常失败")]
        Exception = 2,
        /// <summary>
        /// 订单失败
        /// </summary>
        [Description("订单失败")]
        Faild = 3
    }
}
