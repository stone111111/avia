using GM.Common.Games;
using GM.Common.Models;
using GM.Common.Sites;
using Microsoft.EntityFrameworkCore;

namespace GM.Common
{
    public partial class BizDataContext
    {

        /// <summary>
        /// 商户资料
        /// </summary>
        public DbSet<Site> Site { get; set; }

        /// <summary>
        /// 商户游戏资料
        /// </summary>
        public DbSet<SiteGameSetting> SiteGameSetting { get; set; }
        public DbSet<CreditLog> CreditLog { get; set; }
        public DbSet<UserTransfer> UserTransfer { get; set; }

        public DbSet<GameOrder> GameOrder { get; set; }
    }
}
