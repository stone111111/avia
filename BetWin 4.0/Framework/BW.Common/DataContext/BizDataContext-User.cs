using BW.Common.Sites;
using BW.Common.Systems;
using BW.Common.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common
{
    /// <summary>
    /// 会员设置
    /// </summary>
    public partial class BizDataContext
    {

        /// <summary>
        /// 会员
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// 用户层级
        /// </summary>
        public DbSet<UserDepth> UserDepth { get; set; }

        /// <summary>
        /// 用户邀请码
        /// </summary>
        public DbSet<UserInvite> UserInvite { get; set; }

    }
}
