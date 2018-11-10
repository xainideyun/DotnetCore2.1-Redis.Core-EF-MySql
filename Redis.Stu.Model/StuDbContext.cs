using Microsoft.EntityFrameworkCore;
using Redis.Stu.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Stu.Model
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var now = DateTime.Now;
            modelBuilder.Entity<Grade>().HasData(
                new Grade { ID = 1, CreateTime = now, Name = "一年级" },
                new Grade { ID = 2, CreateTime = now, Name = "二年级" },
                new Grade { ID = 3, CreateTime = now, Name = "三年级" }
                );
            modelBuilder.Entity<Student>().HasData(
                new Student { ID = 1, Age = 20, Code = "sunquan", Name = "孙权", GradeId = 1, CreateTime = now, Password = "000000", Sex = 1 },
                new Student { ID = 2, Age = 20, Code = "liubei", Name = "刘备", GradeId = 1, CreateTime = now, Password = "000000", Sex = 1 },
                new Student { ID = 3, Age = 20, Code = "caocao", Name = "曹操", GradeId = 1, CreateTime = now, Password = "000000", Sex = 1 }
                );
            modelBuilder.Entity<Course>().HasData(
                new Course { ID = 1, CreateTime = now, Name = "语文" },
                new Course { ID = 2, CreateTime = now, Name = "数学" },
                new Course { ID = 3, CreateTime = now, Name = "英语" },
                new Course { ID = 4, CreateTime = now, Name = "物理" }
                );
        }

    }
}
