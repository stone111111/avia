using System;
using System.Collections.Generic;
using System.Text;

namespace SP.StudioCore.Data
{
    public enum DataConnectionMode : byte
    {
        /// <summary>
        /// 不使用数据持久化
        /// </summary>
        None,
        /// <summary>
        /// 实例。 以数据库的操作类为一个实例，操作类注销时关闭链接
        /// </summary>
        Instance,
        /// <summary>
        /// 访问过程。 以一次完整的访问过程为单位，需在golbal的Request_End事件中关闭连接。
        /// 此类型仅限于Web程序
        /// </summary>
        Context
    }
}
