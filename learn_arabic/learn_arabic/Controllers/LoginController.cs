using learn_arabic.Classes;
using learn_arabic.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using m = learn_arabic.Management.Users_Management;

namespace learn_arabic.Controllers
{
    [Route("User/{Action=Login}")]
    public class LoginController : Controller
    {

        public IActionResult Index(string ReturnUrl)
        {
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
                return Redirect("/admin");
            if (!string.IsNullOrWhiteSpace(ReturnUrl))
                Response.StatusCode = 401;
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login, bool rememberMe, string ReturnUrl)
        {
            ER_Ref<string> e = new ER_Ref<string>();
            Ref<TokenModel> tokenRef = new Ref<TokenModel>();
            if(await m.Admin_Login(login, e, tokenRef))
            {

                var val = tokenRef.Value;

                var clamais = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,val.ID),
                    new Claim("User_Name", val.User_Name),
                    new Claim("Token", val.Token),
                    new Claim(ClaimTypes.Role, "admin"),

                };
                var userIdentity = new ClaimsIdentity(clamais, "Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

                if (rememberMe)
                {
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTime.UtcNow.AddDays(5),
                    };
                    await HttpContext.SignInAsync(userPrincipal, properties);
                }
                else
                {
                    await HttpContext.SignInAsync(userPrincipal);
                }



                //save tokenClaimsPrincipal in cookie


                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                {
                    return Redirect(ReturnUrl);//ReturnUrl
                }
                else
                {
                    return RedirectToAction("Index", "ControlPanel");
                }
            }
            ViewBag.Error = e.Error;
            return View( login);
        }

        public async Task<IActionResult> logOut()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/user/login");
        }

        public IActionResult unauthorized()
        {
            return Unauthorized(new
            {
                Status = "unauthorized",
                error_cod = 401,
                description = "You have to log in first from ./user/login"
            });
        }
    }
}
