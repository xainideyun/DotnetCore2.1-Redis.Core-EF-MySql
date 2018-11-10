using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.Stu.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redis.Stu.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Redis.Stu.Web.Controllers
{
    public class BaseController<TService> : Controller
    {
        private const string Login_User = "Login_User";
        public Student Student { get => HttpContext.Session.Get<Student>(Login_User); }
        public TService Service { get; set; }
        public BaseController(TService service)
        {
            Service = service;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = HttpContext.Session.GetString(Login_User);
            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectResult("~/Login");
            }
            base.OnActionExecuting(context);
        }
    }
}
