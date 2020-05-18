using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using SP.StudioCore.Http;
using System.Diagnostics;
using SP.StudioCore.Enums;
using System.Threading.Tasks;
using SP.StudioCore.Model;
using System.Linq;
using SP.StudioCore.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace SP.StudioCore.Mvc
{
    /// <summary>
    /// MVC 控制层基类
    /// </summary>
    public abstract class MvcControllerBase : ControllerBase, IServiceProvider
    {
        private Stopwatch Stopwatch { get; }

        public MvcControllerBase()
        {
            this.Stopwatch = new Stopwatch();
            this.Stopwatch.Start();
        }

        /// <summary>
        /// 本次任务的执行时间
        /// </summary>
        /// <returns></returns> 
        protected virtual string StopwatchMessage()
        {
            Stopwatch.Stop();
            return string.Concat(Stopwatch.ElapsedMilliseconds, "ms");
        }

        protected virtual string QF(string name)
        {
            return this.context.QF(name);
        }

        protected virtual T QF<T>(string name, T defaultValue)
        {
            return this.context.QF<T>(name, defaultValue);
        }

        protected virtual int PageIndex
        {
            get
            {
                return this.context.QF("PageIndex", 1);
            }
        }

        protected virtual int PageSize
        {
            get
            {
                return this.context.QF("PageSize", 20);
            }
        }

        /// <summary>
        /// 当前HTTP请求的上下文对象
        /// </summary>
        protected virtual HttpContext context
        {
            get
            {
                return this.HttpContext;
            }
        }

        /// <summary>
        /// 获取语种
        /// </summary>
        protected virtual Language Language
        {
            get
            {
                return this.context.GetLanguage();
            }
        }

        #region ========  公共Result输出  ========

        /// <summary>
        /// 返回对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Task GetResult(object data)
        {
            return new Result(true, this.StopwatchMessage(), data).WriteAsync(this.context);
        }

        protected virtual Task GetResult(ContentType type, object data)
        {
            return new Result(type, data).WriteAsync(this.context);
        }

        /// <summary>
        /// 返回bool值
        /// </summary>
        /// <param name="success"></param>
        /// <param name="successMessage"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual Task GetResult(bool success, string successMessage = "处理成功", object info = null)
        {
            if (success) return new Result(success, successMessage, info).WriteAsync(HttpContext);
            string message = this.HttpContext.RequestServices.GetService<MessageResult>();
            if (string.IsNullOrEmpty(message)) message = "发生不可描述的错误";
            return new Result(false, message).WriteAsync(HttpContext);
        }

        /// <summary>
        /// 返回数组
        /// </summary>
        protected virtual Task GetResult<T, TOutput>(IEnumerable<T> list, Converter<T, TOutput> converter = null, Object data = null) where TOutput : class
        {
            string resultData = this.ShowResult(list, converter, data);
            return this.GetResult(resultData);
        }


        protected virtual string ShowResult<T, TOutput>(IEnumerable<T> list, Converter<T, TOutput> converter = null, Object data = null)
        {
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;

            if (typeof(TOutput) == typeof(string))
            {
                if (converter == null)
                {
                    result = string.Concat("[", string.Join(",", list.Select(t => t)), "]");
                }
                else
                {
                    result = string.Concat("[", string.Join(",", list.Select(t => converter(t))), "]");
                }
            }
            else
            {
                if (converter == null)
                {
                    result = list.ToJson();
                }
                else
                {
                    result = list.ToList().ConvertAll(converter).ToJson();
                }
            }
            _ = sb.Append("{")
                .AppendFormat("\"RecordCount\":{0},", list.Count())
                  .AppendFormat("\"data\":{0},", data == null ? "null" : data.ToJson())
                  .AppendFormat("\"list\":{0}", result)
                .Append("}");
            return sb.ToString();
        }



        /// <summary>
        /// 返回排序数组（自带分页）
        /// </summary>
        protected virtual Task GetResult<T, TOutput>(IOrderedQueryable<T> list, Func<T, TOutput> converter = null, object data = null, Action<IEnumerable<T>> action = null) where TOutput : class
        {
            string resultData = this.ShowResult(list, converter, data, action);
            return this.GetResult(resultData);
        }

        /// <summary>
        /// 分页输出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="list"></param>
        /// <param name="queryList"></param>
        /// <param name="converter"></param>
        /// <param name="data"></param>
        /// <param name="action">分页数据的前置处理</param>
        /// <returns></returns>
        protected virtual string ShowResult<T, TOutput>(IOrderedQueryable<T> list, Func<T, TOutput> converter = null, object data = null, Action<IEnumerable<T>> action = null) where TOutput : class
        {
            if (converter == null) converter = t => t as TOutput;
            StringBuilder sb = new StringBuilder();
            string json = null;
            IEnumerable<T> query;
            if (this.PageIndex == 1)
            {
                query = list.Take(this.PageSize).ToArray();
            }
            else
            {
                query = list.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToArray();
            }
            action?.Invoke(query);
            if (converter == null)
            {
                json = query.ToJson();
            }
            else
            {
                if (typeof(TOutput).Name == "String")
                {
                    json = string.Concat("[", string.Join(",", query.Select(converter)), "]");
                }
                else
                {
                    json = query.Select(converter).ToJson();
                }
            }
            _ = sb.Append("{")
                .AppendFormat("\"RecordCount\":{0},", list.Count())
                .AppendFormat("\"PageIndex\":{0},", this.PageIndex)
                .AppendFormat("\"PageSize\":{0},", this.PageSize)
                .AppendFormat("\"data\":{0}", data == null ? "null" : data.ToJson())
                .AppendFormat(",\"list\":{0}", json)
                .Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// 输出一条错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual Task ShowError(string message)
        {
            return new Result(false, message).WriteAsync(HttpContext);
        }
        protected virtual Task ShowError(string message, object info)
        {
            return new Result(false, message, info).WriteAsync(HttpContext);
        }

        //共有方法需过滤api，否则swagger出错
        [ApiExplorerSettings(IgnoreApi = true)]
        public object GetService(Type serviceType)
        {
            return HttpContext.RequestServices.GetService(serviceType);
        }

        #endregion
    }
}
