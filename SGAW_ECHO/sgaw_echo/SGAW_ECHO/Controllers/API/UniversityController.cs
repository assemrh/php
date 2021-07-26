using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models.API.Post;
using SGAW_ECHO.Models.API.University;
using SGAW_ECHO.Models.CP.University;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace SGAW_ECHO.Controllers
{
    public class UniversityController : Controller
    {
        [HttpPost]
        public JsonResult Add()
        {
            UniversityModel university = new UniversityModel();
            try
            {
                //  post.PopstInfo = JsonConvert.DeserializeObject<AddPostModel>(json);
                if (Request.Form != null && Request.Form.Count > 0)
                {
                    if (Request.Form["Ar_name"] == null && Request.Form["En_name"] == null && Request.Form["Tr_name"] == null)
                    {
                        return Json(new { @code = 404, msg = "enter university name" });
                    }
                    else
                    {
                        if (Request.Form["Tr_name"] != null) university.Tr = Request.Form["Tr_name"];
                        else return Json(new { @code = 404, msg = "Turkish name is required" });
                        if (Request.Form["Ar_name"] != null) university.Ar = Request.Form["Ar_name"];
                        else university.Ar = Request.Form["Tr_name"];
                        if (Request.Form["En_name"] != null) university.En = Request.Form["En_name"];
                        else university.En = Request.Form["Tr_name"];
                    }
                    if (Request.Form["Ar_Descreption"] == null && Request.Form["En_Descreption"] == null && Request.Form["Tr_Descreption"] == null)
                    {
                        return Json(new { @code = 404, msg = "enter university Descreption" });
                    }
                    else
                    {
                        if (Request.Form["Tr_Descreption"] != null) university.Tr_Descreption = Request.Form["Tr_Descreption"];
                        else return Json(new { @code = 404, msg = "Turkish Descreption is required" });
                        if (Request.Form["Ar_Descreption"] != null) university.Ar_Descreption = Request.Form["Ar_Descreption"];
                        else university.Ar_Descreption = Request.Form["Tr_Descreption"];
                        if (Request.Form["En_Descreption"] != null) university.En_Descreption = Request.Form["En_Descreption"];
                        else university.En_Descreption = Request.Form["Tr_Descreption"];
                    }
                    university.Address = new Models.API.Address.AddressModel();
                    if (Request.Form["Neighborhood_ID"] != null) university.Address.Neighborhood_ID = new Guid(Request.Form["Neighborhood_ID"].ToString());
                    else return Json(new { @code = 404, msg = "enter Neighborhood ID" });
                    if (Request.Form["Long"] != null) university.Address.Long = Request.Form["Long"];
                    else university.Address.Long = "";
                    if (Request.Form["Lat"] != null) university.Address.Lat = Request.Form["Lat"];
                    else university.Address.Lat = "";
                    if (Request.Form["Descreption"] != null) university.Address.Descreption = Request.Form["Descreption"];
                    else return Json(new { @code = 404, msg = "enter Address Descreption" });




                    //// add university info 
                    List<string> cols = new List<string>();
                    List<Object> vals = new List<object>();


                    string[] colsinput = { "Name", "Date_Of_Create", "Date_Of_Update" };
                    cols.AddRange(colsinput);

                    object[] valsinput = { university.En, DateTime.Now, DateTime.Now };
                    vals.AddRange(valsinput);
                    string errMessage = string.Empty;

                    Guid univ_id = Guid.NewGuid();
                    DataRow temp = Database.GetRow("Universities", univ_id);
                    while (temp != null)
                    {
                        univ_id = Guid.NewGuid();
                        temp = Database.GetRow("Universities", univ_id);
                    }

                    if (Database.InsertRow("Universities", univ_id, cols, vals, out errMessage))
                    {
                        //add translations
                        cols = new List<string>();
                        vals = new List<object>();


                        colsinput = new string[] { "Src_ID", "Src_Type", "Ar_Value", "En_Value", "Tr_Value", "Ar_Descreption", "En_Descreption", "TR_Descreption", "Date_Of_Create", "Date_Of_Update" };
                        cols.AddRange(colsinput);

                        valsinput = new object[] { univ_id, "Universities", university.Ar, university.En, university.Tr, university.Ar_Descreption, university.En_Descreption, university.Tr_Descreption, DateTime.Now, DateTime.Now };
                        vals.AddRange(valsinput);
                        errMessage = string.Empty;

                        Guid translation_id = Guid.NewGuid();
                        temp = Database.GetRow("Translations", translation_id);
                        while (temp != null)
                        {
                            translation_id = Guid.NewGuid();
                            temp = Database.GetRow("Translations", translation_id);
                        }
                        Database.InsertRow("Translations", translation_id, cols, vals, out errMessage);
                        //add address
                        cols = new List<string>();
                        vals = new List<object>();


                        colsinput = new string[] { "Address_Details", "Neighborhood_ID",  "Date_Of_Create", "Date_Of_Update" };
                        cols.AddRange(colsinput);

                        valsinput = new object[] { university.Address.Descreption, university.Address.Neighborhood_ID, DateTime.Now, DateTime.Now };
                        vals.AddRange(valsinput);

                        if(university.Address.Long != string.Empty)
                        {
                            cols.Add("longitude");
                            vals.Add(university.Address.Long);
                        }

                        if (university.Address.Lat != string.Empty)
                        {
                            cols.Add("latitude");
                            vals.Add(university.Address.Lat);
                        }
                        errMessage = string.Empty;

                        Guid addressID = Guid.NewGuid();
                        temp = Database.GetRow("Addresses", addressID);
                        while (temp != null)
                        {
                            addressID = Guid.NewGuid();
                            temp = Database.GetRow("Addresses", addressID);
                        }
                        Database.InsertRow("Addresses", addressID, cols, vals, out errMessage);
                        // update university (add address)
                        cols = new List<string>();
                        vals = new List<object>();


                        colsinput = new string[] { "Address_ID", "Date_Of_Update" };
                        cols.AddRange(colsinput);

                        valsinput = new object[] { addressID, DateTime.Now };
                        vals.AddRange(valsinput);
                        errMessage = string.Empty;


                        Database.UpdateRow("Universities", univ_id, cols, vals, out errMessage);
                        //add image
                        if (Request.Files != null && Request.Files.Count > 0)
                        {
                            var file = Request.Files[0];
                            if (file.ContentType != null)
                            {
                                string type = file.ContentType.Split('/')[0];
                                MemoryStream target = new MemoryStream();
                                file.InputStream.CopyTo(target);
                                byte[] data = target.ToArray();
                                if (type == "image")
                                {
                                    string FileName = file.FileName;
                                    Guid image_id = Guid.NewGuid();
                                    temp = Database.GetRow("Images", image_id);
                                    while (temp != null)
                                    {
                                        image_id = Guid.NewGuid();
                                        temp = Database.GetRow("Images", image_id);
                                    }
                                    // uploade image to server
                                    FileName = image_id.ToString() + System.IO.Path.GetExtension(FileName);
                                    string dir = Server.MapPath("~/Images/Universities/");
                                    if (!Directory.Exists(dir))
                                    {
                                        Directory.CreateDirectory(dir);
                                    }
                                    // FileName = dir + FileName;
                                    System.IO.File.WriteAllBytes(dir + FileName, data);
                                    cols = new List<string>();
                                    vals = new List<object>();


                                    colsinput = new string[] { "URL", "Is_Main", "Row_Index", "Src_ID", "Src_Type", "Date_Of_Create", "Date_Of_Update" };
                                    cols.AddRange(colsinput);

                                    valsinput = new object[] { FileName, 1, 1, univ_id, "Universities", DateTime.Now, DateTime.Now };
                                    vals.AddRange(valsinput);

                                    errMessage = string.Empty;


                                    //add image to data base
                                    Database.InsertRow("Images", image_id, cols, vals, out errMessage);
                                }

                            }
                           
                        }
                        return Json(new { @data = "added", @code = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int code = 404;
                        string msg = "added post  failed" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                        return Json(new { @code = code.ToString(), msg = msg });
                    }

                }
                else
                {
                    int code = 404;
                    string msg = "enter post information";
                    return Json(new { @code = code.ToString(), msg = msg });
                }


            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllUniversities()
        {
            string msg;
            string sql = "";
            sql += "select U.ID ,U.Name,U.Domain ,t.Ar_Value as AR,t.En_Value as EN,t.Tr_Value as TR ,I.URL ";
            sql += "from Universities U ";
            sql += "inner join Addresses Ad on U.Address_ID = Ad.ID ";
            sql += "inner join Neighborhoods N on ad.Neighborhood_ID = N.ID ";
            sql += "inner join Cities Ci on N.City_ID = Ci.ID ";
            sql += "inner join Countries C on Ci.Country_ID = C.ID ";
            sql += "inner join Translations T on c.ID = t.Src_ID and t.Src_Type = 'Countries'";
            sql += "left join Images I on U.ID = I.Src_ID and I.Src_Type = 'Universities' and I.Is_Main = 1 ";

            DataTable Universitiess = Database.ReadTableByQuery( sql,null, out msg);
            if (Universitiess != null && Universitiess.Rows.Count > 0)
            {
                List<GetUniversityModel> dtList = Universitiess.AsEnumerable()
                .Select(row => new GetUniversityModel
                {
                    University_ID = row["ID"].ToString(),
                    University_Name = row["Name"].ToString(),
                    Ar = row["Ar"].ToString(),
                    En = row["En"].ToString(),
                    Tr = row["Tr"].ToString(),
                    Domain = row["Domain"].ToString() == string.Empty ? "-----" : row["Domain"].ToString(),
                    URL = row["URL"].ToString()==string.Empty?"def.jpg": row["URL"].ToString()
                }).ToList();

                return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// whene we need to add pagination use this
        /// </summary>
        /// <returns></returns>
        //public JsonResult GetAllUniversities()
        //{
            
        //    int page_number = 1;
        //    if (Request.QueryString["Page_Number"] != null)
        //    {
        //        int.TryParse(Request.QueryString["Page_Number"], out page_number);
        //        if (page_number <= 0) page_number = 1;
        //    }

        //    int per_page_number = 10;
        //    if (Request.QueryString["Per_Page_Number"] != null)
        //    {
        //        int.TryParse(Request.QueryString["Per_Page_Number"], out per_page_number);
        //        if (per_page_number <= 0) per_page_number = 8;
        //    }


        //    int sorting = 1;
        //    if (Request.QueryString["Sorting"] != null)
        //    {
        //        int.TryParse(Request.QueryString["Sorting"], out sorting);
        //        if (sorting <= 0 || sorting > 2) sorting = 1;
        //    }

        //    string Order_Col = "Name ASC ";
        //    switch (sorting)
        //    {
        //        case 1:
        //            Order_Col = "Name ASC ";
        //            break;
        //        case 2:
        //            Order_Col = "Name DESC ";
        //            break;
        //            //case 3:
        //            //    Order_Col = "Final_Price ASC ";
        //            //    break;
        //            //case 4:
        //            //    Order_Col = "Final_Price DESC ";
        //            //    break;
        //            //case 5:
        //            //    Order_Col = "Date DESC ";
        //            //    break;
        //            //case 6:
        //            //    Order_Col = "Date ASC ";
        //            //    break;
        //    }
        //    string msg;
        //    string sql = "";
        //    sql += " select U.ID as ID ,U.Name,I.Url from Universities as U ";
        //    sql += " left join Images as I  on U.ID = I.Src_ID and I.Src_Type = 'Universities' ";
        //    sql += " where U.Address_ID in (select ID from Addresses where Neighborhood_ID in ( ";
        //    sql += " select ID from Neighborhoods where City_ID in ( ";
        //    sql += " select ID from Cities where Country_ID =@Country_ID )))";
        //    List<SqlParameter> li = new List<SqlParameter>();
        //    li.Add(new SqlParameter("@Country_ID", Country_ID));

        //    DataTable Universitiess_Full_Information = Database.ConverSQLQueryPage(
        //        sql, li, Order_Col, page_number, per_page_number, out msg);
        //    if (Universitiess_Full_Information != null && Universitiess_Full_Information.Rows.Count > 0)
        //    {
        //        List<ShowUniversityModel> dtList = Universitiess_Full_Information.AsEnumerable()
        //        .Select(row => new ShowUniversityModel
        //        {
        //            ID = row["ID"].ToString(),
        //            Name = row["Name"].ToString(),
        //            URL = row["Url"].ToString()
        //        }).ToList();

        //        return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult GetAllUniversitiesByCountry()
        {
            if (Request.QueryString["Country_ID"] == null)
            {
                return Json(new { @msg = "Enter Country ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            Guid Country_ID = new Guid(Request.QueryString["Country_ID"].ToString());
            int page_number = 1;
            if (Request.QueryString["Page_Number"] != null)
            {
                int.TryParse(Request.QueryString["Page_Number"], out page_number);
                if (page_number <= 0) page_number = 1;
            }

            int per_page_number = 10;
            if (Request.QueryString["Per_Page_Number"] != null)
            {
                int.TryParse(Request.QueryString["Per_Page_Number"], out per_page_number);
                if (per_page_number <= 0) per_page_number = 8;
            }


            int sorting = 1;
            if (Request.QueryString["Sorting"] != null)
            {
                int.TryParse(Request.QueryString["Sorting"], out sorting);
                if (sorting <= 0 || sorting > 2) sorting = 1;
            }

            string Order_Col = "Name ASC ";
            switch (sorting)
            {
                case 1:
                    Order_Col = "Name ASC ";
                    break;
                case 2:
                    Order_Col = "Name DESC ";
                    break;
                //case 3:
                //    Order_Col = "Final_Price ASC ";
                //    break;
                //case 4:
                //    Order_Col = "Final_Price DESC ";
                //    break;
                //case 5:
                //    Order_Col = "Date DESC ";
                //    break;
                //case 6:
                //    Order_Col = "Date ASC ";
                //    break;
            }
            string msg;
            string sql = "";
            sql += " select U.ID as ID ,U.Name,I.Url from Universities as U ";
            sql += " left join Images as I  on U.ID = I.Src_ID and I.Src_Type = 'Universities' ";
            sql += " where U.Address_ID in (select ID from Addresses where Neighborhood_ID in ( ";
            sql += " select ID from Neighborhoods where City_ID in ( ";
            sql += " select ID from Cities where Country_ID =@Country_ID )))";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@Country_ID", Country_ID));

            DataTable Universitiess_Full_Information = Database.ConverSQLQueryPage(
                sql, li, Order_Col, page_number, per_page_number, out msg);
            if (Universitiess_Full_Information != null && Universitiess_Full_Information.Rows.Count > 0)
            {
                List<ShowUniversityModel> dtList = Universitiess_Full_Information.AsEnumerable()
                .Select(row => new ShowUniversityModel
                {
                    ID = row["ID"].ToString(),
                    Name = row["Name"].ToString(),
                    URL = row["Url"].ToString()
                }).ToList();

                return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllUniversitiesBySearch()
        {
            if (Request.QueryString["Country_ID"] == null)
            {
                return Json(new { @msg = "Enter Country ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            Guid Country_ID = new Guid(Request.QueryString["Country_ID"].ToString());

            if (Request.QueryString["Search"] == null)
            {
                return Json(new { @msg = "Enter search Key !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string  search = Request.QueryString["Search"].ToString();
            int page_number = 1;
            if (Request.QueryString["Page_Number"] != null)
            {
                int.TryParse(Request.QueryString["Page_Number"], out page_number);
                if (page_number <= 0) page_number = 1;
            }

            int per_page_number = 10;
            if (Request.QueryString["Per_Page_Number"] != null)
            {
                int.TryParse(Request.QueryString["Per_Page_Number"], out per_page_number);
                if (per_page_number <= 0) per_page_number = 8;
            }


            int sorting = 1;
            if (Request.QueryString["Sorting"] != null)
            {
                int.TryParse(Request.QueryString["Sorting"], out sorting);
                if (sorting <= 0 || sorting > 2) sorting = 1;
            }

            string Order_Col = "Name ASC ";
            switch (sorting)
            {
                case 1:
                    Order_Col = "Name ASC ";
                    break;
                case 2:
                    Order_Col = "Name DESC ";
                    break;
                    //case 3:
                    //    Order_Col = "Final_Price ASC ";
                    //    break;
                    //case 4:
                    //    Order_Col = "Final_Price DESC ";
                    //    break;
                    //case 5:
                    //    Order_Col = "Date DESC ";
                    //    break;
                    //case 6:
                    //    Order_Col = "Date ASC ";
                    //    break;
            }
            string msg;
            string sql = "";
            sql += " select U.ID as ID ,U.Name,I.Url from Universities as U ";
            sql += " left join Images as I  on U.ID = I.Src_ID and I.Src_Type = 'Universities' ";
            sql += " where  U.Name like @search and U.Address_ID in (select ID from Addresses where Neighborhood_ID in ( ";
            sql += " select ID from Neighborhoods where City_ID in ( ";
            sql += " select ID from Cities where Country_ID =@Country_ID )))";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@Country_ID", Country_ID));
            li.Add(new SqlParameter("@search", "%" + search + "%"));

            DataTable Universitiess_Full_Information = Database.ConverSQLQueryPage(
                sql, li, Order_Col, page_number, per_page_number, out msg);
            if (Universitiess_Full_Information != null && Universitiess_Full_Information.Rows.Count > 0)
            {
                List<ShowUniversityModel> dtList = Universitiess_Full_Information.AsEnumerable()
                .Select(row => new ShowUniversityModel
                {
                    ID = row["ID"].ToString(),
                    Name = row["Name"].ToString(),
                    URL = row["Url"].ToString()
                }).ToList();

                return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUniversityInfo()
        {
            if (Request.QueryString["University_ID"] == null)
            {
                return Json(new { @msg = "Enter University ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string University_ID = Request.QueryString["University_ID"].ToString();

            string msg;
            string sql = "select U.ID , U.Name,T.Ar_Descreption,T.En_Descreption,T.TR_Descreption, I.URL,U.Domain, ";
            sql += " (CT.Ar_Value + '/' + CiT.Ar_Value + '/' + NT.Ar_Value + '/' + AD.Address_Details) as Ar_Address, ";
            sql += " (CT.En_Value + '/' + CiT.En_Value + '/' + NT.En_Value + '/' + AD.Address_Details) as En_Address, ";
            sql += " (CT.Tr_Value + '/' + CiT.Tr_Value + '/' + NT.Tr_Value + '/' + AD.Address_Details) as Tr_Address ";
            sql += " from Universities as U ";
            sql += " inner join Translations as T on U.ID = T.Src_ID ";
            sql += " left join Images as I on U.ID = I.Src_ID  and I.Src_Type = 'Universities' and I.Is_Main = 1";
            sql += " inner join Addresses as AD on U.Address_ID = AD.ID ";
            sql += " inner join Neighborhoods as N on AD.Neighborhood_ID = N.ID ";
            sql += " inner join Cities as Ci on N.City_ID = Ci.ID ";
            sql += " inner join Countries as C on Ci.Country_ID = C.ID ";
            sql += " inner join Translations as NT on N.ID = NT.Src_ID ";
            sql += " inner join Translations as CiT on Ci.ID = CiT.Src_ID ";
            sql += " inner join Translations as CT on C.ID = CT.Src_ID ";
            sql += " where T.Src_Type = 'Universities'  ";
            sql += " and NT.Src_Type = 'Neighborhoods'and CiT.Src_Type = 'Cities' and CT.Src_Type = 'Countries' ";
            sql +=" and U.ID = @UID";
            
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@UID",University_ID));

            DataTable universities = Database.ReadTableByQuery(sql, li, out msg);
            if (universities != null && universities.Rows.Count > 0)
            {
                DataRow university = universities.Rows[0];
                ShowUniversityLargModel UniverityData =  new ShowUniversityLargModel()
                {
                    ID = university["ID"].ToString(),
                    Name = university["Name"].ToString(),
                    URL = university["URL"].ToString(),
                    Ar_Address = university["Ar_Address"].ToString(),
                    Ar_Descreption = university["Ar_Descreption"].ToString(),
                    Domain = university["Domain"].ToString(),
                    En_Address = university["En_Address"].ToString(),
                    En_Descreption = university["En_Descreption"].ToString(),
                    Tr_Address = university["Tr_Address"].ToString(),
                    Tr_Descreption = university["TR_Descreption"].ToString()
                };
                return Json(new { @data = UniverityData, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Register_In_University()
        {
            UniversityRegistrationModel universityRegistration = new UniversityRegistrationModel();
            try
            {
                //  post.PopstInfo = JsonConvert.DeserializeObject<AddPostModel>(json);
                if (Request.Form != null && Request.Form.Count > 0)
                {
                    if (Request.Form["User_ID"] != null) universityRegistration.User_ID = new Guid(Request.Form["User_ID"]);
                    else return Json(new { @code = 404, msg = "enter User ID" });
                    if (Request.Form["University_ID"] != null) universityRegistration.University_ID = new Guid(Request.Form["University_ID"]);
                    else return Json(new { @code = 404, msg = "enter University ID" });
                    if (Request.Form["First_Name"] != null) universityRegistration.First_Name = Request.Form["First_Name"];
                    else return Json(new { @code = 404, msg = "enter First Name" });
                    if (Request.Form["Last_Name"] != null) universityRegistration.Last_Name = Request.Form["Last_Name"];
                    else return Json(new { @code = 404, msg = "enter Last Name" });
                    if (Request.Form["Father_Name"] != null) universityRegistration.Father_Name = Request.Form["Father_Name"];
                    else return Json(new { @code = 404, msg = "enter Father Name" });
                    if (Request.Form["Mother_Name"] != null) universityRegistration.Mother_Name = Request.Form["Mother_Name"];
                    else return Json(new { @code = 404, msg = "enter Mother Name" });
                    if (Request.Form["DateOfBirthday"] != null) universityRegistration.DateOfBirthday = Request.Form["DateOfBirthday"];
                    else return Json(new { @code = 404, msg = "enter Date Of Birthday" });
                    if (Request.Form["Phone"] != null) universityRegistration.Phone = Request.Form["Phone"];
                    else return Json(new { @code = 404, msg = "enter Phone" });
                    if (Request.Form["Email"] != null) universityRegistration.Email = Request.Form["Email"];
                    else return Json(new { @code = 404, msg = "enter Email" });
                    if (Request.Form["Nationality"] != null) universityRegistration.Nationality = Request.Form["Nationality"];
                    else return Json(new { @code = 404, msg = "enter Nationality" });
                    if (Request.Form["IsMale"] != null) universityRegistration.IsMale =Request.Form["IsMale"]=="1"?"Male":"Fmale";
                    else return Json(new { @code = 404, msg = "enter Gender" });
                    if (Request.Form["IsMarried"] != null) universityRegistration.IsMarried = Request.Form["IsMarried"] == "1" ? "Married" : "Single";
                    else return Json(new { @code = 404, msg = "enter Marital status" });

                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        if (Request.Files["Biometric"] != null)
                        {
                            var file = Request.Files["Biometric"];
                            MemoryStream target = new MemoryStream();
                            file.InputStream.CopyTo(target);
                            // Create  the file attachment for this email message.
                            Attachment data = new Attachment(target, file.ContentType);
                            // Add time stamp information for the file.
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(file.FileName);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file.FileName);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(file.FileName);
                            data.Name = "Biometric";
                            universityRegistration.Biometric = data;
                        }
                        else return Json(new { @code = 404, msg = "enter Biometric Image" });
                        if (Request.Files["Passport"] != null)
                        {
                            var file = Request.Files["Passport"];
                            MemoryStream target = new MemoryStream();
                            file.InputStream.CopyTo(target);
                            // Create  the file attachment for this email message.
                            Attachment data = new Attachment(target, file.ContentType);
                            // Add time stamp information for the file.
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(file.FileName);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file.FileName);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(file.FileName);
                            data.Name = "Passport";
                            universityRegistration.Passport = data;
                        }
                        else return Json(new { @code = 404, msg = "enter Passport Image" });
                        if (Request.Files["Transcript"] != null)
                        {
                            var file = Request.Files["Transcript"];
                            MemoryStream target = new MemoryStream();
                            file.InputStream.CopyTo(target);
                            // Create  the file attachment for this email message.
                            Attachment data = new Attachment(target, file.ContentType);
                            // Add time stamp information for the file.
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(file.FileName);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file.FileName);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(file.FileName);
                            data.Name = "Transcript";
                            universityRegistration.Transcript = data;
                        }
                        else return Json(new { @code = 404, msg = "enter Transcript Image" });
                        if (Request.Files["Certificate"] != null)
                        {
                            var file = Request.Files["Certificate"];
                            MemoryStream target = new MemoryStream();
                            file.InputStream.CopyTo(target);
                            // Create  the file attachment for this email message.
                            Attachment data = new Attachment(target, file.ContentType);
                            // Add time stamp information for the file.
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(file.FileName);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file.FileName);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(file.FileName);
                            data.Name = "Certificate";
                            universityRegistration.Certificate = data;
                        }
                        else return Json(new { @code = 404, msg = "enter Certificate Image" });
                    }
                    else
                    {
                        int code = 404;
                        string msg_ = "enter Email Attachments";
                        return Json(new { @code = code.ToString(), msg = msg_ });
                    }
                    string title= Database.GetRow("Users", universityRegistration.User_ID)["Username"].ToString();
                    title += "Ask to Registration in ";
                      title+= Database.GetRow("Universities", universityRegistration.University_ID)["Name"].ToString();
                    string Body = "Dear Admin...<br>";
                    Body += "First Name :"+universityRegistration.First_Name+ "<br>";
                    Body += "Last Name :" + universityRegistration.Last_Name + "<br>";
                    Body += "Father Name :" + universityRegistration.Father_Name + "<br>";
                    Body += "Mother Name :" + universityRegistration.Mother_Name + "<br>";
                    Body += "Date Of Birthday :" + universityRegistration.DateOfBirthday + "<br>";
                    Body += "Phone :" + universityRegistration.Phone + "<br>";
                    Body += "Email :" + universityRegistration.Email + "<br>";
                    Body += "Gender :" + universityRegistration.IsMale + "<br>";
                    Body += "Marital status :" + universityRegistration.IsMarried +"<br>";
                    Body += "Nationality :" + universityRegistration.Nationality + "<br>";
                    string msg = "";
                    List<Attachment> attachments = new List<Attachment>();
                    attachments.Add(universityRegistration.Biometric);
                    attachments.Add(universityRegistration.Passport);
                    attachments.Add(universityRegistration.Transcript);
                    attachments.Add(universityRegistration.Certificate);
                   if(EmailManager.SendEmail(title, Body, "university@stuhere.com", out msg, attachments))
                    {
                        //add to database in version 1.2
                        return Json(new { @data = "Email sended", @code = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int code = 404;
                        string msg_ = "Email sending failed" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                        return Json(new { @code = code.ToString(), msg = msg_ });
                    }
                }
                else
                {
                    int code = 404;
                    string msg = "enter Email information";
                    return Json(new { @code = code.ToString(), msg = msg });
                }


            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Login_To_University()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            Student_LogIn login = new Student_LogIn();
            try
            {
                login = JsonConvert.DeserializeObject<Student_LogIn>(json);
                if (HelperClass.IsObjectNullOrEmpty(login.UserID))
                    return Json(new { @msg = "Enter User ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
                if (HelperClass.IsObjectNullOrEmpty(login.UniversityID))
                    return Json(new { @msg = "Enter University ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
                if (HelperClass.IsObjectNullOrEmpty(login.Email))
                    return Json(new { @msg = "Enter University Email!", @code = 404 }, JsonRequestBehavior.AllowGet);

                string sql = "select User_Type_ID,Std_Email from Student_Universities where User_ID =@USID and University_ID =@UnivID";
                List<SqlParameter> li = new List<SqlParameter>();
                li.Add(new SqlParameter("@USID", login.UserID));
                li.Add(new SqlParameter("@UnivID", login.UniversityID));
                string msg;
                DataTable std_u = Database.ReadTableByQuery(sql, li, out msg);
                if(std_u != null && std_u.Rows.Count > 0)
                {
                    DataRow R_std_u = std_u.Rows[0];
                    if(login.Email != R_std_u["Std_Email"].ToString())
                        return Json(new { @msg = "Email is not correct!", @code = 404 }, JsonRequestBehavior.AllowGet);
                    string title = "Stuhere - Verfication Code";
                    string verfication = HelperClass.RandomString(6);
                    string Body = "your verfication code to sign in to unversity is : " + verfication;
                     msg = "";
                    if (EmailManager.SendEmail(title, Body,login.Email, out msg, null))
                    {
                        Student_Login_verfication info = new Student_Login_verfication()
                        {
                            type = R_std_u["User_Type_ID"].ToString(),
                            verfication = verfication
                        };
                        return Json(new { @data = info, @code = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int code = 404;
                        string msg_ = "Email sending failed" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                        return Json(new { @code = code.ToString(), msg = msg_ });
                    }
                }
                else
                {
                    int code = 404;
                    string msg_ = "Date not found" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                    return Json(new { @code = code.ToString(), msg = msg_ });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult IS_Student_IN_University()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            Old_Student_LogIn login = new Old_Student_LogIn();
            try
            {
                login = JsonConvert.DeserializeObject<Student_LogIn>(json);
                if (HelperClass.IsObjectNullOrEmpty(login.UserID))
                    return Json(new { @msg = "Enter User ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
                if (HelperClass.IsObjectNullOrEmpty(login.UniversityID))
                    return Json(new { @msg = "Enter University ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
               

                string sql = "select User_Type_ID from Student_Universities where User_ID =@USID and University_ID =@UnivID";
                List<SqlParameter> li = new List<SqlParameter>();
                li.Add(new SqlParameter("@USID", login.UserID));
                li.Add(new SqlParameter("@UnivID", login.UniversityID));
                string msg;
                DataTable std_u = Database.ReadTableByQuery(sql, li, out msg);
                if (std_u != null && std_u.Rows.Count > 0)
                {
                    DataRow R_std_u = std_u.Rows[0];
                    
                    return Json(new { @data = R_std_u["User_Type_ID"].ToString(), @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int code = 404;
                    string msg_ = "Date not found" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                    return Json(new { @code = code.ToString(), msg = msg_ });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}