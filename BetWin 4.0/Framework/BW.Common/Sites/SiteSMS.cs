using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace BW.Common.Sites
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    [Table("site_SMS")]
    public partial class SiteSMS
    {

        #region  ========  数据库字段  ========

        /// <summary>
        /// 短信验证码
        /// </summary>
        [Column("SMSID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 所使用的渠道
        /// </summary>
        [Column("GateID")]
        public int GateID { get; set; }


        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 发送的用户ID
        /// </summary>
        [Column("UserID")]
        public int UserID { get; set; }


        /// <summary>
        /// 手机号码（包含区号）
        /// </summary>
        [Column("Mobile")]
        public string Mobile { get; set; }


        /// <summary>
        /// 短信内容
        /// </summary>
        [Column("Content")]
        public string Content { get; set; }


        /// <summary>
        /// 发送时间
        /// </summary>
        [Column("CreateAt")]
        public DateTime CreateAt { get; set; }


        /// <summary>
        /// 发送状态
        /// </summary>
        [Column("Status")]
        public SMSStatus Status { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public enum SMSStatus : byte
        {
            [Description("成功")]
            Success,
            [Description("失败")]
            Faild
        }
        #endregion

    }

}
