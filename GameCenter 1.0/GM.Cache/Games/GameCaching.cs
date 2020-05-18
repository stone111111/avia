using GM.Common.Games;
using SP.StudioCore.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Cache.Games
{
    /// <summary>
    /// 游戏配置相关 #3
    /// </summary>
    public class GameCaching : CacheBase<GameCaching>
    {
        protected override int DB_INDEX => 3;

        /// <summary>
        /// 游戏代码与游戏设定ID的对应值
        /// </summary>
        private const string GAMECODE = "GAMECODE";

        /// <summary>
        /// 游戏配置对象
        /// </summary>
        private const string GAMESETTING = "GAMESETTING:";

        /// <summary>
        /// 通过游戏代码获取游戏ID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int GetGameID(string code)
        {
            return this.NewExecutor().HashGet(GAMECODE, code).GetRedisValue<int>();
        }

        /// <summary>
        /// 保存游戏代码与ID的对应关系
        /// </summary>
        /// <param name="code"></param>
        /// <param name="gameId"></param>
        public void SaveGameID(string code, int gameId)
        {
            this.NewExecutor().HashSet(GAMECODE, code, gameId);
        }

        /// <summary>
        /// 获取游戏配置信息
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public GameSetting GetGameSetting(int gameId)
        {
            string key = $"{GAMESETTING}{gameId}";
            return this.NewExecutor().HashGetAll(key);
        }

        /// <summary>
        /// 保存游戏配置信息
        /// </summary>
        /// <param name="setting"></param>
        public void SaveGameSetting(GameSetting setting)
        {
            string key = $"{GAMESETTING}{setting.ID}";
            this.SaveGameID(setting.Code, setting.ID);
            this.NewExecutor().HashSet(key, setting);
        }

        /// <summary>
        /// 保存订单编号
        /// </summary>
        /// <param name="setting"></param>
        public string SaveOrderID(string OrderID)
        {
            string key = $"{OrderID}";

            string value = Guid.NewGuid().ToString();

            this.NewExecutor().StringSet(key, value);

            return value;
        }

        /// <summary>
        /// 保存订单编号
        /// </summary>
        /// <param name="setting"></param>
        public string GetOrderID(string OrderID)
        {
            string key = $"{OrderID}";

            return this.NewExecutor().StringGet(key).ToString();
        }
    }
}
