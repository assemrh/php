using Newtonsoft.Json;
using SGAW_ECHO.Models;
using SGAW_ECHO.Models.API.Cities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace SGAW_ECHO.Classes
{
    public class Class1<T>
    {
        public string Url { get; set; }
        public T New_object { get; set; }
        public string Method { get; set; } = "POST";
        public string ContentType { get; set; } = "application/json";
        public apiJson<T> GetT()
        {
            //    Session["error"] = null;
            //    AddCityModel New_city = new AddCityModel();
            //    New_city.Ar = Request.Params["Ar"] ?? "";
            //    New_city.En = Request.Params["En"] ?? "";
            //    New_city.Tr = Request.Params["Tr"] ?? "";
            //    New_city.Country_ID = Request.Params["country"] ?? "";

            var str_json = new JavaScriptSerializer().Serialize(New_object);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Method = Method;
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
                apiJson<T> city = JsonConvert.DeserializeObject<apiJson<T>>(result);

                //if (city.code == 200)
                //{
                //    return Json(new { @code = 200, @msg = city.msg });
                //}
                //else
                //    return Json(new { @code = 404, @msg = city.msg });
                return city;
            }
        }
    }
}