using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_VehicleTypesController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_VehicleTypes/GetAll/", Add = "/CP_VehicleTypes/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_VehicleTypes/GetAll/", Adding = "/CP_VehicleTypes/Adding/" });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            VehicleTypesModel new_vehicle = new VehicleTypesModel();
            new_vehicle.Name = Request.Params["vehicle"] != null ? Request.Params["vehicle"] : "";
            if (ISValid(new_vehicle, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "type_name", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_vehicle.Name, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Vehicle_Types", Guid.NewGuid(), cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_VehicleTypes.Added });
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
        public JsonResult GetAll()
        {
            string msg;
            DataTable VehicleTypes = Database.ReadTable("Vehicle_Types", "ORDER BY created_at ASC", null, out msg);
            string HTML_Content = "";
            if (VehicleTypes != null && VehicleTypes.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow VehicleType in VehicleTypes.Rows)
                {
                    string ID = VehicleType["ID"].ToString();
                    HTML_Content += "<tr class=\"vehicle-type-row \">";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + VehicleType["type_name"].ToString() + "</td>";
                    //Tools: 
                    HTML_Content += " <td>";
                    HTML_Content += "<i title = \"" + Resources.CP_VehicleTypes.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_VehicleTypes/Edit/')\"   data-target=\"#Modal\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_VehicleTypes.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_VehicleTypes/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_VehicleTypes.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_VehicleTypes/Detail/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_VehicleTypes.NoVehicle + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            VehicleTypesModel new_vehicle = new VehicleTypesModel();
            new_vehicle.ID = new Guid(Request.Params["vehicle_id"]);
            new_vehicle.Name = Request.Params["type_name"] != null ? Request.Params["type_name"] : "";
            if (ISValid(new_vehicle, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "type_name", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_vehicle.Name, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Vehicle_Types", new_vehicle.ID, cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_VehicleTypes.Edited });
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
            VehicleTypesModel vehicle = new VehicleTypesModel();
            DataRow vehicle_ = Database.GetRow("Vehicle_Types", new Guid(ID));
            if (vehicle_ != null)
            {
                vehicle.ID = new Guid(vehicle_["id"].ToString());
                vehicle.Name = vehicle_["type_name"].ToString();
                vehicle.URL = new URLModel
                {
                    Refresh = "/CP_VehicleTypes/GetAll/",
                    Edit = "/CP_VehicleTypes/Editing/"
                };
            }
            return PartialView(vehicle);
        }

        [HttpPost]
        public PartialViewResult Detail(string ID)
        {
            VehicleTypesModel vehicle = new VehicleTypesModel();
            DataRow vehicle_ = Database.GetRow("Vehicle_Types", new Guid(ID));
            if (vehicle_ != null)
            {
                vehicle.Name = vehicle_["type_name"].ToString();
                vehicle.ID = new Guid(vehicle_["id"].ToString());
            }

            return PartialView(vehicle);
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Vehicle_Types", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_VehicleTypes.Deleted});
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }

        }

        bool ISValid(VehicleTypesModel vehicle, out string msg)
        {
            bool flag = true;
            if (vehicle.Name == "")
            {

                msg = Resources.CP_VehicleTypes.EnterVehicleTypeP;
                return false;

            }

            msg = "";
            return flag;
        }

    }
}