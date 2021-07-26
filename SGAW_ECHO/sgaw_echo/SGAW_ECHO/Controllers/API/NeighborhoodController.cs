using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models;
using SGAW_ECHO.Models.API;
using SGAW_ECHO.Models.API.Countries;
using SGAW_ECHO.Models.API.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SGAW_ECHO.Classes.HelperClass;


namespace SGAW_ECHO.Controllers.API
{
    public class NeighborhoodController : Controller
    {
        string msg;
        [HttpPost]
        public JsonResult Add()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            AddNeighborhoodModel Neighborhood = new AddNeighborhoodModel();
            try
            {
                Neighborhood = JsonConvert.DeserializeObject<AddNeighborhoodModel>(json);
                int code;
                string msg;

                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();


                string[] colsinput = { "Neighborhood_Name", "City_ID", "Date_Of_Create", "Date_Of_Update" };
                cols.AddRange(colsinput);

                object[] valsinput = { Neighborhood.En, Neighborhood.City_ID, DateTime.Now, DateTime.Now };
                vals.AddRange(valsinput);

                string errMessage = string.Empty;

                Guid Neighborhood_ID = Guid.NewGuid();
                DataRow temp = Database.GetRow("Neighborhoods", Neighborhood_ID);
                while (temp != null)
                {
                    Neighborhood_ID = Guid.NewGuid();
                    temp = Database.GetRow("Neighborhoods", Neighborhood_ID);
                }

                if (Database.InsertRow("Neighborhoods", Neighborhood_ID, cols, vals, out errMessage))
                {
                    cols = new List<string>(){ "Src_ID", "Src_Type", "Ar_Value", "En_Value", "Tr_Value", "Date_Of_Create", "Date_Of_Update" };
                    vals = new List<object>(){ Neighborhood_ID, "Neighborhoods", Neighborhood.Ar, Neighborhood.En, Neighborhood.Tr, DateTime.Now, DateTime.Now };



                    errMessage = string.Empty;

                    Guid Translation_ID = Guid.NewGuid();
                    temp = Database.GetRow("Translations", Neighborhood_ID);
                    while (temp != null)
                    {
                        Neighborhood_ID = Guid.NewGuid();
                        temp = Database.GetRow("Translations", Neighborhood_ID);
                    }
                    if (Database.InsertRow("Translations", Translation_ID, cols, vals, out errMessage))
                    {
                        return Json(new { @data = "added", @code = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string err;
                        Database.DeleteRow("Cities", Neighborhood_ID, out err);
                        code = 404;
                        msg = "regestration failed" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                        return Json(new { @code = code.ToString(), msg = msg });
                    }

                }
                else
                {
                    code = 404;
                    msg = "regestration failed" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                    return Json(new { @code = code.ToString(), msg = msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }
//        {
//    "City_ID":"CCC67ADE-85FE-4CDF-9A1D-61B65AE9D1E8",
//     "Ar":"Kizilay",
//      "En":"Kizilay",
//       "Tr":"Kizilay"
//}
    public JsonResult GetAll()
        {
            string msg;
            string sql = @"SELECT 
                            N.ID
                            ,t1.Ar_Value Ar_Neighborhood
                            ,t1.En_Value En_Neighborhood
                            ,t1.Tr_Value Tr_Neighborhood
                            ,t2.Ar_Value Ar_city
                            ,t2.En_Value En_city
                            ,t2.Tr_Value Tr_city
                            FROM Neighborhoods N 
	                        INNER JOIN Cities C ON C.ID = N.City_ID
	                        inner join Translations t1  on N.ID = t1.Src_ID  AND t1.Src_Type = 'Neighborhoods'
	                        inner join Translations t2  on C.ID = t2.Src_ID AND t2.Src_Type = 'Cities' ";
            DataTable Neighborhoods = Database.ReadTableByQuery(sql, null, out msg);
            if (Neighborhoods != null && Neighborhoods.Rows.Count > 0)
            {
                List<NeighborhoodDisplayModel> nhList = Neighborhoods.AsEnumerable()
                .Select(row => new NeighborhoodDisplayModel
                {
                    ID = row["ID"].ToString(),
                    City = new Name() { Ar = row["Ar_city"].ToString(), En= row["En_city"].ToString(), Tr=row["Tr_city"].ToString() },
                    Ar = row["Ar_Neighborhood"].ToString(),
                    En = row["En_Neighborhood"].ToString(),
                    Tr = row["Tr_Neighborhood"].ToString()
                }).ToList();


                return Json(new { @data = nhList, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

    public JsonResult GetAllByCity()
    {
        if (Request.QueryString["City_ID"] == null)
        {
            return Json(new { @msg = "Enter City ID !", @code = 404 },
                JsonRequestBehavior.AllowGet);
        }
        Guid City_ID = new Guid(Request.QueryString["City_ID"].ToString());
        string msg;
        string sql = "select n.ID as ID , n.City_ID,t.Ar_Value,t.En_Value,t.Tr_Value from Neighborhoods as n " +
            " inner join Translations as t  on n.ID = t.Src_ID where t.Src_Type = 'Neighborhoods' and n.City_ID = @City_ID ";
        List<SqlParameter> li = new List<SqlParameter>();
        li.Add(new SqlParameter("@City_ID", City_ID));
        DataTable cities = Database.ReadTableByQuery(sql, li, out msg);
        if (cities != null && cities.Rows.Count > 0)
        {
            List<NeighborhoodModel> dtList = cities.AsEnumerable()
            .Select(row => new NeighborhoodModel
            {
                ID = row["ID"].ToString(),
                City_ID = row["City_ID"].ToString(),
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
    public JsonResult GetNeighborhoodByID(string ID)
    {
        if (!IsValidGuid(ID))
        {
            return Json(new { @msg = "Enter Country ID !", @code = 404 },
                JsonRequestBehavior.AllowGet);
        }
        string str_query = @"    SELECT City_ID 
	                                    ,T.Ar_Value
	                                    ,T.EN_Value
	                                    ,T.Tr_Value
                                    FROM Neighborhoods N
                                    INNER JOIN Translations T ON T.Src_ID = N.ID
                                    WHERE N.ID = @NID";
        List<SqlParameter> li = new List<SqlParameter>();
        li.Add(new SqlParameter("@NID", ID));
        DataTable nTable = Database.ReadTableByQuery(str_query, li, out msg);
        DataRow nRow = nTable.Rows[0];
        NeighborhoodModel neighborhood = new NeighborhoodModel()
            {
                ID = ID,
                City_ID = nRow["City_ID"].ToString(),
                Ar = nRow["Ar_Value"].ToString(),
                En = nRow["En_Value"].ToString(),
                Tr = nRow["Tr_Value"].ToString()
            };


        return Json(new { @data = neighborhood, @code = 200 }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public JsonResult GetNeighborhoodyInformation(string ID)
    {
        if (!IsValidGuid(ID))
        {
            return Json(new { @msg = "Enter Neighborhood ID !", @code = 404 },
                JsonRequestBehavior.AllowGet);
        }
        string nQuery = @"  SELECT City_ID 
	                                ,T.Ar_Value
	                                ,T.EN_Value
	                                ,T.Tr_Value
									,c1.ID CountryID
                                    FROM Neighborhoods N
                                    INNER JOIN Translations T ON T.Src_ID = N.ID
									INNER JOIN Cities c on c.ID = N.City_ID
									INNER JOIN Countries c1 on c1.ID = c.Country_ID  
                                    WHERE N.ID = @NID";

        string c1Query = @"   SELECT c.ID
	                                      ,T.Ar_Value
	                                      ,T.EN_Value
	                                      ,T.Tr_Value
                                      FROM Cities C
                                      INNER JOIN Translations T ON T.Src_ID = C.ID
                                      WHERE C.Country_ID Like(select c1.Country_ID from Cities c1 where c1.ID = @CID)";

        string c2Query = @" SELECT C.ID
	                            , T.Ar_Value     
	                            , T.En_Value     
	                            , T.Tr_Value 
	                            , I.URL
                              FROM Countries C
                              INNER JOIN  Translations T ON T.Src_ID = C.ID 
                              INNER JOIN Images I on C.id = I.Src_ID   ";

        string CountryID = string.Empty;

        List<SqlParameter> li = new List<SqlParameter>();
        li.Add(new SqlParameter("@NID", ID));
        DataTable nTable = Database.ReadTableByQuery(nQuery, li, out msg);
        NeighborhoodModel neighborhood = new NeighborhoodModel();
        if (nTable!=null && nTable.Rows.Count > 0)
        {
            DataRow nRow = nTable.Rows[0];
            neighborhood = new NeighborhoodModel()
            {
                ID = ID,
                City_ID = nRow["City_ID"].ToString(),
                Ar = nRow["Ar_Value"].ToString(),
                En = nRow["En_Value"].ToString(),
                Tr = nRow["Tr_Value"].ToString()
            };
                CountryID = nRow["CountryID"].ToString();
        }
        List<SqlParameter> li1 = new List<SqlParameter>();
        li1.Add(new SqlParameter("@CID", neighborhood.City_ID));
        DataTable c1Table = Database.ReadTableByQuery(c1Query, li1, out msg);
        List<City> citieslist = new List<City>();
        if (c1Table != null && c1Table.Rows.Count > 0)
        {
            foreach (DataRow Row in c1Table.Rows)
            {
                citieslist.Add( new City()
                {
                    ID = Row["ID"].ToString(),
                    Ar = Row["Ar_Value"].ToString(),
                    En = Row["En_Value"].ToString(),
                    Tr = Row["Tr_Value"].ToString()
                });
            }
        }

        DataTable c2Table = Database.ReadTableByQuery(c2Query, null, out msg);
        List<Country> countrylist = new List<Country>();
        if (c2Table != null && c2Table.Rows.Count > 0)
        {
            foreach (DataRow Row in c2Table.Rows)
            {
                countrylist.Add(new Country()
                {
                    ID = Row["ID"].ToString(),
                    Ar = Row["Ar_Value"].ToString(),
                    En = Row["En_Value"].ToString(),
                    Tr = Row["Tr_Value"].ToString(),
                    Flag = Row["URL"].ToString()
                });
            }
        }



        return Json(new { @data = new { @neighborhood=neighborhood,@cities= citieslist, @countries= countrylist ,@countryId= CountryID }, @code = 200 }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    [ValidateInput(false)]
    public JsonResult Edit()
    {
        msg = string.Empty;
        Stream request = Request.InputStream;
        request.Seek(0, SeekOrigin.Begin);
        string json = new StreamReader(request).ReadToEnd();
        NeighborhoodModel Neighborhood = new NeighborhoodModel();
        Neighborhood = JsonConvert.DeserializeObject<NeighborhoodModel>(json);


        try
        {
            int code;
            string msg, errMessage;
            if (Neighborhood.ID == null)
            {
                return Json(new { @msg = "Enter ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }

            Guid Id = new Guid(Neighborhood.ID);
            List<string> cols = new List<string>() { "Neighborhood_Name", "City_ID", "Date_Of_Update" };
            List<object> vals = new List<object>() { Neighborhood.En,Neighborhood.City_ID, DateTime.Now };


            if (Database.UpdateRow("Neighborhoods", Id, cols, vals, out errMessage))
            {
                cols = new List<string>() { "Ar_Value", "En_Value", "Tr_Value", "Date_Of_Update" };
                vals = new List<object>() { Neighborhood.Ar, Neighborhood.En, Neighborhood.Tr, DateTime.Now };

                var temp = Database.FindRow("Translations", "Src_ID", Id);
                var Translation_ID = new Guid(temp["Id"].ToString());

                if (Database.UpdateRow("Translations", Translation_ID, cols, vals, out errMessage))
                {
                    return Json(new { @code = 200, @msg = "Neighborhoods updated!" }, JsonRequestBehavior.AllowGet);
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
            return Json(new { @msg = "Enter Neighborhoods ID !", @code = 404 }, JsonRequestBehavior.AllowGet);
        }
        try
        {
            Guid Id = new Guid(ID);

            if (Database.DeleteRow("Neighborhoods", Id, out msg))
            {
                var temp = Database.FindRow("Translations", "Src_ID", Id);
                var Translation_ID = new Guid(temp["Id"].ToString());
                Database.DeleteRow("Translations", Translation_ID, out msg);
                return Json(new { @code = 200 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { @code = 404, @msg = "The Neighborhoods was not found, or has already deleted" }, JsonRequestBehavior.AllowGet);

        }
        catch (Exception ex)
        {
            return Json(new { @code = 404, @msg = ex.Message }, JsonRequestBehavior.AllowGet);
        }

    }

    }
}