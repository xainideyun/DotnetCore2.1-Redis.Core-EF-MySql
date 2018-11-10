using Redis.Stu.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Stu.Repository
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        /// <summary>
        /// 根据年级id获取学生
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        List<Student> GetStudentsByGrade(int gradeId);
        /// <summary>
        /// 根据用户名获取学生
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Student GetStudentByUserName(string code);
        /// <summary>
        /// 获取年级总人数
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        long GetStudentCountFromGrade(int gradeId);

    }
}
