using SP.Provider.Game.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game.Receives
{
    /// <summary>
    /// 请求余额的公共结构
    /// </summary>
    public class BalanceReceive : ReceiveBase
    {
        /// <summary>
        /// 解析出用户名
        /// </summary>
        public string UserName { get; set; }

        public BalanceReceive(string provider) : base(provider)
        {
        }

        public static implicit operator bool(BalanceReceive receive)
        {
            return receive.Status == ResultStatus.Success;
        }

        public static implicit operator BalanceReceive(ResultStatus status)
        {
            return new BalanceReceive(null)
            {
                Status = status
            };
        }
    }
}
