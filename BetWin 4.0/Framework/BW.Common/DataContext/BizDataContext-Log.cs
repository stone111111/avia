using BW.Common.Logs;
using BW.Common.Sites;
using BW.Common.Systems;
using BW.Common.Users;
using BW.Common.Views;
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
        /// 錯誤日志
        /// </summary>
        public DbSet<ErrorLog> ErrorLog { get; set; }
    }
}
