using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models.API.Cities;
using SGAW_ECHO.Models.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGAW_ECHO.Models.API.Countries;
using static SGAW_ECHO.Classes.HelperClass;
using static System.String;


namespace SGAW_ECHO.Controllers
{
    public class CityController : Controller
    {
        string msg;

        [HttpPost]
        public JsonResult Add()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            AddCityModel city = new AddCityModel();
            try
            {
                city = JsonConvert.DeserializeObject<AddCityModel>(json);
                int code;
                string msg;

                if(IsNullOrWhiteSpace(city.Ar)) return Json(new { @code = 404, @msg = "Enter Arabic Name" }, JsonRequestBehavior.AllowGet);
                if(IsNullOrWhiteSpace(city.En)) return Json(new { @code = 404, @msg = "Enter English Name" }, JsonRequestBehavior.AllowGet);
                if(IsNullOrWhiteSpace(city.Tr)) return Json(new { @code = 404, @msg = "Enter Turkish Name" }, JsonRequestBehavior.AllowGet);
                if(IsValidGuid(city.Country_ID)) return Json(new { @code = 404, @msg = "Enter Country ID or Country ID is not valid" }, JsonRequestBehavior.AllowGet);

                List<string> cols = new List<string>() { "City_Name", "Country_ID", "Date_Of_Create", "Date_Of_Update" };
                List<Object> vals = new List<object>(){ city.En, city.Country_ID, DateTime.Now, DateTime.Now };

                string errMessage = string.Empty;

                Guid City_ID = Guid.NewGuid();
                DataRow temp = Database.GetRow("Cities", City_ID);

                while (temp != null) //gereksiz ama olsun
                {
                    City_ID = Guid.NewGuid();
                    temp = Database.GetRow("Cities", City_ID);
                }

                if (Database.InsertRow("Cities", City_ID, cols, vals, out errMessage))
                {
                    cols = new List<string>() { "Src_ID", "Src_Type", "Ar_Value", "En_Value", "Tr_Value", "Date_Of_Create", "Date_Of_Update" };
                    vals = new List<object>() { City_ID, "Cities", city.Ar, city.En, city.Tr, DateTime.Now, DateTime.Now };
                    errMessage = string.Empty;

                    Guid Translation_ID = Guid.NewGuid();
                    temp = Database.GetRow("Translations", City_ID);
                    while (temp != null) //gereksiz ama olsun
                    {
                        Translation_ID = Guid.NewGuid();
                        temp = Database.GetRow("Translations", City_ID);
                    }
                    if (Database.InsertRow("Translations", Translation_ID, cols, vals, out errMessage))
                    {
                        return Json(new { @code = 200, @msg = "City has been added successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string err;
                        Database.DeleteRow("Cities", City_ID, out err);
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

        public JsonResult GetAll()
        {
            string msg;
            string sql = @"select c.ID 
                            ,t1.Ar_Value Ar_city
                            ,t1.En_Value En_city
                            ,t1.Tr_Value Tr_city
                            ,t2.Ar_Value Ar_country
                            ,t2.En_Value En_country
                            ,t2.Tr_Value Tr_country
                            from Cities  c  
                            INNER JOIN Countries Co ON CO.ID = c.Country_ID
                            inner join Translations t1  on c.ID = t1.Src_ID  AND t1.Src_Type = 'Cities'
                            inner join Translations t2  on Co.ID = t2.Src_ID AND t2.Src_Type = 'Countries'
                            ORDER BY En_country, En_city  ASC ";
            DataTable cities = Database.ReadTableByQuery(sql, null, out msg);
            if (cities != null && cities.Rows.Count > 0)
            {
                List<CityDisplayModel> dtList = cities.AsEnumerable()
                .Select(row => new CityDisplayModel
                {
                    ID = row["ID"].ToString(),
                    Country = new Name() 
                    { 
                        Ar = row["Ar_country"].ToString(),
                        En = row["En_country"].ToString(),
                        Tr = row["Tr_country"].ToString(), 
                    },
                    Ar = row["Ar_city"].ToString(),
                    En = row["En_city"].ToString(),
                    Tr = row["Tr_city"].ToString()
                }).ToList();

                return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllByCountry()
        {
            if (Request.QueryString["Country_ID"] == null)
            {
                return Json(new { @msg = "Enter Country ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            Guid Country_ID = new Guid(Request.QueryString["Country_ID"].ToString());
            string msg;
            string sql = "select c.ID as ID , c.Country_ID,t.Ar_Value,t.En_Value,t.Tr_Value from Cities as c " +
                " inner join Translations as t  on c.ID = t.Src_ID where t.Src_Type = 'Cities' and c.Country_ID = @Country_ID";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@Country_ID", Country_ID));
            DataTable cities = Database.ReadTableByQuery(sql, li, out msg);
            if (cities != null && cities.Rows.Count > 0)
            {
                List<CityModel> dtList = cities.AsEnumerable()
                .Select(row => new CityModel
                {
                    ID = row["ID"].ToString(),
                    Country_ID = row["Country_ID"].ToString(),
                    Ar = row["Ar_Value"].ToString(),
                    En = row["En_Value"].ToString(),
                    Tr = row["Tr_Value"].ToString()
                }).ToList();

                return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCityByID(string ID)
        {
            if (!IsValidGuid(ID))
            {
                return Json(new { @msg = "Enter city ID or city ID is not valid !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string str_query = @"  SELECT [Country_ID]
	                                      ,T.Ar_Value
	                                      ,T.EN_Value
	                                      ,T.Tr_Value
                                      FROM Cities C
                                      INNER JOIN Translations T ON T.Src_ID = C.ID
                                      WHERE C.ID = @CID";
            List<SqlParameter> li = new List<SqlParameter>() { new SqlParameter("@CID", ID) };
            CityModel city = new CityModel();
            DataTable cityTable = Database.ReadTableByQuery(str_query, li, out msg);
            if(cityTable != null && cityTable.Rows.Count > 0)
            {
                DataRow cityRow = cityTable.Rows[0];
                city = new CityModel()
                {
                    ID = ID,
                    Ar = cityRow["Ar_Value"].ToString(),
                    En = cityRow["En_Value"].ToString(),
                    Tr = cityRow["Tr_Value"].ToString(),
                    Country_ID = cityRow["Country_ID"].ToString()
                };   
            }



            return Json(new { @data = city, @code = 200 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCityInformation(string ID)
        {
            if (!IsValidGuid(ID))
            {
                return Json(new { @msg = "Enter city ID or city ID is not valid !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string str_query = @"    SELECT Country_ID
                                            ,t1.Ar_Value Ar_city
                                            ,t1.En_Value En_city
                                            ,t1.Tr_Value Tr_city
                                            ,t2.Ar_Value Ar_country
                                            ,t2.En_Value En_country
                                            ,t2.Tr_Value Tr_country
                                            from Cities  c  
                                            INNER JOIN Countries Co ON CO.ID = c.Country_ID
                                            inner join Translations t1  on c.ID = t1.Src_ID  AND t1.Src_Type = 'Cities'
                                            inner join Translations t2  on Co.ID = t2.Src_ID AND t2.Src_Type = 'Countries'
		                                    WHERE C.ID = @CID";
            List<SqlParameter> li = new List<SqlParameter>() { new SqlParameter("@CID", ID) };
            DataTable cityTable = Database.ReadTableByQuery(str_query, li, out msg);
            CityModel city = new CityModel();
            if (cityTable != null && cityTable.Rows.Count > 0)
            {
                DataRow cityRow = cityTable.Rows[0];
                city = new CityModel()
                {
                    ID = ID,
                    Ar = cityRow["Ar_city"].ToString(),
                    En = cityRow["En_city"].ToString(),
                    Tr = cityRow["Tr_city"].ToString(),
                    Country_ID = cityRow["Country_ID"].ToString()
                };
            }
            string query = @" SELECT C.[ID]
	                            , T.Ar_Value     
	                            , T.En_Value     
	                            , T.Tr_Value 
	                            , I.URL
                              FROM [sgaw].[dbo].[Countries] C
                              INNER JOIN  Translations T ON T.Src_ID = C.ID 
                              INNER JOIN Images I on C.id = I.Src_ID  ";
            DataTable countries = Database.ReadTableByQuery(query, null, out msg);
            countries = Database.ReadTableByQuery(query, null, out msg);
            List<Country> country_List = new List<Country>();
            country_List.Add(new Country()
            {
                ID = (new Guid()).ToString(),
                Ar = "اختيار دولة",
                En = "Select country",
                Tr = "Ülke seç",
                Flag = ""
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
            }
            return Json(new { @data = new { @city = city, @countries = country_List } , @code = 200 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit()
        {
            msg = string.Empty;
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();
            CityModel city = JsonConvert.DeserializeObject<CityModel>(json);

            try
            {
                int code;
                string msg, errMessage;
                Guid Id = new Guid();
                if (city.ID == null || !Guid.TryParse(city.ID, out Id))
                {
                    return Json(new { @msg = "Enter ID or city ID is not valid !", @code = 404 }, JsonRequestBehavior.AllowGet);
                }

                if (city.Ar == null || city.Ar.Trim() == string.Empty) return Json(new { @code = 404, @msg = "Enter Arabic Name" }, JsonRequestBehavior.AllowGet);
                if (city.En == null || city.En.Trim() == string.Empty) return Json(new { @code = 404, @msg = "Enter English Name" }, JsonRequestBehavior.AllowGet);
                if (city.Tr == null || city.Tr.Trim() == string.Empty) return Json(new { @code = 404, @msg = "Enter Turkish Name" }, JsonRequestBehavior.AllowGet);
                if (!IsValidGuid(city.Country_ID)) return Json(new { @code = 404, @msg = "Enter Country ID or Country ID is not valid" }, JsonRequestBehavior.AllowGet);

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
            if (!IsValidGuid(ID))
            {
                return Json(new { @msg = "Enter city ID !", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                Guid Id = new Guid(ID); 

                if (Database.DeleteRow("Cities", Id, out msg))
                {
                    var temp = Database.FindRow("Translations", "Src_ID", Id);
                    var Translation_ID = new Guid(temp["Id"].ToString());
                    Database.DeleteRow("Translations", Translation_ID, out msg);
                    return Json(new { @code = 200, @msg = "The city has deleted" }, JsonRequestBehavior.AllowGet);
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
