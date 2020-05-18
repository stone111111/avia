using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace BW.Common.Sites
{
    /// <summary>
    /// 信用额度变化历史记录表
    /// </summary>
    [Table("site_CreditLog")]
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
                    case "Type":
                        this.Type = (CreditLogType)reader[i];
                        break;
                    case "Credit":
                        this.Credit = (decimal)reader[i];
                        break;
                    case "Balance":
                        this.Balance = (decimal)reader[i];
                        break;
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "OrderID":
                        this.OrderID = (int)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "LogDesc":
                        this.Description = (string)reader[i];
                        break;
                    case "AdminID":
                        this.AdminID = (int)reader[i];
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
                    case "Type":
                        this.Type = (CreditLogType)dr[i];
                        break;
                    case "Credit":
                        this.Credit = (decimal)dr[i];
                        break;
                    case "Balance":
                        this.Balance = (decimal)dr[i];
                        break;
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "OrderID":
                        this.OrderID = (int)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "LogDesc":
                        this.Description = (string)dr[i];
                        break;
                    case "AdminID":
                        this.AdminID = (int)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 记录编号
        /// </summary>
        [Column("LogID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 商户编号
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 变化类型
        /// </summary>
        [Column("Type")]
        public CreditLogType Type { get; set; }


        /// <summary>
        /// 变化额度
        /// </summary>
        [Column("Credit")]
        public decimal Credit { get; set; }


        /// <summary>
        /// 变化之后的额度余额
        /// </summary>
        [Column("Balance")]
        public decimal Balance { get; set; }


        /// <summary>
        /// 游戏编号，只有商户类型为买分且账变类型为转入转出才有值，其他情况为0
        /// </summary>
        [Column("GameID")]
        public int GameID { get; set; }


        /// <summary>
        /// 对应的转账订单编号（关联 game_TranslateOrder），只有类型为转入转出才有值
        /// </summary>
        [Column("OrderID")]
        public int OrderID { get; set; }


        /// <summary>
        /// 记录生成时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 备注信息
        /// </summary>
        [Column("LogDesc")]
        public string Description { get; set; }


        /// <summary>
        /// 加款操作的系统管理员ID
        /// </summary>
        [Column("AdminID")]
        public int AdminID { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public enum CreditLogType : byte
        {
            /// <summary>
            /// 管理员增加额度
            /// </summary>
            [Description("增加额度")]
            AddCredit,
            /// <summary>
            /// 转入到游戏（可用额度减少）
            /// </summary>
            [Description("转入游戏")]
            TranslateIn,
            /// <summary>
            /// 从游戏转出（可用额度增加）
            /// </summary>
            [Description("游戏转出")]
            TranslateOut,
            /// <summary>
            /// 买分商户专用，月结的时候清理超过购买的额度部分
            /// </summary>
            [Description("月结清理")]
            Monthly
        }
        #endregion

    }

}
