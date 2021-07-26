using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models.API.Cities;
using SGAW_ECHO.Models.API.Countries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGAW_ECHO.Controllers
{
    public class CountryController : Controller
    {
        string msg;

        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetUniversities_Countries()
        {
            string sql = " select c.ID,t.Ar_Value as ar,t.En_Value as en,t.Tr_Value as tr,i.URL from Countries as c " +
            " inner join Translations as t  on c.ID = t.Src_ID  inner join Images as i on c.id = i.Src_ID " +
            " where(c.id = '54D862AB-8BA8-4C40-9009-A4318ED65017' or c.id = '721A35F2-8DD1-49D3-A25A-B78648F6C126') " +
            " and t.Src_Type = 'Countries' and i.Src_Type = 'Countries'";
            string msg;
            DataTable countries = Database.ReadTableByQuery(sql, null, out msg);
            if (countries != null && countries.Rows.Count > 0)
            {
                List<Country> Countries = countries.AsEnumerable()
                    .Select(row => new Country
                    {
                        ID = row["ID"].ToString(),
                        Ar = row["ar"].ToString(),
                        En = row["en"].ToString(),
                        Tr = row["tr"].ToString(),
                        Flag = row["URL"].ToString()
                    }).ToList();

                return Json(new { @data = Countries, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetCountries()
        {
            string query = @" SELECT C.[ID]
	                            , T.Ar_Value     
	                            , T.En_Value     
	                            , T.Tr_Value 
	                            , I.URL
                              FROM [sgaw].[dbo].[Countries] C
                              INNER JOIN  Translations T ON T.Src_ID = C.ID 
                              INNER JOIN Images I on C.id = I.Src_ID   ";
            DataTable countries = Database.ReadTableByQuery(query, null, out msg);
            countries = Database.ReadTableByQuery(query, null, out msg);
            List<Country> country_List = new List<Country>();
            country_List.Add(new Country() { 
                ID= (new Guid()).ToString(),
                Ar = "اختيار دولة",
                En="Select country",
                Tr="Ülke seç",
                Flag =""
            });

            if (countries != null && countries.Rows.Count > 0)
            {
                foreach (DataRow row in countries.Rows)
                {
                    country_List.Add(new Country()
                    {
                        ID = row["id"].ToString(),
                        Ar = row["Ar_Value"].ToString(),
                        En = row["En_Value"].ToString(),
                        Tr = row["Tr_Value"].ToString(),
                        Flag = row["URL"].ToString()
                    });
                }
                return Json(new { @code = 200, @data = country_List }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @code = 404, @msg = msg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            AddCountryModel country = new AddCountryModel();
            try
            {
                country = JsonConvert.DeserializeObject<AddCountryModel>(json);
                int code;
                string msg;

                List<string> cols = new List<string>() { "Key_", "Country_ID", "Date_Of_Create", "Date_Of_Update" };
                List<Object> vals = new List<object>() { country.En, DateTime.Now, DateTime.Now };

                string errMessage = string.Empty;

                Guid country_Id = Guid.NewGuid();
                DataRow temp = Database.GetRow("Countries", country_Id);
                while (temp != null)
                {
                    country_Id = Guid.NewGuid();
                    temp = Database.GetRow("Countries", country_Id);
                }

                if (Database.InsertRow("Countries", country_Id, cols, vals, out errMessage))
                {
                    cols = new List<string>(){ "Src_ID", "Src_Type", "Ar_Value", "En_Value", "Tr_Value", "Date_Of_Create", "Date_Of_Update" };
                    vals = new List<object>(){ country_Id, "Countries", country.Ar, country.En, country.Tr, DateTime.Now, DateTime.Now };

                    errMessage = string.Empty;

                    Guid Translation_ID = Guid.NewGuid();
                    temp = Database.GetRow("Translations", country_Id);
                    while (temp != null)
                    {
                        country_Id = Guid.NewGuid();
                        temp = Database.GetRow("Translations", country_Id);
                    }

                    if (Database.InsertRow("Translations", Translation_ID, cols, vals, out errMessage))
                    {
                        return Json(new { @code = 200, @msg = "added" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string err;
                        Database.DeleteRow("countries", country_Id, out err);
                        code = 404;
                        msg = "The entry could not be added" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                        return Json(new { @code = code.ToString(), msg = msg });
                    }

                }
                else
                {
                    code = 404;
                    msg = "The entry could not be added" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                    return Json(new { @code = code.ToString(), msg = msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCountryByID(string ID)
        {
            if (ID == null)
            {
                return Json(new { @msg = "Enter Country ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string str_query = @"  SELECT T.Ar_Value
	                                      ,T.EN_Value
	                                      ,T.Tr_Value
                                      FROM Countries C
                                      INNER JOIN Translations T ON T.Src_ID = C.ID
                                      INNER JOIN Images I ON I.Src_ID = C.ID
                                      WHERE C.ID = @CID";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@CID", ID));
            List<CityModel> country = new List<CityModel>();
            DataTable countryTable = Database.ReadTableByQuery(str_query, li, out msg);
            DataRow countryRow = countryTable.Rows[0];
            country.Add(
                new CityModel()
                {
                    ID = ID,
                    Ar = countryRow["Ar_Value"].ToString(),
                    En = countryRow["En_Value"].ToString(),
                    Tr = countryRow["Tr_Value"].ToString(),
                    Country_ID = countryRow["Country_ID"].ToString()
                }
                );


            return Json(new { @data = country, @code = 200 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit()
        {
            msg = string.Empty;
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();
            CityModel city = new CityModel();
            city = JsonConvert.DeserializeObject<CityModel>(json);


            try
            {
                int code;
                string msg, errMessage;
                if (city.ID == null)
                {
                    return Json(new { @msg = "Enter ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }

                Guid Id = new Guid(city.ID);
                List<string> cols = new List<string>() { "City_Name", "Country_ID", "Date_Of_Update" };
                List<object> vals = new List<object>() { city.En, city.Country_ID, DateTime.Now };


                if (Database.UpdateRow("Cities", Id, cols, vals, out errMessage))
                {
                    cols = new List<string>() { "Ar_Value", "En_Value", "Tr_Value", "Date_Of_Update" };
                    vals = new List<object>() { city.Ar, city.En, city.Tr, DateTime.Now };

                    var temp = Database.FindRow("Translations", "Src_ID", Id);
                    var Translation_ID = new Guid(temp["Id"].ToString());

                    if (Database.UpdateRow("Translations", Translation_ID, cols, vals, out errMessage))
                    {
                        return Json(new { @code = 200, @msg = "City updated!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        code = 404;
                        msg = "update failed" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                        return Json(new { @code = code.ToString(), msg = msg });
                    }

                }
                else
                {
                    code = 404;
                    msg = "update failed" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                    return Json(new { @code = code.ToString(), msg = msg });
                }

            }
            catch (Exception ex)
            {
                return Json(new { @code = 404, @msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Delete(string ID)
        {
            if (ID == null)
            {
                return Json(new { @msg = "Enter Post ID !", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                Guid Id = new Guid(ID);

                if (Database.DeleteRow("Cities", Id, out msg))
                {
                    var temp = Database.FindRow("Translations", "Src_ID", Id);
                    var Translation_ID = new Guid(temp["Id"].ToString());
                    Database.DeleteRow("Translations", Translation_ID, out msg);
                    return Json(new { @code = 200 }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { @code = 404, @msg = "The city was not found, or has already deleted" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { @code = 404, @msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}