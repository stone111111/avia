using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Procedure
{
    /// <summary>
    /// 初始化商户模板所选择的视图模型
    /// </summary>
    public class view_InitSiteTtemplate : IProcedureModel
    {
        public view_InitSiteTtemplate(int templateId)
        {
            this.TemplateID = templateId;
        }

        /// <summary>
        /// 要运行的商户模板编号
        /// </summary>
        public int TemplateID { get; set; }
    }
}
