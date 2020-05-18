using SP.StudioCore.Enums;
using SP.StudioCore.Json;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// 登录/注册账户的信息
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
        public string Game { get; set; }

        /// <summary>
        /// 要进入的游戏类型
        /// </summary>
        public GameCategory? Category;

        /// <summary>
        /// 要进入的游戏代码（一般用于电子）
        /// </summary>
        public string Code;

        /// <summary>
        /// 要调用的语种
        /// </summary>
        public Language Language;

        /// <summary>
        /// 要使用的币种
        /// </summary>
        public Currency Currency;
    }

    /// <summary>
    /// 登录接口要返回的内容
    /// </summary>
    public struct LoginResult
    {
        /// <summary>
        /// 登录失败
        /// </summary>
        public LoginResult(ResultStatus status) : this()
        {
            if (status == ResultStatus.Success) throw new Exception("成功的状态不能调用此方法");
            this.Status = status;
        }

        /// <summary>
        /// 登录成功
        /// </summary>
        public LoginResult(string url, HttpMethod method, Dictionary<string, object> data = null) : this()
        {
            this.Status = ResultStatus.Success;
            this.Url = url;
            this.Method = method.Method;
            this.Data = data;
        }


        /// <summary>
        /// 登录的状态
        /// </summary>
        public ResultStatus Status { get; private set; }

        /// <summary>
        /// 登录地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 登录动作
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// 如果是POST登录的话，需要提交的数据内容
        /// </summary>
        public Dictionary<string, object> Data { get; private set; }

        /// <summary>
        /// 是否登录成功
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(LoginResult result)
        {
            return result.Status == ResultStatus.Success;
        }

        /// <summary>
        /// 要对外返回的JSON内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!this)
            {
                return new Result(false, this.Status.ToString());
            }
            else
            {
                return new Result(true, this.Status.ToString(), new
                {
                    this.Url,
                    this.Method,
                    this.Data
                });
            }
        }
    }
}
