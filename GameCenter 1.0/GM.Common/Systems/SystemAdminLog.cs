using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace GM.Common.Systems
{
    /// <summary>
    /// 系统管理员操作日志
    /// </summary>
    [Table("sys_AdminLog")]
    public partial class SystemAdminLog
    {

        #region  ========  構造函數  ========
        public SystemAdminLog() { }

        public SystemAdminLog(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "LogID":
                        this.ID = (int)reader[i];
                        break;
                    case "AdminID":
                        this.AdminID = (int)reader[i];
                        break;
                    case "Type":
                        this.Type = (LogType)reader[i];
                        break;
                    case "Content":
                        this.Content = (string)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "IP":
                        this.IP = (string)reader[i];
                        break;
                    case "PostData":
                        this.PostData = (string)reader[i];
                        break;
                }
            }
        }


        public SystemAdminLog(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "LogID":
                        this.ID = (int)dr[i];
                        break;
                    case "AdminID":
                        this.AdminID = (int)dr[i];
                        break;
                    case "Type":
                        this.Type = (LogType)dr[i];
                        break;
                    case "Content":
                        this.Content = (string)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "IP":
                        this.IP = (string)dr[i];
                        break;
                    case "PostData":
                        this.PostData = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        [Column("LogID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        [Column("AdminID")]
        public int AdminID { get; set; }


        /// <summary>
        /// 操作类型
        /// </summary>
        [Column("Type")]
        public LogType Type { get; set; }


        /// <summary>
        /// 操作内容
        /// </summary>
        [Column("Content")]
        public string Content { get; set; }


        /// <summary>
        /// 操作时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        [Column("IP")]
        public string IP { get; set; }


        /// <summary>
        /// HTTP交互原始数据
        /// </summary>
        [Column("PostData")]
        public string PostData { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public enum LogType : byte
        {
            /// <summary>
            /// 登录（包含登录成功和登录失败的记录）
            /// </summary>
            [Description("登录")]
            Login,
            [Description("商户资料")]
            Site,
            [Description("系统配置")]
            Setting,
            [Description("系统运维")]
            Set,
            [Description("视图模板")]
            View
        }
        #endregion

    }

}
