using Redis.Stu.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redis.Stu.Repository
{
    public class BaseSqlRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        public SchoolDbContext Context { get; }

        public BaseSqlRepository(SchoolDbContext context)
        {
            Context = context;
        }
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }
        public void Add(IEnumerable<TEntity> entitys)
        {
            Context.Set<TEntity>().AddRange(entitys);
            Context.SaveChanges();
        }
        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().FirstOrDefault(a => a.ID == id);
        }
        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            Context.SaveChanges();
        }
        public void Remove(IEnumerable<TEntity> entitys)
        {
            Context.Set<TEntity>().RemoveRange(entitys);
            Context.SaveChanges();
        }
        public TEntity Update(TEntity entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
            return entity;
        }
        public List<TEntity> GetList(int database = 0, int pageSize = 20, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Set<TEntity>().ToList();
        }
        public bool Commit()
        {
            return Context.SaveChanges() > 0;
        }
    }
}
