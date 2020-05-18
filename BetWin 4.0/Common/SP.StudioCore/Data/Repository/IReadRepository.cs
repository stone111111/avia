using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace SP.StudioCore.Data.Repository
{
    public interface IReadRepository
    {
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get<T>(int id);
        /// <summary>
        /// 条件筛选获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 条件筛选获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>();
        /// <summary>
        /// 根据ID获取某个字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        TValue GetValue<T, TValue>(int id, Expression<Func<T, TValue>> value);
        /// <summary>
        /// 条件筛选获取某个字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        TValue GetValue<T, TValue>(Expression<Func<T, bool>> expression, Expression<Func<T, TValue>> value);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Any<T>();
        /// <summary>
        /// 条件搜索是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Any<T>(Expression<Func<T, bool>> expression);
    }
}
