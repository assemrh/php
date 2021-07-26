using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class RentOfficesController : BaseController
    {
        // GET: Rent

        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = null, Adding = "/RentOffices/AddRentOffices/" });
        }

        [HttpPost]
        public JsonResult AddRentOffices()
        {
            string msg = "";
            int code = 0;

            Session["error"] = null;
            RentOfficesModel new_rentoffice = new RentOfficesModel();
            new_rentoffice.Name = Request.Params["name"] != null ? Request.Params["name"] : "";
            new_rentoffice.Mobile = Request.Params["phoneno"].ToString() != string.Empty ? Request.Params["phone_key"] + " " + Request.Params["phoneno"] : null;
            new_rentoffice.Fax = Request.Params["fax"] != null ? Request.Params["fax"] : "";
            new_rentoffice.Website = Request.Params["website"];
            new_rentoffice.Whatsapp = Request.Params["whatsapp"] != null ? Request.Params["whatsapp"] : "";
            new_rentoffice.Facebook = Request.Params["facebook"] != null ? Request.Params["facebook"] : "";
            new_rentoffice.Tiktok = Request.Params["tiktok"] != null ? Request.Params["tiktok"] : "";
            new_rentoffice.Snapchat = Request.Params["snapchat"] != null ? Request.Params["snapchat"] : "";
            new_rentoffice.Twitter = Request.Params["twitter"] != null ? Request.Params["twitter"] : "";
            new_rentoffice.Instagram = Request.Params["instagram"] != null ? Request.Params["instagram"] : "";
            new_rentoffice.Linkedin = Request.Params["linkedin"] != null ? Request.Params["linkedin"] : "";
            new_rentoffice.Youtube = Request.Params["youtube"] != null ? Request.Params["youtube"] : "";
            new_rentoffice.Description = Request.Params["description"] != null ? Request.Params["description"] : "";
            new_rentoffice.User = new UsersModel();
            new_rentoffice.User.ID = new Guid(Session["id"].ToString());
            new_rentoffice.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                new_rentoffice.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                new_rentoffice.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                new_rentoffice.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                new_rentoffice.Address.ProvinceId = new Guid();
            }

            if (ISValid(new_rentoffice, out msg))
            {
                Guid id = Guid.NewGuid();
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "mobile", "fax", "whatsapp", "facebook", "tiktok", "snapchat", "twitter", "instagram", "linkedin", "youtube", "user_id", "website", "description", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = {
                    new_rentoffice.Name, new_rentoffice.Mobile, new_rentoffice.Fax, new_rentoffice.Whatsapp, new_rentoffice.Facebook, new_rentoffice.Tiktok, new_rentoffice.Snapchat,
                    new_rentoffice.Twitter, new_rentoffice.Instagram, new_rentoffice.Linkedin, new_rentoffice.Youtube, new_rentoffice.User.ID, new_rentoffice.Website, new_rentoffice.Description, DateTime.Now
                };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Rental_Offices", id, cols, vals, out errMessage))
                {
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { new_rentoffice.Address.ProvinceId, new_rentoffice.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Rental_Offices", id, cols, vals, out errMessage);

                    var vt = Request.Params["vehicletypes"].Split(',');
                    List<string> vehicletypes = vt.ToList();
                    foreach (string vehicletype in vehicletypes)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "rental_office_id", "vehicle_type_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, vehicletype, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Vehicle_Types_Rental_Offices", Guid.NewGuid(), cols, vals, out errMessage);
                    }

                    var mo = Request.Params["models"].Split(',');
                    List<string> models = mo.ToList();
                    foreach (string model in models)
                    {
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "rental_office_id", "model_id", "created_at" };
                        cols.AddRange(colsinput);
                        valsin = new object[] { id, model, DateTime.Now };
                        vals.AddRange(valsin);
                        Database.InsertRow("Rental_Offices_Models", Guid.NewGuid(), cols, vals, out errMessage);
                    }
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/RentOffice/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "created_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, id, "Rental_Offices", DateTime.Now };
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


        string msj = "", sqlquery;

        public ActionResult Index()
        {
            Session["data"] = null;
            Session["current_page"] = 1;
            RentOfficesIndexModel RIndex = new RentOfficesIndexModel();
            RIndex.RentOffices = GetRentOfficeList();

            RIndex.Brands = new List<BrandsModel>();
            DataTable BrandsTable = new DataTable();
            sqlquery = "SELECT id, name  FROM Brands;";

            BrandsTable = Database.ReadTableByQuery(sqlquery, null, out msj);

            if (BrandsTable != null && BrandsTable.Rows.Count > 0)
            {
                foreach (DataRow v in BrandsTable.Rows)
                {
                    RIndex.Brands.Add(new BrandsModel() { ID = new Guid (v["id"].ToString()),Name= v["name"].ToString() });
                }
            }

            RIndex.Categories = new List<ServicesModel>();
            RIndex.Cities = new List<CitiesModel>();
            DataTable CityTable = new DataTable();
            sqlquery = "SELECT id, name  FROM Provinces;";

            CityTable = Database.ReadTableByQuery(sqlquery, null, out msj);

            if (CityTable != null && CityTable.Rows.Count > 0)
            {
                foreach (DataRow c in CityTable.Rows)
                {
                    RIndex.Cities.Add(new CitiesModel() {ID=new Guid(c["id"].ToString()),Name=c["name"].ToString() });
                //        < option value = "@c["id"]" > @c["name"] </ option >
                }
            }
            RIndex.Countries = new List<CountriesModel>();

            RIndex.Models = new List<ModelsModel>();
            DataTable ModelsTable = new DataTable();
            sqlquery = "SELECT  id, name  FROM Models;";

            ModelsTable = Database.ReadTableByQuery(sqlquery, null, out msj);

            if (ModelsTable != null && ModelsTable.Rows.Count > 0)
            {
                foreach (DataRow v in ModelsTable.Rows)
                {
                    RIndex.Models.Add(new ModelsModel() {ID=new Guid(v["id"].ToString()), Name = v["name"].ToString() });
                }
            }

            RIndex.VehicleTypes = new List<VehicleTypesModel>();
            DataTable vehiclesTable = new DataTable();
            sqlquery = "SELECT id, type_name FROM Vehicle_Types;";

            vehiclesTable = Database.ReadTableByQuery(sqlquery, null, out msj);

            if (vehiclesTable != null && vehiclesTable.Rows.Count > 0)
            {
                foreach (DataRow v in vehiclesTable.Rows)
                {
                    RIndex.VehicleTypes.Add( new VehicleTypesModel() { ID = new Guid(v["id"].ToString()), Name = v["type_name"].ToString() });
                }
            }

            return View(RIndex);
        }


        [HttpPost]
        public ActionResult GetRentOffices(int page = 1)//
        {
            Session["current_page"] = page;
            List<RentOfficesModel> rentOfficesModels = GetRentOfficeList();

            return PartialView("Content", rentOfficesModels);
        }

        private List<RentOfficesModel> GetRentOfficeList()
        {
            List<SqlParameter> li = new List<SqlParameter>();
            var data = Request.QueryString;
            if (data["Filter"] == null || data["Filter"] != "1")
            {
                if (Session["data"] != null) data = (NameValueCollection)Session["data"];
            }
            Session["data"] = data;
            int page_number = 1;
            page_number = (int)Session["current_page"];

            string Factory = "";// Request.Params["Factory"] ?? string.Empty;
            string Brand = "";// Request.Params["Brand"] ?? string.Empty;
            string model = "";//Request.Params["Model"] ?? string.Empty;
            string vehicle = data["vehicle_type"] ?? string.Empty;
            string country = data["country"] ?? string.Empty;
            string city = data["city"] ?? string.Empty;
            DataTable rents = new DataTable();
            string msg = "";
            string sql_query  = @" select pr.name city,
                                    R.id,R.name,
                                    R.description,
                                    R.mobile,R.website,
                                    RT.rate Rate,
                                    R.created_at,
                                    I.url AS Img  from Rental_Offices R 
                                    inner join Images I on R.id = I.referral_id 
                                    inner join Addresses ad on ad.id=R.address_id 
                                    inner join Provinces pr on pr.id=ad.province_id 
                                    inner join Countries c on c.Id= pr.country_id 
                                    left join  (select Src_ID, avg(value) as rate from Rating GROUP BY src_id ) as RT on R.id=RT.src_id ";
            bool flag = true;
            if (Factory != String.Empty && Factory != "-1")
            {
                if (flag)
                {
                    sql_query += " where brand_id in (select id from Garages_Brands gb where country_id like @factoryid) ";
                    flag = false;
                }
                else
                {
                    sql_query += " and  brand_id in (select id from Garages_Brands gb where country_id like @factoryid)  ";
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
                    sql_query += " where vtr.vehicle_type_id like @vehicle ";
                    flag = false;
                }
                else
                {
                    sql_query += " and vtr.vehicle_type_id like @vehicle ";
                }
                li.Add(new SqlParameter("@vehicle", vehicle));
            }
            if (country != String.Empty && country != "-1")
            {
                if (flag)
                {
                    sql_query += " where  ";
                    flag = false;
                }
                else
                {
                    sql_query += " and  ";
                }
                li.Add(new SqlParameter("@country", country));
            }
            if (city != String.Empty && city != "-1")
            {
                if (flag)
                {
                    sql_query += " where pr.id like @city ";
                    flag = false;
                }
                else
                {
                    sql_query += " and   pr.id like @city   ";
                }
                li.Add(new SqlParameter("@city", city));
            }

            List<RentOfficesModel> rent = new List<RentOfficesModel>();
            //rents = Database.ReadTableByQuery(sql_query, li, out msg);
            rents = Database.ConverSQLQueryPage(sql_query, li, "created_at", page_number, 4, out msg, out int itemCount);

            if (rents != null && rents.Rows.Count > 0)
            {
                foreach (DataRow row in rents.Rows)
                {
                    float f = 0f;
                    float.TryParse(row["Rate"].ToString(), out f);
                    rent.Add(new RentOfficesModel() { 
                        ID=new Guid(row["id"].ToString()),
                        Image=new ImagesModel() { URL= row["Img"].ToString() },
                        Name= row["name"].ToString(),
                        Mobile= row["mobile"].ToString(),
                        Address = new AddressModel() { Province = row["city"].ToString() },
                        Website= row["website"].ToString(),
                        Description= row["description"].ToString(),
                        Rate= f

                    });

                }

            }
            int pages_count = (int)Math.Ceiling(((decimal)itemCount / (decimal)4));
            Session["pages_count"] = pages_count;
            return rent;
        }

        [Route("RentOffice/Details")]
        [Route("RentOffices/RentDetails")]
        public ActionResult RentDetails()
        {

            if (Request.QueryString["id"] != null)
            {
                Guid id = new Guid(Request.QueryString["id"].ToString());
                RentOfficesModel rent = new RentOfficesModel();
                DataTable details = new DataTable();
                DataTable getType = new DataTable();
                DataTable getModels = new DataTable();
                DataTable getImages = new DataTable();
                string msg = "";
                string msg1 = "";

                List<SqlParameter> li = new List<SqlParameter>();

                string queryDetails = "select r.name,(select full_name from Users where id = r.user_id) as owner," +
                               "(select name from Countries where id in (select country_id from Provinces where id in (select province_id from Addresses where id = r.address_id))) as country," +
                               "(select name from Provinces where id in (select province_id from Addresses where id = r.address_id)) as city," +
                               "(select details from Addresses where id = r.address_id) as address,r.mobile," +
                               "r.website,r.fax,r.whatsapp,r.facebook,r.twitter,r.instagram,r.youtube,r.snapchat,r.tiktok,r.linkedin,r.description , i.url  from Rental_Offices as r inner join Images as i " +
                               " on i.referral_id = r.id where r.id = @rid and i.referral_type = 'Rental_Offices' and is_main = 1";
                string queryType = "select type_name from Vehicle_Types where id in (select vehicle_type_id from Vehicle_Types_Rental_Offices where rental_office_id = @rid)";
                string queryImages = "select url from Images inner join Rental_Offices as r on referral_id = @rid";
                string queryModles = "select m.name as Model_Name,b.name as Brand_Name, vt.type_name as VT_Name " +
                                        " from Models as M" +
                                        " inner join Brands as B on m.brand_id =b.id" +
                                        " inner join Vehicle_Types as VT on m.vehicle_type_id=vt.id" +
                                        " where m.id in (select model_id from Rental_Offices_Models where rental_office_id = @rid)";
                li.Add(new SqlParameter("@rid", id));
                details = Database.ReadTableByQuery(queryDetails, li, out msg);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@rid", id));
                getType = Database.ReadTableByQuery(queryType, li, out msg);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@rid", id));
                getModels = Database.ReadTableByQuery(queryModles , li, out msg);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@rid", id));
                getImages = Database.ReadTableByQuery(queryImages, li, out msg);

                //get details
                if (details != null && details.Rows.Count > 0)
                {
                    DataRow rent_row = details.Rows[0];
                    rent.ID = id;
                    rent.Name = rent_row["name"].ToString();

                    rent.User = new UsersModel() { Name = rent_row["owner"].ToString() };
                    rent.Address = new AddressModel();
                    rent.Address.Country = rent_row["country"].ToString();
                    rent.Address.Province = rent_row["city"].ToString();
                    rent.Address.AddressName = rent_row["address"].ToString();
                    rent.Mobile = rent_row["mobile"].ToString();
                    rent.Website = rent_row["website"].ToString();
                    rent.Fax = rent_row["fax"].ToString();
                    rent.Whatsapp = rent_row["whatsapp"].ToString();
                    rent.Facebook = rent_row["facebook"].ToString();
                    rent.Whatsapp = rent_row["whatsapp"].ToString();
                    rent.Twitter = rent_row["twitter"].ToString();
                    rent.Instagram = rent_row["instagram"].ToString();
                    rent.Youtube = rent_row["youtube"].ToString();
                    rent.Snapchat = rent_row["snapchat"].ToString();
                    rent.Tiktok = rent_row["tiktok"].ToString();
                    rent.Linkedin = rent_row["linkedin"].ToString();
                    rent.Description = rent_row["description"].ToString();
                    
                    rent.Image = new ImagesModel()
                    {
                        URL = rent_row["url"].ToString()
                    };
                }

                //get vehicle types
                rent.VehicleTypes = new List<VehicleTypesModel>();
                if (getType != null && getType.Rows.Count > 0)
                {
                    foreach (DataRow vehicle_type in getType.Rows)
                    {
                        rent.VehicleTypes.Add(new VehicleTypesModel() { Name = vehicle_type["type_name"].ToString() });
                    }


                }

                //get Models
                rent.Models_view = new List<ModelsModel_view>();

                if (getModels != null && getModels.Rows.Count > 0 )
                {

                    foreach(DataRow model in getModels.Rows)
                    {
                       rent.Models_view.Add(new ModelsModel_view() {
                           Name = model["Model_Name"].ToString() 
                           , Brand =model["Brand_Name"].ToString()
                           , VehicleType = model["VT_Name"].ToString()
                       });
                    }



                }


                //get Images
                rent.Images = new List<ImagesModel>();
                if (getImages != null && getImages.Rows.Count > 0)
                {
                    foreach (DataRow images in getImages.Rows)
                    {
                        rent.Images.Add(new ImagesModel() { URL = images["url"].ToString() });
                    }
                }

                return View(rent);
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
        //        string sql = @" SELECT TOP (1000) [id]
        //                              ,[name]
        //                          FROM [le-garage].[dbo].[Rental_Offices] ";
        //        if (user["is_admin"].ToString() == "0")
        //            sql += " WHERE [user_id]= @uid";

        //        List<SqlParameter> li = new List<SqlParameter>();
        //        li.Add(new SqlParameter("@uid", user["id"].ToString()));
        //        DataTable rents = Database.ReadTableByQuery(sql, li, out msg);
        //        List<RentOfficesModel> RentList = new List<RentOfficesModel>();
        //        RentList = new List<RentOfficesModel>();
        //        foreach (DataRow rent in rents.Rows)
        //        {
        //            RentList.Add(new RentOfficesModel()
        //            {
        //                ID = new Guid(rent["Id"].ToString()),
        //                Name = rent["name"].ToString(),
        //            });
        //        }

        //        return PartialView(RentList);
        //    }
        //    return null;
        //}

        bool ISValid(RentOfficesModel rentoffice, out string msg)
        {
            bool flag = true;
            if (rentoffice.Name == "")
            {
                msg = Resources.CP.EnterName;
                return false;
            }
            if (rentoffice.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP.EnterCity;
                return false;
            }
            if (rentoffice.Address.AddressName == "")
            {
                msg = Resources.CP.EnterAddress;
                return false;
            }
            if (rentoffice.Mobile == null)
            {
                msg = Resources.CP.EnterPhone;
                return false;
            }
            if (Request.Params["vehicletypes"] == null)
            {
                msg = Resources.CP.EnterVehicle;
                return false;
            }
            if (Request.Params["models"] == null)
            {
                msg = Resources.CP.EnterModel;
                return false;
            }
            if (rentoffice.Whatsapp == "")
            {
                msg = Resources.CP.EnterWhatsapp;
                return false;
            }
            if (rentoffice.Description == "")
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

    }
}