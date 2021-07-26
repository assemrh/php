using SGAW_ECHO.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SGAW_ECHO.Controllers
{
    public class BaseController : Controller
    {

        public Guid UserID = Guid.Empty;
        public static string BaseUrl = "https://localhost:44301";

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            HttpCookie ck = Request.Cookies.Get("legarage_lng");
            if (ck != null && ck.Value == "ar")
            {
                Session["lng"] = "ar";
            }
            else if(ck != null && ck.Value == "tr")
            {
                Session["lng"] = "tr";
            }
            else
            {
                Session["lng"] = "en";
            }

            if (Session["lng"].ToString() == "ar")
            {
                CultureInfo c = new CultureInfo("ar-SY");
                Thread.CurrentThread.CurrentUICulture = c;
                Thread.CurrentThread.CurrentCulture = c;
            }
            else if(Session["lng"].ToString() == "tr")
            {
                CultureInfo c = new CultureInfo("tr-TR");
                Thread.CurrentThread.CurrentUICulture = c;
                Thread.CurrentThread.CurrentCulture = c;
            }
            else
            {
                CultureInfo c = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = c;
                Thread.CurrentThread.CurrentCulture = c;
            }


        }
    }
}