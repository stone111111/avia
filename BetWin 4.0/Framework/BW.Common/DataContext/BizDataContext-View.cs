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
        /// 全局的视图配置
        /// </summary>
        public DbSet<ViewSetting> ViewSetting { get; set; }

        /// <summary>
        /// 视图模型
        /// </summary>
        public DbSet<ViewModel> ViewModel { get; set; }

        /// <summary>
        /// 视图模型的内容
        /// </summary>
        public DbSet<ViewContent> ViewContent { get; set; }

        /// <summary>
        /// 系统模板
        /// </summary>
        public DbSet<ViewTemplate> ViewTemplate { get; set; }

        /// <summary>
        /// 商户的模板设置
        /// </summary>
        public DbSet<ViewSiteTemplate> ViewSiteTemplate { get; set; }

        /// <summary>
        /// 商户的视图配置
        /// </summary>
        public DbSet<ViewSiteConfig> ViewSiteConfig { get; set; }

    }
}
