using BW.Agent.Systems;
using BW.Common.Providers;
using SP.StudioCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using static BW.Common.Systems.SystemAdminLog;
using SP.Provider.CDN;

namespace Web.System.Agent.Systems
{
    /// <summary>
    /// 供应商管理
    /// </summary>
    public sealed class ProviderAgent : IProviderAgent<ProviderAgent>
    {
        #region ========  CDN供应商管理  ========

        /// <summary>
        /// 添加/修改CDN供应商
        /// </summary>
        /// <param name="cdn"></param>
        /// <returns></returns>
        public bool SaveCDNProvider(CDNProvider cdn)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (cdn.Exists(db))
                {
                    cdn.Update(db);
                }
                else
                {
                    cdn.Add(db);
                }
                db.Commit();
            }
            return this.AccountInfo.Log(LogType.Set, $"保存CDN供应商:{cdn.Type}");
        }

        /// <summary>
        /// 获取当前系统中开放的供应商
        /// </summary>
        /// <returns></returns>
        public new List<CDNProvider> GetCDNProviders()
        {
            return base.GetCDNProviders();
        }

        /// <summary>
        /// 获取cdn供应商信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public new CDNProvider GetCDNProviderInfo(CDNProviderType type)
        {
            return base.GetCDNProviderInfo(type);
        }

        #endregion

        #region ========  游戏供应商管理  ========

        /// <summary>
        /// 添加/修改游戏供应商
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool SaveGameProvider(GameProvider game)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (game.Exists(db))
                {
                    game.Update(db);
                }
                else
                {
                    game.Add(db);
                }
                db.Commit();
            }
            return this.AccountInfo.Log(LogType.Set, $"保存游戏供应商:{game.Type}");
        }

        /// <summary>
        /// 获取所有游戏供应商
        /// </summary>
        /// <returns></returns>
        public new List<GameProvider> GetGameProviders()
        {
            return base.GetGameProviders();
        }

        /// <summary>
        /// 根据游戏供应商ID获取供应商信息
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public new GameProvider GetGameProviderInfo(int providerId)
        {
            return base.GetGameProviderInfo(providerId);
        }

        /// <summary>
        /// 删除供应商记录
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public bool DeleteGameProviderInfo(int providerId)
        {
            GameProvider provider = this.GetGameProviderInfo(providerId);
            if (provider == null) return this.FaildMessage("编号错误");
            return provider.Delete(this.WriteDB) &&
                 AccountInfo.Log(LogType.Set, string.Format("删除游戏供应商{0} 名称{1}", provider.ID, provider.Name));
        }

        #endregion
    }
}
