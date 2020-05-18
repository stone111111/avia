using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Users
{
    /// <summary>
    /// 用户的扩展详细信息
    /// </summary>
    [Table("usr_Detail")]
    public partial class UserDetail
    {

        #region  ========  構造函數  ========
        public UserDetail() { }

        public UserDetail(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "UserID":
                        this.UserID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "Birthday":
                        this.Birthday = (DateTime)reader[i];
                        break;
                    case "Country":
                        this.Country = (string)reader[i];
                        break;
                    case "Provice":
                        this.Provice = (string)reader[i];
                        break;
                    case "City":
                        this.City = (string)reader[i];
                        break;
                    case "Address":
                        this.Address = (string)reader[i];
                        break;
                    case "LinkMan":
                        this.LinkMan = (string)reader[i];
                        break;
                    case "Tel":
                        this.Tel = (string)reader[i];
                        break;
                }
            }
        }


        public UserDetail(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "UserID":
                        this.UserID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "Birthday":
                        this.Birthday = (DateTime)dr[i];
                        break;
                    case "Country":
                        this.Country = (string)dr[i];
                        break;
                    case "Provice":
                        this.Provice = (string)dr[i];
                        break;
                    case "City":
                        this.City = (string)dr[i];
                        break;
                    case "Address":
                        this.Address = (string)dr[i];
                        break;
                    case "LinkMan":
                        this.LinkMan = (string)dr[i];
                        break;
                    case "Tel":
                        this.Tel = (string)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        [Column("UserID"), Key]
        public int UserID { get; set; }


        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 会员生日
        /// </summary>
        [Column("Birthday")]
        public DateTime Birthday { get; set; }


        /// <summary>
        /// 国家
        /// </summary>
        [Column("Country")]
        public string Country { get; set; }


        /// <summary>
        /// 地区
        /// </summary>
        [Column("Provice")]
        public string Provice { get; set; }


        /// <summary>
        /// 城市
        /// </summary>
        [Column("City")]
        public string City { get; set; }


        /// <summary>
        /// 详细地址
        /// </summary>
        [Column("Address")]
        public string Address { get; set; }


        /// <summary>
        /// 联系人
        /// </summary>
        [Column("LinkMan")]
        public string LinkMan { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>
        [Column("Tel")]
        public string Tel { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        #endregion

    }

}
