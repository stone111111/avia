using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Common.Base
{
    /// <summary>
    /// 當前登錄的賬號（系统管理员|商户管理员|会员)
    /// </summary>
    public class IAccount
    {
        /// <summary>
        /// ID编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 對外顯示的名字（只读）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 自定义头像
        /// </summary>
        public string Face { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountType AccountType { get; set; }

        public static implicit operator int(IAccount account)
        {
            return account == null ? 0 : account.ID;
        }
    }

    public enum AccountType
    {
        /// <summary>
        /// 会员
        /// </summary>
        User,
        /// <summary>
        /// 商户管理员
        /// </summary>
        SiteAdmin,
        /// <summary>
        /// 系统管理员
        /// </summary>
        SystemAdmin
    }
}
