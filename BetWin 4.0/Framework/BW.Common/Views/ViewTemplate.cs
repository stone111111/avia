using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;
using BW.Views;

namespace BW.Common.Views
{
    /// <summary>
    /// 系统配置的模板，开站的时候选择
    /// </summary>
    [Table("view_Template")]
    public partial class ViewTemplate
    {

        #region  ========  構造函數  ========
        public ViewTemplate() { }

        public ViewTemplate(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "TemplateID":
                        this.ID = (int)reader[i];
                        break;
                    case "Platform":
                        this.Platform = (PlatformSource)reader[i];
                        break;
                    case "TemplateName":
                        this.Name = (string)reader[i];
                        break;
                    case "Preview":
                        this.Preview = (string)reader[i];
                        break;
                    case "Sort":
                        this.Sort = (short)reader[i];
                        break;
                }
            }
        }


        public ViewTemplate(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "TemplateID":
                        this.ID = (int)dr[i];
                        break;
                    case "Platform":
                        this.Platform = (PlatformSource)dr[i];
                        break;
                    case "TemplateName":
                        this.Name = (string)dr[i];
                        break;
                    case "Preview":
                        this.Preview = (string)dr[i];
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
        /// 模板编号
        /// </summary>
        [Column("TemplateID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 适配的平台
        /// </summary>
        [Column("Platform")]
        public PlatformSource Platform { get; set; }


        /// <summary>
        /// 模板名称
        /// </summary>
        [Column("TemplateName")]
        public string Name { get; set; }


        /// <summary>
        /// 模板预览图
        /// </summary>
        [Column("Preview")]
        public string Preview { get; set; }


        /// <summary>
        /// 排序（从大到小）
        /// </summary>
        [Column("Sort")]
        public short Sort { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        /// <summary>
        /// 系统模板的配置信息
        /// </summary>
        [NotMapped]
        public List<ViewTemplateConfig> Configs { get; set; }

        #endregion

    }

}
