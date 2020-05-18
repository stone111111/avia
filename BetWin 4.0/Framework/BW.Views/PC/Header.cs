using BW.Views.IViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.PC
{
    /// <summary>
    /// 头部配置
    /// 需配置头部相关的菜单链接
    /// </summary>
    public sealed class Header : IHeader
    {
        public Header()
        {
        }

        public Header(string jsonString) : base(jsonString)
        {
        }
    }
}
