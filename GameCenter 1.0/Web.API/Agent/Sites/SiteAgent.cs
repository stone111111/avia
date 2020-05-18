using GM.Agent.Sites;
using GM.Cache.Sites;
using GM.Common.Sites;
using SP.StudioCore.Data.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.API.Agent.Sites
{
    public class SiteAgent : ISiteAgent<SiteAgent>
    {
        public new Site GetSiteInfo(int siteId)
        {
            return base.GetSiteInfo(siteId);
        }
    }
}
