using GM.Common.Systems;
using Microsoft.AspNetCore.Mvc;
using SP.StudioCore.API;
using SP.StudioCore.Http;
using SP.StudioCore.Json;
using SP.StudioCore.Model;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Web.System.Agent.Systems;
using Web.System.Properties;
using Web.System.Utils;

namespace Web.System.Handler.Systems
{
    /// <summary>
    /// 账号有关
    /// </summary>
    [Route("system/[controller]/[action]")]
    public sealed class AdminController : SysControllerBase
    {
        /// <summary>
        /// 系统管理员登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost, Guest]
        public Result Login([FromForm]string userName, [FromForm]string password, [FromForm]string code)
        {
            return this.GetResultContent(AdminAgent.Instance().Login(userName, password, code, out string token), "登录成功", new
            {
                Token = token
            });
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result Menu()
        {
            AdminMenu menu = AdminAgent.Instance().GetAdminMenu(this.AdminInfo.ID);
            return this.GetResultContent(menu.ToString());
        }

        /// <summary>
        /// 当前管理员信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result Info()
        {
            SystemAdmin admin = AdminAgent.Instance().GetAdminInfo(this.AdminInfo);
            return this.GetResultContent(new
            {
                admin.ID,
                admin.Name,
                admin.LoginAt,
                admin.LoginIP,
                IPAddress = IPAgent.GetAddress(admin.LoginIP),
                admin.Face
            });
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost, Guest]
        public Result Logout()
        {
            AdminAgent.Instance().Logout(this.AdminInfo);
            return this.GetResultContent(true);
        }

        #region ========  账号管理  ========

        /// <summary>
        /// 账号管理
        /// </summary>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统配置.账号管理.Value)]
        public Result GetList()
        {
            var list = BDC.SystemAdmin.Where(t => t.Status != SystemAdmin.AdminStatus.Delete).OrderByDescending(t => t.ID).AsEnumerable();

            return this.GetResultContent(this.GetResultList(list, t => new
            {
                t.ID,
                t.Name,
                t.Face,
                t.Status,
                t.LoginAt,
                t.LoginIP,
                IPAddress = IPAgent.GetAddress(t.LoginIP).ToString(),
                IsSecretKey = t.SecretKey != Guid.Empty
            }));
        }

        /// <summary>
        /// 查看单个账号的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统配置.账号管理.Value)]
        public Result GetInfo([FromForm]int id)
        {
            SystemAdmin admin = AdminAgent.Instance().GetAdminInfo(id) ?? new SystemAdmin()
            {
                Permission = string.Empty
            };
            XElement root = XElement.Parse(Resources.Permission);
            return this.GetResultContent(new
            {
                admin.ID,
                admin.Status,
                admin.UserName,
                Permission = new JsonString(new AdminMenu(root, false, admin.Permission.Split(',')))
            });
        }

        /// <summary>
        /// 添加一个管理员账号
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统配置.账号管理.Value)]
        public Result AddAdmin([FromForm]string userName, [FromForm]string password)
        {
            return this.GetResultContent(AdminAgent.Instance().AddAdminInfo(userName, password));
        }

        /// <summary>
        /// 修改管理员权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统配置.账号管理.Value)]
        public Result SavePermission([FromForm]int id, [FromForm]SystemAdmin.AdminStatus? status, [FromForm]string permission)
        {
            return this.GetResultContent(AdminAgent.Instance().SavePermission(id, status.Value, permission));
        }

        /// <summary>
        /// 重置管理员密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Permission(GM.Permission.系统配置.账号管理.Value)]
        public Result ResetPassword([FromForm]int id)
        {
            return this.GetResultContent(AdminAgent.Instance().ResetPassword(id));
        }

        #endregion
    }
}
