using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.Provider.Game;

namespace BW.Common.Sites
{
    /// <summary>
    /// 商户的游戏设定
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
                    case "SettingID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "Category":
                        this.Category = (GameCategory)reader[i];
                        break;
                    case "GameName":
                        this.Name = (string)reader[i];
                        break;
                    case "Credit":
                        this.Credit = (decimal)reader[i];
                        break;
                    case "Balance":
                        this.Balance = (decimal)reader[i];
                        break;
                    case "Rate":
                        this.Rate = (decimal)reader[i];
                        break;
                    case "IsOpen":
                        this.IsOpen = (bool)reader[i];
                        break;
                    case "Sort":
                        this.Sort = (short)reader[i];
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
                    case "SettingID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "Category":
                        this.Category = (GameCategory)dr[i];
                        break;
                    case "GameName":
                        this.Name = (string)dr[i];
                        break;
                    case "Credit":
                        this.Credit = (decimal)dr[i];
                        break;
                    case "Balance":
                        this.Balance = (decimal)dr[i];
                        break;
                    case "Rate":
                        this.Rate = (decimal)dr[i];
                        break;
                    case "IsOpen":
                        this.IsOpen = (bool)dr[i];
                        break;
                    case "Sort":
                        this.Sort = (short)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 游戏设置编号
        /// </summary>
        [Column("SettingID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int ID { get; set; }


        /// <summary>
        /// 商户编号
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 游戏编号（关联 game_Setting.GameID）
        /// </summary>
        [Column("GameID")]
        public int GameID { get; set; }


        /// <summary>
        /// 要开启的游戏类型
        /// </summary>
        [Column("Category")]
        public GameCategory Category { get; set; }


        /// <summary>
        /// 游戏名称
        /// </summary>
        [Column("GameName")]
        public string Name { get; set; }


        /// <summary>
        /// 信用额度（如果是买分账号，则此处存放当月已经购买的额度，用于月结清零）
        /// </summary>
        [Column("Credit")]
        public decimal Credit { get; set; }


        /// <summary>
        /// 剩余额度
        /// </summary>
        [Column("Balance")]
        public decimal Balance { get; set; }


        /// <summary>
        /// 买分/交收条件
        /// </summary>
        [Column("Rate")]
        public decimal Rate { get; set; }


        /// <summary>
        /// 是否开启游戏
        /// </summary>
        [Column("IsOpen")]
        public bool IsOpen { get; set; }


        /// <summary>
        /// 排序（从大到小)
        /// </summary>
        [Column("Sort")]
        public short Sort { get; set; }

        #endregion


#region  ========  扩展方法  ========


        #endregion

    }

}
