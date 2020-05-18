using GM.Common.Logs;
using Microsoft.AspNetCore.Mvc;
using SP.StudioCore.Linq;
using SP.StudioCore.Model;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.System.Utils;

namespace Web.System.Handler.Settings
{
    /// <summary>
    /// 日志管理
    /// </summary>
    [Route("Setting/[controller]/[action]")]
    public sealed class LogController : SysControllerBase
    {
        /// <summary>
        /// 錯誤日志
        /// </summary>
        /// <param name="id">錯誤日志編號</param>
        /// <returns></returns>
        [HttpPost]
        public Result ErrorList([FromForm]string id)
        {
            IQueryable<ErrorLog> list = BDC.ErrorLog
                .Where(id, t => t.ErrorID == id.GetValue<Guid>());

            return this.GetResultContent(this.GetResultList(list.OrderByDescending(t => t.CreateAt), t => new
            {
                t.ErrorID,
                t.CreateAt,
                t.Title,
                t.IP,
                t.IPAddress
            }));
        }

        /// <summary>
        /// 查看錯誤信息明細
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Result ErrorDetail([FromForm]string id)
        {
            ErrorLog log = BDC.ErrorLog.FirstOrDefault(t => t.ErrorID == id.GetValue<Guid>());
            if (log == null) return this.GetResultError("編號錯誤");
            return this.GetResultContent(log.Content);
        }
    }
}
