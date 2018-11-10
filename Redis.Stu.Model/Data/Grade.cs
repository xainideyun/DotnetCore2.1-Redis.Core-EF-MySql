using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Redis.Stu.Model.Data
{
    /// <summary>
    /// 年级
    /// </summary>
    [Table("Grade")]
    public class Grade : BaseEntity
    {
        [DisplayName("年级")]
        public string Name { get; set; }
        /// <summary>
        /// 年级学生
        /// </summary>
        public virtual ICollection<Student> Students { get; set; }
    }
}
