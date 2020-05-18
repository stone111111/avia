using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BW.Common.Users
{
    /// <summary>
    /// 用户的层级
    /// </summary>
    [Table("usr_Depth")]
    public partial class UserDepth
    {

        #region  ========  数据库字段  ========

        [Column("DepthID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }


        /// <summary>
        /// 上级
        /// </summary>
        [Column("UserID")]
        public int UserID { get; set; }


        /// <summary>
        /// 下级
        /// </summary>
        [Column("ChildID")]
        public int ChildID { get; set; }


        /// <summary>
        /// 所属商户
        /// </summary>
        [Column("SiteID")]
        public int SiteID { get; set; }


        /// <summary>
        /// 当前关系的层数
        /// </summary>
        [Column("Depth")]
        public int Depth { get; set; }

        #endregion


        #region  ========  扩展方法  ========


        #endregion

    }

}
