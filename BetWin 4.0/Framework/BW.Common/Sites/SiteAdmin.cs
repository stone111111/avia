using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace BW.Common.Sites
{
    /// <summary>
    /// 商户管理员
    /// </summary>
    [Table("site_Admin")]
    public partial class SiteAdmin
    {

        #region  ========  構造函數  ========
        public SiteAdmin() { }

        public SiteAdmin(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "AdminID":
                        this.ID = (int)reader[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)reader[i];
                        break;
                    case "AdminName":
                        this.AdminName = (string)reader[i];
                        break;
                    case "Password":
                        this.Password = (string)reader[i];
                        break;
                    case "NickName":
                        this.NickName = (string)reader[i];
                        break;
                    case "Face":
                        this.Face = (string)reader[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)reader[i];
                        break;
                    case "LoginAt":
                        this.LoginAt = (DateTime)reader[i];
                        break;
                    case "LoginIP":
                        this.LoginIP = (string)reader[i];
                        break;
                    case "Status":
                        this.Status = (AdminStatus)reader[i];
                        break;
                    case "IsDefault":
                        this.IsDefault = (bool)reader[i];
                        break;
                    case "Permission":
                        this.Permission = (string)reader[i];
                        break;
                    case "SecretKey":
                        this.SecretKey = (Guid)reader[i];
                        break;
                }
            }
        }


        public SiteAdmin(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "AdminID":
                        this.ID = (int)dr[i];
                        break;
                    case "SiteID":
                        this.SiteID = (int)dr[i];
                        break;
                    case "AdminName":
                        this.AdminName = (string)dr[i];
                        break;
                    case "Password":
                        this.Password = (string)dr[i];
                        break;
                    case "NickName":
                        this.NickName = (string)dr[i];
                        break;
                    case "Face":
                        this.Face = (string)dr[i];
                        break;
                    case "CreateAt":
                        this.CreateAt = (DateTime)dr[i];
                        break;
                    case "LoginAt":
                        this.LoginAt = (DateTime)dr[i];
                        break;
                    case "LoginIP":
                        this.LoginIP = (string)dr[i];
                        break;
                    case "Status":
                        this.Status = (AdminStatus)dr[i];
                        break;
                    case "IsDefault":
                        this.IsDefault = (bool)dr[i];
                        break;
                    case "Permission":
                        this.Permission = (string)dr[i];
                        break;
                    case "SecretKey":
                        this.SecretKey = (Guid)dr[i];
                        break;
                }
            }
        }

        #endregion

        #region  ========  数据库字段  ========

        /// <summary>
        /// 商户管理员编号
        /// </summary>
        [Column("AdminID"),DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int ID { get; set; }


        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 管理员用户名
        /// </summary>
        [Column("AdminName")]
        public string AdminName { get; set; }


        /// <summary>
        /// 管理员密码（SHA1+MD5 双重加密，40位）
        /// </summary>
        [Column("Password")]
        public string Password { get; set; }


        /// <summary>
        /// 昵称
        /// </summary>
        [Column("NickName")]
        public string NickName { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [Column("Face")]
        public string Face { get; set; }


        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        [Column("LoginAt")]
        public DateTime LoginAt { get; set; }


        [Column("LoginIP")]
        public string LoginIP { get; set; }


        /// <summary>
        /// 管理员状态
        /// </summary>
        [Column("Status")]
        public AdminStatus Status { get; set; }


        /// <summary>
        /// 是否是默认管理员账号（拥有全部权限)
        /// </summary>
        [Column("IsDefault")]
        public bool IsDefault { get; set; }


        /// <summary>
        /// 权限列表
        /// </summary>
        [Column("Permission")]
        public string Permission { get; set; }


        /// <summary>
        /// 谷歌验证码
        /// </summary>
        [Column("SecretKey")]
        public Guid SecretKey { get; set; }

        #endregion


#region  ========  扩展方法  ========

        public enum AdminStatus : byte
        {
            [Description("正常")]
            Normal = 0,
            [Description("停止")]
            Stop = 1,
            [Description("删除")]
            Deleted = 2
        }
        #endregion

    }

}
