using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.API.Agent.Translates
{
    /// <summary>
    /// 管理员账号
    /// </summary>
    [Table("tran_Admin")]
    public class TranslateAdmin
    {
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Column("AdminID"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column("AdminName")]
        public string AdminName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column("Password")]
        public string Password { get; set; }

        /// <summary>
        /// 有权限编辑的语言，逗号隔开，为空表示全部语言
        /// </summary>
        [Column("Language")]
        public string Language { get; set; }
    }
}
