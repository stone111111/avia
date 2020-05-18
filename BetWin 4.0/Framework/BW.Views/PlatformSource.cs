using System;
using System.Collections.Generic;
using System.Text;

namespace BW.Views
{
    /// <summary>
    /// 视图平台
    /// </summary>
    public enum PlatformSource : byte
    {
        /// <summary>
        /// PC
        /// </summary>
        PC,
        /// <summary>
        /// H5版本（包括H5封装的APP）
        /// </summary>
        H5,
        /// <summary>
        /// 原生APP
        /// </summary>
        APP
    }
}
