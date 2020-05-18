using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BW.Views.IViews
{
    /// <summary>
    /// 注册的配置基类
    /// </summary>
    public abstract class IRegister : IViewBase
    {
        public IRegister()
        {
        }

        public IRegister(string jsonString) : base(jsonString)
        {
        }

        /// <summary>
        /// 是否需要填写手机
        /// </summary>
        [Description("手机号码")]
        public bool IsMobile { get; set; }

        /// <summary>
        /// 是否需要验证手机号码
        /// </summary>
        [Description("手机验证")]
        public bool IsMobileCode { get; set; }

        [Description("电子邮件")]
        public bool IsEmail { get; set; }

        [Description("邮箱验证")]
        public bool IsEmailCode { get; set; }

        [Description("真实姓名")]
        public bool AccountName { get; set; }

        [Description("资金密码")]
        public bool IsPayPassword { get; set; }
    }
}
