using Redis.Stu.Model;
using Redis.Stu.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Redis.Stu.Repository
{
    public class CourseSqlRepository : BaseSqlRepository<Course>, ICourseRepository
    {
        public CourseSqlRepository(SchoolDbContext context) : base(context)
        {
        }

        public List<Course> GetCoursesByStudent(int studentId)
        {
            return Context.StudentCourses
                .Include(a => a.Course)
                .Where(a => a.StudentId == studentId)
                .Select(a => a.Course)
                .ToList();
        }
        public List<int> GetCourseIdsByStudent(int studentId)
        {
            return Context.StudentCourses
                .Where(a => a.StudentId == studentId)
                .Select(a => a.CourseId)
                .ToList();
        }
        public void SetStudentCourses(Student student, IEnumerable<int> courses)
        {

        }
        /// <summary>
        /// 给指定的学生添加课程科目
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseIds"></param>
        public void AddStudentCourses(int studentId, params int[] courseIds)
        {
            var now = DateTime.Now;
            var studentCourses = courseIds.Select(a => new StudentCourse { CourseId = a, CreateTime = now, StudentId = studentId });
            Context.StudentCourses.AddRange(studentCourses);
            Context.SaveChanges();
        }
        /// <summary>
        /// 删除学生指定的课程科目
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseIds"></param>
        public void RemoveStudentCourses(int studentId, params int[] courseIds)
        {
            var idsText = string.Empty;
            foreach (var item in courseIds)
            {
                idsText += item + ",";
            }
            if (string.IsNullOrEmpty(idsText)) return;
            idsText = idsText.Substring(0, idsText.Length - 1);
            Context.Database.ExecuteSqlCommand("Delete From StudentCourse Where StudentID={0} And CourseID in ({1})", studentId, idsText);
        }

    }
}
