using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Redis.Stu.Model.Data
{
    /// <summary>
    /// 学生选择的课程
    /// </summary>
    [Table("StudentCourse")]
    public class StudentCourse : BaseEntity
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }
}
