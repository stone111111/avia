using SP.StudioCore.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// 注册会员实例
    /// </summary>
    public struct RegisterUser
    {
        /// <summary>
        /// 要使用的用户名
        /// </summary>
        public string UserName;

        /// <summary>
        /// 商户前缀
        /// </summary>
        public string Prefix;

        /// <summary>
        /// 密码（如果需要的话）
        /// </summary>
        public string Password;

        /// <summary>
        /// 创建一个随机的用户名
        /// </summary>
        /// <param name="count">第多少次重试</param>
        /// <param name="maxLength">用户名允许的最大长度</param>
        /// <returns></returns>
        public string CreateUserName(int count, int maxLength = 16)
        {
            string name = $"{this.Prefix}_{this.UserName}";
            if (count > 0)
            {
                if (name.Length > maxLength - 4) name = name.Substring(0, maxLength - 4);
                name += Encryption.toMD5Short(Guid.NewGuid().ToString("N").Substring(0, 16));
            }
            return name;
        }
    }

    /// <summary>
    /// 注册之后的返回信息
    /// </summary>
    public struct RegisterResult
    {
        /// <summary>
        /// 失败状态
        /// </summary>
        /// <param name="status"></param>
        public RegisterResult(ResultStatus status) : this()
        {
            this.Status = status;
            if (this.Status == ResultStatus.Success) throw new Exception("不支持的状态");
        }

        /// <summary>
        /// 成功状态
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public RegisterResult(string account, string password)
        {
            this.Status = ResultStatus.Success;
            this.Account = account;
            this.Password = password;
        }

        /// <summary>
        /// 成功注册的用户名
        /// </summary>
        public string Account;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password;

        /// <summary>
        /// 状态
        /// </summary>
        public ResultStatus Status;

        public static implicit operator bool(RegisterResult result)
        {
            return result.Status == ResultStatus.Success;
        }
    }
}
