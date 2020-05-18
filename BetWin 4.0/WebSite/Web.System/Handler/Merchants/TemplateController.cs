using BW.Common.Views;
using BW.Views;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// <summary>
        /// 商户的视图参数配置
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        [HttpPost]
        public Task GetViewSetting([FromForm]int viewId)
        {
            //#1 获取视图对象
            ViewSetting setting = ViewAgent.Instance().GetViewSetting(viewId);
            IViewBase view = ViewUtils.CreateInstance(setting.Code, string.Empty);

            return this.GetResult(view.ToJsonString());
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Task GetSiteConfig([FromForm]int id)
        {
            ViewSiteConfig config = ViewAgent.Instance().GetSiteViewConfig(id);
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
        [HttpPost]
        public Task SaveSiteConfig([FromForm]int id, [FromForm]string setting)
        {
            return this.GetResult(ViewAgent.Instance().SaveSiteViewConfig(id, setting));
        }
    }
}
