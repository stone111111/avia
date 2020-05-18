using GM.Cache.Sites;
using GM.Common.Sites;
using SP.StudioCore.Data.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Agent.Sites
{
    /// <summary>
    /// 商户管理得基类（总后台和商户后台都需要用到的方法）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ISiteAgent<T> : AgentBase<T> where T : class, new()
    {
        /// <summary>
        /// 获取商户信息(从缓存读取)
        /// </summary>
        /// <param name="siteid"></param>
        /// <returns></returns>
        protected virtual Site GetSiteInfo(int siteId)
        {
            Site site = SiteCaching.Instance().GetSiteInfo(siteId);
            if (site == null)
            {
                site = this.ReadDB.ReadInfo<Site>(t => t.ID == siteId);
                if (site != null) SiteCaching.Instance().SaveSiteInfo(site);
            }
            return site;
        }


        /// <summary>
        /// 保存商户基本资料(同步删除缓存)
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        protected virtual bool SaveSiteInfo(Site site)
        {
            if (string.IsNullOrEmpty(site.Name)) return this.FaildMessage("商户名错误");
            if (site.Prefix.Length != 3) return this.FaildMessage("前缀错误，只能三位！");
            if (this.ReadDB.Exists<Site>(t => t.Prefix == site.Prefix && t.ID != site.ID)) return this.FaildMessage("前缀重复");
            if (this.ReadDB.Exists<Site>(t => t.Name == site.Name && t.ID != site.ID)) return this.FaildMessage("商户名重复");

            if (this.WriteDB.Update(site, t => t.ID == site.ID, t => t.Name, t => t.Currency, t => t.Language, t => t.Prefix) == 1)
            {
                SiteCaching.Instance().RemoveSiteInfo(site.ID);
                return true;
            }
            return false;
        }
    }
}
