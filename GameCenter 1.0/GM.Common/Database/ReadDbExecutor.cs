using SP.StudioCore.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GM.Common.Database
{
    /// <summary>
    /// 只读数据库操作对象
    /// </summary>
    public sealed class ReadDbExecutor : DbExecutor
    {
        public ReadDbExecutor() : base(Setting.ReadConnection)
        {
        }
    }
}
