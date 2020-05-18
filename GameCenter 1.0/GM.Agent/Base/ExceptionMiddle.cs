using GM.Agent.Logs;
using Microsoft.AspNetCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Mvc.MiddleWare;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Agent.Base
{
    /// <summary>
    /// 通用的错误日志处理
    /// </summary>
    public sealed class ExceptionMiddle : ExceptionMiddlewareBase
    {
        public ExceptionMiddle(RequestDelegate next) : base(next)
        {
        }

        protected override ErrorLogModel ThrowRequestID(HttpContext context, Exception ex)
        {
            int siteId = 0;
            int userId = 0;
            ErrorLogModel model = new ErrorLogModel(ex, siteId, userId, context);

            // 存入数据库 & 发送通知
            LogAgent.Instance().SaveLog(model);

            return model;
        }
    }
}
