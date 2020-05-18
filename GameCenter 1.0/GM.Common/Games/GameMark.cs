using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.Provider.Game.Models;

namespace GM.Common.Games
{
    /// <summary>
    /// 游戏的时间戳标记
    /// </summary>
    [Table("game_Mark")]
    public partial class GameMark
    {

        #region  ========  構造函數  ========
        public GameMark() { }

        public GameMark(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "GameID":
                        this.GameID = (int)reader[i];
                        break;
                    case "Mark":
                        this.Mark = (byte)reader[i];
                        break;
                    case "Type":
                        this.Type = (MarkType)reader[i];
                        break;
                    case "Time":
                        this.Time = (long)reader[i];
                        break;
                }
            }
        }


        public GameMark(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "GameID":
                        this.GameID = (int)dr[i];
                        break;
                    case "Mark":
                        this.Mark = (byte)dr[i];
                        break;
                    case "Type":
                        this.Type = (MarkType)dr[i];
                        break;
                    case "Time":
                        this.Time = (long)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 所属游戏
        /// </summary>
        [Column("GameID"), Key]
        public int GameID { get; set; }


        /// <summary>
        /// 标记类型
        /// </summary>
        [Column("Mark"), Key]
        public byte Mark { get; set; }


        /// <summary>
        /// 标记类型（正常采集还是延时采集）
        /// </summary>
        [Column("Type"), Key]
        public MarkType Type { get; set; }


        /// <summary>
        /// 时间戳
        /// </summary>
        [Column("Time")]
        public long Time { get; set; }

        #endregion


        #region  ========  扩展方法  ========


        #endregion

    }

}
