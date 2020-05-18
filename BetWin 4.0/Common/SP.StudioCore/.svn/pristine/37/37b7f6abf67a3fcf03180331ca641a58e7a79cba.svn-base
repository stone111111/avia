using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace SP.StudioCore.Data.Repository
{
    public interface IWriteRepository
    {
        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete<T>(int id);
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Delete<T>(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 更新某个字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field">更新的字段</param>
        /// <param name="value">更新的值</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        bool Update<T, TValue>(Expression<Func<T, bool>> condition, Expression<Func<T, TValue>> field, TValue value);
        /// <summary>
        /// 更新多个字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="condition">条件</param>
        /// <param name="fields">多个值</param>
        /// <returns></returns>
        bool Upldate<T>(T entity, Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] fields) where T : class, new();
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Insert<T>(T entity);
        /// <summary>
        /// 新增实体并返回Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int InsertById<T>(T entity);
    }
}
