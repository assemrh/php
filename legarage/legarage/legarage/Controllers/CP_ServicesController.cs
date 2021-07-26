using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_ServicesController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Services/GetAll/", Add = "/CP_Services/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Services/GetAll/", Adding = "/CP_Services/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string msg;
            DataTable Categories = Database.ReadTable("Categories", "ORDER BY created_at ASC", null, out msg);
            string HTML_Content = "";
            if (Categories != null && Categories.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow category in Categories.Rows)
                {
                    string ID = category["ID"].ToString();
                    HTML_Content += "<tr class=\"categories-row\">";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + category["Name"].ToString() + "</td>";
                    //Tools: 
                    HTML_Content += " <td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Services.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Services/Edit/')\"data-target=\"#Modal\"></i>&nbsp;";
                    HTML_Content += "<i title = \"" + Resources.CP_Services.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Services/Delete/');\"></i>&nbsp;";
                    HTML_Content += "<i title = \"" + Resources.CP_Services.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Services/Details/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_Services.NoServices + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            ServicesModel new_services = new ServicesModel();
            new_services.Name = Request.Params["service"] != null ? Request.Params["service"] : "";
            if (ISValid(new_services, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_services.Name, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Categories", Guid.NewGuid(), cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Services.Added });
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
            ServicesModel EditService = new ServicesModel();
            EditService.ID = new Guid(Request.Params["id"]);
            EditService.Name = Request.Params["name"] != null ? Request.Params["name"] : "";
            if (ISValid(EditService, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { EditService.Name, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Categories", EditService.ID, cols, vals, out errMessage))
                {
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Services.Edited });
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
            ServicesModel services = new ServicesModel();
            DataRow services_ = Database.GetRow("Categories", new Guid(ID));
            if (services_ != null)
            {
                services.ID = new Guid(services_["id"].ToString());
                services.Name = services_["name"].ToString();
                services.URL = new URLModel
                {
                    Refresh = "/CP_Services/GetAll/",
                    Edit = "/CP_Services/Editing/"
                };
            }
            return PartialView(services);
        }

        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            ServicesModel service = new ServicesModel();
            DataRow service_ = Database.GetRow("Categories", new Guid(ID));
            if (service_ != null)
            {
                service.ID = new Guid(service_["id"].ToString());
                service.Name = service_["name"].ToString();
            }

            return PartialView(service);
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Categories", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Services.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }

        }

        bool ISValid(ServicesModel services, out string msg)
        {
            bool flag = true;
            if (services.Name == "")
            {
                msg = Resources.CP_Services.EnterSerivePlease;
                return false;
            }

            msg = "";
            return flag;
        }
    }
}