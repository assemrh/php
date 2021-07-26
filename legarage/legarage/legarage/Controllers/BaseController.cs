using legarage.Classes;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace legarage.Controllers
{
    public class BaseController : Controller
    {
        public Guid UserID = Guid.Empty;

        
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            HttpCookie ck = Request.Cookies.Get("legarage_lng");
            if (ck == null || ck.Value == "en")
            {
                Session["lng"] = "en";
            }
            else
            {
                Session["lng"] = "ar";
            }

            if (Session["lng"].ToString() == "ar")
            {
                CultureInfo c = new CultureInfo("ar-SY");
                Thread.CurrentThread.CurrentUICulture = c;
                Thread.CurrentThread.CurrentCulture = c;
            }
            else
            {
                CultureInfo c = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = c;
                Thread.CurrentThread.CurrentCulture = c;
            }

            if (Session["token"] == null)
            {
                HttpCookie cktoken = Request.Cookies.Get("token");
                if (cktoken != null && cktoken.Value.Trim() != "")
                {
                    Session["token"] = cktoken.Value;
                }
            }

            if ((Session["market"]??"").ToString().Trim() == string.Empty || (Session["market"] ?? "").ToString().Trim() == "-1")
            {
                Session["market"] = "all";
            }

            if (Session["market_name"] == null)
            {
                Session["market_name"] = "all";
            }

            System.Data.DataRow rUSer = null;
            Tools.FindCurrentUser(out rUSer);
            //!Request.Url.AbsolutePath.ToLower().Contains("DropAttachments".ToLower()
            if (Request.Url.AbsolutePath.ToLower().Contains("/cp") )
            {
                if (rUSer == null)
                   // Go_TO_Index();
                 Response.Redirect("~/",true);
                else
                {
                    Session["Is_Admin"] = rUSer["is_admin"].ToString() == "1" ? "1" : "0";
                }
            }
            

            if (Request.HttpMethod.ToLower() == "get")
            {
                Session["Attachment"] = null;
                Session["Attachment_File_Name"] = null;
                Session["Primary_Attachment"] = null;
                Session["Primary_Attachment_File_Name"] = null;
                Session["Secondary_Attachment"] = null;
                Session["Secondary_Attachment_File_Name"] = null;
            }

        }
    }
}