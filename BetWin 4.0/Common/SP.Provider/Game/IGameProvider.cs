using SP.Provider.Game.Models;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SP.StudioCore.Web;

namespace SP.Provider.Game
{
    /// <summary>
    /// 游戏供应商的基类
    /// </summary>
    public abstract class IGameProvider : ISetting, IServiceProvider
    {
        protected IGameDelegate GameDelegate
        {
            get
            {
                return this.GetService<IGameDelegate>();
            }
        }

        public IGameProvider(string queryString) : base(queryString)
        {
        }

        /// <summary>
        /// 登录游戏
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract LoginResult Login(LoginUser user);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract bool Register(LoginUser user);

        /// <summary>
        /// 保存日志
        /// </summary>
        protected virtual void SaveLog(string url, string postData, string resultData, bool success)
        {

        }

        public object GetService(Type serviceType)
        {
            if (Context.Current == null) return null;
            return Context.Current.RequestServices.GetService(serviceType);
        }
    }
}
