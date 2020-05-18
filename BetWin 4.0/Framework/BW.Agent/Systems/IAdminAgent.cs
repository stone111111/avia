using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Agent.Systems
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IAdminAgent<T> : AgentBase<T> where T : class, new()
    {
    }
}
