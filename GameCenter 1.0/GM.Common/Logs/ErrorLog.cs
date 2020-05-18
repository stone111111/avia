using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace GM.Common.Logs
{
    /// <summary>
    /// 错误日志
    /// </summary>
    [Table("log_Error")]
    public partial class ErrorLog
    {

        #region  ========  構造函數  ========
        public ErrorLog() { }

        public ErrorLog(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ErrorID":
                        this.ErrorID = (Guid)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "UserID":
                        this.UserID = (int)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "IP":
                        this.IP = (string)reader[i];
                        break;
                    case "IPAddress":
                        this.IPAddress = (string)reader[i];
                        break;
                    case "Title":
                        this.Title = (string)reader[i];
                        break;
                    case "Content":
                        this.Content = (string)reader[i];
                        break;
                }
            }
        }

        public ErrorLog(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ErrorID":
                        this.ErrorID = (Guid)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "UserID":
                        this.UserID = (int)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "IP":
                        this.IP = (string)dr[i];
                        break;
                    case "IPAddress":
                        this.IPAddress = (string)dr[i];
                        break;
                    case "Title":
                        this.Title = (string)dr[i];
                        break;
                    case "Content":
                        this.Content = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        [Column("ErrorID"), Key]
        public Guid ErrorID { get; set; }


        /// <summary>
        /// 发生错误的商户
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 产生错误的用户
        /// </summary>
        [Column("UserID")]
        public int UserID { get; set; }


        /// <summary>
        /// 错误发生时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        [Column("IP")]
        public string IP { get; set; }


        [Column("IPAddress")]
        public string IPAddress { get; set; }


        /// <summary>
        /// 错误标题
        /// </summary>
        [Column("Title")]
        public string Title { get; set; }


        /// <summary>
        /// 错误内容
        /// </summary>
        [Column("Content")]
        public string Content { get; set; }

        #endregion


        #region  ========  扩展方法  ========


        #endregion

    }

}
