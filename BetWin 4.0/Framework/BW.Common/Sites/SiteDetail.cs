using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Sites
{
    /// <summary>
    /// 商户的详细资料
    /// </summary>
    [Table("site_Detail")]
    public partial class SiteDetail
    {

        #region  ========  構造函數  ========
        public SiteDetail() { }

        public SiteDetail(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "Mobile":
                        this.Mobile = (string)reader[i];
                        break;
                    case "Email":
                        this.Email = (string)reader[i];
                        break;
                    case "WhiteIP":
                        this.WhiteIP = (string)reader[i];
                        break;
                    case "AdminURL":
                        this.AdminURL = (string)reader[i];
                        break;
                }
            }
        }


        public SiteDetail(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "Mobile":
                        this.Mobile = (string)dr[i];
                        break;
                    case "Email":
                        this.Email = (string)dr[i];
                        break;
                    case "WhiteIP":
                        this.WhiteIP = (string)dr[i];
                        break;
                    case "AdminURL":
                        this.AdminURL = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        [Column("SiteID"), Key]
        public int SiteID { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        [Column("Mobile")]
        public string Mobile { get; set; }


        /// <summary>
        /// 电子邮件
        /// </summary>
        [Column("Email")]
        public string Email { get; set; }


        /// <summary>
        /// 商户白名单
        /// </summary>
        [Column("WhiteIP")]
        public string WhiteIP { get; set; }


        /// <summary>
        /// 商户后台管理地址
        /// </summary>
        [Column("AdminURL")]
        public string AdminURL { get; set; }

        #endregion


        #region  ========  扩展方法  ========


        #endregion

    }

}
