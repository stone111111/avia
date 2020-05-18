using BW.Cache.Sites;
using BW.Common.Views;
using BW.Views;
using BW.Views.IViews;
using Newtonsoft.Json;
using SP.StudioCore.Cache.Redis;
using SP.StudioCore.Enums;
using SP.StudioCore.Security;
using SP.StudioCore.Web;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BW.Cache.Systems
{
    /// <summary>
    /// 视图配置 #4
    /// </summary>
    public sealed class ViewCaching : CacheBase<ViewCaching>
    {
        protected override int DB_INDEX => 4;

        #region ========  系统视图配置信息  ========

        /// <summary>
        /// 视图的编号（Key=Type）
        /// </summary>
        private const string VIEWID = "VIEW:ID";

        /// <summary>
        /// 视图ID对应的类型（Key=ID）
        /// </summary>
        private const string VIEWCODE = "VIEW:CODE";

        /// <summary>
        /// 视图的实体类
        /// </summary>
        private const string VIEWSETTING = "VIEWSETTING:";

        /// <summary>
        /// 获取系统配置的视图类对应的配置ID（系统配置）
        /// </summary>
        /// <typeparam name="TView">具体平台的实现类</typeparam>
        /// <returns></returns>
        public int GetViewID<TView>() where TView : IViewBase, new()
        {
            return this.GetViewID(typeof(TView).FullName);
        }

        public int GetViewID(string fullname)
        {
            return this.NewExecutor().HashGet(VIEWID, fullname).GetRedisValue<int>();
        }

        /// <summary>
        /// 保存视图所关联的视图编号
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="viewId"></param>
        public void SaveViewID<TView>(int viewId) where TView : IViewBase, new()
        {
            this.SaveViewID(typeof(TView).FullName, viewId);
        }

        /// <summary>
        /// 保存全名
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="viewId"></param>
        public void SaveViewID(string fullName, int viewId)
        {
            IBatch batch = this.NewExecutor().CreateBatch();
            batch.HashSetAsync(VIEWID, fullName, viewId.GetRedisValue());
            batch.HashSetAsync(VIEWCODE, viewId.GetRedisValue(), fullName);
            batch.Execute();
        }

        /// <summary>
        /// 获取视图的类型内容（完整路径）
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public string GetViewCode(int viewId)
        {
            return this.NewExecutor().HashGet(VIEWCODE, viewId).GetRedisValue<string>();
        }

        /// <summary>
        /// 获取视图对象
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public ViewSetting GetViewSettingInfo(int viewId)
        {
            string key = $"{VIEWSETTING}{viewId}";
            return this.NewExecutor().HashGetAll(key);
        }

        /// <summary>
        /// 保存视图对象
        /// </summary>
        /// <param name="setting"></param>
        public void SaveViewSettingInfo(ViewSetting setting)
        {
            string key = $"{VIEWSETTING}{setting.ID}";
            this.NewExecutor().HashSet(key, setting);
        }

        #endregion

        #region ========  商户模板配置  ========
        /*
         * 商户模板定义：商户配置的模板中可选择对应的视图模型，拼合起来形成一个模板，为一对多关系。
         * 默认商户模板：位于Site表的 PCTemplate、H5Template、APPTemplate 字段，如果没有给当前域名分配模板的话，读取默认模板。
         * 
        */

        private const string TEMPLATE = "TEMPLATE:";

        /// <summary>
        /// 模板配置实体类
        /// </summary>
        private const string TEMPLATEINFO = "TEMPLATEINFO:";

        /// <summary>
        /// 商户模板对于视图的配置 SITECONFIG:{TemplateID}:{ViewID}
        /// </summary>
        private const string SITECONFIG = "SITECONFIG:";

        /// <summary>
        /// 保存商户的模板配置
        /// </summary>
        /// <param name="template"></param>
        public void SaveTemplateInfo(ViewSiteTemplate template)
        {
            string key = $"{TEMPLATEINFO}{template.ID}";
            this.NewExecutor().HashSet(key, template);
        }

        /// <summary>
        /// 获取商户的模板配置
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ViewSiteTemplate GetTemplateInfo(int templateId)
        {
            string key = $"{TEMPLATEINFO}{templateId}";
            return this.NewExecutor().HashGetAll(key);
        }

        /// <summary>
        /// 获取商户模板所属的平台类型
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public PlatformSource? GetSiteTemplatePlatform(int templateId)
        {
            string key = $"{TEMPLATEINFO}{templateId}";
            RedisValue value = this.NewExecutor().HashGet(key, "Platform");
            if (value.IsNull) return null;
            return value.GetRedisValue<PlatformSource>();
        }

        /// <summary>
        /// 保存商户模板对视图的配置
        /// </summary>
        /// <param name="config"></param>
        public void SaveSiteConfig(ViewSiteConfig config)
        {
            string key = $"{SITECONFIG}{config.TemplateID}:{config.ViewID}";
            this.NewExecutor().HashSet(key, config);
        }

        /// <summary>
        /// 获取商户模板对于视图的配置
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public ViewSiteConfig GetSiteConfig(int templateId, int viewId)
        {
            string key = $"{SITECONFIG}{templateId}:{viewId}";
            return this.NewExecutor().HashGetAll(key);
        }

        /// <summary>
        /// 获取商户配置的视图
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public TView GetSiteView<TView>(int templateId) where TView : IViewBase
        {
            Type type = typeof(TView);
            string code = typeof(TView).FullName;
            if (type.IsAbstract)
            {
                PlatformSource? platform = this.GetSiteTemplatePlatform(templateId);
                if (platform == null) return default;
                code = ViewUtils.GetCode<TView>(platform.Value);
            }
            int viewId = this.GetViewID(code);
            if (viewId == 0) return default;

            string key = $"{SITECONFIG}{templateId}:{viewId}";
            RedisValue value = this.NewExecutor().HashGet(key, "Setting");
            if (value.IsNull) return default;
            return (TView)ViewUtils.CreateInstance(code, value.GetRedisValue<string>());
        }


        /// <summary>
        /// 给域名分配模板（取出当前的所有模板)
        /// </summary>
        public void SaveTemplateDomain(int siteId, PlatformSource platform, Dictionary<int, string> domains)
        {
            string key = $"{ TEMPLATE }:{ siteId }:{ platform }";
            this.NewExecutor().KeyDelete(key);
            IBatch batch = this.NewExecutor().CreateBatch();
            foreach (int templateId in domains.Keys)
            {
                foreach (string name in domains[templateId].Split(','))
                {
                    batch.HashSetAsync(key, name, templateId);
                }
            }
            batch.Execute();
        }

        /// <summary>
        /// 根据域名找到模板ID
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public int GetTemplateID(int siteId, PlatformSource platform, string domain)
        {
            string key = $"{ TEMPLATE }{ siteId }:{ platform }";
            int templateId = this.NewExecutor().HashGet(key, domain).GetRedisValue<int>();
            if (templateId != 0) return templateId;

            string topDomain = $"*.{ WebAgent.GetTopDomain(domain)}";
            templateId = this.NewExecutor().HashGet(key, topDomain).GetRedisValue<int>();
            if (templateId != 0) return templateId;

            switch (platform)
            {
                case PlatformSource.PC:
                    templateId = SiteCaching.Instance().GetSiteInfo(siteId, t => t.PCTemplate);
                    break;
                case PlatformSource.H5:
                    templateId = SiteCaching.Instance().GetSiteInfo(siteId, t => t.H5Template);
                    break;
                case PlatformSource.APP:
                    templateId = SiteCaching.Instance().GetSiteInfo(siteId, t => t.APPTemplate);
                    break;
            }
            return templateId;
        }

        #endregion
    }
}
