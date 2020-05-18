using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SP.Provider.Game;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;
using System.Linq;

namespace GM.Common.Games
{
    /// <summary>
    /// 系统游戏设置
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
                    case "Type":
                        this.Type = (GameType)reader[i];
                        break;
                    case "GameName":
                        this.Name = (string)reader[i];
                        break;
                    case "GameCode":
                        this.Code = (string)reader[i];
                        break;
                    case "Setting":
                        this.SettingString = (string)reader[i];
                        break;
                    case "Status":
                        this.Status = (GameStatus)reader[i];
                        break;
                    case "MaintainTime":
                        this.MaintainTime = (DateTime)reader[i];
                        break;
                    case "Remark":
                        this.Remark = (string)reader[i];
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
                    case "Type":
                        this.Type = (GameType)dr[i];
                        break;
                    case "GameName":
                        this.Name = (string)dr[i];
                        break;
                    case "GameCode":
                        this.Code = (string)dr[i];
                        break;
                    case "Setting":
                        this.SettingString = (string)dr[i];
                        break;
                    case "Status":
                        this.Status = (GameStatus)dr[i];
                        break;
                    case "MaintainTime":
                        this.MaintainTime = (DateTime)dr[i];
                        break;
                    case "Remark":
                        this.Remark = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 游戏编号
        /// </summary>
        [Column("GameID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int ID { get; set; }


        /// <summary>
        /// 游戏厂商类型
        /// </summary>
        [Column("Type")]
        public GameType Type { get; set; }


        /// <summary>
        /// 游戏对外显示的名称
        /// </summary>
        [Column("GameName")]
        public string Name { get; set; }


        /// <summary>
        /// 自定义的游戏代码（API接入使用此值）
        /// </summary>
        [Column("GameCode")]
        public string Code { get; set; }


        /// <summary>
        /// 游戏参数设置
        /// </summary>
        [Column("Setting")]
        public string SettingString { get; set; }


        /// <summary>
        /// 游戏状态
        /// </summary>
        [Column("Status")]
        public GameStatus Status { get; set; }


        /// <summary>
        /// 预计的结束维护时间
        /// </summary>
        [Column("MaintainTime")]
        public DateTime MaintainTime { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }

        #endregion


#region  ========  扩展方法  ========

        private IGameProvider _setting;
        /// <summary>
        /// 游戏供应商对象
        /// </summary>
        [NotMapped]
        public IGameProvider Setting
        {
            get
            {
                if (this._setting == null) this._setting = GameFactory.GetFactory(this.Type.ToString(), this.SettingString);
                return this._setting;
            }
        }

        public static implicit operator GameSetting(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            return hashes.Fill<GameSetting>();
        }

        public static implicit operator HashEntry[](GameSetting setting)
        {
            if (setting == null) return null;
            return setting.ToHashEntry().ToArray();
        }
        #endregion

    }

}
