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
    public class GradeController : BaseController<IGradeRepository>
    {
        public GradeController(IGradeRepository service) : base(service)
        {
        }

        public IActionResult Index([FromServices]IStudentRepository studentRepository)
        {
            ViewBag.Grades = JsonConvert.SerializeObject(Service.GetList().Select(a => new { a.ID, a.Name, Count = studentRepository.GetStudentCountFromGrade(a.ID), Active = false }), AppSetting.JsonSetting);
            return View();
        }
        public IActionResult GetStudents(int id, [FromServices]IStudentRepository studentRepository)
        {
            return Json(new JsonData { Data = studentRepository.GetStudentsByGrade(id), Success = true });
        }
    }
}