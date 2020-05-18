using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace SP.StudioCore.Data
{
    internal static class DbFactory
    {
        internal static DbExecutor CreateExecutor(string connectionString, DatabaseType dbType = DatabaseType.SqlServer, DataConnectionMode connMode = DataConnectionMode.None, IsolationLevel tranLevel = IsolationLevel.Unspecified)
        {
            return new DbExecutor(connectionString, dbType, connMode, tranLevel);
        }

        /// <summary>
        /// 数据库实体类反射缓存类库
        /// </summary>
        internal static Dictionary<Type, Dictionary<string, PropertyInfo>> _dataPropertyModel = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
    }

   
}
