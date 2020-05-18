using BW.Common;
using BW.Common.Systems;
using SP.StudioCore.Http;
using SP.StudioCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BW.Common.Base;

namespace Web.System.Utils
{
    public abstract class SysControllerBase : MvcControllerBase
    {
        /// <summary>
        /// 当前管理员
        /// </summary>
        protected virtual IAccount AdminInfo
        {
            get
            {
                return this.context.GetItem<IAccount>();
            }
        }

        protected BizDataContext BDC
        {
            get
            {
                return this.GetService<BizDataContext>();
            }
        }
    }
}
