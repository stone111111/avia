using GM.Common.Logs;
using GM.Common.Sites;
using GM.Common.Systems;
using GM.Common.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Common
{
    /// <summary>
    /// 会员设置
    /// </summary>
    public partial class BizDataContext
    {
        /// <summary>
        /// 錯誤日志
        /// </summary>
        public DbSet<ErrorLog> ErrorLog { get; set; }
    }
}
