using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace SP.StudioCore.Model
{
    /// <summary>
    /// 存储过程的基类（标记这是一个存储过程生成类）
    /// </summary>
    public abstract class IProcedureModel
    {
        private DynamicParameters _parameters;
        /// <summary>
        /// 转化成为参数
        /// </summary>
        /// <returns></returns>
        public virtual DynamicParameters ToParameters()
        {
            _parameters = new DynamicParameters(this);
            return _parameters;
        }


        private DbParameter[] _dbParameters;
        public virtual DbParameter[] ToDbParameter()
        {
            Stack<DbParameter> list = new Stack<DbParameter>();
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                list.Push(new SqlParameter($"@{property.Name}", property.GetValue(this)));
            }
            return list.ToArray();
        }

        /// <summary>
        /// 填充Output的数据
        /// </summary>
        public virtual void Fill()
        {

        }
    }

    /// <summary>
    /// 标记参数是带输出值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputAttribute : Attribute
    {

    }
}
