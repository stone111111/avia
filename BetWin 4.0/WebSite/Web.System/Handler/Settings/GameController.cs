using BW.Common.Games;
using Microsoft.AspNetCore.Mvc;
using SP.Provider.Game;
using SP.StudioCore.Enums;
//using SP.Game.CDN;
//using SP.Game.Game;
using SP.StudioCore.Json;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.System.Agent.Sites;
using Web.System.Agent.Systems;
using Web.System.Utils;

namespace Web.System.Handler.Settings
{
    /// <summary>
    /// 游戏配置
    /// </summary>
    [Route("Setting/[controller]/[action]")]
    public class GameController : SysControllerBase
    {
        /// <summary>
        /// 获取游戏配置列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统配置.游戏配置.Value)]
        public Task GameSettingList()
        {
            var list = BDC.GameSetting.Join(BDC.GameProvider, t => t.ProviderID, t => t.ID, (a, b) => new
            {
                a.ID,
                a.Category,
                a.IsOpen,
                a.Name,
                a.ProviderID,
                a.Sort,
                a.Type,
                ProviderName = b.Name
            });
            return this.GetResult(this.ShowResult(list, t => t));
        }

        /// <summary>
        /// 保存游戏配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="providerId"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统配置.游戏配置.Value)]
        public Task SaveSetting([FromForm]int id, [FromForm]int providerId, [FromForm]string name, [FromForm]string type,[FromForm]string category,[FromForm]bool isOpen)
        {
            if (string.IsNullOrWhiteSpace(name)) return this.ShowError("请输入游戏名称");
            if (string.IsNullOrWhiteSpace(type)) return this.ShowError("请输入游戏类型");
            if (string.IsNullOrWhiteSpace(category)) return this.ShowError("至少选择游戏分类");
            if (providerId <= 0) return this.ShowError("请选择供应商");

            //分类集合
            var cate = (GameCategory)0;
            var arrCategory = category.Split(',');
            foreach (string item in arrCategory)
            {
                cate = cate | item.ToEnum<GameCategory>();
            }

            GameSetting setting = new GameSetting()
            {
                ID = id,
                Name = name,
                Type = type,
                Category = cate,
                IsOpen = isOpen,
                ProviderID = providerId,
                Sort = 0
            };
            return this.GetResult(GameSettingAgent.Instance().SaveSetting(setting));
        }

        /// <summary>
        /// 获取游戏配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统配置.游戏配置.Value)]
        public Task GameSettingInfo([FromForm]int? id)
        {
            GameSetting setting = id == null ? new GameSetting():
                (GameSettingAgent.Instance().GetGameSettingInfo(id.Value) ?? new GameSetting());

            return this.GetResult(setting);
        }

        /// <summary>
        /// 修改开启状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统配置.游戏配置.Value)]
        public Task UpdateIsOpen([FromForm]int id, [FromForm]bool isOpen)
        {
            var setting = GameSettingAgent.Instance().GetGameSettingInfo(id);
            if (setting == null) return this.ShowError("记录不存在");
            setting.IsOpen = isOpen;

            return this.GetResult(GameSettingAgent.Instance().UpdateIsOpen(setting));
        }

        /// <summary>
        /// 删除游戏设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(BW.Permission.系统配置.游戏配置.Value)]
        public Task GameSettingDelete([FromForm]int id)
        {
            return this.GetResult(GameSettingAgent.Instance().DeleteGameSettingInfo(id));
        }
    }
}
