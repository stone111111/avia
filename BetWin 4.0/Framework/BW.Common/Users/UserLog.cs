using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SP.StudioCore.Enums;
using System.ComponentModel;

namespace BW.Common.Users
{
    /// <summary>
    /// 用户操作日志
    /// </summary>
    [Table("usr_Log")]
    public partial class UserLog
    {

        #region  ========  数据库字段  ========

        [Column("LogID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        [Column("SiteID")]
        public int SiteID { get; set; }


        [Column("UserID")]
        public int UserID { get; set; }


        /// <summary>
        /// 操作类型
        /// </summary>
        [Column("Type")]
        public LogType Type { get; set; }


        [Column("IP")]
        public string IP { get; set; }


        /// <summary>
        /// 操作设备
        /// </summary>
        [Column("Platform")]
        public PlatformType Platform { get; set; }


        /// <summary>
        /// 设备编号
        /// </summary>
        [Column("IMEI")]
        public string IMEI { get; set; }


        /// <summary>
        /// 操作时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 操作内容
        /// </summary>
        [Column("Content")]
        public string Content { get; set; }


        /// <summary>
        /// 管理员操作的会员信息
        /// </summary>
        [Column("AdminID")]
        public int AdminID { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public enum LogType : byte
        {
            [Description("登录成功")]
            LoginSuccess,
            [Description("登录失败")]
            LoginFaild
        }

        #endregion

    }

}
