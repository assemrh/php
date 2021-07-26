using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGAW_ECHO.Classes;

namespace SGAW_ECHO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Build_Database.RebuildDatabase();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}