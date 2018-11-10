using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.Stu.Common
{
    public static class RedisExtensions
    {
        private static JsonSerializerSettings _jsonSetting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        public static bool ObjectSet<T>(this IDatabase database, RedisKey key, T obj, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (obj == null)
            {
                throw new NullReferenceException("设置Redis对象引用为空");
            }
            return database.StringSet(key, JsonConvert.SerializeObject(obj, _jsonSetting), expiry, when, flags);
        }
        public static async Task<bool> ObjectSetAsync<T>(this IDatabase database, RedisKey key, T obj, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            if (obj == null)
            {
                throw new NullReferenceException("设置Redis对象引用为空");
            }
            return await database.StringSetAsync(key, JsonConvert.SerializeObject(obj, _jsonSetting), expiry, when, flags);
        }


        public static bool ObjectSet<T>(this IDatabase database, KeyValuePair<RedisKey, T>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var list = values.Select(a => new KeyValuePair<RedisKey, RedisValue>(a.Key, JsonConvert.SerializeObject(a.Value, _jsonSetting))).ToArray();
            return database.StringSet(list, when, flags);
        }
        public static async Task<bool> ObjectSetAsync<T>(this IDatabase database, KeyValuePair<RedisKey, T>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            var list = values.Select(a => new KeyValuePair<RedisKey, RedisValue>(a.Key, JsonConvert.SerializeObject(a.Value, _jsonSetting))).ToArray();
            return await database.StringSetAsync(list, when, flags);
        }

        public static T ObjectGet<T>(this IDatabase database, string key, CommandFlags flags = CommandFlags.None)
        {
            var value = database.StringGet(key, flags);
            if (value.IsNull) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static async Task<T> ObjectGetAsync<T>(this IDatabase database, string key, CommandFlags flags = CommandFlags.None)
        {
            var value = await database.StringGetAsync(key, flags);
            if (value.IsNull) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static List<T> ObjectGet<T>(this IDatabase database, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            var value = database.StringGet(keys, flags);
            if (value == null) return null;
            return value.Select(a => JsonConvert.DeserializeObject<T>(a)).ToList();
        }
        public static async Task<List<T>> ObjectGetAsync<T>(this IDatabase database, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            var value = await database.StringGetAsync(keys, flags);
            if (value == null) return null;
            return value.Select(a => JsonConvert.DeserializeObject<T>(a)).ToList();
        }

        public static long ListRightPush<T>(this IDatabase database, RedisKey key, IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            var list = values.Select(a => (RedisValue)JsonConvert.SerializeObject(a, _jsonSetting)).ToArray();
            return database.ListRightPush(key, list, flags);
        }
        public static async Task<long> ListRightPushAsync<T>(this IDatabase database, RedisKey key, IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            var list = values.Select(a => (RedisValue)JsonConvert.SerializeObject(a, _jsonSetting)).ToArray();
            return await database.ListRightPushAsync(key, list, flags);
        }


    }
}
