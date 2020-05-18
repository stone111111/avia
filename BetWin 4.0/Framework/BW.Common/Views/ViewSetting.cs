using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;
using System.ComponentModel;
using BW.Views;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;

namespace BW.Common.Views
{
    /// <summary>
    /// 系统的视图配置，从类库中读取
    /// </summary>
    [Table("view_Setting")]
    public partial class ViewSetting
    {

        #region  ========  構造函數  ========
        public ViewSetting() { }

        public ViewSetting(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ViewID":
                        this.ID = (int)reader[i];
                        break;
                    case "Name":
                        this.Name = (string)reader[i];
                        break;
                    case "Platform":
                        this.Platform = (PlatformSource)reader[i];
                        break;
                    case "Code":
                        this.Code = (string)reader[i];
                        break;
                    case "Status":
                        this.Status = (ViewStatus)reader[i];
                        break;
                    case "Sort":
                        this.Sort = (short)reader[i];
                        break;
                }
            }
        }


        public ViewSetting(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ViewID":
                        this.ID = (int)dr[i];
                        break;
                    case "Name":
                        this.Name = (string)dr[i];
                        break;
                    case "Platform":
                        this.Platform = (PlatformSource)dr[i];
                        break;
                    case "Code":
                        this.Code = (string)dr[i];
                        break;
                    case "Status":
                        this.Status = (ViewStatus)dr[i];
                        break;
                    case "Sort":
                        this.Sort = (short)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 视图的编号
        /// </summary>
        [Column("ViewID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 视图名字
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }


        /// <summary>
        /// 所属平台（PC、H5、APP）
        /// </summary>
        [Column("Platform")]
        public PlatformSource Platform { get; set; }


        /// <summary>
        /// 视图所对应的类路径（完整）
        /// </summary>
        [Column("Code")]
        public string Code { get; set; }


        /// <summary>
        /// 视图的状态
        /// </summary>
        [Column("Status")]
        public ViewStatus Status { get; set; }


        /// <summary>
        /// 自定义的排序（从大到小）
        /// </summary>
        [Column("Sort")]
        public short Sort { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public enum ViewStatus : byte
        {
            /// <summary>
            /// 正常
            /// </summary>
            [Description("正常")]
            Normal,
            /// <summary>
            /// 已被删除
            /// </summary>
            [Description("已删除")]
            Delete
        }

        public static implicit operator ViewSetting(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            ViewSetting setting = new ViewSetting();
            foreach (HashEntry hash in hashes)
            {
                switch (hash.Name.GetRedisValue<string>())
                {
                    case "ID":
                        setting.ID = hash.Value.GetRedisValue<int>();
                        break;
                    case "Name":
                        setting.Name = hash.Value.GetRedisValue<string>();
                        break;
                    case "Platform":
                        setting.Platform = hash.Value.GetRedisValue<PlatformSource>();
                        break;
                    case "Code":
                        setting.Code = hash.Value.GetRedisValue<string>();
                        break;
                    case "Status":
                        setting.Status = hash.Value.GetRedisValue<ViewStatus>();
                        break;
                    case "Sort":
                        setting.Sort = hash.Value.GetRedisValue<short>();
                        break;
                };
            }
            return setting;
        }

        public static implicit operator HashEntry[](ViewSetting setting)
        {
            if (setting == null) return null;
            return new[]
            {
                new HashEntry("ID",setting.ID.GetRedisValue()),
                new HashEntry("Name",setting.Name.GetRedisValue()),
                new HashEntry("Platform",setting.Platform.GetRedisValue()),
                new HashEntry("Code",setting.Code.GetRedisValue()),
                new HashEntry("Status",setting.Status.GetRedisValue()),
                new HashEntry("Sort",setting.Sort.GetRedisValue())
            };
        }
        #endregion

    }

}
