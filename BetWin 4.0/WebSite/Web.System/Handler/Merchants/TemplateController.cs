using BW.Common.Sites;
using BW.Common.Views;
using BW.Views;
using Microsoft.AspNetCore.Mvc;
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

namespace Web.System.Handler.Merchants
{
    /// <summary>
    /// 商户模板配置
    /// </summary>
    [Route("Merchant/[controller]/[action]")]
    public sealed class TemplateController : SysControllerBase
    {
        #region ========  视图参数设置管理  ========

        /// <summary>
        /// 商户的视图参数配置
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task GetViewSetting([FromForm]int viewId)
        {
            //#1 获取视图对象
            ViewSetting setting = ViewAgent.Instance().GetViewSetting(viewId);
            IViewBase view = ViewUtils.CreateInstance(setting.Code, string.Empty);
            return this.GetResult(view.ToJsonString());
        }

        /// <summary>
        /// 获取商户视图的配置参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task GetSiteConfig([FromForm]int id)
        {
            ViewSiteConfig config = TemplateAgent.Instance().GetSiteViewConfig(id);
            ViewSetting setting = ViewAgent.Instance().GetViewSetting(config.ViewID);
            IViewBase view = ViewUtils.CreateInstance(setting.Code, config.Setting);
            return this.GetResult(view.ToSettingObject());
        }

        /// <summary>
        /// 保存商户对于视图的配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task SaveSiteConfig([FromForm]int id, [FromForm]string setting)
        {
            return this.GetResult(TemplateAgent.Instance().SaveSiteViewConfig(id, setting));
        }

        #endregion

        #region ========  模板管理  ========

        /// <summary>
        /// 商户模板列表
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task TemplateList([FromForm]int siteId, [FromForm]PlatformSource? platform)
        {
            var list = BDC.ViewSiteTemplate.Where(t => t.SiteID == siteId)
                .Where(platform, t => t.Platform == platform.Value);
            Site site = SiteAgent.Instance().GetSiteInfo(siteId);
            return this.GetResult(this.ShowResult(TemplateAgent.Instance().GetSiteTemplateList(siteId, platform), t => new
            {
                t.ID,
                t.Name,
                t.Platform,
                t.SiteID,
                t.Domain,
                IsDefault = site.GetTemplateID(t.Platform) == t.ID
            }));
        }

        /// <summary>
        /// 获取模板信息（包含模型配置)
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task GetTemplate([FromForm]int templateId)
        {
            ViewSiteTemplate template = TemplateAgent.Instance().GetTemplateInfo(templateId);
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
            return this.GetResult(TemplateAgent.Instance().AddTemplate(template, siteId, isDefault == 1, templateId));
        }

        /// <summary>
        /// 商户模板配置的保存
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.商户管理.商户列表.模板配置)]
        public Task SaveTemplate([FromForm]int id, [FromForm]int siteId, [FromForm]string name, [FromForm]int isDefault, [FromForm]string domain, [FromForm]string models)
        {
            ViewSiteTemplate template = new ViewSiteTemplate()
            {
                ID = id,
                SiteID = siteId,
                Name = name,
                Domain = domain
            };
            return this.GetResult(TemplateAgent.Instance().SaveTemplate(template, isDefault == 1, WebAgent.GetArray<int>(models)));
        }

        /// <summary>
        /// 删除商户模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost, Permission(BW.Permission.视图模板.模板管理.Value)]
        public Task DeleteTemplate([FromForm]int siteId, [FromForm]int id)
        {
            return this.GetResult(TemplateAgent.Instance().DeleteTemplate(siteId, id));
        }


        #endregion

    }
}
