using GM.Agent.Sites;
using GM.Cache.Sites;
using GM.Common.Games;
using GM.Common.Models;
using GM.Common.Sites;
using GM.Common.Systems;
using SP.StudioCore.Data;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static GM.Common.Models.SiteGameSetting;
using static GM.Common.Sites.Site;
using static GM.Common.Systems.SystemAdminLog;

namespace Web.System.Agent.Sites
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    public sealed class SiteAgent : ISiteAgent<SiteAgent>
    {
        #region ========  商户资料  ========

        /// <summary>
        /// 新建站点
        /// </summary>
        public bool AddSite(Site site)
        {
            if (site.ID <= 0 || this.ReadDB.Exists<Site>(t => t.ID == site.ID)) return this.FaildMessage("编号已经存在");
            if (string.IsNullOrEmpty(site.Name)) return this.FaildMessage("请输入商户名");
            if (this.ReadDB.Exists<Site>(t => t.Name == site.Name)) return this.FaildMessage("商户名重复");
            if (site.Prefix.Length != 3) return this.FaildMessage("前缀错误，只能三位！");

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                //自动产生密钥
                Guid secretKey = Guid.NewGuid();
                site.SecretKey = secretKey.ToString();

                site.Add(db);

                db.Commit();
            }

            this.AccountInfo.Log(LogType.Site, $"新建商户{site.ID}");

            return true;
        }

        /// <summary>
        /// 获取商户信息（从数据库读取）
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public new Site GetSiteInfo(int siteId)
        {
            return this.ReadDB.ReadInfo<Site>(t => t.ID == siteId);
        }

        /// <summary>
        /// 保存商户信息
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public new bool SaveSiteInfo(Site site)
        {
            return base.SaveSiteInfo(site);
        }


        #endregion

        #region ========  安全管理  ========

        /// <summary>
        /// 保存白名单
        /// </summary>
        public bool SaveWhiteIP(int siteId, string whiteIP, string secretKey)
        {
            if (siteId == 0) return this.FaildMessage("商户ID错误");
            if (string.IsNullOrEmpty(whiteIP)) return this.FaildMessage("请输入白名单");
            if (string.IsNullOrEmpty(secretKey)) return this.FaildMessage("请输入密钥");
            IEnumerable<string> iplist = whiteIP.Split(',').Where(t => IPAgent.regex.IsMatch(t)).Select(t => t).Distinct();
            if (!iplist.Any()) return this.FaildMessage("白名单IP输入错误");
            whiteIP = string.Join(",", iplist);

            Site site = new Site()
            {
                ID = siteId,
                WhiteIP = whiteIP,
                SecretKey = secretKey
            };

            this.WriteDB.Update<Site>(site, t => t.ID == siteId, t => t.WhiteIP, t => t.SecretKey);
            SiteCaching.Instance().SaveWhiteIP(siteId, iplist);
            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"修改商户{siteId}白名单");
        }

        /// <summary>
        /// 保存状态
        /// </summary>
        public bool SaveStatus(int siteId, SiteStatus status)
        {
            this.WriteDB.Update<Site, SiteStatus>(t => t.Status, status, t => t.ID == siteId);
            SiteCaching.Instance().RemoveSiteInfo(siteId);
            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"修改商户{siteId}状态");
        }
        #endregion

        #region ========  游戏管理  ========

        /// <summary>
        /// 保存游戏配置
        /// </summary>
        public bool SaveSiteGameSetting(int Id, SiteGameStatus status, decimal rate, byte sort)
        {
            SiteGameSetting sitegamesetting = new SiteGameSetting()
            {
                ID = Id,
                Status = status,
                Rate = rate,
                Sort = sort
            };
            this.WriteDB.Update<SiteGameSetting>(sitegamesetting, t => t.ID == Id, t => t.Status, t => t.Rate, t => t.Sort);

            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"修改商户游戏配置成功{Id}");
        }

        /// <summary>
        /// 加载游戏配置
        /// </summary>
        public bool LoadSiteGameSetting(int siteId)
        {
            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                IEnumerable<GameSetting> list = ReadDB.ReadList<GameSetting>(t => t.Status != GameStatus.Close);

                foreach (GameSetting gameSetting in list)
                {
                    if (this.ReadDB.Exists<SiteGameSetting>(t => t.SiteID == siteId && t.GameID == gameSetting.ID)) continue;
                    SiteGameSetting siteGameSetting = new SiteGameSetting()
                    {
                        SiteID = siteId,
                        GameID = gameSetting.ID,
                        Status = SiteGameStatus.Open
                    };

                    siteGameSetting.Add(db);
                }

                db.Commit();
            }

            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"加载商户游戏配置成功{siteId}");
        }

        /// <summary>
        /// 获取商户游戏配置信息（从数据库读取）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SiteGameSetting GetSiteGameSetting(int Id)
        {
            return this.ReadDB.ReadInfo<SiteGameSetting>(t => t.ID == Id);
        }

        /// <summary>
        /// 为商户游戏买分
        /// </summary>
        public bool SitePay(int Id, decimal money)
        {
            SiteGameSetting sitegamesetting = this.ReadDB.ReadInfo<SiteGameSetting>(t => t.ID == Id);

            if (sitegamesetting == null) return this.FaildMessage("买分错误");
            if (money == 0) return this.FaildMessage("金额不能为0");

            sitegamesetting.Paid += money;
            sitegamesetting.Credit += money;

            //添加买分日志
            CreditLog creditLog = new CreditLog()
            {
                GameID = sitegamesetting.GameID,
                SiteID = sitegamesetting.SiteID,
                Type = CreditLog.ChangeType.Add,
                ChangeCredit = money,
                Balance = sitegamesetting.Paid,
                OrderID = sitegamesetting.ID.ToString(),
                CreateAt = DateTime.Now
            };

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                sitegamesetting.Update(db, t => t.Paid, t => t.Credit);

                creditLog.Add(db);

                db.Commit();
            }
            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"修改商户游戏配置成功{Id}");
        }

        /// <summary>
        /// 为商户游戏买分
        /// </summary>
        public bool SitePayMul(string IDs, decimal money)
        {
            string[] ids = IDs.Split("|");
            if (ids.Length == 0) return this.FaildMessage("请选择需要买分的游戏");

            using (DbExecutor db = NewExecutor(IsolationLevel.ReadUncommitted))
            {
                foreach (string id in ids)
                {
                    if (string.IsNullOrEmpty(id)) continue;

                    int temp = int.Parse(id);
                    SiteGameSetting sitegamesetting = this.ReadDB.ReadInfo<SiteGameSetting>(t => t.ID == temp);
                    sitegamesetting.Paid += money;
                    sitegamesetting.Credit += money;

                    //添加买分日志
                    CreditLog creditLog = new CreditLog()
                    {
                        GameID = sitegamesetting.GameID,
                        SiteID = sitegamesetting.SiteID,
                        Type = CreditLog.ChangeType.Add,
                        ChangeCredit = money,
                        Balance = sitegamesetting.Paid,
                        OrderID = sitegamesetting.ID.ToString(),
                        CreateAt = DateTime.Now
                    };
                    sitegamesetting.Update(db, t => t.Paid, t => t.Credit);

                    creditLog.Add(db);
                }

                db.Commit();
            }
            return this.AccountInfo.Log(SystemAdminLog.LogType.Site, $"买分成功");
        }
        #endregion
    }
}
