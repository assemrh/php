using learn_arabic.Classes;
using learn_arabic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using m = learn_arabic.Management.Countries_Managment;


namespace learn_arabic.Controllers
{
    //[Route("admin/Countries/{Action=Index}")]
    public class CountriesController : _BaseController
    {
        public async  Task<IActionResult> Index()
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            Task<List<ShowCountryModel>> usersTask;
            List<ShowCountryModel> items;

            await(usersTask = m.Get_Countries(lang, msg));
            items = usersTask.Result.OrderBy(o => o.Name).ToList();
            ViewBag.ColTitles = new List<string>()
            {
                Resources.General.Flag,
                Resources.General.Name,
                Resources.General.Code,
                Resources.General.ISO,
            };
            ViewBag.colKeys = new List<string>()
            {
                "image",
                "Name",
                "Code",
                "ISO",
            };

            List<RowT> keyValues = new List<RowT>() { };
            foreach (var item in items)
            {
                keyValues.Add(
                    new RowT() {
                        {"Id", item.ID },
                        { "Name", item.Name },
                        { "Code", item.Code },
                        { "ISO", item.ISO },
                        { "image", "/img/CountryFlags/"+ item.URL },
                    });
            }


            ViewBag.Title = "Admin Panel - Country";
            ViewBag.TableTitle = "Country table";
            ViewBag.Img = "/img/svg/users.svg";
            ViewBag.Message = Resources.Message.NoCountry;

            return View("IndexTable", keyValues);
        }
    }
}
