using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models;
using SGAW_ECHO.Models.API.Cities;
using SGAW_ECHO.Models.API.Countries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SGAW_ECHO.Controllers
{
    public class CP_CitiesController : BaseController
    {
        // GET: CP_Cities
        public ActionResult Index()
        {
            ViewBag.ControllerName = "CP_Cities";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/City/GetAll");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //HttpFileCollection httpFileCollection = new HttpFileCollection()
               

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(" ");
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<List<CityDisplayModel>> cityList = JsonConvert.DeserializeObject<apiJson<List<CityDisplayModel>>>(result);
                if (cityList.code == 200)
                {
                    return View(cityList.data);
                }

                ViewBag.msg = cityList.msg;
                return View();
            }

        }
        //get Add PartialView
        public PartialViewResult Add()
        {
            ViewBag.ControllerName = "CP_Cities";
            return PartialView();
        }

        //Adding new city
        public JsonResult Adding()
        {
            //Session["error"] = null;
            AddCityModel New_city = new AddCityModel();
            New_city.Ar = Request.Params["Ar"] ?? "";
            New_city.En = Request.Params["En"] ?? "";
            New_city.Tr = Request.Params["Tr"] ?? "";
            New_city.Country_ID = Request.Params["country"] ??"";

            var str_json = new JavaScriptSerializer().Serialize(New_city);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/City/Add");
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
                apiJson<string> city = JsonConvert.DeserializeObject<apiJson<string>>(result);
                if (city.code == 200)
                {
                    return Json(new { @code = 200, @msg = city.msg });
                }
                else
                    return Json(new { @code = 404, @msg = city.msg });
            }

        }

        //get Edit-PartialView
        [HttpPost]
        public PartialViewResult Edit(string ID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/City/GetCityInformation");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Post";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var str_json = $"{(char)123}\"ID\": \"{ID}\"{(char)125}"; // Output: {"ID": "ID"}
                streamWriter.Write(str_json); 
                streamWriter.Flush();
                streamWriter.Close();
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<CityLargeModel> city = JsonConvert.DeserializeObject<apiJson<CityLargeModel>>(result);
                if (city.code == 200)
                {
                    ViewBag.msg = city.msg;
                    ViewData["countrieslist"] = city.data.Countries;
                    return PartialView(city.data.City);
                }

                ViewBag.msg = city.msg;
                return PartialView();
            }



        }

        // get: Details
        //get Edit PartialView
        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/City/GetCityInformation");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Post";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var str_json = $"{(char)123}\"ID\": \"{ID}\"{(char)125}"; // Output: {ID: ID}
                streamWriter.Write(str_json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<CityLargeModel> city = JsonConvert.DeserializeObject<apiJson<CityLargeModel>>(result);
                if (city.code == 200)
                {
                    ViewBag.msg = city.msg;
                    ViewData["countrieslist"] = city.data.Countries;
                    return PartialView(city.data.City);
                }

                ViewBag.msg = city.msg;
                return PartialView();
            }
        }

        [HttpPost]
        public JsonResult Editing()
        {

            //Session["error"] = null;
            CityModel edited_City = new CityModel();
            edited_City.ID = Request.Params["ID"] ?? "";
            edited_City.Country_ID = Request.Params["country"] ?? "";
            edited_City.Ar = Request.Params["Ar"] ?? "";
            edited_City.En = Request.Params["En"] ?? "";
            edited_City.Tr = Request.Params["Tr"] ?? "";

            // string json = user.AddUser().ToString();
            //userJson u = JsonConvert.DeserializeObject<userJson>(json);
            var str_json = new JavaScriptSerializer().Serialize(edited_City);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/City/Edit");
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
                apiJson<UserModel> user = JsonConvert.DeserializeObject<apiJson<UserModel>>(result);
                if (user.code == 200)
                {
                    return Json(new { @code = 200, @msg = "Updated" });
                }
                else
                    return Json(new { @code = 404, @msg = user.msg });
            }

        }

        public PartialViewResult Content()
        {
            ViewBag.ControllerName = "CP_Cities";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/City/GetAll");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write("");
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<List<CityDisplayModel>> cityList = JsonConvert.DeserializeObject<apiJson<List<CityDisplayModel>>>(result);
                if (cityList.code == 200)
                {
                    return PartialView(cityList.data);
                }
                else
                {
                    ViewBag.msg = cityList.msg;
                    return PartialView();
                }
            }

        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/City/Delete");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Post";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var str_json = $"{(char)123}\"ID\": \"{ID}\"{(char)125}"; // Output: {ID: ID}
                streamWriter.Write(str_json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<CityModel> city = JsonConvert.DeserializeObject<apiJson<CityModel>>(result);
                if (city.code == 200)
                {
                    ViewBag.msg = city.msg;
                    return Json(new { code = 200, msg = city.msg });
                }

                ViewBag.msg = city.msg;
                return Json(new { code = 404, msg = city.msg });
                //return PartialView();
            }

            //string msg = "";
            //int code = 0;
            //if (Database.DeleteRow("Citites", new Guid(ID), out msg))
            //{
            //    List<SqlParameter> li = new List<SqlParameter>();
            //    li.Add(new SqlParameter("@BID", new Guid(ID)));
            //    Database.ReadTableByQuery("DELETE FROM Images Where referral_id = @BID ", li, out msg);
            //    code = 200;

            //    return Json(new { code = code.ToString(), msg = "Resources.CP_Brands.Deleted" });
            //}
            //else
            //{
            //    code = 404;
            //    msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
            //    return Json(new { code = code.ToString(), msg = msg });
            //}
        }
    }
}