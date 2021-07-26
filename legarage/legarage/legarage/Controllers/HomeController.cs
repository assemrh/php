using legarage.Classes;
using legarage.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace legarage.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            string msg = "";
            string sql = @"select S.id,
	                            I.url AS URL,
	                            I.id  AS IID,
	                            S.description,
								S.referral_type,
								s.referral_id,
	                            S.link ,
	                            S.title
	                            FROM Slider AS S 
                                LEFT JOIN Images AS I on S.id = I.referral_id ORDER BY S.roworder ";

            DataTable dataTable = Database.ReadTableByQuery(sql, null, out msg);
            List<SlidersModel> Sliders = new List<SlidersModel>();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow slider in dataTable.Rows)
                {
                    //DataRow slider = dataTable.Rows[i];
                    SlidersModel s = new SlidersModel();
                    s.ID = new Guid(slider["id"].ToString());
                    //if(HelperClass.NotNull(slider["IID"]) && slider["IID"].ToString()!=string.Empty)
                    //{
                    //    s.Image = new ImagesModel()
                    //    {
                    //        ID = new Guid(slider["IID"].ToString()) ,
                    //        URL = slider["URL"].ToString()// : "no-images.png"
                    //    };
                    //}
                    //else {
                    //    s.Image = new ImagesModel()
                    //    {
                    //        URL =  "no-images.png"
                    //    };
                    //}
                    if (slider["referral_type"] == null || slider["referral_type"].ToString() == string.Empty)
                    {
                        s.Title = slider["Title"].ToString();
                        s.Link = slider["Link"].ToString();
                        s.Image = new ImagesModel()
                        {
                            URL = slider["URL"].ToString(),
                            ID = new Guid(slider["IID"].ToString())
                        };

                    }
                    else
                    {
                        s.ReferralType = slider["referral_type"].ToString();
                        string referral_type = slider["referral_type"].ToString();
                        s.ReferralID = new Guid(slider["referral_id"].ToString());
                        string id = slider["referral_id"].ToString();
                        string masg = "";
                        string sql1 = "";
                        sql1 += " select ";
                        if (referral_type == "Garages" || referral_type == "Rental_Offices")
                            sql1 += "S.id, S.name as title ,I.url AS URL, I.id  AS ImageID ";
                        else
                            sql1 += " S.title ";
                        sql1 += " from " + referral_type + " AS S inner join Images AS I ON I.referral_id = S.id ";

                        sql1 += " where S.id = @id";
                        List<SqlParameter> li1 = new List<SqlParameter>();
                        li1.Add(new SqlParameter("@id", id));
                        DataTable data = Database.ReadTableByQuery(sql1, li1, out masg);
                        s.Title = data.Rows[0]["title"].ToString();
                        s.Image = new ImagesModel()
                        {
                            URL = data.Rows[0]["URL"].ToString(),
                            ID = new Guid(data.Rows[0]["ImageID"].ToString())
                        };

                    }
                    s.Title = slider["Title"].ToString();
                    s.Link = slider["Link"].ToString();
                    Sliders.Add(s);
                }
            }
            else
            {
                SlidersModel s = new SlidersModel();
                s.ID = new Guid();
                s.Image = new ImagesModel()
                {
                    ID = new Guid(),
                    URL = "default.png"
                };
                s.Title = "Welcome ..";
                s.Link = "#";
                Sliders.Add(s);
            }
            if (Request.QueryString["lng"] != null)
            {
                HttpCookie ck = new HttpCookie("legarage_lng", Request.QueryString["lng"]);
                ck.Expires = DateTime.Now.AddMonths(6);
                Response.Cookies.Add(ck);
                Response.Redirect("/");
            }
            return View(Sliders);

        }


        public ActionResult Viewd()
        {
            return Json(new { name = "amir" }, JsonRequestBehavior.AllowGet);
        }

        [Route("BuildDB")]
        public ActionResult BuildDB()
        {
            Build_Database.RebuildDatabase();
            //return View("Index");
            return Json("OK");
        }

        public ActionResult _ItemSliderPartialtest()
        {
            return PartialView();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult MyServices()
        {
            return View();
        }

        public ActionResult CPIndex()
        {
            return View("~/Views/CP/CPIndex.cshtml");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LogOut()
        {
            Session["token"] = null;
            HttpCookie ck = new HttpCookie("token", "");
            ck.Expires = DateTime.Now.AddMonths(-6);
            Response.Cookies.Add(ck);
            return RedirectToAction("Index", "Home");
        }
        //public ActionResult Get_Market()
        //{

        //    Session["Market_Name"] = "";
        //    return null;
        //}
        [HttpPost]
        public JsonResult DropAttachmentsSingle()
        {
            if (Request.Files[0] != null)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    byte[] b = new byte[Request.Files[i].ContentLength];
                    Request.Files[i].InputStream.Read(b, 0, Request.Files[i].ContentLength);

                    Session["Attachment"] = b;
                    Session["Attachment_File_Name"] = Request.Files[i].FileName;
                }
            }
            return Json(new { code = 200 });
        }

        [HttpPost]
        public JsonResult CelarImagesSession()
        {
            Session["Attachment"] = null;
            Session["Attachment_File_Name"] = null;

            return Json(new { code = 200 });
        }


        public JsonResult GetCountries(string Option = "", bool iscpanel = false) //Option => MarketOrFactoryOrAll
        {
            string morf, msg;
            switch (Option)
            {
                case "Market":
                    string market = Session["market"].ToString();
                    morf = string.Format(" where " + ((market == "all" || iscpanel) ? " is_market =1 " : "  id= '{0}' "), market);
                    break;
                case "Factory":
                    morf = " where is_factory =1";
                    break;
                default:
                    morf = "";
                    break;
            }


            DataTable countries = Database.ReadTable("Countries " + morf, out msg);
            if (Session["lng"].ToString() == "en")
            { countries.DefaultView.Sort = "name asc"; }
            if (Session["lng"].ToString() == "ar")
            { countries.DefaultView.Sort = "ar_name asc"; }
            //countries.DefaultView.Sort = "name asc";
            countries = countries.DefaultView.ToTable();
            List<CountriesModel> CountriesList = new List<CountriesModel>();

            if (msg == string.Empty)
            {
                if (countries != null && countries.Rows.Count > 0)
                {
                    foreach (DataRow row in countries.Rows)
                    {
                        if (Session["lng"].ToString() == "en")
                        { CountriesList.Add(new CountriesModel() { ID = new Guid(row["id"].ToString()), Name = row["Name"].ToString() }); }
                        if (Session["lng"].ToString() == "ar")
                        { CountriesList.Add(new CountriesModel() { ID = new Guid(row["id"].ToString()), Name = row["ar_Name"].ToString() }); }
                        //CountriesList.Add(new CountriesModel() { ID = new Guid(row["id"].ToString()), Name = row["Name"].ToString() });
                    }
                }
                string json_contries = JsonConvert.SerializeObject(CountriesList);
                return Json(new { code = 200, data = json_contries, label = Resources.CP.SelectCountry });
            }
            else
            {
                return Json(new { code = 404, msg = msg });
            }

            //string HTML_Content = "<option value='-1'>" + Resources.CP.Chose + " " + Resources.CP.Country + "</option>";

            //if (countries != null && countries.Rows.Count > 0)
            //{

            //    string HTML_Content_Code = "<option value='-1'>" + Resources.CP.Chose + " " + Resources.CP.Code + "</option>";
            //    foreach (DataRow row in countries.Rows)
            //    {
            //        string country_name = row["name"].ToString();
            //        HTML_Content += "<option value=\"" + row["id"].ToString() + "\">" + country_name + "</option>";
            //        string country_code = row["Phone_key"].ToString();
            //        HTML_Content_Code += "<option value=\"" + country_code + "\">" + country_code + "</option>";
            //    }
            //    return Json(new { code = 200, data = HTML_Content, data_code = HTML_Content_Code });
            //}
            //else
            //{
            //    return Json(new { code = 404, msg = msg });
            //}
        }
        public JsonResult GetPhoneKeys()
        {
            string HTML_Content = "";
            DataTable markets = new DataTable();
            string msg = "";
            string orderby = (Session["lng"].ToString() == "en") ? " order by name " : (Session["lng"].ToString() == "ar") ? " order by ar_name " : "";
            markets = Database.ReadTable("Countries ", orderby, null, out msg);

            if (markets != null && markets.Rows.Count > 0)
            {
                foreach (DataRow Contry in markets.Rows)
                {
                    HTML_Content += "<div role=\"option\"  class=\"dropdown-item\" onclick=\"phone_key('" + Contry["ISO"].ToString() + "', '" + Contry["Phone_key"].ToString() + "')\">" +
                           "<span id = \"countryflag\"><img src=\"https://www.countryflags.io/" + Contry["ISO"].ToString() + "/shiny/24.png\" height=\"24\" >&nbsp;" +
                           "</span>";
                    if (Session["lng"].ToString() == "en")
                    { HTML_Content += Contry["name"].ToString(); }
                    if (Session["lng"].ToString() == "ar")
                    { HTML_Content += Contry["ar_name"].ToString(); }
                    HTML_Content += "&nbsp; (<span id = \"getPhoneKey\"> +" + Contry["Phone_key"].ToString() + "</span>)</div>";
                }
                return Json(new { code = 200, data = HTML_Content });
            }
            else
            {
                return Json(new { code = 404, msg = msg });
            }

        }

        [HttpPost]
        public JsonResult GetCities(string ID = "")
        {
            string msg = "";
            if (Session["Market"].ToString() != "all") ID = Session["Market"].ToString();
            List<SqlParameter> li = new List<SqlParameter>();
            List<CitiesModel> CityList = new List<CitiesModel>();
            li.Add(new SqlParameter("@CID", ID));
            DataTable cities = Database.ReadTable("Provinces", " Where country_id = @CID", li, out msg);
            if (msg == string.Empty)
            {
                if (cities != null && cities.Rows.Count > 0)
                {
                    foreach (DataRow row in cities.Rows)
                    {
                        CityList.Add(new CitiesModel() { ID = new Guid(row["id"].ToString()), Name = row["name"].ToString() });
                    }
                }
                string json_brands = JsonConvert.SerializeObject(CityList);
                return Json(new { code = 200, data = json_brands, label = Resources.Garages.SelectCity });
            }
            else
            {
                return Json(new { code = 404, msg = msg });
            }


        } 

        [HttpPost]
        public JsonResult GetBrands(string ID)
        {
            string msg = "";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@ID", ID));
            List<BrandsModel> Branlist = new List<BrandsModel>();
            DataTable brands = Database.ReadTable("Brands", " Where country_id = @ID", li, out msg);
            if (msg == string.Empty)
            {
                if (brands != null && brands.Rows.Count > 0)
                {
                    foreach (DataRow row in brands.Rows)
                    {
                        Branlist.Add(new BrandsModel() { ID = new Guid(row["id"].ToString()), Name = row["name"].ToString() });
                    }
                }
                string json_brands = JsonConvert.SerializeObject(Branlist);
                return Json(new { code = 200, data = json_brands });
            }
            else
            {
                return Json(new { code = 404, msg = msg });
            }
        }
        [HttpPost]
        public JsonResult GetModels(string ID)
        {
            string msg = "";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@ID", ID));
            List<BrandsModel> BrandList = new List<BrandsModel>();
            DataTable brands = Database.ReadTable("Models", " Where brand_id = @ID", li, out msg);
            if (msg == string.Empty)
            {
                if (brands != null && brands.Rows.Count > 0)
                {
                    foreach (DataRow row in brands.Rows)
                    {
                        BrandList.Add(new BrandsModel() { ID = new Guid(row["id"].ToString()), Name = row["name"].ToString() });
                    }

                }
                string json_brands = JsonConvert.SerializeObject(BrandList);
                return Json(new { 
                    code = 200,
                    data = json_brands,
                    labels = new {
                        SelectModel = Resources.Garages.SelectModel
                    }
                });
            }
            else
            {
                return Json(new
                {
                    code = 404,
                    msg = msg,

                });
            }
        }
        public JsonResult GetVehicleTypes()
        {

            string msg = "";
            List<VehicleTypesModel> VehicleTypeList = new List<VehicleTypesModel>();
            DataTable vehiclesTable = Database.ReadTable("Vehicle_Types", " ", null, out msg);

            if (msg == string.Empty)
            {
                if (vehiclesTable != null && vehiclesTable.Rows.Count > 0)
                {
                    foreach (DataRow VT in vehiclesTable.Rows)
                    {
                        VehicleTypeList.Add(new VehicleTypesModel() { ID = new Guid(VT["id"].ToString()), Name = VT["type_name"].ToString() });

                    }
                }
                string json_brands = JsonConvert.SerializeObject(VehicleTypeList);
                return Json(new { code = 200, data = json_brands });
            }
            else
            {
                return Json(new { code = 404, msg = msg });
            }
        }
        public JsonResult GetMarket()
        {
            string MID = (Session["market"].ToString() == "all") ? Convert.ToString(Guid.Empty) : Session["market"].ToString();
            Guid ID = new Guid(MID);
            DataRow market;
            string market_name = "", iso = "";
            market = Database.GetRow("Countries", ID); //.ReadTable("Countries", " where is_market =1", null, out msg);
            if (market != null)
            {

                market_name = (Session["lng"].ToString() == "en") ? market["name"].ToString() : (Session["lng"].ToString() == "ar") ? market["ar_name"].ToString() : "";
                /*/if (Session["lng"].ToString() == "en")
                { market_name = market["name"].ToString(); }
                if (Session["lng"].ToString() == "ar")
                { market_name = market["ar_name"].ToString(); }*/

                iso = market["iso"].ToString();
                Session["market_name"] = market["name"].ToString();
                return Json(new { code = 200, name = market_name, iso = iso });
            }
            else
            {
                if (Session["lng"].ToString() == "en")
                    return Json(new { code = 200, name = "All Market" });
                else if (Session["lng"].ToString() == "ar")
                    return Json(new { code = 200, name = "عام" });
                else
                    return Json(new { code = 200, name = "All Market" });
            }

        }
        [HttpPost]
        public JsonResult SelectMarket(string MID)
        {
            Session["market"] = MID;
            return Json(new { code = 200 });
        }

        [HttpGet]
        public JsonResult GetSearchResult() 
        {
            string keyWord = Request.Params["keyWord"];
            string sql = @" SELECT U.id, Title, Type, keywords,I.url FROM (SELECT 'Garages' AS Type, id, G.name Title, keywords FROM Garages G
                            UNION
                            SELECT 'Offers' AS Type, id, CAST(O.name AS VARCHAR(max)) title, O.name keywords FROM Offers O
                            UNION
                            SELECT 'Products' AS Type, id, CAST(p.title AS VARCHAR(max)) title, keywords FROM Products P
                            UNION
                            SELECT 'RentOffice' AS Type, id, CAST(R.name AS VARCHAR(max)) title , R.name keywords FROM Rental_Offices R
                            UNION
                            SELECT 'Vehicles' AS Type, id, CAST(V.title AS VARCHAR(max)) title  , keywords FROM Vehicles V
                            UNION
                            SELECT 'Winches' AS Type, id, CAST(w.title AS VARCHAR(max)) title , keywords FROM Winches W
                            ) as U 
                             left join Images I on U.id = I.referral_id and I.is_main = 1 
                                where keywords like @keyWord
                                ";
            keyWord = $"%{keyWord}%";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@keyWord", keyWord) };
            var dataTable = Database.ReadTableByQuery(sql, parameters, out string str);
            List<object> list = new List<object>();
            foreach(DataRow row in dataTable.Rows)
            {
                string dir = Server.MapPath($"~/Images/{row["Type"].ToString()}/");
                string url = System.IO.File.Exists(dir+row["URL"].ToString()) ? row["URL"].ToString() : "no-img.jpg";
                list.Add(
                    new { 
                        ID = row["id"].ToString(),
                        Type = row["Type"].ToString(),
                        keywords = row["keywords"].ToString(),
                        Title = row["Title"].ToString(),
                        Img = url
                    });
            }
            return Json(new { 
                code= 200,
                count=dataTable.Rows.Count,
                data = list
            }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult DropAllDataFromDatabases()
        //{

        //    Build_Database.DropAllDataFromDatabases();
        //    return RedirectToAction("Index");
        //}

        public ActionResult InsertCountries()
        {


            Build_Database.EnterCuntries();
            return RedirectToAction("Index");
        }
        //declare @command varchar(15)  set @command = 'drop table ?'  exec sp_msforeachtable @command
    }
}