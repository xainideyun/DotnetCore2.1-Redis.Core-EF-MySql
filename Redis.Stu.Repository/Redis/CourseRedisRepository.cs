using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Redis.Stu.Common;
using Redis.Stu.Model.Data;
using StackExchange.Redis;
using System.Linq;

namespace Redis.Stu.Repository.Redis
{
    public class CourseRedisRepository : BaseRedisRepository<Course, CourseSqlRepository>, ICourseRepository
    {
        public CourseRedisRepository(IConnectionMultiplexer cache, CourseSqlRepository rep) : base(cache, rep)
        {

        }

        public string KeyForCoursesByStudent(int studentId) => $"Map:Student:{studentId}:Course";

        public List<Course> GetCoursesByStudent(int studentId)
        {
            var ids = GetCourseIdsByStudent(studentId);
            if (ids == null || ids.Count == 0) return null;
            return Database.ObjectGet<Course>(KeysForId(ids));
        }
        public List<int> GetCourseIdsByStudent(int studentId)
        {
            var key = KeyForCoursesByStudent(studentId);
            var ids = Database.SortedSetRangeByScore(key);
            if (ids == null || ids.Length == 0)
            {
                var list = Repository.GetCourseIdsByStudent(studentId);
                if (list.Count == 0) return list;
                var set = list.Select(a => new SortedSetEntry(a, a)).ToArray();
                Database.SortedSetAdd(key, set);
                return list;
            }
            return ids.Select(a => (int)a).ToList();
        }
        public void SetStudentCourses(Student student, IEnumerable<int> courses)
        {
            var selected = GetCourseIdsByStudent(student.ID);                   // 已选择的课程
            var newCourse = courses.Where(a => !selected.Contains(a));          // 新选择的课程
            var removeCourse = selected.Where(a => !courses.Contains(a));       // 删除的课程
            Repository.AddStudentCourses(student.ID, newCourse.ToArray());
            Repository.RemoveStudentCourses(student.ID, removeCourse.ToArray());
            var key = KeyForCoursesByStudent(student.ID);
            Database.KeyDelete(key);
            Database.SortedSetAdd(key, courses.Select(a => new SortedSetEntry(a, a)).ToArray());
        }

    }
}
