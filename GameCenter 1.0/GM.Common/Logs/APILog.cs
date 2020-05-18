using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.Provider.Game.Models;

namespace GM.Common.Logs
{
    /// <summary>
    /// 日志信息表
    /// </summary>
    [Table("log_API")]
    public partial class APILog
    {

        #region  ========  構造函數  ========
        public APILog() { }

        public APILog(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "LogID":
                        this.LogID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "Game":
                        this.Game = (string)reader[i];
                        break;
                    case "Url":
                        this.Url = (string)reader[i];
                        break;
                    case "PostData":
                        this.PostData = (string)reader[i];
                        break;
                    case "ResultData":
                        this.ResultData = (string)reader[i];
                        break;
                    case "Status":
                        this.Status = (ResultStatus)reader[i];
                        break;
                    case "Time":
                        this.Time = (int)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                }
            }
        }


        public APILog(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "LogID":
                        this.LogID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "Game":
                        this.Game = (string)dr[i];
                        break;
                    case "Url":
                        this.Url = (string)dr[i];
                        break;
                    case "PostData":
                        this.PostData = (string)dr[i];
                        break;
                    case "ResultData":
                        this.ResultData = (string)dr[i];
                        break;
                    case "Status":
                        this.Status = (ResultStatus)dr[i];
                        break;
                    case "Time":
                        this.Time = (int)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 日志编号
        /// </summary>
        [Column("LogID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int LogID { get; set; }


        /// <summary>
        /// 所属商户
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 游戏类型
        /// </summary>
        [Column("Game")]
        public string Game { get; set; }


        /// <summary>
        /// 请求地址
        /// </summary>
        [Column("Url")]
        public string Url { get; set; }


        /// <summary>
        /// 发送数据
        /// </summary>
        [Column("PostData")]
        public string PostData { get; set; }


        /// <summary>
        /// 返回数据
        /// </summary>
        [Column("ResultData")]
        public string ResultData { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [Column("Status")]
        public ResultStatus Status { get; set; }


        /// <summary>
        /// 耗时
        /// </summary>
        [Column("Time")]
        public int Time { get; set; }


        /// <summary>
        /// 发生时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }

        #endregion


#region  ========  扩展方法  ========

        #endregion

    }

}
