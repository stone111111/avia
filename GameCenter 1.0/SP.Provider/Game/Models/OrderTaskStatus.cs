using Newtonsoft.Json;
using SP.StudioCore.Cache.Redis;
using SP.StudioCore.Net;
using SP.StudioCore.Types;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SP.Provider.Game.Models
{

    /// <summary>
    /// 采集任务（消息队列）
    /// </summary>
    public struct OrderTaskModel
    {
        public OrderTaskModel(string game, MarkType type)
        {
            this.Game = game;
            this.Type = type;
        }

        /// <summary>
        /// 游戏代码
        /// </summary>
        public string Game;

        /// <summary>
        /// 采集任务类型（正常/延时）
        /// </summary>
        public MarkType Type;


        public static implicit operator RedisValue(OrderTaskModel model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public static implicit operator OrderTaskModel(RedisValue value)
        {
            if (value.IsNull) return default;
            return JsonConvert.DeserializeObject<OrderTaskModel>(value.GetRedisValue<string>());
        }

        public static implicit operator bool(OrderTaskModel model)
        {
            return !string.IsNullOrEmpty(model.Game);
        }

        public static implicit operator string(OrderTaskModel model)
        {
            return model.Game;
        }

        public override string ToString()
        {
            return this.Game;
        }
    }

    /// <summary>
    /// 任务执行状态
    /// </summary>
    public enum OrderTaskStatus
    {
        /// <summary>
        /// 等待执行
        /// </summary>
        Wait,
        /// <summary>
        /// 采集中
        /// </summary>
        Collecting,
        /// <summary>
        /// 采集完毕（等待保存)
        /// </summary>
        Collectioned,
        /// <summary>
        /// 保存中（落入数据库）
        /// </summary>
        Saving,
        /// <summary>
        /// 任务完成（删除状态标记）
        /// </summary>
        Ended
    }
}
