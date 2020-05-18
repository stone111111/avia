using GM.Agent;
using GM.Agent.Sites;
using GM.Agent.Users;
using GM.Cache.Games;
using GM.Common.Games;
using GM.Common.Models;
using GM.Common.Sites;
using GM.Common.Users;
using SP.Provider.Game;
using SP.Provider.Game.Models;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Enums;
using SP.StudioCore.Utils;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Web.API.Agent.Sites;
using static GM.Common.Users.UserGame;

namespace Web.API.Agent
{
    /// <summary>
    /// API接口的逻辑处理类
    /// </summary>
    public sealed class APIAgent : AgentBase<APIAgent>
    {
        /// <summary>
        /// 进入游戏
        /// </summary>
        /// <param name="site">所属商户</param>
        /// <param name="game">当前游戏配置</param>
        /// <param name="user">用户信息</param>
        /// <param name="category">要进入的游戏类型，不指定则为默认</param>
        /// <param name="code">要进入的单独游戏的代码</param>
        /// <returns></returns>
        public LoginResult Login(Site site, GameSetting game, UserGame user, GameCategory? category, string code)
        {
            if (user == null) return new LoginResult(ResultStatus.NoUser);
            if (user.Status == UserGameStatus.Lock) return new LoginResult(ResultStatus.UserLock);

            // 调用API登录接口
            return game.Setting.Login(new LoginUser()
            {
                UserName = user.Account,
                Password = user.Password,
                Category = category,
                Code = code
            });
        }

        /// <summary>
        /// 游客进入游戏（试玩游戏）
        /// </summary>
        /// <param name="site">所属商户</param>
        /// <param name="game">当前游戏配置</param>
        /// <param name="category">要进入的游戏类型，不指定则为默认</param>
        /// <param name="code">要进入的单独游戏的代码</param>
        /// <returns></returns>
        public LoginResult Guest(GameSetting game, GameCategory? category, string code)
        {
            // 调用API登录接口
            return game.Setting.Guest(new LoginUser());
        }

