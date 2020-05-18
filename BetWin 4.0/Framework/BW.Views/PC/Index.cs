using BW.Views.IViews;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BW.Views.PC
{
    /// <summary>
    /// PC版本首页
    /// </summary>
    public sealed class Index : IIndex
    {
        public Index()
        {
        }

        public Index(string jsonString) : base(jsonString)
        {
        }

        /// <summary>
        /// 自定义标题
        /// </summary>
        [Description("标题")]
        public string Title { get; set; }

        [Description("关键词")]
        public string Keyword { get; set; }

        [Description("描述")]
        public string Description { get; set; }

        [Description("头部代码")]
        public string Header { get; set; }

        [Description("底部代码")]
        public string Footer { get; set; }

        /// <summary>
        /// LOGO图片
        /// </summary>
        [Description("Logo"), SettingCustomType(SettingCustomType.Upload)]
        public string Logo { get; set; }
    }
}
