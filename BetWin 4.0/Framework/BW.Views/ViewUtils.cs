using SP.StudioCore.Enums;
using SP.StudioCore.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BW.Views
{
    /// <summary>
    /// 视图工具
    /// </summary>
    public static class ViewUtils
    {
        /// <summary>
        /// 获取视图的配置代码（短路径）
        /// 例如： BW.Views.APP.Login => Login
        ///        BW.Views.IViews.ILogin => Login 
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <returns></returns>
        public static string GetCode<TView>() where TView : IViewBase
        {
            return GetCode(typeof(TView).FullName);
        }

        /// <summary>
        /// 获取视图的配置代码（短路径）
        /// 例如： BW.Views.APP.Login => Login
        ///        BW.Views.IViews.ILogin => Login 
        /// </summary>
        /// <param name="fullName">完整路径</param>
        /// <returns></returns>
        public static string GetCode(string fullName)
        {
            return Regex.Replace(fullName, @"^BW\.Views\.(APP\.|H5\.|PC\.|IViews\.I)", string.Empty);
        }

        /// <summary>
        /// 获取视图配置的完整类型路径（自动判断是否是抽象类）
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static string GetCode<TView>(PlatformSource platform) where TView : IViewBase
        {
            Type type = typeof(TView);
            if (!type.IsAbstract) return type.FullName;
            return type.FullName.Replace("IViews.I", $"{platform}.", StringComparison.Ordinal);
        }

        /// <summary>
        /// 获取视图的配置代码
        /// </summary>
        /// <typeparam name="IView"></typeparam>
        /// <param name="isAbstract">是否是抽象接口</param>
        /// <returns></returns>
        public static string GetCode<TView>(out PlatformSource? platform) where TView : IViewBase
        {
            platform = null;
            string fullname = typeof(TView).FullName;
            Regex regex = new Regex(@"^BW\.Views\.(?<Type>APP|H5|PC|IViews)(\.|\.I)");
            if (regex.IsMatch(fullname)) return null;

            string type = regex.Match(fullname).Groups["Type"].Value;
            if (Enum.IsDefined(typeof(PlatformSource), type))
            {
                platform = (PlatformSource)Enum.Parse(typeof(PlatformSource), type);
            }
            return regex.Replace(fullname, string.Empty);
        }

        public static PlatformSource? GetPlatform(Type type)
        {
            string fullname = type.FullName;
            Regex regex = new Regex(@"^BW\.Views\.(?<Type>APP|H5|PC)\.");
            if (!regex.IsMatch(fullname)) return null;
            return regex.Match(fullname).Groups["Type"].Value.ToEnum<PlatformSource>();
        }

        private static Dictionary<string, IViewBase> _instanceCache = new Dictionary<string, IViewBase>();
        /// <summary>
        /// 创建视图对象
        /// </summary>
        /// <param name="fullname">视图类的路径（不能是抽象类)</param>
        /// <param name="setting">配置参数</param>
        /// <returns></returns>
        public static IViewBase CreateInstance(string fullname, string setting)
        {
            string key = Encryption.toMD5($"{fullname}:{setting}");
            if (_instanceCache.ContainsKey(key)) return _instanceCache[key];

            Type type = typeof(ViewUtils).Assembly.GetType(fullname);
            IViewBase view = null;
            if (type != null && type.IsSubclassOf(typeof(IViewBase)) && !type.IsAbstract)
            {
                view = (IViewBase)Activator.CreateInstance(type, new object[] { setting });
                _instanceCache.Add(key, view);
            }
            return view;
        }

        /// <summary>
        /// 返回视图的默认配置内容
        /// </summary>
        /// <typeparam name="TView">可以是抽象类</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TView CreateInstance<TView>(PlatformSource platform, string setting = null) where TView : IViewBase
        {
            string fullname = GetCode<TView>(platform);
            return (TView)CreateInstance(fullname, setting);
        }

    }
}
