using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;
using System.ComponentModel;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;
using System.Linq;

namespace GM.Common.Sites
{
    [Table("Site")]
    public partial class Site
    {

        #region  ========  構造函數  ========
        public Site() { }

        public Site(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "SiteID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteName":
                        this.Name = (string)reader[i];
                        break;
                    case "Currency":
                        this.Currency = (Currency)reader[i];
                        break;
                    case "Language":
                        this.Language = (Language)reader[i];
                        break;
                    case "Status":
                        this.Status = (SiteStatus)reader[i];
                        break;
                    case "SecretKey":
                        this.SecretKey = (string)reader[i];
                        break;
                    case "Prefix":
                        this.Prefix = (string)reader[i];
                        break;
                    case "WhiteIP":
                        this.WhiteIP = (string)reader[i];
                        break;
                }
            }
        }


        public Site(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "SiteID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteName":
                        this.Name = (string)dr[i];
                        break;
                    case "Currency":
                        this.Currency = (Currency)dr[i];
                        break;
                    case "Language":
                        this.Language = (Language)dr[i];
                        break;
                    case "Status":
                        this.Status = (SiteStatus)dr[i];
                        break;
                    case "SecretKey":
                        this.SecretKey = (string)dr[i];
                        break;
                    case "Prefix":
                        this.Prefix = (string)dr[i];
                        break;
                    case "WhiteIP":
                        this.WhiteIP = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 商户编号
        /// </summary>
        [Column("SiteID"),Key]
        public int ID { get; set; }


        /// <summary>
        /// 商户名称
        /// </summary>
        [Column("SiteName")]
        public string Name { get; set; }


        /// <summary>
        /// 默认货币
        /// </summary>
        [Column("Currency")]
        public Currency Currency { get; set; }


        /// <summary>
        /// 默认语言
        /// </summary>
        [Column("Language")]
        public Language Language { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [Column("Status")]
        public SiteStatus Status { get; set; }


        /// <summary>
        /// 商户密钥
        /// </summary>
        [Column("SecretKey")]
        public string SecretKey { get; set; }


        /// <summary>
        /// 用户名的前缀，固定3位
        /// </summary>
        [Column("Prefix")]
        public string Prefix { get; set; }


        [Column("WhiteIP")]
        public string WhiteIP { get; set; }

        #endregion


#region  ========  扩展方法  ========

        /// <summary>
        /// 商户状态
        /// </summary>
        public enum SiteStatus : byte
        {
            /// <summary>
            /// 开启
            /// </summary>
            [Description("开启")]
            Open = 1,
            /// <summary>
            /// 关闭
            /// </summary>
            [Description("关闭")]
            Close = 0
        }        public static implicit operator Site(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            return hashes.Fill<Site>();
        }

        public static implicit operator HashEntry[](Site site)
        {
            if (site == null || site.ID == 0) return null;
            return site.ToHashEntry().ToArray();
        }
        #endregion

    }

}
