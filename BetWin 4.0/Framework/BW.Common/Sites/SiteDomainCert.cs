using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Sites
{
    /// <summary>
    /// 商户的域名证书
    /// </summary>
    [Table("site_DomainCert")]
    public partial class SiteDomainCert
    {

        #region  ========  構造函數  ========
        public SiteDomainCert() { }

        public SiteDomainCert(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "CertID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "CertName":
                        this.Name = (string)reader[i];
                        break;
                    case "Domain":
                        this.Domain = (string)reader[i];
                        break;
                    case "PEM":
                        this.PEM = (string)reader[i];
                        break;
                    case "KEY":
                        this.KEY = (string)reader[i];
                        break;
                    case "Expire":
                        this.Expire = (DateTime)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                }
            }
        }


        public SiteDomainCert(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "CertID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "CertName":
                        this.Name = (string)dr[i];
                        break;
                    case "Domain":
                        this.Domain = (string)dr[i];
                        break;
                    case "PEM":
                        this.PEM = (string)dr[i];
                        break;
                    case "KEY":
                        this.KEY = (string)dr[i];
                        break;
                    case "Expire":
                        this.Expire = (DateTime)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        [Column("CertID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int ID { get; set; }


        /// <summary>
        /// 商户ID
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 证书名称（一般读取证书中的主域名）
        /// </summary>
        [Column("CertName")]
        public string Name { get; set; }


        /// <summary>
        /// 证书中包含的域名，多个域名用逗号隔开
        /// </summary>
        [Column("Domain")]
        public string Domain { get; set; }


        /// <summary>
        /// 证书内容（PEM格式，即适用于Nginx的证书格式）
        /// </summary>
        [Column("PEM")]
        public string PEM { get; set; }


        /// <summary>
        /// 证书密钥
        /// </summary>
        [Column("KEY")]
        public string KEY { get; set; }


        /// <summary>
        /// 证书的到期时间
        /// </summary>
        [Column("Expire")]
        public DateTime Expire { get; set; }


        /// <summary>
        /// 创建/更新时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        #endregion

    }

}
