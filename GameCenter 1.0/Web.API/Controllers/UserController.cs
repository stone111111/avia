using GM.Agent.Users;
using GM.Common.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Provider.Game;
using SP.Provider.Game.Models;
using SP.StudioCore.Enums;
using SP.StudioCore.Http;
using SP.StudioCore.Model;
using System;
using System.Threading.Tasks;
using Web.API.Agent;
using Web.API.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI
{
    [Route("API/[controller]")]
    public class UserController : APIControllerBase
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        [HttpPost, Route("Login")]
        public ContentResult Login([FromForm]GameCategory? category, [FromForm]string code)
        {
            LoginResult result = APIAgent.Instance().Login(this.SiteInfo, this.GameInfo, this.UserInfo, category, code);
            return new ContentResult()
            {
                ContentType = "application/json",
                StatusCode = 200,
                Content = result.ToString()
            };
        }

        /// <summary>
        /// 游客登录
        /// </summary>
        [HttpPost, Route("Guest")]
        public ContentResult Guest([FromForm]GameCategory? category, [FromForm]string code)
        {
            LoginResult result = APIAgent.Instance().Guest(this.GameInfo, category, code);
            return new ContentResult()
            {
                ContentType = "application/json",
                StatusCode = 200,
                Content = result.ToString()
            };
        }

        /// <summary>
        /// 转账
        /// </summary>
        [HttpPost, Route("Transfer")]
        public ContentResult Transfer([FromForm]TransferAction action, [FromForm]string orderId, [FromForm]decimal money)
        {
            TransferResult result = APIAgent.Instance().Transfer(this.SiteInfo, this.GameInfo, this.UserInfo, action, orderId, money);
            return new ContentResult()
            {
                ContentType = "application/json",
                StatusCode = 200,
                Content = result.ToString()
            };
        }

        /// <summary>
        /// 转账查询
        /// </summary>
        [HttpPost, Route("QueryTransfer")]
        public ContentResult QueryTransfer([FromForm]string orderID)
        {
            QueryTransferResult result = APIAgent.Instance().QueryTransfer(this.SiteInfo, this.GameInfo, orderID);
            return new ContentResult()
            {
                ContentType = "application/json",
                StatusCode = 200,
                Content = result.ToString()
            };
        }

        /// <summary>
        /// 余额查询
        /// </summary>
        [HttpPost, Route("GetBalance")]
        public ContentResult GetBalance()
        {
            BalanceResult result = APIAgent.Instance().GetBalance(this.SiteInfo, this.GameInfo, this.UserInfo);
            return new ContentResult()
            {
                ContentType = "application/json",
                StatusCode = 200,
                Content = result.ToString()
            };
        }
    }
}
