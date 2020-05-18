using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;
using GM.Common.Base;

namespace GM.Common.Systems
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    [Table("sys_Admin")]
    public partial class SystemAdmin
    {

        #region  ========  構造函數  ========
        public SystemAdmin() { }

        public SystemAdmin(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "AdminID":
                        this.ID = (int)reader[i];
                        break;
                    case "UserName":
                        this.UserName = (string)reader[i];
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
                    case "LoginAt":
                        this.LoginAt = (DateTime)reader[i];
                        break;
                    case "LoginIP":
                        this.LoginIP = (string)reader[i];
                        break;
                    case "Status":
                        this.Status = (AdminStatus)reader[i];
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


        public SystemAdmin(DataRow dr)
        {
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                switch (dr.Table.Columns[i].ColumnName)
                {
                    case "AdminID":
                        this.ID = (int)dr[i];
                        break;
                    case "UserName":
                        this.UserName = (string)dr[i];
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
                    case "LoginAt":
                        this.LoginAt = (DateTime)dr[i];
                        break;
                    case "LoginIP":
                        this.LoginIP = (string)dr[i];
                        break;
                    case "Status":
                        this.Status = (AdminStatus)dr[i];
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

        [Column("AdminID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        [Column("UserName")]
        public string UserName { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        [Column("Password")]
        public string Password { get; set; }


        /// <summary>
        /// 昵称（用于对外发布信息需要展示的名字）
        /// </summary>
        [Column("NickName")]
        public string NickName { get; set; }


        /// <summary>
        /// 自定义的头像
        /// </summary>
        [Column("Face")]
        public string Face { get; set; }


        /// <summary>
        /// 上次登录时间
        /// </summary>
        [Column("LoginAt")]
        public DateTime LoginAt { get; set; }


        /// <summary>
        /// 上次登录IP
        /// </summary>
        [Column("LoginIP")]
        public string LoginIP { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [Column("Status")]
        public AdminStatus Status { get; set; }


        /// <summary>
        /// 权限
        /// </summary>
        [Column("Permission")]
        public string Permission { get; set; }


        /// <summary>
        /// 密钥
        /// </summary>
        [Column("SecretKey")]
        public Guid SecretKey { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public enum AdminStatus : byte
        {
            Normal,
            Lock,
            Delete
        }

        /// <summary>
        /// 对外显示的名字
        /// </summary>
        public string Name
        {
            get
            {
                return string.IsNullOrEmpty(this.NickName) ? this.UserName : this.NickName;
            }
        }

        public AccountType AccountType => AccountType.SystemAdmin;
        public static implicit operator IAccount(SystemAdmin admin)
        {
            return new IAccount()
            {
                ID = admin.ID,
                Name = admin.Name,
                Face = admin.Face,
                AccountType = AccountType.SystemAdmin
            };
        }        #endregion

    }

}
