using BW.Common.Games;
using BW.Common.Sites;
using BW.Common.Systems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common
{
    /// <summary>
    /// 游戏设置
    /// </summary>
    public partial class BizDataContext
    {
        /// <summary>
        /// 游戏设置
        /// </summary>
        public DbSet<GameSetting> GameSetting { get; set; }
    }
}
