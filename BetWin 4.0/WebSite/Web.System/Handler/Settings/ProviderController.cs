using BW.Common.Providers;
using Microsoft.AspNetCore.Mvc;
using SP.Provider.CDN;
using SP.Provider.Game;
using SP.StudioCore.Json;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.System.Agent.Systems;
using Web.System.Utils;

namespace Web.System.Handler.Settings
{
    /// <summary>
    /// 供应商管理
    /// </summary>
    [Route("Setting/[controller]/[action]")]
    public class ProviderController : SysControllerBase
    {
        /// <summary>
        /// CDN供应商列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.CDN供应商.Value)]
        public Task CDNList()
        {
            return this.GetResult(this.ShowResult(BDC.CDNProvider, t => new
            {
                t.Type,
                t.IsOpen
            }));
        }

        /// <summary>
        /// 获取CDN供应商详情
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.CDN供应商.Value)]
        public Task CDNInfo([FromForm]CDNProviderType? type)
        {
            CDNProvider provider = type == null ? new CDNProvider()
            {
                Type = CDNProviderType.Manual,
            } : (ProviderAgent.Instance().GetCDNProviderInfo(type.Value) ?? new CDNProvider()
            {
                Type = type.Value
            });
            return this.GetResult(new
            {
                provider.Type,
                provider.IsOpen
            });
        }

        /// <summary>
        /// 获取供应商的配置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.CDN供应商.Value)]
        public Task GetCDNSetting([FromForm]CDNProviderType type)
        {
            CDNProvider provider = ProviderAgent.Instance().GetCDNProviderInfo(type);
            ICDNProvider cdn = provider?.Setting;
            if (cdn == null)
            {
                return this.GetResult(true);
            }
            else
            {
                return this.GetResult(cdn.ToSetting());
            }
        }

        /// <summary>
        /// 保存CDN的设置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isOpen"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.CDN供应商.Value)]
        public Task SaveCDNInfo([FromForm]CDNProviderType type, [FromForm]int? isOpen, [FromForm]string setting)
        {
            CDNProvider provider = new CDNProvider()
            {
                Type = type,
                SettingString = setting,
                IsOpen = isOpen == null ? false : isOpen == 1
            };
            return this.GetResult(ProviderAgent.Instance().SaveCDNProvider(provider));
        }

        #region ========  游戏供应商  ========

        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.游戏供应商.Value)]
        public Task SaveGameInfo([FromForm]int id,[FromForm]GameProviderType type,[FromForm]string name,[FromForm]string setting)
        {
            if (string.IsNullOrWhiteSpace(name)) return this.ShowError("请输入供应商名称");
            if (type == 0) return this.ShowError("请选择类型");

            GameProvider provider = new GameProvider()
            {
                ID = id,
                Name = name,
                Type =  type,
                SettingString = setting
            };
            return this.GetResult(ProviderAgent.Instance().SaveGameProvider(provider));
        }

        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.游戏供应商.Value)]
        public Task GameList()
        {
            return this.GetResult(this.ShowResult(BDC.GameProvider, t => new
            {
                t.ID,
                t.Name,
                t.Type
            }));
        }

        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.游戏供应商.Value)]
        public Task GameInfo([FromForm]int? id)
        {
            GameProvider provider = id == null ? new GameProvider() { Name = "",SettingString = "",}
             : (ProviderAgent.Instance().GetGameProviderInfo(id.Value) ?? new GameProvider() { Name = "", SettingString = "", });
            IGameProvider iProvider = GameFactory.GetFactory(provider.Type, provider.SettingString);

            return this.GetResult(new
            {
                provider.ID,
                provider.Name,
                provider.Type,
                Setting = provider.ID == 0 ? new JsonString("[]"): new JsonString(iProvider.ToSetting())
            });
        }

        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.游戏供应商.Value)]
        public Task GameGetSetting([FromForm]GameProviderType type)
        {
            IGameProvider iProvider = GameFactory.GetFactory(type, string.Empty);

            return this.GetResult(new
            {
                Setting = iProvider == null ? new JsonString("[]") : new JsonString(iProvider.ToSetting())
            });
        }

        [HttpPost, Permission(BW.Permission.系统运维.供应商管理.游戏供应商.Value)]
        public Task GameDelete([FromForm]int id)
        {
            return this.GetResult(ProviderAgent.Instance().DeleteGameProviderInfo(id));
        }

        #endregion
    }
}
