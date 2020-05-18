using BW.Common.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace BW.Common
{
    public partial class BizDataContext
    {


        /// <summary>
        /// 系统管理员
        /// </summary>
        public DbSet<CDNProvider> CDNProvider { get; set; }

        /// <summary>
        /// 游戏供应商
        /// </summary>
        public DbSet<GameProvider> GameProvider { get; set; }

    }
}
