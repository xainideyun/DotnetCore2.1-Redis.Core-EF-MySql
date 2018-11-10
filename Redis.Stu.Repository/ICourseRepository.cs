using Redis.Stu.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Stu.Repository
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        /// <summary>
        /// 根据学生id获取选择的课程
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        List<Course> GetCoursesByStudent(int studentId);
        /// <summary>
        /// 根据学生id获取选择课程的id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        List<int> GetCourseIdsByStudent(int studentId);
        /// <summary>
        /// 设置学生选择的课程
        /// </summary>
        /// <param name="student"></param>
        /// <param name="courses"></param>
        void SetStudentCourses(Student student, IEnumerable<int> courses);

    }
}
