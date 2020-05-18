using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.Attributes
{
    /// <summary>
    /// 标识这是一个控件视图（非页面)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ControlAttribute : Attribute
    {
    }
}
