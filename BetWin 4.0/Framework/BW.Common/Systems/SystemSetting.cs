using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace BW.Common.Systems
{
    /// <summary>
    /// 全局的参数设定
    /// </summary>
    [Table("sys_Setting")]
    public partial class SystemSetting
    {

        #region  ========  数据库字段  ========

        /// <summary>
        /// 设置类型
        /// </summary>
        [Column("Type"), Key]
        public SettingType Type { get; set; }


        [Column("Value")]
        public string Value { get; set; }

        #endregion


        #region  ========  扩展方法  ========

        public enum SettingType : byte
        {
            /// <summary>
            /// API域名地址（用于动态API请求） 为固定的地址
            /// </summary>
            [Description("动态CDN")]
            APIUrl,
            /// <summary>
            /// 静态CDN域名
            /// </summary>
            [Description("静态CDN")]
            CDNUrl,
            /// <summary>
            /// 放置于OSS上的静态文件的参数配置
            /// </summary>
            [Description("OSS配置")]
            CDNOSS,
            /// <summary>
            /// 所支持的语种，逗号隔开
            /// </summary>
            [Description("支持语种")]
            Language,
            /// <summary>
            /// 语言包的API接口
            /// </summary>
            [Description("语言API")]
            Translate,
            /// <summary>
            /// 后台域名
            /// </summary>
            [Description("后台域名")]
            AdminDomain,
            /// <summary>
            /// 供商户进行别名指向的地址
            /// </summary>
            [Description("商户别名")]
            CNAME,
            /// <summary>
            /// Studio框架的地址
            /// </summary>
            [Description("Studio网址")]
            Studio,
            /// <summary>
            /// CDN的回源地址,多个地址用逗号隔开
            /// </summary>
            [Description("CDN回源")]
            CDNHost
        }
        #endregion

    }

}
