using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Common.Models
{
    /// <summary>
    /// 放入消息队列对象
    /// </summary>
    [Serializable]
    public class GameCacheModel
    {
        /// <summary>
        /// 游戏代码
        /// </summary>
        public string GameCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartAt { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndAt { get; set; }

        /// <summary>
        /// 厂商数据延时时长，单位秒
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// 厂商最高采集频率，单位秒
        /// </summary>
        public int MaxRate { get; set; }

    }
}
