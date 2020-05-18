using SP.Provider.Game.Models;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SP.StudioCore.Web;
using System.Net;
using SP.StudioCore.Net;
using SP.StudioCore.Array;
using System.Diagnostics;
using SP.StudioCore.Types;
using SP.StudioCore.Security;
using System.Text.RegularExpressions;
using SP.StudioCore.Ioc;
using SP.StudioCore.Json;

namespace SP.Provider.Game
{
    /// <summary>
    /// 游戏供应商的基类
    /// </summary>
    public abstract class IGameProvider : ISetting, IServiceProvider
    {
        /// <summary>
        /// 注入的逻辑实现类
        /// </summary>
        protected IGameDelegate GameDelegate
        {
            get
            {
                return IocCollection.GetService<IGameDelegate>();
            }
        }

        /// <summary>
        /// 当前游戏厂商的名字
        /// </summary>
        protected virtual string Provider => this.GetType().Name;

        public IGameProvider()
        {
        }

        public IGameProvider(string queryString) : base(queryString)
        {
        }

        #region ========  API接口交互  ========

        /// <summary>
        /// 登录游戏
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract LoginResult Login(LoginUser user);

        /// <summary>
        /// 游客登录游戏
        /// </summary>
        /// <returns></returns>
        public abstract LoginResult Guest(LoginUser guest);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract RegisterResult Register(RegisterUser user);

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract TransferResult Transfer(TransferInfo info);

        /// <summary>
        /// 转账查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract QueryTransferResult QueryTransfer(QueryTransferInfo info);

        /// <summary>
        /// 余额查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract BalanceResult Balance(BalanceInfo info);

        /// <summary>
        /// 进行订单采集
        /// </summary>
        /// <param name="task">采集任务</param>
        /// <returns></returns>
        public abstract IEnumerable<OrderModel> GetOrderLog(OrderTaskModel task);

        #endregion

        #region ========  工具方法  ========

        /// <summary>
        /// 对返回的内容进行正确性判断
        /// </summary>
        /// <returns></returns>
        protected abstract ResultStatus GetStatus(string result);

