using legarage.Classes;
using legarage.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Resources;
using System.Resources;

namespace legarage.Controllers
{
    public class UsersController : BaseController
    {


        // GET: User
        public ActionResult Index()
        {
            if (Tools.FindCurrentUser(out DataRow user))
                return RedirectToAction("Index", "Home");

            return View();
        }
        // GET: Logout
        public ActionResult LogOut()
        {
            Session["token"] = null;
            Session["Username"] = null;
            Session["id"] = null;
            HttpCookie ck = new HttpCookie("token", "");
            ck.Expires = DateTime.Now.AddMonths(-6);
            Response.Cookies.Add(ck);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult Login()
        {
            string username = Request.Params["username"] ?? "";
            string password = Request.Params["password"] ?? "";
            string msg = "";
            DataRow dtr = Database.FindRow("Users", "email", username);
            if (dtr == null) dtr = Database.FindRow("Users", "phone", username);

            if (dtr != null)
            {
                if (dtr["password"].ToString() == Ciphering.GetMD5HashData(password) || dtr["password"].ToString() == password)
                {
                    string Token;
                    if (dtr["token"] != null)
                    {
                        Token = dtr["token"].ToString();
                    }
                    else
                    {
                        Token = HelperClass.RandomString(50);
                        Guid guid = new Guid(dtr["id"].ToString());
                        List<string> cols = new List<string>();
                        cols.Add("Token");
                        List<object> vals = new List<object>();
                        vals.Add(Token);
                        msg = "";
                        try { Database.UpdateRow("users", guid, cols, vals, out msg); } catch { }
                    }

                    if (Convert.ToBoolean(Request.Params["rememberme"]) == true)
                    {
                        HttpCookie ck = new HttpCookie("token", Token);
                        ck.Expires = DateTime.Now.AddMonths(6);
                        Response.Cookies.Add(ck);
                    }
                    else
                    {
                        HttpCookie ck = new HttpCookie("token", Token);
                        ck.Expires = DateTime.Now.AddHours(24);
                        Response.Cookies.Add(ck);
                    }

                    Session["token"] = Token;
                    Session["Username"] = dtr["username"];
                    Session["id"] = dtr["id"];

                    //return RedirectToAction("Index", "Home");
                    //return View("Index");
                    return Json(new { code = "200", msg = "success" });//success  You have successfully logged in


                }
                else
                {
                    //if password not correct 
                    string err = Resources.Users.PasswordIncorrect;// "كلمة المرور غير صحيحة";
                    //Session["error"] = err;
                    //return View("Index");
                    msg = err;

                    return Json(new { code = "200", msg = msg });
                }
            }
            else
            {
                //if username not exite
                string err = "اسم المستخدم غير صحيحة";
                //Session["error"] = err;
                //return View("Index");
                msg = err;
                return Json(new { code = "200", msg = msg });
            }

        }
        //[HttpPost]
        public ActionResult Register()
        {
            if (Tools.FindCurrentUser(out DataRow user))
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public JsonResult NewUser()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            UsersModel New_user = new UsersModel();
            New_user.Name = Request.Params["fullname"] != null ? Request.Params["fullname"] : "";
            New_user.UserName = Request.Params["username"] != null ? Request.Params["username"] : "";
            New_user.Email = Request.Params["email"] != null ? Request.Params["email"] : "";
            New_user.Password = Request.Params["password"] != null ? Ciphering.GetMD5HashData(Request.Params["password"]) : "";
            New_user.Phone = Request.Params["phoneno"].ToString() != string.Empty ? Request.Params["phone_key"] + Request.Params["phoneno"] : null;
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
            if (Request.Params["country"] != null && Request.Params["country"] != "-1")
            {
                New_user.Address.CountryId = new Guid(Request.Params["country"].ToString());
            }

            New_user.Address.AddressName = Request.Params["Address"] ?? "";
            New_user.Fax = Request.Params["fax"] ?? "";
            New_user.Website = Request.Params["site"] ?? "";
            New_user.Facebook = Request.Params["facebook"] ?? "";
            New_user.Whatsapp = Request.Params["whatsapp"] ?? "";
            New_user.IsAdmin = 0;
            //if (Request.Params["phoneno"] != null && Request.Params["phoneno"] != "")
            //{
            //    New_user.Address.= Request.Params["phone_key"] + Request.Params["phoneno"]; //Request.Params["Phone"].ToString() != string.Empty ?
            //}
            //else
            //{
            //    New_user.Address.ProvinceId = new Guid();
            //}
            New_user.Twitter = Request.Params["twitter"] ?? "";
            New_user.Instagram = Request.Params["instagram"] ?? "";
            New_user.Youtube = Request.Params["youtube"] ?? "";
            New_user.Linkedin = Request.Params["linkedin"] ?? "";
            New_user.Snapchat = Request.Params["snapchat"] ?? "";
            New_user.Tiktok = Request.Params["tiktok"] ?? "";
            New_user.Description = Request.Params["desc"] ?? "";
            New_user.Image = new ImagesModel() { URL = "" };


            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                New_user.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                New_user.Address.ProvinceId = new Guid();
            }
            if (ISValid(New_user, out msg))
            {
                //string Token = HelperClass.RandomString(50);
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
                    msg = Resources.CP.Added;
                    Session["token"] = New_user.Token;
                    Session["Username"] = New_user.UserName;//["username"];
                    Session["id"] = New_user.ID;//["id"];
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

        public ActionResult Forgotpassword()
        {
            return View();
        }

        public ActionResult Resetpassword(string UserMail ="test@domain.tld")
        {
            //Send Email
            string MailAddress = "senderEmailId@gmail.com";
            string MailPass = "password";
            //if (legarage.Classes.Tools.FindCurrentUser(out DataRow user))
            //{
            //    UserMail = user["email"].ToString();
            //}
            string Host = "smtp.gmail.com";


            MailMessage Msg = new MailMessage();
            Msg.From = new MailAddress(MailAddress, "Le-Garage");// Sender details here, replace with valid value
            Msg.Subject = Resources.Users.PasswordResetRequest; // subject of email
            Msg.To.Add(UserMail); //Add Email id, to which we will send email
            Msg.Body = "EmailMessage.Text";
            Msg.IsBodyHtml = true;
            Msg.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false; // to get rid of error "SMTP server requires a secure connection"
            smtp.Host = Host;
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential(MailAddress, MailPass);// replace with valid value
            smtp.EnableSsl = true;
            smtp.Timeout = 20000;

            smtp.Send(Msg);
            return Json("Code has been sent");
        }
        public ActionResult MyProfile()
        {
            if (!Tools.FindCurrentUser(out DataRow userRow))
                return RedirectToAction("Index", "Users");

            DataRow imgRow = Database.FindRow("Images", "referral_id", userRow["id"].ToString());
            //DataRow AdressRow = Database.FindRow("Addresses", "id", userRow["address_id"].ToString());
            //DataRow ProvincesRow = Database.FindRow("Provinces", "id", AdressRow["province_id"].ToString());
            //DataRow CountryRow = Database.FindRow("Images", "id", ProvincesRow["country_id"].ToString());

            string AdressQurey = @"SELECT P.name city
		                                  ,C.Name country
                                          ,details
                                      FROM Addresses A
                                      INNER JOIN Provinces P ON P.id= A.province_id
                                      INNER JOIN Countries C ON C.Id =P.country_id
                                      WHERE A.id = @address";


            UsersModel model = new UsersModel() {
                ID= new Guid(userRow["id"].ToString()),
                Name = userRow["full_name"].ToString(),
                Email = userRow["email"].ToString(),
                Phone = userRow["phone"].ToString(),
                Website = userRow["website"].ToString(),
                Youtube = userRow["youtube"].ToString(),
                Linkedin = userRow["linkedin"].ToString(),
                Instagram = userRow["instagram"].ToString(),
                Twitter = userRow["twitter"].ToString(),
                Snapchat = userRow["snapchat"].ToString(),
                Tiktok = userRow["tiktok"].ToString(),
                Whatsapp = userRow["whatsapp"].ToString(),
                Fax = userRow["fax"].ToString(),
                Facebook = userRow["facebook"].ToString(),
                Image = new ImagesModel() { URL = "unknown-person-profile.jpg" },
                UserName  = userRow["username"].ToString(),
                Address = new AddressModel() { AddressName = "", Country = "", Province = "" }
            };
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter("@address", userRow["address_id"].ToString())
            };
            DataTable AdressTable = Database.ReadTableByQuery(AdressQurey, parameters, out string msg);
            if (AdressTable != null && AdressTable.Rows.Count > 0)
            {
                DataRow AdressRow = AdressTable.Rows[0];
                model.Address = new AddressModel() { AddressName = AdressRow["details"].ToString(), Country = AdressRow["country"].ToString(), Province = AdressRow["city"].ToString() };

            }
            if (imgRow != null)
            {
                model.Image = new ImagesModel() { URL = imgRow["url"].ToString() };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Updating()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            UsersModel UpdateProfile = new UsersModel();
            UpdateProfile.ID = new Guid(Request.Params["ID"].ToString());
            UpdateProfile.Name = Request.Params["Name"] != null ? Request.Params["Name"] : "";
            UpdateProfile.Phone = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            UpdateProfile.Whatsapp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            UpdateProfile.Fax = Request.Params["Fax"] != null ? Request.Params["Fax"] : "";
            UpdateProfile.Description = Request.Params["Description"] ?? "";
            UpdateProfile.Website = Request.Params["Website"] ?? "";
            UpdateProfile.Facebook = Request.Params["Facebook"] ?? "";
            UpdateProfile.Tiktok = Request.Params["Tiktok"] ?? "";
            UpdateProfile.Snapchat = Request.Params["Snapchat"] ?? "";
            UpdateProfile.Twitter = Request.Params["Twitter"] ?? "";
            UpdateProfile.Instagram = Request.Params["Instagram"] != null ? Request.Params["Instagram"] : "";
            UpdateProfile.Linkedin = Request.Params["Linkedin"] ?? "";
            UpdateProfile.Youtube = Request.Params["Youtube"] ?? "";
            //UpdateProfile.Keywords = Request.Params["Keywords"] != null ? Request.Params["Keywords"] : "";
            //if (Session["Is_Admin"].ToString() != "1")
            //{
            //    DataRow user;
            //    Tools.FindCurrentUser(out user);
            //    UpdateProfile.User = new UsersModel()
            //    {
            //        ID = new Guid(user["id"].ToString())
            //    };
            //}
            //else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            //{
            //    UsersModel user = new UsersModel();
            //    user.ID = new Guid(Request.Params["User"].ToString());
            //    UpdateProfile.User = user;
            //}
            //else
            //{
            //    UpdateProfile.User = null;
            //}

            UpdateProfile.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                UpdateProfile.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                UpdateProfile.Address.AddressName = "";
            }

            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                UpdateProfile.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                UpdateProfile.Address.ProvinceId = new Guid();
            }

            if (ISValid(UpdateProfile, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = {  "name", "mobile", "fax", "whatsapp", "facebook", "tiktok", "snapchat", "twitter", "instagram", "linkedin", "youtube", "website", "description", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = {  UpdateProfile.Name, UpdateProfile.Phone, UpdateProfile.Fax, UpdateProfile.Whatsapp, UpdateProfile.Facebook, UpdateProfile.Tiktok, UpdateProfile.Snapchat, UpdateProfile.Twitter, UpdateProfile.Instagram, UpdateProfile.Linkedin, UpdateProfile.Youtube, UpdateProfile.Website, UpdateProfile.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Garages", UpdateProfile.ID, cols, vals, out errMessage))
                {
                    Database.DeleteRow("Addresses", UpdateProfile.Address.AddressId, out msg);
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "updated_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { UpdateProfile.Address.ProvinceId, UpdateProfile.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Garages", UpdateProfile.ID, cols, vals, out errMessage);
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
                        valsinput = new object[] { 1, ImageName, UpdateProfile.ID, "Garages", DateTime.Now };
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
        public JsonResult Send_me_Code()
        {


            string email;
            email = (Request.Params["valied_email"] ?? string.Empty).ToString();
            if (email == string.Empty)
            {
                return Json(new { @code = 404, @msg = Resources.Users.valid_email });
            }
            DataRow dtr = Database.FindRow("Users", "Email", email);
            if (dtr != null)
            {
                Session["temp-email"] = email;
                string verfication_code = HelperClass.RandomString(6);
                BodyBuilder body = new BodyBuilder();
                string url = Server.MapPath("~/Registiration/Forget_Password");
                body.HtmlBody = "<h2> " + "Resources.Users.verification_code" + " : " + verfication_code + " <br/>" +
                    "<a href=\"" + url + "\" >" + "Resources.Users.chng_password" + " </a> ";
                string errMessage = "";
                if (EmailManager.SendEmail("Forget Password", body.HtmlBody, email, out errMessage))
                {
                    Session["temp_Token"] = dtr["token"];
                    Session["forget_password_code"] = verfication_code;
                    return Json(new { @code = 200, @msg = Resources.Users.code_sent });
                }
                else
                {
                    return Json(new { @code = 404, @msg = "Resources.Users.code_not_sent" + "<br/>" + errMessage });
                }

            }
            else
            {
                return Json(new { @code = 404, @msg = "Resources.Users.email_not_found " });
            }

        }

        [HttpPost]
        public ActionResult Check_code()
        {


            string verfication_code;
            verfication_code = Request.Params["verfication_code"]?.ToString();

            if (verfication_code == string.Empty)
            {
                ViewBag.msg = "valid_code";
            }
            if (verfication_code != Session["forget_password_code"].ToString())
            {
                ViewBag.msg = "worng_code";
            }
            else
            {
                Session["forget_password_code"] = "true";

            }
            return View("ForgotPassword");
        }



        [HttpPost]
        public JsonResult resend_code()
        {

            string email = Session["temp-email"].ToString();
            email = Request.Params["valied_email"].ToString();
            string verfication_code = HelperClass.RandomString(6);
            BodyBuilder body = new BodyBuilder();
            string url = Server.MapPath("~/Registiration/Forget_Password");
            body.HtmlBody = "<h2> " + "Resources.Users.verification_code" + " : " + verfication_code + " <br/>" +
                    "<a href=\"" + url + "\" >" + Resources.Users.chng_password + " </a> ";
            string errMessage = "";
            if (EmailManager.SendEmail("Forget Password", body.HtmlBody, email, out errMessage))
            {
                Session["forget_password_code"] = verfication_code;
                return Json(new { @code = 200, @msg = Resources.Users.code_sent });
            }
            else
            {
                return Json(new { @code = 404, @msg = "Resources.Users.code_not_sent" + "<br/>" + errMessage });
            }

        }

        [HttpPost]
        public JsonResult reset_Password()
        {
            string msg = "";
            string new_password;
            string confirm_password;
            new_password = Request.Params["new-password"].ToString();
            confirm_password = Request.Params["conf-password"].ToString();
            if (new_password == string.Empty)
            {
                return Json(new { @code = 404, @msg = @Resources.Users.valid_password });
            }
            if (confirm_password == string.Empty)
            {
                return Json(new { @code = 404, @msg = @Resources.Users.valid_conf_pass });
            }

            if (new_password != confirm_password)
            {
                return Json(new { @code = 404, @msg = @Resources.Users.valid_pass_confpass });
            }
            var Token = Session["temp_Token"] ?? "";
            DataRow dr = Database.FindRow("Users", "Token", Token);
            if (dr != null)
            {

                Guid ID = (Guid)dr["ID"];
                List<string> cols = new List<string>();
                cols.Add("Password");
                List<object> vals = new List<object>();
                vals.Add(Ciphering.GetMD5HashData(new_password));
                if (Database.UpdateRow("Users", ID, cols, vals, out msg))
                {

                    Session["temp_Token"] = null;
                    Session["forget_password_code"] = "done";
                    return Json(new { @code = 200, @msg = "Resources.errors.password_changed " });
                }
                else
                {
                    return Json(new { @code = 404, @msg = msg });
                }
            }
            else
            {
                return Json(new { @code = 404, @msg = " Resources.Users.Uncompleated_process" });
            }

        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ChangeProfilePic()
        {
            Guid USID;
                if (!Tools.FindCurrentUser(out DataRow userRow))
                {
                    return Json(new { @msg = Response.StatusDescription = "Unauthorized!", @code = Response.StatusCode = 401 },
                        JsonRequestBehavior.AllowGet);
                }
                USID = new Guid(userRow["id"].ToString());

                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file.ContentType == null)
                    {
                        return Json(new { @msg = Response.StatusDescription = "Choose an Image", @code = Response.StatusCode = 404 }, JsonRequestBehavior.AllowGet);
                    }
                    string type = file.ContentType.Split('/')[0];
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    if (type == "image")
                    {
                        /// check if there is an old image
                        string sql_ = "select url from images where referral_id = @UID and referral_type = 'Users' and  is_main = 1";
                        List<SqlParameter> li = new List<SqlParameter>();
                        li.Add(new SqlParameter("@UID", USID));
                        string msg = "";
                        DataTable dataTable = Database.ReadTableByQuery(sql_, li, out msg);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            DataRow oldImg = dataTable.Rows[0];
                            List<string> cols1 = new List<string>() { "referral_id", "referral_type", "is_main" };
                            List<Object> vals1 = new List<object>() { USID, "Users", 1 };


                            string errMessage1 = string.Empty;
                            Database.DeleteRow("images", cols1, vals1, out errMessage1);
                            string oldUrl = oldImg["url"].ToString();
                            oldUrl = Server.MapPath("~/Images/Users/Profile/" + oldUrl);
                            if (System.IO.File.Exists(oldUrl))
                            {
                                System.IO.File.Delete(oldUrl);
                            }

                        }

                        string FileName = file.FileName;
                        Guid image_id = Guid.NewGuid();
                        DataRow temp = Database.GetRow("Images", image_id);
                        while (temp != null)
                        {
                            image_id = Guid.NewGuid();
                            temp = Database.GetRow("Images", image_id);
                        }
                        // uploade image to server
                        FileName = image_id.ToString() + System.IO.Path.GetExtension(FileName);
                        string dir = Server.MapPath("~/Images/Users/Profile/");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        // FileName = dir + FileName;
                        System.IO.File.WriteAllBytes(dir + FileName, data);
                        List<string> cols = new List<string>();
                        List<object> vals = new List<object>();


                        string[] colsinput = new string[] { "url", "is_main", "referral_id", "referral_type", "created_at", "updated_at" };
                        cols.AddRange(colsinput);

                        object[] valsinput = new object[] { FileName, 1, USID, "Users", DateTime.Now, DateTime.Now };
                        vals.AddRange(valsinput);

                        string errMessage = string.Empty;


                        //add image to data base
                        if (Database.InsertRow("Images", image_id, cols, vals, out errMessage))
                            return Json(new { @data = Response.StatusDescription = "The image successfully added", @code = Response.StatusCode = 201 }, JsonRequestBehavior.AllowGet);
                        else
                        {
                            return Json(new { @msg = Response.StatusDescription = "Failed to add image!" + errMessage, @code = Response.StatusCode = 400 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { @msg = Response.StatusDescription = "Enter a valied Image", @code = Response.StatusCode = 406 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { @msg = Response.StatusDescription = "Image is missing!", @code = Response.StatusCode = 404 }, JsonRequestBehavior.AllowGet);
            }

        [HttpGet]
        public ActionResult Change_Password()
        {
            if (!Tools.FindCurrentUser(out DataRow user))
                return RedirectToAction("Users", "Index");
            return View();
        }

        [HttpPost]
        public JsonResult Change_Password(string old_password,string new_password, string confirm_password)
        {
            string msg = "";

            new_password = Request.Params["new-password"].ToString();
            confirm_password = Request.Params["conf-password"].ToString();
            if (string.IsNullOrWhiteSpace(old_password))
            {
                return Json(new { @code = 404, @msg = @Resources.Users.valid_password });
            }
            var Token = Session["Token"] ?? "";
            DataRow dr = Database.FindRow("Users", "Token", Token);

            if (dr != null)
            {
                if (new_password == string.Empty)
                {
                    return Json(new { @code = Response.StatusCode = 404, @msg = @Resources.Users.valid_password });
                }
                if (confirm_password == string.Empty)
                {
                    return Json(new { @code = Response.StatusCode = 404, @msg = @Resources.Users.valid_conf_pass });
                }

                if (new_password != confirm_password)
                {
                    return Json(new { @code = Response.StatusCode = 404, @msg = @Resources.Users.valid_pass_confpass });
                }
                if (dr["ID"]?.ToString()== old_password)
                {
                    Guid ID = (Guid)dr["ID"];
                    List<string> cols = new List<string>();
                    cols.Add("Password");
                    List<object> vals = new List<object>();
                    vals.Add(Ciphering.GetMD5HashData(new_password));
                    if (Database.UpdateRow("Users", ID, cols, vals, out msg))
                    {
                        return Json(new { @code = Response.StatusCode = 200, @msg = "Password changed successfully " });
                    }
                    else
                    {
                        return Json(new { @code = Response.StatusCode = 404, @msg = msg });
                    }
                }
                else
                {
                    return Json(new { @code = Response.StatusCode = 401, @msg = "Unauthorized!" });
                }

            }
            else
            {
                return Json(new { @code = 404, @msg = " you are not loging in " });
            }

        }

        bool ISValid(UsersModel user, out string msg)
        {
            bool flag = true;
            if (user.Name == "")
            {
                msg = Resources.CP.EnterFullName;
                return false;
            }
            if (user.UserName == "")
            {
                msg = Resources.CP.EnterUser;
                return false;
            }
            DataRow dtr = Database.FindRow("Users", "username", user.UserName);
            if (dtr != null)
            {
                msg = "The UserName is already in use";
                return false;
            }

            if (user.Email == "" && user.Phone == null)
            {
                msg = Resources.CP.EnterEmail + " or " + Resources.CP.EnterPhone;
                return false;
            }

             dtr = Database.FindRow("Users", "email", user.Email);
            if (dtr != null )
            {
                msg = "The email is already in use";
                return false;
            }
            dtr = Database.FindRow("Users", "phone", user.Phone);
            if (dtr != null)
            {
                msg = "The phone number is already in use";
                return false;
            }

            if (user.Password == "")
            {
                msg = Resources.CP.EnterPassword;
                return false;
            }

            if (Session["Attachment"] == null)
            {
                msg = Resources.CP.EnterImage;
                return false;
            }
            msg = "";
            return flag;
        }
    }
}