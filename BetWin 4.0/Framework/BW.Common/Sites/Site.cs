using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;
using System.ComponentModel;
using SP.StudioCore.Model;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;
using BW.Views;

namespace BW.Common.Sites
{
    /// <summary>
    /// 商户资料
    /// </summary>
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
                    case "PCTemplate":
                        this.PCTemplate = (int)reader[i];
                        break;
                    case "H5Template":
                        this.H5Template = (int)reader[i];
                        break;
                    case "APPTemplate":
                        this.APPTemplate = (int)reader[i];
                        break;
                    case "Setting":
                        this.SettingString = (string)reader[i];
                        break;
                    case "CreditType":
                        this.CreditType = (GameCreditType)reader[i];
                        break;
                    case "Remark":
                        this.Remark = (string)reader[i];
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
                    case "PCTemplate":
                        this.PCTemplate = (int)dr[i];
                        break;
                    case "H5Template":
                        this.H5Template = (int)dr[i];
                        break;
                    case "APPTemplate":
                        this.APPTemplate = (int)dr[i];
                        break;
                    case "Setting":
                        this.SettingString = (string)dr[i];
                        break;
                    case "CreditType":
                        this.CreditType = (GameCreditType)dr[i];
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
        /// 商户ID
        /// </summary>
        [Column("SiteID"), Key]
        public int ID { get; set; }


        /// <summary>
        /// 商户名（内部识别）
        /// </summary>
        [Column("SiteName")]
        public string Name { get; set; }


        /// <summary>
        /// 默认币种
        /// </summary>
        [Column("Currency")]
        public Currency Currency { get; set; }


        /// <summary>
        /// 默认语种
        /// </summary>
        [Column("Language")]
        public Language Language { get; set; }


        /// <summary>
        /// 商户状态
        /// </summary>
        [Column("Status")]
        public SiteStatus Status { get; set; }


        /// <summary>
        /// 默认的PC端模板
        /// </summary>
        [Column("PCTemplate")]
        public int PCTemplate { get; set; }


        /// <summary>
        /// 默认的H5端模板
        /// </summary>
        [Column("H5Template")]
        public int H5Template { get; set; }


        /// <summary>
        /// 默认的APP模板
        /// </summary>
        [Column("APPTemplate")]
        public int APPTemplate { get; set; }


        /// <summary>
        /// 商户设置
        /// </summary>
        [Column("Setting")]
        public string SettingString { get; set; }


        /// <summary>
        /// 游戏信用额度类型
        /// </summary>
        [Column("CreditType")]
        public GameCreditType CreditType { get; set; }


        /// <summary>
        /// 备注信息
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        private SiteSetting _setting;
        /// <summary>
        /// 商户参数配置
        /// </summary>
        [NotMapped]
        public SiteSetting Setting
        {
            get
            {
                if (this._setting == null) this._setting = new SiteSetting(this.SettingString);
                return this._setting;
            }
            set
            {
                this.SettingString = this._setting = value;
            }
        }

        public class SiteSetting : ISetting
        {
            public SiteSetting()
            {
            }

            public SiteSetting(string queryString) : base(queryString)
            {
            }

            /// <summary>
            /// 支持的币种
            /// </summary>
            public Currency[] Currencies { get; set; }

            /// <summary>
            /// 支持的语种
            /// </summary>
            public Language[] Languages { get; set; }
        }

        /// <summary>
        /// 商户状态
        /// </summary>
        public enum SiteStatus : byte
        {
            [Description("正常")]
            Normal,
            [Description("维护中")]
            Maintain,
            [Description("停止")]
            Stop
        }

        /// <summary>
        /// 游戏额度类型
        /// </summary>
        public enum GameCreditType : byte
        {
            /// <summary>
            /// 所有游戏公用一个额度
            /// </summary>
            [Description("月结")]
            Monthly,
            /// <summary>
            /// 每个游戏单独计算额度
            /// </summary>
            [Description("买分")]
            Credit
        }

        public static implicit operator HashEntry[](Site site)
        {
            return new[]
            {
                new HashEntry("ID",site.ID.GetRedisValue()),
                new HashEntry("Name",site.Name.GetRedisValue()),
                new HashEntry("Currency",site.Currency.GetRedisValue()),
                new HashEntry("Language",site.Language.GetRedisValue()),
                new HashEntry("Status",site.Status.GetRedisValue()),
                new HashEntry("PCTemplate",site.PCTemplate.GetRedisValue()),
                new HashEntry("H5Template",site.H5Template.GetRedisValue()),
                new HashEntry("APPTemplate",site.APPTemplate.GetRedisValue()),
                new HashEntry("SettingString",site.SettingString.GetRedisValue()),
                new HashEntry("CreditType",site.CreditType.GetRedisValue()),
                new HashEntry("Remark",site.Remark.GetRedisValue())
            };
        }

        public static implicit operator Site(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            Site site = new Site();
            foreach (HashEntry item in hashes)
            {
                switch (item.Name.GetRedisValue<string>())
                {
                    case "ID":
                        site.ID = item.Value.GetRedisValue<int>();
                        break;
                    case "Name":
                        site.Name = item.Value.GetRedisValue<string>();
                        break;
                    case "Currency":
                        site.Currency = item.Value.GetRedisValue<Currency>();
                        break;
                    case "Language":
                        site.Language = item.Value.GetRedisValue<Language>();
                        break;
                    case "Status":
                        site.Status = item.Value.GetRedisValue<SiteStatus>();
                        break;
                    case "PCTemplate":
                        site.PCTemplate = item.Value.GetRedisValue<int>();
                        break;
                    case "H5Template":
                        site.H5Template = item.Value.GetRedisValue<int>();
                        break;
                    case "APPTemplate":
                        site.APPTemplate = item.Value.GetRedisValue<int>();
                        break;
                    case "SettingString":
                        site.SettingString = item.Value.GetRedisValue<string>();
                        break;
                    case "CreditType":
                        site.CreditType = item.Value.GetRedisValue<GameCreditType>();
                        break;
                    case "Remark":
                        site.Remark = item.Value.GetRedisValue<string>();
                        break;
                }
            }
            return site;
        }

        /// <summary>
        /// 获取平台对应的默认模板
        /// C# 8.0 的新语法
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public int GetTemplateID(PlatformSource platform) =>
            platform switch
            {
                PlatformSource.PC => this.PCTemplate,
                PlatformSource.H5 => this.H5Template,
                PlatformSource.APP => this.APPTemplate,
                _ => 0
            };


        #endregion

    }

}
