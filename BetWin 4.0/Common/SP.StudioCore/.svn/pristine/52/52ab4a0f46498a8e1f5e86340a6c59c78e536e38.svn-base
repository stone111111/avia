using Microsoft.EntityFrameworkCore;
using SP.StudioCore.Http;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.StudioCore.Data
{
    /// <summary>
    /// 数据连接池控制
    /// </summary>
    public class DataPooling
    {
        protected static Dictionary<string, DbContext> _dbContext;

        /// <summary>
        /// 获取EF的数据库操作实例
        /// </summary>
        /// <typeparam name="DB"></typeparam>
        /// <returns></returns>
        public static DB GetDataContext<DB>() where DB : DbContext, new()
        {
            var context = Context.Current;
            DB dbContext;
            if (context == null)
            {
                if (_dbContext == null) _dbContext = new Dictionary<string, DbContext>();
                if (_dbContext.ContainsKey(typeof(DB).FullName)) return (DB)_dbContext[typeof(DB).FullName];
                dbContext = new DB();
                _dbContext.Add(typeof(DB).FullName, dbContext);
                return dbContext;
            }
            else
            {
                dbContext = context.GetItem<DB>();
                if (dbContext == null)
                {
                    dbContext = new DB();
                    context.SetItem(dbContext);

                }
                return dbContext;
            }
        }
    }
}
