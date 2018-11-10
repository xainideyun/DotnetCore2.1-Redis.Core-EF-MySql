using Redis.Stu.Model;
using Redis.Stu.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redis.Stu.Repository
{
    public class GradeSqlRepository : BaseSqlRepository<Grade>, IGradeRepository
    {
        public GradeSqlRepository(SchoolDbContext context) : base(context)
        {
        }

    }
}
