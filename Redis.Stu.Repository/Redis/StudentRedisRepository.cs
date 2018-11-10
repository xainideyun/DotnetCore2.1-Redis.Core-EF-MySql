using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Redis.Stu.Common;
using Redis.Stu.Model.Data;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;

namespace Redis.Stu.Repository.Redis
{
    public class StudentRedisRepository : BaseRedisRepository<Student, IStudentRepository>, IStudentRepository
    {
        public StudentRedisRepository(IConnectionMultiplexer cache, IStudentRepository rep) : base(cache, rep)
        {

        }

        public string KeyForStudentsByGrade(int gradeId) => $"Map:Grade:{gradeId}:Student";
        public string KeyForStudentPropertyCode(string code) => $"Property:Student:{code}:id";

        //protected override void Init()
        //{
        //    base.Init();
        //    //// 判断学生表根据属性code映射的Redis列表是否已经缓存，取列表的第一个元素判断
        //    //var keys = Database.SortedSetRangeByScore(KeyForSortedSet(), 0, 0);
        //    //if (keys == null || keys.Length == 0) return;
        //    //var key = keys.First();
        //    //var student = Database.ObjectGet<Student>(KeyForId((int)key));
        //    //if (Database.KeyExists(KeyForStudentPropertyCode(student.Code))) return;
        //    //// 没有缓存则写入
        //    //var ids = Database.ListRange(KeyForSortedSet()).Select(a => (RedisKey)KeyForId((int)a)).ToArray();
        //    //var students = Database.ObjectGet<Student>(ids);
        //    //var keyPairs = students.Select(item => new KeyValuePair<RedisKey, RedisValue>(KeyForStudentPropertyCode(item.Code), student.ID)).ToArray();
        //    //Database.StringSet(keyPairs);
        //    // 管道线
        //    //var tasks = new List<Task>();
        //    //foreach (var item in students)
        //    //{
        //    //    tasks.Add(db.ObjectSetAsync(KeyForStudentPropertyCode(item.Code), item));
        //    //}
        //    //Task.WaitAll(tasks.ToArray());

        //}

        public new void Add(Student student)
        {
            base.Add(student);
            var keyGrade = KeyForStudentsByGrade(student.GradeId);
            Database.StringSet(KeyForStudentPropertyCode(student.Code), student.ID);    // 向Redis中添加用户名键
            AddStudentToGrade(student);                                                 // 将学生写入年纪对应关系
        }
        public new Student Update(Student student)
        {
            var entity = Get(student.ID);
            var oldGradeId = entity.GradeId;
            entity.Age = student.Age;
            entity.GradeId = student.GradeId;
            entity.Name = student.Name;
            entity.Remark = student.Remark;
            entity.Sex = student.Sex;
            base.Update(entity);
            Database.ObjectSet(KeyForId(entity.ID), entity);
            if (oldGradeId != student.GradeId)              // 如果年级改变了，则修改映射关系
            {
                RemoveStudentFromGrade(oldGradeId, entity);
                AddStudentToGrade(entity);
            }
            return entity;
        }
        public List<Student> GetStudentsByGrade(int gradeId)
        {
            var key = KeyForStudentsByGrade(gradeId);
            var ids = Database.SortedSetRangeByScore(key);
            if (ids == null || ids.Length == 0)
            {
                return SetStudentsToGrade(gradeId);
            }
            var keys = ids.Select(a => (RedisKey)KeyForId((int)a)).ToArray();
            return Database.ObjectGet<Student>(keys);
        }
        public Student GetStudentByUserName(string code)
        {
            var key = KeyForStudentPropertyCode(code);
            var id = Database.StringGet(key);
            if (!id.HasValue)
            {
                var student = Repository.GetStudentByUserName(code);
                if (student == null) return null;
                Database.StringSet(key, student.ID);
                return student;
            }
            return Database.ObjectGet<Student>(KeyForId((int)id));
        }
        public long GetStudentCountFromGrade(int gradeId)
        {
            return Database.SortedSetLength(KeyForStudentsByGrade(gradeId));
        }

        /// <summary>
        /// 向Student/Grade集合中添加对象
        /// </summary>
        /// <param name="student"></param>
        private void AddStudentToGrade(Student student)
        {
            var key = KeyForStudentsByGrade(student.GradeId);
            if (!Database.KeyExists(key))
            {
                SetStudentsToGrade(student.GradeId);
            }
            Database.SortedSetAdd(key, student.ID, student.ID);
        }
        /// <summary>
        /// 将学生从指定的年级映射中移除
        /// </summary>
        /// <param name="oldGradeId"></param>
        /// <param name="student"></param>
        private void RemoveStudentFromGrade(int oldGradeId, Student student)
        {
            var key = KeyForStudentsByGrade(oldGradeId);
            if (!Database.KeyExists(key)) return;
            Database.SortedSetRemove(key, student.ID);
        }
        /// <summary>
        /// 初始化Student/Grade集合对象
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        private List<Student> SetStudentsToGrade(int gradeId)
        {
            var key = KeyForStudentsByGrade(gradeId);
            var list = Repository.GetStudentsByGrade(gradeId);
            if (list.Count == 0) return list;
            // 缓存student与grade的一对多关系，使用id排序
            var sets = list.Select(item => new SortedSetEntry(item.ID, item.ID)).ToArray();
            Database.SortedSetAdd(key, sets);
            return list;
        }

    }
}
