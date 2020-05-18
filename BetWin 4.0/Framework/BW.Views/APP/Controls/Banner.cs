using BW.Views.Attributes;
using BW.Views.IViews.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.APP.Controls
{
    [Control]
    public sealed class Banner : IBanner
    {
        public Banner()
        {
        }

        public Banner(string jsonString) : base(jsonString)
        {
        }
    }
}
