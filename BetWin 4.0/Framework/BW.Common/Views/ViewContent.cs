using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;

namespace BW.Common.Views
{
    /// <summary>
    /// 视图模型的内容
    /// </summary>
    [Table("view_Content")]
    public partial class ViewContent
    {

        #region  ========  構造函數  ========
        public ViewContent() { }

        public ViewContent(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ModelID":
                        this.ModelID = (int)reader[i];
                        break;
                    case "Language":
                        this.Language = (Language)reader[i];
                        break;
                    case "Path":
                        this.Path = (string)reader[i];
                        break;
                    case "Translate":
                        this.Translate = (decimal)reader[i];
                        break;
                }
            }
        }


        public ViewContent(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ModelID":
                        this.ModelID = (int)dr[i];
                        break;
                    case "Language":
                        this.Language = (Language)dr[i];
                        break;
                    case "Path":
                        this.Path = (string)dr[i];
                        break;
                    case "Translate":
                        this.Translate = (decimal)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 所关联的模型编号
        /// </summary>
        [Column("ModelID"),Key]
        public int ModelID { get; set; }


        /// <summary>
        /// 所适配的语种
        /// </summary>
        [Column("Language"),Key]
        public Language Language { get; set; }


        /// <summary>
        /// 文件内容
        /// </summary>
        [Column("Path")]
        public string Path { get; set; }


        /// <summary>
        /// 翻译进度
        /// </summary>
        [Column("Translate")]
        public decimal Translate { get; set; }

        #endregion


#region  ========  扩展方法  ========


        #endregion

    }

}
