using BW.Common;
using SP.StudioCore.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using BW.Common.Database;
using BW.Common.Base;
using SP.StudioCore.Http;

namespace BW.Agent
{
    public abstract class AgentBase<T> : DbAgent<T> where T : class, new()
    {
        protected AgentBase() : base(Setting.DbConnection)
        {
        }

        /// <summary>
        /// 获取EF数据库对象（优先从IOC容器中读取，如果不存在则读取静态变量）
        /// 注：应尽量减少EF的使用
        /// </summary>
        protected virtual BizDataContext BDC
        {
            get
            {
                return this.GetService<BizDataContext>() ?? this.DbContext<BizDataContext>();
            }
        }

        /// <summary>
        /// 可写数据库操作对象（无事务）
        /// </summary>
        protected virtual DbExecutor WriteDB
        {
            get
            {
                return this.GetService<WriteDbExecutor>();
            }
        }

        /// <summary>
        /// 只读数据库操作对象
        /// </summary>
        protected virtual DbExecutor ReadDB
        {
            get
            {
                return this.GetService<ReadDbExecutor>();
            }
        }

        /// <summary>
        /// 当前登录的账号
        /// </summary>
        protected virtual IAccount AccountInfo
        {
            get
            {
                return this.context.GetItem<IAccount>();
            }
        }
    }
}
