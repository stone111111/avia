using SP.Provider.Game.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game.Receives
{
    /// <summary>
    /// 接受到信息的基类
    /// </summary>
    public abstract class ReceiveBase
    {
        /// <summary>
        /// 游戏厂商类型
        /// </summary>
        public string Provider { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ResultStatus Status { get; set; }

        public ReceiveBase(string provider)
        {
            this.Provider = provider;
        }
    }
}
