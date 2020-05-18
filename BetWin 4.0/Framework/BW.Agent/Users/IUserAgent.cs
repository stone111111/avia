using BW.Cache.Users;
using BW.Common.Procedure;
using BW.Common.Sites;
using BW.Common.Users;
using BW.Common.Views;
using SP.StudioCore.API;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Enums;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BW.Agent.Users
{
    /// <summary>
    /// 会员操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IUserAgent<T> : AgentBase<T> where T : class, new()
    {
        /// <summary>
        /// 添加会员
        /// 规则1：如果是后台添加的会员，不设置注册IP，注册设备
        /// </summary>
        /// <param name="user"></param>
        /// <param name="inviteCode">邀请码（可以是会员也可以是代理的邀请码）|手动创建账号不需要填邀请码，但是需要指定AgentID</param>
        /// <returns></returns>
        protected bool AddUser(Site site, User user, string inviteCode, Language language = Language.CHN)
        {
            if (!WebAgent.IsUserName(user.UserName, 2, 16)) return this.FaildMessage("用户名格式错误", language);
            int userId = this.GetUserID(user.SiteID, user.UserName);
            if (userId != 0) return this.FaildMessage("用户名已存在", language);

            if (!string.IsNullOrEmpty(inviteCode))
            {
                UserInvite invite = this.GetInviteInfo(site.ID, inviteCode);
                if (invite == null) return this.FaildMessage("邀请码错误", language);
                User inviteUser = this.GetUserInfo(site.ID, invite.UserID);
                if (inviteUser.Type == User.UserType.Member)
                {
                    user.AgentID = inviteUser.AgentID;
                }
                else
                {
                    user.AgentID = invite.UserID;
                }
            }

            // 上级
            if (user.AgentID != 0)
            {
                User parent = this.GetUserInfo(site.ID, user.AgentID);
                if (parent == null) return this.FaildMessage("上级错误", language);
                switch (parent.Type)
                {
                    case User.UserType.Partner:
                        user.Type = User.UserType.Agent;
                        break;
                    case User.UserType.Agent:
                        user.Type = User.UserType.Broker;
                        break;
                    case User.UserType.Broker:
                        user.Type = User.UserType.Member;
                        break;
                }
            }
            else
            {
                // 股东账号
                user.Type = User.UserType.Partner;
            }

            #region ========  默认值设定  ========

            user.SiteID = site.ID;
            user.CreateAt = DateTime.Now;
            user.Currency = site.Currency;
            user.Language = site.Language;

            UserDetail detail = new UserDetail();

            #endregion

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                user.AddIdentity(db);

                detail.UserID = user.ID;
                detail.Add(db);

                db.ExecuteNonQuery(new usr_CreateDepth(user.SiteID,user.AgentID,user.ID));

                // 邀请码注册人数增加
                if (!string.IsNullOrEmpty(inviteCode))
                {
                    db.UpdatePlus(new UserInvite() { Member = 1 }, t => t.SiteID == site.ID && t.ID == inviteCode);
                }

                db.Commit();
            }

            return true;
        }

        /// <summary>
        /// 获取用户资料（Redis内读取，不存在则读取数据库）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected User GetUserInfo(int siteId, int userId)
        {
            User user = UserCaching.Instance().GetUserInfo(userId);
            if (user == null)
            {
                user = this.ReadDB.ReadInfo<User>(t => t.SiteID == siteId && t.ID == userId);
                if (user != null)
                {
                    UserCaching.Instance().SaveUserInfo(user);
                }
            }
            return user;
        }

        /// <summary>
        /// 清除用户缓存（Redis）
        /// </summary>
        /// <param name="userId"></param>
        protected void RemoveCache(int userId)
        {
            UserCaching.Instance().RemoveCache(userId);
        }

        /// <summary>
        /// 从数据库内读取用户资料
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected User GetUserInfo(int userId)
        {
            return this.ReadDB.ReadInfo<User>(t => t.ID == userId);
        }

        /// <summary>
        /// 根据用户名获取用户的ID
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int GetUserID(int siteId, string userName)
        {
            if (siteId == 0 || string.IsNullOrEmpty(userName)) return 0;
            int userId = UserCaching.Instance().GetUserID(siteId, userName);
            if (userId == 0)
            {
                userId = this.ReadDB.ReadInfo<User, int>(t => t.ID, t => t.SiteID == siteId && t.UserName == userName);
                if (userId != 0) UserCaching.Instance().SaveUserID(siteId, userId, userName);
            }
            return userId;
        }

        /// <summary>
        /// 用户日志写入
        /// </summary>
        protected void Log(UserLog log)
        {
            if (log.SiteID == 0 || log.UserID == 0 || string.IsNullOrEmpty(log.Content)) return;

            log.CreateAt = DateTime.Now;
            log.IP = IPAgent.IP;
            using (DbExecutor db = NewExecutor())
            {
                log.Add(db);
            }
        }

        #region ========  邀请码  ========

        /// <summary>
        /// 获取邀请码信息（只获取开放的）
        /// </summary>
        /// <param name="inviteId"></param>
        /// <returns></returns>
        protected UserInvite GetInviteInfo(int siteId, string inviteId)
        {
            return this.ReadDB.ReadInfo<UserInvite>(t => t.ID == inviteId && t.SiteID == siteId && t.IsOpen == true);
        }

        #endregion


        #region ========  手机验证码  ========



        #endregion
    }
}
