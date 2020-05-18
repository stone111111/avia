using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;
using BW.Views;

namespace BW.Common.Views
{
    /// <summary>
    /// 商户的模板配置
    /// </summary>
    [Table("view_SiteTemplate")]
    public partial class ViewSiteTemplate
    {

        #region  ========  構造函數  ========
        public ViewSiteTemplate() { }

        public ViewSiteTemplate(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "TemplateID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "Platform":
                        this.Platform = (PlatformSource)reader[i];
                        break;
                    case "TemplateName":
                        this.Name = (string)reader[i];
                        break;
                    case "Domain":
                        this.Domain = (string)reader[i];
                        break;
                }
            }
        }


        public ViewSiteTemplate(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "TemplateID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "Platform":
                        this.Platform = (PlatformSource)dr[i];
                        break;
                    case "TemplateName":
                        this.Name = (string)dr[i];
                        break;
                    case "Domain":
                        this.Domain = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 配置编号
        /// </summary>
        [Column("TemplateID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 模板所属商户
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 模板所属平台
        /// </summary>
        [Column("Platform")]
        public PlatformSource Platform { get; set; }


        /// <summary>
        /// 模板名称
        /// </summary>
        [Column("TemplateName")]
        public string Name { get; set; }


        /// <summary>
        /// 所要绑定到的域名（支持通配符），多个域名用逗号隔开。
        /// </summary>
        [Column("Domain")]
        public string Domain { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public static implicit operator HashEntry[](ViewSiteTemplate template)
        {
            return new[]
            {
                 new HashEntry("ID",template.ID),
                 new HashEntry("SiteID",template.SiteID),
                 new HashEntry("Platform",template.Platform.GetRedisValue()),
                 new HashEntry("Name",template.Name),
                 new HashEntry("Domain",template.Domain)
            };
        }

        public static implicit operator ViewSiteTemplate(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            ViewSiteTemplate template = new ViewSiteTemplate();
            foreach (HashEntry item in hashes)
            {
                switch (item.Name)
                {
                    case "ID":
                        template.ID = item.Value.GetRedisValue<int>();
                        break;
                    case "SiteID":
                        template.SiteID = item.Value.GetRedisValue<int>();
                        break;
                    case "Platform":
                        template.Platform = item.Value.GetRedisValue<PlatformSource>();
                        break;
                    case "Name":
                        template.Name = item.Value.GetRedisValue<string>();
                        break;
                    case "Domain":
                        template.Domain = item.Value.GetRedisValue<string>();
                        break;
                }
            }
            return template;
        }

        public static implicit operator int(ViewSiteTemplate template)
        {
            return template?.ID ?? 0;
        }

        /// <summary>
        /// 商户模板的配置信息
        /// </summary>
        [NotMapped]
        public List<ViewSiteConfig> Configs { get; set; }
        #endregion

    }

}
