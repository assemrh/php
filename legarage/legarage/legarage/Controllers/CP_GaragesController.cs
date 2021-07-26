using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_GaragesController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Garages/GetAll/", Add = "/CP_Garages/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Garages/GetAll/", Adding = "/CP_Garages/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string sql = "";
            string msg;
            DataRow user; Tools.FindCurrentUser(out user);
            string condetion = " ";
            if (Session["Is_Admin"].ToString() != "1")
            {
                condetion = " where U.id = '" + user["id"].ToString() + "' ";
            }
            sql += " select G.id AS ID, G.name, U.full_name AS OwnerName, ";
            sql += " (P.name + ' , ' + AD.details) AS Address, G.mobile ,G.whatsapp from ";
            sql += " Garages AS G inner join Users AS U on G.user_id = U.id ";
            sql += " inner join Addresses AS AD on AD.id = g.address_id ";
            sql += " inner join Provinces AS P on AD.province_id = p.id " + condetion + "  ORDER BY G.created_at ASC";
            DataTable garages = Database.ReadTableByQuery(sql, null, out msg);
            string HTML_Content = "";
            if (garages != null && garages.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow garage in garages.Rows)
                {
                    string ID = garage["ID"].ToString();
                    HTML_Content += "<tr class='garage-row'>";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + garage["Name"].ToString() + "</td>";
                    if (Session["Is_Admin"] == null || Session["Is_Admin"].ToString() == "1")
                    {
                        HTML_Content += "<td> " + garage["OwnerName"].ToString() + "</td>";
                    }
                    HTML_Content += "<td> " + garage["Address"].ToString() + "</td>";
                    HTML_Content += "<td class='telefon'> " + garage["Mobile"].ToString() + "</td>";
                    HTML_Content += "<td class='telefon'> " + garage["Whatsapp"].ToString() + "</td>";
                    //Tools: 
                    HTML_Content += "<td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Garages.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Garages/Edit/')\"data-target=\"#Modal\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Garages.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Garages/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Garages.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Garages/Details/')\" data-target=\"#Modal\"></i>&nbsp&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Garages.Images + "\" style=\"color:SlateGrey; cursor:pointer;\" class=\"far fa-images\" data-toggle=\"modal\" onclick=\"Images('" + ID + "','/CP_Garages/Images/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP.NoGarages + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            GaragesModel new_garage = new GaragesModel();
            new_garage.Name = Request.Params["Name"] != null ? Request.Params["Name"] : "";
            new_garage.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+"+Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            new_garage.Whatsapp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            new_garage.Fax = Request.Params["Fax"] != null ? Request.Params["Fax"] : "";
            new_garage.Description = Request.Params["Description"] != null ? Request.Params["Description"] : "";
            new_garage.Website = Request.Params["Website"] != null ?  Request.Params["Website"] : "";
            new_garage.Facebook = Request.Params["Facebook"] != null ? Request.Params["Facebook"] : "";
            new_garage.Tiktok = Request.Params["Tiktok"] != null ? Request.Params["Tiktok"] : "";
            new_garage.Snapchat = Request.Params["Snapchat"] != null ? Request.Params["Snapchat"] : "";
            new_garage.Twitter = Request.Params["Twitter"] != null ? Request.Params["Twitter"] : "";
            new_garage.Instagram = Request.Params["Instagram"] != null ? Request.Params["Instagram"] : "";
            new_garage.Linkedin = Request.Params["Linkedin"] != null ? Request.Params["Linkedin"] : "";
            new_garage.Youtube = Request.Params["Youtube"] != null ? Request.Params["Youtube"] : "";
            new_garage.Keywords = Request.Params["Keywords"] != null ? Request.Params["Keywords"] : "";
            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                new_garage.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                new_garage.User = user;
            }
            else
            {
                new_garage.User = null;
            }
            new_garage.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                new_garage.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                new_garage.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                new_garage.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                new_garage.Address.ProvinceId = new Guid();
            }

            if (ISValid(new_garage, true, out msg))
            {
                Guid id = Guid.NewGuid();
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "user_id", "name", "mobile", "fax", "whatsapp", "facebook", "tiktok", "snapchat", "twitter", "instagram", "linkedin", "youtube", "website", "Keywords", "description", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_garage.User.ID, new_garage.Name, new_garage.Mobile, new_garage.Fax, new_garage.Whatsapp, new_garage.Facebook, new_garage.Tiktok, new_garage.Snapchat, new_garage.Twitter,
                    new_garage.Instagram, new_garage.Linkedin, new_garage.Youtube, new_garage.Website,new_garage.Keywords, new_garage.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Garages", id, cols, vals, out errMessage))
                {
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { new_garage.Address.ProvinceId, new_garage.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Garages", id, cols, vals, out errMessage);
                    var ss = Request.Params["Services"].Split(',');
                    List<string> services = ss.ToList();
                    foreach (string service in services)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "garage_id", "category_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, service, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Garages_Categories", Guid.NewGuid(), cols, vals, out errMessage);
                    }

                    var br = Request.Params["brand"].Split(',');
                    List<string> brands = br.ToList();
                    foreach (string brand in brands)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "garage_id", "brand_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, brand, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Garages_Brands", Guid.NewGuid(), cols, vals, out errMessage);
                    }

                    var vt = Request.Params["Vehicletype"].Split(',');
                    List<string> vehicletypes = vt.ToList();
                    foreach (string vehicletype in vehicletypes)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "garage_id", "vehicle_type_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, vehicletype, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Vehicle_Types_Garages", Guid.NewGuid(), cols, vals, out errMessage);
                    }
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Garages/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { ImageName, id, "Garages", DateTime.Now, 1 };
                        vals.AddRange(valsinput);
                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Garages.Added });
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
            string sql = "";
            sql += " select G.id ,I.url AS URL, G.name ,I.id  AS IID , U.full_name AS OwnerName, ";
            sql += " G.website , G.youtube , G.linkedin, ";
            sql += " G.instagram , G.snapchat, G.tiktok, ";
            sql += " G.facebook, G.fax, G.twitter,AD.id AS AddressID, ";
            sql += " AD.details AS AddressDetailes , P.id AS CityID,P.country_id AS CountryID, ";
            sql += " G.mobile ,G.whatsapp ,G.description,G.keywords ";
            sql += " from Garages AS G ";
            sql += " inner join Users AS U on G.user_id = U.id ";
            sql += " inner join Addresses AS AD on AD.id = G.address_id ";
            sql += " inner join Provinces AS P on AD.province_id = p.id ";
            sql += " inner join Images AS I on G.id = I.referral_id ";
            sql += " Where G.id = @GID AND I.referral_type = 'Garages'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@GID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow garage_ = dataTable.Rows[0];
                GaragesModel garages = new GaragesModel();
                garages.ID = new Guid(garage_["ID"].ToString());
                garages.Name = garage_["Name"].ToString();
                garages.Website = garage_["Website"].ToString();
                garages.Youtube = garage_["Youtube"].ToString();
                garages.Linkedin = garage_["Linkedin"].ToString();
                garages.Instagram = garage_["Instagram"].ToString();
                garages.Twitter = garage_["Twitter"].ToString();
                garages.Snapchat = garage_["Snapchat"].ToString();
                garages.Tiktok = garage_["Tiktok"].ToString();
                garages.Facebook = garage_["Facebook"].ToString();
                garages.Whatsapp = garage_["Whatsapp"].ToString();
                garages.Fax = garage_["Fax"].ToString();
                garages.Mobile = garage_["Mobile"].ToString();
                garages.Keywords = garage_["Keywords"].ToString();
                garages.Description = garage_["Description"].ToString();
                garages.Image = new ImagesModel()
                {
                    ID = new Guid(garage_["IID"].ToString()),
                    URL = garage_["URL"].ToString()
                };
                UsersModel user = new UsersModel();
                user.UserName = garage_["OwnerName"].ToString();
                garages.User = user;

                garages.Address = new AddressModel()
                {
                    AddressId = new Guid(garage_["AddressID"].ToString()),
                    AddressName = garage_["AddressDetailes"].ToString(),
                    ProvinceId = new Guid(garage_["CityID"].ToString()),
                    CountryId = new Guid(garage_["CountryID"].ToString())
                };

                sql = "";
                sql += " SELECT C.id, C.name from Categories AS C WHERE C.id in ";
                sql += " (SELECT category_id FROM Garages_Categories WHERE garage_id = @GID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@GID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    garages.Services = new List<ServicesModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        ServicesModel service = new ServicesModel();
                        service.Name = item["Name"].ToString();
                        service.ID = new Guid(item["ID"].ToString());
                        garages.Services.Add(service);
                    }
                }

                sql = "";
                sql += " select V.id, V.type_name AS Name from Vehicle_Types AS V WHERE V.id in ";
                sql += " (SELECT vehicle_type_id FROM Vehicle_Types_Garages Where garage_id = @GID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@GID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    garages.VehicleTypes = new List<VehicleTypesModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        VehicleTypesModel vehicle = new VehicleTypesModel();
                        vehicle.Name = item["Name"].ToString();
                        vehicle.ID = new Guid(item["ID"].ToString());
                        garages.VehicleTypes.Add(vehicle);
                    }
                }

                sql = "";
                sql += " SELECT B.id ,B.name from Brands AS B WHERE B.id in ";
                sql += " (SELECT brand_id FROM Garages_Brands WHERE garage_id = @GID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@GID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    garages.Brands = new List<BrandsModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        BrandsModel brand = new BrandsModel();
                        brand.Name = item["Name"].ToString();
                        brand.ID = new Guid(item["ID"].ToString());
                        garages.Brands.Add(brand);
                    }
                }

                garages.URL = new URLModel
                {
                    Refresh = "/CP_Garages/GetAll/",
                    Edit = "/CP_Garages/Editing/"
                };
                return PartialView(garages);
            }
            else
            {
                return PartialView(new GaragesModel());
            }
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            GaragesModel EditGarage = new GaragesModel();
            EditGarage.ID = new Guid(Request.Params["ID"].ToString());
            EditGarage.Name = Request.Params["Name"] != null ? Request.Params["Name"] : "";
            EditGarage.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            EditGarage.Whatsapp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            EditGarage.Fax = Request.Params["Fax"] != null ? Request.Params["Fax"] : "";
            EditGarage.Description = Request.Params["Description"] != null ? Request.Params["Description"] : "";
            EditGarage.Website = Request.Params["Website"] != null ? Request.Params["Website"] : "";
            EditGarage.Facebook = Request.Params["Facebook"] != null ? Request.Params["Facebook"] : "";
            EditGarage.Tiktok = Request.Params["Tiktok"] != null ? Request.Params["Tiktok"] : "";
            EditGarage.Snapchat = Request.Params["Snapchat"] != null ? Request.Params["Snapchat"] : "";
            EditGarage.Twitter = Request.Params["Twitter"] != null ? Request.Params["Twitter"] : "";
            EditGarage.Instagram = Request.Params["Instagram"] != null ? Request.Params["Instagram"] : "";
            EditGarage.Linkedin = Request.Params["Linkedin"] != null ? Request.Params["Linkedin"] : "";
            EditGarage.Youtube = Request.Params["Youtube"] != null ? Request.Params["Youtube"] : "";
            EditGarage.Keywords = Request.Params["Keywords"] != null ? Request.Params["Keywords"] : "";
            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                EditGarage.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                EditGarage.User = user;
            }
            else
            {
                EditGarage.User = null;
            }
            EditGarage.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                EditGarage.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                EditGarage.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                EditGarage.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                EditGarage.Address.ProvinceId = new Guid();
            }

            if (ISValid(EditGarage, false, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "user_id", "name", "mobile", "fax", "whatsapp", "facebook", "tiktok", "snapchat", "twitter", "instagram", "linkedin", "youtube", "website", "keywords", "description", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { EditGarage.User.ID, EditGarage.Name, EditGarage.Mobile, EditGarage.Fax, EditGarage.Whatsapp, EditGarage.Facebook, EditGarage.Tiktok, EditGarage.Snapchat, EditGarage.Twitter, EditGarage.Instagram, EditGarage.Linkedin, EditGarage.Youtube, EditGarage.Website, EditGarage.Keywords, EditGarage.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Garages", EditGarage.ID, cols, vals, out errMessage))
                {
                    Database.DeleteRow("Addresses", EditGarage.Address.AddressId, out msg);
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "updated_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { EditGarage.Address.ProvinceId, EditGarage.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Garages", EditGarage.ID, cols, vals, out errMessage);
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = new Guid(Request.Params["IID"].ToString());
                        string ImageURL = Request.Params["image_url"] != null ? "/Images/Garages/" + Request.Params["image_url"] : "/Images/dafault.png";
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
                        FileName = Server.MapPath("~/Images/Garages/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);

                        cols = new List<string>();
                        vals = new List<object>();

                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "updated_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, EditGarage.ID, "Garages", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Garages.Edited });
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
            string sql = "";
            sql += " SELECT G.id ,I.url AS URL, G.name, U.full_name AS OwnerName,";
            sql += " G.website, G.Youtube, G.linkedin,";
            sql += " G.instagram, G.snapchat, G.tiktok,";
            sql += " G.facebook, G.fax, G.twitter,(P.name + ' , ' + AD.details) AS Address,";
            sql += " G.mobile,G.whatsapp ,G.description ,G.keywords";
            sql += " from Garages AS G ";
            sql += " inner join Users AS U on G.user_id = U.id ";
            sql += " inner join Addresses AS AD on AD.id = G.address_id ";
            sql += " inner join Provinces AS P on AD.province_id = p.id ";
            sql += " inner join Images AS I on G.id = I.referral_id ";
            sql += " Where G.id = @GID AND I.referral_type = 'Garages'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@GID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow garage_ = dataTable.Rows[0];
                GaragesModel garages = new GaragesModel();
                garages.ID = new Guid(garage_["ID"].ToString());
                garages.Name = garage_["Name"].ToString();
                garages.Website = garage_["Website"].ToString();
                garages.Youtube = garage_["Youtube"].ToString();
                garages.Linkedin = garage_["Linkedin"].ToString();
                garages.Instagram = garage_["Instagram"].ToString();
                garages.Twitter = garage_["Twitter"].ToString();
                garages.Snapchat = garage_["Snapchat"].ToString();
                garages.Tiktok = garage_["Tiktok"].ToString();
                garages.Facebook = garage_["Facebook"].ToString();
                garages.Whatsapp = garage_["Whatsapp"].ToString();
                garages.Fax = garage_["Fax"].ToString();
                garages.Mobile = garage_["Mobile"].ToString();
                garages.Keywords = garage_["Keywords"].ToString();
                garages.Description = garage_["Description"].ToString();
                garages.Address = new AddressModel();
                garages.Address.AddressName = garage_["Address"].ToString();
                garages.Image = new ImagesModel()
                {
                    URL = garage_["URL"].ToString()
                };
                UsersModel user = new UsersModel();
                user.UserName = garage_["OwnerName"].ToString();
                garages.User = user;

                sql = "";
                sql += " SELECT C.id, C.name from Categories AS C WHERE C.id in ";
                sql += " (SELECT category_id FROM Garages_Categories WHERE garage_id = @GID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@GID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    garages.Services = new List<ServicesModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        ServicesModel service = new ServicesModel();
                        service.Name = item["Name"].ToString();
                        service.ID = new Guid(item["ID"].ToString());
                        garages.Services.Add(service);
                    }
                }

                sql = "";
                sql += " select V.id, V.type_name AS Name from Vehicle_Types AS V WHERE V.id in ";
                sql += " (SELECT vehicle_type_id FROM Vehicle_Types_Garages Where garage_id = @GID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@GID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    garages.VehicleTypes = new List<VehicleTypesModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        VehicleTypesModel vehicle = new VehicleTypesModel();
                        vehicle.Name = item["Name"].ToString();
                        vehicle.ID = new Guid(item["ID"].ToString());
                        garages.VehicleTypes.Add(vehicle);
                    }
                }

                sql = "";
                sql += " select B.id ,B.name from Brands AS B WHERE B.id in ";
                sql += " (SELECT brand_id FROM Garages_Brands WHERE garage_id = @GID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@GID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    garages.Brands = new List<BrandsModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        BrandsModel brand = new BrandsModel();
                        brand.Name = item["Name"].ToString();
                        brand.ID = new Guid(item["ID"].ToString());
                        garages.Brands.Add(brand);
                    }
                }
                return PartialView(garages);
            }
            else
            {
                return PartialView(new GaragesModel());
            }
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Garages", new Guid(ID), out msg))
            {
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@GID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Garages_Brands Where garage_id = @GID ", li, out msg);
                }
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@GID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Garages_Categories Where garage_id = @GID ", li, out msg);
                }
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@GID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Vehicle_Types_Garages Where garage_id = @GID ", li, out msg);
                }
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Garages.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        [HttpPost]
        public JsonResult RefreshImages(string ID)
        {
            string sql = "";
            sql += " SELECT G.id AS GID, I.url AS URL,I.id AS ID from Garages AS G ";
            sql += " inner join Images AS I on G.id = I.referral_id ";
            sql += " Where G.id = @GID ";
            sql += " AND I.referral_type = 'Garages' AND I.is_main = 0 ORDER BY I.created_at ASC";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@GID", ID));
            string msg = "";
            DataTable Images = Database.ReadTableByQuery(sql, li, out msg);
            string HTML_Content = "";
            if (Images != null)
            {
                foreach (DataRow Image in Images.Rows)
                {
                    HTML_Content += "<div class=\'py-3 border m-1\'>";
                    HTML_Content += "<img class=\"rounded w-50\" src=\"/Images/Garages/SubImages/" + Image["URL"] + " \"/>";
                    HTML_Content += "<button type=\"button\" class=\"btn btn-danger ml-5 mt-5\" style=\"margin: 14%;\" onclick =\"DeleteImage('" + Image["GID"] + "','" + Image["ID"] + "','/CP_Garages/DeleteImage/','/CP_Garages/RefreshImages/')\">";
                    HTML_Content += Resources.CP_Garages.DeleteImage;
                    HTML_Content += "</button>";
                    HTML_Content += "</div>";
                }
            }
            else
            {
                HTML_Content = Resources.CP_Garages.NoImage;
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult AddImage(string ID)
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            ImagesModel image = new ImagesModel();
            string errMessage = string.Empty;
            if (Session["Attachment"] != null)
            {
                Guid ImageID = Guid.NewGuid();
                string ImageName = "";
                byte[] b = (byte[])Session["Attachment"];
                string FileName = (string)Session["Attachment_File_Name"];
                FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                ImageName = FileName;
                FileName = Server.MapPath("~/Images/Garages/SubImages/" + FileName);
                System.IO.File.WriteAllBytes(FileName, b);
                List<string> cols = new List<string>();
                List<object> vals = new List<object>();
                string[] colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                cols.AddRange(colsinput);
                object[] valsinput = new object[] { ImageName, new Guid(ID), "Garages", DateTime.Now, 0 };
                vals.AddRange(valsinput);
                Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                Session["Attachment"] = null;
                Session["Attachment_File_Name"] = null;
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Garages.Added });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        [HttpPost]
        public PartialViewResult Images(string ID)
        {
            string sql = "";
            sql += " select G.id AS GRID,I.url AS URL ,I.id AS ID from Garages AS G ";
            sql += " inner join Images AS I on G.id = I.referral_id Where G.id = @GID ";
            sql += " AND I.referral_type = 'Garages' AND I.is_main = 0 ORDER BY I.created_at ASC";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@GID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            GaragesModel garages = new GaragesModel();
            garages.ID = new Guid(ID);
            garages.URL = new URLModel
            {
                RefreshImages = "/CP_Garages/RefreshImages/",
                DeleteImage = "/CP_Garages/DeleteImage/",
                AddingImages = "/CP_Garages/AddImage/"
            };
            garages.Images = new List<ImagesModel>();
            foreach (DataRow images in dataTable.Rows)
            {
                ImagesModel image = new ImagesModel();
                image.URL = images["URL"].ToString();
                image.ID = new Guid(images["ID"].ToString());
                garages.Images.Add(image);
            };

            return PartialView(garages);
        }

        [HttpPost]
        public JsonResult DeleteImage(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Images", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Garages.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }

        }

        bool ISValid(GaragesModel garage, bool Is_Add, out string msg)
        {
            bool flag = true;
            if (garage.Name == "")
            {
                msg = Resources.CP_Garages.EnterNamePlease;
                return false;
            }
            if (garage.User == null)
            {
                msg = Resources.CP_Garages.EnterOwnerNamePlease;
                return false;
            }
            if (garage.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP_Garages.EnterCityPlease;
                return false;
            }
            if (garage.Address.AddressName == "")
            {
                msg = Resources.CP_Garages.EnterAddressPlease;
                return false;
            }
            if (garage.Mobile == null)
            {
                msg = Resources.CP_Garages.EnterPhonePlease;
                return false;
            }
            if (Request.Params["Vehicletype"] == null)
            {
                msg = Resources.CP_Garages.EnterVehiclePlease;
                return false;
            }
            if (Request.Params["brand"] == null)
            {
                msg = Resources.CP_Garages.EnterBrandPlease;
                return false;
            }
            if (Request.Params["services"] == null)
            {
                msg = Resources.CP_Garages.EnterSerivesPlease;
                return false;
            }
            if (garage.Whatsapp == null)
            {
                msg = Resources.CP_Garages.EnterWhatsappPlease;
                return false;
            }
            if (garage.Keywords == "")
            {
                msg = Resources.CP_Garages.EnterKeywordsPlease;
                return false;
            }
            if (garage.Description == "")
            {
                msg = Resources.CP_Garages.EnterDescrptionPlease;
                return false;
            }
            if (Session["Attachment"] == null && Is_Add)
            {
                msg = Resources.CP_Garages.EnterImagePlease;
                return false;
            }
            msg = "";
            return flag;
        }
    }

}


