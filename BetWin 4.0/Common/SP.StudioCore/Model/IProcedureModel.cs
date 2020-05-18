using Dapper;
using Microsoft.Data.SqlClient;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
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
            _parameters = new DynamicParameters();
            int site = 0;
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                string parameterName = $"@{property.Name}";
                object value = property.GetValue(this).GetValue(property.PropertyType);

                if (!property.HasAttribute<OutputAttribute>())
                {
                    site++;
                    _parameters.Add(parameterName, value);
                    continue;
                }
                DbType? dbType = null;
                string type = property.PropertyType.ToString();
                if (type.Contains("Decimal"))
                {
                    dbType = DbType.Decimal;
                }
                //DbType? dbType = property.PropertyType.Name switch
                //{
                //    "Decimal" => DbType.Decimal,
                //    _ => null
                //};
                _parameters.Add(parameterName, value, dbType, ParameterDirection.Output, site);
            }
            return _parameters;
        }

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
            foreach (PropertyInfo property in this.GetType().GetProperties().Where(t => t.HasAttribute<OutputAttribute>()))
            {
                object outputValue = this._parameters.Get<object>($"@{property.Name}");
                if (outputValue != null && outputValue != DBNull.Value)
                {
                    property.SetValue(this, outputValue);
                }
            }
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
