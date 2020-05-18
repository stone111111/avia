using SP.Provider.Game.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game
{
    /// <summary>
    /// 与业务逻辑层交互的委托，应使用IOC容器注入到容器内
    /// </summary>
    public interface IGameDelegate
    {
        /// <summary>
        /// 保存API日志
        /// </summary>
        void SaveAPILog(APILogModel model);

        /// <summary>
        /// 获取标记
        /// </summary>
        /// <param name="game">游戏标记</param>
        /// <param name="mark">标记类型</param>
        /// <param name="type">采集类型</param>
        /// <returns>毫秒</returns>
        long GetMarkTime(OrderTaskModel task, byte mark = 0);

        /// <summary>
        /// 写入标记
        /// </summary>
        /// <param name="game">游戏标记</param>
        /// <param name="time">当前标记（毫秒)</param>
        /// <param name="mark">标记类型</param>
        /// <param name="type">采集类型</param>
        void SaveMarkTime(OrderTaskModel task, long time, byte mark = 0);

        /// <summary>
        /// 保存订单采集任务的状态
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool SaveOrderTaskStatus(OrderTaskModel task, OrderTaskStatus status);
        
        /// <summary>
        /// 获取余额
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        decimal? GetBalance(string provider, string userName);

    }
}
