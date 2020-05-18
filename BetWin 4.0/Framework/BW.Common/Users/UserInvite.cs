using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Users
{
    /// <summary>
    /// 邀请码
    /// </summary>
    [Table("usr_Invite")]
    public partial class UserInvite
    {

        #region  ========  構造函數  ========
        public UserInvite() { }

        public UserInvite(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "InviteID":
                        this.ID = (string)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "UserID":
                        this.UserID = (int)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "Member":
                        this.Member = (int)reader[i];
                        break;
                    case "IsOpen":
                        this.IsOpen = (bool)reader[i];
                        break;
                }
            }
        }


        public UserInvite(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "InviteID":
                        this.ID = (string)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "UserID":
                        this.UserID = (int)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "Member":
                        this.Member = (int)dr[i];
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
        /// 邀请码，全局唯一
        /// </summary>
        [Column("InviteID"), Key]
        public string ID { get; set; }


        [Column("SiteID")]
        public int SiteID { get; set; }


        [Column("UserID")]
        public int UserID { get; set; }


        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 使用该邀请码注册的用户数量
        /// </summary>
        [Column("Member")]
        public int Member { get; set; }


        /// <summary>
        /// 是否开启该邀请码
        /// </summary>
        [Column("IsOpen")]
        public bool IsOpen { get; set; }

        #endregion


        #region  ========  扩展方法  ========


        #endregion

    }

}
