using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Sites
{
    /// <summary>
    /// 商户的域名
    /// </summary>
    [Table("site_Domain")]
    public partial class SiteDomain
    {

        #region  ========  構造函數  ========
        public SiteDomain() { }

        public SiteDomain(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "DomainID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "Domain":
                        this.Domain = (string)reader[i];
                        break;
                }
            }
        }


        public SiteDomain(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "DomainID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "Domain":
                        this.Domain = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 域名ID
        /// </summary>
        [Column("DomainID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 域名（根域名）
        /// </summary>
        [Column("Domain")]
        public string Domain { get; set; }

        #endregion


        #region  ========  扩展方法  ========


        #endregion

    }

}
