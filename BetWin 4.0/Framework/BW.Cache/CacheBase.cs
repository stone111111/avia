using System;
using System.Collections.Generic;
using System.Text;
using BW.Common;
using SP.StudioCore.Cache.Redis;

namespace BW.Cache
{
    public abstract class CacheBase<TCache> : RedisCacheBase where TCache : class, new()
    {
        protected CacheBase() : base(Setting.RedisConnection)
        {

        }

        private static TCache _instance;
        /// <summary>
        /// 返回单例对象
        /// </summary>
        /// <returns></returns>
        public static TCache Instance()
        {
            if (_instance == null)
            {
                _instance = new TCache();
            }
            return _instance;
        }
    }
}
