using learn_arabic.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace learn_arabic.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/admin/{Controller=ControlPanel}/{Action=Index}/{Id?}")]
    public class _BaseController : Controller
    {
        public _BaseController()
        {
            
        }
        public static IWebHostEnvironment _environment;

        public _BaseController(IWebHostEnvironment environment)
        {
            Storage.rootPath = environment.WebRootPath;
        }
        public static string lang { get; set; } 
        public static int page { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var r = context.HttpContext.Request;
            if (r.Query.ContainsKey("page"))
            {

                if (int.TryParse(r.Query["page"].FirstOrDefault(), out int p))
                    page = p;
            }
            else
                page = 1;


            //Identity and Authenticate
            if (User.Identity.IsAuthenticated)
            {
                var claims = User.Identities.First().Claims.ToList();
                ViewBag.Username = claims.FirstOrDefault(x => x.Type.Equals("User_Name", StringComparison.OrdinalIgnoreCase))?.Value;
            }

            //TODO : Config multi languages
            if (context.HttpContext.Request.Query.ContainsKey("lang"))
            {
                lang = r.Query["lang"].FirstOrDefault().ToString();
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Append("lang", lang, option);
            }
            else
                lang = r.Cookies["lang"] ?? "En";

            if (string.IsNullOrWhiteSpace(lang))
                lang = "En";

            if (lang.ToLower() == "ar")
            {
                CultureInfo c = new CultureInfo("ar-SY");
                Thread.CurrentThread.CurrentUICulture = c;
                Thread.CurrentThread.CurrentCulture = c;
            }
            else if (lang.ToLower() == "en")
            {
                CultureInfo c = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = c;
                Thread.CurrentThread.CurrentCulture = c;
            }
        }
    }
}
