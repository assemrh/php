using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_UsersController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Users/GetAll/", Add = "/CP_Users/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Users/GetAll/", Adding = "/CP_Users/Adding/" });
        }
        [HttpPost]
        public JsonResult GetAll()
        {
            string msg;
            string sql = "";
            sql += " select U.id, U.full_name AS Name,U.email,U.phone,U.username, ";
            sql += " (P.name + ' , ' + AD.details) AS Address from Users AS U ";
            sql += " inner join Addresses AS AD on AD.id = U.address_id ";
            sql += " inner join Provinces AS P on AD.province_id = P.id ";
            sql += " where U.is_admin != 1 ORDER BY U.created_at ASC";
            DataTable Users = Database.ReadTableByQuery(sql, null, out msg);
            string HTML_Content = "";
            if (Users != null && Users.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow User in Users.Rows)
                {
                    string ID = User["ID"].ToString();
                    HTML_Content += "<tr class=\"user-row \"> ";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + User["Name"].ToString() + "</td>";
                    HTML_Content += "<td> " + User["Email"].ToString() + " </td>";
                    HTML_Content += "<td> " + User["Phone"].ToString() + " </td >";
                    HTML_Content += "<td> " + User["Address"].ToString() + " </td >";
                    HTML_Content += "<td> " + User["Username"].ToString() + " </ td >";
                    //Tools: 
                    HTML_Content += "<td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Users.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\" data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Users/Edit/')\"   data-target=\"#Modal\"></i>&nbsp;";
                    HTML_Content += "<i title = \"" + Resources.CP_Users.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Users/Delete/');\"></i>&nbsp;";
                    HTML_Content += "<i title = \"" + Resources.CP_Users.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Users/Details/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_Users.NoUser + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            UsersModel New_user = new UsersModel();
            New_user.Name = Request.Params["fullname"] != null ? Request.Params["fullname"] : "";
            New_user.UserName = Request.Params["username"] != null ? Request.Params["username"] : "";
            New_user.Email = Request.Params["email"] != null ? Request.Params["email"] : "";
            New_user.Password = Request.Params["password"] != null ? Ciphering.GetMD5HashData(Request.Params["password"]) : "";
            New_user.Phone = Request.Params["phoneno"].ToString() != string.Empty ? Request.Params["phone_key"] + " " + Request.Params["phoneno"] : null;
            New_user.Whatsapp = Request.Params["whatsapp"] != null ? Request.Params["whatsapp"] : "";
            New_user.Fax = Request.Params["fax"] != null ? Request.Params["fax"] : "";
            New_user.Website = Request.Params["site"];
            New_user.Facebook = Request.Params["facebook"] != null ? Request.Params["facebook"] : "";
            New_user.IsAdmin = 0;
            New_user.Twitter = Request.Params["twitter"] != null ? Request.Params["twitter"] : "";
            New_user.Instagram = Request.Params["instagram"] != null ? Request.Params["instagram"] : "";
            New_user.Youtube = Request.Params["youtube"] != null ? Request.Params["youtube"] : "";
            New_user.Linkedin = Request.Params["linkedin"] != null ? Request.Params["linkedin"] : "";
            New_user.Snapchat = Request.Params["snapchat"] != null ? Request.Params["snapchat"] : "";
            New_user.Tiktok = Request.Params["tiktok"] != null ? Request.Params["tiktok"] : "";
            New_user.Description = Request.Params["desc"] != null ? Request.Params["desc"] : "";
            New_user.Token = HelperClass.RandomString(50);

            New_user.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                New_user.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                New_user.Address.AddressName = "";
            }

            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                New_user.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                New_user.Address.ProvinceId = new Guid();
            }
            if (ISValid(New_user, true, out msg))
            {
                string Token = HelperClass.RandomString(50);
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "full_name", "email", "phone", "token", "password", "website", "youtube", "linkedin", "instagram", "twitter", "snapchat", "tiktok", "facebook", "is_admin", "whatsapp", "fax", "username", "description", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { New_user.Name, New_user.Email, New_user.Phone, New_user.Token, New_user.Password, New_user.Website, New_user.Youtube, New_user.Linkedin, New_user.Instagram, New_user.Twitter, New_user.Snapchat, New_user.Tiktok, New_user.Facebook, New_user.IsAdmin, New_user.Whatsapp, New_user.Fax, New_user.UserName, New_user.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                Guid ID = Guid.NewGuid();
                if (Database.InsertRow("Users", ID, cols, vals, out errMessage))
                {
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { New_user.Address.ProvinceId, New_user.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Users", ID, cols, vals, out errMessage);

                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Users/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "created_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, ID, "Users", DateTime.Now };
                        vals.AddRange(valsinput);
                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    msg = Resources.CP_Users.Added;
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

        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            string sql = "";
            sql += " select Users.id AS ID,I.url AS URL, Users.full_name AS Name,Users.website,";
            sql += " Users.youtube, Users.email, Users.linkedin, Users.instagram, Users.username,";
            sql += " Users.snapchat, Users.tiktok, Users.facebook, Users.fax, ";
            sql += " Users.twitter, (P.name + ' , ' + AD.details) AS Address, Users.phone,";
            sql += " Users.whatsapp, Users.description, Users.password from Users";
            sql += " inner join Addresses AS AD on AD.id = Users.address_id";
            sql += " inner join Provinces AS P on AD.province_id = p.id";
            sql += " inner join Images AS I on Users.id = I.referral_id";
            sql += " Where Users.id = @UID ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@UID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null)
            {
                DataRow user = dataTable.Rows[0];
                UsersModel users = new UsersModel();
                users.Name = user["Name"].ToString();
                users.Email = user["Email"].ToString();
                users.Password = user["Password"].ToString();
                users.UserName = user["UserName"].ToString();
                users.Address = new AddressModel();
                users.Address.AddressName = user["Address"].ToString();
                users.Phone = user["Phone"].ToString();
                users.Website = user["Website"].ToString();
                users.Youtube = user["Youtube"].ToString();
                users.Linkedin = user["Linkedin"].ToString();
                users.Instagram = user["Instagram"].ToString();
                users.Twitter = user["Twitter"].ToString();
                users.Snapchat = user["Snapchat"].ToString();
                users.Tiktok = user["Tiktok"].ToString();
                users.Facebook = user["Facebook"].ToString();
                users.Whatsapp = user["Whatsapp"].ToString();
                users.Fax = user["Fax"].ToString();
                users.Description = user["Description"].ToString();
                users.Image = new ImagesModel()
                {
                    URL = user["URL"].ToString()
                };

                return PartialView(users);
            }
            else
            {
                return PartialView(Resources.CP_Users.NoUser);
            }
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            UsersModel Edit_user = new UsersModel();
            Edit_user.ID = new Guid(Request.Params["id"]);
            Edit_user.Name = Request.Params["full_name"] != null ? Request.Params["full_name"] : "";
            Edit_user.Email = Request.Params["email"] != null ? Request.Params["email"] : "";
            Edit_user.Phone = Request.Params["phone"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phone"]) ? "+" + Request.Params["phone_key"] + Request.Params["phone"] : null;
            Edit_user.Whatsapp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            Edit_user.Password = Request.Params["password"] != null ?Ciphering.GetMD5HashData( Request.Params["password"]) : "";
            Edit_user.Website = Request.Params["website"] != null ? Request.Params["website"] : "";
            Edit_user.Youtube = Request.Params["youtube"] != null ? Request.Params["youtube"] : "";
            Edit_user.Linkedin = Request.Params["linkedin"] != null ? Request.Params["linkedin"] : "";
            Edit_user.Instagram = Request.Params["instagram"] != null ? Request.Params["instagram"] : "";
            Edit_user.Twitter = Request.Params["twitter"] != null ? Request.Params["twitter"] : "";
            Edit_user.Snapchat = Request.Params["snapchat"] != null ? Request.Params["snapchat"] : "";
            Edit_user.Tiktok = Request.Params["tiktok"] != null ? Request.Params["tiktok"] : "";
            Edit_user.Facebook = Request.Params["facebook"] != null ? Request.Params["facebook"] : "";
            Edit_user.Fax = Request.Params["fax"] != null ? Request.Params["fax"] : "";
            Edit_user.Description = Request.Params["description"] != null ? Request.Params["description"] : "";
            Edit_user.UserName = Request.Params["username"] != null ? Request.Params["username"] : "";

            Edit_user.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                Edit_user.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                Edit_user.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                Edit_user.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                Edit_user.Address.ProvinceId = new Guid();
            }

            if (ISValid(Edit_user, false, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput;
                object[] valsinput;
                if(Edit_user.Password == string.Empty)
                {
                    colsinput = new string[] { "full_name", "email", "phone",  "website", "youtube", "linkedin", "instagram", "twitter", "snapchat", "tiktok", "facebook", "whatsapp", "fax", "username", "description", "updated_at" };
                    cols.AddRange(colsinput);
                    valsinput = new object[] { Edit_user.Name, Edit_user.Email, Edit_user.Phone,  Edit_user.Website, Edit_user.Youtube, Edit_user.Linkedin, Edit_user.Instagram, Edit_user.Twitter, Edit_user.Snapchat, Edit_user.Tiktok, Edit_user.Facebook, Edit_user.Whatsapp, Edit_user.Fax, Edit_user.UserName, Edit_user.Description, DateTime.Now };
                    vals.AddRange(valsinput);
                }
                else
                {
                    colsinput = new string[] { "full_name", "email", "phone", "password", "website", "youtube", "linkedin", "instagram", "twitter", "snapchat", "tiktok", "facebook", "whatsapp", "fax", "username", "description", "updated_at" };
                    cols.AddRange(colsinput);
                    valsinput = new object[] { Edit_user.Name, Edit_user.Email, Edit_user.Phone, Edit_user.Password, Edit_user.Website, Edit_user.Youtube, Edit_user.Linkedin, Edit_user.Instagram, Edit_user.Twitter, Edit_user.Snapchat, Edit_user.Tiktok, Edit_user.Facebook, Edit_user.Whatsapp, Edit_user.Fax, Edit_user.UserName, Edit_user.Description, DateTime.Now };
                    vals.AddRange(valsinput);
                }
               
                string errMessage = string.Empty;
                if (Database.UpdateRow("Users", Edit_user.ID, cols, vals, out errMessage))
                {
                    Database.DeleteRow("Addresses", Edit_user.Address.AddressId, out msg);
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "updated_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { Edit_user.Address.ProvinceId, Edit_user.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Users", Edit_user.ID, cols, vals, out errMessage);

                    if (Session["Attachment"] != null)
                    {
                        //Get the ID and URL 
                        Guid ImageID = new Guid(Request.Params["image_id"]);
                        string ImageURL = Request.Params["image_url"] != null ? "/Images/Users/" + Request.Params["image_url"] : "/Images/dafault.png";
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
                        FileName = Server.MapPath("~/Images/Users/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);

                        cols = new List<string>();
                        vals = new List<object>();

                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "updated_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, Edit_user.ID, "Users", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Users.Edited });
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
            sql += " select U.id AS ID,I.url AS URL,I.id as IID, U.full_name AS FullName, ";
            sql += " U.website, U.youtube, U.linkedin, ";
            sql += " U.instagram, U.snapchat, U.tiktok, ";
            sql += " U.facebook, U.fax, U.twitter, U.email,";
            sql += " U.username, U.password,";
            sql += " AD.id AS AddressID, AD.details AS AddressDetailes,U.phone,";
            sql += " U.whatsapp, U.description, P.id AS CityID,";
            sql += " P.country_id AS CountryID from Users  AS U ";
            sql += " inner join Addresses AS AD on AD.id = U.address_id ";
            sql += " inner join Provinces AS P on AD.province_id = p.id ";
            sql += " inner join Images AS I on U.id = I.referral_id Where U.id=@UID";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@UID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow user_ = dataTable.Rows[0];
                UsersModel user = new UsersModel();
                user.ID = new Guid(user_["ID"].ToString());
                user.Name = user_["FullName"].ToString();
                user.Email = user_["Email"].ToString();
                user.UserName = user_["Username"].ToString();
                user.Phone = user_["Phone"].ToString();
                user.Website = user_["Website"].ToString();
                user.Youtube = user_["Youtube"].ToString();
                user.Linkedin = user_["Linkedin"].ToString();
                user.Instagram = user_["Instagram"].ToString();
                user.Twitter = user_["Twitter"].ToString();
                user.Snapchat = user_["Snapchat"].ToString();
                user.Tiktok = user_["Tiktok"].ToString();
                user.Facebook = user_["Facebook"].ToString();
                user.Whatsapp = user_["Whatsapp"].ToString();
                user.Fax = user_["Fax"].ToString();
                user.Description = user_["Description"].ToString();
                user.Image = new ImagesModel()
                {
                    URL = user_["URL"].ToString(),
                    ID = new Guid(user_["IID"].ToString())
                };
                user.Address = new AddressModel()
                {
                    AddressId = new Guid(user_["AddressID"].ToString()),
                    AddressName = user_["AddressDetailes"].ToString(),
                    ProvinceId = new Guid(user_["CityID"].ToString()),
                    CountryId = new Guid(user_["CountryID"].ToString())
                };

                user.URL = new URLModel
                {
                    Refresh = "/CP_Users/GetAll/",
                    Edit = "/CP_Users/Editing/"
                };
                return PartialView(user);
            }

            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Users", new Guid(ID), out msg))
            {
                List<SqlParameter> li = new List<SqlParameter>();
                li.Add(new SqlParameter("@UID", new Guid(ID)));
                Database.ReadTableByQuery("DELETE FROM Images Where referral_id = @UID ", li, out msg);
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Users.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }

        }

        bool ISValid(UsersModel user, bool Is_Add, out string msg)
        {
            bool flag = true;
            if (user.Name == "")
            {
                msg = Resources.CP_Users.EnterNamePlease;
                return false;
            }
            if (user.UserName == "")
            {
                msg = Resources.CP_Users.EnterUserPlease;
                return false;
            }
            if (user.Email == "" && user.Phone == null)
            {
                msg = Resources.CP_Users.EnterEmailOrPhonePlease;
                return false;
            }
            if (user.Password == "" && Is_Add)
            {
                msg = Resources.CP_Users.EnterPasswordPlease;
                return false;
            }
            if (user.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP_Users.EnterCityPlease;
                return false;
            }
            if (user.Address.AddressName == "")
            {
                msg = Resources.CP_Users.EnterAddressPlease;
                return false;
            }
            if (user.Whatsapp == null)
            {
                msg = Resources.CP_Users.EnterWhatsAppPlease;
                return false;
            }
            if (user.Description == "")
            {
                msg = Resources.CP_Users.EnterDescrptionPlease;
                return false;
            }
            if (Session["Attachment"] == null && Is_Add)
            {
                msg = Resources.CP_Users.EnterImagePlease;
                return false;
            }
            msg = "";
            return flag;
        }

    }
}
