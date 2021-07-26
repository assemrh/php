using System.Web.Mvc;

namespace legarage.Controllers
{
    public class AddController : BaseController
    {
        // GET: Add
        //public ActionResult Index()
        //{
        //    return View("../Sections/View");
        //}
        public ActionResult Index(string type)
        {
            if(type ==null)
                return View("../Sections/View");
            if (type == "garage")
                return PartialView("../Sections/_add_form");
            return View();
        }
    }
}