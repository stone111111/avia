using BW.Agent;
using BW.Agent.Sites;
using BW.Common.Procedure;
using BW.Common.Sites;
using BW.Common.Views;
using BW.Views;
using SP.StudioCore.API;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.System.Agent.Sites;
using Web.System.Agent.Systems;
using static BW.Common.Systems.SystemAdminLog;
using static BW.Common.Systems.SystemSetting;

namespace Web.System.Agent.Sites
{
    /// <summary>
    /// 管理商户的模板和视图配置（代替商户进行操作）
    /// </summary>
    public sealed class TemplateAgent : ITemplateAgent<TemplateAgent>
    {
        #region ========  视图参数管理  ========

        /// <summary>
        /// 获取商户视图的参数配置对象
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        internal new ViewSiteConfig GetSiteViewConfig(int configId)
        {
            return base.GetSiteViewConfig(configId);
        }

        /// <summary>
        /// 保存视图的配置参数
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        internal bool SaveSiteViewConfig(int configId, string setting)
        {
            return base.SaveSiteViewConfig(configId, setting, out ViewSiteConfig config, out ViewSetting view) &&
                this.AccountInfo.Log(LogType.Site, $"修改商户{config.SiteID}视图配置参数,ID={configId},View={view.Code}");
        }


        #endregion

        #region ========  模板管理  ========

        /// <summary>
        /// 复制系统模板到商户模板
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="systemTemplateId"></param>
        /// <returns></returns>
        internal int CopySystemTemplate(int siteId, int systemTemplateId)
        {
            return base.CopySystemTemplate(siteId, ViewAgent.Instance().GetTemplateInfo(systemTemplateId));
        }

        /// <summary>
        /// 获取商户模板的信息
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        internal new ViewSiteTemplate GetTemplateInfo(int templateId)
        {
            return base.GetTemplateInfo(templateId);
        }

        /// <summary>
        /// 新建一个商户模板
        /// </summary>
        /// <param name="template"></param>
        /// <param name="siteId"></param>
        /// <param name="isDefault"></param>
        /// <param name="systemTemplateId"></param>
        /// <returns></returns>
        internal bool AddTemplate(ViewSiteTemplate template, int siteId, bool isDefault, int systemTemplateId)
        {
            Site site = SiteAgent.Instance().GetSiteInfo(siteId);
            ViewTemplate systemTemplate = ViewAgent.Instance().GetTemplateInfo(systemTemplateId);
            return base.AddTemplate(template, site, isDefault, systemTemplate)
                && this.AccountInfo.Log(LogType.Site, $"添加商户模板,商户:{siteId},模板名:{template.Name}");
        }

        /// <summary>
        /// 修改商户模板的设置（包括视图模型选择）
        /// </summary>
        /// <param name="template"></param>
        /// <param name="isDefault"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        internal new bool SaveTemplate(ViewSiteTemplate template, bool isDefault, int[] models)
        {
            return base.SaveTemplate(template, isDefault, models) &&
                this.AccountInfo.Log(LogType.Site, $"修改商户{template.SiteID}的模板配置，ID:{template.ID}"); ;
        }

        /// <summary>
        /// 获取商户的所有模板
        /// </summary>
        internal new List<ViewSiteTemplate> GetSiteTemplateList(int siteId, PlatformSource? platform)
        {
            return base.GetSiteTemplateList(siteId, platform);
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        internal bool DeleteTemplate(int siteId, int templateId)
        {
            Site site = SiteAgent.Instance().GetSiteInfo(siteId);
            return
                base.DeleteTemplate(site, templateId, out ViewSiteTemplate template)
                && this.AccountInfo.Log(LogType.View, $"删除商户模板 {template.Platform}/{template.Name}");
        }


        /// <summary>
        /// 保存模板关联到的样式文件
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        internal bool SaveStyle(int templateId)
        {
            string style = string.Join("\n", this.ReadDB.ReadScalar<string, site_GetTemplateStyle>(new site_GetTemplateStyle(templateId)));
            string path = $"css/{style.GetLongHashCode()}.css";
            if (!OSSAgent.Upload(new OSSSetting(SettingType.CDNOSS.GetSetting()), path, Encoding.UTF8.GetBytes(style), null, out string message)) return this.FaildMessage(message);
            return this.WriteDB.Update<ViewSiteTemplate, string>(t => t.Style, path, t => t.ID == templateId) == 1
                && this.AccountInfo.Log(LogType.View, $"更新模板{templateId}的样式配置");
        }

        #endregion
    }
}
