using SGAW_ECHO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SGAW_ECHO.Controllers.CP
{
    public class CPController : BaseController
    {
        public JsonResult GetCountries()
        {
            string msg = "";
            DataTable countries = Database.ReadTable("Countries", out msg);
            if (countries != null && countries.Rows.Count > 0)
            {
                string HTML_Content = "<option value='"+(new Guid()).ToString()+"'>" + "Choose Country" + "</option>";                
                foreach (DataRow row in countries.Rows)
                {
                    string country_name = row["key_"].ToString();
                    HTML_Content += "<option value=\"" + row["id"].ToString() + "\">" + country_name + "</option>";                    
                }
                return Json(new { @code = 200, @data = HTML_Content});
            }
            else
            {
                return Json(new { @code = 404, @msg = msg });
            }
        }

        [HttpPost]
        public JsonResult GetCities(string country_id)
        {
            string msg = "";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@country_id", country_id));
            DataTable cities = Database.ReadTable("Cities", " Where country_id = @country_id", li, out msg);
            if (cities != null && cities.Rows.Count > 0)
            {
                string HTML_Content = "<option value='"+(new Guid()).ToString()+"'>" + "Choose City" + "</option>";
                foreach (DataRow row in cities.Rows)
                {
                    string city_name = row["city_name"].ToString();
                    HTML_Content += "<option value=\"" + row["id"].ToString() + "\">" + city_name + "</option>";
                }

                return Json(new { @code = 200, @data = HTML_Content });
            }
            else
            {
                string HTML_Content = "<option value='-1'>" + "No Cities" + "</option>";
                return Json(new { @code = 404, @data = HTML_Content });
            }
        }

        [HttpPost]
        public JsonResult GetNeighborhoods(string city_id)
        {
            string msg = "";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@city_id", city_id));
            DataTable cities = Database.ReadTable("Neighborhoods", " Where city_id = @city_id", li, out msg);
            if (cities != null && cities.Rows.Count > 0)
            {
                string HTML_Content = "<option value='-1'>" + "Choose Neighborhood" + "</option>";
                foreach (DataRow row in cities.Rows)
                {
                    string city_name = row["neighborhood_name"].ToString();
                    HTML_Content += "<option value=\"" + row["id"].ToString() + "\">" + city_name + "</option>";
                }

                return Json(new { @code = 200, @data = HTML_Content });
            }
            else
            {
                string HTML_Content = "<option value='-1'>" + "No Neighborhoods" + "</option>";
                return Json(new { @code = 404, @data = HTML_Content });
            }
        }
    }
}