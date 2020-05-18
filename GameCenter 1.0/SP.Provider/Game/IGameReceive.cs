using Microsoft.AspNetCore.Http;
using SP.Provider.Game.Models;
using SP.Provider.Game.Receives;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.Game
{
    /// <summary>
    /// 支持免转的游戏类型实现
    /// </summary>
    public interface IGameReceive
    {
        /// <summary>
        /// 获取余额查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Result GetBalanceReceive(HttpContext context);

        /// <summary>
        /// 获取金额请求对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Result GetMoneyReceive(HttpContext context);

        /// <summary>
        /// 接收订单推送
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<OrderModel> GetOrders(HttpContext context); 
    }
}
