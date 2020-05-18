using Newtonsoft.Json;
using SP.StudioCore.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BW.Views.IViews
{
    /// <summary>
    /// 登录
    /// </summary>
    public abstract class ILogin : IViewBase
    {
        public ILogin()
        {
        }

        public ILogin(string jsonString) : base(jsonString)
        {
        }

        /// <summary>
        /// 失败次数
        /// </summary>
        [Description("失败次数")]
        public int FaildCount { get; set; } = 5;

        /// <summary>
        /// 锁定时间
        /// </summary>
        [Description("锁定时间")]
        public int LockTime { get; set; } = 30;

        /// <summary>
        /// 开启QQ快捷登录
        /// </summary>
        [Description("QQ登录")]
        public bool QQLogin { get; set; }

        /// <summary>
        /// QQ快捷登录的参数配置
        /// </summary>
        [Description("QQ接口")]
        public string QQSetting { get; set; }

        /// <summary>
        /// 开启微信快捷登录
        /// </summary>
        [Description("微信登录")]
        public bool WXLogin { get; set; }

        /// <summary>
        /// 微信快捷登录的参数配置
        /// </summary>
        [Description("微信接口")]
        public string WXSetting { get; set; }

        /// <summary>
        /// 开启Steam快捷登录
        /// </summary>
        [Description("Steam登录")]
        public bool SteamLogin { get; set; }

        /// <summary>
        /// Steam快捷登录的参数配置
        /// </summary>
        [Description("Steam接口")]
        public string SteamSetting { get; set; }

        public override JsonString ToJsonString()
        {
            return JsonConvert.SerializeObject(new
            {
                this.FaildCount,
                this.LockTime,
                this.QQLogin,
                this.WXLogin,
                this.SteamLogin
            });
        }
    }
}
