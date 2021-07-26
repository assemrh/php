
using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using static legarage.Classes.Database;

namespace legarage.Controllers
{
    //[Route("Products/[Action]")]
    public class PartsController : BaseController
    {
        // GET: Part
        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = null, Adding = "/Parts/AddParts/" });
        }

        [HttpPost]
        public JsonResult AddParts()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            ProductsModel new_product = new ProductsModel();
            new_product.Title = Request.Params["Title"] != null ? Request.Params["Title"] : "";
            new_product.Description = Request.Params["Description"] != null ? Request.Params["Description"] : "";
            new_product.IsNew = Convert.ToInt32(Request.Params["is_new"]);
            new_product.Price = Request.Params["price"] != null ? Request.Params["price"] : "";
            new_product.WhatsApp = Request.Params["whatsapp"] != null ? Request.Params["whatsapp"] : "";
            new_product.Keywords = Request.Params["keywords"] != null ? Request.Params["keywords"] : "";
            new_product.OwnerName = Request.Params["owner_name"] != null ? Request.Params["owner_name"] : "";
            new_product.Year = Request.Params["Year"] != null ? Request.Params["Year"] : "";
            new_product.Mieage = Request.Params["mieage"] != null ? Request.Params["mieage"] : "";
            new_product.GearBox = Request.Params["gearbox"] != null ? Request.Params["gearbox"] : "";
            new_product.EngineSize = Request.Params["engine_size"] != null ? Request.Params["engine_size"] : "";
            new_product.Color = Request.Params["Color"] != null ? Request.Params["Color"] : "";
            new_product.FuelType = Request.Params["fuel_type"] != null ? Request.Params["fuel_type"] : "";
            new_product.Mobile = Request.Params["phoneno"]?.ToString() != string.Empty ? Request.Params["phone_key"] + " " + Request.Params["phoneno"] : null;
            new_product.Quantity = Request.Params["quantity"].AsInt().ToString() != null ? Request.Params["quantity"].AsInt().ToString() : "";
            new_product.User = new UsersModel();
            new_product.User.ID = new Guid(Session["id"]?.ToString());
            if (Request.Params["model"] != null)
            {
                ModelsModel model = new ModelsModel();
                model.ID = new Guid(Request.Params["model"]?.ToString());
                new_product.Model = model;
            }
            else
            {
                new_product.Model = null;
            }
            if (Request.Params["brand"] != null)
            {
                BrandsModel brand = new BrandsModel();
                brand.ID = new Guid(Request.Params["brand"]?.ToString());
                new_product.Brand = brand;
            }
            else
            {
                new_product.Model = null;
            }
            if (Request.Params["vehicletype"] != null)
            {
                VehicleTypesModel vehicleType = new VehicleTypesModel();
                vehicleType.ID = new Guid(Request.Params["vehicletype"]?.ToString());
                new_product.VehicleType = vehicleType;
            }
            else
            {
                new_product.VehicleType = null;
            }
            new_product.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                new_product.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                new_product.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"]?.ToString() != "-1")
            {
                new_product.Address.ProvinceId = new Guid(Request.Params["City"]?.ToString());
            }
            else
            {
                new_product.Address.ProvinceId = new Guid();
            }

            if (ISValid(new_product, out msg))
            {
                Guid id = Guid.NewGuid();
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "whatsapp", "fuel_type", "color", "engine_size", "gearbox", "mieage", "year", "brand_id", "user_id", "owner_name", "title", "mobile", "model_id", "vehicle_type_id", "is_new", "quantity", "price", "description", "keywords", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_product.WhatsApp, new_product.FuelType, new_product.Color, new_product.EngineSize, new_product.GearBox, new_product.Mieage, new_product.Year, new_product.Brand.ID, new_product.User.ID, new_product.OwnerName, new_product.Title, new_product.Mobile, new_product.Model.ID, new_product.VehicleType.ID, new_product.IsNew, new_product.Quantity, new_product.Price, new_product.Description, new_product.Keywords, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Products", id, cols, vals, out errMessage))
                {
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { new_product.Address.ProvinceId, new_product.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Products", id, cols, vals, out errMessage);
                    var ss = Request.Params["services"].Split(',');
                    List<string> services = ss.ToList();
                    foreach (string service in services)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "product_id", "category_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, service, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Products_Categories", Guid.NewGuid(), cols, vals, out errMessage);
                    }
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Products/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { ImageName, id, "Products", DateTime.Now, 1 };
                        vals.AddRange(valsinput);
                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
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

        //[HttpPost]
        //public JsonResult FilterResult()//
        //{
        //    DataTable garages = new DataTable();
        //    List<SqlParameter> li = new List<SqlParameter>();

        //    string Factory = Request.Params["Factory"] ?? string.Empty;
        //    string Brand = Request.Params["Brand"] ?? string.Empty;
        //    string model = Request.Params["Model"] ?? string.Empty;
        //    string vehicle = Request.Params["vehicle_type"] ?? string.Empty;
        //    string country = Request.Params["country"] ?? string.Empty;
        //    string city = Request.Params["city"] ?? string.Empty;//categories1
        //    string categories1 = Request.Params["categories1"] ?? string.Empty;

        //    List<string> Categories = new List<string>();
        //    int i = 0;
        //    do
        //    {
        //        Categories.Add(Request.Params["Categories" + i++.ToString()] ?? string.Empty);
        //    } while (Request.Params["Categories" + i.ToString()] != null);

        //    string status = Request.Params["status"] ?? string.Empty;
        //    string Pirce = Request.Params["Categories"] ?? string.Empty;
        //    List<string> HTML_Garages = new List<string>();
        //    string sql_query = " SELECT p.id AS id, " +
        //                            "       p.price AS price ,   " +
        //                            "       p.quantity AS quantity,  " +
        //                            "       p.title AS title ,    " +
        //                            "       p.is_new AS State,   " +
        //                            "       I.url AS Img ,   " +
        //                            "		v.type_name AS vehicle_type ,  " +
        //                            "		B.name AS brand ," +
        //                            "		PV.name  AS city ," +
        //                            "		C.name AS cate , " +
        //                            "       M.name as model  " +
        //                            "       FROM Products AS P " +
        //                            "       inner join Images AS I on p.id = i.referral_id  " +
        //                            "		INNER JOIN Models AS M ON M.id = P.model_id " +
        //                            "		INNER JOIN Brands AS B ON B.id = M.brand_id " +
        //                            "		INNER JOIN Vehicle_Types AS V ON V.id = M.vehicle_type_id " +
        //                            "		INNER JOIN Addresses AS A ON A.id = P.address_id " +
        //                            "		INNER JOIN Provinces AS PV ON PV.id = A.province_id ";
        //    bool flag = true;
        //    if (Factory != String.Empty && Factory != "-1")
        //    {
        //        if (flag)
        //        {
        //            sql_query += " where brand_id in (select id from Garages_Brands gb where country_id like @factoryid) ";
        //            flag = false;
        //        }
        //        else
        //        {
        //            sql_query += " and  brand_id in (select id from Garages_Brands gb where country_id like @factoryid)  ";
        //        }
        //        li.Add(new SqlParameter("@factoryid", Factory));
        //    }
        //    if (model != String.Empty && model != "-1")
        //    {
        //        if (flag)
        //        {
        //            sql_query += " where model_id like @model_id ";
        //            flag = false;
        //        }
        //        else
        //        {
        //            sql_query += " and model_id like @model_id  ";
        //        }
        //        li.Add(new SqlParameter("@brandid", model));
        //    }
        //    if (Brand != String.Empty && Brand != "-1")
        //    {
        //        if (flag)
        //        {
        //            sql_query += " where brand_id like @brandid ";
        //            flag = false;
        //        }
        //        else
        //        {
        //            sql_query += " and  brand_id like @brandid ";
        //        }
        //        li.Add(new SqlParameter("@brandid", Brand));
        //    }
        //    if (vehicle != String.Empty && vehicle != "-1")
        //    {
        //        if (flag)
        //        {
        //            sql_query += " where vehicle_type_id like @vehicle ";
        //            flag = false;
        //        }
        //        else
        //        {
        //            sql_query += " and  vehicle_type_id like @vehicle ";
        //        }
        //        li.Add(new SqlParameter("@vehicle", vehicle));
        //    }
        //    if (country != String.Empty && country != "-1")
        //    {
        //        if (flag)
        //        {
        //            sql_query += " where address_id in ( select id from Addresses ad inner join Provinces pr on ad.province_id=pr.id  where country_id like @country) ";
        //            flag = false;
        //        }
        //        else
        //        {
        //            sql_query += " and  address_id in ( select id from Addresses ad inner join Provinces pr on ad.province_id=pr.id  where country_id like @country)  ";
        //        }
        //        li.Add(new SqlParameter("@country", country));
        //    }

        //    if (city != String.Empty && city != "-1")
        //    {
        //        if (flag)
        //        {
        //            sql_query += " where address_id in ( select id from Addresses province_id like @city) ";
        //            flag = false;
        //        }
        //        else
        //        {
        //            sql_query += " and  address_id in ( select id from Addresses province_id like @city)   ";
        //        }
        //        li.Add(new SqlParameter("@city", city));
        //    }

        //    List<ProductsModel> part = new List<ProductsModel>();
        //    garages = Database.ReadTableByQuery(sql_query, li, out string msg);
        //    if (garages != null && garages.Rows.Count > 0)
        //    {
        //        foreach (DataRow Product in garages.Rows)
        //        {
        //            part.Add(new ProductsModel()
        //            {
        //                ID = new Guid(Product["id"]?.ToString()),
        //                Title = Product["title"]?.ToString(),
        //                IsNew = Convert.ToInt32(Product["State"]),
        //                Price = Product["price"]?.ToString(),
        //                Address = new AddressModel() { Province = Product["city"]?.ToString() },
        //                VehicleType = new VehicleTypesModel() { Name = Product["vehicle_type"]?.ToString() },
        //                Image = new ImagesModel() { URL = Product["Img"]?.ToString() },
        //                Model = new ModelsModel() { Name = Product["model"]?.ToString() },
        //                Brand = new BrandsModel() { Name = Product["brand"]?.ToString() }

        //            });


        //        }

        //    }
        //    //string sJSONResponse = JsonConvert.SerializeObject(Garages);
        //    string jsonObj = JsonConvert.SerializeObject(part);

        //    return Json(new { code = "200", data = jsonObj, });
        //}


        [HttpGet]
        public ActionResult Get_Parts(int page = 1)//
        {
            Session["current_page"] = page;
            List<ProductsModel> partModelList = getProductsList();
            return PartialView("Content", partModelList);
        }

        [Route("Products")]
        [Route("Parts")]
        [Route("Parts/Index")]
        public ActionResult Index()
        {
            Session["data"] = null;
            Session["current_page"] = 1;
            ProductsIndexModel PIndex = new ProductsIndexModel();
            PIndex.Products = getProductsList();
            DataTable getProducts = new DataTable();
            DataTable getCountries = new DataTable();
            DataTable getVehicle_Types = new DataTable();
            DataTable getBrands = new DataTable();
            DataTable getCities = new DataTable();
            DataTable getCategories = new DataTable();
            DataTable getModels = new DataTable();

            string msg = "";

            //  string queryProducts = @"SELECT p.id AS id, 
            //R.rate Rate,
            //                              p.price AS price ,  
            //                              p.quantity AS quantity, 
            //                              p.title AS title ,   
            //                              p.is_new AS State,  
            //                              I.url AS Img ,  
            //                              v.type_name AS vehicle_type , 
            //                              B.name AS brand ,
            //                              PV.name  AS city ,
            //                              M.name as model 
            //                              FROM Products AS P
            //                              inner join Images AS I on p.id = i.referral_id 
            //                              INNER JOIN Models AS M ON M.id = P.model_id
            //                              INNER JOIN Brands AS B ON B.id = M.brand_id
            //                              INNER JOIN Vehicle_Types AS V ON V.id = M.vehicle_type_id
            //                              INNER JOIN Addresses AS A ON A.id = P.address_id
            //                              INNER JOIN Provinces AS PV ON PV.id = A.province_id 
            //         		    left join  (select Src_ID, avg(value) as rate from Rating GROUP BY src_id ) as R on P.id=R.src_id";

            string getCountriesquery = "SELECT id, name FROM Countries ;";
            string getVehicle_Typesquery = "SELECT id, type_name FROM Vehicle_Types;";
            string getBrandsquery = "SELECT id, name  FROM Brands;";
            string getModelsquery = "SELECT id, name  FROM Models;";
            string getProvincesquery = "SELECT id, name  FROM Provinces;";
            string getCategoriesquery = "SELECT  id, name FROM Categories;";

            //  //getProducts = Database.ReadTableByQuery(queryProducts, null, out msg);
            //getProducts = GetPartsTable();


            getCountries = Database.ReadTableByQuery(getCountriesquery, null, out msg);
            if (getCountries != null && getCountries.Rows.Count > 0)
            {
                PIndex.Countries = new List<CountriesModel>();
                foreach (DataRow Country in getCountries.Rows)
                {
                    PIndex.Countries.Add(new CountriesModel()
                    {
                        Name = Country["name"]?.ToString(),
                        ID = new Guid(Country["id"]?.ToString())
                    });
                }
            }
            getVehicle_Types = Database.ReadTableByQuery(getVehicle_Typesquery, null, out msg);
            if (getVehicle_Types != null && getVehicle_Types.Rows.Count > 0)
            {
                PIndex.VehicleTypes = new List<VehicleTypesModel>();
                foreach (DataRow Type in getVehicle_Types.Rows)
                {
                    PIndex.VehicleTypes.Add(new VehicleTypesModel()
                    {
                        Name = Type["type_name"]?.ToString(),
                        ID = new Guid(Type["id"]?.ToString())
                    });
                }
            }
            getBrands = Database.ReadTableByQuery(getBrandsquery, null, out msg);
            if (getBrands != null && getBrands.Rows.Count > 0)
            {
                PIndex.Brands = new List<BrandsModel>();
                foreach (DataRow Brand in getBrands.Rows)
                {
                    PIndex.Brands.Add(new BrandsModel()
                    {
                        Name = Brand["name"]?.ToString(),
                        ID = new Guid(Brand["id"]?.ToString())
                    });
                }
            }
            getCities = Database.ReadTableByQuery(getProvincesquery, null, out msg);
            if (getCities != null && getCities.Rows.Count > 0)
            {
                PIndex.Cities = new List<CitiesModel>();
                foreach (DataRow city in getCities.Rows)
                {
                    PIndex.Cities.Add(new CitiesModel()
                    {
                        Name = city["name"]?.ToString(),
                        ID = new Guid(city["id"]?.ToString())
                    });
                }
            }
            getCategories = Database.ReadTableByQuery(getCategoriesquery, null, out msg);
            if (getCategories != null && getCategories.Rows.Count > 0)
            {
                PIndex.Categories = new List<ServicesModel>();
                foreach (DataRow Cate in getCategories.Rows)
                {
                    PIndex.Categories.Add(new ServicesModel()
                    {
                        Name = Cate["name"]?.ToString(),
                        ID = new Guid(Cate["id"]?.ToString())
                    });
                }
            }
            getModels = Database.ReadTableByQuery(getModelsquery, null, out msg);
            if (getModels != null && getModels.Rows.Count > 0)
            {
                PIndex.Models = new List<ModelsModel>();
                foreach (DataRow model in getModels.Rows)
                {
                    PIndex.Models.Add(new ModelsModel()
                    {
                        Name = model["name"]?.ToString(),
                        ID = new Guid(model["id"]?.ToString())
                    });
                }
            }
            
            return View(PIndex); ;
        }


        public ActionResult UsedParts()
        {
            return View();
        }
        [Route("Parts/PartDetails")]
        [Route("Products/Details")]
        public ActionResult PartDetails()
        {


            if (Request.QueryString["id"] != null)
            {
                Guid id = new Guid(Request.QueryString["id"]?.ToString());
                ProductsModel part = new ProductsModel();
                DataTable details = new DataTable();
                DataTable getImages = new DataTable();
                DataTable getAddress = new DataTable();
                string msg = "";

                List<SqlParameter> prmtr = new List<SqlParameter>();
                string queryAllDetails = " SELECT p.id as id " +
                                        //" ,category_id " +
                                        //" ,(select name from Categories where id = p.category_id) as category " +
                                        " ,description " +
                                        " ,price " +
                                        " ,(select name from Stores where id = p.store_id) as store  " +
                                        " ,(select name from Models where id = p.model_id) as  model " +
                                        " ,(select type_name from Vehicle_Types where id = p.vehicle_type_id) as vehicle_type " +
                                        " ,is_new " +
                                        " ,mobile " +
                                        " ,quantity " +
                                        " ,keywords " +
                                        " ,(select full_name from Users as u where u.id = p.user_id) as owner " +
                                        " ,owner_name " +
                                        " ,(select details from Addresses where Addresses.id = p.address_id) as   address " +
                                        " ,title " +
                                        " ,whatsapp " +
                                        " ,i.url as url " +
                                        " FROM Products as p " +
                                        " inner join Images as i " +
                                        " on i.referral_id = p.id "+
                                        " where p.id = @pid and i.referral_type = 'Products' and is_main = 1";
                string queryImages = "select url from Images inner join Products as p on referral_id = @pid";
                string queryAddress = "";
                    //"SELECT Provinces.name as city "+
                    //                " ,(SELECT name  FROM Countries where id = country_id) as country " +
	                   //             "      , details " + 
                    //                "  FROM Addresses " +
                    //                "  INNER JOIN Provinces " +
                    //                "  ON Addresses.province_id = Provinces.id " + 
                    //                "  INNER JOIN Provinces " +
                    //                "  ON Addresses.province_id = Provinces.id " +
                    //                " where Addresses.id in (select address_id from Products where id = @pid); ";
                queryAddress = " SELECT  c.name as country, "+
                                "        pr.name as city, "+
                                "        ad.details as details " +
                                "  FROM Products as p "+
                                "  inner join Addresses as ad on ad.id = p.address_id "+
                                "  inner join Provinces as pr on pr.id = ad.province_id "+
                                "  inner join Countries as c on c.id = pr.country_id "+
                                "  where p.id = @pid";
                string queryServics = "  SELECT name " +
                                      "  FROM Categories as C " +
                                      "  INNER JOIN Products_Categories as PC ON C.id = PC.category_id " +
                                      "  INNER JOIN Products as P ON P.id = PC.product_id "+
                                      "  where p.id =@pid ";
                //string queryBrand = "select name from Brands where id in (select brand_id from where id = @pid)";

                prmtr.Add(new SqlParameter("@pid", id));
                details = Database.ReadTableByQuery(queryAllDetails, prmtr, out msg);

                        /* * * * * * * */
                prmtr = new List<SqlParameter>();
                prmtr.Add(new SqlParameter("@pid", id) );
                getAddress = Database.ReadTableByQuery(queryAddress, prmtr, out msg);


                //get details
                if (details != null && details.Rows.Count > 0)
                {
                    DataRow part_row = details.Rows[0];
                    part.ID = id;
                    part.Title = part_row["title"]?.ToString();

                    part.User = new UsersModel() { Name = part_row["owner"]?.ToString() };
                    part.Address = new AddressModel();

                    part.Mobile = part_row["mobile"]?.ToString();
                    part.WhatsApp = part_row["whatsapp"]?.ToString();
                    part.Description = part_row["description"]?.ToString();
                    part.IsNew = Convert.ToInt32(part_row["is_new"]);
                    part.Keywords = part_row["keywords"]?.ToString();
                    //part.Model = part_row["model"]?.ToString();
                    part.OwnerName = part_row["owner"]?.ToString();//owner_name
                    part.OwnerName += part_row["owner_name"]?.ToString();//owner_name
                    part.Price = part_row["price"]?.ToString();
                    part.Quantity = part_row["quantity"]?.ToString();
                    part.WhatsApp = part_row["Whatsapp"]?.ToString();

                    part.VehicleType= new VehicleTypesModel()
                    {
                        Name = part_row["vehicle_type"]?.ToString()
                    };


                    //part.Services = new List<ServicesModel>();                    
                    //part.Services.Add(new ServicesModel() 
                    //{
                    //    Name = part_row["category"]?.ToString() 
                    //});
                 

                    part.Model = new ModelsModel()
                    {
                        Name = part_row["model"]?.ToString(),
                    };

                    part.Image = new ImagesModel()
                    {
                        URL = part_row["url"]?.ToString()
                    };
                }

                // get Sevices
                //prmtr = new List<SqlParameter>();
                //prmtr.Add(new SqlParameter("@pid", id));
                //getServics = Database.ReadTableByQuery(queryServics, prmtr, out msg);
                //if(getServics !=null && getServics.Rows.Count > 0)
                //{
                //    foreach(DataRow service in getServics.Rows)
                //    {
                //        part.Service = new List<ServicesModel>() { new ServicesModel() { Name = service["name"]?.ToString() } };
                //    }
                //}


                //get Adress 
                if (getAddress != null && getAddress.Rows.Count > 0)
                {
                    DataRow adres_row = getAddress.Rows[0];
                    part.Address = new AddressModel() { Country = adres_row["country"]?.ToString(), Province = adres_row["city"]?.ToString(), AddressName = adres_row["details"]?.ToString() };
                    //part.Address.Country = adres_row["country"]?.ToString();
                    //part.Address.Province = adres_row["city"]?.ToString();
                    //part.Address.AddressName = adres_row["details"]?.ToString();
                }


                //get Images
                prmtr = new List<SqlParameter>();
                prmtr.Add(new SqlParameter("@pid", id));
                getImages = Database.ReadTableByQuery(queryImages, prmtr, out msg);

                part.Images = new List<ImagesModel>();
                if (getImages != null && getImages.Rows.Count > 0)
                {
                    foreach (DataRow images in getImages.Rows)
                    {
                        part.Images.Add(new ImagesModel() { URL = images["url"]?.ToString() });
                    }
                }

                return View(part);
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
        //        List<SqlParameter> li = new List<SqlParameter>();
        //        string msg;
        //        string sql = @"SELECT P.id ID
        //                      ,title
        //                      ,I.url AS Img 
        //                  FROM Products AS P
        //                    INNER JOIN Images AS I on P.id = I.referral_id    ";
                                                           
        //        if (user["is_admin"]?.ToString() == "0")
        //        {
        //            sql += " WHERE P.user_id= @uid";
        //        li.Add(new SqlParameter("@uid", user["id"]?.ToString()));
        //        }


        //        DataTable parts = Database.ReadTableByQuery(sql, li, out msg);
        //        List<ProductsModel> ProductsList = new List<ProductsModel>();
        //        foreach (DataRow part in parts.Rows)
        //        {
        //            ProductsList.Add(new ProductsModel()
        //            {
        //                ID = new Guid(part["ID"]?.ToString()),
        //                Title = part["Title"]?.ToString(),
        //                Image=new ImagesModel() { URL= part["Img"]?.ToString()}
        //            });
        //        }

        //        return PartialView(ProductsList);
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
        //    //model.referal_id = Request.Params["select_part"] != null ? new Guid(Request.Params["select_part"]) : new Guid();
        //    //model.referal_id = new Guid(Request.Params["select_winches"] ?? (new Guid()).ToString());
        //    model.referal_type = Request.Params["Offer_type"] ?? "";
        //    model.name = Request.Params["name"] ?? "";
        //    model.Is_Active = Request.Params["Is_Active"] ?? "1";
        //    model.description = Request.Params["desc"] ?? "";
        //    model.discount_percentage = Convert.ToDouble((Request.Params["discount_percentage"] ?? "0").ToString());
        //    model.paymentmethods = Request.Params["paymentmethod"] ?? "";
        //    model.mobile = Request.Params["phonenum"] ?? "";
        //    model.website = Request.Params["site"] ?? "";
        //    model.address_id = new Guid(Request.Params["address_id"] ?? new Guid().ToString());
        //    model.start_date = (Request.Params["meeting-time-st"] ?? "").AsDateTime().ToString();
        //    model.end_date = (Request.Params["meeting-time-en"] ?? "").AsDateTime().ToString();
        //    model.referal_id = new Guid();
        //    List<string> referal_products = (Request.Params["ProductsList"] ?? string.Empty).Split(',').ToList();

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
        //                foreach (var item in referal_products)
        //                {
        //                    List<string> listr = new List<string>() 
        //                    {
        //                       "Product_id"
        //                        ,"offer_id"
        //                        ,"created_at"
        //                        ,"updated_at"
        //                    };
        //                    List<object> liobj = new List<object>()
        //                    {
        //                        new Guid(item),
        //                        model.id,
        //                        DateTime.Now,
        //                        DateTime.Now
        //                    };
        //                    Database.InsertRow("Offers_Products", Guid.NewGuid(), listr, liobj, out errMessage);
        //                }

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

        bool ISValid(ProductsModel Product, out string msg)
        {
            bool flag = true;
            if (Product.Title == "")
            {
                msg = Resources.CP.EnterTitle;
                return false;
            }
            if (Product.OwnerName == "")
            {
                msg = Resources.CP.EnterOwnerName;
                return false;
            }
            if (Product.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP.EnterCity;
                return false;
            }
            if (Product.Address.AddressName == "")
            {
                msg = Resources.CP.EnterAddress;
                return false;
            }
            if (Product.Mobile == null)
            {
                msg = Resources.CP.EnterPhone;
                return false;
            }
            if (Request.Params["services"] == null)
            {
                msg = Resources.CP.EnterSerives;
                return false;
            }
            if (Product.VehicleType == null)
            {
                msg = Resources.CP.EnterVehicle;
                return false;
            }
            if (Product.Model == null)
            {
                msg = Resources.CP.EnterModel;
                return false;
            }
            if (Product.Brand == null)
            {
                msg = Resources.CP.EnterBrand;
                return false;
            }
            if (Product.IsNew == -1)
            {
                msg = Resources.CP.EnterIsNew;
                return false;
            }
            if (Product.Price == "")
            {
                msg = Resources.CP.EnterPrice;
                return false;
            }
            if (Product.Quantity == "0")
            {
                msg = Resources.CP.EnterQuantity;
                return false;
            }
            if (Product.Year == "-1")
            {
                msg = Resources.CP.EnterYear;
                return false;
            }
            if (Product.Keywords == "")
            {
                msg = Resources.CP.EnterKeywords;
                return false;
            }
            if (Product.Description == "")
            {
                msg = Resources.CP.EnterDescrption;
                return false;
            }
            if (Session["Attachment"] == null)
            {
                msg = Resources.CP.EnterImage;
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


        private List<ProductsModel> getProductsList()
        {
            List<SqlParameter> li = new List<SqlParameter>();
            var data = Request.QueryString;
            if (data["Filter"] == null || data["Filter"] != "1")
            {
                if (Session["data"] != null) data = (NameValueCollection)Session["data"];
            }
            Session["data"] = data;
            string Factory = data["Factory"] ?? string.Empty;
            string Brand = data["Brand"] ?? string.Empty;
            string model = data["Model"] ?? string.Empty;
            string vehicle = data["vehicle_type"] ?? string.Empty;
            string country = data["country"] ?? string.Empty;
            string city = data["city"] ?? string.Empty;
            string minrange = data["minrange"] ?? string.Empty;
            string maxrange = data["maxrange"] ?? string.Empty;
            string statuses = data["status"] ?? string.Empty;
            string status = data["status"] ?? string.Empty;
            string Pirce = data["Categories"] ?? string.Empty;
            List<string> HTML_Garages = new List<string>();
            string sql_query = @" SELECT p.id AS id, 
                                        P.created_at,
										R.rate Rate,
                                        p.price AS price ,  
                                        p.quantity AS quantity, 
                                        p.title AS title ,   
                                        p.is_new AS State,  
                                        I.url AS Img ,  
                                        v.type_name AS vehicle_type , 
                                        B.name AS brand ,
                                        PV.name  AS city ,
                                        M.name as model 
                                        FROM Products AS P
                                        inner join Images AS I on p.id = i.referral_id 
                                        INNER JOIN Models AS M ON M.id = P.model_id
                                        INNER JOIN Brands AS B ON B.id = M.brand_id
                                        INNER JOIN Vehicle_Types AS V ON V.id = M.vehicle_type_id
                                        INNER JOIN Addresses AS A ON A.id = P.address_id
                                        INNER JOIN Provinces AS PV ON PV.id = A.province_id 
			                		    left join  (select Src_ID, avg(value) as rate from Rating GROUP BY src_id ) as R on P.id=R.src_id 
";
            bool flag = true;
            if (Factory != String.Empty && Factory != "-1")
            {
                if (flag)
                {
                    sql_query += " where B.country_id like @factoryid ";
                    flag = false;
                }
                else
                {
                    sql_query += " and  B.country_id like @factoryid  ";
                }
                li.Add(new SqlParameter("@factoryid", Factory));
            }
            if (model != String.Empty && model != "-1")
            {
                if (flag)
                {
                    sql_query += " where model_id like @model_id ";
                    flag = false;
                }
                else
                {
                    sql_query += " and model_id like @model_id  ";
                }
                li.Add(new SqlParameter("@brandid", model));
            }
            if (Brand != String.Empty && Brand != "-1")
            {
                if (flag)
                {
                    sql_query += " where brand_id like @brandid ";
                    flag = false;
                }
                else
                {
                    sql_query += " and  brand_id like @brandid ";
                }
                li.Add(new SqlParameter("@brandid", Brand));
            }
            if (vehicle != String.Empty && vehicle != "-1")
            {
                if (flag)
                {
                    sql_query += " where vehicle_type_id like @vehicle ";
                    flag = false;
                }
                else
                {
                    sql_query += " and  vehicle_type_id like @vehicle ";
                }
                li.Add(new SqlParameter("@vehicle", vehicle));
            }
            if (country != String.Empty && country != "-1")
            {
                if (flag)
                {
                    sql_query += " where address_id in ( select id from Addresses ad inner join Provinces pr on ad.province_id=pr.id  where country_id like @country) ";
                    flag = false;
                }
                else
                {
                    sql_query += " and  address_id in ( select id from Addresses ad inner join Provinces pr on ad.province_id=pr.id  where country_id like @country)  ";
                }
                li.Add(new SqlParameter("@country", country));
            }
            if (city != String.Empty && city != "-1")
            {
                if (flag)
                {
                    sql_query += " where address_id in ( select id from Addresses province_id like @city) ";
                    flag = false;
                }
                else
                {
                    sql_query += " and  address_id in ( select id from Addresses province_id like @city)   ";
                }
                li.Add(new SqlParameter("@city", city));
            }
            if (minrange != String.Empty && minrange != "-1")
            {
                if (flag)
                {
                    sql_query += " where  price  > @minrange";
                    flag = false;
                }
                else
                {
                    sql_query += " and price  > @minrange";
                }
                li.Add(new SqlParameter("@minrange", minrange));
            }
            if (maxrange != String.Empty && maxrange != "-1")
            {
                if (flag)
                {
                    sql_query += " where  price < @maxrange";
                    flag = false;
                }
                else
                {
                    sql_query += " and price  < @maxrange";
                }
                li.Add(new SqlParameter("@maxrange", maxrange));
            }
            if (statuses != String.Empty && statuses != "-1")
            {
                if (flag)
                {
                    sql_query += " where  is_new  like @statuses";
                    flag = false;
                }
                else
                {
                    sql_query += " and is_new  like @statuses";
                }
                li.Add(new SqlParameter("@statuses", statuses));
            }
            List<string> Categories = data["Categories"]?.Split(',').ToList();
            if (Categories != null && Categories.Count > 0)
            {
                int i = 0;
                if (flag)
                {
                    sql_query += " where p.id in (SELECT product_id FROM Products_Categories  ";
                    flag = false;
                }
                else
                {
                    sql_query += " and  p.id in (SELECT product_id FROM Products_Categories   ";
                }

                foreach (var Category in Categories)
                {
                    if (Category == Categories[0])
                    {
                        sql_query += " where category_id = @Category_" + i.ToString();
                    }
                    else
                    {
                        sql_query += " and category_id = @Category_" + i.ToString();
                    }
                    li.Add(new SqlParameter("@Category_" + i.ToString(), Category));
                    i++;
                }
                sql_query += "  )  ";
            }
            List<ProductsModel> part = new List<ProductsModel>();
            int page_number = 1;

            page_number = (int)Session["current_page"];

            DataTable getProducts = ConverSQLQueryPage(sql_query, li, "created_at", page_number, 4, out string msg, out int itemCount);
            //DataTable getProducts= Database.ReadTableByQuery(sql_query, li, out msg);
            List<ProductsModel> products = new List<ProductsModel>();
            if (getProducts != null && getProducts.Rows.Count > 0)
            {
                foreach (DataRow Product in getProducts.Rows)
                {
                    float f = 0f;
                    float.TryParse(Product["Rate"].ToString(), out f);
                    products.Add(new ProductsModel()
                    {
                        ID = new Guid(Product["id"]?.ToString()),
                        Title = Product["title"]?.ToString(),
                        IsNew = Convert.ToInt32(Product["State"]),
                        Price = Product["price"]?.ToString(),
                        Address = new AddressModel() { Province = Product["city"]?.ToString() },
                        VehicleType = new VehicleTypesModel() { Name = Product["vehicle_type"]?.ToString() },
                        Image = new ImagesModel() { URL = Product["Img"]?.ToString() },
                        Model = new ModelsModel() { Name = Product["model"]?.ToString() },
                        Brand = new BrandsModel() { Name = Product["brand"]?.ToString() },
                        Rate = f
                    });
                }
            }
            int pages_count = (int)Math.Ceiling(((decimal)itemCount / (decimal)4));
            Session["pages_count"] = pages_count;
            return products;
        }
//        private DataTable GetPartsTable()
//        {
//            List<SqlParameter> li = new List<SqlParameter>();
//            string Factory = Request.Params["Factory"] ?? string.Empty;
//            string Brand = Request.Params["Brand"] ?? string.Empty;
//            string model = Request.Params["Model"] ?? string.Empty;
//            string vehicle = Request.Params["vehicle_type"] ?? string.Empty;
//            string country = Request.Params["country"] ?? string.Empty;
//            string city = Request.Params["city"] ?? string.Empty;
//            string minrange = Request.Params["minrange"] ?? string.Empty;
//            string maxrange = Request.Params["maxrange"] ?? string.Empty;
//            string statuses = Request.Params["status"] ?? string.Empty;
//            string status = Request.Params["status"] ?? string.Empty;
//            string Pirce = Request.Params["Categories"] ?? string.Empty;
//            List<string> HTML_Garages = new List<string>();
//            string sql_query = @" SELECT p.id AS id, 
//										R.rate Rate,
//                                        p.price AS price ,  
//                                        p.quantity AS quantity, 
//                                        p.title AS title ,   
//                                        p.is_new AS State,  
//                                        I.url AS Img ,  
//                                        v.type_name AS vehicle_type , 
//                                        B.name AS brand ,
//                                        PV.name  AS city ,
//                                        M.name as model 
//                                        FROM Products AS P
//                                        inner join Images AS I on p.id = i.referral_id 
//                                        INNER JOIN Models AS M ON M.id = P.model_id
//                                        INNER JOIN Brands AS B ON B.id = M.brand_id
//                                        INNER JOIN Vehicle_Types AS V ON V.id = M.vehicle_type_id
//                                        INNER JOIN Addresses AS A ON A.id = P.address_id
//                                        INNER JOIN Provinces AS PV ON PV.id = A.province_id 
//			                		    left join  (select Src_ID, avg(value) as rate from Rating GROUP BY src_id ) as R on P.id=R.src_id 
//";
//            bool flag = true;
//            if (Factory != String.Empty && Factory != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where brand_id in (select id from Garages_Brands gb where country_id like @factoryid) ";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and  brand_id in (select id from Garages_Brands gb where country_id like @factoryid)  ";
//                }
//                li.Add(new SqlParameter("@factoryid", Factory));
//            }
//            if (model != String.Empty && model != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where model_id like @model_id ";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and model_id like @model_id  ";
//                }
//                li.Add(new SqlParameter("@brandid", model));
//            }
//            if (Brand != String.Empty && Brand != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where brand_id like @brandid ";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and  brand_id like @brandid ";
//                }
//                li.Add(new SqlParameter("@brandid", Brand));
//            }
//            if (vehicle != String.Empty && vehicle != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where vehicle_type_id like @vehicle ";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and  vehicle_type_id like @vehicle ";
//                }
//                li.Add(new SqlParameter("@vehicle", vehicle));
//            }
//            if (country != String.Empty && country != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where address_id in ( select id from Addresses ad inner join Provinces pr on ad.province_id=pr.id  where country_id like @country) ";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and  address_id in ( select id from Addresses ad inner join Provinces pr on ad.province_id=pr.id  where country_id like @country)  ";
//                }
//                li.Add(new SqlParameter("@country", country));
//            }
//            if (city != String.Empty && city != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where address_id in ( select id from Addresses province_id like @city) ";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and  address_id in ( select id from Addresses province_id like @city)   ";
//                }
//                li.Add(new SqlParameter("@city", city));
//            }
//            if (minrange != String.Empty && minrange != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where  price  > @minrange";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and price  > @minrange";
//                }
//                li.Add(new SqlParameter("@minrange", minrange));
//            }
//            if (maxrange != String.Empty && maxrange != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where  price < @maxrange";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and price  < @maxrange";
//                }
//                li.Add(new SqlParameter("@maxrange", maxrange));
//            }
//            if (statuses != String.Empty && statuses != "-1")
//            {
//                if (flag)
//                {
//                    sql_query += " where  is_new  like @statuses";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and is_new  like @statuses";
//                }
//                li.Add(new SqlParameter("@statuses", statuses));
//            }
//            List<string> Categories = Request.Params["Categories"]?.Split(',').ToList();
//            if (Categories != null && Categories.Count > 0)
//            {
//                int i = 0;
//                if (flag)
//                {
//                    sql_query += " where p.id in (SELECT product_id FROM Products_Categories  ";
//                    flag = false;
//                }
//                else
//                {
//                    sql_query += " and  p.id in (SELECT product_id FROM Products_Categories   ";
//                }

//                foreach (var Category in Categories)
//                {
//                    if (Category == Categories[0])
//                    {
//                        sql_query += " where category_id = @Category_" + i.ToString();
//                    }
//                    else
//                    {
//                        sql_query += " and category_id = @Category_" + i.ToString();
//                    }
//                    li.Add(new SqlParameter("@Category_" + i.ToString(), Category));
//                    i++;
//                }
//                sql_query += "  )  ";
//            }
//            List<ProductsModel> part = new List<ProductsModel>();
//            return Database.ReadTableByQuery(sql_query, li, out string msg);
//        }

    }
}