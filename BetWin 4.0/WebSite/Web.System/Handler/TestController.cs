using BW.Common.Sites;
using Microsoft.AspNetCore.Mvc;
using SP.StudioCore.Enums;
using SP.StudioCore.Linq;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.System.Utils;

namespace Web.System.Handler
{
    /// <summary>
    /// 测试类
    /// </summary>
    [Route("[controller]/[action]")]
    public class TestController : SysControllerBase
    {
        /// <summary>
        /// 测试排序值
        /// </summary>
        /// <returns></returns>
        [HttpGet, Guest]
        public Task Sort()
        {
            var list = BDC.Site.OrderBy("ID", SortType.DESC);

            return this.GetResult(this.ShowResult(list, t => t));
        }
    }
}
