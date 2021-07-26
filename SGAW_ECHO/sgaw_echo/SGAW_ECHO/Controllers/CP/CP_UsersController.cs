using Newtonsoft.Json;
using SGAW_ECHO.Models;
using SGAW_ECHO.Models.CP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SGAW_ECHO.Classes;
using System.Net;
using SGAW_ECHO.Models.API.Neighborhoods;
using static SGAW_ECHO.Classes.HelperClass;


namespace SGAW_ECHO.Controllers
{
    public class CP_UsersController : BaseController
    {
        UserController user = new UserController();
        // GET: CP_Users

        public ActionResult Index()
        {
            try
            {
                ViewBag.ControllerName = "CP_Users";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/user/GetAllUsers");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "Get";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    apiJson<List<UserModel>> usersList = JsonConvert.DeserializeObject<apiJson<List<UserModel>>>(result);
                    if (usersList.code == 200)
                    {
                        return View(usersList.data);
                    }
                    else
                    {
                        ViewBag.msg = usersList.msg;
                        return View();
                    }
                }
            }
            catch
            {
                return Content("<h2 class=\"ml-auto mr-auto\"> Http Not Found</h2>");
            }

        }

        public ActionResult Content()
        {
            try
            {
                ViewBag.ControllerName = "CP_Users";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/user/GetAllUsers");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "Get";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    apiJson<List<UserModel>> userList = JsonConvert.DeserializeObject<apiJson<List<UserModel>>>(result);
                    if (userList.code == 200)
                    {
                        return PartialView(userList.data);
                    }
                    else
                    {
                        ViewBag.msg = userList.msg;
                        return PartialView();
                    }
                }
            }
            catch
            {
                return Content("<h2 class=\"ml-auto mr-auto\"> Http Not Found</h2>");
            }

        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                ViewBag.ControllerName = "CP_Users";
                return PartialView();
            }
            catch
            {
                return Content("<h2 class=\"ml-auto mr-auto\"> Http Not Found</h2>");
            }

        }

        [HttpPost]
        public JsonResult Adding(AddUserModel User)
        {
            try
            {
                User = new AddUserModel();
                User.FullName = Request.Form["fullname"] ?? "";
                User.UserName = Request.Form["username"]?? "";
                User.Email = Request.Form["email"] ?? "";
                User.Password = Request.Form["password"] ?? "";
                User.UserTypeID = "02DD85FE-6F1B-41EE-BEE6-DF82161467CE";
                User.Bio = Request.Form["bio"] ?? "";
                //TODO:change image path to /images/user
                if (Request.Files["profile"] != null)
                {
                    var file = Request.Files["profile"];
                    if (file.ContentType != null)
                    {
                        string type = file.ContentType.Split('/')[0];
                        var _fileName = file.FileName;
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();
                        string strFile = Convert.ToBase64String(data);
                        User.Image = new FileModel(){ Data=strFile, FileName= _fileName };;
                    }
                }
                User.Token = RandomString(50);
                var json_ = new JavaScriptSerializer().Serialize(User);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/User/AddUser");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
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
                    apiJson<Guid> user = JsonConvert.DeserializeObject<apiJson<Guid>>(result);
                    if(user.code==200)
                    {
                        return Json(new { @code = 200, @msg = "added" });
                    }
                    else
                        return Json(new { @code = 404, @msg = user.msg});
                }
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            //Session["error"] = null;


        }






        //get Edit PartialView
        [HttpGet]
        public ActionResult Edit(string ID)
        {
            try
            {
                ViewBag.ControllerName = "CP_Users";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/user/GetUserInformation?UserID=" + ID);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "Get";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    apiJson<UserModel> user = JsonConvert.DeserializeObject<apiJson<UserModel>>(result);
                    if (user.code == 200)
                    {
                        return PartialView(user.data);
                    }

                    ViewBag.msg = user.msg;
                    return PartialView();
                }
            }
            catch
            {
                return Content("<h2 class=\"ml-auto mr-auto\"> Http Not Found</h2>"); 
            }

        }

        // get: Details
        //get Edit PartialView
        [HttpGet]
        public ActionResult Details(string ID)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/user/GetUserInformation?UserID=" + ID);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "Get";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    apiJson<UserModel> user = JsonConvert.DeserializeObject<apiJson<UserModel>>(result);
                    if (user.code == 200)
                    {
                        return PartialView(user.data);
                    }

                    ViewBag.msg = user.msg;
                    return PartialView();
                }
            }
            catch
            {
                return Content("<h2 class=\"ml-auto mr-auto\"> Http Not Found</h2>");
            }

        }

        [HttpPost]
        public JsonResult Editing()
        {
            try
            {
                UserModel edite_User = new UserModel();
                edite_User.FullName = Request.Params["fullname"] ?? "";
                edite_User.UserName = Request.Params["username"] ?? "";
                edite_User.Email = Request.Params["email"] ?? "";
                edite_User.Password = Request.Params["password"] ?? "";
                edite_User.Bio = Request.Params["bio"] ?? "";
                var id = Request.Params["id"] ?? "";
                edite_User.ID= Guid.TryParse(id, out Guid temp)? temp: new Guid();
                //if (Guid.TryParse(id, Guid edite_User.ID)) edite_User.ID = Request.Params["id"] ?? "";
                var str_json = new JavaScriptSerializer().Serialize(edite_User);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/User/EditUser");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    streamWriter.Write(str_json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    apiJson user = JsonConvert.DeserializeObject<apiJson>(result);
                    if (user.code == 200)
                    {
                        return Json(new { @code = 200, @msg = Resources.Shared.Updated });
                    }
                    else
                        return Json(new { @code = 404, @msg = user.msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/user/DeleteUser?ID=" + ID);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "Get";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    apiJson user = JsonConvert.DeserializeObject<apiJson>(result);
                    if (user.code == 200)
                    {
                        ViewBag.msg = user.msg;
                        return Json(new { code = 200, msg = Resources.Shared.Deleted });
                    }

                    ViewBag.msg = user.msg;
                    return Json(new { code = 404, msg = user.msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}
            //[HttpPost]
        //public JsonResult EditUser()
        //{
        //    //Session["error"] = null;
        //    AddUserModel edite_User = new AddUserModel();
        //    edite_User.FullName = Request.Params["fullname"] ?? "";
        //    edite_User.UserName = Request.Params["username"]?? "";
        //    edite_User.Email = Request.Params["email"] ??"";
        //    edite_User.Password = Request.Params["password"] ?? "";
        //    edite_User.Phone = Request.Params["phone"] ?? "";
        //    edite_User.Bio = Request.Params["bio"] ?? "";
        //    edite_User.Token = RandomString(50);
        //    var json_ = new JavaScriptSerializer().Serialize(edite_User);
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
        //        apiJson<UserModel> user = JsonConvert.DeserializeObject<apiJson<UserModel>>(result);
        //        if (user.code == 200)
        //        {
        //            return Json(new { @code = 200, @msg = "added" });
        //        }
        //        else
        //            return Json(new { @code = 404, @msg = user.msg });
        //    }
        //}

        //public PartialViewResult Content()
        //{
        //    ViewBag.ControllerName = "CP_Neighborhood";
        //    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/Neighborhood/GetAll");
        //    httpWebRequest.ContentType = "application/json";
        //    httpWebRequest.Method = "Get";
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();
        //        apiJson<List<NeighborhoodDisplayModel>> NeighborhoodList = JsonConvert.DeserializeObject<apiJson<List<NeighborhoodDisplayModel>>>(result);
        //        if (NeighborhoodList.code == 200)
        //        {
        //            return PartialView(NeighborhoodList.data);
        //        }
        //        else
        //        {
        //            ViewBag.msg = NeighborhoodList.msg;
        //            return PartialView();
        //        }
        //    }
        //}

        //get Add PartialView
        //public PartialViewResult Add()
        //{
        //    ViewBag.ControllerName = "CP_Neighborhood";

        //    return PartialView();
        //}
