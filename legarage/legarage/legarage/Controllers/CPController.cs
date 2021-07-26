using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CPController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP", Add = "/CP/Add/" });
        }
        public JsonResult SetSession(string id)
        {
            Session["cp_view"] = id;
            return Json(new { });
        }


        //[HttpPost]
        //public JsonResult GetCities(string CID)
        //{
        //    string msg = "";
        //    List<SqlParameter> li = new List<SqlParameter>();
        //    li.Add(new SqlParameter("@CID", CID));
        //    DataTable cities = Database.ReadTable("Provinces", " Where country_id = @CID", li, out msg);
        //    if (msg == string.Empty)
        //    {
        //        string HTML_Content = "<option value='-1'>" + Resources.CP.Chose + " " + Resources.CP.City + "</option>";
        //        if (cities != null && cities.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in cities.Rows)
        //            {
        //                string city_name = row["country_id"].ToString();
        //                HTML_Content += "<option value=\"" + row["id"].ToString() + "\">" + row["name"] + "</option>";
        //            }

        //        }
        //        return Json(new { code = 200, data = HTML_Content });
        //    }
        //    else
        //    {
        //        return Json(new { code = 404, msg = msg });
        //    }
        //}
        [HttpPost]
        public JsonResult AddAddresses()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            UsersModel new_address = new UsersModel();
            new_address.Address.AddressId = new Guid(Request.Params["id"]); ;
            new_address.Address.AddressName = Request.Params["address"] != null ? Request.Params["address"] : "";
            if (ISAddressValid(new_address, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "province_id", "details", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_address.Address.AddressId, new_address.Address.AddressName, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Addresses", Guid.NewGuid(), cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = msg });
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
        public JsonResult Test()
        {
            if (Session["Is_Admin"] == null)
            {
                return Json(new { code = 200, Is_Admin = false });
            }
            else
            {
                return Json(new { code = 200, Is_Admin = true });
            }

        }

        bool ISAddressValid(UsersModel address, out string msg)
        {
            bool flag = true;
            if (address.Address.AddressName == "")
            {
                msg = Resources.CP.EnterAddress;
                return false;
            }
            msg = "";
            return flag;
        }


    }
}