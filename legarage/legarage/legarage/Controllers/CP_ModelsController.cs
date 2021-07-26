using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_ModelsController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Models/GetAll/", Add = "/CP_Models/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Models/GetAll/", Adding = "/CP_Models/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string msg;
            string sql = "";
            sql += " SELECT Models.id as ID , Models.name, Brands.name AS BrandName,";
            sql += " Vehicle_Types.type_name as vehicleName from Models";
            sql += " inner join Brands on Models.brand_id = Brands.id";
            sql += " inner join Vehicle_Types on Models.vehicle_type_id = Vehicle_Types.id ORDER BY Models.created_at ASC";
            DataTable Models = Database.ReadTableByQuery(sql, null, out msg);
            string HTML_Content = "";
            if (Models != null && Models.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow model in Models.Rows)
                {
                    string ID = model["ID"].ToString();
                    HTML_Content += "<tr class=\"model-row "+ model["BrandName"].ToString().Replace(" ", "-") + " "+ model["vehicleName"].ToString() + "\">";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + model["Name"].ToString().Trim() + "</td>";
                    HTML_Content += "<td> <span onclick=\"select_brands('" + model["BrandName"].ToString().Trim() + "');\">" + model["BrandName"].ToString() + " </td>";
                    HTML_Content += "<td> " + model["vehicleName"].ToString() + " </td>";
                    //Tools: 
                    HTML_Content += " <td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Models.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edite('" + ID + "','/CP_Models/Edit/')\"data-target=\"#Modal\"></i>&nbsp;";
                    HTML_Content += "<i title = \"" + Resources.CP_Models.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Models/Delete/');\"></i>&nbsp;";
                    HTML_Content += "<i title = \"" + Resources.CP_Models.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Models/Details/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP.NoModel + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        //[HttpPost]
        //public JsonResult GetAllData()
        //{
        //    string msg;
        //    string sql = "";
        //    sql += " SELECT Models.id as ID , Models.name, Brands.name AS BrandName,";
        //    sql += " Vehicle_Types.type_name as vehicleName from Models";
        //    sql += " inner join Brands on Models.brand_id = Brands.id";
        //    sql += " inner join Vehicle_Types on Models.vehicle_type_id = Vehicle_Types.id ORDER BY Models.created_at ASC";
        //    DataTable Models = Database.ReadTableByQuery(sql, null, out msg);
        //    string HTML_Content = "";
        //    List<ModelsModel_view> modelsList = new List<ModelsModel_view>();
        //    var Labels = new { Edit = Resources.CP_Models.Edit, Delete = Resources.CP_Models.Delete, Details = Resources.CP_Models.Details };
        //    if (Models != null && Models.Rows.Count > 0)
        //    {
        //        foreach (DataRow model in Models.Rows)
        //        {
        //            modelsList.Add(new ModelsModel_view()
        //            {
        //                ID = new Guid(model["ID"].ToString()),
        //                Brand = model["BrandName"].ToString(),
        //                VehicleType = model["vehicleName"].ToString(),
        //                Name = model["Name"].ToString()

        //            });
        //        }
        //    }
        //    else
        //    {
        //        HTML_Content = "<h3>" + Resources.CP.NoModel + "</h3>";
        //    }
        //    return Json(new { code = 200, data = modelsList, label = Labels });
        //}

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            ModelsModel new_model = new ModelsModel();
            new_model.Name = Request.Params["Model"] != null ? Request.Params["Model"] : "";
            new_model.BrandId = Request.Params["Brands"] != null ? Request.Params["Brands"] : "";
            new_model.VehicleTypeId = Request.Params["Vehicle"] != null ? Request.Params["Vehicle"] : "";
            if (ISValid(new_model, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "brand_id", "vehicle_type_id", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_model.Name, new_model.BrandId, new_model.VehicleTypeId, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Models", Guid.NewGuid(), cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Models.Added });
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
            ModelsModel new_model = new ModelsModel();
            new_model.ID = new Guid(Request.Params["ID"]);
            new_model.Name = Request.Params["Name"] != null ? Request.Params["Name"] : "";
            new_model.BrandId = Request.Params["brand_id"] != null ? Request.Params["brand_id"] : "";
            new_model.VehicleTypeId = Request.Params["vehicle_type_id"] != null ? Request.Params["vehicle_type_id"] : "";
            if (ISValid(new_model, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "brand_id", "vehicle_type_id", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_model.Name, new_model.BrandId, new_model.VehicleTypeId, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Models", new_model.ID, cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Models.Edited });
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
            ModelsModel Models = new ModelsModel();
            DataRow models_ = Database.GetRow("Models", new Guid(ID));
            if (models_ != null)
            {
                Models.ID = new Guid(models_["id"].ToString());
                Models.Name = models_["name"].ToString();
                Models.BrandId = models_["brand_id"].ToString();
                Models.VehicleTypeId = models_["vehicle_type_id"].ToString();
                Models.URL = new URLModel
                {
                    Refresh = "/CP_Models/GetAll/",
                    Edit = "/CP_Models/Editing/"
                };
            }
            return PartialView(Models);
        }

        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            ModelsModel model = new ModelsModel();
            DataRow model_ = Database.GetRow("Models", new Guid(ID));
            if (model_ != null)
            {
                model.ID = new Guid(model_["id"].ToString());
                model.Name = model_["name"].ToString();
                model.BrandId = model_["brand_id"].ToString();
                model.VehicleTypeId = model_["vehicle_type_id"].ToString();
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Models", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Models.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        bool ISValid(ModelsModel model, out string msg)
        {
            bool flag = true;
            if (model.Name == "")
            {
                msg = Resources.CP_Models.EnterModel;
                return false;
            }
            if (model.BrandId == null || model.BrandId == "-1")
            {
                msg = Resources.CP_Models.EnterBrand;
                return false;
            }
            if (model.VehicleTypeId == null || model.VehicleTypeId == "-1")
            {

                msg = Resources.CP_Models.EnterVehicleType;
                return false;
            }
            msg = "";
            return flag;
        }
    }
}