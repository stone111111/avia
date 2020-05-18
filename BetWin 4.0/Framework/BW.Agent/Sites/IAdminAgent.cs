using BW.Common.Sites;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Security;
using SP.StudioCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static BW.Common.Sites.SiteAdmin;

namespace BW.Agent.Sites
{
    /// <summary>
    /// 商户管理员操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IAdminAgent<T> : AgentBase<T> where T : class, new()
    {
        /// <summary>
        /// 获取商户下的所有管理员账号（不包括被删除的）
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        protected List<SiteAdmin> GetAdminList(int siteId)
        {
            return this.ReadDB.ReadList<SiteAdmin>(t => t.SiteID == siteId && t.Status != AdminStatus.Deleted).ToList();
        }

        /// <summary>
        /// 重置管理员密码
        /// </summary>
        /// <param name="adminId">要重置的管理员</param>
        /// <param name="siteId">商户</param>
        /// <param name="newPassword">生成的随机密码</param>
        /// <returns></returns>
        protected bool ResetAdminPassword(int adminId, int siteId, out string newPassword)
        {
            newPassword = Encryption.toMD5Short(Guid.NewGuid().ToString("N"));
            if (this.WriteDB.Update<SiteAdmin, string>(t => t.Password, newPassword, t => t.ID == adminId && t.SiteID == siteId) == 1)
            {
                return true;
            }
            return this.FaildMessage("编号错误");
        }

        /// <summary>
        /// 重置谷歌验证码
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public bool ResetAdminSecretKey(int adminId, int siteId)
        {
            if (this.WriteDB.Update<SiteAdmin, Guid>(t => t.SecretKey, Guid.Empty, t => t.ID == adminId && t.SiteID == siteId) == 1)
            {
                return true;
            }
            return this.FaildMessage("编号错误");
        }

        /// <summary>
        /// 获取管理员信息（数据库读取）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected SiteAdmin GetAdminInfo(int id)
        {
            return this.ReadDB.ReadInfo<SiteAdmin>(t => t.ID == id);
        }
    }
}
