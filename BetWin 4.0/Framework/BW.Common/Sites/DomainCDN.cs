using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SP.Provider.CDN;

namespace BW.Common.Sites
{
    /// <summary>
    ///  域名的CDN配置状态
    /// </summary>
    [Table("site_DomainCDN")]
    public partial class DomainCDN
    {

        #region  ========  構造函數  ========
        public DomainCDN() { }

        public DomainCDN(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "ConfigID":
                        this.ID = (int)reader[i];
                        break;
                    case "RecordID":
                        this.RecordID = (int)reader[i];
                        break;
                    case "CDNType":
                        this.CDNType = (CDNProviderType)reader[i];
                        break;
                    case "CName":
                        this.CName = (string)reader[i];
                        break;
                    case "Status":
                        this.Status = (CDNStatus)reader[i];
                        break;
                    case "Https":
                        this.Https = (CDNStatus)reader[i];
                        break;
                }
            }
        }


        public DomainCDN(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "ConfigID":
                        this.ID = (int)dr[i];
                        break;
                    case "RecordID":
                        this.RecordID = (int)dr[i];
                        break;
                    case "CDNType":
                        this.CDNType = (CDNProviderType)dr[i];
                        break;
                    case "CName":
                        this.CName = (string)dr[i];
                        break;
                    case "Status":
                        this.Status = (CDNStatus)dr[i];
                        break;
                    case "Https":
                        this.Https = (CDNStatus)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 配置ID
        /// </summary>
        [Column("ConfigID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 记录ID，关联 site_DomainRecord.RecordID
        /// </summary>
        [Column("RecordID")]
        public int RecordID { get; set; }


        /// <summary>
        /// 对应的CDN供应商
        /// </summary>
        [Column("CDNType")]
        public CDNProviderType CDNType { get; set; }


        /// <summary>
        /// CDN供应商提供的别名指向地址
        /// </summary>
        [Column("CName")]
        public string CName { get; set; }


        /// <summary>
        /// 与CDNAPI通信的状态
        /// </summary>
        [Column("Status")]
        public CDNStatus Status { get; set; }


        /// <summary>
        ///  https证书的配置状态
        /// </summary>
        [Column("Https")]
        public CDNStatus Https { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        /// <summary>
        /// 与CDN服务的通信状态
        /// </summary>
        public enum CDNStatus : byte
        {
            /// <summary>
            /// 未配置（等待开始指令）
            /// </summary>
            [Description("未配置")]
            None,
            /// <summary>
            /// 等待配置（等待服务程序处理）
            /// </summary>
            [Description("等待配置")]
            Wait,
            /// <summary>
            /// 配置中（服务程序已经开始处理，等待API接口返回）
            /// </summary>
            [Description("配置中")]
            Config,
            /// <summary>
            /// 发生错误（与CDN供应商进行API通信的过程中发生了错误，等待人工来处理)
            /// </summary>
            [Description("发生错误")]
            Error,
            /// <summary>
            /// 配置完成（配置完成，如果记录引用的供应商是本条记录，则修改记录的CNAME指向，并且设置记录的状态）
            /// </summary>
            [Description("配置完成")]
            Finish,
            Deleting
        }
        #endregion

    }

}
