using BW.Views.Attributes;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BW.Views.IViews.Controls
{
    /// <summary>
    /// banner图
    /// </summary>
    [Control]
    public abstract class IBanner : IViewBase
    {
        public IBanner()
        {
        }

        public IBanner(string jsonString) : base(jsonString)
        {
        }

        /// <summary>
        /// Banner图列表
        /// </summary>
        [Description("Banner图")]
        public List<BannerItem> Items { get; set; }
    }

    /// <summary>
    /// Banner图元素
    /// </summary>
    public class BannerItem
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        [SettingCustomType(SettingCustomType.Upload), Description("图片")]
        public string Cover { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [SettingCustomType(SettingCustomType.Link), Description("链接地址")]
        public string Link { get; set; }
    }
}
