using GM.Common.Games;
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
using Web.System.Agent.Systems;
using Web.System.Utils;
using static GM.Common.Games.GameSetting;

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
        [HttpPost, Permission(GM.Permission.系统配置.游戏配置.Value)]
        public Task GameSettingList()
        {
            IQueryable<GameSetting> list = BDC.GameSetting;

            return this.GetResult(this.ShowResult(BDC.GameSetting.OrderByDescending(t => t.ID), t => new
            {
                t.ID,
                t.Type,
                t.Code,
                t.Name,
                t.Status,
                t.MaintainTime,
                t.SettingString,
                t.Remark
            }));
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
        [HttpPost, Permission(GM.Permission.系统配置.游戏配置.Value)]
        public Task SaveSetting([FromForm]int id, [FromForm]GameType? type, [FromForm]string code, [FromForm]string name,
            [FromForm]string setting, [FromForm]string remark)
        {
            if (string.IsNullOrWhiteSpace(name)) return this.ShowError("请输入游戏名称");
            if (type == null) return this.ShowError("请输入游戏类型");

            GameSetting gamesetting = new GameSetting()
            {
                ID = id,
                Type = type.Value,
                Code = code,
                Name = name,
                Status = GameStatus.Open,
                MaintainTime = DateTime.MinValue,
                SettingString = setting,
                Remark = remark
            };
            return this.GetResult(GameSettingAgent.Instance().SaveSetting(gamesetting));
        }

        /// <summary>
        /// 获取游戏配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统配置.游戏配置.Value)]
        public Task GameSettingInfo([FromForm]int? id)
        {
            GameSetting setting = id == null ? new GameSetting():
                (GameSettingAgent.Instance().GetGameSettingInfo(id.Value) ?? new GameSetting());

            IGameProvider iProvider = GameFactory.GetFactory(setting.Type.ToString(), setting.SettingString);

            return this.GetResult(new
            {
                setting.ID,
                setting.Name,
                setting.Code,
                setting.Type,
                setting.Status,
                setting.MaintainTime,
                setting.Remark,
                Setting = setting.ID == 0 ? new JsonString("[]") : new JsonString(iProvider.ToSetting())
            });
        }

        [HttpPost, Permission(GM.Permission.系统配置.游戏配置.Value)]
        public Task GameGetSetting([FromForm]GameType type)
        {
            IGameProvider iProvider = GameFactory.GetFactory(type.ToString(), string.Empty);

            return this.GetResult(new
            {
                Setting = iProvider == null ? new JsonString("[]") : new JsonString(iProvider.ToSetting())
            });
        }

        /// <summary>
        /// 修改开启状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统配置.游戏配置.Value)]
        public Task UpdateStatus([FromForm]int id, [FromForm]GameStatus status, [FromForm]DateTime maintainTime)
        {
            var setting = GameSettingAgent.Instance().GetGameSettingInfo(id);
            if (setting == null) return this.ShowError("记录不存在");
            setting.Status = status;

            if (status == GameStatus.Maintain)
            {
                setting.MaintainTime = maintainTime;
            }
            else
            {
                setting.MaintainTime = DateTime.MinValue;
            }

            return this.GetResult(GameSettingAgent.Instance().UpdateStatus(setting));
        }

        /// <summary>
        /// 删除游戏设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统配置.游戏配置.Value)]
        public Task GameSettingDelete([FromForm]int id)
        {
            return this.GetResult(GameSettingAgent.Instance().DeleteGameSettingInfo(id));
        }
    }
}
