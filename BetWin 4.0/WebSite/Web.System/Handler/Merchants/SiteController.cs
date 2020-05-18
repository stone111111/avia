using BW.Common.Sites;
using BW.Common.Views;
using BW.Views;
using Microsoft.AspNetCore.Mvc;
using SP.Provider.CDN;
using SP.StudioCore.Enums;
using SP.StudioCore.Http;
using SP.StudioCore.Linq;
using SP.StudioCore.Model;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.System.Agent.Sites;
using Web.System.Agent.Systems;
using Web.System.Utils;
using static BW.Common.Systems.SystemSetting;

namespace Web.System.Handler.Merchants
{
    /// <summary>
    /// 商户管理
    /// </summary>
    [Route("Merchant/[controller]/[action]")]
    public class SiteController : SysControllerBase
    {
        /// <summary>
        /// 商户列表管理
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task List([FromForm]int? siteId, [FromForm]string name, [FromForm]Site.SiteStatus Status)
        {
            IQueryable<Site> list = BDC.Site.Where(siteId, t => t.ID == siteId.Value)
                .Where(name, t => t.Name.Contains(name)).Where(Status, t => t.Status == Status);

            return this.GetResult(this.ShowResult(list.OrderByDescending(t => t.ID), t => new
            {
                t.ID,
                t.Name,
                t.Status,
                t.Language,
                t.Currency,
                t.Setting
            }));
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task AddSite([FromForm]int id, [FromForm]string name, [FromForm]Currency currency, [FromForm]Language language,
            [FromForm]int pcTemplate, [FromForm]int h5Template, [FromForm]int appTemplate)
        {
            Site site = new Site()
            {
                ID = id,
                Name = name,
                Currency = currency,
                Language = language,
                PCTemplate = pcTemplate,
                H5Template = h5Template,
                APPTemplate = appTemplate,
                Status = Site.SiteStatus.Normal
            };
            return this.GetResult(SiteAgent.Instance().AddSite(site));
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Task Info([FromForm]int id)
        {
            Site site = SiteAgent.Instance().GetSiteInfo(id);
            return this.GetResult(site);
        }

        /// <summary>
        /// 保存商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="currency"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpPost]
        public Task Save([FromForm]int id, [FromForm]string name, [FromForm]Currency currency, [FromForm]Language language)
        {
            Site site = SiteAgent.Instance().GetSiteInfo(id);
            site.Setting = this.context.Request.Form.Fill(site.Setting, "Setting.", false);
            site.Name = name;
            site.Currency = currency;
            site.Language = language;

            return this.GetResult(SiteAgent.Instance().SaveSiteInfo(site));
        }

        /// <summary>
        /// 读取商户资料
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.商户详情)]
        public Task GetSiteDetail([FromForm]int id)
        {
            SiteDetail detail = SiteAgent.Instance().GetDetailInfo(id) ?? new SiteDetail() { SiteID = id };
            return this.GetResult(new
            {
                detail.SiteID,
                detail.Mobile,
                detail.Email
            });
        }

