using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.CDN
{
    /// <summary>
    /// 自建的CDN，使用CDNBest系统
    /// </summary>
    public sealed class CDNBest : ICDNProvider
    {
        public CDNBest()
        {
        }

        public CDNBest(string queryString) : base(queryString)
        {
        }

        public override bool Delete(string recordId, out string msg)
        {
            throw new NotImplementedException();
        }
    }
}
