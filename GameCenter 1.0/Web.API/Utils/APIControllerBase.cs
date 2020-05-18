using GM.Common.Games;
using GM.Common.Sites;
using GM.Common.Users;
using SP.StudioCore.Http;
using SP.StudioCore.Mvc;

namespace Web.API.Utils
{
    /// <summary>
    /// API接口基类
    /// </summary>
    public abstract class APIControllerBase : MvcControllerBase
    {
        /// <summary>
        /// 当前的商户
        /// </summary>
        protected virtual Site SiteInfo
        {
            get
            {
                return this.context.GetItem<Site>();
            }
        }

        /// <summary>
        /// 当前进入的游戏
        /// </summary>
        protected virtual GameSetting GameInfo
        {
            get
            {
                return this.context.GetItem<GameSetting>();
            }
        }

        /// <summary>
        /// 当前会员在游戏中的账号
        /// </summary>
        protected virtual UserGame UserInfo
        {
            get
            {
                return this.context.GetItem<UserGame>();
            }
        }
    }
}
