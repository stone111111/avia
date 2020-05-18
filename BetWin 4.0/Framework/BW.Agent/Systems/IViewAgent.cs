using BW.Cache.Systems;
using BW.Common.Views;
using BW.Views;
using BW.Views.IViews;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BW.Agent.Systems
{
    /// <summary>
    /// 视图管理的相关方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IViewAgent<T> : AgentBase<T> where T : class, new()
    {
        /// <summary>
        /// 获取商户的视图配置
        /// 如果商户模板中没有针对该视图的配置，则返回默认配置
        /// </summary>
        /// <typeparam name="TView">视图配置类(支持抽象类)</typeparam>
        /// <param name="siteId"></param>
        /// <returns></returns>
        protected TView GetViewConfig<TView>(int templateId) where TView : IViewBase
        {
            TView view = ViewCaching.Instance().GetSiteView<TView>(templateId);
            if (view != null) return view;

            ViewSiteTemplate template = this.GetSiteTemplate(templateId);
            if (template == null) return default;

            //# 获取视图ID
            int viewId = this.GetViewID<TView>(template.Platform);
            if (viewId == 0) return default;

            //# 从数据库中读取商户对于该视图的配置
            ViewSiteConfig siteConfig = this.ReadDB.ReadInfo<ViewSiteConfig>(t => t.ViewID == viewId && t.TemplateID == templateId);

            if (siteConfig != null)
            {
                ViewCaching.Instance().SaveSiteConfig(siteConfig);
                return ViewUtils.CreateInstance<TView>(template.Platform, siteConfig.Setting);
            }

            //# 商户模板中没有针对该视图的配置，返回默认配置
            return ViewUtils.CreateInstance<TView>(template.Platform);
        }

        /// <summary>
        /// 获取商户的视图模板（Redis缓存中读取）
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        protected ViewSiteTemplate GetSiteTemplate(int templateId)
        {
            ViewSiteTemplate template = ViewCaching.Instance().GetTemplateInfo(templateId);
            if (template == null)
            {
                template = this.ReadDB.ReadInfo<ViewSiteTemplate>(t => t.ID == templateId);
                if (template == null) return null;
                ViewCaching.Instance().SaveTemplateInfo(template);
            }
            return template;
        }

        /// <summary>
        /// 根据全名路径获取视图的ViewID
        /// </summary>
        /// <param name="fullName">视图配置类的全名路径</param>
        /// <returns></returns>
        protected int GetViewID(string fullName)
        {
            int viewId = ViewCaching.Instance().GetViewID(fullName);
            if (viewId == 0)
            {
                viewId = this.ReadDB.ReadInfo<ViewSetting, int>(t => t.ID, t => t.Code == fullName);
                if (viewId != 0)
                {
                    ViewCaching.Instance().SaveViewID(fullName, viewId);
                }
            }
            return viewId;
        }

        /// <summary>
        /// 获取类型对应的ViewID
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="platform"></param>
        /// <returns></returns>
        protected int GetViewID<TView>(PlatformSource platform) where TView : IViewBase
        {
            string code = ViewUtils.GetCode<TView>(platform);
            int viewId = ViewCaching.Instance().GetViewID(code);
            if (viewId != 0) return viewId;
            viewId = this.ReadDB.ReadInfo<ViewSetting, int>(t => t.ID, t => t.Code == code);
            if (viewId != 0)
            {
                ViewCaching.Instance().SaveViewID(code, viewId);
            }
            return viewId;
        }

        /// <summary>
        /// 获取视图模型（数据库读取）
        /// </summary>
        /// <param name="modelId">模型ID</param>
        /// <param name="viewId">视图ID</param>
        /// <returns></returns>
        protected ViewModel GetModelInfo(int modelId)
        {
            return this.ReadDB.ReadInfo<ViewModel>(t => t.ID == modelId);
        }

        /// <summary>
        /// 获取视图信息
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        protected ViewSetting GetSettingInfo(int viewId)
        {
            ViewSetting setting = ViewCaching.Instance().GetViewSettingInfo(viewId);
            if (setting == null)
            {
                setting = this.ReadDB.ReadInfo<ViewSetting>(t => t.ID == viewId);
                if (setting != null) ViewCaching.Instance().SaveViewSettingInfo(setting);
            }
            return setting;
        }

        #region ========  系统模板管理  ========

        /// <summary>
        /// 获取当前系统支持的模板（根据平台类型筛选）
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public IEnumerable<ViewTemplate> GetTemplateList(PlatformSource platform)
        {
            return this.ReadDB.ReadList<ViewTemplate>(t => t.Platform == platform).OrderByDescending(t => t.Sort).AsEnumerable();
        }

        /// <summary>
        /// 获取系统模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        protected ViewTemplate GetTemplateInfo(int templateId)
        {
            ViewTemplate template = this.ReadDB.ReadInfo<ViewTemplate>(t => t.ID == templateId);
            if (template == null) return null;
            template.Configs = this.ReadDB.ReadList<ViewTemplateConfig>(t => t.TemplateID == templateId).ToList();

            return template;
        }

        #endregion

        #region ========  商户模板  ========

        #endregion
    }
}
