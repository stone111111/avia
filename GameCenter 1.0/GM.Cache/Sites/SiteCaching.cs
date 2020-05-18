using GM.Common.Models;
using GM.Common.Sites;
using SP.StudioCore.Cache.Redis;
using SP.StudioCore.Security;
using SP.StudioCore.Types;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GM.Cache.Sites
{
    /// <summary>
    /// 商户缓存 #1
    /// </summary>
    public sealed class SiteCaching : CacheBase<SiteCaching>
    {
        protected override int DB_INDEX => 1;


        #region ========  商户资料  ========

        private const string SITEINFO = "SITEINFO:";

        /// <summary>
        /// 获取商户资料
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public Site GetSiteInfo(int siteId)
        {
            if (siteId == 0) return null;
            string key = $"{SITEINFO}{siteId}";
            return this.NewExecutor().HashGetAll(key);
        }

        /// <summary>
        /// 获取单项信息
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="siteId"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public TValue GetSiteInfo<TValue>(int siteId, Expression<Func<Site, TValue>> field)
        {
            string name = field.GetName();
            string key = $"{SITEINFO}{siteId}";
            return this.NewExecutor().HashGet(key, name).GetRedisValue<TValue>();
        }

        /// <summary>
        /// 保存商户资料
        /// </summary>
        /// <param name="site"></param>
        public void SaveSiteInfo(Site site)
        {
            string key = $"{SITEINFO}{site.ID}";
            this.NewExecutor().HashSet(key, site);
        }

        /// <summary>
        /// 删除站点的缓存
        /// </summary>
        /// <param name="siteId"></param>
        public void RemoveSiteInfo(int siteId)
        {
            string key = $"{SITEINFO}{siteId}";
            this.NewExecutor().KeyDelete(key);
        }

        #endregion




        #region ========  商户后台  ========

        /// <summary>
        /// 后台白名单IP
        /// </summary>
        private const string WHITEIP = "WHITEIP:";

        /// <summary>
        /// 保存白名单IP
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="iplist"></param>
        public void SaveWhiteIP(int siteId, IEnumerable<string> iplist)
        {
            string key = $"{WHITEIP}{siteId}";
            IBatch batch = this.NewExecutor().CreateBatch(key);
            batch.KeyDeleteAsync(key);
            foreach (string ip in iplist)
            {
                batch.SetAddAsync(key, ip);
            }
            batch.Execute();
        }

        /// <summary>
        /// 是否在白名单IP中
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="ip"></param>
        public bool IsWhiteIP(int siteId, string ip)
        {
            string key = $"{WHITEIP}{siteId}";
            return this.NewExecutor().SetContains(key, ip);
        }

        #endregion
    }
}
