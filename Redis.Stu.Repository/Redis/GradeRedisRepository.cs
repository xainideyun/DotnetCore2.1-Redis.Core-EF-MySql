using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Redis.Stu.Common;
using Redis.Stu.Model.Data;
using StackExchange.Redis;

namespace Redis.Stu.Repository.Redis
{
    public class GradeRedisRepository : BaseRedisRepository<Grade, IGradeRepository>, IGradeRepository
    {
        public GradeRedisRepository(IConnectionMultiplexer cache, IGradeRepository rep) : base(cache, rep)
        {

        }

    }
}
