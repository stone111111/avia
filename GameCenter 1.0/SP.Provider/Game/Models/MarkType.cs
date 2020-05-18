using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game.Models
{
    public enum MarkType : byte
    {
        /// <summary>
        /// 正常采集
        /// </summary>
        Normal,
        /// <summary>
        /// 延迟采集
        /// </summary>
        Delay
    }
}
