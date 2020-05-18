using GM.Common;
using GM.Common.Systems;
using SP.StudioCore.Http;
using SP.StudioCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using GM.Common.Base;
using SP.StudioCore.Ioc;

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
                return IocCollection.GetService<BizDataContext>();
            }
        }
    }
}
