using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models;
using SGAW_ECHO.Models.API.Address;
using SGAW_ECHO.Models.CP.University;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static SGAW_ECHO.Classes.HelperClass;

namespace SGAW_ECHO.Controllers
{
    public class CP_UniversitiesController : BaseController
    {
        // GET: CP_Universities
        public ActionResult Index()
        {
            ViewBag.ControllerName = "CP_Universities";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(BaseUrl+ "/University/GetAllUniversities");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<List<GetUniversityModel>> universities = JsonConvert.DeserializeObject<apiJson<List<GetUniversityModel>>>(result);
                if (universities.code == 200)
                {
                    return View(universities.data);
                }

                ViewBag.msg = universities.msg;
                return View();
            }
        }


        public PartialViewResult Content()
        {
            ViewBag.ControllerName = "CP_Universities";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(BaseUrl + "/University/GetAllUniversities");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<List<GetUniversityModel>> universities = JsonConvert.DeserializeObject<apiJson<List<GetUniversityModel>>>(result);
                if (universities.code == 200)
                {
                    return PartialView(universities.data);
                }

                ViewBag.msg = universities.msg;
                return PartialView();
            }


        }



        [HttpPost]
        public PartialViewResult Add()
        {
            ViewBag.ControllerName = "CP_Universities";
            return PartialView();
        }

        //public JsonResult AddUser()
        //{
        //    Session["error"] = null;
        //    AddUserModel New_user = new AddUserModel();
        //    New_user.FullName = Request.Params["fullname"] != null ? Request.Params["fullname"] : "";
        //    New_user.UserName = Request.Params["username"] != null ? Request.Params["username"] : "";
        //    New_user.Email = Request.Params["email"] != null ? Request.Params["email"] : "";
        //    New_user.Password = Request.Params["password"] != null ? Request.Params["password"] : "";
        //    New_user.Phone = Request.Params["phone"] != null ? Request.Params["phone"] : "";
        //    New_user.UserTypeID = "02DD85FE-6F1B-41EE-BEE6-DF82161467CE";
        //    New_user.Bio = Request.Params["bio"] != null ? Request.Params["bio"] : "";

        //    AddAddress address = new AddAddress();
        //    address.Descreption = Request.Params["address"] != null ? Request.Params["address"] : "";
        //    address.Neighborhood_ID = Request.Params["city"] != null ? new Guid(Request.Params["city"]) : new Guid();
        //    New_user.Address = address;
        //    New_user.Token = HelperClass.RandomString(50);
        //    // string json = user.AddUser().ToString();
        //    //userJson u = JsonConvert.DeserializeObject<userJson>(json);
        //    var json_ = new JavaScriptSerializer().Serialize(New_user);
        //    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/User/AddUser");
        //    httpWebRequest.ContentType = "application/json";
        //    httpWebRequest.Method = "POST";
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    {

        //        streamWriter.Write(json_);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }

        //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();
        //        apiJson<GetUniversityModel> universities = JsonConvert.DeserializeObject<apiJson<GetUniversityModel>>(result);
        //        if (user.code == 200)
        //        {
        //            return Json(new { @code = 200, @msg = "added" });
        //        }
        //        else
        //            return Json(new { @code = 404, @msg = user.msg });
        //    }

        //}


        //
        //[HttpPost]
        //public PartialViewResult Details(string UID)
        //{
        //    string sql = "";
        //    sql += " select Users.id AS ID,I.url AS URL, Users.full_name AS Name,Users.website AS Website, ";
        //    sql += " Users.youtube AS Youtube,Users.email AS Email, Users.linkedin AS Linkedin, Users.instagram AS Instagram, Users.username AS UserName,";
        //    sql += " Users.snapchat AS Snapchat, Users.tiktok AS Tiktok, Users.facebook AS Facebook, Users.fax AS Fax, ";
        //    sql += " Users.twitter AS Twitter, (P.name + ' , ' + AD.details) AS Address, Users.phone AS Phone,";
        //    sql += " Users.whatsapp AS Whatsapp ,Users.description AS Description, Users.password AS Password";
        //    sql += " from Users";
        //    sql += " left join Addresses AS AD on AD.id = Users.address_id";
        //    sql += " left join Provinces AS P on AD.province_id = p.id";
        //    sql += " left join Images AS I on Users.id = I.referral_id";
        //    sql += " Where Users.id = @UID ";
        //    List<SqlParameter> li = new List<SqlParameter>();
        //    li.Add(new SqlParameter("@UID", UID));
        //    string msg = "";
        //    DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
        //    if (dataTable != null)
        //    {
        //        DataRow user = dataTable.Rows[0];
        //        UserModel users = new UserModel();

        //        users.Name = user["Name"].ToString();
        //        users.Email = user["Email"].ToString();
        //        users.Password = user["Password"].ToString();
        //        users.UserName = user["UserName"].ToString();
        //        users.Address = new AddressModel();
        //        users.Address.AddressName = user["Address"].ToString();
        //        users.Phone = user["Phone"].ToString();
        //        users.Website = user["Website"].ToString();
        //        users.Youtube = user["Youtube"].ToString();
        //        users.Linkedin = user["Linkedin"].ToString();
        //        users.Instagram = user["Instagram"].ToString();
        //        users.Twitter = user["Twitter"].ToString();
        //        users.Snapchat = user["Snapchat"].ToString();
        //        users.Tiktok = user["Tiktok"].ToString();
        //        users.Facebook = user["Facebook"].ToString();
        //        users.Whatsapp = user["Whatsapp"].ToString();
        //        users.Fax = user["Fax"].ToString();
        //        users.Description = user["Description"].ToString();
        //        users.Image = new ImageModel()
        //        {
        //            URL = user["URL"].ToString()
        //        };

        //        return PartialView(users);
        //    }
        //    else
        //    {
        //        return PartialView(Resources.CP.NoUser);
        //    }
        //}

        [HttpPost]
        public JsonResult EditUser()
        {

            Session["error"] = null;
            AddUserModel edite_User = new AddUserModel();
            edite_User.FullName = Request.Params["fullname"] ?? "";
            edite_User.UserName = Request.Params["username"] ?? "";
            edite_User.Email = Request.Params["email"] ?? "";
            edite_User.Password = Request.Params["password"] ?? "";
            edite_User.Phone = Request.Params["phone"] ?? "";
            edite_User.Bio = Request.Params["bio"] ?? "";

            AddAddress address = new AddAddress();
            address.Descreption = Request.Params["address"] ?? "";
            address.Neighborhood_ID = Request.Params["city"] != null ? new Guid(Request.Params["city"]) : new Guid();
            //edite_User.Address = address;
            edite_User.Token = RandomString(50);
            // string json = user.AddUser().ToString();
            //userJson u = JsonConvert.DeserializeObject<userJson>(json);
            var json_ = new JavaScriptSerializer().Serialize(edite_User);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/User/AddUser");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpPostedFileBase currentFile = Request.Files[0];
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(json_);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<UserModel> user = JsonConvert.DeserializeObject<apiJson<UserModel>>(result);
                if (user.code == 200)
                {
                    return Json(new { @code = 200, @msg = "added" });
                }
                else
                    return Json(new { @code = 404, @msg = user.msg });
            }

        }

        [HttpPost]
        public PartialViewResult Edit(string UID)
        {
            UserModel user = new UserModel();
            user.Address = "";
            user.Bio = "";
            user.Email = "";
            user.FullName = "";
            user.Password = "";
            user.Phone = "";
            user.UserName = "";

            if (UID != string.Empty)
            {
                return PartialView(user);
            }
            else
            {
                return PartialView("no data");
            }
        }

        //[HttpPost]
        //public JsonResult DeleteUser(string id)
        //{
        //    string msg = "";
        //    int code = 0;
        //    if (Database.DeleteRow("Users", new Guid(id), out msg))
        //    {
        //        List<SqlParameter> li = new List<SqlParameter>();
        //        li.Add(new SqlParameter("@UID", new Guid(id)));
        //        Database.ReadTableByQuery("DELETE FROM Images Where referral_id = @UID ", li, out msg);
        //        code = 200;
        //        return Json(new { @code = code.ToString(), @msg = msg });
        //    }
        //    else
        //    {
        //        code = 404;
        //        msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
        //        return Json(new { @code = code.ToString(), msg = msg });
        //    }

        //}

    }
}