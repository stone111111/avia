using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.Provider.Game;

namespace BW.Common.Providers
{
    /// <summary>
    /// 游戏供应商
    /// </summary>
    [Table("provider_Game")]
    public partial class GameProvider
    {

        #region  ========  構造函數  ========
        public GameProvider() { }

        public GameProvider(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ProviderID":
                        this.ID = (int)reader[i];
                        break;
                    case "ProviderName":
                        this.Name = (string)reader[i];
                        break;
                    case "Type":
                        this.Type = (GameProviderType)reader[i];
                        break;
                    case "Setting":
                        this.SettingString = (string)reader[i];
                        break;
                }
            }
        }


        public GameProvider(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ProviderID":
                        this.ID = (int)dr[i];
                        break;
                    case "ProviderName":
                        this.Name = (string)dr[i];
                        break;
                    case "Type":
                        this.Type = (GameProviderType)dr[i];
                        break;
                    case "Setting":
                        this.SettingString = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 游戏供应商ID
        /// </summary>
        [Column("ProviderID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 供应商名称
        /// </summary>
        [Column("ProviderName")]
        public string Name { get; set; }


        /// <summary>
        /// 供应商类型
        /// </summary>
        [Column("Type")]
        public GameProviderType Type { get; set; }


        /// <summary>
        /// 接口的参数配置内容
        /// </summary>
        [Column("Setting")]
        public string SettingString { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        /// <summary>
        /// 隐式转换成为游戏供应商接口实现
        /// </summary>
        /// <param name="provider"></param>
        public static implicit operator IGameProvider(GameProvider provider)
        {
            return GameFactory.GetFactory(provider.Type, provider.SettingString);
        }

        #endregion

    }

}
