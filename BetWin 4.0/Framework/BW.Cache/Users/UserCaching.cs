using BW.Common.Users;
using BW.Common.Views;
using BW.Views;
using SP.StudioCore.Cache.Redis;
using SP.StudioCore.Security;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BW.Cache.Users
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
        /// 清楚用户缓存
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveCache(int userId)
        {
            string key = $"{USERINFO}{userId}";
            this.NewExecutor().KeyDelete(key);
        }


        /// <summary>
        /// 会员登录，产生Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Guid SaveUserID(int userId)
        {
            return base.SaveToken(TOKEN, userId);
        }


        #region ========  登录锁定  ========

        /// <summary>
        /// 用户被锁定登录的时间
        /// </summary>
        private const string LOGIN_LOCK = "LOGINLOCK:";

        /// <summary>
        /// 获取用户锁定登录的时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>错误的次数</returns>
        public int CheckLoginLock(int userId, out TimeSpan? time)
        {
            string key = $"{LOGIN_LOCK}{userId}";
            time = this.NewExecutor().KeyTimeToLive(key);
            if (time == null) return 0;
            return this.NewExecutor().StringGet(key).GetRedisValue<int>();
        }

        /// <summary>
        /// 锁定会员登录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="time">要锁定的时间（分钟）</param>
        public int SaveLoginLock(int userId, int? time)
        {
            if (time == null || time.Value == 0) return 0;
            string key = $"{LOGIN_LOCK}{userId}";
            IBatch batch = this.NewExecutor().CreateBatch();
            Task<long> task = batch.StringIncrementAsync(key);
            batch.KeyExpireAsync(key, TimeSpan.FromMinutes(time.Value));
            batch.Execute();
            return (int)task.Result;
        }


        /// <summary>
        /// 清除登录锁定
        /// </summary>
        /// <param name="userId"></param>
        public void CleanLoginLock(int userId)
        {
            string key = $"{LOGIN_LOCK}{userId}";
            this.NewExecutor().KeyDelete(key);
        }

        #endregion

        #region ========  登录/Token  ========

        /// <summary>
        /// 会员的Token
        /// </summary>
        private const string TOKEN = "TOKEN:";

        /// <summary>
        /// 会员登录之后的Token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public Guid Login(int userId, PlatformSource source)
        {
            string key = $"{TOKEN}{source}";
            return base.SaveToken(key, userId);
        }

        /// <summary>
        /// 根据Token获取用户ID
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int GetUserID(Guid token, PlatformSource source)
        {
            string key = $"{TOKEN}{source}";
            return base.GetTokenID(key, token);
        }


        #endregion
    }
}
