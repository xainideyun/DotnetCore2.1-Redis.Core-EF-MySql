using Redis.Stu.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Stu.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// 新增实体对象
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entitys"></param>
        void Add(IEnumerable<TEntity> entitys);
        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entitys"></param>
        void Remove(IEnumerable<TEntity> entitys);
        /// <summary>
        /// 修改实体对象
        /// </summary>
        /// <param name="entity"></param>
        TEntity Update(TEntity entity);
        /// <summary>
        /// 根据id获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(int id);
        /// <summary>
        /// 获取实体对象列表
        /// </summary>
        /// <param name="database">数据库序号</param>
        /// <param name="pageSize">每页的大小</param>
        /// <param name="cursor">游标</param>
        /// <param name="pageOffset">页偏移量</param>
        /// <param name="flags">标识</param>
        /// <returns></returns>
        List<TEntity> GetList(int database = 0, int pageSize = 20, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None);
        /// <summary>
        /// 提交修改
        /// </summary>
        /// <returns></returns>
        bool Commit();
    }
}
