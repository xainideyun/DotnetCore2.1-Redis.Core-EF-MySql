using Redis.Stu.Model;
using Redis.Stu.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redis.Stu.Repository
{
    public class StudentSqlRepository : BaseSqlRepository<Student>, IStudentRepository
    {
        public StudentSqlRepository(SchoolDbContext context) : base(context)
        {
        }

        public List<Student> GetStudentsByGrade(int gradeId)
        {
            return Context.Students.Where(a => a.GradeId == gradeId).ToList();
        }
        public Student GetStudentByUserName(string code)
        {
            return Context.Students.SingleOrDefault(a => a.Code.Equals(code));
        }
        public long GetStudentCountFromGrade(int gradeId)
        {
            return Context.Students.Count(a => a.GradeId == gradeId);
        }

    }
}
