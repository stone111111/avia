using Dapper;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SP.StudioCore.Data
{
    /// <summary>
    /// 数据扩展
    /// </summary>
    public static class DataExtension
    {
        private static Dictionary<Type, List<ColumnProperty>> cache = new Dictionary<Type, List<ColumnProperty>>();

        private static object lockObj = new object();

        /// <summary>
        /// 把表类型存入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static bool SaveColumnProperty<T>() where T : class, new()
        {
            return SaveColumnProperty(typeof(T));
        }

        private static bool SaveColumnProperty(Type type)
        {
            if (cache.ContainsKey(type)) return true;
            if (!type.HasAttribute<TableAttribute>()) return false;
            lock (lockObj)
            {
                if (cache.ContainsKey(type)) return true;
                List<ColumnProperty> list = new List<ColumnProperty>();
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(t => t.HasAttribute<ColumnAttribute>()))
                {
                    list.Add(new ColumnProperty(property));
                }
                cache.Add(type, list);
            }
            return true;
        }

        /// <summary>
        /// 获取符合条件的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IEnumerable<ColumnProperty> GetProperty<T>(Func<ColumnProperty, bool> condition = null) where T : class, new()
        {
            if (!SaveColumnProperty(typeof(T))) return null;
            if (condition == null) condition = t => true;
            return cache[typeof(T)].Where(condition);
        }

        private static IEnumerable<ColumnProperty> GetProperty(Type type, Func<ColumnProperty, bool> condition = null)
        {
            if (!SaveColumnProperty(type)) return null;
            if (condition == null) condition = t => true;
            return cache[type].Where(condition);
        }

        /// <summary>
        /// 获取字段的数据库信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public static ColumnProperty GetColumn<T, TValue>(this Expression<Func<T, TValue>> field) where T : class, new()
        {
            Type type = typeof(T);
            if (!SaveColumnProperty(type)) return default;
            return cache[type].FirstOrDefault(t => t.Property.Name == field.ToPropertyInfo().Name);
        }

        /// <summary>
        /// 获取主键（实体类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ColumnProperty[] GetKey<T>(this T t) where T : class, new()
        {
            return GetKey(typeof(T));
        }

        /// <summary>
        /// 获取主键（类型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ColumnProperty[] GetKey(this Type type)
        {
            IEnumerable<ColumnProperty> columns = GetProperty(type, t => t.IsKey);
            return columns.ToArray();
        }

        /// <summary>
        /// 获取查询条件（实体类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="fields">不指定则使用主键</param>
        /// <returns></returns>
        public static Dictionary<ColumnProperty, object> GetCondition<T>(this T obj, params Expression<Func<T, object>>[] fields) where T : class, new()
        {
            IEnumerable<ColumnProperty> columns = fields.Length == 0 ?
                GetProperty<T>(t => t.IsKey) :
                GetProperty<T>(t => fields.Select(p => p.ToPropertyInfo().Name).Contains(t.Property.Name));

            return columns.ToDictionary(t => t, t => t.Property.GetValue(obj));
        }

        /// <summary>
        /// 获取数据库表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>(this T obj) where T : class, new()
        {
            return typeof(T).GetTableName();
        }

        public static string GetTableName(this Type type)
        {
            return type.GetAttribute<TableAttribute>().Name;
        }

        private static DynamicParameters GetParameters<T>(this T obj, IEnumerable<ColumnProperty> fields) where T : class, new()
        {
            DynamicParameters param = new DynamicParameters();
            foreach (var item in fields)
            {
                object value = item.Property.GetValue(obj);
                param.Add(item.Property.Name, value.GetValue(item.Property.PropertyType));
            }
            return param;
        }

        #region ========  数据库操作（增删查改、存在判断） 均为针对单条记录  ========

        /// <summary>
        /// 从数据库内获取对象（会赋值到自身）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="precate"></param>
        /// <returns></returns>
        public static T Info<T>(this T obj, DbExecutor db, params Expression<Func<T, object>>[] precate) where T : class, new()
        {
            Dictionary<ColumnProperty, object> condition = obj.GetCondition(precate);
            string tableName = obj.GetTableName();
            IEnumerable<ColumnProperty> fields = GetProperty<T>();
            string sql = $"SELECT { string.Join(",", fields.Select(t => string.Format("[{0}]", t.Name))) } FROM [{tableName}] WHERE { string.Join(" AND ", condition.Select(t => $"[{t.Key.Name}] = @{t.Key.Property.Name}")) }";
            DataSet ds = db.GetDataSet(CommandType.Text, sql, condition.Select(t => db.NewParam($"@{t.Key.Property.Name}", t.Value)).ToArray());
            if (ds.Tables[0].Rows.Count == 0) return null;
            DataRow dr = ds.Tables[0].Rows[0];
            foreach (ColumnProperty property in fields)
            {
                property.Property.SetValue(obj, dr[property.Name].GetValue(property.Property.PropertyType));
            }
            return obj;
        }

        /// <summary>
        /// 更新 条件（使用主键）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="fields">要更新的字段 如果为空则更新除主键之外的全部字段</param>
        /// <returns></returns>
        public static int Update<T>(this T obj, DbExecutor db, params Expression<Func<T, object>>[] fields) where T : class, new()
        {
            return obj.Update(db, null, fields);
        }

        /// <summary>
        /// 自定义条件更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="condition"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static int Update<T>(this T obj, DbExecutor db, Dictionary<ColumnProperty, object> condition, params Expression<Func<T, object>>[] fields) where T : class, new()
        {
            if (condition == null) condition = obj.GetCondition();
            string tableName = obj.GetTableName();
            string sql = $"UPDATE [{tableName}] SET ";

            // 要更新的字段
            IEnumerable<ColumnProperty> updateFields =
                (
                fields.Length == 0 ? GetProperty<T>(t => !t.IsKey) : GetProperty<T>(t => fields.Any(p => p.ToPropertyInfo().Name == t.Property.Name))
                ).Where(t => !condition.Select(p => p.Key).Any(p => p.Name == t.Name));

            sql += string.Join(",", updateFields.Select(t => $"[{t.Name}] = @{t.Property.Name}"));
            if (condition.Count() != 0)
            {
                sql += " WHERE " + string.Join(" AND ", condition.Select(t => $"[{t.Key.Name}] = @{t.Key.Property.Name}"));
            }
            foreach (ColumnProperty property in updateFields)
            {
                if (!condition.ContainsKey(property)) condition.Add(property, property.Property.GetValue(obj).GetValue(property.Property.PropertyType));
            }

            DynamicParameters param = new DynamicParameters();
            foreach (KeyValuePair<ColumnProperty, object> item in condition)
            {
                param.Add(item.Key.Property.Name, item.Value);
            }

            return db.ExecuteNonQuery(CommandType.Text, sql, param);
        }


        /// <summary>
        /// 添加进入数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static bool Add<T>(this T obj, DbExecutor db) where T : class, new()
        {
            // 获取所有要插入的字段
            var fields = GetProperty<T>(t => !t.Identity);
            string sql = $"INSERT INTO [{obj.GetTableName()}]({string.Join(",", fields.Select(t => "[" + t.Name + "]"))}) VALUES({string.Join(",", fields.Select(t => "@" + t.Property.Name))})";
            return db.ExecuteNonQuery(CommandType.Text, sql, obj.GetParameters(fields)) != 0;
        }

        /// <summary>
        /// 忽略自动编号的插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static bool AddNoIdentity<T>(this T obj, DbExecutor db) where T : class, new()
        {
            IEnumerable<ColumnProperty> fields = GetProperty<T>(t => true);
            string sql = $"INSERT INTO [{obj.GetTableName()}]({string.Join(",", fields.Select(t => "[" + t.Name + "]"))}) VALUES({string.Join(",", fields.Select(t => "@" + t.Property.Name))})";
            StringBuilder sb = new StringBuilder();
            if (fields.Any(t => t.Identity))
            {
                sb.Append($"SET IDENTITY_INSERT [{obj.GetTableName()}] ON;");
                sb.Append(sql);
                sb.Append($"SET IDENTITY_INSERT [{obj.GetTableName()}] OFF;");
            }
            else
            {
                sb.Append(sql);
            }
            return db.ExecuteNonQuery(CommandType.Text, sb.ToString(), obj.GetParameters(fields)) != 0;
        }

        /// <summary>
        /// 添加数据，并且获取自动编号字段的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        public static bool AddIdentity<T>(this T obj, DbExecutor db) where T : class, new()
        {
            var fields = GetProperty<T>(t => !t.Identity);
            var identity = GetProperty<T>(t => t.Identity);
            if (identity.Count() != 1) throw new Exception("当前表没有自增列");
            string sql = $"INSERT INTO [{obj.GetTableName()}]({string.Join(",", fields.Select(t => "[" + t.Name + "]"))})" +
                $" VALUES({string.Join(",", fields.Select(t => "@" + t.Property.Name))});SELECT @@IDENTITY;";
            object result = db.ExecuteScalar(CommandType.Text, sql, obj.GetParameters(fields));
            if (result == null || result == DBNull.Value) return false;
            identity.First().Property.SetValue(obj, Convert.ChangeType(result, identity.First().Property.PropertyType));
            return true;
        }

        /// <summary>
        /// 使用主键删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static bool Delete<T>(this T obj, DbExecutor db) where T : class, new()
        {
            var fields = GetProperty<T>(t => t.IsKey);
            string sql = $"DELETE FROM [{obj.GetTableName()}] WHERE { string.Join(" AND ", fields.Select(t => string.Format("[{0}] = @{1}", t.Name, t.Property.Name))) }";
            return db.ExecuteNonQuery(CommandType.Text, sql, obj.GetParameters(fields)) != 0;
        }

        /// <summary>
        /// 单主键删除（不需要新建实体）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pk"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Delete<T, TKey>(this DbExecutor db, TKey value) where T : class, new()
        {
            var fields = GetProperty<T>(t => t.IsKey);
            if (fields.Count() != 1)
            {
                throw new Exception("主键数量不为1");
            }
            ColumnProperty key = fields.First();
            if (key.Property.PropertyType != typeof(TKey))
            {
                throw new Exception("主键值类型不对应");
            }
            string sql = $"DELETE FROM [{typeof(T).GetTableName()}] WHERE [{key.Name}] = @{key.Property.Name}";
            DynamicParameters param = new DynamicParameters();
            param.Add(key.Property.Name, value);
            return db.ExecuteNonQuery(CommandType.Text, sql, param) != 0;
        }

        /// <summary>
        /// 使用自定义条件判断数据是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="fields">如果不存在则使用主键</param>
        /// <returns></returns>
        public static bool Exists<T>(this T obj, DbExecutor db, params Expression<Func<T, object>>[] fields) where T : class, new()
        {
            // 要更新的字段
            IEnumerable<ColumnProperty> condition =
                fields.Length == 0 ? GetProperty<T>(t => t.IsKey) : GetProperty<T>(t => fields.Any(p => p.ToPropertyInfo().Name == t.Property.Name));

            string sql = $"SELECT 0 FROM [{obj.GetTableName()}] WHERE { string.Join(" AND ", condition.Select(t => string.Format("[{0}] = @{1}", t.Name, t.Property.Name))) }";
            Object result = db.ExecuteScalar(CommandType.Text, sql, obj.GetParameters(condition));
            return result != null;
        }

        /// <summary>
        /// 获取某一个字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj"></param>
        /// <param name="db"></param>
        /// <param name="field"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static TValue Query<T, TValue>(this T obj, DbExecutor db, Expression<Func<T, TValue>> field, params Expression<Func<T, object>>[] where)
            where T : class, new()
        {
            // 条件字段
            IEnumerable<ColumnProperty> condition =
                where.Length == 0 ? GetProperty<T>(t => t.IsKey) : GetProperty<T>(t => where.Any(p => p.ToPropertyInfo().Name == t.Property.Name));

            string sql = $"SELECT [{field.GetColumn().Name}] FROM [{obj.GetTableName()}] WHERE { string.Join(" AND ", condition.Select(t => string.Format("[{0}] = @{1}", t.Name, t.Property.Name))) }";
            Object result = db.ExecuteScalar(CommandType.Text, sql, obj.GetParameters(condition));
            return (TValue)result;
        }

        #endregion

        /// <summary>
        /// 从数据库中自动填充内容到实体类
        /// </summary>
        public static T Fill<T>(this DataRow dr) where T : class, new()
        {
            Dictionary<string, PropertyInfo> properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(t => t.HasAttribute<ColumnAttribute>()).ToDictionary(t => t.GetAttribute<ColumnAttribute>().Name, t => t);
            DataColumnCollection columns = dr.Table.Columns;
            T t = new T();
            foreach (DataColumn column in columns)
            {
                string columnName = column.ColumnName;
                if (!properties.ContainsKey(columnName)) continue;
                PropertyInfo property = properties[columnName];
                property.SetValue(t, dr[column]);
            }
            return t;
        }

        public static T Fill<T>(this DataSet ds) where T : class, new()
        {
            if (ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return default;
            return ds.Tables[0].Rows[0].Fill<T>();
        }

        public static IEnumerable<T> ToList<T>(this IDataReader reader) where T : class, new()
        {
            while (reader.Read())
            {
                yield return reader.Fill<T>();
            }
        }

        public static T Fill<T>(this IDataReader reader) where T : class, new()
        {
            Dictionary<string, PropertyInfo> properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(t => t.HasAttribute<ColumnAttribute>()).ToDictionary(t => t.GetAttribute<ColumnAttribute>().Name, t => t);
            string[] columns = new string[reader.FieldCount];
            for (int index = 0; index < columns.Length; index++)
            {
                columns[index] = reader.GetName(index);
            }
            T t = new T();
            foreach (string columnName in columns)
            {
                if (!properties.ContainsKey(columnName)) continue;
                PropertyInfo property = properties[columnName];
                property.SetValue(t, reader[columnName]);
            }
            return t;
        }
    }

    /// <summary>
    /// 字段属性
    /// </summary>
    public struct ColumnProperty
    {
        public ColumnProperty(PropertyInfo property)
        {
            this.Property = property;
            ColumnAttribute column = property.GetAttribute<ColumnAttribute>();
            DatabaseGeneratedAttribute generate = property.GetAttribute<DatabaseGeneratedAttribute>();
            KeyAttribute key = property.GetAttribute<KeyAttribute>();

            this.Name = column.Name;
            this.Identity = generate == null ? false : generate.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity;
            this.IsKey = key != null;
        }

        /// <summary>
        /// 本身的字段属性
        /// </summary>
        public PropertyInfo Property;

        /// <summary>
        /// 数据库的字段名
        /// </summary>
        public string Name;

        /// <summary>
        /// 是否自动编号
        /// </summary>
        public bool Identity;

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsKey;
    }

}
