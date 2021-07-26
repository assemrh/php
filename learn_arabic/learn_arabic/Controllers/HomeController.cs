using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using learn_arabic.Models;
using learn_arabic.Classes;
using Microsoft.AspNetCore.Hosting;

namespace learn_arabic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static IWebHostEnvironment _environment;

        
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
              Build_Database.RebuildDatabase();
           // Build_Database.Remove_Constants();
           // Build_Database.Add_Constants();
           // Build_Database.Add_Constants();
            //Storage.rootPath = _environment.WebRootPath;
            //CountryProcreses.Initialization(new string[] { "AR", "EN", "TR", "RU" });//C:\websites\learn_arabic\learn_arabic\wwwroot\
            // Build_Database.Remove_Constants();
          //  Build_Database.Add_Constants();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("/{*url}", Order = 999)]
        public IActionResult Page_Not_Found()
        {
            Response.StatusCode = 404;
            return View(new
            {
                Status = "Not Found",
                error_cod = 404,
                description = "The Url You Ask For Is Not Found"
            });
        }
    }
}
