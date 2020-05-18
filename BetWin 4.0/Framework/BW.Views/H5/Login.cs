using BW.Views.IViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.H5
{
    /// <summary>
    /// H5端配置
    /// </summary>
    public class Login : ILogin
    {
        public Login()
        {
        }

        public Login(string jsonString) : base(jsonString)
        {
        }
    }
}
