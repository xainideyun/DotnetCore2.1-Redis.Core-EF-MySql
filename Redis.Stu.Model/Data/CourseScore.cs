using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Redis.Stu.Model.Data
{
    /// <summary>
    /// 学生科目分数
    /// </summary>
    [Table("CourseScore")]
    public class CourseScore : BaseEntity
    {
        public double Score { get; set; }
        public string CourseName { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }
}
