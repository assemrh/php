using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models;
using SGAW_ECHO.Models.API.Address;
using SGAW_ECHO.Models.API.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SGAW_ECHO.Controllers.CP
{
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            if (Tools.FindCurrentUser())
            {
                RedirectToAction("Index", "Home");
            }
            return View("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel User)
        {
            string WebRequesUrl = "https://localhost:44301/User/Login";
            User = new LoginModel();
            User.Email = Request.Params["Email"] ?? "";
            User.Password = Request.Params["Password"] ?? "";
            User.RememberMe = Request.Params["RememberMe"] == "on" ? true : false;
            var json_ = new JavaScriptSerializer().Serialize(User);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebRequesUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json_);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<Token> user = JsonConvert.DeserializeObject<apiJson<Token>>(result);
                if (user.code == 200)
                {
                    if (User.RememberMe)
                    {
                        HttpCookie ck = new HttpCookie("token", user.data.User_Token);
                        ck.Expires = DateTime.Now.AddMonths(6);
                        Response.Cookies.Add(ck);
                    }
                    else
                    {
                        HttpCookie ck = new HttpCookie("token", user.data.User_Token);
                        ck.Expires = DateTime.Now.AddHours(1);
                        
                        Response.Cookies.Add(ck);
                    }

                    Session["token"] = user.data.User_Token;
                    RedirectToAction("Index", "Home");
                }

                ViewBag.msg = user.msg;
                return View();
            }
        }

    }
}