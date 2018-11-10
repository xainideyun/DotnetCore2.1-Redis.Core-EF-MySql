using Redis.Stu.Common;
using Redis.Stu.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.Stu.Repository
{
    /*   数据存储格式：
            1. 表格：Table:[TableName]:[Id] -> [JSON]
            2. 列表ID记录：List:[TableName] -> [List]
            3. 一对多关系记录：Map:[TableName]:[Id]:[TableName] -> [SortedSet]
            4. 属性查询记录：Table:[TableName]:[PropertyName]:[Id] -> [Id]
            5. 多对多关系记录：与一对多相同
            6. 其他无关数据库表的建：Other:[Key] -> [Value]
    */

    public class BaseRedisRepository<TEntity, TSql> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
        where TSql : IBaseRepository<TEntity>
    {
        /// <summary>
        /// 类型是否已经初始化
        /// </summary>
        private static bool _isInit = false;
        public IConnectionMultiplexer Cache { get; }
        public TSql Repository { get; }
        public BaseRedisRepository(IConnectionMultiplexer cache, TSql repository)
        {
            Cache = cache;
            Repository = repository;
            if (!_isInit)
            {
                Init();
            }
        }
        public IDatabase Database { get => Cache.GetDatabase(); }

        public string KeyForId(int id) => $"Entity:{typeof(TEntity).Name}:{id}";
        protected RedisKey[] KeysForId(IEnumerable<int> ids) => ids.Select(a => (RedisKey)KeyForId(a)).ToArray();
        public string KeyForSortedSet() => $"SortedSet:{typeof(TEntity).Name}";
        /// <summary>
        /// 初始化Redis数据库
        /// </summary>
        protected virtual void Init()
        {
            var listKey = KeyForSortedSet();
            if (!Database.KeyExists(listKey))
            {
                // 表格写入Redis
                var list = Repository.GetList();
                var tasks = new List<Task>();
                var keyPairs = list.Select(item => new KeyValuePair<RedisKey, TEntity>(KeyForId(item.ID), item)).ToArray();
                Database.ObjectSet(keyPairs);
                // 表格ID映射写入Redis
                Database.SortedSetAdd(listKey, list.Select(a => new SortedSetEntry(a.ID, a.ID)).ToArray());
            }
            _isInit = true;
        }


        public void Add(TEntity entity)
        {
            entity.CreateTime = DateTime.Now;
            Repository.Add(entity);
            Database.ObjectSet(KeyForId(entity.ID), entity);
            //Database.ListRightPush(KeyForList(), entity.ID);
            Database.SortedSetAdd(KeyForSortedSet(), entity.ID, entity.ID);
        }
        public void Add(IEnumerable<TEntity> entitys)
        {
            foreach (var item in entitys)
            {
                item.CreateTime = DateTime.Now;
            }
            Repository.Add(entitys);
            var keys = entitys.Select(entity => new KeyValuePair<RedisKey, TEntity>(KeyForId(entity.ID), entity)).ToArray();
            Database.ObjectSet(keys);
            //Database.ListRightPush(KeyForSortedSet(), entitys.Select(a => a.ID));
            Database.SortedSetAdd(KeyForSortedSet(), entitys.Select(a => new SortedSetEntry(a.ID, a.ID)).ToArray());
        }
        public TEntity Get(int id)
        {
            var key = KeyForId(id);
            var obj = Database.ObjectGet<TEntity>(key);
            if (obj == null)
            {
                obj = Repository.Get(id);
                if (obj == null) return null;
                Database.ObjectSet(key, obj);
            }
            return obj;
        }
        public void Remove(TEntity entity)
        {
            Repository.Remove(entity);
            var key = KeyForId(entity.ID);
            Database.KeyDelete(key);
            Database.ListRemove(KeyForSortedSet(), entity.ID);
        }
        public void Remove(IEnumerable<TEntity> entitys)
        {
            Repository.Remove(entitys);
            Database.KeyDelete(entitys.Select(a => (RedisKey)KeyForId(a.ID)).ToArray());
        }
        public TEntity Update(TEntity entity)
        {
            Repository.Update(entity);
            Database.ObjectSet(KeyForId(entity.ID), entity);
            return entity;
        }
        public List<TEntity> GetList(int database = 0, int pageSize = 20, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            var key = KeyForSortedSet();
            if (!Database.KeyExists(key))
            {
                var list = Repository.GetList();
                //Database.ListRightPush(key, list.Select(a => (RedisValue)a.ID).ToArray());
                Database.SortedSetAdd(key, list.Select(a => new SortedSetEntry(a.ID, a.ID)).ToArray());
            }
            //var ids = Database.ListRange(key, pageSize * cursor, pageSize * (cursor + 1));
            var ids = Database.SortedSetRangeByScore(key, pageSize * cursor, pageSize * (cursor + 1));
            var keys = ids.Select(a => (RedisKey)KeyForId((int)a)).ToArray();
            return Database.ObjectGet<TEntity>(keys);
        }
        public bool Commit()
        {
            return Repository.Commit();
        }
    }
}
