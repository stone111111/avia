using GM.Cache;
using GM.Common.Systems;
using SP.StudioCore.Cache.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM.Cache.Systems
{
    /// <summary>
    /// 系统管理员缓存 #4
    /// </summary>
    public sealed class AdminCaching : CacheBase<AdminCaching>
    {
        protected override int DB_INDEX => 4;

        private const string SYSTEMADMIN = "SYSTEMADMIN:";

        private const string TOKEN = "TOKEN";

        /// <summary>
        /// 权限
        /// </summary>
        private const string PERMISSION = "PERMISSION:";

        /// <summary>
        /// 保存管理员信息进入缓存
        /// </summary>
        /// <param name="admin"></param>
        public void SaveAdminInfo(SystemAdmin admin)
        {
            string key = $"{SYSTEMADMIN}{admin.ID}";
            this.NewExecutor().HashSet(key, admin.ToHashEntry().ToArray());
            this.NewExecutor().KeyExpire(key, TimeSpan.FromDays(1));
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="adminId"></param>
        public void RemoveCache(int adminId)
        {
            string key = $"{SYSTEMADMIN}{adminId}";
            this.NewExecutor().KeyDelete(key);
        }

        /// <summary>
        /// 保存管理员权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="permissions"></param>
        public void SavePermission(int adminId, params string[] permissions)
        {
            string key = $"{PERMISSION}{adminId}";
            IBatch batch = this.NewExecutor().CreateBatch();
            batch.KeyDeleteAsync(key);
            foreach (string permission in permissions)
            {
                batch.SetAddAsync(key, permission);
            }
            batch.Execute();
        }

        /// <summary>
        /// 判断是否存在权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool IsPermission(int adminId, string permission)
        {
            string key = $"{PERMISSION}{adminId}";
            return this.NewExecutor().SetContains(key, permission);
        }

        /// <summary>
        /// 获取管理员的全部权限列表
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetPermission(int adminId)
        {
            string key = $"{PERMISSION}{adminId}";
            return this.NewExecutor().SetMembers(key).Select(t => t.GetRedisValue<string>());

        }

        /// <summary>
        /// 获取系统管理员信息
        /// </summary>
        /// <param name="adminId"></param>
        public SystemAdmin GetAdminInfo(int adminId)
        {
            string key = $"{SYSTEMADMIN}{adminId}";
            return this.NewExecutor().HashGetAll(key).Fill<SystemAdmin>();
        }

        /// <summary>
        /// 管理员登录重新生成Token
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public Guid SaveToken(int adminId)
        {
            return base.SaveToken(TOKEN, adminId);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="adminId"></param>
        public void Logout(int adminId)
        {
            base.RemoveToken(TOKEN, adminId);
        }

        /// <summary>
        /// 获取当前登录的管理员
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int GetAdminID(Guid token)
        {
            return base.GetTokenID(TOKEN, token);
        }
    }
}
