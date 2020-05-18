using GM.Common.Games;
using SP.Provider.Game;
using SP.StudioCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Common.ViewModels
{
    public class LoginParam : RequestParam
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public int SiteID { get; set; }
        /// <summary>
        /// 游戏类型
        /// </summary>
        public GameCategory gameType { get; set; }
        /// <summary>
        /// 游戏名称
        /// </summary>
        public string GameName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public Language language { get; set; }
    }
}
