using Microsoft.AspNetCore.Http;
using SP.StudioCore.Array;
using SP.StudioCore.Enums;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Web.API.Action
{
    public static class ActionFactory
    {
        /// <summary>
        /// 执行
        /// </summary>
        public static Result Invote(HttpContext context)
        {
            string path = context.Request.Path.ToString().Substring(1);
            if (path.StartsWith("imageupload.ashx")) path = "upload";
            string typeName = $"Web.API.Action.{path}";
            Type type = typeof(ActionFactory).Assembly.GetType(typeName, false, true);
            if (type == null) return context.ShowError(HttpStatusCode.NotFound, $"未找到类型{typeName}");
            IAction action = (IAction)Activator.CreateInstance(type, context);
            return action.Invote();
        }

        /// <summary>
        /// 命令行执行
        /// </summary>
        /// <param name="args"></param>
        public static void Invote(string[] args)
        {
            string action = args.Get("-action");
            switch (action)
            {
                case "Translate":
                    new Translate(args).Execute();
                    break;
            }
            //string typeName = $"Web.API.Action.{action}";
            //Type type = typeof(ActionFactory).Assembly.GetType(typeName, false, true);
            //if (type == null)
            //{
            //    Console.WriteLine($"未找到类型{typeName}");
            //    return;
            //}
            //IAction obj = (IAction)Activator.CreateInstance(type, args);
            //obj.Execute();
        }
    }
}
