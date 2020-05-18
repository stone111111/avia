using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SP.StudioCore.Enums;
using SP.Provider.Game.Models;

namespace GM.Common.Models
{
    /// <summary>
    /// 会员转账订单
    /// </summary>
    [Table("usr_Transfer")]
    public partial class UserTransfer
    {

        #region  ========  構造函數  ========
        public UserTransfer() { }

        public UserTransfer(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "OrderID":
                        this.OrderID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "UserID":
                        this.UserID = (int)reader[i];
                        break;
                    case "Money":
                        this.Money = (decimal)reader[i];
                        break;
                    case "Action":
                        this.Action = (TransferAction)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "FinishAt":
                        this.FinishAt = (DateTime)reader[i];
                        break;
                    case "SystemID":
                        this.SystemID = (string)reader[i];
                        break;
                    case "SourceID":
                        this.SourceID = (string)reader[i];
                        break;
                    case "Status":
                        this.Status = (TransferStatus)reader[i];
                        break;
                }
            }
        }


        public UserTransfer(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "OrderID":
                        this.OrderID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "UserID":
                        this.UserID = (int)dr[i];
                        break;
                    case "Money":
                        this.Money = (decimal)dr[i];
                        break;
                    case "Action":
                        this.Action = (TransferAction)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "FinishAt":
                        this.FinishAt = (DateTime)dr[i];
                        break;
                    case "SystemID":
                        this.SystemID = (string)dr[i];
                        break;
                    case "SourceID":
                        this.SourceID = (string)dr[i];
                        break;
                    case "Status":
                        this.Status = (TransferStatus)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 订单号
        /// </summary>
        [Column("OrderID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int OrderID { get; set; }


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
        /// 会员账号
        /// </summary>
        [Column("UserID")]
        public int UserID { get; set; }


        /// <summary>
        /// 转账金额（转入为正数，转出为负数）
        /// </summary>
        [Column("Money")]
        public decimal Money { get; set; }


        /// <summary>
        /// 转账操作（0：转入  1：转出)
        /// </summary>
        [Column("Action")]
        public TransferAction Action { get; set; }


        /// <summary>
        /// 订单创建时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 订单完成时间
        /// </summary>
        [Column("FinishAt")]
        public DateTime FinishAt { get; set; }


        /// <summary>
        /// 游戏厂商返回的转账系统单号（不一定会有）
        /// </summary>
        [Column("SystemID")]
        public string SystemID { get; set; }


        /// <summary>
        /// 来源订单号（商户提交的来源订单号，必须全局唯一）
        /// </summary>
        [Column("SourceID")]
        public string SourceID { get; set; }


        /// <summary>
        /// 状态  0：等待处理  1：成功  2：异常失败  3、4、5。。。 其他失败原因
        /// </summary>
        [Column("Status")]
        public TransferStatus Status { get; set; }

        #endregion


#region  ========  扩展方法  ========
        
        #endregion

    }

}
