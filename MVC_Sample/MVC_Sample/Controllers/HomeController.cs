using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Sample.Services;

namespace MVC_Sample.Controllers
{
    public class Person
    {
        public string Name{ get; set; }
    }

    public class HomeController : Controller
    {
        private readonly IMemberService _service;

        public HomeController(IMemberService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(Person p)
        {
            ViewBag.Message = $"Member {p?.Name??string.Empty} Balance { _service.GetMemberBalance(123)}";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}