using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Types;

namespace Web.API.Action
{
    /// <summary>
    /// 全站GHost追踪
    /// </summary>
    public class GHost : IAction
    {
        private const string KEY = "GHOST";

        public GHost(HttpContext context) : base(context)
        {
        }

        public override Result Invote()
        {
            string js = Properties.Resources.ghost;
            string ghost = this.context.Request.Cookies["GHOST"];
            if (!ghost.IsType<Guid>())
            {
                ghost = Guid.NewGuid().ToString("N");
                ghost = ghost.ToLower();
                this.context.Response.Cookies.Append(KEY, ghost, new CookieOptions()
                {
                    Expires = DateTime.Now.AddYears(1)
                });
            }
            return new Result(ContentType.JS, js.Replace("00000000000000000000000000000000", ghost, StringComparison.CurrentCulture));
        }
    }
}
