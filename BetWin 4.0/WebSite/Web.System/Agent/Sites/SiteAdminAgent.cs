using BW.Agent.Sites;
using BW.Common.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BW.Common.Systems.SystemAdminLog;

namespace Web.System.Agent.Sites
{
    /// <summary>
    /// 对于商户管理员的操作
    /// </summary>
    public sealed class SiteAdminAgent : IAdminAgent<SiteAdminAgent>
    {
        /// <summary>
        /// 获取商户下所有的管理员账号
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public new List<SiteAdmin> GetAdminList(int siteId)
        {
            return base.GetAdminList(siteId);
        }

        public new bool ResetAdminPassword(int adminId, int siteId, out string newPassword)
        {
            if (base.ResetAdminPassword(adminId, siteId, out newPassword))
            {
                return this.AccountInfo.Log(LogType.Site, $"重置商户{siteId}管理员{adminId}的登录密码为{newPassword}");
            }
            return false;
        }

        public new bool ResetAdminSecretKey(int adminId, int siteId)
        {
            if (base.ResetAdminSecretKey(adminId, siteId))
            {
                return this.AccountInfo.Log(LogType.Site, $"重置商户{siteId}管理员{adminId}的登录验证码");
            }
            return false;
        }
    }
}
