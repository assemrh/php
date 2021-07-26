using learn_arabic.Classes;
using learn_arabic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using m = learn_arabic.Management.Users_Management;


namespace learn_arabic.Controllers
{
    //[Route("admin/user/{Action=Index}/{Id?}")]
    public class UserController : _BaseController
    {
        public async Task<IActionResult> Index()
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            Task<List<UserModel>> usersTask;
            List<UserModel> users;

            await (usersTask = m.GetAllUsers(lang, msg));
            users = usersTask.Result;
            //ViewBag.TotalItems = users.ItemsCount;

            ViewBag.ColTitles = new List<string>() 
            {
                Resources.General.Country,
               // Resources.General.DOB,
                Resources.General.Email,
                Resources.General.FullName,
                Resources.General.Gender,
                Resources.General.Phone,
            };
            ViewBag.colKeys = new List<string>() 
            { 
                "Country",
                "Email",
                "FullName",
                "Gender",
                "Phone",
            };

            List<RowT> keyValues = new List<RowT>() {};
            foreach(var item in users)
            {
                keyValues.Add(
                    new RowT() { 
                        {"Id","000000" },
                        { "Country", item.Country },
                        { "Email", item.Email },
                        { "FullName", item.FullName },
                        { "Gender", item.Gender },
                        { "Phone", item.Phone },
                    });
            }

            ViewBag.Title = "Admin Panel - Users";
            ViewBag.TableTitle = "Users table";
            ViewBag.Img = "/img/svg/users.svg";
            ViewBag.Message = Resources.Message.NoUser;

            return View("IndexTable", keyValues);
        }

    }


}
