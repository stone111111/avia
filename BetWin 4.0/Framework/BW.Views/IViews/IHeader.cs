using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.IViews
{
    /// <summary>
    /// 头部
    /// </summary>
    public abstract class IHeader : IViewBase
    {
        public IHeader()
        {
        }

        public IHeader(string jsonString) : base(jsonString)
        {
        }
    }
}
