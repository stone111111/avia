using BW;
using BW.Common.Base;
using BW.Common.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SP.StudioCore.API;
using SP.StudioCore.API.Translates;
using SP.StudioCore.Enums;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Web.System.Agent.Sites;
using Web.System.Agent.Systems;

namespace Web.System.Utils
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public sealed class SysFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 全局的一次性执行
        /// </summary>
        static SysFilterAttribute()
        {
            // 视图的数据初始化
            ViewAgent.Instance().Initialize();
        }

        /// <summary>
        /// 方法执行之前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            MethodInfo method = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo;
            bool isGuest = method.HasAttribute<GuestAttribute>();
            bool isUser = context.HttpContext.GetAuth(out string userName, out string token);
            SystemAdmin admin = isUser ? AdminAgent.Instance().GetAdminInfo(token) : null;
            Language language = context.HttpContext.GetLanguage();
            if (!isGuest && admin == null)
            {
                context.Result = (ContentResult)new Result(false, "请先登录".Get(language), new
                {
                    Error = ErrorType.Login
                });
            }
            else if (method.HasAttribute<PermissionAttribute>() &&
                !AdminAgent.Instance().IsPermission(admin?.ID, method.GetAttribute<PermissionAttribute>()))
            {
                string permission = method.GetAttribute<PermissionAttribute>();
                context.Result = (ContentResult)new Result(false, $"{"没有权限".Get(language)}：{ string.Join(",", Permission.NAME.Get(permission, method.Name).Split('.').Select(t => t.Get(language))) }", new
                {
                    Error = ErrorType.Permission
                });
            }

            if (admin != null)
            {
                context.HttpContext.SetItem<IAccount>(admin);
            }
            context.HttpContext.SetItem(language);
        }
    }
}
