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
    /// 转账信息
    /// </summary>
    public struct TransferInfo
    {
        /// <summary>
        /// 商户前缀
        /// </summary>
        public string Prefix;

        /// <summary>
        /// 游戏账户名
        /// </summary>
        public string UserName;

        /// <summary>
        /// 本地订单号
        /// </summary>
        public string OrderID;

        /// <summary>
        /// 币种
        /// </summary>
        public Currency Currency;

        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal Money;

        /// <summary>
        /// 转入转出
        /// </summary>
        public TransferAction Action;
    }

    /// <summary>
    /// 转账接口要返回的内容
    /// </summary>
    public struct TransferResult
    {
        /// <summary>
        /// 转账失败
        /// </summary>
        public TransferResult(ResultStatus status) : this()
        {
            if (status == ResultStatus.Success) throw new Exception("成功的状态不能调用此方法");
            this.Status = status;
        }

        /// <summary>
        /// 转账成功
        /// </summary>
        public TransferResult(string systemId, decimal? balance, Dictionary<string, object> data = null) : this()
        {
            this.Status = ResultStatus.Success;
            this.SystemID = systemId;
            this.Balance = balance;
            this.Data = data;
        }

        /// <summary>
        /// 与厂商通信的系统单号（订单查询使用此单号）
        /// </summary>
        public string SystemID { get; set; }

        /// <summary>
        /// 转账状态
        /// </summary>
        public ResultStatus Status { get; private set; }

        /// <summary>
        /// 转账成功之后的余额（如果接口可直接返回就赋值，否则保留为null）
        /// </summary>
        public decimal? Balance { get; set; }

        /// <summary>
        /// 如果是POST登录的话，需要提交的数据内容
        /// </summary>
        public Dictionary<string, object> Data { get; private set; }

        /// <summary>
        /// 是否转账成功
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(TransferResult result)
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
                    this.Balance
                });
            }
        }
    }
}
