using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;

namespace BW.Common.Views
{
    /// <summary>
    /// 商户的视图配置
    /// </summary>
    [Table("view_SiteConfig")]
    public partial class ViewSiteConfig
    {

        #region  ========  構造函數  ========
        public ViewSiteConfig() { }

        public ViewSiteConfig(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ConfigID":
                        this.ID = (int)reader[i];
                        break;
                    case "TemplateID":
                        this.TemplateID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "ViewID":
                        this.ViewID = (int)reader[i];
                        break;
                    case "ModelID":
                        this.ModelID = (int)reader[i];
                        break;
                    case "Setting":
                        this.Setting = (string)reader[i];
                        break;
                }
            }
        }


        public ViewSiteConfig(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ConfigID":
                        this.ID = (int)dr[i];
                        break;
                    case "TemplateID":
                        this.TemplateID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "ViewID":
                        this.ViewID = (int)dr[i];
                        break;
                    case "ModelID":
                        this.ModelID = (int)dr[i];
                        break;
                    case "Setting":
                        this.Setting = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        [Column("ConfigID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 所属的商户模板
        /// </summary>
        [Column("TemplateID")]
        public int TemplateID { get; set; }


        /// <summary>
        /// 所属商户
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 所属的视图
        /// </summary>
        [Column("ViewID")]
        public int ViewID { get; set; }


        /// <summary>
        /// 所选择的视图模型
        /// </summary>
        [Column("ModelID")]
        public int ModelID { get; set; }


        /// <summary>
        /// 商户的配置内容
        /// </summary>
        [Column("Setting")]
        public string Setting { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public static implicit operator HashEntry[](ViewSiteConfig config)
        {
            return new[]
            {
                new HashEntry("ID",config.ID),
                new HashEntry("SiteID",config.SiteID),
                new HashEntry("TemplateID",config.TemplateID),
                new HashEntry("ViewID",config.ViewID),
                new HashEntry("ModelID",config.ModelID),
                new HashEntry("Setting",config.Setting)
            };
        }

        public static implicit operator ViewSiteConfig(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            ViewSiteConfig config = new ViewSiteConfig();
            foreach (HashEntry item in hashes)
            {
                switch (item.Name)
                {
                    case "ID":
                        config.ID = item.Value.GetRedisValue<int>();
                        break;
                    case "TemplateID":
                        config.TemplateID = item.Value.GetRedisValue<int>();
                        break;
                    case "SiteID":
                        config.SiteID = item.Value.GetRedisValue<int>();
                        break;
                    case "ViewID":
                        config.ViewID = item.Value.GetRedisValue<int>();
                        break;
                    case "ModelID":
                        config.ModelID = item.Value.GetRedisValue<int>();
                        break;
                    case "Setting":
                        config.Setting = item.Value.GetRedisValue<string>();
                        break;
                }
            }
            return config;
        }

        #endregion

    }

}
