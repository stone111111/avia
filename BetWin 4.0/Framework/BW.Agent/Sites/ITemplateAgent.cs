using BW.Cache.Sites;
using BW.Cache.Systems;
using BW.Common.Sites;
using BW.Common.Views;
using BW.Views;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Linq;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BW.Agent.Sites
{
    /// <summary>
    /// 商户的模板、视图管理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ITemplateAgent<T> : AgentBase<T> where T : class, new()
    {
        #region ========  视图配置  ========

        /// <summary>
        /// 获取商户对于视图的配置（数据库读取)
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        protected ViewSiteConfig GetSiteViewConfig(int configId)
        {
            return this.ReadDB.ReadInfo<ViewSiteConfig>(t => t.ID == configId);
        }

        /// <summary>
        /// 保存商户对于视图的配置
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        protected bool SaveSiteViewConfig(int configId, string setting, out ViewSiteConfig config, out ViewSetting view)
        {
            config = this.GetSiteViewConfig(configId);
            if (config == null)
            {
                view = null;
                return this.FaildMessage("配置项目错误");
            }
            // 验证setting内容是否正确
            int viewId = config.ViewID;
            view = this.ReadDB.ReadInfo<ViewSetting>(t => t.ID == viewId);
            try
            {
                ViewUtils.CreateInstance(view.Code, setting);
            }
            catch (Exception ex)
            {
                return this.FaildMessage(ex.Message);
            }
            config.Setting = setting;
            this.WriteDB.Update<ViewSiteConfig, string>(t => t.Setting, setting, t => t.ID == configId);
            ViewCaching.Instance().SaveSiteConfig(config);
            return true;
        }

        #endregion

        #region ========  模板管理  ========


        /// <summary>
        /// 获取商户模板信息（包括配置信息，数据库读取）
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        protected ViewSiteTemplate GetTemplateInfo(int templateId)
        {
            ViewSiteTemplate template = this.ReadDB.ReadInfo<ViewSiteTemplate>(t => t.ID == templateId);
            template.Configs = this.ReadDB.ReadList<ViewSiteConfig>(t => t.TemplateID == templateId).ToList();
            return template;
        }

        /// <summary>
        /// 复制系统模板到商户模板中
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="systemTemplate">系统模板</param>
        /// <returns></returns>
        protected int CopySystemTemplate(int siteId, ViewTemplate systemTemplate)
        {
            if (systemTemplate == null) return 0;
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                //#1 插入商户模板
                ViewSiteTemplate siteTemplate = new ViewSiteTemplate()
                {
                    SiteID = siteId,
                    Platform = systemTemplate.Platform,
                    Name = systemTemplate.Name
                };
                siteTemplate.AddIdentity(db);

                //#2 插入所选择的视图模型
                foreach (ViewTemplateConfig config in systemTemplate.Configs)
                {
                    new ViewSiteConfig()
                    {
                        TemplateID = siteTemplate.ID,
                        SiteID = siteId,
                        ViewID = config.ViewID,
                        ModelID = config.ModelID,
                        Setting = config.Setting
                    }.Add(db);
                }

                db.Commit();

                return siteTemplate.ID;
            }
        }

        /// <summary>
        /// 添加商户模板
        /// </summary>
        /// <param name="template">模板参数</param>
        /// <param name="isDefault">是否设置成为默认模板</param>
        /// <param name="systemTemplate">系统模板</param>
        /// <returns></returns>
        protected bool AddTemplate(ViewSiteTemplate template, Site site, bool isDefault, ViewTemplate systemTemplate)
        {
            if (template.SiteID == 0 || site == null) return this.FaildMessage("商户ID错误");
            if (string.IsNullOrEmpty(template.Name)) return this.FaildMessage("模板名称错误");

            if (site.GetTemplateID(template.Platform) == 0) isDefault = true;
            if (isDefault) template.Domain = string.Empty;

            if (!string.IsNullOrEmpty(template.Domain) && !this.CheckTemplateDomain(site.ID, template.Domain)) return false;

            template.ID = this.CopySystemTemplate(template.SiteID, systemTemplate);
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
            this.WriteDB.Update(template, t => t.ID == template.ID, t => t.Name, t => t.Domain);
            return true;
        }

        /// <summary>
        /// 保存商户模板的信息和模型选择配置
        /// </summary>
        /// <param name="template"></param>
        /// <param name="isDefault"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        protected bool SaveTemplate(ViewSiteTemplate template, bool isDefault, int[] models)
        {
            ViewSiteTemplate siteTemplate = this.ReadDB.ReadInfo<ViewSiteTemplate>(t => t.ID == template.ID && t.SiteID == template.SiteID);
            if (siteTemplate == null) return this.FaildMessage("参数错误");

            if (string.IsNullOrEmpty(template.Name)) return this.FaildMessage("请输入模板名");
            if (isDefault) template.Domain = string.Empty;
            if (!string.IsNullOrEmpty(template.Domain) && !this.CheckTemplateDomain(template.SiteID, template.Domain)) return false;

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                template.Update(db, t => t.Name, t => t.Domain);
                if (isDefault)
                {
                    Expression<Func<Site, int>> field = siteTemplate.Platform switch
                    {
                        PlatformSource.PC => t => t.PCTemplate,
                        PlatformSource.H5 => t => t.H5Template,
                        PlatformSource.APP => t => t.APPTemplate,
                        _ => null
                    };
                    db.Update(field, template.ID, t => t.ID == template.SiteID);
                    db.AddCallback(() =>
                    {
                        SiteCaching.Instance().RemoveSiteInfo(template.SiteID);
                    });
                }
                foreach (int modelId in models)
                {
                    int viewId = db.ExecuteScalar<ViewModel, int>(t => t.ViewID, t => t.ID == modelId);
                    if (db.Exists<ViewSiteConfig>(t => t.TemplateID == template.ID && t.ViewID == viewId))
                    {
                        db.Update<ViewSiteConfig, int>(t => t.ModelID, modelId, t => t.TemplateID == template.ID && t.SiteID == template.SiteID && t.ViewID == viewId && t.ModelID != modelId);
                    }
                    else
                    {
                        new ViewSiteConfig()
                        {
                            SiteID = template.SiteID,
                            TemplateID = template.ID,
                            Setting = string.Empty,
                            ViewID = viewId,
                            ModelID = modelId
                        }.Add(db);
                    }
                }
                db.Commit();
            }
            return true;
        }

        /// <summary>
        /// 删除商户模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        protected bool DeleteTemplate(Site site, int templateId, out ViewSiteTemplate template)
        {
            template = this.GetTemplateInfo(templateId);
            if (template == null) return this.FaildMessage("模板ID错误");
            if (site.GetTemplateID(template.Platform) == template.ID) return this.FaildMessage("不可删除默认模板");
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                template.Configs.ForEach(config =>
                {
                    config.Delete(db);
                });
                template.Delete(db);

                db.AddCallback(() =>
                {
                    ViewCaching.Instance().DeleteTemplateInfo(templateId);
                });

                db.Commit();
            }
            return true;
        }


        /// <summary>
        /// 获取当前系统支持的模板（根据平台类型筛选）
        /// 数据库读取
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public List<ViewSiteTemplate> GetSiteTemplateList(int siteId, PlatformSource? platform)
        {
            return BDC.ViewSiteTemplate.Where(t => t.SiteID == siteId)
                .Where(platform, t => t.Platform == platform.Value).ToList();
        }

        /// <summary>
        /// 检查模板的域名是否合法以及是否在已登记的域名列表中
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="domainlist"></param>
        /// <returns></returns>
        private bool CheckTemplateDomain(int siteId, string domainlist)
        {
            IEnumerable<string> domains = this.ReadDB.ReadList<SiteDomain, string>(t => t.Domain, t => t.SiteID == siteId);
            foreach (string domain in domainlist.Split(','))
            {
                string topDomain = WebAgent.GetTopDomain(domain);
                if (!domains.Contains(topDomain)) return this.FaildMessage($"域名{domain}未登记");
            }
            return true;
        }

        #endregion

    }
}
