using BW.Common.Providers;
using SP.Provider.CDN;
using SP.StudioCore.Data.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Agent.Systems
{
    /// <summary>
    /// 供应商管理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IProviderAgent<T> : AgentBase<T> where T : class, new()
    {
        /// <summary>
        /// 获取当前系统中开放的CDN供应商
        /// </summary>
        /// <returns></returns>
        protected List<CDNProvider> GetCDNProviders()
        {
            // 此处表达式存在BUG，布尔型的判断一定要有等于号
            return this.ReadDB.ReadList<CDNProvider>(t => t.IsOpen == true);
        }

        /// <summary>
        /// 获取CDN 供应商信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected CDNProvider GetCDNProviderInfo(CDNProviderType type)
        {
            return this.ReadDB.ReadInfo<CDNProvider>(t => t.Type == type);
        }

        #region ========  游戏供应商管理  ========

        /// <summary>
        /// 获取所有游戏供应商
        /// </summary>
        /// <returns></returns>
        protected List<GameProvider> GetGameProviders()
        {
            // 此处表达式存在BUG，布尔型的判断一定要有等于号
            return this.ReadDB.ReadList<GameProvider>();
        }

        /// <summary>
        /// 获取游戏供应商信息
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        protected GameProvider GetGameProviderInfo(int providerId)
        {
            return this.ReadDB.ReadInfo<GameProvider>(t => t.ID == providerId);
        }


        #endregion
    }
}
