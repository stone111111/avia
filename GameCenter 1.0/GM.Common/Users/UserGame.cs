using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;
using System.Linq;

namespace GM.Common.Users
{
    /// <summary>
    /// 会员的游戏账号
    /// </summary>
    [Table("usr_Game")]
    public partial class UserGame
    {

        #region  ========  構造函數  ========
        public UserGame() { }

        public UserGame(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "UserID":
                        this.UserID = (int)reader[i];
                        break;
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "Account":
                        this.Account = (string)reader[i];
                        break;
                    case "Password":
                        this.Password = (string)reader[i];
                        break;
                    case "Balance":
                        this.Balance = (decimal)reader[i];
                        break;
                    case "UpdateAt":
                        this.UpdateAt = (DateTime)reader[i];
                        break;
                    case "Status":
                        this.Status = (UserGameStatus)reader[i];
                        break;
                }
            }
        }


        public UserGame(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "UserID":
                        this.UserID = (int)dr[i];
                        break;
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "Account":
                        this.Account = (string)dr[i];
                        break;
                    case "Password":
                        this.Password = (string)dr[i];
                        break;
                    case "Balance":
                        this.Balance = (decimal)dr[i];
                        break;
                    case "UpdateAt":
                        this.UpdateAt = (DateTime)dr[i];
                        break;
                    case "Status":
                        this.Status = (UserGameStatus)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 会员ID
        /// </summary>
        [Column("UserID"), Key]
        public int UserID { get; set; }


        /// <summary>
        /// 游戏编号（对应 game_Setting.GameID)
        /// </summary>
        [Column("GameID"), Key]
        public int GameID { get; set; }


        /// <summary>
        /// 所属商户
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 用户在游戏中的账号
        /// </summary>
        [Column("Account")]
        public string Account { get; set; }


        /// <summary>
        /// 在此游戏中使用的密码
        /// </summary>
        [Column("Password")]
        public string Password { get; set; }


        /// <summary>
        /// 游戏账号余额
        /// </summary>
        [Column("Balance")]
        public decimal Balance { get; set; }


        /// <summary>
        /// 余额更新时间
        /// </summary>
        [Column("UpdateAt")]
        public DateTime UpdateAt { get; set; }


        /// <summary>
        /// 游戏账号状态 0：正常  1：禁止登录
        /// </summary>
        [Column("Status")]
        public UserGameStatus Status { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public enum UserGameStatus : byte
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal,
            /// <summary>
            /// 禁止登录
            /// </summary>
            Lock
        }

        public static implicit operator UserGame(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            return hashes.Fill<UserGame>();
        }

        public static implicit operator HashEntry[](UserGame userGame)
        {
            if (userGame == null) return null;
            return userGame.ToHashEntry().ToArray();
        }

        /// <summary>
        /// 组成一个唯一字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(this.GameID, "-", this.UserID);
        }
        #endregion

    }

}
