using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SP.StudioCore.Enums;

namespace GM.Common.Models
{
    /// <summary>
    /// 信用额度变化历史记录表
    /// </summary>
    [Table("site_CreidtLog")]
    public partial class CreditLog
    {

        #region  ========  構造函數  ========
        public CreditLog() { }

        public CreditLog(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "LogID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "ChangeType":
                        this.Type = (ChangeType)reader[i];
                        break;
                    case "ChangeCredit":
                        this.ChangeCredit = (decimal)reader[i];
                        break;
                    case "Balance":
                        this.Balance = (decimal)reader[i];
                        break;
                    case "OrderID":
                        this.OrderID = (string)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "LogDesc":
                        this.Description = (string)reader[i];
                        break;
                }
            }
        }


        public CreditLog(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "LogID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "ChangeType":
                        this.Type = (ChangeType)dr[i];
                        break;
                    case "ChangeCredit":
                        this.ChangeCredit = (decimal)dr[i];
                        break;
                    case "Balance":
                        this.Balance = (decimal)dr[i];
                        break;
                    case "OrderID":
                        this.OrderID = (string)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "LogDesc":
                        this.Description = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 记录编号
        /// </summary>
        [Column("LogID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int ID { get; set; }


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
        /// 额度变化类型 1、转入  2、转出 3、增加额度
        /// </summary>
        [Column("ChangeType")]
        public ChangeType Type { get; set; }


        /// <summary>
        /// 变化额度
        /// </summary>
        [Column("ChangeCredit")]
        public decimal ChangeCredit { get; set; }


        /// <summary>
        /// 变化之后的额度
        /// </summary>
        [Column("Balance")]
        public decimal Balance { get; set; }


        /// <summary>
        /// 订单编号
        /// </summary>
        [Column("OrderID")]
        public string OrderID { get; set; }


        /// <summary>
        /// 变化日期
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 备注信息
        /// </summary>
        [Column("LogDesc")]
        public string Description { get; set; }

        #endregion


#region  ========  扩展方法  ========

        /// <summary>
        /// 1、转入  2、转出 3、增加额度
        /// </summary>
        public enum ChangeType : byte
        {
            /// <summary>
            /// 转入
            /// </summary>
            [Description("转入")]
            Into = 1,
            /// <summary>
            /// 转出
            /// </summary>
            [Description("转出")]
            Out = 2,
            /// <summary>
            /// 增加额度
            /// </summary>
            [Description("增加额度")]
            Add = 3,
        }

        #endregion

    }

}
