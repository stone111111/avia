﻿using SP.StudioCore.Array;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SP.StudioCore.Data
{
    /// <summary>
    /// 数据库实体类映射的基类
    /// </summary>
    public abstract class DataModelBase
    {
        public DataModelBase() { }

        public DataModelBase(DataRow dr)
        {
            Type type = this.GetType();
            Dictionary<string, PropertyInfo> data = DbFactory._dataPropertyModel.Get(type);
            if (data == null)
            {
                lock (type)
                {
                    data = type.GetProperties().Where(t => t.HasAttribute<ColumnAttribute>()).ToDictionary(t => t.GetAttribute<ColumnAttribute>().Name, t => t);
                    if (!DbFactory._dataPropertyModel.ContainsKey(type)) DbFactory._dataPropertyModel.Add(type, data);
                }
            }
            foreach (DataColumn column in dr.Table.Columns)
            {
                string colName = column.ColumnName;
                if (data.ContainsKey(colName))
                {
                    data[colName].SetValue(this, dr[colName]);
                }
            }
        }
    }
}
