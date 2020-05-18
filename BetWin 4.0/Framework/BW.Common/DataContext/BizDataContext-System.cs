using BW.Common.Sites;
using BW.Common.Systems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public partial class BizDataContext
    {

        /// <summary>
        /// 系统参数配置
        /// </summary>
        public DbSet<SystemSetting> SystemSetting { get; set; }

        /// <summary>
        /// 系统管理员
        /// </summary>
        public DbSet<SystemAdmin> SystemAdmin { get; set; }

    }
}
