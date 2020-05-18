using GM.Common.Sites;
using GM.Common.Systems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Common
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public partial class BizDataContext
    {

        /// <summary>
        /// 系统管理员
        /// </summary>
        public DbSet<SystemAdmin> SystemAdmin { get; set; }

    }
}
