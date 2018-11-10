using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redis.Stu.Common;
using Redis.Stu.Model.Data;
using Redis.Stu.Repository;

namespace Redis.Stu.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login([FromServices]IStudentRepository service, [FromQuery]string username,[FromQuery]string password)
        {
            username = username.Trim();
            var student = service.GetStudentByUserName(username);
            if (student == null) return Json(new JsonData { Msg = "用户不存在" });
            if (!student.Password.Equals(password)) return Json(new JsonData { Msg = "密码错误" });
            SaveSession(student);
            return Json(new JsonData { Msg = "登录成功", Success = true });
        }

        private void SaveSession(Student student)
        {
            HttpContext.Session.Set("Login_User", student);
        }
    }
}