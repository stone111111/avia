using GM.Common.Base;
using GM.Common.Logs;
using SP.StudioCore.Data;
using SP.StudioCore.Model;
using SP.StudioCore.Types;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Agent.Logs
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public sealed class LogAgent : AgentBase<LogAgent>
    {
        public LogAgent() : base()
        {
        }

        /// <summary>
        /// 保存错误日志
        /// </summary>
        /// <param name="model"></param>
        public void SaveLog(ErrorLogModel model)
        {
            ErrorLog log = new ErrorLog()
            {
                SiteID = model.SiteID,
                UserID = model.UserID,
                CreateAt = DateTime.Now,
                Title = model.Title.Left(100),
                Content = model.Content,
                IP = model.IP,
                IPAddress = IPAgent.GetAddress(model.IP),
                ErrorID = model.RequestID
            };

            using (DbExecutor db = NewExecutor())
            {
                log.Add(db);
            }
        }
    }
}
