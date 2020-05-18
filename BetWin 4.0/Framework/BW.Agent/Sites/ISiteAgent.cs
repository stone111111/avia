using BW.Agent.Systems;
using BW.Cache.Sites;
using BW.Common.Sites;
using SP.Provider.CDN;
using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BW.Agent.Sites
{
    public abstract class ISiteAgent<T> : AgentBase<T> where T : class, new()
    {
        /// <summary>
        /// 获取商户对象（Redis 中读取，不存在则从数据库读取）
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        protected Site GetSiteInfo(int siteId)
        {
            Site site = SiteCaching.Instance().GetSiteInfo(siteId);
            if (site != null) return site;
            site = this.ReadDB.ReadInfo<Site>(t => t.ID == siteId);
            if (site == null) return null;
            SiteCaching.Instance().SaveSiteInfo(site);
            return site;
        }

        /// <summary>
        /// 获取商户详细信息（数据库读取）
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        protected SiteDetail GetDetailInfo(int siteId)
        {
            return this.ReadDB.ReadInfo<SiteDetail>(t => t.SiteID == siteId);
        }

        /// <summary>
        /// 保存商户基本资料(同步删除缓存)
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        protected bool SaveSiteInfo(Site site)
        {
            if (string.IsNullOrEmpty(site.Name)) return this.FaildMessage("商户名错误");
            if (this.ReadDB.Exists<Site>(t => t.Name == site.Name && t.ID != site.ID)) return this.FaildMessage("商户名重复");
            if (site.Setting.Languages.Length == 0) return this.FaildMessage("支持语种未选择");
            if (site.Setting.Currencies.Length == 0) return this.FaildMessage("支持币种未选择");
            if (!site.Setting.Languages.Contains(site.Language)) return this.FaildMessage("主语种选择错误");
            if (!site.Setting.Currencies.Contains(site.Currency)) return this.FaildMessage("主币种选择错误");

            if (this.WriteDB.Update(site, t => t.ID == site.ID, t => t.Name, t => t.Currency, t => t.Language, t => t.SettingString) == 1)
            {
                SiteCaching.Instance().RemoveSiteInfo(site.ID);
                return true;
            }
            return false;
        }

        #region ========  商户域名管理  ========

        /// <summary>
        /// 获取商户域名
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="domainId"></param>
        /// <returns></returns>
        protected SiteDomain GetDomainInfo(int siteId, int domainId)
        {
            return this.ReadDB.ReadInfo<SiteDomain>(t => t.ID == domainId && t.SiteID == siteId);
        }

        /// <summary>
        /// 获取商户的所有域名
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        protected List<SiteDomain> GetDomainList(int siteId)
        {
            return this.ReadDB.ReadList<SiteDomain>(t => t.SiteID == siteId);
        }

        /// <summary>
        /// 获取域名下的记录值
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        protected List<DomainRecord> GetDomainRecordList(int domainId)
        {
            return this.ReadDB.ReadList<DomainRecord>(t => t.DomainID == domainId);
        }

        /// <summary>
        /// 添加域名
        /// </summary>
        /// <param name="siteId">商户ID</param>
        /// <param name="domain">根域名</param>
        /// <param name="subName">子域名</param>
        /// <param name="provider">CDN供应商（如果是商户操作，供应商为系统默认值，不可被商户自主选择）</param>
        /// <returns></returns>
        protected bool AddDomain(int siteId, string domain, string[] subName, CDNProviderType provider)
        {
            domain = WebAgent.GetTopDomain(domain);
            if (string.IsNullOrEmpty(domain)) return this.FaildMessage("域名错误");
            if (this.ReadDB.Exists<SiteDomain>(t => t.Domain == domain)) return this.FaildMessage("域名已被添加");
            foreach (string name in subName)
            {
                if (!Regex.IsMatch(name, @"^@$|^\*$|^\w+$")) return this.FaildMessage($"子域名{name}不符合规范");
            }
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadCommitted))
            {
                SiteDomain siteDomain = new SiteDomain()
                {
                    SiteID = siteId,
                    Domain = domain
                };
                siteDomain.AddIdentity(db);

                foreach (string name in subName.Distinct())
                {
                    // 添加域名记录
                    DomainRecord record = new DomainRecord()
                    {
                        CDNType = provider,
                        CName = this.CreateRecordCName(name, domain),
                        DomainID = siteDomain.ID,
                        Status = DomainRecord.RecordStatus.Wait,
                        SubName = name
                    };
                    record.AddIdentity(db);

                    // 添加CDN记录
                    DomainCDN cdn = new DomainCDN()
                    {
                        RecordID = record.ID,
                        Https = provider == CDNProviderType.Manual ? DomainCDN.CDNStatus.None : DomainCDN.CDNStatus.Wait,
                        CName = string.Empty,
                        CDNType = provider,
                        Status = provider == CDNProviderType.Manual ? DomainCDN.CDNStatus.None : DomainCDN.CDNStatus.Wait
                    };
                    cdn.Add(db);

                }
                db.Commit();
            }

            return true;
        }

        /// <summary>
        /// 添加域名记录
        /// </summary>
        /// <param name="siteId">商户ID</param>
        /// <param name="domainId">根域名ID</param>
        /// <param name="subName">子域名</param>
        /// <param name="provider">CDN供应商（如果是商户操作，供应商为系统默认值，不可被商户自主选择）</param>
        /// <returns></returns>
        protected bool AddDomainRecord(int siteId, int domainId, string subName, CDNProviderType provider)
        {
            if (string.IsNullOrEmpty(subName) || !Regex.IsMatch(subName, @"^@$|^\*$|^\w+$")) return this.FaildMessage($"子域名{subName}不符合规范");
            SiteDomain domain = this.GetDomainInfo(siteId, domainId);
            if (domain == null) return this.FaildMessage("域名ID错误");

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (db.Exists<DomainRecord>(t => t.DomainID == domainId && t.SubName == subName))
                {
                    return this.FaildMessage("子域名记录已经存在");
                }

                DomainRecord record = new DomainRecord()
                {
                    CDNType = provider,
                    CName = this.CreateRecordCName(subName, domain.Domain),
                    DomainID = domainId,
                    Status = DomainRecord.RecordStatus.Wait,
                    SubName = subName
                };
                record.AddIdentity(db);

                // 添加CDN记录
                DomainCDN cdn = new DomainCDN()
                {
                    RecordID = record.ID,
                    Https = provider == CDNProviderType.Manual ? DomainCDN.CDNStatus.None : DomainCDN.CDNStatus.Wait,
                    CName = string.Empty,
                    CDNType = provider,
                    Status = provider == CDNProviderType.Manual ? DomainCDN.CDNStatus.None : DomainCDN.CDNStatus.Wait
                };
                cdn.Add(db);

                db.Commit();
            }
            return true;
        }


        /// <summary>
        /// 域名记录的别名规则
        /// </summary>
        /// <param name="subName">子域名</param>
        /// <param name="domain">根域名</param>
        /// <returns></returns>
        private string CreateRecordCName(string subName, string domain)
        {
            switch (subName)
            {
                case "@":
                    subName = string.Empty;
                    break;
                case "*":
                    subName = "_";
                    break;
                default:
                    subName = subName.ToLower() + "_";
                    break;
            }
            return $"{subName}{domain.Replace('.', '_')}.{this.CName}";
        }

        /// <summary>
        /// CDN的商户别名
        /// </summary>
        protected abstract string CName { get; }

        /// <summary>
        /// 获取域名记录信息
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        protected DomainRecord GetRecordInfo(int recordId)
        {
            return this.ReadDB.ReadInfo<DomainRecord>(t => t.ID == recordId);
        }

        #endregion

    }
}
