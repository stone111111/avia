using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SP.Provider.CDN;

namespace BW.Common.Providers
{
    /// <summary>
    /// CDN供应商的配置
    /// </summary>
    [Table("provider_CDN")]
    public partial class CDNProvider
    {

        #region  ========  構造函數  ========
        public CDNProvider() { }

        public CDNProvider(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Type":
                        this.Type = (CDNProviderType)reader[i];
                        break;
                    case "Setting":
                        this.SettingString = (string)reader[i];
                        break;
                    case "IsOpen":
                        this.IsOpen = (bool)reader[i];
                        break;
                }
            }
        }


        public CDNProvider(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "Type":
                        this.Type = (CDNProviderType)dr[i];
                        break;
                    case "Setting":
                        this.SettingString = (string)dr[i];
                        break;
                    case "IsOpen":
                        this.IsOpen = (bool)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// CDN供应商的类型
        /// </summary>
        [Column("Type"), Key]
        public CDNProviderType Type { get; set; }


        /// <summary>
        /// CDN供应商的参数配置
        /// </summary>
        [Column("Setting")]
        public string SettingString { get; set; }


        /// <summary>
        /// 是否开启
        /// </summary>
        [Column("IsOpen")]
        public bool IsOpen { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        private ICDNProvider _cdnProvider;
        [NotMapped]
        public ICDNProvider Setting
        {
            get
            {
                if (this.Type == CDNProviderType.Manual) return null;
                if (this._cdnProvider == null)
                {
                    _cdnProvider = CDNFactory.GetFactory(this.Type, this.SettingString);
                }
                return this._cdnProvider;
            }
            set
            {
                this.SettingString = this._cdnProvider = value;
            }
        }

        #endregion

    }

}
