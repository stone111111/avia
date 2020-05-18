using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Procedure
{
    /// <summary>
    /// 初始化系统模板所选择的视图模型
    /// </summary>
    public class view_InitSystemTtemplate : IProcedureModel
    {
        public view_InitSystemTtemplate(int templateId)
        {
            this.TemplateID = templateId;
        }

        /// <summary>
        /// 要运行的系统模板编号
        /// </summary>
        public int TemplateID { get; set; }
    }
}
