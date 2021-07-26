using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.WebPages;

namespace legarage.Controllers
{
    public class VehiclesController : BaseController
    {
        // GET: Vehicles

        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = null, Adding = "/Vehicles/AddVehicles/" });
        }

        [HttpPost]
        public JsonResult AddVehicles()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            VehiclesModel new_vehicle = new VehiclesModel();
            new_vehicle.Mobile = Request.Params["phoneno"].ToString() != string.Empty ? Request.Params["phone_key"] + " " + Request.Params["phoneno"] : null;
            new_vehicle.Description = Request.Params["description"];
            new_vehicle.WhatsApp = Request.Params["Whatsapp"];
            new_vehicle.Price = Request.Params["price"];
            new_vehicle.Keywords = Request.Params["keywords"];
            new_vehicle.OwnerName = Request.Params["owner_name"];
            new_vehicle.Year = Request.Params["year"];
            new_vehicle.Mieage = Request.Params["mieage"];
            new_vehicle.Gearbox = Request.Params["gearbox"];
            new_vehicle.FuelType = Request.Params["fuel_type"];
            new_vehicle.EngineSize = Request.Params["engine_size"];
            new_vehicle.Color = Request.Params["color"];
            new_vehicle.Title = Request.Params["title"];
            new_vehicle.IsNew = Convert.ToInt32(Request.Params["is_new"]);
            new_vehicle.User = new UsersModel();
            new_vehicle.User.ID = new Guid(Session["id"].ToString());
            if (Request.Params["quantity"] != "")
            {
                new_vehicle.Quantity = Request.Params["quantity"].AsInt().ToString() != null ? Request.Params["quantity"].AsInt().ToString() : "";
            }
            else
            {
                new_vehicle.Quantity = "";
            }
            if (Request.Params["model"] != null)
            {
                ModelsModel model = new ModelsModel();
                model.ID = new Guid(Request.Params["model"].ToString());
                new_vehicle.Model = model;
            }
            else
            {
                new_vehicle.Model = null;
            }
            if (Request.Params["vehicletype"] != null)
            {
                VehicleTypesModel vehicleType = new VehicleTypesModel();
                vehicleType.ID = new Guid(Request.Params["vehicletype"].ToString());
                new_vehicle.VehicleType = vehicleType;
            }
            else
            {
                new_vehicle.VehicleType = null;
            }

            new_vehicle.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                new_vehicle.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                new_vehicle.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                new_vehicle.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                new_vehicle.Address.ProvinceId = new Guid();
            }
            if (ISValid(new_vehicle, out msg))
            {
                Guid id = Guid.NewGuid();
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "whatsapp", "user_id", "model_id", "vehicle_type_id", "owner_name", "keywords", "mobile", "is_new", "quantity", "price", "description", "year", "mieage", "gearbox", "fuel_type", "engine_size", "color", "title", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_vehicle.WhatsApp, new_vehicle.User.ID, new_vehicle.Model.ID, new_vehicle.VehicleType.ID, new_vehicle.OwnerName, new_vehicle.Keywords, new_vehicle.Mobile, new_vehicle.IsNew, new_vehicle.Quantity, new_vehicle.Price, new_vehicle.Description, new_vehicle.Year, new_vehicle.Mieage, new_vehicle.Gearbox, new_vehicle.FuelType, new_vehicle.EngineSize, new_vehicle.Color, new_vehicle.Title, DateTime.Now, };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Vehicles", id, cols, vals, out errMessage))
                {
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { new_vehicle.Address.ProvinceId, new_vehicle.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Vehicles", id, cols, vals, out errMessage);

                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Vehicles/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { ImageName, id, "Vehicles", DateTime.Now, 1 };
                        vals.AddRange(valsinput);
                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    msg = "sucsess";
                    return Json(new { code = code.ToString(), msg = Resources.CP.Added });
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



        public ActionResult Index()
        {
            Session["data"] = null;
            Session["current_page"] = 1;
            VehiclesIndexModel vIndex = new VehiclesIndexModel();
            vIndex.Vehicles = GetVehiclesList();

            //GET: Vehicles Cards 
//            DataTable VehiclesTable = new DataTable();
//            string SQLVehicles = " SELECT v.id as id "
//                                + "      , description "
//                                + "      , price "
//                                + "      , store_id "
//                                + "      , vehicle_type_id "
//                                + "      , is_new "
//                                + "      , mobile "
//                                + "      , quantity "
//                                + "      , address_id "
//                                + "      , title "
//                                + "      , c.name as city "
//                                + "      , I.url as url "
//                                + "   FROM  Vehicles as v "
//                                + "  inner join Addresses as a on a.id = v.address_id "
//                                + "  inner join Provinces as c on c.id = a.province_id "
//                                + "	inner join Images as I on I.referral_id = v.id  ";
//;
//            VehiclesTable = Database.ReadTableByQuery(SQLVehicles, null, out string msj);


//            if (VehiclesTable != null && VehiclesTable.Rows.Count > 0)
//            {
//                foreach (DataRow vehicleRow in VehiclesTable.Rows)
//                {
//                    vIndex.Vehicles.Add(new VehiclesModel()
//                    {
//                        ID = new Guid(vehicleRow["id"].ToString()),
//                        Title = vehicleRow["title"].ToString(),
//                        Address = new AddressModel() {Province = vehicleRow["city"].ToString() },
//                        Quantity = vehicleRow["quantity"].ToString(),
//                        Price = vehicleRow["price"].ToString(),
//                        IsNew = Convert.ToInt32(vehicleRow["is_new"]),
//                        VehicleType = new VehicleTypesModel(),
//                        Model = new ModelsModel(),
//                        URL = new URLModel() { },
//                        Image = new ImagesModel() { URL = vehicleRow["url"].ToString()}, 
//                    }) ;
//                }
//            }

            // GET: Filters Brands
            vIndex.Brands = new List<BrandsModel>();
            DataTable BrandsTable = new DataTable();
            string sqlquery = "SELECT id, name  FROM Brands;";

            BrandsTable = Database.ReadTableByQuery(sqlquery, null, out string msj);

            if (BrandsTable != null && BrandsTable.Rows.Count > 0)
            {
                foreach (DataRow Brand in BrandsTable.Rows)
                {
                    vIndex.Brands.Add(new BrandsModel() { ID= new Guid(Brand["id"].ToString()), Name= Brand["name"].ToString() });
                }
            }

            vIndex.Countries = new List<CountriesModel>();
            DataTable CountryTable = new DataTable();
            sqlquery = "SELECT id, name  FROM Countries;";
            CountryTable = Database.ReadTableByQuery(sqlquery, null, out msj);

            if (CountryTable != null && CountryTable.Rows.Count > 0)
            {
                foreach (DataRow Country in CountryTable.Rows)
                {
                    vIndex.Countries.Add(new CountriesModel() { ID=new Guid(Country["id"].ToString()), Name= Country["name"].ToString() });
                }
            }

            vIndex.VehicleTypes = new List<VehicleTypesModel>();
            DataTable vehiclesTable = new DataTable();
            sqlquery = "SELECT id, type_name FROM Vehicle_Types;";
            vehiclesTable = Database.ReadTableByQuery(sqlquery, null, out msj);
            if (vehiclesTable != null && vehiclesTable.Rows.Count > 0)
            {
                foreach (DataRow VT in vehiclesTable.Rows)
                {
                    vIndex.VehicleTypes.Add(new VehicleTypesModel() { ID = new Guid(VT["id"].ToString()), Name= VT["type_name"].ToString()});
                }
            }

            vIndex.Cities = new List<CitiesModel>();
            DataTable CityTable = new DataTable();
            sqlquery = "SELECT id, name  FROM Provinces;";
            CityTable = Database.ReadTableByQuery(sqlquery, null, out msj);

            if (CityTable != null && CityTable.Rows.Count > 0)
            {
                foreach (DataRow c in CityTable.Rows)
                {
                    vIndex.Cities.Add(new CitiesModel() {ID=new Guid(c["id"].ToString()), Name=c["name"].ToString()});
                }
            }

            vIndex.Models = new List<ModelsModel>();
            DataTable ModelsTable = new DataTable();
            sqlquery = "SELECT id, name  FROM Models;";
            ModelsTable = Database.ReadTableByQuery(sqlquery, null, out msj);
            if (ModelsTable != null && ModelsTable.Rows.Count > 0)
            {
                foreach (DataRow mdl in ModelsTable.Rows)
                {
                    vIndex.Models.Add(new ModelsModel() {ID =new Guid(mdl["id"].ToString()), Name = mdl["name"].ToString() });
                }
            }

            return View(vIndex);
        }

        [HttpGet]
        public ActionResult GetVehicles(int page =1)
        {
            List<VehiclesModel> Vehicles = GetVehiclesList();
            Session["current_page"] = page;
            return PartialView("Content", Vehicles);
        }

        private List<VehiclesModel> GetVehiclesList()
        {
            List<VehiclesModel> Vehicles = new List<VehiclesModel>();
            DataTable VehiclesTable = new DataTable();
            List<SqlParameter> li = new List<SqlParameter>();
            var data = Request.QueryString;
            if (data["Filter"] == null || data["Filter"] != "1")
            {
                if (Session["data"] != null) data = (NameValueCollection)Session["data"];
            }
            Session["data"] = data;
            int page_number = 1;
            page_number = (int)Session["current_page"];


            string vehicle_type = data["vehicle-types"] ?? string.Empty;
            string Factory = data["Factory"] ?? string.Empty;
            string Brand = data["Brand"] ?? string.Empty;
            string model = data["Model"] ?? string.Empty;
            string vehicle_size = data["sizes"] ?? string.Empty;
            string country = data["country"] ?? string.Empty;
            string city = data["city"] ?? string.Empty;
            string mileage = data["mileage"] ?? string.Empty;
            string gears = data["gears"] ?? string.Empty;
            string fuels = data["fuels"] ?? string.Empty;
            string engine_size = data["engine-size"] ?? string.Empty;
            string colors = data["colors"] ?? string.Empty;
            string ratings = data["ratings"] ?? string.Empty;
            string statuses = data["statuses"] ?? string.Empty;
            string minrange = data["minrange"] ?? string.Empty;
            string maxrange = data["maxrange"] ?? string.Empty;
            string vehicle_type4 = data["vehicle-types"] ?? string.Empty;
            string SQLVehicles = @" SELECT v.id as id 
                                      , description 
                                      , price 
                                      , RT.rate Rate
                                      , store_id 
                                      , vehicle_type_id 
                                      , is_new 
                                      , mobile 
                                      , v.quantity 
                                      , v.address_id 
                                      , v.title 
                                      , v.created_at
                                      , c.name as city 
                                      , I.url as url 
                                   FROM  Vehicles as v 
                                  inner join Addresses as a on a.id = v.address_id 
                                  inner join Provinces as c on c.id = a.province_id 
                                  inner join Images as I on I.referral_id = v.id  
                                  left join  (select Src_ID, avg(value) as rate from Rating GROUP BY src_id ) as RT on V.id=RT.src_id ";
            //string area =  Request.Params["area"] ?? string.Empty;

            bool flag = true;
            if (vehicle_type != String.Empty && vehicle_type != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where vehicle_type_id  like @vehicle_type_id ";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and  vehicle_type_id  like @vehicle_type_id  ";
                }
                li.Add(new SqlParameter("@vehicle_type_id", vehicle_type));
            }
            if (Factory != String.Empty && Factory != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where model_id in (select id from Models m inner join Brands  b on b.id= m.brand_id where b.country_id like @factoryid) ";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and  model_id in (select id from Models m inner join Brands  b on b.id= m.brand_id where b.country_id like @factoryid) ";
                }
                li.Add(new SqlParameter("@factoryid", Factory));
            }
            if (Brand != String.Empty && Brand != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where model_id in (select id from Models where brand_id = @brand) ";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and model_id in (select id from Models where brand_id = @brand) ";
                }
                li.Add(new SqlParameter("@brand", Brand));
            }
            if (model != String.Empty && model != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where model_id like @mid ";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and  model_id like @mid  ";
                }
                li.Add(new SqlParameter("@mid", model));
            }
            if (vehicle_size != String.Empty && vehicle_size != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where vehicle_type_id  like @vt ";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and vehicle_type_id  like @vt ";
                }
                li.Add(new SqlParameter("@vt", vehicle_size));
            }
            if (country != String.Empty && country != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  a.province_id in (select id from Provinces where country_id =@countryid) ";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and a.province_id in (select id from Provinces where country_id =@countryid) ";
                }
                li.Add(new SqlParameter("@countryid", country));
            } // country is market
            if (city != String.Empty && city != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  a.province_id  like @city";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and a.province_id  like @city";
                }
                li.Add(new SqlParameter("@city", city));
            }
            if (mileage != String.Empty && mileage != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  mieage  like @mileage";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and mieage  like @mileage";
                }
                li.Add(new SqlParameter("@mileage", mileage));
            }
            if (gears != String.Empty && gears != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  gearbox like @gears";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and gearbox  like @gears";
                }
                li.Add(new SqlParameter("@gears", gears));
            }
            if (fuels != String.Empty && fuels != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  fuel_type like @fuels";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and fuel_type like @fuels";
                }
                li.Add(new SqlParameter("@fuels", fuels));
            }
            if (engine_size != String.Empty && engine_size != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  engine_size like @engine_size";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and engine_size like @engine_size";
                }
                li.Add(new SqlParameter("@engine_size", engine_size));
            }
            if (colors != String.Empty && colors != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  color  like @colors";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and color  like @colors";
                }
                li.Add(new SqlParameter("@colors", "%" + colors + "%"));
            }
            if (ratings != String.Empty && ratings != "-1")
            {
                if (flag)
                {
                    SQLVehicles += "  ";
                    flag = false;
                }
                else
                {
                    SQLVehicles += "  ";
                }
                //li.Add(new SqlParameter("@ratings", ratings));
            }
            if (statuses != String.Empty && statuses != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  is_new  like @statuses";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and is_new  like @statuses";
                }
                li.Add(new SqlParameter("@statuses", statuses));
            }
            if (minrange != String.Empty && minrange != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  price  > @minrange";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and price  > @minrange";
                }
                li.Add(new SqlParameter("@minrange", minrange));
            }
            if (maxrange != String.Empty && maxrange != "-1")
            {
                if (flag)
                {
                    SQLVehicles += " where  price < @maxrange";
                    flag = false;
                }
                else
                {
                    SQLVehicles += " and price  < @maxrange";
                }
                li.Add(new SqlParameter("@maxrange", maxrange));
            }



            //VehiclesTable = Database.ReadTableByQuery(SQLVehicles, null, out  msj);
            VehiclesTable = Database.ConverSQLQueryPage(SQLVehicles, li, "created_at", page_number, 4, out string msj, out int itemCount);


            if (VehiclesTable != null && VehiclesTable.Rows.Count > 0)
            {
                foreach (DataRow vehicleRow in VehiclesTable.Rows)
                {
                    float f = 0f;
                    float.TryParse(vehicleRow["Rate"].ToString(), out f);
                    Vehicles.Add(new VehiclesModel()
                    {
                        ID = new Guid(vehicleRow["id"].ToString()),
                        Title = vehicleRow["title"].ToString(),
                        Address = new AddressModel() { Province = vehicleRow["city"].ToString() },
                        Quantity = vehicleRow["quantity"].ToString(),
                        Price = vehicleRow["price"].ToString(),
                        IsNew = Convert.ToInt32(vehicleRow["is_new"]),
                        VehicleType = new VehicleTypesModel(),
                        Model = new ModelsModel(),
                        URL = new URLModel() { },
                        Image = new ImagesModel() { URL = vehicleRow["url"].ToString() },
                        Rate=f
                    });
                }
            }
            int pages_count = (int)Math.Ceiling(((decimal)itemCount / (decimal)4));
            Session["pages_count"] = pages_count;
            return Vehicles;
        }

        [Route("Vehicles/Details")]
        [Route("Vehicles/BuyingDetails")]
        public ActionResult BuyingDetails()
        {
            if (Request.QueryString["id"] != null)
            {
                VehiclesModel vehicles = new VehiclesModel();
                Guid id = new Guid(Request.QueryString["id"].ToString());
                DataTable details = new DataTable();
                DataTable getImages = new DataTable();
                DataTable getAddress = new DataTable();
                DataTable getServics = new DataTable();
                List<SqlParameter> prmtr = new List<SqlParameter>();
                string queryDetails = " SELECT v.description "
                                    + "      ,price "
                                    + "      ,model_id "
                                    + "      ,s.name as store "
                                    + "      ,vt.type_name as type_name "
                                    + "      ,is_new "
                                    + "      ,v.mobile "
                                    + "      ,quantity "
                                    + "      ,keywords "
                                    + "      ,owner_name "
                                    + "      ,year "
                                    + "      ,mieage "
                                    + "      ,gearbox "
                                    + "      ,engine_size "
                                    + "      ,color "
                                    + "      ,title "
                                    + "      ,v.whatsapp "
                                    + "      ,fuel_type "
                                    + "	  ,I.url as img "
                                    + "	  ,a.details as adres "
                                    + "	  ,PV.name as city "
                                    + "	  ,C.name as country "
                                    + "	  ,m.name as model "
                                    + "	  ,b.name as brand "
                                    + "  FROM Vehicles as v "
                                    + "	inner join Images as I on I.referral_id = v.id  "
                                    + "	inner join Addresses as A on A.id = v.address_id  "
                                    + "	inner join Provinces as PV on PV.id = A.province_id "
                                    + "	inner join Countries as C on c.id = pv.country_id  "
                                    + "	inner join Stores as s on s.id = v.store_id "
                                    + "	inner join Vehicle_Types as vt on vt.id=v.vehicle_type_id "
                                    + "	inner join Models as m on m.id = v.model_id "
                                    + "	inner join Brands as b on b.id = m.brand_id "
                                    + "	where v.id= @vid	 ";
                prmtr.Add(new SqlParameter("@vid", id));
                details = Database.ReadTableByQuery(queryDetails, prmtr, out string msg);
                if (details != null && details.Rows.Count > 0)
                {
                    DataRow winche_row = details.Rows[0];
                    vehicles.ID = new Guid(winche_row["id"].ToString());
                    vehicles.Address = new AddressModel() { AddressName = winche_row["adres"].ToString(), Province = winche_row["city"].ToString(), Country = winche_row["country"].ToString() };
                    vehicles.Description = winche_row["description"].ToString();
                    vehicles.Image = new ImagesModel() { URL = winche_row["img"].ToString() };
                    vehicles.Mobile = winche_row["mobile"].ToString();
                    vehicles.Title = winche_row["title"].ToString();
                    vehicles.Color = winche_row["color"].ToString();

                }
                prmtr = new List<SqlParameter>();
                string queryImages = "select I.url as url from Images as I inner join Vehicles as V on referral_id = @vid";
                prmtr.Add(new SqlParameter("@vid", id));
                getImages = Database.ReadTableByQuery(queryImages, prmtr, out msg);
                if (getImages != null && getImages.Rows.Count > 0)
                {
                    vehicles.Images = new List<ImagesModel>();
                    foreach (DataRow row in getImages.Rows)
                    {
                        vehicles.Images.Add(new ImagesModel() { URL = row["url"].ToString() });
                    }
                }
                return View(vehicles);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        //public PartialViewResult AddOffer()
        //{
        //    DataRow user;
        //    if (Tools.FindCurrentUser(out user))
        //    {
        //        string msg;
        //        string sql = @"SELECT [id]
        //                      ,[description]
        //                      ,[price]
        //                      ,[model_id]
        //                      ,[store_id]
        //                      ,[vehicle_type_id]
        //                      ,[is_new]
        //                      ,[created_at]
        //                      ,[updated_at]
        //                      ,[mobile]
        //                      ,[quantity]
        //                      ,[keywords]
        //                      ,[owner_name]
        //                      ,[address_id]
        //                      ,[user_id]
        //                      ,[year]
        //                      ,[mieage]
        //                      ,[gearbox]
        //                      ,[engine_size]
        //                      ,[color]
        //                      ,[title]
        //                      ,[fuel_type]
        //                  FROM [le-garage].[dbo].[Vehicles]";
        //        if (user["is_admin"].ToString() != "1") //if user not Admin
        //            sql += " WHERE [user_id]= @uid";

        //        List<SqlParameter> li = new List<SqlParameter>();
        //        li.Add(new SqlParameter("@uid", user["id"].ToString()));
        //        DataTable vehiclesTable = Database.ReadTableByQuery(sql, li, out msg);
        //        List<VehiclesModel> vehicleList = new List<VehiclesModel>();
        //        foreach (DataRow row in vehiclesTable.Rows)
        //        {
        //            vehicleList.Add(new VehiclesModel()
        //            {
        //                ID = new Guid(row["Id"].ToString()),
        //                Title = row["Title"].ToString(),
        //                Price = row["price"].ToString()
        //            });
        //        }

        //        return PartialView(vehicleList);
        //    }
        //    return null;
        //}

        //[HttpPost]
        //public JsonResult NewOffer()
        //{
        //    string msg;
        //    DataTable table = new DataTable();
        //    OffersModel model = new OffersModel();
        //    model.id = Guid.NewGuid();
        //    model.referal_id = Request.Params["select_vehicle"] != null? new Guid(Request.Params["select_vehicle"]): new Guid();
        //    //model.referal_id = new Guid(Request.Params["select_winches"] ?? (new Guid()).ToString());
        //    model.referal_type = Request.Params["Offer_type"] ?? "";
        //    model.name = Request.Params["name"] ?? "";
        //    model.Is_Active = Request.Params["Is_Active"] ?? "1";
        //    model.description = Request.Params["desc"] ?? "";
        //    model.discount_percentage = Convert.ToDouble((Request.Params["discount_percentage"]??"0").ToString());
        //    model.paymentmethods = Request.Params["paymentmethod"] ?? "";
        //    model.mobile = Request.Params["phonenum"] ?? "";
        //    model.website = Request.Params["site"] ?? "";
        //    model.address_id = new Guid(Request.Params["address_id"] ?? new Guid().ToString());
        //    model.start_date = (Request.Params["meeting-time-st"] ?? "").AsDateTime().ToString();
        //    model.end_date = (Request.Params["meeting-time-en"] ?? "").AsDateTime().ToString();
        //    if (true || ISOfferValid(model, out msg))
        //    {
        //        List<string> cols = new List<string>
        //        {
        //             "description"
        //              ,"referal_id"
        //              ,"referal_type"
        //              ,"start_date"
        //              ,"end_date"
        //              ,"discount_percentage"
        //              ,"created_at"
        //              ,"updated_at"
        //              ,"name"
        //              ,"paymentmethods"
        //              ,"address_id"
        //              ,"mobile"
        //              ,"website"
        //              ,"Is_Active"
        //        };

        //        List<Object> vals = new List<object>
        //        {
        //            model.description,
        //            model.referal_id,
        //            model.referal_type,
        //            model.start_date,
        //            model.end_date,
        //            model.discount_percentage,
        //            DateTime.Now,
        //            DateTime.Now ,
        //            model.name,
        //            model.paymentmethods,
        //            model.address_id,
        //            model.mobile,
        //            model.website,
        //            model.Is_Active,
        //        };
        //        string errMessage = string.Empty;
        //        if (vals.Count == cols.Count)
        //        {
        //            if (Database.InsertRow("Offers", Guid.NewGuid(), cols, vals, out errMessage))
        //            {
        //                return Json(new { code = 200.ToString(), msg = "sucsess" });
        //            }
        //            else
        //            {
        //                msg = "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
        //                return Json(new { code = 404.ToString(), msg = msg });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return Json(new { code = 404.ToString(), msg = msg });
        //    }
        //    return Json(new { code = "200" });
        //}

        bool ISValid(VehiclesModel Vehicle, out string msg)
        {
            bool flag = true;
            if (Vehicle.Title == "")
            {
                msg = Resources.CP.EnterTitle;
                return false;
            }
            if (Vehicle.OwnerName == "")
            {
                msg = Resources.CP.EnterOwnerName;
                return false;
            }
            if (Vehicle.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP.EnterCity;
                return false;
            }
            if (Vehicle.Address.AddressName == "")
            {
                msg = Resources.CP.EnterAddress;
                return false;
            }
            if (Vehicle.Mobile == null)
            {
                msg = Resources.CP.EnterPhone;
                return false;
            }
            if (Vehicle.WhatsApp == "")
            {
                msg = Resources.CP.EnterWhatsapp;
                return false;
            }
            if (Vehicle.VehicleType == null)
            {
                msg = Resources.CP.EnterVehicle;
                return false;
            }
            if (Vehicle.Model == null)
            {
                msg = Resources.CP.EnterModel;
                return false;
            }
            if (Vehicle.IsNew == -1)
            {
                msg = Resources.CP.EnterIsNew;
                return false;
            }
            if (Vehicle.Price == "")
            {
                msg = Resources.CP.EnterPrice;
                return false;
            }
            if (Vehicle.Quantity == "")
            {
                msg = Resources.CP.EnterQuantity;
                return false;
            }
            if (Vehicle.Year == "-1")
            {
                msg = Resources.CP.EnterYear;
                return false;
            }
            if (Vehicle.Mieage == "-1")
            {
                msg = Resources.CP.EnterMieage;
                return false;
            }
            if (Vehicle.Gearbox == "-1")
            {
                msg = Resources.CP.EnterGearBox;
                return false;
            }
            if (Vehicle.FuelType == "-1")
            {
                msg = Resources.CP.EnterFuelType;
                return false;
            }
            if (Vehicle.EngineSize == "")
            {
                msg = Resources.CP.EnterEngineSize;
                return false;
            }
            if (Vehicle.Color == "")
            {
                msg = Resources.CP.EnterColor;
                return false;
            }
            if (Vehicle.Keywords == "")
            {
                msg = Resources.CP.EnterKeywords;
                return false;
            }
            if (Vehicle.Description == "")
            {
                msg = Resources.CP.EnterDescrption;
                return false;
            }
            if (Session["Attachment"] == null)
            {
                msg = Resources.CP.AddImage;
                return false;
            }

            msg = "";
            return flag;
        }
        bool ISOfferValid(OffersModel offer, out string msg)
        {
            //TODO: add resources to error msgs
            bool flag = true;
            if (offer.name == "")
            {
                msg = "wrong in offer name";
                return false;
            }
            if (offer.referal_type == "")
            {
                msg = "wrong in offer referal_type";
                return false;
            }

            if (offer.description == "")
            {
                msg = "wrong in offer description";
                return false;
            }
            //if (offer.mobile == "")
            //{
            //    msg = "wrong in offer phone";
            //    //return false;
            //}
            msg = "";
            return flag;
        }

    }
}