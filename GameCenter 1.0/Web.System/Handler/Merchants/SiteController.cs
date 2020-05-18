using GM.Agent.Games;
using GM.Common.Games;
using GM.Common.Models;
using GM.Common.Sites;
using GM.Common.Users;
using Microsoft.AspNetCore.Mvc;
using SP.Provider.CDN;
using SP.StudioCore.Enums;
using SP.StudioCore.Http;
using SP.StudioCore.Linq;
using SP.StudioCore.Model;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.System.Agent.Sites;
using Web.System.Agent.Systems;
using Web.System.Utils;
using static GM.Common.Models.SiteGameSetting;
using static GM.Common.Sites.Site;

namespace Web.System.Handler.Merchants
{
    /// <summary>
    /// 商户管理
    /// </summary>
    [Route("Merchant/[controller]/[action]")]
    public class SiteController : SysControllerBase
    {
        #region ========  商户管理  ========
        /// <summary>
        /// 商户列表管理
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task List([FromForm]int? siteId, [FromForm]string name, [FromForm]Site.SiteStatus Status)
        {
            IQueryable<Site> list = BDC.Site.Where(siteId, t => t.ID == siteId.Value)
                .Where(name, t => t.Name.Contains(name)).Where(Status, t => t.Status == Status);

            return this.GetResult(this.ShowResult(list.OrderByDescending(t => t.ID), t => new
            {
                t.ID,
                t.Name,
                t.Status,
                t.Language,
                t.Currency,
                t.Prefix
            }));
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task AddSite([FromForm]int id, [FromForm]string name, [FromForm]Currency currency, [FromForm]Language language,
            [FromForm]string prefix)
        {
            Site site = new Site()
            {
                ID = id,
                Name = name,
                Currency = currency,
                Language = language,
                Status = Site.SiteStatus.Open,
                Prefix = prefix
            };
            return this.GetResult(SiteAgent.Instance().AddSite(site));
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Task Info([FromForm]int id)
        {
            Site site = SiteAgent.Instance().GetSiteInfo(id);
            return this.GetResult(site);
        }

        /// <summary>
        /// 保存商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="currency"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpPost]
        public Task Save([FromForm]int id, [FromForm]string name, [FromForm]Currency currency, [FromForm]Language language,
            [FromForm]string prefix)
        {
            Site site = SiteAgent.Instance().GetSiteInfo(id);
            site.Name = name;
            site.Currency = currency;
            site.Language = language;
            site.Prefix = prefix;

            return this.GetResult(SiteAgent.Instance().SaveSiteInfo(site));
        }
        #endregion

        #region ========  安全管理  ========

        /// <summary>
        /// 获取白名单列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task GetWhiteIP([FromForm]int id)
        {
            Site detail = SiteAgent.Instance().GetSiteInfo(id);
            return this.GetResult(new
            {
                detail.ID,
                detail.WhiteIP,
                detail.SecretKey
            });
        }

        /// <summary>
        /// 保存白名单IP
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <param name="whiteIP">白名单IP列表</param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task SaveWhiteIP([FromForm]int id, [FromForm]string whiteIP, [FromForm]string secretKey)
        {
            return this.GetResult(SiteAgent.Instance().SaveWhiteIP(id, whiteIP, secretKey));
        }

        /// <summary>
        /// 保存状态
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task SaveStatus([FromForm]int id, [FromForm]SiteStatus status)
        {
            return this.GetResult(SiteAgent.Instance().SaveStatus(id, status));
        }

        #endregion

        #region ========  游戏管理  ========
        /// <summary>
        /// 获取游戏列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task SiteGameList([FromForm]int? siteId)
        {
            var list = BDC.SiteGameSetting.Where(siteId, t => t.SiteID == siteId.Value).
                Join(BDC.GameSetting, t => t.GameID, t => t.ID, (a, b) => new
                {
                    a.ID,
                    a.SiteID,
                    a.GameID,
                    GameName = b.Name,
                    a.Credit,
                    a.Paid,
                    a.Rate,
                    a.Sort,
                    a.Status
                });
            return this.GetResult(this.ShowResult(list.OrderByDescending(t => t.Sort), t => t));
        }

        /// <summary>
        /// 保存商户游戏配置
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <param name="gameids">游戏编号列表</param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task SaveSiteGameSetting([FromForm]int id, [FromForm]SiteGameStatus status, [FromForm]decimal rate, [FromForm]byte sort)
        {
            return this.GetResult(SiteAgent.Instance().SaveSiteGameSetting(id, status, rate, sort));
        }

        /// <summary>
        /// 加载商户游戏配置
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <param name="gameids">游戏编号列表</param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task LoadSiteGameSetting([FromForm]int siteid)
        {
            return this.GetResult(SiteAgent.Instance().LoadSiteGameSetting(siteid));
        }

        /// <summary>
        /// 保存商户游戏配置
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <param name="gameids">游戏编号列表</param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task GetSiteGameSetting([FromForm]int id)
        {
            return this.GetResult(SiteAgent.Instance().GetSiteGameSetting(id));
        }

        /// <summary>
        /// 为商户买分
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="money">额度</param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task SitePay([FromForm]int id, [FromForm]decimal money)
        {
            return this.GetResult(SiteAgent.Instance().SitePay(id, money));
        }

        /// <summary>
        /// 为商户批量买分
        /// </summary>
        /// <param name="id">IDs</param>
        /// <param name="money">额度</param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task SitePayMul([FromForm]string IDs, [FromForm]decimal money)
        {
            return this.GetResult(SiteAgent.Instance().SitePayMul(IDs, money));
        }
        #endregion

        #region  游戏订单

        /// <summary>
        /// 商户列表管理
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task GetSiteList()
        {
            IQueryable<Site> list = BDC.Site.Where(t => t.Status == SiteStatus.Open);

            return this.GetResult(this.ShowResult(list.OrderByDescending(t => t.ID), t => new
            {
                t.ID,
                t.Name
            }));
        }

        /// <summary>
        /// 获取游戏列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.商户管理.商户列表.Value)]
        public Task GetGameOrderList([FromForm]int siteID, [FromForm]GameType gameType, [FromForm]int gameID, 
            [FromForm]string playerName, [FromForm]DateTime startAt, [FromForm]DateTime endAt)
        {
            //List<GameOrder> list = GameOrderAgent.Instance().GetGameOrderList(SiteID, GameType, GameID, PlayerName, StartAt, EndAt);

            IQueryable<GameOrder> list = this.BDC.GameOrder.Where(t => t.SiteID == siteID && t.Type == gameType &&
                t.GameID == gameID && t.PlayerName == playerName && t.CreateAt >= startAt && t.CreateAt < endAt);

            IEnumerable<Site> sites = this.BDC.Site;

            IEnumerable<GameSetting> gameSettings = this.BDC.GameSetting;

            IEnumerable<User> users = this.BDC.User;
            return this.GetResult(this.ShowResult(list.OrderByDescending(t => t.CreateAt), t => new
            {
                t.ID,
                t.Type,
                t.SourceID,
                t.SiteID,
                SiteName = sites.SingleOrDefault(p => p.ID == t.SiteID) == null ? "" : sites.SingleOrDefault(p => p.ID == t.SiteID).Name,
                t.UserID,
                UserName = users.SingleOrDefault(p => p.ID == t.UserID) == null ? "" : users.SingleOrDefault(p => p.ID == t.UserID).UserName,
                t.GameID,
                GameName = gameSettings.SingleOrDefault(p=>p.ID == t.GameID) == null ? "" : gameSettings.SingleOrDefault(p => p.ID == t.GameID).Name,
                t.PlayerName,
                t.Code,
                t.CreateAt,
                t.ResultAt,
                t.SettlementAt,
                t.BetMoney,
                t.BetAmount,
                t.Money,
                t.Status,
                t.Content,
                t.UpdateTime,
                t.RawData
            }));
        }
        #endregion
    }
}
