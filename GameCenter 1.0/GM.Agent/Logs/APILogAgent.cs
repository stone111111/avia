using GM.Common.Base;
using GM.Common.Logs;
using SP.Provider.Game.Models;
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
    public sealed class APILogAgent : AgentBase<APILogAgent>
    {
        public APILogAgent() : base()
        {
        }

        /// <summary>
        /// 保存错误日志
        /// </summary>
        /// <param name="model"></param>
        public void SaveLog(APILogModel model)
        {
            APILog log = new APILog()
            {
                SiteID = 0,
                Game = model.Game,
                Url = model.Url,
                PostData = model.PostData,
                ResultData = model.ResultData,
                Status = model.Status,
                Time = model.Time,
                CreateAt = model.CreateAt
            };

            using (DbExecutor db = NewExecutor())
            {
                log.Add(db);
            }
        }
    }
}
