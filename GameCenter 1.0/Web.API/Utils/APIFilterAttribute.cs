using GM.Agent.Games;
using GM.Agent.Sites;
using GM.Common.Games;
using GM.Common.Sites;
using GM.Common.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SP.Provider.Game.Models;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Web.API.Agent;
using Web.API.Agent.Sites;

namespace Web.API.Utils
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public sealed class APIFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            MethodInfo method = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo;
            // 如果带有游客标记则不需要做验证判断
            if (method.HasAttribute<GuestAttribute>()) return;

            //#1 从Http头中得到商户信息
            bool isSite = context.HttpContext.GetAuth(out string merchant, out string secretKey);
            if (!isSite)
            {
                context.Result = (ContentResult)new Result(false, ResultStatus.SecretKey.ToString());
                return;
            }
            int siteId = merchant.GetValue<int>();
            Site site = SiteAgent.Instance().GetSiteInfo(siteId);
            if (site == null || site.SecretKey != secretKey)
            {
                context.Result = (ContentResult)new Result(false, ResultStatus.SecretKey.ToString());
                return;
            }

            //#2 固定参数判断（自动在游戏中创建账户）
            string gameCode = context.HttpContext.Request.Form["GameCode"];
            string userName = context.HttpContext.Request.Form["UserName"];
            if (!string.IsNullOrEmpty(gameCode))
            {
                GameSetting game = GameAgent.Instance().GetGameSetting(gameCode);
                if (game == null)
                {
                    context.Result = (ContentResult)new Result(false, ResultStatus.NoGame.ToString());
                    return;
                }
                if (!string.IsNullOrEmpty(userName))
                {
                    ResultStatus registerStatus = APIAgent.Instance().Register(siteId, userName, game, out UserGame user);
                    if (registerStatus != ResultStatus.Success)
                    {
                        context.Result = (ContentResult)new Result(false, registerStatus.ToString());
                        return;
                    }
                    context.HttpContext.SetItem(user);
                }
                context.HttpContext.SetItem(game);
            }
            context.HttpContext.SetItem(site);
        }
    }
}
