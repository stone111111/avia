using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Enums;
using StackExchange.Redis;
using SP.StudioCore.Cache.Redis;
using System.Linq;

namespace GM.Common.Users
{
    /// <summary>
    /// 会员信息表
    /// </summary>
    [Table("Users")]
    public partial class User
    {

        #region  ========  構造函數  ========
        public User() { }

        public User(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "UserID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "UserName":
                        this.UserName = (string)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "LoginIP":
                        this.LoginIP = (string)reader[i];
                        break;
                    case "LoginAt":
                        this.LoginAt = (DateTime)reader[i];
                        break;
                    case "Status":
                        this.Status = (UserStatus)reader[i];
                        break;
                }
            }
        }


        public User(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "UserID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "UserName":
                        this.UserName = (string)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "LoginIP":
                        this.LoginIP = (string)dr[i];
                        break;
                    case "LoginAt":
                        this.LoginAt = (DateTime)dr[i];
                        break;
                    case "Status":
                        this.Status = (UserStatus)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 账号ID
        /// </summary>
        [Column("UserID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int ID { get; set; }


        /// <summary>
        /// 商户编号
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 账号名
        /// </summary>
        [Column("UserName")]
        public string UserName { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 最后登录IP
        /// </summary>
        [Column("LoginIP")]
        public string LoginIP { get; set; }


        /// <summary>
        /// 最后登陆时间
        /// </summary>
        [Column("LoginAt")]
        public DateTime LoginAt { get; set; }


        /// <summary>
        /// 会员整体状态 0：正常 1：锁定
        /// </summary>
        [Column("Status")]
        public UserStatus Status { get; set; }

        #endregion


#region  ========  扩展方法  ========

        public static implicit operator User(HashEntry[] hashes)
        {
            if (hashes == null || hashes.Length == 0) return null;
            return hashes.Fill<User>();
        }

        public static implicit operator HashEntry[](User user)
        {
            if (user == null) return null;
            return user.ToHashEntry().ToArray();
        }
        #endregion

    }

}