        /// <summary>
        /// 注册接口（自动）
        /// 在 FilterAttribute 中判断是否存在，如果不存在则自动注册
        /// </summary>
        /// <param name="siteId">所属商户</param>
        /// <param name="userName">用户名</param>
        /// <param name="gameinfo">要进入的游戏接口</param>
        /// <returns></returns>
        public ResultStatus Register(int siteId, string userName, GameSetting game, out UserGame userGame)
        {
            int userId = UserAgent.Instance().GetUserID(siteId, userName);

            if (userId == 0)
            {
                if (!WebAgent.IsUserName(userName, 2, 16))
                {
                    userGame = null;
                    return ResultStatus.BadUserName;
                }

                // 用户主表注册
                User user = new User()
                {
                    SiteID = siteId,
                    UserName = userName,
                    CreateAt = DateTime.Now,
                    Status = UserStatus.Normal
                };
                this.WriteDB.InsertIdentity(user);
                userId = user.ID;
            }

            //#2 写入游戏表
            userGame = UserAgent.Instance().GetUserGameInfo(userId, game.ID);
            if (userGame == null)
            {
                Site site = SiteAgent.Instance().GetSiteInfo(siteId);
                RegisterResult result = game.Setting.Register(new RegisterUser()
                {
                    Prefix = site.Prefix,
                    UserName = userName
                });
                if (result)
                {
                    userGame = new UserGame()
                    {
                        SiteID = siteId,
                        UserID = userId,
                        Account = result.Account,
                        Password = result.Password,
                        GameID = game.ID,
                        Status = UserGameStatus.Normal,
                        UpdateAt = DateTime.Now
                    };
                    this.WriteDB.Insert(userGame);
                }
                return result.Status;
            }
            return ResultStatus.Success;
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="siteId">所属商户</param>
        /// <param name="game">当前游戏配置</param>
        /// <param name="user">用户信息</param>
        /// <param name="action">转入转出类型</param>
        /// <param name="orderID">转账订单号（本地）</param>
        /// <param name="currency">货币类型</param>
        /// <param name="money">金额</param>
        /// <returns></returns>
        public TransferResult Transfer(Site site, GameSetting game, UserGame user, TransferAction action, string orderID, decimal money)
        {
            if (user == null) return new TransferResult(ResultStatus.NoUser);
            // 订单格式的判断
            if (WebAgent.IsUserName(orderID, 2, 16)) return new TransferResult(ResultStatus.OrderIDFormat);

            // 金额判断（默认是2位小数，如果游戏接口的特别要求，则在游戏接口中返回金额错误）
            if (money <= 0M || Math.Round(money, 2) != money) return new TransferResult(ResultStatus.BadMoney);

            // 本地锁（如果部署集群则需要修改成为分布式锁）
            lock (LockHelper.GetLoker($"{user.ToString()}"))
            {
                //同一个商户订单重复，不允许操作
                if (this.ReadDB.Exists<UserTransfer>(t => t.SiteID == site.ID && t.SourceID == orderID)) return new TransferResult(ResultStatus.ExistsOrder);

                //添加转账记录,把状态设置为转账中
                UserTransfer userTransfer = new UserTransfer()
                {
                    SiteID = site.ID,
                    GameID = game.ID,
                    UserID = user.UserID,
                    Money = money,
                    Action = action,
                    CreateAt = DateTime.Now,
                    FinishAt = DateTime.MinValue,
                    SystemID = string.Empty,
                    SourceID = orderID,
                    Status = TransferStatus.Paying
                };
                this.WriteDB.InsertIdentity(userTransfer);

                // 调用API接口
                TransferResult result = game.Setting.Transfer(new TransferInfo()
                {
                    Prefix = site.Prefix,
                    UserName = user.Account,
                    Action = action,
                    OrderID = orderID,
                    Currency = site.Currency,
                    Money = money
                });

                userTransfer.SystemID = result.SystemID;
                userTransfer.FinishAt = DateTime.Now;
                userTransfer.Status = result.Status switch
                {
                    ResultStatus.Exception => TransferStatus.Exception,
                    ResultStatus.Success => TransferStatus.Success,
                    _ => TransferStatus.Faild
                };
                this.WriteDB.Update(userTransfer, t => t.SystemID, t => t.FinishAt, t => t.Status);

                if (!result) return new TransferResult(result.Status);

                if (result.Balance != null)
                {
                    UserAgent.Instance().UpdateBalance(user, result.Balance.Value);
                }
                else
                {
                    BalanceResult balanceResult = this.GetBalance(site, game, user);
                    if (balanceResult) result.Balance = balanceResult.Balance;
                }
                return result;
            }
        }

        /// <summary>
        /// 查询转账信息
        /// </summary>
        /// <param name="site">商户</param>
        /// <param name="game">游戏配置</param>
        /// <param name="orderId">商户的转账订单号</param>
        /// <returns></returns>
        public QueryTransferResult QueryTransfer(Site site, GameSetting game, string orderId)
        {
            // 加锁，不允许对同一个订单号进行并发查询
            lock (LockHelper.GetLoker($"{game.ID}-{orderId}"))
            {
                UserTransfer order = TransferAgent.Instance().GetUserTransfer(site.ID, game.ID, orderId);
                if (order == null) return new QueryTransferResult(ResultStatus.NoOrder);
                if (order.Status == TransferStatus.Paying) return new QueryTransferResult(ResultStatus.OrderPaying);

                if (order.Status == TransferStatus.Exception)
                {
                    UserGame user = UserAgent.Instance().GetUserGameInfo(order.UserID, game.ID);
                    // 调用API接口
                    ResultStatus status = game.Setting.QueryTransfer(new QueryTransferInfo()
                    {
                        UserName = user.Account,
                        OrderID = orderId,
                        Currency = site.Currency
                    });
                    if (status == ResultStatus.Exception) return new QueryTransferResult(ResultStatus.Exception);

                    order.Status = status == ResultStatus.Success ? TransferStatus.Success : TransferStatus.Faild;
                    this.WriteDB.Update(order, t => t.Status);
                }

                if (order.Status == TransferStatus.Success)
                {
                    return new QueryTransferResult(order.Money, order.CreateAt, UserAgent.Instance().GetUserName(order.UserID), order.Action, site.Currency);
                }

                return new QueryTransferResult(ResultStatus.OrderFaild);
            }
        }


        /// <summary>
        /// 余额
        /// </summary>
        /// <param name="siteId">所属商户</param>
        /// <param name="game">当前游戏配置</param>
        /// <param name="userName">用户信息</param>
        /// <param name="currency">货币类型</param>
        /// <returns></returns>
        public BalanceResult GetBalance(Site site, GameSetting game, UserGame user)
        {
            if (user == null) return new BalanceResult(ResultStatus.NoUser);
            // 调用API接口
            BalanceResult result = game.Setting.Balance(new BalanceInfo()
            {
                UserName = user.Account,
                Currency = site.Currency
            });
            if (result)
            {
                UserAgent.Instance().UpdateBalance(user, result.Balance);
            }
            return result;
        }
    }
}
