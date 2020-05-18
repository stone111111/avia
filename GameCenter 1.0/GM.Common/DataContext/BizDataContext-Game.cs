using GM.Common.Games;
using Microsoft.EntityFrameworkCore;

namespace GM.Common
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
