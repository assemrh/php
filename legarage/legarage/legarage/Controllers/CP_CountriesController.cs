using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_CountriesController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Countries/GetAll/", Add = "/CP_Countries/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Countries/GetAll/", Adding = "/CP_Countries/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string msg;
            DataTable Countries = Database.ReadTable("Countries", " where is_factory = 1 or is_market =1 ", null, out msg);

            string HTML_Content = "";
            if (Countries != null && Countries.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow country in Countries.Rows)
                {
                    int ismarket_n = 0;
                    int isfactory_n = 0;
                    int.TryParse(country["is_market"].ToString(), out ismarket_n);
                    int.TryParse(country["is_factory"].ToString(), out isfactory_n);
                    string ismarket = ismarket_n == 1 ? "yes" : "No";
                    string isfactory = isfactory_n == 1 ? "yes" : "No";
                    string ID = country["ID"].ToString();
                    HTML_Content += "<tr class='country-row'>";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + country["name"].ToString() + "</td>";
                    HTML_Content += "<td> " + country["Phone_key"].ToString() + " </td>";
                    HTML_Content += "<td> " + ismarket + " </td>";
                    HTML_Content += "<td> " + isfactory + " </td>";
                    //Tools: 
                    HTML_Content += " <td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Countries.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Countries/Edit/')\"   data-target=\"#Modal\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Countries.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Countries/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Countries.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Countries/Details/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_Countries.NoCountry + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            CountriesModel new_contry = new CountriesModel();
            new_contry.ID = Request.Params["country"] != null ? new Guid(Request.Params["country"]):new Guid();
            //new_contry.Country = Request.Params["Country"] != null ? Request.Params["Country"] : "";
            //new_contry.Code = Request.Params["phone_key"] != null ? Request.Params["phone_key"] : "";
            new_contry.IsMarket = Convert.ToInt32(Request.Params["is_market"]);
            new_contry.IsFactory = Convert.ToInt32(Request.Params["is_factory"]);
            if (ISValid(new_contry, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "is_factory","is_market", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_contry.IsFactory, new_contry.IsMarket, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Countries", new_contry.ID , cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Countries.Added });
                }
                else
                {
                    code = 404;
                    msg = "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                    return Json(new { code = code.ToString(), msg = msg });
                }
            }
            else
            {
                code = 404;
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            CountriesModel EditCountry = new CountriesModel();
            EditCountry.ID = new Guid(Request.Params["CountryId"]);
            Object o = Request.Params["is_market"];
            EditCountry.IsMarket = Convert.ToInt32(Request.Params["is_market"]);
            EditCountry.IsFactory= Convert.ToInt32(Request.Params["is_factory"]);
            if (ISValid(EditCountry, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "is_market", "is_factory", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { EditCountry.IsMarket, EditCountry.IsFactory, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Countries", EditCountry.ID, cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Countries.Edited });
                }
                else
                {
                    code = 404;
                    msg = "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                    return Json(new { code = code.ToString(), msg = msg });
                }
            }
            else
            {
                code = 404;
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        [HttpPost]
        public PartialViewResult Edit(string ID)
        {
            CountriesModel country = new CountriesModel();
            DataRow country_ = Database.GetRow("Countries", new Guid(ID));
            if (country_ != null)
            {
                int ismarket_n = 0;
                int isfactory_n = 0;
                int.TryParse(country_["is_market"].ToString(), out ismarket_n);
                int.TryParse(country_["is_factory"].ToString(), out isfactory_n);
                country.Name = country_["Name"].ToString();
                country.Code = country_["Phone_key"].ToString();
                country.ID = new Guid(country_["ID"].ToString());
                country.IsMarket = ismarket_n;
                country.IsFactory = isfactory_n;

                country.URL = new URLModel
                {
                    Refresh = "/CP_Countries/GetAll/",
                    Edit = "/CP_Countries/Editing/"
                };
            }
            return PartialView(country);
        }

        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            CountriesModel country = new CountriesModel();
            DataRow country_ = Database.GetRow("Countries", new Guid(ID));
            if (country_ != null)
            {
                int ismarket_n = 0; 
                int isfactory_n = 0;
                int.TryParse(country_["is_market"].ToString(), out ismarket_n);
                int.TryParse(country_["is_factory"].ToString(), out isfactory_n);
                country.Name = country_["Name"].ToString();
                country.Code = country_["Phone_key"].ToString();
                country.ID = new Guid(country_["ID"].ToString());
                country.IsMarket = ismarket_n;
                country.IsFactory = isfactory_n;
            }

            return PartialView(country);
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();
            string[] colsinput = { "is_factory", "is_market", "updated_at" };
            cols.AddRange(colsinput);
            object[] valsinput = { 0, 0, DateTime.Now };
            vals.AddRange(valsinput);
            string msg = "";
            int code = 0;
            if (Database.UpdateRow("Countries", new Guid(ID), cols, vals, out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Countries.Deleted });
            }
            else
            {
                code = 404;
                msg = "fail" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        bool ISValid(CountriesModel country, out string msg)
        {
            bool flag = true;
            if (country.ID == new Guid())
            {
                msg = Resources.CP_Countries.EnterCountryPlease;
                return false;
            }

            if (country.IsMarket == -1)
            {
                msg = Resources.CP_Countries.IsMarket;
                return false;
            }


            if (country.IsFactory == -1)
            {
                msg = Resources.CP_Countries.IsFactory;
                return false;
            }

            msg = "";
            return flag;
        }

    }
}