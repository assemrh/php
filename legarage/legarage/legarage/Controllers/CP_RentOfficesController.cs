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
    public class CP_RentOfficesController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_RentOffices/GetAll/", Add = "/CP_RentOffices/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_RentOffices/GetAll/", Adding = "/CP_RentOffices/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string msg;
            string sql = "";
            DataRow user; Tools.FindCurrentUser(out user);
            string condetion = " ";
            if ((Session["Is_Admin"] ?? "").ToString() != "1")
            {
                condetion = " where U.id = '" + user["id"].ToString() + "' ";
            }
            sql += " SELECT RO.id , RO.name ,U.full_name AS OwnerName,RO.mobile, ";
            sql += " RO.whatsapp, (P.name + ' , ' + AD.details) AS Address ";
            sql += " FROM Rental_Offices AS RO ";
            sql += " inner join Users AS U ON U.id = RO.user_id ";
            sql += " inner join Addresses AS AD on AD.id = RO.address_id ";
            sql += " inner join Provinces AS P on AD.province_id = p.id " + condetion + " ORDER BY RO.created_at ASC";
            DataTable rentoffices = Database.ReadTableByQuery(sql, null, out msg);
            string HTML_Content = "";
            if (rentoffices != null && rentoffices.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow rent in rentoffices.Rows)
                {
                    string ID = rent["ID"].ToString();
                    HTML_Content += "<tr class=\"rent-office-row \">";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + rent["Name"].ToString() + "</td>";
                    if (Session["Is_Admin"] == null || Session["Is_Admin"].ToString() == "1")
                    {
                        HTML_Content += "<td> " + rent["OwnerName"].ToString() + "</td>";
                    }
                    HTML_Content += "<td class='telefon'> " + rent["Mobile"].ToString() + " </td>";
                    HTML_Content += "<td class='telefon'> " + rent["Whatsapp"].ToString() + " </td>";
                    HTML_Content += "<td> " + rent["Address"].ToString() + " </td>";
                    //Tools: 
                    HTML_Content += " <td>";
                    HTML_Content += " <i title = \"" + Resources.CP_RentOffices.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\" data-toggle= \"modal\"  onclick=\"Edit('" + ID + "','/CP_RentOffices/Edit/')\"data-target = \"#Modal\" ></i>&nbsp;";
                    HTML_Content += " <i title = \"" + Resources.CP_RentOffices.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_RentOffices/Delete/');\"></i>&nbsp;";
                    HTML_Content += " <i title = \"" + Resources.CP_RentOffices.Details + "\" style=\"color:lawngreen; cursor:pointer;\"class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_RentOffices/Details/')\" data-target=\"#Modal\"></i>&nbsp;&nbsp;";
                    HTML_Content += " <i title = \"" + Resources.CP_RentOffices.Images + "\" style=\"color:SlateGrey; cursor:pointer;\" class=\"far fa-images\" data-toggle=\"modal\" onclick=\"Images('" + ID + "','/CP_RentOffices/Images/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += " </td> ";
                    HTML_Content += " </tr> ";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_RentOffices.NoRentOffices + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;

            Session["error"] = null;
            RentOfficesModel new_rentoffice = new RentOfficesModel();
            new_rentoffice.Name = Request.Params["name"] != null ? Request.Params["name"] : "";
            new_rentoffice.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            new_rentoffice.Whatsapp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            new_rentoffice.Fax = Request.Params["fax"] != null ? Request.Params["fax"] : "";
            new_rentoffice.Website = Request.Params["website"];
            new_rentoffice.Facebook = Request.Params["facebook"] != null ? Request.Params["facebook"] : "";
            new_rentoffice.Tiktok = Request.Params["tiktok"] != null ? Request.Params["tiktok"] : "";
            new_rentoffice.Snapchat = Request.Params["snapchat"] != null ? Request.Params["snapchat"] : "";
            new_rentoffice.Twitter = Request.Params["twitter"] != null ? Request.Params["twitter"] : "";
            new_rentoffice.Instagram = Request.Params["instagram"] != null ? Request.Params["instagram"] : "";
            new_rentoffice.Linkedin = Request.Params["linkedin"] != null ? Request.Params["linkedin"] : "";
            new_rentoffice.Youtube = Request.Params["youtube"] != null ? Request.Params["youtube"] : "";
            new_rentoffice.Description = Request.Params["description"] != null ? Request.Params["description"] : "";

            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                new_rentoffice.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                new_rentoffice.User = user;
            }
            else
            {
                new_rentoffice.User = null;
            }
            new_rentoffice.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                new_rentoffice.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                new_rentoffice.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                new_rentoffice.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                new_rentoffice.Address.ProvinceId = new Guid();
            }

            if (ISValid(new_rentoffice, true, out msg))
            {
                Guid id = Guid.NewGuid();
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "mobile", "fax", "whatsapp", "facebook", "tiktok", "snapchat", "twitter", "instagram", "linkedin", "youtube", "user_id", "website", "description", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = {
                    new_rentoffice.Name, new_rentoffice.Mobile, new_rentoffice.Fax, new_rentoffice.Whatsapp, new_rentoffice.Facebook, new_rentoffice.Tiktok, new_rentoffice.Snapchat,
                    new_rentoffice.Twitter, new_rentoffice.Instagram, new_rentoffice.Linkedin, new_rentoffice.Youtube, new_rentoffice.User.ID, new_rentoffice.Website, new_rentoffice.Description, DateTime.Now
                };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Rental_Offices", id, cols, vals, out errMessage))
                {
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { new_rentoffice.Address.ProvinceId, new_rentoffice.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Rental_Offices", id, cols, vals, out errMessage);

                    var vt = Request.Params["Vehicletype"].Split(',');
                    List<string> vehicletypes = vt.ToList();
                    foreach (string vehicletype in vehicletypes)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "rental_office_id", "vehicle_type_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, vehicletype, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Vehicle_Types_Rental_Offices", Guid.NewGuid(), cols, vals, out errMessage);
                    }

                    var mo = Request.Params["model"].Split(',');
                    List<string> models = mo.ToList();
                    foreach (string model in models)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "rental_office_id", "model_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, model, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Rental_Offices_Models", Guid.NewGuid(), cols, vals, out errMessage);
                    }
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/RentOffice/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "created_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, id, "Rental_Offices", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }

                    code = 200;
                    msg = "sucsess";
                    return Json(new { code = code.ToString(), msg = Resources.CP_RentOffices.Added });
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
            sql += " SELECT RO.id AS ID,I.url AS URL, RO.name, U.full_name AS OwnerName, RO.whatsapp, RO.youtube, RO.website, ";
            sql += " RO.linkedin, RO.instagram, RO.snapchat, RO.tiktok, RO.facebook, RO.fax, RO.twitter, ";
            sql += " (P.name + ' , ' + AD.details) AS Address, RO.mobile, RO.whatsapp, RO.description";
            sql += " from Rental_Offices AS RO ";
            sql += " inner join Users AS U on RO.user_id = U.id ";
            sql += " inner join Addresses AS AD on AD.id = RO.address_id ";
            sql += " inner join Provinces AS P on AD.province_id = p.id ";
            sql += " inner join Images AS I on RO.id = I.referral_id ";
            sql += " WHERE @ROID = RO.id ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@ROID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow RentalOffice = dataTable.Rows[0];
                RentOfficesModel RentalOffices = new RentOfficesModel();
                RentalOffices.ID = new Guid(RentalOffice["id"].ToString());
                RentalOffices.Name = RentalOffice["Name"].ToString();
                RentalOffices.Website = RentalOffice["Website"].ToString();
                RentalOffices.Youtube = RentalOffice["Youtube"].ToString();
                RentalOffices.Linkedin = RentalOffice["Linkedin"].ToString();
                RentalOffices.Instagram = RentalOffice["Instagram"].ToString();
                RentalOffices.Twitter = RentalOffice["Twitter"].ToString();
                RentalOffices.Snapchat = RentalOffice["Snapchat"].ToString();
                RentalOffices.Tiktok = RentalOffice["Tiktok"].ToString();
                RentalOffices.Facebook = RentalOffice["Facebook"].ToString();
                RentalOffices.Whatsapp = RentalOffice["Whatsapp"].ToString();
                RentalOffices.Fax = RentalOffice["Fax"].ToString();
                RentalOffices.Mobile = RentalOffice["Mobile"].ToString();
                RentalOffices.Description = RentalOffice["Description"].ToString();

                UsersModel user = new UsersModel();
                user.UserName = RentalOffice["OwnerName"].ToString();
                RentalOffices.User = user;

                RentalOffices.Image = new ImagesModel()
                {
                    URL = RentalOffice["URL"].ToString()
                };

                RentalOffices.Address = new AddressModel();
                RentalOffices.Address.AddressName = RentalOffice["Address"].ToString();

                sql = "";
                sql += " select V.id, V.type_name from Vehicle_Types AS V WHERE V.id in ";
                sql += " (SELECT vehicle_type_id FROM Vehicle_Types_Rental_Offices Where rental_office_id =  @ROID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@ROID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    RentalOffices.VehicleTypes = new List<VehicleTypesModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        VehicleTypesModel vehicle = new VehicleTypesModel();
                        vehicle.Name = item["type_name"].ToString();
                        vehicle.ID = new Guid(item["id"].ToString());
                        RentalOffices.VehicleTypes.Add(vehicle);
                    }
                }
                sql = "";
                sql += " select M.id, M.name from Models AS M WHERE M.id in ";
                sql += " (SELECT model_id FROM Rental_Offices_Models Where rental_office_id =  @ROID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@ROID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    RentalOffices.Models = new List<ModelsModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        ModelsModel model = new ModelsModel();
                        model.Name = item["name"].ToString();
                        model.ID = new Guid(item["id"].ToString());
                        RentalOffices.Models.Add(model);
                    }
                }
                return PartialView(RentalOffices);
            }
            else
            {
                return PartialView(new RentOfficesModel());
            }
        }

        [HttpPost]
        public PartialViewResult Edit(string ID)
        {
            string sql = "";
            sql += " select RO.id ,I.url AS URL,I.id AS IID, RO.name, U.full_name AS OwnerName, ";
            sql += " RO.website, RO.youtube, RO.linkedin, RO.Instagram, RO.Snapchat, RO.Tiktok, ";
            sql += " RO.facebook, RO.fax, RO.twitter AS twitter ,AD.id AS AddressID, ";
            sql += " AD.details AS AddressDetailes , P.id AS CityID,P.country_id AS CountryID, ";
            sql += " RO.mobile, RO.whatsapp, RO.description from Rental_Offices AS RO ";
            sql += " inner join Users AS U on RO.user_id = U.id  inner ";
            sql += " join Addresses AS AD on AD.id = RO.address_id  ";
            sql += " inner join Provinces AS P on AD.province_id = p.id ";
            sql += " inner join Images AS I on RO.id = I.referral_id  ";
            sql += " Where @ROID = RO.id AND I.referral_type = 'Rental_Offices'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@ROID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            DataRow Rent = dataTable.Rows[0];
            RentOfficesModel RentOffice = new RentOfficesModel();
            RentOffice.ID = new Guid(Rent["ID"].ToString());
            RentOffice.Name = Rent["Name"].ToString();
            RentOffice.Website = Rent["Website"].ToString();
            RentOffice.Youtube = Rent["Youtube"].ToString();
            RentOffice.Linkedin = Rent["Linkedin"].ToString();
            RentOffice.Instagram = Rent["Instagram"].ToString();
            RentOffice.Twitter = Rent["Twitter"].ToString();
            RentOffice.Snapchat = Rent["Snapchat"].ToString();
            RentOffice.Tiktok = Rent["Tiktok"].ToString();
            RentOffice.Facebook = Rent["Facebook"].ToString();
            RentOffice.Whatsapp = Rent["Whatsapp"].ToString();
            RentOffice.Fax = Rent["Fax"].ToString();
            RentOffice.Mobile = Rent["Mobile"].ToString();
            RentOffice.Description = Rent["Description"].ToString();
            RentOffice.Image = new ImagesModel()
            {
                URL = Rent["URL"].ToString(),
                ID = new Guid(Rent["IID"].ToString())
            };
            UsersModel user = new UsersModel();
            user.UserName = Rent["OwnerName"].ToString();
            RentOffice.User = user;

            RentOffice.Address = new AddressModel()
            {
                AddressId = new Guid(Rent["AddressID"].ToString()),
                AddressName = Rent["AddressDetailes"].ToString(),
                ProvinceId = new Guid(Rent["CityID"].ToString()),
                CountryId = new Guid(Rent["CountryID"].ToString())
            };

            sql = " select M.id ,M.name from Models AS M WHERE M.id in (SELECT model_id FROM Rental_Offices_Models Where rental_office_id = @ROID )";
            li = new List<SqlParameter>();
            li.Add(new SqlParameter("@ROID", ID));
            dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                RentOffice.Models = new List<ModelsModel>();
                foreach (DataRow item in dataTable.Rows)
                {
                    ModelsModel model = new ModelsModel();
                    model.Name = item["name"].ToString();
                    model.ID = new Guid(item["id"].ToString());
                    RentOffice.Models.Add(model);
                }
            }
            sql = "select V.id, V.type_name from Vehicle_Types AS V WHERE V.id in (SELECT vehicle_type_id FROM Vehicle_Types_Rental_Offices Where rental_office_id = @ROID)";
            li = new List<SqlParameter>();
            li.Add(new SqlParameter("@ROID", ID));
            dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                RentOffice.VehicleTypes = new List<VehicleTypesModel>();
                foreach (DataRow item in dataTable.Rows)
                {
                    VehicleTypesModel vehicle = new VehicleTypesModel();
                    vehicle.Name = item["type_name"].ToString();
                    vehicle.ID = new Guid(item["id"].ToString());
                    RentOffice.VehicleTypes.Add(vehicle);
                }
            }
            RentOffice.URL = new URLModel
            {
                Refresh = "/CP_RentOffices/GetAll/",
                Edit = "/CP_RentOffices/Editing/"
            };
            return PartialView(RentOffice);
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            RentOfficesModel edit_rent = new RentOfficesModel();
            edit_rent.ID = new Guid(Request.Params["id"].ToString());
            edit_rent.Name = Request.Params["name"] != null ? Request.Params["name"] : "";
            edit_rent.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            edit_rent.Whatsapp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            edit_rent.Fax = Request.Params["fax"] != null ? Request.Params["fax"] : "";
            edit_rent.Description = Request.Params["description"] != null ? Request.Params["description"] : "";
            edit_rent.Website = Request.Params["website"] != null ? Request.Params["website"] : "";
            edit_rent.Facebook = Request.Params["facebook"] != null ? Request.Params["facebook"] : "";
            edit_rent.Tiktok = Request.Params["tiktok"] != null ? Request.Params["tiktok"] : "";
            edit_rent.Snapchat = Request.Params["snapchat"] != null ? Request.Params["snapchat"] : "";
            edit_rent.Twitter = Request.Params["twitter"] != null ? Request.Params["twitter"] : "";
            edit_rent.Instagram = Request.Params["instagram"] != null ? Request.Params["instagram"] : "";
            edit_rent.Linkedin = Request.Params["linkedin"] != null ? Request.Params["linkedin"] : "";
            edit_rent.Youtube = Request.Params["youtube"] != null ? Request.Params["youtube"] : "";
            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                edit_rent.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                edit_rent.User = user;
            }
            else
            {
                edit_rent.User = null;
            }
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                edit_rent.Address = new AddressModel();
                edit_rent.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                edit_rent.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                edit_rent.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                edit_rent.Address.ProvinceId = new Guid();
            }
            if (ISValid(edit_rent, false, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "user_id", "name", "mobile", "fax", "whatsapp", "facebook", "tiktok", "snapchat", "twitter", "instagram", "linkedin", "youtube", "website", "description", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { edit_rent.User.ID, edit_rent.Name, edit_rent.Mobile, edit_rent.Fax, edit_rent.Whatsapp, edit_rent.Facebook, edit_rent.Tiktok, edit_rent.Snapchat, edit_rent.Twitter, edit_rent.Instagram, edit_rent.Linkedin, edit_rent.Youtube, edit_rent.Website, edit_rent.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Rental_Offices", edit_rent.ID, cols, vals, out errMessage))
                {
                    Database.DeleteRow("Addresses", edit_rent.Address.AddressId, out msg);
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "updated_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { edit_rent.Address.ProvinceId, edit_rent.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Rental_Offices", edit_rent.ID, cols, vals, out errMessage);

                    var mo = Request.Params["model"].Split(',');
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "rental_office_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { edit_rent.ID };
                    vals.AddRange(valsin);
                    Database.DeleteRow("Rental_Offices_Models", cols, vals, out msg);
                    List<string> brands = mo.ToList();
                    foreach (string brand in brands)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "rental_office_id", "model_id", "updated_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { edit_rent.ID, brand, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Rental_Offices_Models", Guid.NewGuid(), cols, vals, out errMessage);
                    }
                    var vt = Request.Params["Vehicletype"].Split(',');
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "rental_office_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { edit_rent.ID };
                    vals.AddRange(valsin);
                    Database.DeleteRow("Vehicle_Types_Rental_Offices", cols, vals, out msg);
                    List<string> vehicletypes = vt.ToList();
                    foreach (string vehicletype in vehicletypes)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "rental_office_id", "vehicle_type_id", "updated_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { edit_rent.ID, vehicletype, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Vehicle_Types_Rental_Offices", Guid.NewGuid(), cols, vals, out errMessage);
                    }
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = new Guid(Request.Params["image_id"].ToString());
                        string ImageURL = Request.Params["image_url"] != null ? "/Images/RentOffice/" + Request.Params["image_url"] : "/Images/dafault.png";
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
                        FileName = Server.MapPath("~/Images/RentOffice/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);

                        cols = new List<string>();
                        vals = new List<object>();

                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "updated_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, edit_rent.ID, "Rental_Offices", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_RentOffices.Edited });
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
        public PartialViewResult Images(string ID)
        {
            string sql = "";
            sql += " select RO.id AS ROID,I.url AS URL ,I.id AS ID from Rental_Offices AS RO ";
            sql += " inner join Images AS I on RO.id = I.referral_id Where RO.id = @ROID ";
            sql += " AND I.referral_type = 'Rental_Offices' AND I.is_main = 0 ORDER BY I.created_at ASC";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@ROID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            RentOfficesModel RentOffice = new RentOfficesModel();
            RentOffice.ID = new Guid(ID);
            RentOffice.URL = new URLModel
            {
                RefreshImages = "/CP_RentOffices/RefreshImages/",
                DeleteImage = "/CP_RentOffices/DeleteImage/",
                AddingImages = "/CP_RentOffices/AddImage/"
            };
            RentOffice.Images = new List<ImagesModel>();
            foreach (DataRow images in dataTable.Rows)
            {
                ImagesModel image = new ImagesModel();
                image.URL = images["URL"].ToString();
                image.ID = new Guid(images["ID"].ToString());
                RentOffice.Images.Add(image);
            };
            return PartialView(RentOffice);

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
                FileName = Server.MapPath("~/Images/RentOffice/SubImages/" + FileName);
                System.IO.File.WriteAllBytes(FileName, b);
                List<string> cols = new List<string>();
                List<object> vals = new List<object>();
                string[] colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                cols.AddRange(colsinput);
                object[] valsinput = new object[] { ImageName, new Guid(ID), "Rental_Offices", DateTime.Now, 0 };
                vals.AddRange(valsinput);
                Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                Session["Attachment"] = null;
                Session["Attachment_File_Name"] = null;
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_RentOffices.Added });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });

            }
        }

        [HttpPost]
        public JsonResult RefreshImages(string ID)
        {
            string sql = "";
            sql += " select RO.id AS ROID,I.url AS URL,I.id AS ID from Rental_Offices AS RO ";
            sql += " inner join Images AS I on RO.id = I.referral_id Where RO.id = @ROID ";
            sql += " AND I.referral_type = 'Rental_Offices' AND I.is_main = 0 ORDER BY I.created_at ASC";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@ROID", ID));
            string msg = "";
            DataTable Images = Database.ReadTableByQuery(sql, li, out msg);
            string HTML_Content = "";
            if (Images != null && Images.Rows.Count > 0)
            {
                foreach (DataRow Image in Images.Rows)
                {
                    HTML_Content += "<div class=\'py-3 border m-1\'>";
                    HTML_Content += "<img class=\"rounded w-50\" src=\"/Images/RentOffice/SubImages/" + Image["URL"] + " \"/>";
                    HTML_Content += "<button type=\"button\" class=\"btn btn-danger ml-5 mt-5\" style=\"margin: 14%;\" onclick =\"DeleteImage('" + Image["ROID"] + "','" + Image["ID"] + "','/CP_RentOffices/DeleteImage/','/CP_RentOffices/RefreshImages/')\">";
                    HTML_Content += Resources.CP_RentOffices.Delete;
                    HTML_Content += "</button>";
                    HTML_Content += "</div>";
                }
            }
            else
            {
                HTML_Content = Resources.CP_RentOffices.NoImage;
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult DeleteImage(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Images", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_RentOffices.Deleted});
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }


        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Rental_Offices", new Guid(ID), out msg))
            {
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@GID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Rental_Offices_Models Where rental_office_id = @GID ", li, out msg);
                }

                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_RentOffices.Deleted});
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        bool ISValid(RentOfficesModel rentoffice, bool Is_Add, out string msg)
        {
            bool flag = true;
            if (rentoffice.Name == "")
            {
                msg = Resources.CP_RentOffices.EnterNamePlease;
                return false;
            }
            if (rentoffice.User == null)
            {
                msg = Resources.CP_RentOffices.EnterOwnerNamePlease;
                return false;
            }
            if (rentoffice.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP_RentOffices.EnterCityPlease;
                return false;
            }
            if (rentoffice.Address.AddressName == "")
            {
                msg = Resources.CP_RentOffices.EnterAddressPlease;
                return false;
            }
            if (rentoffice.Mobile == null)
            {
                msg = Resources.CP_RentOffices.EnterPhonePlease;
                return false;
            }
            if (Request.Params["Vehicletype"] == null)
            {
                msg = Resources.CP_RentOffices.EnterVehiclePlease;
                return false;
            }
            if (Request.Params["model"] == null)
            {
                msg = Resources.CP_RentOffices.EnterModelPlease;
                return false;
            }
            if (rentoffice.Whatsapp == null)
            {
                msg = Resources.CP_RentOffices.EnterWhatsappPlease;
                return false;
            }
            if (rentoffice.Description == "")
            {
                msg = Resources.CP_RentOffices.EnterDescrptionPlease;
                return false;
            }
            if (Session["Attachment"] == null && Is_Add)
            {
                msg = Resources.CP_RentOffices.EnterImagePlease;
                return false;
            }
            msg = "";
            return flag;
        }
    }
}