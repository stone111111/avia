using GM.Common.Models;
using GM.Common.Users;
using SP.StudioCore.Cache.Redis;
using SP.StudioCore.Security;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace GM.Cache.Users
{
    /// <summary>
    /// 用户缓存 #2
    /// </summary>
    public sealed class UserCaching : CacheBase<UserCaching>
    {
        protected override int DB_INDEX => 2;

        /// <summary>
        /// 用户名对应的用户ID
        /// </summary>
        private const string USERID = "USERID:";


        /// <summary>
        /// 用户ID对应的站点ID
        /// </summary>
        private const string USERSITE = "USERSITEID:";

        /// <summary>
        /// 用户ID对应用户名
        /// </summary>
        private const string USERNAME = "USERNAME:";

        /// <summary>
        /// 用户信息
        /// </summary>
        private const string USERINFO = "USERINFO:";

        /// <summary>
        /// 会员在游戏中的已注册账户信息
        /// </summary>
        private const string USERGAME = "USERGAME:";

        #region ========  会员资料有关  ========

        /// <summary>
        /// 保存用户名与ID的对应关系
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public void SaveUserID(int siteId, int userId, string userName)
        {
            if (siteId == 0 || userId == 0 || string.IsNullOrEmpty(userName)) return;
            string hash = userId.ToString().GetHash();
            IBatch batch = this.NewExecutor().CreateBatch();
            batch.HashSetAsync($"{USERID}{siteId}:{ userName.GetHash() }", userName, userId);
            batch.HashSetAsync($"{USERNAME}{ hash }", userId, userName);
            batch.HashSetAsync($"{USERSITE}{hash}", userId, siteId);
            batch.Execute();
        }

        /// <summary>
        /// 根据用户名获取用户ID
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int GetUserID(int siteId, string userName)
        {
            string key = $"{USERID}{siteId}:{ userName.GetHash() }";
            RedisValue value = this.NewExecutor().HashGet(key, userName);
            return value.IsNull ? 0 : value.GetRedisValue<int>();
        }


        /// <summary>
        /// 根据用户ID获取用户名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(int userId)
        {
            string key = $"{USERNAME}{ userId.ToString().GetHash() }";
            return this.NewExecutor().HashGet(key, userId).GetRedisValue<string>();
        }

        /// <summary>
        /// 获取会员所在的商户ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetSiteID(int userId)
        {
            string key = $"{USERSITE}{ userId.ToString().GetHash() }";
            return this.NewExecutor().HashGet(key, userId).GetRedisValue<int>();
        }

        /// <summary>
        /// 保存用户所在的商户ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public void SaveSiteID(int userId, int siteId)
        {
            string key = $"{USERSITE}{ userId.ToString().GetHash() }";
            this.NewExecutor().HashSet(key, userId, siteId);
        }

        /// <summary>
        /// 从Redis中读取会员资料
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserInfo(int userId)
        {
            string key = $"{USERINFO}{userId}";
            return this.NewExecutor().HashGetAll(key);
        }

        /// <summary>
        /// 保存用户资料进入Redis
        /// </summary>
        /// <param name="user"></param>
        public void SaveUserInfo(User user)
        {
            string key = $"{USERINFO}{user.ID}";
            this.NewExecutor().HashSet(key, user);
        }

        /// <summary>
        /// 清除用户缓存
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveCache(int userId)
        {
            string key = $"{USERINFO}{userId}";
            this.NewExecutor().KeyDelete(key);
        }


        #endregion

        #region ========  会员游戏账号  ========

        /// <summary>
        /// 获取用户的游戏账号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public UserGame GetUserGameInfo(int userId, int gameId)
        {
            string key = $"{USERGAME}{userId}:{gameId}";
            return this.NewExecutor().HashGetAll(key);
        }

        /// <summary>
        /// 保存用户的游戏账号信息
        /// </summary>
        /// <param name="userGame"></param>
        public void SaveUserGameInfo(UserGame userGame)
        {
            string key = $"{USERGAME}{userGame.UserID}:{userGame.GameID}";
            this.NewExecutor().HashSet(key, userGame);
        }


        /// <summary>
        /// 通过游戏账户名获取用户ID
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public int GetGameUserID(int gameId, string playerName)
        {
            string key = $"{USERGAME}{gameId}:{ playerName.GetHash() }";
            return this.NewExecutor().HashGet(key, playerName).GetRedisValue<int>();
        }

        /// <summary>
        /// 保存游戏账户与用户ID的对应管理
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerName"></param>
        /// <param name="userId"></param>
        public void SaveGameUserID(int gameId, string playerName, int userId)
        {
            string key = $"{USERGAME}{gameId}:{ playerName.GetHash() }";
            this.NewExecutor().HashSet(key, playerName, userId);
        }

        #endregion

    }
}
