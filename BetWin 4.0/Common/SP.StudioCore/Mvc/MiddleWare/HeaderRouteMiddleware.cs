using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.StudioCore.Mvc.MiddleWare
{
    /// <summary>
    /// Header路由中间件
    /// </summary>
    public class HeaderRouteMiddleware
    {
        private readonly RequestDelegate _next;
        public HeaderRouteMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/")
            {
                string path = context.Request.Headers["Path"].ToString();
                if (string.IsNullOrWhiteSpace(path))
                {
                    context.Response.StatusCode = 404;
                    return context.Response.WriteAsync(string.Empty);
                }
                context.Request.Path = path;
            }
            return _next.Invoke(context);
        }
    }
}
