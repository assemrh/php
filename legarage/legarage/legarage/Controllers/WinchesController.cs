using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace legarage.Controllers
{

    public class WinchesController : BaseController
    {
        // GET: Recovery

        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = null, Adding = "/Winches/AddWinches/" });
        }

        [HttpPost]
        public JsonResult AddWinches()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            WinchesModel new_winche = new WinchesModel();
            new_winche.Title = Request.Params["Title"];
            new_winche.DriverName = Request.Params["DriverName"];
            new_winche.Mobile = Request.Params["phoneno"].ToString() != string.Empty ? Request.Params["phone_key"] + " " + Request.Params["phoneno"] : null;
            new_winche.DriverPhone = Request.Params["DriverPhone"];
            new_winche.VehicleSize = Request.Params["vehiclesize"];
            new_winche.Area = Request.Params["area"];
            new_winche.Whatsapp = Request.Params["whatsapp"];
            new_winche.Keywords = Request.Params["keywords"];
            new_winche.Description = Request.Params["description"];
            new_winche.User = new UsersModel();
            new_winche.User.ID = new Guid(Session["id"].ToString());
            new_winche.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                new_winche.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                new_winche.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                new_winche.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                new_winche.Address.ProvinceId = new Guid();
            }

            if (ISValid(new_winche,out msg))
            {
                Guid id = Guid.NewGuid();
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "keywords", "title", "driver_name", "mobile", "driver_phone", "whatsapp", "user_id", "vehiclesize", "area", "description", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_winche.Keywords, new_winche.Title, new_winche.DriverName, new_winche.Mobile, new_winche.DriverPhone, new_winche.Whatsapp, new_winche.User.ID, new_winche.VehicleSize, new_winche.Area, new_winche.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Winches", id, cols, vals, out errMessage))
                {
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { new_winche.Address.ProvinceId, new_winche.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Winches", id, cols, vals, out errMessage);
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Winches/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { ImageName, id, "Winches", DateTime.Now, 1 };
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



        private string msg = "";
        public ActionResult Index()
        {
            Session["data"] = null;
            Session["current_page"] = 1;
            WinchesIndexModel WIndex = new WinchesIndexModel();
            WIndex.Winches = getWincheList();
            

            WIndex.Brands = new List<BrandsModel>();
            WIndex.Categories = new List<ServicesModel>();
            WIndex.Cities = new List<CitiesModel>();
            WIndex.Countries = new List<CountriesModel>();
            WIndex.Models = new List<ModelsModel>();
            WIndex.VehicleTypes = new List<VehicleTypesModel>();
            DataTable vehiclesTable = new DataTable();

            vehiclesTable = Database.ReadTableByQuery("SELECT id, type_name FROM Vehicle_Types;", null, out msg);

            if (vehiclesTable != null && vehiclesTable.Rows.Count > 0)
            {
                foreach (DataRow v in vehiclesTable.Rows)
                {
                    WIndex.VehicleTypes.Add(new VehicleTypesModel() {ID=new Guid(v["id"].ToString()),Name= v["type_name"].ToString()});
                }
            }

            return View(WIndex);
        }

        [HttpGet]
        public ActionResult GetWinches(int page = 1)
        {
            Session["current_page"] = page;
            List<WinchesModel> partModelList = getWincheList();
            return PartialView("Content", partModelList);
        }

        private List<WinchesModel> getWincheList()
        {
            List<WinchesModel>  Winches = new List<WinchesModel>();
            DataTable getWinches = new DataTable();
            List<SqlParameter> li = new List<SqlParameter>();
            var data = Request.QueryString;
            if (data["Filter"] == null || data["Filter"] != "1")
            {
                if (Session["data"] != null) data = (NameValueCollection)Session["data"];
            }
            Session["data"] = data;
            int page_number = 1;
            page_number = (int)Session["current_page"];

            string size = data["sizes"] ?? string.Empty;
            string country = data["country"] ?? string.Empty;
            string city = data["city"] ?? string.Empty;
            string area = string.Empty;
            if (data["area"] != null)
            {
                area = "%" + data["area"].ToString() + "%";
            }
            string SQL = @" SELECT w.id as id 
                                          ,driver_name 
                                          ,driver_phone 
                                          ,address_id 
                                          ,user_id 
                                          ,whatsapp 
                                          ,mobile 
                                          ,description 
                                          ,area 
                                          ,vehiclesize 
                                          ,keywords 
                                          ,title 
                                          ,w.created_at
                                    	  ,PV.name  city
                                    	  ,PV.Id city_id
                                    	  ,pv.country_id country_id
                                    	  ,I.url as img
                                      , RT.rate Rate
                                       FROM Winches as w  
                                    	inner join Images as I on I.referral_id = w.id 
                                       inner join Addresses as A on A.id = w.address_id 
                                       inner join Provinces as PV on PV.id = A.province_id 
                                        left join  (select Src_ID, avg(value) as rate from Rating GROUP BY src_id ) as RT on W.id=RT.src_id ";

            bool flag = true;
            if (size != String.Empty && size != "-1")
            {
                if (flag)
                {
                    SQL += " where vehiclesize like @size ";
                    flag = false;
                }
                else
                {
                    SQL += " and vehiclesize like @size ";
                }
                li.Add(new SqlParameter("@size", size));
            }

            if (country != String.Empty && country != "-1")
            {
                if (flag)
                {
                    SQL += " where country_id like @country ";
                    flag = false;
                }
                else
                {
                    SQL += " and country_id like @country ";
                }
                li.Add(new SqlParameter("@country", country));
            }
            if (city != String.Empty && city != "-1")
            {
                if (flag)
                {
                    SQL += " where city_id like @city ";
                    flag = false;
                }
                else
                {
                    SQL += " and city_id like @city ";
                }
                li.Add(new SqlParameter("@city", city));
            }
            if (area != String.Empty && area != "%%")
            {
                if (flag)
                {
                    SQL += " where area like @Area_str ";
                    flag = false;
                }
                else
                {
                    SQL += " and area like @Area_str ";
                }
                li.Add(new SqlParameter("@Area_str", area));
            }
            //getWinches = Database.ReadTableByQuery(SQL, li, out msg);
            getWinches =  Database.ConverSQLQueryPage(SQL, li, "created_at", page_number, 4, out msg, out int itemCount);

            if (getWinches != null && getWinches.Rows.Count > 0)
            {
                foreach (DataRow winche in getWinches.Rows)
                {
                    float f = 0f;
                    float.TryParse(winche["Rate"].ToString(), out f);
                    Winches.Add(new WinchesModel()
                    {
                        ID = new Guid(winche["id"].ToString()),
                        Title = winche["title"].ToString(),
                        Image = new ImagesModel() { URL = winche["img"].ToString() },
                        Address = new AddressModel() { Province = winche["city"].ToString() },
                        Area = winche["area"].ToString(),
                        VehicleSize = winche["vehiclesize"].ToString(),
                        DriverPhone = winche["driver_phone"].ToString(),
                        Rate= f
                    });
                }
            }
            int pages_count = (int)Math.Ceiling(((decimal)itemCount / (decimal)4));
            Session["pages_count"] = pages_count;
            return Winches;
        }

        [Route("Winches/Details")]
        [Route("Winches/RecoveryDetails")]
        public ActionResult RecoveryDetails()
        {
            if (Request.QueryString["id"] != null)
            {
                Guid id = new Guid(Request.QueryString["id"].ToString());
                WinchesModel winches = new WinchesModel();

                // dataTables
                DataTable details = new DataTable();
                DataTable getImages = new DataTable();
                DataTable getAddress = new DataTable();
                DataTable getServics = new DataTable();
                List<SqlParameter> prmtr = new List<SqlParameter>();
                string queryDetails = " SELECT w.id as id " +
                                    "      ,driver_name " +
                                    "      ,driver_phone " +
                                    "      ,address_id " +
                                    "      ,user_id " +
                                    "      ,whatsapp " +
                                    "      ,mobile " +
                                    "      ,description " +
                                    "      ,area " +
                                    "      ,vehiclesize " +
                                    "      ,keywords " +
                                    "      ,title " +
                                    "	  ,PV.name as city" +
                                    "	  ,A.details as adres" +
                                    "	  ,PV.name as city" +
                                    "	  ,c.name as country" +
                                    "	  ,I.url as img" +
                                    "   FROM Winches as w  " +
                                    "	inner join Images as I on I.referral_id = w.id " +
                                    "   inner join Addresses as A on A.id = w.address_id " +
                                    "   inner join Provinces as PV on PV.id = A.province_id " +
                                    "   inner join Countries as C on c.id = pv.country_id " +
                                    "   where w.id = @wid ";
                string queryImages = "select I.url as url from Images as I inner join Winches as w on referral_id = @wid";

                
                
                    prmtr.Add(new SqlParameter("@wid", id));
                    details = Database.ReadTableByQuery(queryDetails, prmtr, out msg);
                //getAddress = Database.ReadTableByQuery()

                    if (details != null && details.Rows.Count > 0)
                    {
                        DataRow winche_row = details.Rows[0];
                        winches.ID = new Guid(winche_row["id"].ToString());
                        winches.Address = new AddressModel() { AddressName= winche_row["adres"].ToString(), Province= winche_row["city"].ToString(), Country = winche_row["country"].ToString() };
                        winches.Area = winche_row["area"].ToString();
                        winches.Description = winche_row["description"].ToString();
                        winches.DriverName = winche_row["driver_name"].ToString();
                        winches.DriverPhone = winche_row["driver_phone"].ToString();
                        winches.Image = new ImagesModel() { URL = winche_row["img"].ToString() };
                        
                        winches.Mobile = winche_row["mobile"].ToString();
                        winches.Title = winche_row["title"].ToString();
                        winches.VehicleSize = winche_row["vehiclesize"].ToString();
                        winches.Whatsapp = winche_row["whatsapp"].ToString();

                    }
                    prmtr = new List<SqlParameter>();
                    prmtr.Add(new SqlParameter("@wid", id));
                    getImages = Database.ReadTableByQuery(queryImages, prmtr, out msg);
                    winches.Images = new List<ImagesModel>();
                    if (getImages != null && getImages.Rows.Count>0)
                    {
                        foreach(DataRow img in getImages.Rows )
                        {
                            //winches.Images.Add(new ImagesModel() { URL= img["url"].ToString() });
                            winches.Images.Add(new ImagesModel() { URL = img["url"].ToString() });
                        }
                    }

                

                //catch (Exception e)
                //{
                //    winches.mesaj = e.Message.ToString() + msg;
                //}

                return View(winches);

            }
            else
            {
                return RedirectToAction("Index");
            }




        }


        bool ISValid(WinchesModel Winche, out string msg)
        {
            bool flag = true;
            if (Winche.Title == "")
            {
                msg = Resources.CP.EnterTitle;
                return false;
            }
            if (Winche.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP.EnterCity;
                return false;
            }
            if (Winche.Address.AddressName == "")
            {
                msg = Resources.CP.EnterAddress;
                return false;
            }
            if (Winche.Mobile == null)
            {
                msg = Resources.CP.EnterPhone;
                return false;
            }
            if (Winche.Whatsapp == "")
            {
                msg = Resources.CP.EnterWhatsapp;
                return false;
            }
            if (Winche.DriverName == "")
            {
                msg = Resources.CP.EnterDriverName;
                return false;
            }
            if (Winche.DriverPhone == "")
            {
                msg = Resources.CP.EnterDriverPhone;
                return false;
            }
            if (Winche.VehicleSize == "0")
            {
                msg = Resources.CP.EnterVehicleSizes;
                return false;
            }
            if (Winche.Area == "")
            {
                msg = Resources.CP.EnterAddress;
                return false;
            }
            if (Winche.Keywords == "")
            {
                msg = Resources.CP.EnterKeywords;
                return false;
            }
            if (Winche.Description == "")
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