        /// <summary>
        /// 保存资料
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.商户详情)]
        public Task SaveSiteDetail([FromForm]int id, [FromForm]string mobile, [FromForm]string email)
        {
            SiteDetail sitedetail = new SiteDetail()
            {
                SiteID = id,
                Mobile = mobile,
                Email = email
            };
            return this.GetResult(SiteAgent.Instance().SaveDetail(sitedetail));
        }


        #region ========  域名管理  ========


        /// <summary>
        /// 域名列表
        /// </summary>
        /// <returns>显示域名/对应的CDN/状态</returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.域名管理)]
        public Task DomainList([FromForm]int id)
        {
            List<SiteDomain> domains = SiteAgent.Instance().GetDomainList(id);
            List<DomainRecord> records = BDC.DomainRecord.Where(t => BDC.SiteDomain.Any(p => p.SiteID == id && p.ID == t.DomainID)).ToList();

            return this.GetResult(this.ShowResult(domains, t => new
            {
                t.ID,
                t.SiteID,
                t.Domain,
                Records = records.Where(p => p.DomainID == t.ID)
            }));
        }


        /// <summary>
        /// 域名记录列表
        /// </summary>
        /// <param name="id">域名ID</param>
        /// <returns>显示域名/对应的CDN/状态</returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.域名管理)]
        public Task DomainRecordList([FromForm]int id)
        {
            List<DomainRecord> records = SiteAgent.Instance().GetDomainRecordList(id);
            return this.GetResult(this.ShowResult(records, t => t));
        }

        /// <summary>
        /// 添加域名
        /// </summary>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.域名管理)]
        /// <returns></returns>
        public Task AddDomain([FromForm]int siteId, [FromForm]string domain, [FromForm]string subName, [FromForm]CDNProviderType provider)
        {
            return this.GetResult(SiteAgent.Instance().AddDomain(siteId, domain,
                (subName ?? string.Empty).Split(','), provider));
        }

        /// <summary>
        /// 删除域名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteDomain([FromForm]int id)
        {
            return this.GetResult(SiteAgent.Instance().DeleteDomain(id));
        }

        /// <summary>
        /// 手动添加记录
        /// </summary>
        /// <param name="domianId"></param>
        /// <param name="subdomain"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.域名管理)]
        public Task AddRecord([FromForm]int siteId, [FromForm]int domainId, [FromForm]string subName, [FromForm]CDNProviderType provider)
        {
            return this.GetResult(SiteAgent.Instance().AddDomainRecord(siteId, domainId, subName, provider));
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteRecord([FromForm]int id)
        {
            return this.GetResult(SiteAgent.Instance().DeleteRecord(id));
        }

        /// <summary>
        /// 获取域名记录信息
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.域名管理)]
        public Task GetRecordInfo([FromForm]int id, [FromForm]int siteId)
        {
            DomainRecord record = SiteAgent.Instance().GetRecordInfo(id);
            if (record == null) return this.ShowError("记录不存在");
            SiteDomain domain = SiteAgent.Instance().GetDomainInfo(siteId, record.DomainID);
            if (domain == null) return this.ShowError("记录数据错误");
            return this.GetResult(new
            {
                record.ID,
                record.DomainID,
                record.CDNType,
                record.CName,
                record.Status,
                record.SubName,
                domain.Domain
            });
        }

        /// <summary>
        /// 获取当前域名记录的CDN供应商信息
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.域名管理)]
        public Task GetCDNInfo([FromForm]int recordId, [FromForm]CDNProviderType type)
        {
            DomainCDN cdn = SiteAgent.Instance().GetDomainCDNInfo(recordId, type);
            if (cdn == null) return this.ShowError("没有记录");
            return this.GetResult(cdn);
        }

        /// <summary>
        /// 切换CDN供应商
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="type"></param>
        /// <param name="cname"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.域名管理)]
        public Task UpdateCDNProvider([FromForm]int recordId, [FromForm]CDNProviderType provider, [FromForm]string cname)
        {
            return this.GetResult(SiteAgent.Instance().UpdateCDNProvider(recordId, provider, cname));
        }

        /// <summary>
        /// 复制到剪切板，方便客服通知客户
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.域名管理)]
        public Task GetDomainClipboard([FromForm]int siteId, [FromForm]int domainId)
        {
            SiteDomain domain = SiteAgent.Instance().GetDomainInfo(siteId, domainId);
            if (domain == null) return this.ShowError("域名错误");
            IEnumerable<DomainRecord> records = SiteAgent.Instance().GetDomainRecordList(domainId).Where(t => t.Status == DomainRecord.RecordStatus.Finish);
            if (records.Count() == 0) return this.ShowError("没有已完成的记录");

            List<string> clipboard = new List<string>();
            foreach (DomainRecord record in records)
            {
                clipboard.Add($"域名：{record.SubName}.{domain.Domain} 指向到(别名记录) {record.CName}");
            }
            return this.GetResult(clipboard);
        }

        #endregion

        #region ========  安全管理  ========

        /// <summary>
        /// 获取白名单列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task GetWhiteIP([FromForm]int id)
        {
            SiteDetail detail = SiteAgent.Instance().GetDetailInfo(id) ?? new SiteDetail() { SiteID = id };
            return this.GetResult(new
            {
                detail.SiteID,
                detail.WhiteIP
            });
        }

        /// <summary>
        /// 保存白名单IP
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <param name="whiteIP">白名单IP列表</param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task SaveWhiteIP([FromForm]int id, [FromForm]string whiteIP)
        {
            return this.GetResult(SiteAgent.Instance().SaveWhiteIP(id, whiteIP));
        }

        /// <summary>
        /// 获取白名单列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task GetAdminUrl([FromForm]int id)
        {
            SiteDetail detail = SiteAgent.Instance().GetDetailInfo(id) ?? new SiteDetail() { SiteID = id };
            return this.GetResult(new
            {
                detail.SiteID,
                detail.AdminURL,
                Domain = (SettingAgent.Instance().GetSetting(SettingType.AdminDomain) ?? string.Empty).Split(',')
            });
        }

        /// <summary>
        /// 保存后台管理别名
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task SaveAdminUrl([FromForm]int id, [FromForm]string adminURL)
        {
            return this.GetResult(SiteAgent.Instance().SaveAdminUrl(id, adminURL));
        }

        #endregion

        #region ========  商户域名管理  ========

        /// <summary>
        /// 获取SSL证书列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task CertList([FromForm]int? siteId)
        {
            IQueryable<SiteDomainCert> list = BDC.SiteDomainCert.Where(siteId, t => t.SiteID == siteId.Value);
            return this.GetResult(this.ShowResult(list.OrderByDescending(t => t.ID), t => t));
        }

        /// <summary>
        /// 保存SSL证书信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task SaveCert([FromForm]int id, [FromForm]int siteId, [FromForm]string pem, [FromForm]string key)
        {
            SiteDomainCert cert = new SiteDomainCert()
            {
                ID = id,
                SiteID = siteId,
                KEY = key,
                PEM = pem
            };
            return this.GetResult(CertAgent.Instance().SaveCertInfo(cert));
        }

        /// <summary>
        /// 读取SSL证书内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Task CertInfo([FromForm]int id, [FromForm]int siteId)
        {
            SiteDomainCert cert = CertAgent.Instance().GetCertInfo(id) ?? new SiteDomainCert()
            {
                SiteID = siteId
            };

            return this.GetResult(cert);
        }

        /// <summary>
        /// 读取SSL证书内容
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task CertView([FromForm]string content)
        {
            CertInfo result = HttpsHelper.GetFirstCertInfo(content);
            if (!result.Success) return this.ShowError(result.Message);
            return this.GetResult(result.ToString());
        }

        /// <summary>
        /// 删除SSL证书记录
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.Value)]
        public Task CertDelete([FromForm]int id)
        {
            return this.GetResult(CertAgent.Instance().DeleteCertInfo(id));
        }

        #endregion

        #region ========  商户管理员  ========

        /// <summary>
        /// 获取商户管理员列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.安全设置)]
        public Task GetAdminList([FromForm]int siteId)
        {
            IEnumerable<SiteAdmin> list = SiteAdminAgent.Instance().GetAdminList(siteId);
            return this.GetResult(this.ShowResult(list, t => new
            {
                t.ID,
                t.SiteID,
                t.AdminName,
                t.IsDefault,
                t.Status,
                t.LoginAt,
                t.LoginIP,
                IsSecretKey = t.SecretKey != Guid.Empty,
                IPAddress = IPAgent.GetAddress(t.LoginIP),
                t.CreateAt
            }));
        }


        /// <summary>
        /// 重置商户管理员密码
        /// </summary>
        /// <param name="id">管理员ID</param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.安全设置)]
        public Task ResetPassword([FromForm]int id, [FromForm]int siteId)
        {
            return this.GetResult(SiteAdminAgent.Instance().ResetAdminPassword(id, siteId, out string newPassword), newPassword);
        }

        /// <summary>
        /// 重置商户管理员验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.安全设置)]
        public Task ResetSecretKey([FromForm]int id, [FromForm]int siteId)
        {
            return this.GetResult(SiteAdminAgent.Instance().ResetAdminSecretKey(id, siteId));
        }


        #endregion

        #region ========  商户模板配置  ========

        /// <summary>
        /// 商户模板配置
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task TemplateList([FromForm]int siteId, [FromForm]PlatformSource? platform)
        {
            var list = BDC.ViewSiteTemplate.Where(t => t.SiteID == siteId)
                .Where(platform, t => t.Platform == platform.Value);
            Site site = SiteAgent.Instance().GetSiteInfo(siteId);
            return this.GetResult(this.ShowResult(list, t => new
            {
                t.ID,
                t.Name,
                t.Platform,
                t.Domain,
                IsDefault = site.GetTemplateID(t.Platform) == t.ID
            }));
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="name">模板名称</param>
        /// <param name="source">所属平台</param>
        /// <param name="isDefault">是否为默认模板</param>
        /// <param name="domain">适配的域名</param>
        /// <param name="templateId">从系统模板中复制</param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task AddTemplate([FromForm]string name, [FromForm]int siteId, [FromForm]PlatformSource source, [FromForm]int isDefault, [FromForm]string domain, [FromForm]int templateId)
        {
            if (templateId == 0) return this.ShowError("请选择来源模板");

            ViewSiteTemplate template = new ViewSiteTemplate()
            {
                SiteID = siteId,
                Name = name,
                Platform = source,
                Domain = domain
            };

            return this.GetResult(SiteAgent.Instance().AddTemplate(template, isDefault == 1, templateId));
        }

        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task GetTemplate([FromForm]int templateId)
        {
            ViewSiteTemplate template = SiteAgent.Instance().GetTemplateInfo(templateId);
            Site site = SiteAgent.Instance().GetSiteInfo(template.SiteID);
            return this.GetResult(new
            {
                template.ID,
                template.SiteID,
                template.Name,
                template.Platform,
                template.Domain,
                IsDefault = site.GetTemplateID(template.Platform) == templateId,
                Models = template.Configs.ToDictionary(t => t.ViewID, t => new { t.ModelID, t.ID })
            });
        }

        #endregion
    }
}
