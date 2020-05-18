using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace GM.Common.Models
{
    /// <summary>
    /// 会员的日报表统计（均按照订单本地结算时间进行统计）
    /// </summary>
    [Table("rpt_UserDate")]
    public partial class UserDate
    {

        #region  ========  構造函數  ========
        public UserDate() { }

        public UserDate(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "UserID":
                        this.UserID = (int)reader[i];
                        break;
                    case "Date":
                        this.Date = (DateTime)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "BetMoney":
                        this.BetMoney = (decimal)reader[i];
                        break;
                    case "BetAmount":
                        this.BetAmount = (decimal)reader[i];
                        break;
                    case "Money":
                        this.Money = (decimal)reader[i];
                        break;
                    case "OrderCount":
                        this.OrderCount = (int)reader[i];
                        break;
                }
            }
        }


        public UserDate(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "UserID":
                        this.UserID = (int)dr[i];
                        break;
                    case "Date":
                        this.Date = (DateTime)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "BetMoney":
                        this.BetMoney = (decimal)dr[i];
                        break;
                    case "BetAmount":
                        this.BetAmount = (decimal)dr[i];
                        break;
                    case "Money":
                        this.Money = (decimal)dr[i];
                        break;
                    case "OrderCount":
                        this.OrderCount = (int)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 会员账号
        /// </summary>
        [Column("UserID"),Key]
        public int UserID { get; set; }


        /// <summary>
        /// 日期
        /// </summary>
        [Column("Date"),Key]
        public DateTime Date { get; set; }


        /// <summary>
        /// 商户编号
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 游戏编号
        /// </summary>
        [Column("GameID")]
        public int GameID { get; set; }


        /// <summary>
        /// 投注金额
        /// </summary>
        [Column("BetMoney")]
        public decimal BetMoney { get; set; }


        /// <summary>
        /// 有效投注金额
        /// </summary>
        [Column("BetAmount")]
        public decimal BetAmount { get; set; }


        /// <summary>
        /// 盈亏金额
        /// </summary>
        [Column("Money")]
        public decimal Money { get; set; }


        /// <summary>
        /// 订单数量
        /// </summary>
        [Column("OrderCount")]
        public int OrderCount { get; set; }

        #endregion


#region  ========  扩展方法  ========


        #endregion

    }

}
