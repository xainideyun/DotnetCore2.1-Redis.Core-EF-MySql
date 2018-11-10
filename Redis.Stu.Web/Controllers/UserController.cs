using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Redis.Stu.Common;
using Redis.Stu.Model.Data;
using Redis.Stu.Repository;

namespace Redis.Stu.Web.Controllers
{
    public class UserController : BaseController<IStudentRepository>
    {
        public UserController(IStudentRepository service) : base(service)
        {
        }

        public IActionResult Index()
        {
            return View(Service.GetList());
        }
        [HttpGet]
        public IActionResult Create([FromServices]IGradeRepository gradeService)
        {
            ViewBag.Grades = gradeService.GetList();
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromBody]Student student)
        {
            Service.Add(student);
            return Json(student);
        }
        [HttpGet]
        public IActionResult Edit(int id, [FromServices]IGradeRepository gradeService)
        {
            ViewBag.Grades = gradeService.GetList();
            ViewBag.Entity = JsonConvert.SerializeObject(Service.Get(id), AppSetting.JsonSetting);
            return View();
        }
        [HttpPost]
        public IActionResult Edit([FromBody]Student student)
        {
            var entity = Service.Update(student);
            return Json(new JsonData { Success = true, Data = entity });
        }
        [HttpGet]
        public IActionResult SelectCourse([FromServices]ICourseRepository courseService)
        {
            var courses = courseService.GetList();
            var studentCourse = courseService.GetCourseIdsByStudent(Student.ID);
            var selecteds = ViewBag.courses = JsonConvert.SerializeObject(courses.Select(a => new { a.ID, a.Name, @checked = studentCourse.Contains(a.ID) }), AppSetting.JsonSetting);
            return View(Student);
        }
        [HttpPost]
        public IActionResult SaveCourse([FromServices]ICourseRepository courseService, [FromBody]IEnumerable<Course> courses)
        {
            if (courses == null) return Json("请选择课程");
            courseService.SetStudentCourses(Student, courses.Select(a => a.ID));
            return Json("ok");
        }


    }
}