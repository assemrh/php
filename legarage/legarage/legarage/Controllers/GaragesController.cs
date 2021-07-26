using legarage.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using static legarage.Classes.Database;

namespace legarage.Controllers
{
    public class GaragesController : BaseController
    {

        public ActionResult Index()
        {
            string msg;
            Session["data"] = null;
            Session["current_page"] = 1;
            List<GaragesModel> garageslist = GetGaragesList();
            
            //const int pageSize = 10;
            //int page = 1;
            //if (page<1)
            //{
            //    page = 1;
            //}

            //int recsCount = GetGaragesList().Count;

            //var pager = new Pager(recsCount, page, pageSize);

            //int recSkip = (page - 1) * pageSize;

            //var data = garageslist.Skip(recSkip).Take(pager.PagesSize).ToList();

            //this.ViewBag.Pager = pager;






            List<VehicleTypesModel> VehicleTypes = new List<VehicleTypesModel>();
            VehicleTypes = new List<VehicleTypesModel>();
            DataTable vehiclesTable = new DataTable();
            string sqlquery = "SELECT id,type_name FROM Vehicle_Types;";
            vehiclesTable = ReadTableByQuery(sqlquery, null, out msg);
            if (vehiclesTable != null && vehiclesTable.Rows.Count > 0)
            {
                foreach (DataRow v in vehiclesTable.Rows)
                {
                    VehicleTypes.Add(new VehicleTypesModel() {
                        ID = new Guid(v["id"].ToString()),
                        Name = v["type_name"].ToString()
                    }) ;
                }
            }
            ViewData["VehicleTypes"] = VehicleTypes;

            List<BrandsModel> Brands = new List<BrandsModel>();
            DataTable BrandsTable = new DataTable();
            sqlquery = "SELECT id, name  FROM Brands;";
            BrandsTable = ReadTableByQuery(sqlquery, null, out  msg);
            if (BrandsTable != null && BrandsTable.Rows.Count > 0)
            {
                foreach (DataRow B in BrandsTable.Rows)
                {
                    Brands.Add(new BrandsModel()
                    {
                        ID = new Guid(B["id"].ToString()),
                        Name = B["name"].ToString()
                    });
                }
            }
            ViewData["Brands"] = Brands;
            List<CountriesModel> Markets = new List<CountriesModel>();
            DataTable CountriesTable = new DataTable();
            string market = Session["market"].ToString();
            sqlquery = string.Format(" SELECT id, name FROM Countries where "+((market == "all") ? " is_market =1 " : "  id= '{0}' "), market);
            CountriesTable = ReadTableByQuery(sqlquery, null, out msg);
            if (CountriesTable != null && CountriesTable.Rows.Count > 0)
            {
                foreach (DataRow C in CountriesTable.Rows)
                {
                    Markets.Add(new CountriesModel() { ID = new Guid(C["id"].ToString()), Name = C["name"].ToString() });
                }
            }
            List<CountriesModel> Factories = new List<CountriesModel>();
            DataTable FactoriesTable = new DataTable();
            sqlquery = " SELECT id, name FROM Countries where  is_factory =1 ";
            CountriesTable = ReadTableByQuery(sqlquery, null, out msg);
            if (CountriesTable != null && CountriesTable.Rows.Count > 0)
            {
                foreach (DataRow C in CountriesTable.Rows)
                {
                    Factories.Add(new CountriesModel() { ID = new Guid(C["id"].ToString()), Name = C["name"].ToString() });
                }
            }
            List<CitiesModel> Cities = new List<CitiesModel>();
            DataTable CityTable = new DataTable();
            sqlquery = "SELECT id, name  FROM Provinces;";
            sqlquery = string.Format("SELECT P.id ID ,P.name Name ,country_id  FROM Provinces P inner join Countries C on P.country_id = C.Id where P.country_id like " + ((market == "all") ? " ' ' " : " '{0}' "), market);
            CityTable = ReadTableByQuery(sqlquery, null, out msg);
            if (CityTable != null && CityTable.Rows.Count > 0)
            {
                foreach (DataRow c in CityTable.Rows)
                {
                    Cities.Add(new CitiesModel() {ID= new Guid(c["id"].ToString()),Name=c["name"].ToString() });
                }
            }
            List<ServicesModel> Categories = new List<ServicesModel>();
            DataTable ServiceTable = new DataTable();
            sqlquery = "SELECT id, name FROM Categories;";
            ServiceTable = ReadTableByQuery(sqlquery, null, out msg);
            if (ServiceTable != null && ServiceTable.Rows.Count > 0)
            {
                foreach (DataRow s in ServiceTable.Rows)
                {
                    Categories.Add(new ServicesModel() {ID=new Guid(s["id"].ToString()), Name=s["name"].ToString() });
                }
            }
            ViewData["Categories"] = Categories;

            return View(garageslist); 
        }
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = null, Adding = "/Garages/AddGarages/" });
        }
        [HttpPost]
        public JsonResult AddGarages()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            GaragesModel new_garage = new GaragesModel();
            new_garage.Name = Request.Params["Name"] != null ? Request.Params["Name"] : "";
            new_garage.Mobile = Request.Params["phoneno"].ToString() != string.Empty ? Request.Params["phone_key"] + " " + Request.Params["phoneno"] : null;
            new_garage.Fax = Request.Params["Fax"] != null ? Request.Params["Fax"] : "";
            new_garage.Description = Request.Params["Description"] != null ? Request.Params["Description"] : "";
            new_garage.Website = Request.Params["Website"] != null ? Request.Params["Website"] : "";
            new_garage.Whatsapp = Request.Params["Whatsapp"] != null ? Request.Params["Whatsapp"] : "";
            new_garage.Facebook = Request.Params["Facebook"] != null ? Request.Params["Facebook"] : "";
            new_garage.Tiktok = Request.Params["Tiktok"] != null ? Request.Params["Tiktok"] : "";
            new_garage.Snapchat = Request.Params["Snapchat"] != null ? Request.Params["Snapchat"] : "";
            new_garage.Twitter = Request.Params["Twitter"] != null ? Request.Params["Twitter"] : "";
            new_garage.Instagram = Request.Params["Instagram"] != null ? Request.Params["Instagram"] : "";
            new_garage.Linkedin = Request.Params["Linkedin"] != null ? Request.Params["Linkedin"] : "";
            new_garage.Youtube = Request.Params["Youtube"] != null ? Request.Params["Youtube"] : "";
            new_garage.Keywords = Request.Params["Keywords"] != null ? Request.Params["Keywords"] : "";
            new_garage.User = new UsersModel();
            new_garage.User.ID = new Guid(Session["id"].ToString());

            new_garage.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                new_garage.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                new_garage.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                new_garage.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                new_garage.Address.ProvinceId = new Guid();
            }

            if (ISValid(new_garage, out msg))
            {
                Guid id = Guid.NewGuid();
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "user_id", "name", "mobile", "fax", "whatsapp", "facebook", "tiktok", "snapchat", "twitter", "instagram", "linkedin", "youtube", "website", "Keywords", "description", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_garage.User.ID, new_garage.Name, new_garage.Mobile, new_garage.Fax, new_garage.Whatsapp, new_garage.Facebook, new_garage.Tiktok, new_garage.Snapchat, new_garage.Twitter,
                    new_garage.Instagram, new_garage.Linkedin, new_garage.Youtube, new_garage.Website,new_garage.Keywords, new_garage.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (InsertRow("Garages", id, cols, vals, out errMessage))
                {
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { new_garage.Address.ProvinceId, new_garage.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    UpdateRow("Garages", id, cols, vals, out errMessage);
                    var ss = Request.Params["Services"].Split(',');
                    List<string> services = ss.ToList();
                    foreach (string service in services)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "garage_id", "category_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, service, DateTime.Now };
                        vals.AddRange(valsin);
                        InsertRow("Garages_Categories", Guid.NewGuid(), cols, vals, out errMessage);
                    }

                    var br = Request.Params["Brands"].Split(',');
                    List<string> brands = br.ToList();
                    foreach (string brand in brands)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "garage_id", "brand_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, brand, DateTime.Now };
                        vals.AddRange(valsin);
                        InsertRow("Garages_Brands", Guid.NewGuid(), cols, vals, out errMessage);
                    }

                    var vt = Request.Params["Vehicletypes"].Split(',');
                    List<string> vehicletypes = vt.ToList();
                    foreach (string vehicletype in vehicletypes)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "garage_id", "vehicle_type_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, vehicletype, DateTime.Now };
                        vals.AddRange(valsin);
                        InsertRow("Vehicle_Types_Garages", Guid.NewGuid(), cols, vals, out errMessage);
                    }
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = Session["Attachment_File_Name"]?.ToString();
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Garages/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { ImageName, id, "Garages", DateTime.Now, 1 };
                        vals.AddRange(valsinput);
                        InsertRow("Images", ImageID, cols, vals, out errMessage);
                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Garages.Added });
                }
                else
                {
                    code = 404;
                    msg = "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                    return Json(new { code = code.ToString(), msg = msg });
                }
            }
            else
            {
                code = 404;
                return Json(new { code = code.ToString(), msg = msg });
            }
        }
        [Route("Garages/Details")]
        [Route("Garages/GarageDetails")]

        public ActionResult GarageDetails()
        {
           if(Request.QueryString["id"]!=null)
            { 
                Guid id = new Guid(Request.QueryString["id"].ToString());
                GaragesModel garage = new GaragesModel();
                DataTable details = new DataTable();
                DataTable getType = new DataTable();
                DataTable getBrands = new DataTable();
                DataTable getServices = new DataTable();
                DataTable getImages = new DataTable();
                string msg = "";
                List<SqlParameter> li = new List<SqlParameter>();

                string queryDetails = "select g.name,(select full_name from Users where id = g.user_id) as owner," +
                               "(select name from Countries where id in (select country_id from Provinces where id in (select province_id from Addresses where id = g.address_id))) as country," +
                               "(select name from Provinces where id in (select province_id from Addresses where id = g.address_id)) as city," +
                               "(select details from Addresses where id = g.address_id) as address,g.mobile, " +
                               " (select avg(value) from Rating where Src_ID  = g.id) as Rate, "+
                               " (select count(value) from Rating where Src_ID  = g.id) as RateCount, " +
                               "g.website,g.fax,g.whatsapp,g.facebook,g.twitter,g.instagram,g.youtube,g.snapchat,g.tiktok,g.linkedin,g.keywords,g.description , i.url  from Garages as g inner join Images as i " +
                               " on i.referral_id = g.id where g.id = @gid and i.referral_type = 'Garages' and is_main = 1";
                string queryType = "select type_name from Vehicle_Types where id in (select vehicle_type_id from Vehicle_Types_Garages where garage_id = @gid)";
                string queryBrands = "select name from Brands where id in (select brand_id from Garages_Brands where garage_id = @gid)";
                string queryServices = "select name from Categories where id in (select category_id from Garages_Categories where garage_id = @gid)";
                string queryImages = "select url from Images inner join Garages as g on referral_id = @gid";

                li.Add(new SqlParameter("@gid", id));                
                details = ReadTableByQuery(queryDetails, li, out msg);
                li= new List<SqlParameter>();
                li.Add(new SqlParameter("@gid", id));
                getType = ReadTableByQuery(queryType, li, out msg);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@gid", id));
                getBrands = ReadTableByQuery(queryBrands, li, out msg);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@gid", id));
                getServices = ReadTableByQuery(queryServices, li, out msg);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@gid", id));
                getImages = ReadTableByQuery(queryImages, li, out msg);

                //get details
                if(details !=null && details.Rows.Count>0)
                {
                    DataRow garage_row = details.Rows[0];
                    garage.ID = id;
                    garage.Name = garage_row["name"]?.ToString();

                    garage.User = new UsersModel() { Name = garage_row["owner"]?.ToString() };
                    garage.Address = new AddressModel();
                    garage.Address.Country = garage_row["country"]?.ToString();
                    garage.Address.Province = garage_row["city"]?.ToString();
                    garage.Address.AddressName = garage_row["address"]?.ToString();
                    garage.Mobile = garage_row["mobile"]?.ToString();
                    garage.Website = garage_row["website"]?.ToString();
                    garage.Fax = garage_row["fax"]?.ToString();
                    garage.Whatsapp = garage_row["whatsapp"]?.ToString();
                    garage.Facebook = garage_row["facebook"]?.ToString();
                    garage.Whatsapp = garage_row["whatsapp"]?.ToString();
                    garage.Twitter = garage_row["twitter"]?.ToString();
                    garage.Instagram = garage_row["instagram"]?.ToString();
                    garage.Youtube = garage_row["youtube"]?.ToString();
                    garage.Snapchat = garage_row["snapchat"]?.ToString();
                    garage.Tiktok = garage_row["tiktok"]?.ToString();
                    garage.Linkedin = garage_row["linkedin"]?.ToString();
                    garage.Keywords = garage_row["keywords"]?.ToString();
                    garage.Description = garage_row["description"]?.ToString();
                    float f = 0f;
                    if (garage_row["RateCount"] != null && (int)garage_row["RateCount"] > 0)
                    {
                        float.TryParse(garage_row["Rate"].ToString(), out f);
                        garage.Rate = f;
                    }

                    garage.Image = new ImagesModel()
                    {
                        URL = garage_row["url"]?.ToString()
                    };
                }

                //get vehicle types
                garage.VehicleTypes = new List<VehicleTypesModel>();
                if (getType != null && getType.Rows.Count > 0)
                {
                    foreach (DataRow vehicle_type in getType.Rows)
                    {
                        garage.VehicleTypes.Add(new VehicleTypesModel() { Name = vehicle_type["type_name"]?.ToString() });
                    }


                }

                //get brands
                garage.Brands = new List<BrandsModel>();
                if (getBrands != null && getBrands.Rows.Count > 0)
                {
                    foreach (DataRow brands in getBrands.Rows)
                    {
                        garage.Brands.Add(new BrandsModel() { Name = brands["name"]?.ToString() });
                    }
                }

                //get services
                garage.Services = new List<ServicesModel>();
                if (getServices != null && getServices.Rows.Count > 0)
                {
                    foreach (DataRow services in getServices.Rows)
                    {
                        garage.Services.Add(new ServicesModel() { Name = services["name"]?.ToString() });
                    }
                }

                //get Images
                garage.Images = new List<ImagesModel>();
                if (getImages != null && getImages.Rows.Count > 0)
                {
                    foreach (DataRow images in getImages.Rows)
                    {
                        garage.Images.Add(new ImagesModel() { URL = images["url"]?.ToString() });
                    }
                }
                
                return View(garage);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        [HttpGet]
        public ActionResult GetGarages(int page = 1)
        {
            Session["current_page"] = page;
            List<GaragesModel> garageslist = GetGaragesList();

            return PartialView("Content", garageslist);
            //return Content($"no data <br>");
        }
        //[HttpGet]
        //public JsonResult GetGarages()
        //{
        //    DataTable garages;
        //    GetGarages(out garages);
        //    string HTML_Content = String.Empty;
        //    if (garages != null && garages.Rows.Count > 0)
        //    {
        //        foreach (DataRow garage in garages.Rows)
        //        {
        //            HTML_Content += "<div class=\"card col-lg-5 index-card\" id=\"garages_div\" onclick=\"goToGarageDetails('" + garage["gid"].ToString() + "');\">"
        //                    + "<span class=\"rating2\">"
        //                        + "<em class=\"fa fa-star checked\"></em> " + "<em class=\"fa fa-star checked\"></em> " + "<em class=\"fa fa-star checked\"></em> " + "<em class=\"fa fa-star checked\"></em> " + "<em class=\"fa fa-star checked\"></em> " + "</span>"
        //                        + "<img class=\"card-img-top\" src=\"/Images/Garages/" + garage["Img"].ToString() + "\" alt=\"Card image cap\">"
        //                        + "<div class=\"card-body\">" + "<div class=\"row\">" + "<div class=\"col col-lg\">"
        //                        + "<h4 class=\"card-title\" style=\"display:inline\">" + garage["g_name"].ToString() + "</h4></div>"
        //                        + "<div class=\"col-md align-self-end\" style=\"text-align: end;\">"
        //                        + "<em style = \"color:dimgray;\" ><i class=\"fa fa-phone text-success fa-1x fa-flip-horizontal\" aria-hidden=\"true\"></i></em>"
        //                        + garage["Mobile"].ToString()
        //                        + "</div>" + " </div>" + "<hr />" + "<button class=\"btn btn-outline-primary\" onclick=\"goToGarageDetails('" + garage["gid"].ToString() + "');\" hidden>" + Resources.Home.garages_show + "</button>"
        //                        + "<div class=\"\">" + "<div class=\"row details\">" + "<div class=\"col-12 col-sm-12\">" + "<ul class=\"list-group list-group-flush no-border\">"
        //                        + "<li class=\"list-group-item no-border\"><em style = \"color:dimgray;\" > CityName:</em> " + garage["city"].ToString() + "</li>"
        //                        + "<li class=\"list-group-item no-border\"><em style = \"color:dimgray;\" > Website:</em> " + garage["website"].ToString() + "</li>"
        //                        + "<li class=\"list-group-item no-border\"><em style = \"color:dimgray;\" > Desecription:</em>" + garage["g_desc"].ToString() + "</li>"
        //                        + "</ul></div></div></div></div></div>";
        //        }

        //    }
        //    return Json(new { code = "200", data = HTML_Content }, JsonRequestBehavior.AllowGet);
        //}


        //public ActionResult Content()
        //{
        //    var t = Session["Page_Number"] ?? 1;
        //    int page_Number = Convert.ToInt32(t);

        //    //Data ConverSQLQueryPage(;
        //    return null;
        //}
        public PartialViewResult slider() 
        {
            return PartialView("../Home/_ItemSliderPartial", new ItemSliderModel());
        }
        public ActionResult AddDemoGarages()
        {

            return null;
        }

        //private DataTable GetGaragesTable(Pager p)
        //{
        //    //DataTable table;
        //    try
        //    {
        //        if (Request.QueryString.Count > 0)
        //        {
        //            Session["QueryString"] = Request.QueryString;
        //        }

        //        string sql_query = @" select  g.id gid, 
        //        		    g.name g_name, 
        //        		    g.description g_desc, 
        //        		    g.mobile mobile, 
        //        		    g.website website, 
        //                    g.created_at,
        //        		    I.url Img,
        //                    R.rate Rate,
        //        		    p.name city 
        //        		    from Garages G 
        //        		    left join Images I on g.id = i.referral_id 
        //        		    left join Addresses  A on g.address_id=a.id 
        //        		    left join Provinces  P on p.id=a.province_id 
        //        		    left join  (select Src_ID, avg(value) as rate from Rating GROUP BY src_id ) as R on g.id=R.src_id
        //                    ";
        //                    //order by g.created_at DESC 
        //        List<SqlParameter> li = new List<SqlParameter>();
        //        var data = Request.QueryString;
        //        string Factory = data["Factory"] ?? string.Empty;
        //        string Brand = data["Brand"] ?? string.Empty;
        //        string model = data["Model"] ?? string.Empty;
        //        string vehicle_type = data["vehicle_type"] ?? string.Empty;
        //        string country = data["country"] ?? string.Empty;
        //        string city = data["city"] ?? string.Empty;
        //        string Category = data["Category"] ?? string.Empty;
        //        string Rate = data["ratings"] ?? string.Empty;

        //        int page_number;
        //        // = (int)Session["current_page_number"];
        //        if (data["page_number"] != null)
        //            page_number = Convert.ToInt32(data["page_number"]);
        //        //where a.province_id in (select id from Provinces where country_id =@countryid)
        //        bool flag = true;
        //        if (Factory != String.Empty && Factory != "-1") // will remove
        //        {
        //            if (flag)
        //            {
        //                sql_query += " where g.id in (select garage_id from Garages_Brands gb inner join Brands b on b.id= gb.brand_id where country_id like @factoryid) ";
        //                flag = false;
        //            }
        //            else
        //            {
        //                sql_query += " and g.id in (select garage_id from Garages_Brands gb inner join Brands b on b.id= gb.brand_id where country_id like @factoryid) ";
        //            }
        //            li.Add(new SqlParameter("@factoryid", Factory));
        //        }
        //        if (Brand != String.Empty && Brand != "-1")
        //        {
        //            if (flag)
        //            {
        //                sql_query += " where g.id in (select garage_id from Garages_Brands where brand_id = @gb)";
        //                flag = false;
        //            }
        //            else
        //            {
        //                sql_query += " and g.id in (select garage_id from Garages_Brands where brand_id = @gb)";
        //            }
        //            li.Add(new SqlParameter("@gb", Brand));
        //        }
        //        if (model != String.Empty && model != "-1")
        //        {
        //            if (flag)
        //            {
        //                sql_query += " where g.id in(select garage_id from Garages_Brands gb inner join Models M on gb.brand_id= m.brand_id where  m.id like @mid) ";
        //                flag = false;
        //            }
        //            else
        //            {
        //                sql_query += " and g.id in(select garage_id from Garages_Brands gb inner join Models M on gb.brand_id= m.brand_id where  m.id like @mid) ";
        //            }
        //            li.Add(new SqlParameter("@mid", model));
        //        }
        //        if (vehicle_type != String.Empty && vehicle_type != "-1")
        //        {
        //            if (flag)
        //            {
        //                sql_query += " where g.id in (select garage_id from Vehicle_Types_Garages where vehicle_type_id like @vtg) ";
        //                flag = false;
        //            }
        //            else
        //            {
        //                sql_query += " and g.id in (select garage_id from Vehicle_Types_Garages where vehicle_type_id like @vtg) ";
        //            }
        //            li.Add(new SqlParameter("@vtg", vehicle_type));
        //        }
        //        if (Category != String.Empty && Category != "-1")
        //        {
        //            if (flag)
        //            {
        //                sql_query += " where g.id in(select garage_id from Garages_Categories where category_id like @gc ) ";
        //                flag = false;
        //            }
        //            else
        //            {
        //                sql_query += " and g.id in(select garage_id from Garages_Categories where category_id like @gc ) ";
        //            }
        //            li.Add(new SqlParameter("@gc", Category));
        //        }
        //        if (country != String.Empty && country != "-1")
        //        {
        //            if (flag)
        //            {
        //                sql_query += " where  a.province_id in (select id from Provinces where country_id =@countryid) ";
        //                flag = false;
        //            }
        //            else
        //            {
        //                sql_query += " and a.province_id in (select id from Provinces where country_id =@countryid) ";
        //            }
        //            li.Add(new SqlParameter("@countryid", country));
        //        } // country is market
        //        if (city != String.Empty && city != "-1")
        //        {
        //            if (flag)
        //            {
        //                sql_query += " where  p.id like @city";
        //                flag = false;
        //            }
        //            else
        //            {
        //                sql_query += " and p.id like @city";
        //            }
        //            li.Add(new SqlParameter("@city", city));
        //        }
        //        if (Rate != String.Empty && Rate != "-1")
        //        {
        //            if (flag)
        //            {
        //                sql_query += " where  Rate >= @Rate  ";
        //                flag = false;
        //            }
        //            else
        //            {
        //                sql_query += " and Rate >= @Rate";
        //            }
        //            li.Add(new SqlParameter("@Rate", Rate));
        //        }
        //        //Session["li"] = li;
        //        //legarage.Classes.Tools.SaveError(msg);

        //        return ConverSQLQueryPage(sql_query, li, "created_at", p.CurrentPage, p.PagesSize, out string msg, out int itemCount) ;



        //        //return ReadTableByQuery(sql_query, li, out string msg);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    //garages = ReadTableByQuery(sql_query, li, out msg);
        //}

        private List<GaragesModel> GetGaragesList()
        {
            string sql_query = @" select  g.id gid, 
                		g.name g_name, 
                		g.description g_desc, 
                		g.mobile mobile, 
                		g.website website, 
                        g.created_at,
                		I.url Img,
                        R.rate Rate,
                		p.name city 
                		from Garages G 
                		left join Images I on g.id = i.referral_id 
                		left join Addresses  A on g.address_id=a.id 
                		left join Provinces  P on p.id=a.province_id 
                		left join  (select Src_ID, avg(value) as rate from Rating GROUP BY src_id ) as R on g.id=R.src_id
                        ";
            //order by g.created_at DESC 
            List<SqlParameter> li = new List<SqlParameter>();
            var data = Request.QueryString;
            if (data["Filter"] == null || data["Filter"] != "1")
            {
                if (Session["data"] != null) data = (NameValueCollection)Session["data"];
            }
            Session["data"] = data;
            int page_number = 1;
            page_number =(int) Session["current_page"];

            string Factory = data["Factory"] ?? string.Empty;
            string Brand = data["Brand"] ?? string.Empty;
            string model = data["Model"] ?? string.Empty;
            string vehicle_type = data["vehicle_type"] ?? string.Empty;
            string country = data["country"] ?? string.Empty;
            string city = data["city"] ?? string.Empty;
            string Category = data["Category"] ?? string.Empty;
            string Rate = data["ratings"] ?? string.Empty;
            //where a.province_id in (select id from Provinces where country_id =@countryid)
            bool flag = true;
            if (Factory != String.Empty && Factory != "-1") // will remove
            {
                if (flag)
                {
                    sql_query += " where g.id in (select garage_id from Garages_Brands gb inner join Brands b on b.id= gb.brand_id where country_id like @factoryid) ";
                    flag = false;
                }
                else
                {
                    sql_query += " and g.id in (select garage_id from Garages_Brands gb inner join Brands b on b.id= gb.brand_id where country_id like @factoryid) ";
                }
                li.Add(new SqlParameter("@factoryid", Factory));
            }
            if (Brand != String.Empty && Brand != "-1")
            {
                if (flag)
                {
                    sql_query += " where g.id in (select garage_id from Garages_Brands where brand_id = @gb)";
                    flag = false;
                }
                else
                {
                    sql_query += " and g.id in (select garage_id from Garages_Brands where brand_id = @gb)";
                }
                li.Add(new SqlParameter("@gb", Brand));
            }
            if (model != String.Empty && model != "-1")
            {
                if (flag)
                {
                    sql_query += " where g.id in(select garage_id from Garages_Brands gb inner join Models M on gb.brand_id= m.brand_id where  m.id like @mid) ";
                    flag = false;
                }
                else
                {
                    sql_query += " and g.id in(select garage_id from Garages_Brands gb inner join Models M on gb.brand_id= m.brand_id where  m.id like @mid) ";
                }
                li.Add(new SqlParameter("@mid", model));
            }
            if (vehicle_type != String.Empty && vehicle_type != "-1")
            {
                if (flag)
                {
                    sql_query += " where g.id in (select garage_id from Vehicle_Types_Garages where vehicle_type_id like @vtg) ";
                    flag = false;
                }
                else
                {
                    sql_query += " and g.id in (select garage_id from Vehicle_Types_Garages where vehicle_type_id like @vtg) ";
                }
                li.Add(new SqlParameter("@vtg", vehicle_type));
            }
            if (Category != String.Empty && Category != "-1")
            {
                if (flag)
                {
                    sql_query += " where g.id in(select garage_id from Garages_Categories where category_id like @gc ) ";
                    flag = false;
                }
                else
                {
                    sql_query += " and g.id in(select garage_id from Garages_Categories where category_id like @gc ) ";
                }
                li.Add(new SqlParameter("@gc", Category));
            }
            if (country != String.Empty && country != "-1")
            {
                if (flag)
                {
                    sql_query += " where  a.province_id in (select id from Provinces where country_id =@countryid) ";
                    flag = false;
                }
                else
                {
                    sql_query += " and a.province_id in (select id from Provinces where country_id =@countryid) ";
                }
                li.Add(new SqlParameter("@countryid", country));
            } // country is market
            if (city != String.Empty && city != "-1")
            {
                if (flag)
                {
                    sql_query += " where  p.id like @city";
                    flag = false;
                }
                else
                {
                    sql_query += " and p.id like @city";
                }
                li.Add(new SqlParameter("@city", city));
            }
            if (Rate != String.Empty && Rate != "-1")
            {
                if (flag)
                {
                    sql_query += " where  Rate >= @Rate  ";
                    flag = false;
                }
                else
                {
                    sql_query += " and Rate >= @Rate";
                }
                li.Add(new SqlParameter("@Rate", Rate));
            }
            List<GaragesModel> garageslist = new List<GaragesModel>();
            // = (int)Session["current_page_number"];
            //if (data["page_number"] != null)
            //    page_number = Convert.ToInt32(data["page_number"]);
            //DataTable garages = ConverSQLQueryPage(sql_query, li, "created_at", p.CurrentPage, p.PagesSize, out string msg, out int itemCount);
            DataTable garages = ConverSQLQueryPage(sql_query, li, "created_at", page_number, 4, out string msg, out int itemCount);
            if (garages != null && garages.Rows.Count > 0)
            {
                foreach (DataRow garage in garages.Rows)
                {
                    float f = 0f;
                    float.TryParse(garage["Rate"].ToString(), out f);
                    garageslist.Add(new GaragesModel()
                    {
                        Address = new AddressModel() { Province = garage["city"].ToString() },
                        ID = new Guid(garage["gid"].ToString()),
                        Image = new ImagesModel() { URL = garage["Img"].ToString() },
                        Name = garage["g_name"].ToString(),
                        Mobile = garage["mobile"].ToString(),
                        Description = garage["g_desc"].ToString(),
                        Website = garage["website"].ToString(),
                        Rate = f
                    });
                }
            }
            int pages_count = (int)Math.Ceiling(((decimal)itemCount / (decimal)4));
            Session["pages_count"] = pages_count;
            return garageslist;
            
         }
        bool ISValid(GaragesModel garage, out string msg)
        {
            bool flag = true;
            if (garage.Name == "")
            {
                msg = Resources.CP_Garages.EnterNamePlease;
                return false;
            }
            if (garage.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP_Garages.EnterCityPlease;
                return false;
            }
            if (garage.Address.AddressName == "")
            {
                msg = Resources.CP_Garages.EnterAddressPlease;
                return false;
            }
            if (garage.Mobile == null)
            {
                msg = Resources.CP_Garages.EnterPhonePlease;
                return false;
            }
            if (Request.Params["vehicletypes"] == null)
            {
                msg = Resources.CP_Garages.EnterVehiclePlease;
                return false;
            }
            if (Request.Params["brands"] == null)
            {
                msg = Resources.CP_Garages.EnterBrandPlease;
                return false;
            }
            if (Request.Params["services"] == null)
            {
                msg = Resources.CP_Garages.EnterSerivesPlease;
                return false;
            }
            if (garage.Whatsapp == "")
            {
                msg = Resources.CP_Garages.EnterWhatsappPlease;
                return false;
            }
            if (garage.Keywords == "")
            {
                msg = Resources.CP_Garages.EnterKeywordsPlease;
                return false;
            }
            if (garage.Description == "")
            {
                msg = Resources.CP_Garages.EnterDescrptionPlease;
                return false;
            }
            if (Session["Attachment"] == null )
            {
                msg = Resources.CP_Garages.EnterImagePlease;
                return false;
            }
            msg = "";
            return flag;
        }


    }
}