using GM.Agent.Sites;
using GM.Cache.Users;
using GM.Common.Games;
using GM.Common.Sites;
using GM.Common.Users;
using SP.Provider.Game;
using SP.Provider.Game.Models;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Enums;
using SP.StudioCore.Web;
using System;
using System.Data;
using static GM.Common.Users.UserGame;

namespace GM.Agent.Users
{
    public class UserAgent : AgentBase<UserAgent>
    {

        /// <summary>
        /// 根据用户名获取用户ID（缓存中读取）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int GetUserID(int siteId, string userName)
        {
            int userId = UserCaching.Instance().GetUserID(siteId, userName);
            if (userId == 0)
            {
                userId = this.ReadDB.ReadInfo<User, int>(t => t.ID, t => t.SiteID == siteId && t.UserName == userName);
                if (userId != 0) UserCaching.Instance().SaveUserID(siteId, userId, userName);
            }
            return userId;
        }



        /// <summary>
        /// 获取用户名 （缓存中读取）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(int userId)
        {
            string userName = UserCaching.Instance().GetUserName(userId);
            if (string.IsNullOrEmpty(userName))
            {
                User user = this.ReadDB.ReadInfo<User>(t => t.ID == userId);
                if (user != null)
                {
                    userName = user.UserName;
                    UserCaching.Instance().SaveUserID(user.SiteID, user.ID, user.UserName);
                }
            }
            return userName;
        }

        /// <summary>
        /// 根据用户ID得到所在的商户ID（缓存读取)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetSiteID(int userId)
        {
            if (userId == 0) return 0;
            int siteId = UserCaching.Instance().GetSiteID(userId);
            if (siteId != 0)
            {
                siteId = this.ReadDB.ReadInfo<User, int>(t => t.SiteID, t => t.ID == userId);
                if (siteId != 0) UserCaching.Instance().SaveSiteID(userId, siteId);
            }
            return siteId;
        }

        #region ========  游戏账户  ========


        /// <summary>
        /// 获取会员在游戏中的信息（缓存中读取）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public UserGame GetUserGameInfo(int userId, int gameId)
        {
            UserGame userGame = UserCaching.Instance().GetUserGameInfo(userId, gameId);
            if (userGame == null)
            {
                userGame = this.ReadDB.ReadInfo<UserGame>(t => t.GameID == gameId && t.UserID == userId);
                if (userGame != null) UserCaching.Instance().SaveUserGameInfo(userGame);
            }
            return userGame;
        }

        /// <summary>
        /// 更新会员在游戏厂商中的余额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <param name="balance"></param>
        public void UpdateBalance(UserGame user, decimal balance)
        {
            user.Balance = balance;
            user.UpdateAt = DateTime.Now;
            this.WriteDB.Update(user, t => t.Balance, t => t.UpdateAt);
        }

        /// <summary>
        /// 通过游戏账户名获取用户ID（同步返回商户ID）
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerName"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int GetGameUserID(int gameId, string playerName, out int siteId)
        {
            int userId = UserCaching.Instance().GetUserID(gameId, playerName);
            if (userId == 0)
            {
                userId = this.ReadDB.ReadInfo<UserGame, int>(t => t.UserID, t => t.GameID == gameId && t.Account == playerName);
                if (userId != 0)
                {
                    UserCaching.Instance().SaveGameUserID(gameId, playerName, userId);
                }
            }
            if (userId == 0)
            {
                siteId = 0;
            }
            else
            {
                siteId = this.GetSiteID(userId);
            }

            return userId;
        }


        #endregion
    }
}
