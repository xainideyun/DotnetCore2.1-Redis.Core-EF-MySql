using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Redis.Stu.Model.Data
{
    /// <summary>
    /// 课程
    /// </summary>
    [Table("Course")]
    public class Course : BaseEntity
    {
        [DisplayName("课程名称")]
        public string Name { get; set; }

    }
}
