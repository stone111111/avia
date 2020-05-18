using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.Provider.CDN;

namespace BW.Common.Sites
{
    /// <summary>
    /// 域名记录
    /// </summary>
    [Table("site_DomainRecord")]
    public partial class DomainRecord
    {

        #region  ========  構造函數  ========
        public DomainRecord() { }

        public DomainRecord(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "RecordID":
                        this.ID = (int)reader[i];
                        break;
                    case "DomainID":
                        this.DomainID = (int)reader[i];
                        break;
                    case "SubName":
                        this.SubName = (string)reader[i];
                        break;
                    case "CDNType":
                        this.CDNType = (CDNProviderType)reader[i];
                        break;
                    case "CName":
                        this.CName = (string)reader[i];
                        break;
                    case "Status":
                        this.Status = (RecordStatus)reader[i];
                        break;
                }
            }
        }


        public DomainRecord(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "RecordID":
                        this.ID = (int)dr[i];
                        break;
                    case "DomainID":
                        this.DomainID = (int)dr[i];
                        break;
                    case "SubName":
                        this.SubName = (string)dr[i];
                        break;
                    case "CDNType":
                        this.CDNType = (CDNProviderType)dr[i];
                        break;
                    case "CName":
                        this.CName = (string)dr[i];
                        break;
                    case "Status":
                        this.Status = (RecordStatus)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 记录编号
        /// </summary>
        [Column("RecordID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 所属的主域名
        /// </summary>
        [Column("DomainID")]
        public int DomainID { get; set; }


        /// <summary>
        /// 子域名，支持 @ 和 *
        /// </summary>
        [Column("SubName")]
        public string SubName { get; set; }


        /// <summary>
        /// 当前选用的CDN供应商，
        /// </summary>
        [Column("CDNType")]
        public CDNProviderType CDNType { get; set; }


        /// <summary>
        /// 要指向的别名地址
        /// </summary>
        [Column("CName")]
        public string CName { get; set; }


        /// <summary>
        /// 记录状态
        /// </summary>
        [Column("Status")]
        public RecordStatus Status { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        /// <summary>
        /// 域名记录的状态
        /// </summary>
        public enum RecordStatus : byte
        {
            /// <summary>
            /// 等待中（设置了别名地址，等待CDN配置完成）
            /// </summary>
            Wait,
            /// <summary>
            /// 配置中
            /// </summary>
            Config,
            /// <summary>
            /// 发生错误
            /// </summary>
            Error,
            /// <summary>
            /// 正常
            /// </summary>
            Finish
        }
        #endregion

    }

}
