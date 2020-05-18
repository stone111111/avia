using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.StudioCore.Cache.Redis
{
    public class RedisManager
    {
        private static ConnectionMultiplexer instance;
        private static readonly object locker = new object();

        private static readonly Dictionary<int, IDatabase> dbCache = new Dictionary<int, IDatabase>();

        public RedisManager(string connection)
        {
            lock (locker)
            {
                if (instance == null)
                {
                    if (instance == null)
                        instance = ConnectionMultiplexer.Connect(connection);
                }
            }
        }

        public ConnectionMultiplexer Instance()
        {
            return instance;
        }

        public virtual IDatabase NewExecutor(int db = -1)
        {
            if (dbCache.ContainsKey(db))
            {
                return dbCache[db];
            }
            else
            {
                IDatabase database = this.Instance().GetDatabase(db);
                lock (locker)
                {
                    if (!dbCache.ContainsKey(db)) dbCache.Add(db, database);
                }
                return database;
            }
        }
    }
}
