using SP.StudioCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.API.Agent
{
    public abstract class AgentBase<T> : DbAgent<T> where T : class, new()
    {
        protected AgentBase(string dbConnection) : base(dbConnection)
        {
        }
    }
}
