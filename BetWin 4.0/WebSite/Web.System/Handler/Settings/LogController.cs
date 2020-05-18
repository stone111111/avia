using BW.Common.Logs;
using Microsoft.AspNetCore.Mvc;
using SP.StudioCore.Linq;
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
        public Task ErrorList([FromForm]string id)
        {
            IQueryable<ErrorLog> list = BDC.ErrorLog
                .Where(id, t => t.ErrorID == id.GetValue<Guid>());

            return this.GetResult(this.ShowResult(list.OrderByDescending(t => t.CreateAt), t => new
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
        public Task ErrorDetail([FromForm]string id)
        {
            ErrorLog log = BDC.ErrorLog.FirstOrDefault(t => t.ErrorID == id.GetValue<Guid>());
            if (log == null) return this.ShowError("編號錯誤");
            return this.GetResult(log.Content);
        }
    }
}
