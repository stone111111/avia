using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Common.Procedure
{
    /// <summary>
    /// 添加用户的上下级关系
    /// </summary>
    public sealed class usr_CreateDepth : IProcedureModel
    {
        public usr_CreateDepth(int siteId, int userId, int childId)
        {
            this.SiteID = siteId;
            this.UserID = userId;
            this.ChildID = childId;
        }

        /// <summary>
        /// 所属商户
        /// </summary>
        public int SiteID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 下级ID
        /// </summary>
        public int ChildID { get; set; }
    }
}
