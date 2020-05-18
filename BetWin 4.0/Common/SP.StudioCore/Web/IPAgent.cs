using ipdb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using SP.StudioCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SP.StudioCore.Web
{
    /// <summary>
    /// 获取IP
    /// </summary>
    public static class IPAgent
    {

        /// <summary>
        /// 没有IP地址
        /// </summary>
        private const string NO_IP = "0.0.0.0";

        /// <summary>
        /// 不支持的IP（可能是IPv6）
        /// </summary>
        private const string ERROR_IP = "255.255.255.255";

        /// <summary>
        /// IP库的路径
        /// </summary>
        private const string IPDATA_PATH = "ipipfree.ipdb";

        /// <summary>
        /// 获取当前访问的IP
        /// </summary>
        public static string IP
        {
            get
            {
                return Context.Current.GetIP();
            }
        }

        /// <summary>
        /// IPv4的正则验证
        /// </summary>
        public static readonly Regex regex = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
        public static string GetIP(this HttpContext context, bool isUserIp = false)
        {
            if (context == null) return NO_IP;
            string ip = string.Empty;

            string[] keys = new[] { null, "Ali-CDN-Real-IP", "X-Real-IP", "X-Forwarded-For" };
            if (isUserIp) keys[0] = Const.USERIP;
            foreach (string key in keys)
            {
                if (key == null || !context.Request.Headers.ContainsKey(key)) continue;
                string value = context.Request.Headers[key];
                if (regex.IsMatch(value)) ip = regex.Match(value).Value;
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.MapToIPv4().ToString();
            }
            if (!regex.IsMatch(ip))
            {
                ip = ERROR_IP;
            }
            return ip;
        }

        /// <summary>
        /// 本地缓存库
        /// </summary>
        private static Dictionary<string, CityInfo> addressCache = new Dictionary<string, CityInfo>();
        public static CityInfo GetAddress(string ip)
        {
            if (!regex.IsMatch(ip) || ip == NO_IP) return ip ?? string.Empty;
            if (addressCache.ContainsKey(ip)) return addressCache[ip];
            lock (addressCache)
            {
                if (!File.Exists(IPDATA_PATH)) return new CityInfo(); ;
                City db = new City(IPDATA_PATH);
                CityInfo info = db.findInfo(ip, "CN");
                if (!addressCache.ContainsKey(ip)) addressCache.Add(ip, info);
                return info;
            }
        }

        /// <summary>
        /// 获取当前访问IP
        /// </summary>
        /// <returns></returns>
        public static string GetAddress()
        {
            return GetAddress(IP);
        }

        /// <summary>
        /// IP地址转化成为long型
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IPToLong(this string ip)
        {
            if (!regex.IsMatch(ip) || ip == NO_IP) return 0;
            byte[] ip_bytes = new byte[8];
            string[] strArr = ip.Split(new char[] { '.' });
            for (int i = 0; i < 4; i++)
            {
                ip_bytes[i] = byte.Parse(strArr[3 - i]);
            }
            return BitConverter.ToInt64(ip_bytes, 0);
        }
    }
}
