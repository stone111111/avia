using SP.StudioCore.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// 登录账户的信息
    /// </summary>
    public struct LoginUser
    {
        /// <summary>
        /// 游戏账户名
        /// </summary>
        public string UserName;

        /// <summary>
        /// 游戏密码
        /// </summary>
        public string Password;

        /// <summary>
        /// 要进入的游戏代码
        /// </summary>
        public string Game;

        /// <summary>
        /// 要进入的游戏类型
        /// </summary>
        public GameCategory Category;

        /// <summary>
        /// 要进入的游戏代码（一般用于电子）
        /// </summary>
        public string Code;
    }

    /// <summary>
    /// 登录接口要返回的内容
    /// </summary>
    public struct LoginResult
    {
        /// <summary>
        /// 登录失败
        /// </summary>
        public LoginResult(LoginStatus status) : this()
        {
            if (status == LoginStatus.Success) throw new Exception("成功的状态不能调用此方法");
            this.Status = status;
        }

        /// <summary>
        /// 登录成功
        /// </summary>
        public LoginResult(string url, HttpMethod method, Dictionary<string, object> data = null) : this()
        {
            this.Status = LoginStatus.Success;
            this.Url = url;
            this.Method = method;
            this.Data = data;
        }


        /// <summary>
        /// 登录的状态
        /// </summary>
        public LoginStatus Status;

        /// <summary>
        /// 登录地址
        /// </summary>
        public string Url;

        /// <summary>
        /// 登录动作
        /// </summary>
        public HttpMethod Method;

        /// <summary>
        /// 如果是POST登录的话，需要提交的数据内容
        /// </summary>
        public Dictionary<string, object> Data;

        /// <summary>
        /// 是否登录成功
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(LoginResult result)
        {
            return result.Status == LoginStatus.Success;
        }

        /// <summary>
        /// 要对外返回的JSON内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!this)
            {
                return new
                {
                    this.Status
                }.ToJson();
            }
            else
            {
                return this.ToJson();
            }
        }
    }

    /// <summary>
    /// 登录状态
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 发生异常
        /// </summary>
        Exception = 1
    }
}
