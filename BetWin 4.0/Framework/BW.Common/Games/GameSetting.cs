using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.Provider.Game;

namespace BW.Common.Games
{
    /// <summary>
    /// 游戏参数配置
    /// </summary>
    [Table("game_Setting")]
    public partial class GameSetting
    {

        #region  ========  構造函數  ========
        public GameSetting() { }

        public GameSetting(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "GameID":
                        this.ID = (int)reader[i];
                        break;
                    case "ProviderID":
                        this.ProviderID = (int)reader[i];
                        break;
                    case "GameName":
                        this.Name = (string)reader[i];
                        break;
                    case "Type":
                        this.Type = (string)reader[i];
                        break;
                    case "Category":
                        this.Category = (GameCategory)reader[i];
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


        public GameSetting(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "GameID":
                        this.ID = (int)dr[i];
                        break;
                    case "ProviderID":
                        this.ProviderID = (int)dr[i];
                        break;
                    case "GameName":
                        this.Name = (string)dr[i];
                        break;
                    case "Type":
                        this.Type = (string)dr[i];
                        break;
                    case "Category":
                        this.Category = (GameCategory)dr[i];
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
        /// 游戏编号
        /// </summary>
        [Column("GameID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 游戏供应商
        /// </summary>
        [Column("ProviderID")]
        public int ProviderID { get; set; }


        /// <summary>
        /// 游戏名称
        /// </summary>
        [Column("GameName")]
        public string Name { get; set; }


        /// <summary>
        /// 游戏类型（代码）
        /// </summary>
        [Column("Type")]
        public string Type { get; set; }


        /// <summary>
        /// 当前接口所包含的游戏类型
        /// </summary>
        [Column("Category")]
        public GameCategory Category { get; set; }


        /// <summary>
        /// 是否开放接口
        /// </summary>
        [Column("IsOpen")]
        public bool IsOpen { get; set; }


        /// <summary>
        /// 排序值（从大到小)
        /// </summary>
        [Column("Sort")]
        public short Sort { get; set; }

        #endregion


        #region  ========  扩展方法  ========


        #endregion

    }

}
