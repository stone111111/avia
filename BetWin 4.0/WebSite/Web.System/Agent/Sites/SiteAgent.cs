using BW.Agent.Sites;
using BW.Agent.Systems;
using BW.Cache.Systems;
using BW.Common.Sites;
using BW.Common.Systems;
using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Data;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Security;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Web.System.Properties;
using System.Data;
using SP.StudioCore.Data.Extension;
using BW.Common.Providers;
using SP.StudioCore.Text;
using static BW.Common.Systems.SystemAdminLog;
using BW.Cache.Sites;
using System.Text.RegularExpressions;
using SP.Provider.CDN;
using static BW.Common.Systems.SystemSetting;
using SP.StudioCore.Enums;
using BW.Common.Views;
using BW.Views;
using System.Linq.Expressions;

namespace Web.System.Agent.Systems
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    public sealed class SiteAgent : ISiteAgent<SiteAgent>
    {
        #region ========  商户资料  ========

        /// <summary>
        /// 新建站点
        /// </summary>
        public bool AddSite(Site site)
        {
            if (site.ID <= 0 || this.ReadDB.Exists<Site>(t => t.ID == site.ID)) return this.FaildMessage("编号已经存在");
            if (string.IsNullOrEmpty(site.Name)) return this.FaildMessage("请输入商户名");
            if (this.ReadDB.Exists<Site>(t => t.Name == site.Name)) return this.FaildMessage("商户名重复");

            site.Setting = new Site.SiteSetting()
            {
                Currencies = new[] { site.Currency },
                Languages = new[] { site.Language }
            };

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                site.Add(db);

                //　复制ＰＣ模板到商户模板
                if (site.PCTemplate != 0)
                {
                    db.AddCallback(() =>
                    {
                        site.PCTemplate = ViewAgent.Instance().CopySystemTemplate(site.ID, site.PCTemplate);
                    });
                }

                // 复制H5模板到商户模板
                if (site.H5Template != 0)
                {
                    db.AddCallback(() =>
                    {
                        site.H5Template = ViewAgent.Instance().CopySystemTemplate(site.ID, site.H5Template);
                    });
                }

                // 复制APP模板到商户模板
                if (site.APPTemplate != 0)
                {
                    db.AddCallback(() =>
                    {
                        site.APPTemplate = ViewAgent.Instance().CopySystemTemplate(site.ID, site.APPTemplate);
                    });
                }

                new SiteDetail()
                {
                    SiteID = site.ID,
                    AdminURL = this.CreateDefaultAdminUrl(db, site.Name)
                }.Add(db);

                this.CreateDefaultAdmin(db, site.ID);

                db.Commit();
            }

            site.Update(this.WriteDB, t => t.PCTemplate, t => t.H5Template, t => t.APPTemplate);

            this.AccountInfo.Log(LogType.Site, $"新建商户{site.ID}");

            return true;
        }

        /// <summary>
        /// 创建默认的商户管理员账号
        /// </summary>
        private void CreateDefaultAdmin(DbExecutor db, int siteId)
        {
            if (db.Exists<SiteAdmin>(t => t.SiteID == siteId)) return;
            SiteAdmin admin = new SiteAdmin()
            {
                SiteID = siteId,
                AdminName = "admin",
                Password = Encryption.SHA1WithMD5("admin"),
                CreateAt = DateTime.Now,
                IsDefault = true,
                Status = SiteAdmin.AdminStatus.Normal
            };
            admin.Add(db);
        }

        /// <summary>
        /// 根据用户名生成后台域名
        /// </summary>
        /// <param name="siteName"></param>
        private string CreateDefaultAdminUrl(DbExecutor db, string siteName)
        {
            string shortName = PinyinAgent.ToPinYinAbbr(siteName).ToLower();
            if (shortName.Length < 4) shortName += Guid.NewGuid().ToString("N").Substring(4 - shortName.Length);
            while (db.Exists<SiteDetail>(t => t.AdminURL == shortName))
            {
                shortName += Guid.NewGuid().ToString("N").Substring(1);
            }
            return shortName;
        }


        /// <summary>
        /// 获取商户信息（从数据库读取）
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public new Site GetSiteInfo(int siteId)
        {
            return this.ReadDB.ReadInfo<Site>(t => t.ID == siteId);
        }

        /// <summary>
        /// 保存商户信息
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public new bool SaveSiteInfo(Site site)
        {
            return base.SaveSiteInfo(site);
        }

        /// <summary>
        /// 获取商户详情资料
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public new SiteDetail GetDetailInfo(int siteId)
        {
            return base.GetDetailInfo(siteId);
        }

        /// <summary>
        /// 保存资料
        /// </summary>
        public bool SaveDetail(SiteDetail detail)
        {
            if (!string.IsNullOrEmpty(detail.Mobile) && !WebAgent.IsMobile(detail.Mobile)) return this.FaildMessage("手机号码错误");
            if (!string.IsNullOrEmpty(detail.Email) && !WebAgent.IsEMail(detail.Email)) return this.FaildMessage("邮箱错误");

            //存在就修改，否则就插入
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (detail.Exists(db))
                {
                    detail.Update(db, t => t.Mobile, t => t.Email);
                }
                else
                {
                    detail.Add(db);
                }

                db.Commit();
            }
            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"修改商户资料");
        }

        #endregion

        #region ========  域名管理  ========

        protected override string CName => SettingAgent.Instance().GetSetting(SettingType.CNAME);

        /// <summary>
        /// 当前选择的CDN供应商是否被系统支持
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsCDNProvider(CDNProviderType type)
        {
            return this.ReadDB.Exists<CDNProvider>(t => t.Type == type && t.IsOpen == true);
        }

        /// <summary>
        /// 获取商户的域名列表
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public new List<SiteDomain> GetDomainList(int siteId)
        {
            return base.GetDomainList(siteId);
        }

        /// <summary>
        /// 获取域名下的记录
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public new List<DomainRecord> GetDomainRecordList(int domainId)
        {
            return base.GetDomainRecordList(domainId);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public bool DeleteRecord(int recordId)
        {
            DomainRecord record = this.WriteDB.ReadInfo<DomainRecord>(t => t.ID == recordId);
            if (record == null) return this.FaildMessage("记录不存在");
            SiteDomain domain = this.ReadDB.ReadInfo<SiteDomain>(t => t.ID == record.DomainID);

            List<DomainCDN> cdnlist = this.ReadDB.ReadList<DomainCDN>(t => t.RecordID == record.ID);
            // 调用CDN接口，删除CDN记录
            foreach (DomainCDN cdn in cdnlist)
            {
                if (cdn.CDNType != CDNProviderType.Manual)
                {
                    CDNProvider provider = ProviderAgent.Instance().GetCDNProviderInfo(cdn.CDNType);
                    if (provider != null || provider.Setting != null)
                    {
                        if (!provider.Setting.Delete(cdn.CName, out string msg)) return this.FaildMessage(msg);
                    }
                }
                this.WriteDB.Delete<DomainCDN>(t => t.ID == cdn.ID && t.RecordID == recordId);
            }

            // 此处调用DNSPOD接口，删除别名记录
            // do something
            this.WriteDB.Delete<DomainRecord>(t => t.ID == recordId);

            return this.AccountInfo.Log(LogType.Site, $"删除商户{domain.SiteID}域名记录{record.SubName}.{domain.Domain}");
        }

        /// <summary>
        /// 删除域名
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public bool DeleteDomain(int domainId)
        {
            SiteDomain domain = domainId == 0 ? null : this.WriteDB.ReadInfo<SiteDomain>(t => t.ID == domainId);
            if (domain == null) this.FaildMessage("该域名不存在");
            List<DomainRecord> records = this.ReadDB.ReadList<DomainRecord>(t => t.DomainID == domain.ID);

            foreach (DomainRecord record in records)
            {
                if (!this.DeleteRecord(record.ID)) return false;
            }
            this.WriteDB.Delete<SiteDomain>(t => t.ID == domainId);
            return this.AccountInfo.Log(LogType.Site, $"删除商户{domain.SiteID}域名：{domain.Domain}");
        }


        /// <summary>
        /// 添加域名解析记录
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="domainId"></param>
        /// <param name="subName"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public new bool AddDomainRecord(int siteId, int domainId, string subName, CDNProviderType provider)
        {
            if (!IsCDNProvider(provider)) return this.FaildMessage("当前不支持该供应商");
            SiteDomain domain = base.GetDomainInfo(siteId, domainId);
            return base.AddDomainRecord(siteId, domainId, subName, provider)
                && this.AccountInfo.Log(LogType.Site, $"给商户{siteId}添加域名记录{subName}.{domain.Domain}");
        }

        /// <summary>
        /// 获取域名记录信息
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public new DomainRecord GetRecordInfo(int recordId)
        {
            return base.GetRecordInfo(recordId);
        }

        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public new SiteDomain GetDomainInfo(int siteId, int domainId)
        {
            return base.GetDomainInfo(siteId, domainId);
        }

        /// <summary>
        /// 获取CDN配置信息
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DomainCDN GetDomainCDNInfo(int recordId, CDNProviderType type)
        {
            return this.ReadDB.ReadInfo<DomainCDN>(t => t.RecordID == recordId && t.CDNType == type);
        }

        /// <summary>
        /// 切换域名记录的CDN供应商
        /// </summary>
        /// <param name="recordId">域名记录</param>
        /// <param name="type">CDN供应商</param>
        /// <param name="cname">自定义的别名记录</param>
        /// <returns></returns>
        public bool UpdateCDNProvider(int recordId, CDNProviderType type, string cname)
        {
            DomainRecord record = this.GetRecordInfo(recordId);
            if (record == null) return this.FaildMessage("域名记录错误");
            if (type == CDNProviderType.Manual && string.IsNullOrEmpty(cname)) return this.FaildMessage("请手动设置CDN别名");
            if (!IsCDNProvider(type)) return this.FaildMessage("当前不支持该供应商");

            SiteDomain domain = this.ReadDB.ReadInfo<SiteDomain>(t => t.ID == record.DomainID);
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                bool isExists = db.Exists<DomainCDN>(t => t.RecordID == recordId && t.CDNType == type);
                switch (type)
                {
                    case CDNProviderType.Manual:
                        {
                            if (isExists)
                            {
                                db.Update(new DomainCDN()
                                {
                                    Status = DomainCDN.CDNStatus.Finish,
                                    CName = cname
                                }, t => t.RecordID == recordId && t.CDNType == type, t => t.Status, t => t.CName);
                            }
                            else
                            {
                                new DomainCDN()
                                {
                                    RecordID = recordId,
                                    CName = cname,
                                    Status = DomainCDN.CDNStatus.Finish,
                                    CDNType = type
                                }.Add(db);
                            }

                            record.Status = DomainRecord.RecordStatus.Finish;

                            db.AddCallback(() =>
                            {
                                // 调用DNS供应商的接口，设定记录的别名指向到此处手动设定的CDN别名地址
                            });
                        }
                        break;
                    default:
                        {
                            if (isExists)
                            {
                                db.Update(new DomainCDN()
                                {
                                    Status = DomainCDN.CDNStatus.Wait
                                }, t => t.RecordID == recordId && t.CDNType == type, t => t.Status);
                            }
                            else
                            {
                                new DomainCDN()
                                {
                                    RecordID = recordId,
                                    Status = DomainCDN.CDNStatus.Wait,
                                    CDNType = type
                                }.Add(db);
                            }
                            record.Status = DomainRecord.RecordStatus.Wait;
                        }
                        break;
                }

                record.CDNType = type;
                record.Update(db, t => t.Status, t => t.CDNType);

                db.Commit();
            }
            return this.AccountInfo.Log(LogType.Site, $"设定域名{record.SubName}.{domain.Domain}的CDN供应商为:{type.GetDescription()}");
        }

        /// <summary>
        /// 添加域名
        /// </summary>
        /// <returns></returns>
        public new bool AddDomain(int siteId, string domain, string[] subName, CDNProviderType provider)
        {
            if (!this.IsCDNProvider(provider)) return this.FaildMessage("当前不支持该CDN供应商");
            return base.AddDomain(siteId, domain, subName, provider)
                && this.AccountInfo.Log(LogType.Site, $"给商户{siteId}添加域名{domain}");
        }

        #endregion

        #region ========  安全管理  ========

        /// <summary>
        /// 保存白名单
        /// </summary>
        public bool SaveWhiteIP(int siteId, string whiteIP)
        {
            if (siteId == 0) return this.FaildMessage("商户ID错误");
            if (string.IsNullOrEmpty(whiteIP)) return this.FaildMessage("请输入白名单");
            IEnumerable<string> iplist = whiteIP.Split(',').Where(t => IPAgent.regex.IsMatch(t)).Select(t => t).Distinct();
            if (!iplist.Any()) return this.FaildMessage("白名单IP输入错误");
            whiteIP = string.Join(",", iplist);

            this.WriteDB.Update<SiteDetail, string>(t => t.WhiteIP, whiteIP, t => t.SiteID == siteId);
            SiteCaching.Instance().SaveWhiteIP(siteId, iplist);
            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"修改商户{siteId}白名单");
        }

        /// <summary>
        /// 保存后台管理地址
        /// </summary>
        public bool SaveAdminUrl(int siteId, string adminUrl)
        {
            if (!Regex.IsMatch(adminUrl, @"^\w{4,10}$")) return this.FaildMessage("格式错误");

            this.WriteDB.Update<SiteDetail, string>(t => t.AdminURL, adminUrl, t => t.SiteID == siteId);
            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"修改商户{siteId}后台管理地址为{adminUrl}");
        }
        #endregion

        #region ========  商户模板管理  ========

        /// <summary>
        /// 添加商户模板
        /// </summary>
        /// <param name="template"></param>
        /// <param name="isDefault"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool AddTemplate(ViewSiteTemplate template, bool isDefault, int templateId)
        {
            if (template.SiteID == 0) return this.FaildMessage("商户ID错误");
            if (string.IsNullOrEmpty(template.Name)) return this.FaildMessage("模板名称错误");

            Site site = this.GetSiteInfo(template.SiteID);
            if (site.GetTemplateID(template.Platform) == 0) isDefault = true;
            if (isDefault) template.Domain = string.Empty;

            template.ID = ViewAgent.Instance().CopySystemTemplate(template.SiteID, templateId);
            if (isDefault)
            {
                Expression<Func<Site, int>> field = template.Platform switch
                {
                    PlatformSource.PC => t => t.PCTemplate,
                    PlatformSource.H5 => t => t.H5Template,
                    PlatformSource.APP => t => t.APPTemplate,
                    _ => null
                };
                this.WriteDB.Update(field, template.ID, t => t.ID == site.ID);
                SiteCaching.Instance().RemoveSiteInfo(site.ID);
            }
            template.Update(this.WriteDB, t => t.Name, t => t.Domain);
            return true;
        }

        /// <summary>
        /// 获取商户模板信息（包括配置信息）
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ViewSiteTemplate GetTemplateInfo(int templateId)
        {
            ViewSiteTemplate template = this.ReadDB.ReadInfo<ViewSiteTemplate>(t => t.ID == templateId);
            template.Configs = this.ReadDB.ReadList<ViewSiteConfig>(t => t.TemplateID == templateId);
            return template;
        }

        #endregion
    }
}
