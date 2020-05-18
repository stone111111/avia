using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Model;
using SP.StudioCore.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SP.Provider.CDN
{
    /// <summary>
    /// CDN供应商
    /// </summary>
    public abstract class ICDNProvider : ISetting
    {
        public ICDNProvider()
        {
        }

        public ICDNProvider(string queryString) : base(queryString)
        {
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="msg">删除错误的信息</param>
        /// <returns></returns>
        public abstract bool Delete(string recordId,out string msg);
    }

    /// <summary>
    /// CDN供应商枚举
    /// </summary>
    public enum CDNProviderType : byte
    {
        /// <summary>
        /// 手动管理
        /// </summary>
        [Description("手动管理")]
        Manual,
        [Description("CDNBest")]
        CDNBest,
        [Description("阿里云")]
        ALIYUN
    }

   
}
