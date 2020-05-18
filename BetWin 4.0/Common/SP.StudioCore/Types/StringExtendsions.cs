using System;
using System.Collections.Generic;
using System.Text;

namespace SP.StudioCore.Types
{
    /// <summary>
    /// 字符串的扩展处理
    /// </summary>
    public static class StringExtendsions
    {
        /// <summary>
        /// 从左侧截取字符串（自动增加省略号）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string str, int length)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            if (str.Length > length)
            {
                if (length > 3) return str.Substring(0, length - 3) + "...";
                return str.Substring(0, length);
            }

            return str;
        }

        /// <summary>
        /// 获取字符串的唯一Hash值（比MD5快）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long GetLongHashCode(this string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            byte[] byteContents = Encoding.UTF8.GetBytes(str);
            System.Security.Cryptography.SHA256 hash = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            byte[] hashText = hash.ComputeHash(byteContents);
            long hashCodeStart = BitConverter.ToInt64(hashText, 0);
            long hashCodeMedium = BitConverter.ToInt64(hashText, 8);
            long hashCodeEnd = BitConverter.ToInt64(hashText, 24);
            return hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
        }
    }
}