        /// <summary>
        /// 获取开始标记
        /// 默认规则 开始时间取得标记然后减去5分钟
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected virtual long GetStartMark(OrderTaskModel task, byte mark = 0)
        {
            long start = Math.Max(WebAgent.GetTimestamps(DateTime.Now.AddDays(-1)), this.GameDelegate.GetMarkTime(task, mark));
            return task.Type switch
            {
                MarkType.Normal => WebAgent.GetTimestamps(WebAgent.GetTimestamps(start).AddMinutes(-5)),
                MarkType.Delay => WebAgent.GetTimestamps(WebAgent.GetTimestamps(start).AddMinutes(-5)),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// 获取结束标记
        /// 默认规则 常规采集每次采集1个小时， 延迟采集每次采集3个小时
        /// </summary>
        /// <param name="task"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        protected virtual long GetEndMark(OrderTaskModel task, long start, byte mark = 0)
        {
            DateTime startTime = WebAgent.GetTimestamps(start);
            return task.Type switch
            {
                MarkType.Normal => WebAgent.GetTimestamps(startTime.AddHours(1).Min(DateTime.Now)),
                MarkType.Delay => WebAgent.GetTimestamps(startTime.AddHours(3).Min(DateTime.Now)),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// 创建用户名规则（默认前缀_用户名随机数)
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="userName">用户名</param>
        /// <param name="length">允许的最大长度</param>
        /// <param name="count">重试次数（为0不加随机数）</param>
        /// <returns></returns>
        protected virtual string GetPlayerName(string prefix, string userName, int count = 0, int length = 16)
        {
            //#1 去除用户名中非字母的字符，并转化成为小写
            userName = Regex.Replace(userName, @"[^\w]", string.Empty).ToLower();
            //#2 加上前缀
            userName = string.Concat(prefix, "_", userName);
            //#3 长度判断
            if (userName.Length > length) userName = userName.Substring(0, length);
            //#4 随机字符串
            if (count > 0)
            {
                if (userName.Length > length - 4) userName = userName.Substring(0, length - 4);
                // 4位的小写字母/数字
                userName += Encryption.toMD5Short(Guid.NewGuid().ToString("N").Substring(0, 16)).ToLower();
            }
            return userName;
        }

        /// <summary>
        /// 创建一个随机的转账订单号
        /// </summary>
        /// <param name="prefix">商户前缀</param>
        /// <param name="length">当前接口允许最大订单号长度</param>
        /// <returns></returns>
        protected virtual string GetSystemID(string prefix, int length = 16)
        {
            string systemId = string.Concat(prefix, "_", Guid.NewGuid().ToString("N").ToLower());
            if (systemId.Length > length) systemId = systemId.Substring(0, length);
            return systemId;
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        protected virtual void SaveLog(string url, string postData, string resultData, ResultStatus status, long time)
        {
            this.GameDelegate?.SaveAPILog(new APILogModel()
            {
                CreateAt = DateTime.Now,
                Game = this.Provider,
                Status = status,
                PostData = postData,
                ResultData = resultData,
                Time = (int)time,
                Url = url
            });
        }


        /// <summary>
        /// 当前游戏使用的时区偏移量（与北京时间比对）
        /// </summary>
        protected virtual TimeSpan TimeOffset => TimeSpan.FromHours(0);

        /// <summary>
        /// 时区转换（转化成为北京时间）
        /// 一般用于订单数据内的时间转换
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        protected virtual DateTime TimeConvert(DateTime datetime)
        {
            return datetime.Add(this.TimeOffset);
        }

        /// <summary>
        /// 时区还原（把北京时间还原成为游戏供应商的时区）
        /// 一般用于订单查询
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        protected virtual DateTime TimeRevert(DateTime datetime)
        {
            return datetime.AddTicks(this.TimeOffset.Ticks * -1);
        }


        /// <summary>
        /// 统一的网关交互方法（POST方式）
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="headers">自定义的头部内容</param>
        /// <param name="data">要发送的数据</param>
        /// <param name="result">网关返回内容</param>
        /// <returns></returns>
        protected virtual ResultStatus POST(string url, Dictionary<string, string> headers, Dictionary<string, object> data, out string result)
        {
            result = string.Empty;
            string postData = data == null ? string.Empty : data.ToQueryString();
            Stopwatch sw = new Stopwatch();
            ResultStatus status = ResultStatus.Success;
            sw.Start();
            try
            {
                using (WebClient wc = new WebClient())
                {
                    result = NetAgent.UploadData(url, postData, Encoding.UTF8, wc, headers);
                    return status = this.GetStatus(result);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message + "\n\r" + result;
                return status = ResultStatus.Exception;
            }
            finally
            {
                this.SaveLog(url, new { Header = headers.ToQueryString(), PostData = postData }.ToJson(), result, status, sw.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// 统一网关交互方法（GET方法）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual ResultStatus GET(string url, Dictionary<string, string> headers, out string result)
        {
            result = string.Empty;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ResultStatus status = ResultStatus.Success;
            try
            {
                result = NetAgent.DownloadData(url, Encoding.UTF8, headers);
                return status = this.GetStatus(result);
            }
            catch (Exception ex)
            {
                result = ex.Message + "\n\r" + result;
                return status = ResultStatus.Exception;
            }
            finally
            {
                this.SaveLog(url, new { Header = headers.ToQueryString() }.ToJson(), result, status, sw.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// 统一的网关交互方法（POST JSON）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="json">要发送到API接口的JSON格式内容</param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual ResultStatus POST(string url, Dictionary<string, string> headers, string json, out string result)
        {
            result = string.Empty;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ResultStatus status = ResultStatus.Success;
            try
            {
                if (headers == null) headers = new Dictionary<string, string>();
                if (!headers.ContainsKey("Content-Type")) headers.Add("Content-Type", "application/json");
                result = NetAgent.UploadData(url, json, Encoding.UTF8, null, headers);
                return status = this.GetStatus(result);
            }
            catch (Exception ex)
            {
                result = ex.Message + "\n\r" + result;
                return status = ResultStatus.Exception;
            }
            finally
            {
                this.SaveLog(url, new { Header = headers.ToQueryString(), Content = json }.ToJson(), result, status, sw.ElapsedMilliseconds);
            }
        }

        #endregion

        public object GetService(Type serviceType)
        {
            return IocCollection.GetService(serviceType);
        }
    }
}
