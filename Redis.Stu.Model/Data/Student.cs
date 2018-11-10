using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Redis.Stu.Model.Data
{
    /// <summary>
    /// 学生表
    /// </summary>
    [Table("Student")]
    public class Student : BaseEntity
    {
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("年龄")]
        public int Age { get; set; }
        [DisplayName("编号")]
        public string Code { get; set; }
        [DisplayName("性别")]
        public int Sex { get; set; }
        [DisplayName("备注")]
        public string Remark { get; set; }
        [DisplayName("密码")]
        public string Password { get; set; }
        /// <summary>
        /// 所属年级ID
        /// </summary>
        public int GradeId { get; set; }
        /// <summary>
        /// 所属年级对象
        /// </summary>
        public virtual Grade Grade { get; set; }
    }
}
