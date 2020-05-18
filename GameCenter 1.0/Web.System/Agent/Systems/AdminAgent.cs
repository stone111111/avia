using GM.Agent.Systems;
using GM.Cache.Systems;
using GM.Common.Base;
using GM.Common.Systems;
using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using SP.StudioCore.Security;
using SP.StudioCore.Types;
using SP.StudioCore.Web;
using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Web.System.Properties;
using static GM.Common.Systems.SystemAdminLog;

namespace Web.System.Agent.Systems
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    public sealed class AdminAgent : IAdminAgent<AdminAgent>
    {
        /// <summary>
        /// 系统管理员登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Login(string userName, string password, string code, out string token)
        {
            token = null;
            SystemAdmin admin = this.ReadDB.ReadInfo<SystemAdmin>(t => t.UserName == userName);
            if (admin == null)
            {
                if (userName == "admin" && !this.ReadDB.Exists<SystemAdmin>(t => t.ID != 0))
                {
                    this.CreateDefaultAdmin();
                    return this.FaildMessage("已创建默认管理员账号，请重新登录");
                }
                return this.FaildMessage("用户名不存在");
            }
            if (admin.Status != SystemAdmin.AdminStatus.Normal)
            {
                ((IAccount)admin).Log(LogType.Login, "当前账号被禁止登录");
                return this.FaildMessage("当前账号被禁止登录");
            }
            if (admin.SecretKey != Guid.Empty && !GoogleAuthenticator.Validate(admin.SecretKey.ToString("N"), code))
            {
                ((IAccount)admin).Log(LogType.Login, "验证码错误");
                return this.FaildMessage("验证码错误");
            }

            if (admin.Password != Encryption.SHA1WithMD5(password))
            {
                ((IAccount)admin).Log(LogType.Login, "密码错误");
                return this.FaildMessage("密码错误");
            }

            using (DbExecutor db = NewExecutor())
            {
                admin.LoginAt = DateTime.Now;
                admin.LoginIP = IPAgent.IP;
                admin.Update(db, t => t.LoginAt, t => t.LoginIP);
            }

            token = $"{admin.UserName}:{AdminCaching.Instance().SaveToken(admin.ID).ToString("N")}".ToBasicAuth();
            AdminCaching.Instance().SavePermission(admin.ID, admin.Permission.Split(','));
            this.RemoveCache(admin.ID);
            return ((IAccount)admin).Log(LogType.Login, "登录成功");
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="adminId"></param>
        public void Logout(int adminId)
        {
            if (adminId == 0) return;
            AdminCaching.Instance().Logout(adminId);
            this.RemoveCache(adminId);
        }

        /// <summary>
        /// 创建一个默认的管理员账号
        /// </summary>
        private void CreateDefaultAdmin()
        {
            SystemAdmin admin = new SystemAdmin()
            {
                UserName = "admin",
                Password = Encryption.SHA1WithMD5("admin"),
                NickName = "超级管理员",
                Permission = this.GetPermission(),
                Status = SystemAdmin.AdminStatus.Normal
            };
            using (DbExecutor db = NewExecutor())
            {
                admin.Add(db);
            }
        }

        /// <summary>
        /// 得到所有的权限
        /// </summary>
        /// <returns></returns>
        private string GetPermission()
        {
            return string.Join(",", AdminMenu.GetPermissions(XElement.Parse(Resources.Permission)));
        }

        /// <summary>
        /// 是否包含权限
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="permission">权限</param>
        /// <returns></returns>
        public bool IsPermission(int? adminId, string permission)
        {
            if (!adminId.HasValue || adminId == 0) return false;
            return AdminCaching.Instance().IsPermission(adminId.Value, permission);
        }

        /// <summary>
        /// 获取管理员信息(缓存中读取)
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public SystemAdmin GetAdminInfo(int adminId)
        {
            SystemAdmin admin = AdminCaching.Instance().GetAdminInfo(adminId);
            if (admin == null)
            {
                admin = this.ReadDB.ReadInfo<SystemAdmin>(t => t.ID == adminId);
                if (admin != null)
                {
                    AdminCaching.Instance().SaveAdminInfo(admin);
                }
            }
            return admin;
        }

        /// <summary>
        /// 获取当前登录的管理员信息（本地缓存10分钟)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public SystemAdmin GetAdminInfo(string input)
        {
            if (!Guid.TryParse(input, out Guid token)) return null;
            int adminId = AdminCaching.Instance().GetAdminID(token);
            if (adminId == 0) return null;
            string key = $"SYSTEMADMIN:{adminId}";
            return MemoryUtils.Get(key, TimeSpan.FromMinutes(10), () =>
            {
                return this.GetAdminInfo(adminId);
            });
        }

        /// <summary>
        /// 清除缓存（本地缓存+Redis)
        /// </summary>
        /// <param name="adminId"></param>
        public void RemoveCache(int adminId)
        {
            string key = $"SYSTEMADMIN:{adminId}";
            MemoryUtils.Remove(key);
            AdminCaching.Instance().RemoveCache(adminId);
        }

        /// <summary>
        /// 获取管理员有权限的菜单列表
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public AdminMenu GetAdminMenu(int adminId)
        {
            XElement root = XElement.Parse(Resources.Permission);
            return new AdminMenu(root, true, AdminCaching.Instance().GetPermission(adminId).ToArray());
        }

        #region ========  账号管理  ========

        /// <summary>
        /// 添加管理员账号
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AddAdminInfo(string username, string password)
        {
            if (!WebAgent.IsUserName(username)) return this.FaildMessage("账户名错误，账户名长度再5~16位之间，字母与数字的组合");
            if (string.IsNullOrEmpty(password)) return this.FaildMessage("请设置管理员密码");

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                if (db.Exists<SystemAdmin>(t => t.UserName == username))
                {
                    return this.FaildMessage("账户名已存在");
                }

                new SystemAdmin()
                {
                    UserName = username,
                    Password = Encryption.SHA1WithMD5(password),
                    Status = SystemAdmin.AdminStatus.Normal
                }.Add(db);

                db.Commit();
            }
            return this.AccountInfo.Log(SystemAdminLog.LogType.Setting, $"添加管理员账号:{username}");
        }

        /// <summary>
        /// 修改权限状态
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="status"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool SavePermission(int adminId, SystemAdmin.AdminStatus status, string permission)
        {
            SystemAdmin admin = this.GetAdminInfo(adminId);
            if (admin == null) return this.FaildMessage("账号错误");
            admin.Status = status;
            admin.Permission = permission;
            this.WriteDB.Update(admin, t => t.Status, t => t.Permission);
            this.RemoveCache(admin.ID);
            return this.AccountInfo.Log(SystemAdminLog.LogType.Setting, $"修改管理员{admin.UserName}权限");
        }

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public bool ResetPassword(int adminId)
        {
            SystemAdmin admin = this.GetAdminInfo(adminId);
            if (admin == null) return this.FaildMessage("账号错误");
            admin.Password = Encryption.SHA1WithMD5("123456");
            this.WriteDB.Update(admin, t => t.Password);
            this.RemoveCache(admin.ID);
            return this.AccountInfo.Log(SystemAdminLog.LogType.Setting, $"重置管理员{admin.UserName}密码");
        }

        #endregion


        #region ========  管理员日志  ========

        /// <summary>
        /// 保存操作日志
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        internal void SaveLog(int adminId, SystemAdminLog.LogType type, string content)
        {
            this.WriteDB.Insert(new SystemAdminLog()
            {
                AdminID = adminId,
                Type = type,
                Content = content.Left(500),
                CreateAt = DateTime.Now,
                IP = IPAgent.IP,
                PostData = this.context.GetLog()
            });
        }

        #endregion
    }
}
