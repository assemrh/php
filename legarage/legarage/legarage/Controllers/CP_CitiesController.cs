using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_CitiesController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Cities/GetAll/", Add = "/CP_Cities/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Cities/GetAll/", Adding = "/CP_Cities/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string msg;
            string sql = "";
            sql += " select Provinces.id as ID , Provinces.name as CityName ,";
            sql += " Countries.name AS CountryName from Provinces ";
            sql += " inner join Countries on Provinces.country_id = Countries.id ORDER BY Provinces.created_at ASC";
            DataTable Cities = Database.ReadTableByQuery(sql, null, out msg);
            string HTML_Content = "";
            if (Cities != null && Cities.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow City in Cities.Rows)
                {
                    string ID = City["ID"].ToString();
                    HTML_Content += "<tr class='city-row'>";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + City["CountryName"].ToString() + " </td>";
                    HTML_Content += "<td> " + City["CityName"].ToString() + "</td>";
                    //Tools: 
                    HTML_Content += " <td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Cities.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Cities/Edit/')\"   data-target=\"#Modal\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Cities.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Cities/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Cities.Details + "\" style=\"color:lawngreen; cursor:pointer;\"class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Cities/Details/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_Cities.NoCity + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            CitiesModel NewCity = new CitiesModel();
            NewCity.Name = Request.Params["city"] != null ? Request.Params["city"] : "";
            NewCity.CountryId = Request.Params["country"] != null ? Request.Params["country"] : "";
            if (ISValid(NewCity, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "country_id", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { NewCity.Name, NewCity.CountryId, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Provinces", Guid.NewGuid(), cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Cities.Added });
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
            CitiesModel EditCity = new CitiesModel();
            EditCity.Name = Request.Params["city"] != null ? Request.Params["city"] : "";
            EditCity.CountryId = Request.Params["Country"] != null ? Request.Params["Country"] : "";
            EditCity.ID = new Guid(Request.Params["province_id"].ToString());
            if (ISValid(EditCity, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "country_id", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { EditCity.Name, EditCity.CountryId, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Provinces", EditCity.ID, cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Cities.Edited });
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
            CitiesModel Cities = new CitiesModel();
            DataRow City = Database.GetRow("Provinces", new Guid(ID));
            if (City != null)
            {
                Cities.Name = City["name"].ToString();
                Cities.CountryId = City["country_id"].ToString();
                Cities.ID = new Guid(City["id"].ToString());

                Cities.URL = new URLModel()
                {
                    Refresh = "/CP_Cities/GetAll/",
                    Edit = "/CP_Cities/Editing/"
                };
            }

            return PartialView(Cities);
        }

        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            CitiesModel Cities = new CitiesModel();
            DataRow City = Database.GetRow("Provinces", new Guid(ID));
            if (City != null)
            {
                Cities.Name = City["name"].ToString();
                Cities.CountryId = City["country_id"].ToString();
                Cities.ID = new Guid(City["id"].ToString());
            }

            return PartialView(Cities);
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Provinces", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Cities.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }

        }

        bool ISValid(CitiesModel City, out string msg)
        {
            bool flag = true;
            if (City.CountryId == null || City.CountryId == "-1")
            {

                msg = Resources.CP_Cities.EnterCountryPlease;
                return false;

            }
            if (City.Name == "")
            {

                msg = Resources.CP_Cities.EnterCityPlease;
                return false;

            }
            msg = "";
            return flag;
        }

    }
}