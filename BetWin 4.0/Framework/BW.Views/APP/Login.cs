﻿using BW.Views.IViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views.APP
{
    /// <summary>
    /// 原生APP端的登录配置
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
