using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models;
using SGAW_ECHO.Models.API.Cities;
using SGAW_ECHO.Models.API.Countries;
using SGAW_ECHO.Models.API.Neighborhoods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SGAW_ECHO.Controllers
{
    public class CP_NeighborhoodController : BaseController
    {
        // GET: CP_Neighborhood
        public ActionResult Index()
        {
            ViewBag.ControllerName = "CP_Neighborhood";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/Neighborhood/GetAll");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                 apiJson<List<NeighborhoodDisplayModel>> NeighborhoodList = JsonConvert.DeserializeObject<apiJson<List<NeighborhoodDisplayModel>>>(result);
                if (NeighborhoodList.code == 200)
                {
                    return View(NeighborhoodList.data);
                }
                ViewBag.msg = NeighborhoodList.msg;
                return View();
            }

        }


        public PartialViewResult Content()
        {
            ViewBag.ControllerName = "CP_Neighborhood";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/Neighborhood/GetAll");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                apiJson<List<NeighborhoodDisplayModel>> NeighborhoodList = JsonConvert.DeserializeObject<apiJson<List<NeighborhoodDisplayModel>>>(result);
                if (NeighborhoodList.code == 200)
                {
                    return PartialView(NeighborhoodList.data);
                }
                else
                {
                    ViewBag.msg = NeighborhoodList.msg;
                    return PartialView();
                }
            }
        }

        //get Add PartialView
        public PartialViewResult Add()
        {
            ViewBag.ControllerName = "CP_Neighborhood";

            return PartialView();
        }

        //Adding new Neighborhood
        [HttpPost]
        public JsonResult Adding()
        {
            //Session["error"] = null; 
            //AddNeighborhoodModel New_Neighborhood = new AddNeighborhoodModel();
            NeighborhoodModel New_Neighborhood = new NeighborhoodModel();
            New_Neighborhood.Ar = Request.Params["Ar"] ?? "";
            New_Neighborhood.En = Request.Params["En"] ?? "";
            New_Neighborhood.Tr = Request.Params["Tr"] ?? "";
            New_Neighborhood.City_ID = Request.Params["city"] ?? "";

            var str_json = new JavaScriptSerializer().Serialize(New_Neighborhood);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/Neighborhood/Add");
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
                apiJson<string> Neighborhood = JsonConvert.DeserializeObject<apiJson<string>>(result);
                if (Neighborhood.code == 200)
                {
                    return Json(new { @code = 200, @msg = Neighborhood.msg });
                }
                else
                    return Json(new { @code = 404, @msg = Neighborhood.msg });
            }

        }

        //get Edit PartialView
        [HttpGet]
        public PartialViewResult Edit(string ID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/Neighborhood/GetNeighborhoodyInformation");
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
                apiJson<NeighborhoodLargeModel> Neighborhood = JsonConvert.DeserializeObject<apiJson<NeighborhoodLargeModel>>(result);
                if (Neighborhood.code == 200)
                {
                    ViewData["countrieslist"] = Neighborhood.data.Countries;
                    ViewData["citites"]= Neighborhood.data.Cities;
                    ViewData["countryId"]= Neighborhood.data.CountryId;
                    ViewBag.msg = Neighborhood.msg;
                    return PartialView(Neighborhood.data.Neighborhood);
                }

                ViewBag.msg = Neighborhood.msg;
                return PartialView();
            }



        }

        // get: Details
        //get Edit PartialView
        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/Neighborhood/GetNeighborhoodByID?Id=" + ID);
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
                apiJson<NeighborhoodModel> Neighborhood = JsonConvert.DeserializeObject<apiJson<NeighborhoodModel>>(result);
                if (Neighborhood.code == 200)
                {
                    return PartialView(Neighborhood.data);
                }

                ViewBag.msg = Neighborhood.msg;
                return PartialView();
            }



        }

        [HttpPost]
        public JsonResult Editing()
        {

            //Session["error"] = null;
            NeighborhoodModel edited_Neighborhood = new NeighborhoodModel();
            edited_Neighborhood.ID = Request.Params["ID"] ?? "";
            edited_Neighborhood.City_ID = Request.Params["country"] ?? "";
            edited_Neighborhood.Ar = Request.Params["Ar"] ?? "";
            edited_Neighborhood.En = Request.Params["En"] ?? "";
            edited_Neighborhood.Tr = Request.Params["Tr"] ?? "";

            // string json = user.AddUser().ToString();
            //userJson u = JsonConvert.DeserializeObject<userJson>(json);
            var str_json = new JavaScriptSerializer().Serialize(edited_Neighborhood);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/Neighborhood/Edit");
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

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44301/Neighborhood/Delete");
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
                apiJson<NeighborhoodModel> Neighborhood = JsonConvert.DeserializeObject<apiJson<NeighborhoodModel>>(result);
                if (Neighborhood.code == 200)
                {
                    ViewBag.msg = Neighborhood.msg;
                    return Json(new { code = 200, msg = Neighborhood.msg });
                }

                ViewBag.msg = Neighborhood.msg;
                return Json(new { code = 404, msg = Neighborhood.msg });
            }
        }
    
    }
}