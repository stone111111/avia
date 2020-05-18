using SP.StudioCore.Enums;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;

namespace SP.Provider.Game.Models
{
    /// <summary>
    /// 转账信息
    /// </summary>
    public struct QueryTransferInfo
    {
        /// <summary>
        /// 转账的用户名
        /// </summary>
        public string UserName;

        /// <summary>
        /// 在厂商处的订单号
        /// </summary>
        public string OrderID;

        /// <summary>
        /// 货币种类
        /// </summary>
        public Currency Currency;
    }

    /// <summary>
    /// 转账接口要返回的内容
    /// </summary>
    public struct QueryTransferResult
    {
        /// <summary>
        /// 转账失败
        /// </summary>
        public QueryTransferResult(ResultStatus status) : this()
        {
            if (status == ResultStatus.Success) throw new Exception("成功的状态不能调用此方法");
            this.Status = status;
        }

        /// <summary>
        /// 转账成功
        /// </summary>
        public QueryTransferResult(decimal money, DateTime createAt,
            string userName, TransferAction action, Currency currency, Dictionary<string, object> data = null) : this()
        {
            this.Status = ResultStatus.Success;
            this.Money = money;
            this.CreateAt = createAt;
            this.UserName = userName;
            this.Action = action;
            this.Currency = currency;
            this.Data = data;
        }


        /// <summary>
        /// 转账状态
        /// </summary>
        public ResultStatus Status { get; private set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Money { get; private set; }

        /// <summary>
        /// 货币种类
        /// </summary>
        public Currency Currency { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateAt { get; private set; }

        /// <summary>
        /// 会员账号
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 转账类型
        /// </summary>
        public TransferAction Action { get; private set; }

        /// <summary>
        /// 附加返回信息
        /// </summary>
        public Dictionary<string, object> Data { get; private set; }

        /// <summary>
        /// 是否转账成功
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(QueryTransferResult result)
        {
            return result.Status == ResultStatus.Success;
        }

        public static implicit operator ResultStatus(QueryTransferResult result)
        {
            return result.Status;
        }

        public static implicit operator QueryTransferResult(ResultStatus status)
        {
            return new QueryTransferResult()
            {
                Status = status
            };
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
                    this.Action,
                    this.Money,
                    this.CreateAt,
                    this.UserName,
                    this.Currency,
                    this.Data
                });
            }
        }
    }
}
