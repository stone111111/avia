using BW.Common.Sites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common
{
    public partial class BizDataContext
    {

        /// <summary>
        /// 商户资料
        /// </summary>
        public DbSet<Site> Site { get; set; }

        /// <summary>
        /// 商户域名
        /// </summary>
        public DbSet<SiteDomain> SiteDomain { get; set; }

        /// <summary>
        /// 商户资料
        /// </summary>
        public DbSet<SiteDetail> SiteDetail { get; set; }


        /// <summary>
        /// 商户域名证书
        /// </summary>
        public DbSet<SiteDomainCert> SiteDomainCert { get; set; }


        /// <summary>
        /// 域名列表
        /// </summary>
        public DbSet<DomainRecord> DomainRecord { get; set; }

        /// <summary>
        /// 域名CDN
        /// </summary>
        public DbSet<DomainCDN> DomainCDN { get; set; }

    }
}
