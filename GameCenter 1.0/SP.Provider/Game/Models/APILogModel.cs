using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// API的交互日志
    /// </summary>
    public struct APILogModel
    {

        /// <summary>
        /// 游戏类型
        /// </summary>
        public string Game;

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url;

        /// <summary>
        /// 发送数据（包括Header头和Post数据内容）
        /// </summary>
        public string PostData;

        /// <summary>
        /// 返回数据
        /// </summary>
        public string ResultData;

        /// <summary>
        /// 返回的统一状态判断
        /// </summary>
        public ResultStatus Status;

        /// <summary>
        /// 耗时
        /// </summary>
        public int Time;

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime CreateAt;
    }
}
