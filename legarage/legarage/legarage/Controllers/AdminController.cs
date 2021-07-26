using legarage.Classes;
using System;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Logout
        public ActionResult LogOut()
        {
            Session["token"] = null;
            HttpCookie ck = new HttpCookie("token", "");
            ck.Expires = DateTime.Now.AddMonths(-6);
            Response.Cookies.Add(ck);
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult Login(String username, String password)
        {
            DataRow dtr = Database.FindRow("Users", "username", username);

            if (dtr != null)
            {
                if (dtr["password"].ToString() == password || dtr["password"].ToString() == Ciphering.GetMD5HashData(password))
                {
                    string Token;
                    Token = dtr["token"].ToString();
                    if (Convert.ToBoolean(Request.Params["rememberme"]) == true)
                    {
                        HttpCookie ck = new HttpCookie("token", Token);
                        ck.Expires = DateTime.Now.AddMonths(6);
                        Response.Cookies.Add(ck);
                    }
                    else
                    {
                        HttpCookie ck = new HttpCookie("token", "");
                        ck.Expires = DateTime.Now.AddMonths(-6);
                        Response.Cookies.Add(ck);
                    }
                    Session["token"] = Token;
                    Session["Username"] = dtr["username"];
                    return RedirectToAction("CPIndex", "CP");
                }
                else
                {
                    string err = "كلمة المرور غير صحيحة";
                    Session["error"] = err;
                    return View("Index");
                }
            }
            else
            {
                string err = "الحساب غير موجود في قاعدة البيانات";
                Session["error"] = err;
                return View("Index");
            }
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}