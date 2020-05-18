using SP.StudioCore.Enums;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Procedure
{
    /// <summary>
    /// 获取商户所有的视图配置
    /// </summary>
    public sealed class site_GetViewConfig : IProcedureModel
    {

        public site_GetViewConfig(int templateId, Language language)
        {
            this.TemplateID = templateId;
            this.Language = language;
        }

        /// <summary>
        /// 模板
        /// </summary>
        public int TemplateID { get; set; }

        /// <summary>
        /// 当前语言
        /// </summary>
        public Language Language { get; set; }
    }
}
