using GM.Common.Games;
using GM.Common.Models;
using SP.Provider.Game.Models;
using SP.StudioCore.Cache.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Cache.Games
{
    /// <summary>
    /// 游戏配置相关 #5
    /// </summary>
    public class GameOrderCaching : CacheBase<GameOrderCaching>
    {
        protected override int DB_INDEX => 5;

        #region ========  订单采集任务  ========

        /// <summary>
        /// 任务调度的消息队列
        /// </summary>
        private const string TASK = "TASK:ORDER";

        /// <summary>
        /// 任务编号
        /// </summary>
        private const string TASKID = "TASK:";

        /// <summary>
        /// 延迟任务标记
        /// </summary>
        private const string TASKDELAY = "TASK:DELAY:";

        /// <summary>
        /// 生产订单采集任务（右进）
        /// 同步生产延迟任务
        /// </summary>
        /// <param name="gameCode">游戏代码</param>
        /// <returns>写入队列是否成功</returns>
        public bool SaveTask(string gameCode)
        {
            string taskKey = $"{TASKID}{gameCode}";
            bool success = false;
            // 常规任务
            if (!this.NewExecutor().KeyExists(taskKey))
            {
                IBatch batch = this.NewExecutor().CreateBatch();
                batch.ListRightPushAsync(TASK, new OrderTaskModel(gameCode, MarkType.Normal));
                // 为了防止任务发生死锁，5分钟没有得到执行则抛弃
                batch.StringSetAsync(taskKey, OrderTaskStatus.Wait.GetRedisValue(), TimeSpan.FromMinutes(5));
                batch.Execute();
                success = true;
            }

            string delayKey = $"{TASKDELAY}{gameCode}";
            // 延迟任务
            if (!this.NewExecutor().KeyExists(delayKey))
            {
                IBatch batch = this.NewExecutor().CreateBatch();
                batch.ListRightPushAsync(TASK, new OrderTaskModel(gameCode, MarkType.Delay));
                // 延迟任务60分钟执行一次（只能等待过期删除）
                batch.StringSetAsync(delayKey, OrderTaskStatus.Wait.GetRedisValue(), TimeSpan.FromMinutes(60));
                batch.Execute();
                success = true;
            }

            return success;
        }

        /// <summary>
        /// 消费采集任务（左出）
        /// </summary>
        /// <returns>游戏代码</returns>
        public OrderTaskModel GetOrderTask()
        {
            return this.NewExecutor().ListLeftPop(TASK);
        }

        /// <summary>
        /// 设定任务采集状态（只可设置删除常规任务状态）
        /// </summary>
        /// <param name="task">采集任务</param>
        /// <param name="status">要设置的状态</param>
        /// <returns></returns>
        public void SetOrderTaskStatus(OrderTaskModel task, OrderTaskStatus status)
        {
            string taskKey = task.Type switch
            {
                MarkType.Normal => $"{TASKID}{task}",
                MarkType.Delay => $"{TASKDELAY}{task}",
                _ => $"{TASKID}{task}"
            };

            if (status == OrderTaskStatus.Ended && task.Type == MarkType.Normal)
            {
                this.NewExecutor().KeyDelete(taskKey);
            }
            else
            {
                this.NewExecutor().StringSet(taskKey, status.GetRedisValue());
            }
        }

        #endregion

        #region ========  订单数据  ========

        /// <summary>
        /// 订单的HashCode值
        /// </summary>
        private const string HASHCODE = "HASH:";

        /// <summary>
        /// 获取订单的Hash值
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public long GetHashCode(int gameId, string sourceId)
        {
            return this.NewExecutor().StringGet($"{HASHCODE}{gameId}:{sourceId}").GetRedisValue<long>();
        }

        /// <summary>
        /// 保存订单的HashCode值（有效期72小时）
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public void SaveHashCode(int gameId, string sourceId, long hashCode)
        {
            this.NewExecutor().StringSet($"{HASHCODE}{gameId}:{sourceId}", hashCode, TimeSpan.FromHours(72));
        }

        ///// <summary>
        ///// 保存游戏的时间节点
        ///// </summary>
        ///// <param name="setting"></param>
        //public void SaveMarkTime(string GameCode, long value)
        //{
        //    string key = $"MarkTime:{GameCode}";

        //    this.NewExecutor().StringSet(key, value.ToString());
        //}

        ///// <summary>
        ///// 获取游戏的时间节点
        ///// </summary>
        ///// <param name="setting"></param>
        //public long GetMarkTime(string GameCode)
        //{
        //    string key = $"MarkTime:{GameCode}";

        //    var aa = this.NewExecutor().StringGet(key);

        //    if (aa.IsNullOrEmpty) return 0;

        //    return long.Parse(this.NewExecutor().StringGet(key).ToString());
        //}

        #endregion
    }
}
