using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Sites
{
    /// <summary>
    /// 与域名服务供应商通信过程中产生的日志（包括DNS、CDN）
    /// </summary>
    [Table("site_DomainLog")]
    public partial class DomainLog
    {

        #region  ========  構造函數  ========
        public DomainLog() { }

        public DomainLog(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "LogID":
                        this.ID = (int)reader[i];
                        break;
                    case "RecordID":
                        this.RecordID = (int)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "Content":
                        this.Content = (string)reader[i];
                        break;
                }
            }
        }


        public DomainLog(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "LogID":
                        this.ID = (int)dr[i];
                        break;
                    case "RecordID":
                        this.RecordID = (int)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "Content":
                        this.Content = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 日志编号
        /// </summary>
        [Column("LogID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 所关联的域名记录
        /// </summary>
        [Column("RecordID")]
        public int RecordID { get; set; }


        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 日志内容
        /// </summary>
        [Column("Content")]
        public string Content { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        #endregion

    }

}
