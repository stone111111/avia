using GM.Common.Models;
using GM.Common.Users;
using Microsoft.EntityFrameworkCore;

namespace GM.Common
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

    }
}
