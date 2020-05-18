using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Provider.CDN
{
    /// <summary>
    /// 阿里云提供的CDN 接口
    /// </summary>
    public sealed class ALIYUN : ICDNProvider
    {
        public ALIYUN()
        {
        }

        public ALIYUN(string queryString) : base(queryString)
        {
        }

        public override bool Delete(string recordId, out string msg)
        {
            throw new NotImplementedException();
        }
    }
}
