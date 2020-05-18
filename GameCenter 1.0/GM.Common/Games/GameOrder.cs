using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.Provider.Game.Models;
using SP.StudioCore.Enums;
using SP.StudioCore.Web;

namespace GM.Common.Games
{
    /// <summary>
    /// 全局的游戏订单
    /// </summary>
    [Table("game_Order")]
    public partial class GameOrder
    {

        #region  ========  構造函數  ========
        public GameOrder() { }

        public GameOrder(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "OrderID":
                        this.ID = (int)reader[i];
                        break;
                    case "Type":
                        this.Type = (GameType)reader[i];
                        break;
                    case "SourceID":
                        this.SourceID = (string)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "UserID":
                        this.UserID = (int)reader[i];
                        break;
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "PlayerName":
                        this.PlayerName = (string)reader[i];
                        break;
                    case "Code":
                        this.Code = (string)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "ResultAt":
                        this.ResultAt = (DateTime)reader[i];
                        break;
                    case "SettlementAt":
                        this.SettlementAt = (DateTime)reader[i];
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
                    case "Status":
                        this.Status = (OrderStatus)reader[i];
                        break;
                    case "Content":
                        this.Content = (string)reader[i];
                        break;
                    case "UpdateTime":
                        this.UpdateTime = (long)reader[i];
                        break;
                    case "HashCode":
                        this.HashCode = (long)reader[i];
                        break;
                    case "RawData":
                        this.RawData = (string)reader[i];
                        break;
                }
            }
        }


        public GameOrder(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "OrderID":
                        this.ID = (int)dr[i];
                        break;
                    case "Type":
                        this.Type = (GameType)dr[i];
                        break;
                    case "SourceID":
                        this.SourceID = (string)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "UserID":
                        this.UserID = (int)dr[i];
                        break;
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "PlayerName":
                        this.PlayerName = (string)dr[i];
                        break;
                    case "Code":
                        this.Code = (string)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "ResultAt":
                        this.ResultAt = (DateTime)dr[i];
                        break;
                    case "SettlementAt":
                        this.SettlementAt = (DateTime)dr[i];
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
                    case "Status":
                        this.Status = (OrderStatus)dr[i];
                        break;
                    case "Content":
                        this.Content = (string)dr[i];
                        break;
                    case "UpdateTime":
                        this.UpdateTime = (long)dr[i];
                        break;
                    case "HashCode":
                        this.HashCode = (long)dr[i];
                        break;
                    case "RawData":
                        this.RawData = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 本地订单号
        /// </summary>
        [Column("OrderID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 游戏类型
        /// </summary>
        [Column("Type")]
        public GameType Type { get; set; }


        /// <summary>
        /// 游戏订单号
        /// </summary>
        [Column("SourceID")]
        public string SourceID { get; set; }


        /// <summary>
        /// 商户编号
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 会员账号
        /// </summary>
        [Column("UserID")]
        public int UserID { get; set; }


        /// <summary>
        /// 所属的系统游戏配置编号(关联game_Setting.GameID)
        /// </summary>
        [Column("GameID")]
        public int GameID { get; set; }


        /// <summary>
        /// 供应商处的用户名
        /// </summary>
        [Column("PlayerName")]
        public string PlayerName { get; set; }


        /// <summary>
        /// 游戏代码
        /// </summary>
        [Column("Code")]
        public string Code { get; set; }


        /// <summary>
        /// 游戏下注时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 订单的派奖时间
        /// </summary>
        [Column("ResultAt")]
        public DateTime ResultAt { get; set; }


        /// <summary>
        /// 订单的结算时间（本地时间)
        /// </summary>
        [Column("SettlementAt")]
        public DateTime SettlementAt { get; set; }


        /// <summary>
        /// 投注金额
        /// </summary>
        [Column("BetMoney")]
        public decimal BetMoney { get; set; }


        /// <summary>
        /// 有效投注
        /// </summary>
        [Column("BetAmount")]
        public decimal BetAmount { get; set; }


        /// <summary>
        /// 盈亏
        /// </summary>
        [Column("Money")]
        public decimal Money { get; set; }


        /// <summary>
        /// 订单状态
        /// </summary>
        [Column("Status")]
        public OrderStatus Status { get; set; }


        /// <summary>
        /// 订单摘要信息
        /// </summary>
        [Column("Content")]
        public string Content { get; set; }


        /// <summary>
        /// 订单数据的更新时间戳（秒）
        /// </summary>
        [Column("UpdateTime")]
        public long UpdateTime { get; set; }


        /// <summary>
        /// 原始数据的HASHCODE
        /// </summary>
        [Column("HashCode")]
        public long HashCode { get; set; }


        /// <summary>
        /// 原始数据
        /// </summary>
        [Column("RawData")]
        public string RawData { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        /// <summary>
        /// 采集的数据赋值
        /// </summary>
        /// <param name="model"></param>
        public GameOrder(OrderModel model)
        {
            this.Type = model.Provider.ToEnum<GameType>();
            this.PlayerName = model.UserName;
            this.SourceID = model.SourceID;
            this.Code = model.Code;
            this.CreateAt = model.CreateAt;
            this.ResultAt = model.ResultAt;
            this.BetMoney = model.BetMoney;
            this.BetAmount = model.BetAmount;
            this.Money = model.Money;
            this.Status = model.Status;
            this.Content = model.Content;
            this.UpdateTime = WebAgent.GetTimestamp();
            this.HashCode = model.HashCode;
            this.RawData = model.RawData;
        }

        #endregion

    }

}
