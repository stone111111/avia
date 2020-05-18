using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SP.StudioCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SP.StudioCore.Json
{
    public static class JsonExtensions
    {
        /// <summary>
        /// 转化成为json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, JsonSerializerSettingConfig.Setting);
        }

        /// <summary>
        /// 转换指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, params Expression<Func<T, object>>[] fields) where T : class
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (Expression<Func<T, object>> field in fields)
            {
                PropertyInfo property = field.ToPropertyInfo();
                data.Add(property.Name, field.Compile().Invoke(obj));
            }
            return JsonConvert.SerializeObject(data, JsonSerializerSettingConfig.Setting);
        }

        /// <summary>
        /// 获取Map对象值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this JObject info, string key)
        {
            if (info[key] == null) return default;
            return info[key].Value<T>();
        }
    }
}
