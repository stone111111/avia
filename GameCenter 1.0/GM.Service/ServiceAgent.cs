using GM.Agent;
using GM.Agent.Games;
using GM.Agent.Users;
using GM.Cache.Games;
using GM.Common.Games;
using GM.Common.Procedure;
using SP.Provider.Game;
using SP.Provider.Game.Models;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GM.Service
{
    /// <summary>
    /// 服务代理类
    /// </summary>
    public sealed class ServiceAgent : AgentBase<ServiceAgent>
    {
        /// <summary>
        /// 消费订单采集任务，同步保存报表
        /// </summary>
        public string Execute(out string msg)
        {
            OrderTaskModel task = GameOrderCaching.Instance().GetOrderTask();
            if (!task)
            {
                msg = "当前没有任务";
                return task;
            }

            GameSetting setting = GameAgent.Instance().GetGameSetting(task);
            if (setting == null || setting.Status != GameStatus.Open)
            {
                msg = "接口维护中";
                return task;
            }

            int count = 0;
            // 执行订单采集任务
            foreach (OrderModel model in setting.Setting.GetOrderLog(task))
            {
                if (string.IsNullOrEmpty(model.UserName)) continue;

                GameOrder order = new GameOrder(model)
                {
                    UserID = UserAgent.Instance().GetGameUserID(setting.ID, model.UserName, out int siteId),
                    SiteID = siteId,
                    GameID = setting.ID,
                    SettlementAt = new DateTime(1900, 1, 1),
                };
                if (order.Status.IsFinish()) order.SettlementAt = DateTime.Now;

                long hashCode = GameOrderAgent.Instance().GetOrderHashCode(order.GameID, order.SourceID);
                // 未有更新
                if (hashCode == order.HashCode) continue;

                using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
                {
                    // 是否需要重新结算
                    bool reSettlement = false;
                    // 是否需要结算
                    bool isSettlement = false;
                    if (hashCode == 0)
                    {
                        order.AddIdentity(db);
                        isSettlement = order.Status.IsFinish();
                    }
                    else
                    {
                        // 获取原来的状态和原来的结算日期，判断是否需要重新结算
                        GameOrder oldOrder = db.ReadInfo<GameOrder>(t => t.GameID == order.GameID && t.SourceID == order.SourceID,
                            t => t.ID, t => t.SettlementAt, t => t.Status, t => t.BetMoney, t => t.BetAmount, t => t.Money);

                        // 是否需要重新结算
                        reSettlement = oldOrder.Status.IsFinish() && oldOrder.Money != order.Money;

                        // 如果需要重新结算则减去结算过的值
                        if (reSettlement)
                        {
                            db.ExecuteNonQuery(new rpt_SaveUserDate(order.UserID, oldOrder.SettlementAt.Date, order.GameID, order.SiteID, oldOrder.BetMoney * -1M,
                                oldOrder.BetAmount * -1M, oldOrder.Money * -1M, -1));
                        }
                        // 不需要重新结算，且之前已经结算过，则保持原来的结算时间
                        else if (oldOrder.Status.IsFinish())
                        {
                            order.SettlementAt = oldOrder.SettlementAt;
                        }

                        db.Update(order, t => t.ID == oldOrder.ID,
                            t => t.ResultAt, t => t.SettlementAt, t => t.BetAmount, t => t.Money, t => t.Status, t => t.Content, t => t.Status, t => t.UpdateTime, t => t.HashCode, t => t.RawData);

                        isSettlement = !oldOrder.Status.IsFinish() && order.Status.IsFinish();
                    }

                    // 新的结算
                    if (reSettlement || isSettlement)
                    {
                        db.ExecuteNonQuery(new rpt_SaveUserDate(order.UserID, order.SettlementAt.Date, order.GameID, order.SiteID, order.BetMoney,
                              order.BetAmount, order.Money, 1));
                    }

                    db.AddCallback(() =>
                    {
                        GameOrderCaching.Instance().SaveHashCode(order.GameID, order.SourceID, order.HashCode);
                    });

                    db.Commit();
                }

                count++;
            }

            msg = @$"采集完成，共采集到条数：{count}";
            return task;
        }

        /// <summary>
        /// 生产任务（采集和延迟保护）
        /// </summary>
        public int Build()
        {
            int count = 0;
            foreach (string gameCode in this.ReadDB.ReadList<GameSetting, string>(t => t.Code, t => t.Status == GameStatus.Open))
            {
                if (GameOrderCaching.Instance().SaveTask(gameCode)) count++;
            }
            return count;
        }
    }
}
