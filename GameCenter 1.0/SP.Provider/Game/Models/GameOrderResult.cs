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
    /// 订单查询信息
    /// </summary>
    public struct GameOrderInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartAt;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndAt;
    }

    /// <summary>
    /// 订单查询接口要返回的内容
    /// </summary>
    public struct GameOrderResult
    {
        /// <summary>
        /// 订单查询失败
        /// </summary>
        public GameOrderResult(ResultStatus status) : this()
        {
            if (status == ResultStatus.Success) throw new Exception("成功的状态不能调用此方法");
            this.Status = status;
        }

        /// <summary>
        /// 订单查询成功
        /// </summary>
        public GameOrderResult(int recordCount, List<GameOrderBase> gameOrderList, Dictionary<string, object> data = null) : this()
        {
            this.Status = ResultStatus.Success;
            this.RecordCount = recordCount;
            this.GameOrderList = gameOrderList;
            this.Data = data;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public ResultStatus Status { get; private set; }

        /// <summary>
        /// 记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        public List<GameOrderBase> GameOrderList { get; set; }

        /// <summary>
        /// 如果是POST登录的话，需要提交的数据内容
        /// </summary>
        public Dictionary<string, object> Data { get; private set; }

        /// <summary>
        /// 是否查询成功
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(GameOrderResult result)
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
                    this.RecordCount,
                    this.GameOrderList
            });
            }
        }
    }

    /// <summary>
    /// 游戏日志的公共字段
    /// </summary>
    public class GameOrderBase
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 游戏的类别（ESport、Sport、Live、Lottery、Chess、Slot）
        /// </summary>
        public string GameType { get; }

        /// <summary>
        /// 游戏类型（对应枚举 GameType 的名字）
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 游戏帐号名字
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 游戏的开始时间（下注时间）
        /// </summary>
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public decimal BetMoney { get; set; }

        /// <summary>
        /// 有效投注 . 老虎机与彩票 按照投注金额来计算
        /// </summary>
        public decimal BetAmount { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Reward { get; set; }

        /// <summary>
        /// 盈亏
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 下注平台
        /// </summary>
        public PlatformType Platform { get; set; }

        /// <summary>
        /// 下注的IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public string RawData { get; set; }

        /// <summary>
        /// 是否订单已经完成
        /// </summary>
        /// <returns></returns>
        public bool IsFinish { get; set; }

    }
}
