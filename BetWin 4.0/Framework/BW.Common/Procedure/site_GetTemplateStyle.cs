using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Procedure
{
    /// <summary>
    /// 获取商户模板关联的所有样式
    /// </summary>
    public class site_GetTemplateStyle : IProcedureModel
    {
        public site_GetTemplateStyle(int templateId)
        {
            this.TemplateID = templateId;
        }

        /// <summary>
        /// 商户模板编号
        /// </summary>
        public int TemplateID { get; set; }
    }
}
