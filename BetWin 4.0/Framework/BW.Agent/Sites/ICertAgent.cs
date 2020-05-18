using BW.Cache.Sites;
using BW.Common.Sites;
using SP.StudioCore.Cache.Memory;
using SP.StudioCore.Data.Extension;
using SP.StudioCore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BW.Agent.Sites
{
    public abstract class ICertAgent<T> : AgentBase<T> where T : class, new()
    {
    }
}
