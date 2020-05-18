using SP.StudioCore.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BW.Common.Database
{
    /// <summary>
    /// 写库
    /// </summary>
    public class WriteDbExecutor : DbExecutor
    {
        public WriteDbExecutor():base(Setting.DbConnection)
        {
        }
    }
}
