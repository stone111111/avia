using BW.Common.Base;
using BW.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.System.Agent.Systems;

namespace Web.System.Agent
{
    /// <summary>
    /// 总后台逻辑工具类
    /// </summary>
    public static class SystemUtils
    {
        /// <summary>
        /// 管理员操作日志
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool Log(this IAccount account, SystemAdminLog.LogType type, string content)
        {
            if (account == null) return false;
            AdminAgent.Instance().SaveLog(account.ID, type, content);
            return true;
        }
    }
}
