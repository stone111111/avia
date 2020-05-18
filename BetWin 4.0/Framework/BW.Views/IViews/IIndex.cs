using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.IViews
{
    /// <summary>
    /// 首页
    /// </summary>
    public abstract class IIndex : IViewBase
    {
        public IIndex()
        {
        }

        public IIndex(string jsonString) : base(jsonString)
        {
        }
    }
}
