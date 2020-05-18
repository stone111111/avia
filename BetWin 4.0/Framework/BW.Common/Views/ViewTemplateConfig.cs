using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Views
{
    /// <summary>
    /// 系统模板配置
    /// </summary>
    [Table("view_TemplateConfig")]
    public partial class ViewTemplateConfig
    {

        #region  ========  構造函數  ========
        public ViewTemplateConfig() { }

        public ViewTemplateConfig(IDataReader reader)
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


        public ViewTemplateConfig(DataRow dr)
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

        /// <summary>
        /// 配置编号
        /// </summary>
        [Column("ConfigID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        [Column("TemplateID")]
        public int TemplateID { get; set; }


        /// <summary>
        /// 所属的视图
        /// </summary>
        [Column("ViewID")]
        public int ViewID { get; set; }


        /// <summary>
        /// 所选择的模板
        /// </summary>
        [Column("ModelID")]
        public int ModelID { get; set; }


        /// <summary>
        /// 该模板对于此视图的参数配置
        /// </summary>
        [Column("Setting")]
        public string Setting { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        #endregion

    }

}
