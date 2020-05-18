using SP.StudioCore.Enums;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// 余额信息
    /// </summary>
    public struct BalanceInfo
    {
        /// <summary>
        /// 游戏账户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 币种
        /// </summary>
        public Currency Currency;
    }

    /// <summary>
    /// 余额接口要返回的内容
    /// </summary>
    public struct BalanceResult
    {
        /// <summary>
        /// 查询余额失败
        /// </summary>
        public BalanceResult(ResultStatus status) : this()
        {
            if (status == ResultStatus.Success) throw new Exception("成功的状态不能调用此方法");
            this.Status = status;
        }

        /// <summary>
        /// 查询余额成功
        /// </summary>
        public BalanceResult(decimal balance, Dictionary<string, object> data = null) : this()
        {
            this.Status = ResultStatus.Success;
            this.Balance = balance;
            this.Data = data;
        }


        /// <summary>
        /// 状态
        /// </summary>
        public ResultStatus Status { get; private set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// 额外的信息
        /// </summary>
        public Dictionary<string, object> Data { get; private set; }

        /// <summary>
        /// 是否查询成功
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(BalanceResult result)
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
                    this.Balance,
                    this.Data
                });
            }
        }
    }
}
