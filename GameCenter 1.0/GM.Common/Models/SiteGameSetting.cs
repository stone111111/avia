using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace GM.Common.Models
{
    /// <summary>
    /// 商户的游戏开通情况
    /// </summary>
    [Table("site_GameSetting")]
    public partial class SiteGameSetting
    {

        #region  ========  構造函數  ========
        public SiteGameSetting() { }

        public SiteGameSetting(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "Credit":
                        this.Credit = (decimal)reader[i];
                        break;
                    case "Paid":
                        this.Paid = (decimal)reader[i];
                        break;
                    case "Rate":
                        this.Rate = (decimal)reader[i];
                        break;
                    case "Sort":
                        this.Sort = (byte)reader[i];
                        break;
                    case "Status":
                        this.Status = (SiteGameStatus)reader[i];
                        break;
                }
            }
        }


        public SiteGameSetting(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "Credit":
                        this.Credit = (decimal)dr[i];
                        break;
                    case "Paid":
                        this.Paid = (decimal)dr[i];
                        break;
                    case "Rate":
                        this.Rate = (decimal)dr[i];
                        break;
                    case "Sort":
                        this.Sort = (byte)dr[i];
                        break;
                    case "Status":
                        this.Status = (SiteGameStatus)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        [Column("ID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int ID { get; set; }


        /// <summary>
        /// 商户编号
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 游戏编号（关联 game_Setting.GameID)
        /// </summary>
        [Column("GameID")]
        public int GameID { get; set; }


        /// <summary>
        /// 当前信用额度
        /// </summary>
        [Column("Credit")]
        public decimal Credit { get; set; }


        /// <summary>
        /// 剩余额度
        /// </summary>
        [Column("Paid")]
        public decimal Paid { get; set; }


        /// <summary>
        /// 交收条件
        /// </summary>
        [Column("Rate")]
        public decimal Rate { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        [Column("Sort")]
        public byte Sort { get; set; }


        /// <summary>
        /// 商户的游戏接口状态   0、停止   1、正常
        /// </summary>
        [Column("Status")]
        public SiteGameStatus Status { get; set; }

        #endregion


#region  ========  扩展方法  ========
        /// <summary>
        /// 用户游戏表状态
        /// </summary>
        public enum SiteGameStatus : byte
        {
            /// <summary>
            /// 开启
            /// </summary>
            [Description("开启")]
            Open = 1,
            /// <summary>
            /// 关闭
            /// </summary>
            [Description("关闭")]
            Close = 0,
            /// <summary>
            /// 禁用(系统控制，禁用后，商户无法自行开启游戏)
            /// </summary>
            [Description("禁用")]
            Prohibit = 2
        }
        #endregion

    }

}
