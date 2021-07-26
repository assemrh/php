using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_BrandsController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Brands/GetAll/", Add = "/CP_Brands/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Brands/GetAll/", Adding = "/CP_Brands/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string msg = "";
            string sql = "";
            sql += " select B.id as ID , B.name , C.name AS CountryName , I.url AS URL ";
            sql += " from Brands AS B ";
            sql += " inner join Countries AS C on B.country_id = C.id ";
            sql += " inner join Images AS I on I.referral_id = B.id  ORDER BY B.created_at ASC";
            DataTable Brands = Database.ReadTableByQuery(sql, null, out msg);
            string HTML_Content = "";
            if (Brands != null && Brands.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow brand in Brands.Rows)
                {
                    string ID = brand["ID"].ToString();
                    HTML_Content += "<tr class='brand-row'>";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + brand["Name"].ToString() + "</td>";
                    HTML_Content += "<td> " + brand["CountryName"].ToString() + " </td>";
                    //HTML_Content += "<td><img class='rounded ' style='width: 60px;height: 43px;' src=' /Images/Brands/" + brand["URL"].ToString() + "' /> </td>";
                    //Tools: 
                    HTML_Content += " <td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Brands.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Brands/Edit/')\"   data-target=\"#Modal\"></i>&nbsp;";
                    HTML_Content += "<i title = \"" + Resources.CP_Brands.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Brands/Delete/');\"></i>&nbsp;";
                    HTML_Content += "<i title = \"" + Resources.CP_Brands.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Brands/Details/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_Brands.NoBrand + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            BrandsModel new_brand = new BrandsModel();
            new_brand.Name = Request.Params["brand"] != null ? Request.Params["brand"] : "";
            new_brand.CountryId = Request.Params["country"] != null ? Request.Params["country"] : "";
            if (ISValid(new_brand, true, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "country_id", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_brand.Name, new_brand.CountryId, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                Guid BID = Guid.NewGuid();
                if (Database.InsertRow("Brands", BID, cols, vals, out errMessage))
                {
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Brands/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "created_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, BID, "Brands", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Brands.Added });
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
            string msg = "";
            string sql = "";
            sql += " select B.id as ID , B.name , C.id AS CountryId , I.url AS URL, I.id  AS ImageID ";
            sql += " from Brands AS B ";
            sql += " inner join Countries AS C on B.country_id = C.id ";
            sql += " inner join Images AS I on I.referral_id = B.id ";
            sql += " Where B.id = @BID";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@BID", new Guid(ID)));
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            BrandsModel brand = new BrandsModel();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow brand_ = dataTable.Rows[0];
                brand.ID = new Guid(brand_["ID"].ToString());
                brand.Name = brand_["Name"].ToString();
                brand.CountryId = brand_["CountryId"].ToString();
                brand.Image = new ImagesModel()
                {
                    URL = brand_["URL"].ToString(),
                    ID = new Guid(brand_["ImageID"].ToString())
                };
                brand.URL = new URLModel()
                {
                    Refresh = "/CP_Brands/GetAll/",
                    Edit = "/CP_Brands/Editing/"
                };
            }
            return PartialView(brand);
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            BrandsModel brand = new BrandsModel();
            brand.ID = new Guid(Request.Params["id"]);
            brand.Name = Request.Params["name"] != null ? Request.Params["name"] : "";
            brand.CountryId = Request.Params["brand_id"] != null ? Request.Params["brand_id"] : "";
            if (ISValid(brand, false, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "country_id", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { brand.Name, brand.CountryId, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Brands", brand.ID, cols, vals, out errMessage))
                {
                    if (Session["Attachment"] != null)
                    {
                        //Get the ID and URL 
                        Guid ImageID = new Guid(Request.Params["image_id"]);
                        string ImageURL = Request.Params["image_url"] != null ? "/Images/Brands/" + Request.Params["image_url"] : "/Images/dafault.png";
                        System.IO.File.Delete(Server.MapPath("~" + ImageURL));
                        //Delete fromFiles
                        Database.DeleteRow("Images", ImageID, out msg);
                        //Delete Image from DB
                        ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Brands/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);

                        cols = new List<string>();
                        vals = new List<object>();

                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "updated_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, brand.ID, "Brands", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Brands.Edited });
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
        public PartialViewResult Details(string ID)
        {
            BrandsModel brand = new BrandsModel();
            string msg = "";
            string sql = "";
            sql += " select B.id as ID , B.name as Name , C.name AS CountryName , I.url AS URL ";
            sql += " from Brands AS B ";
            sql += " inner join Countries AS C on B.country_id = C.id ";
            sql += " inner join Images AS I on I.referral_id = B.id ";
            sql += " where B.id = @BID ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@BID", new Guid(ID)));
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow brand_ = dataTable.Rows[0];
                brand.ID = new Guid(brand_["ID"].ToString());
                brand.Name = brand_["Name"].ToString();
                brand.CountryName = brand_["CountryName"].ToString();

                ImagesModel Brand = new ImagesModel();
                Brand.URL = brand_["URL"].ToString();
                brand.Image = Brand;
            }
            return PartialView(brand);
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Brands", new Guid(ID), out msg))
            {
                List<SqlParameter> li = new List<SqlParameter>();
                li.Add(new SqlParameter("@BID", new Guid(ID)));
                Database.ReadTableByQuery("DELETE FROM Images Where referral_id = @BID ", li, out msg);
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Brands.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        bool ISValid(BrandsModel brand, bool Is_Add, out string msg)
        {
            bool flag = true;
            if (brand.Name == "")
            {
                msg = Resources.CP_Brands.EnterBrandNamePlease;
                return false;
            }
            if (brand.CountryId == null || brand.CountryId == "-1")
            {
                msg = Resources.CP_Brands.SelectCountryPlease;
                return false;
            }
            if (Session["Attachment"] == null && Is_Add)
            {
                msg = Resources.CP_Brands.EnterBrandLogoPlease;
                return false;
            }
            msg = "";
            return flag;
        }
    }
}