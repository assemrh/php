//using legarage.Classes;
using static legarage.Classes.Database;
using static legarage.Classes.HelperClass;
using static legarage.Classes.Tools;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace legarage.Controllers
{
    public class OffersController : BaseController
    {
        // GET: Offer
        string msg;
        public ActionResult Index()
        {
            List<OffersModel> offers = new List<OffersModel>();
            DataTable table = ReadTable("Offers", out msg);
            if(table != null && table.Rows.Count > 0)
            {
                foreach(DataRow row in table.Rows)
                offers.Add(
                        new OffersModel()
                        {
                            id = new Guid(row["id"].ToString()),
                            name = row["name"].ToString(),
                            paymentmethods = row["paymentmethods"].ToString(),
                            description = row["description"].ToString(),
                        }
                    );
            }
            string market = Session["market"].ToString();
            var query = string.Format(" SELECT id, Name FROM Provinces WHERE " + ((market == "all") ? " 1=0 " : "  country_id= '{0}' "), market);
            var temp = ReadTableByQuery(query, null, out msg);
            if(temp != null && temp.Rows.Count >0)
            ViewData["citieslist"] = ConvertToList<CitiesModel>(temp);
            return View(offers);
        }

        [Route("Offers/Details")]
        [Route("Offers/OfferDetails")]
        public ActionResult OfferDetails()
        {
            if (Request.QueryString["id"] != null)
            {
                Guid id = new Guid(Request.QueryString["id"].ToString());

                DataRow row = GetRow("offers", id);
                OffersModel model = new OffersModel()
                {
                    name = row["name"].ToString(),
                    start_date = row["start_date"].ToString(),
                    end_date = row["end_date"].ToString(),
                    description = row["description"].ToString(),
                    discount_percentage = Convert.ToDouble(row["discount_percentage"].ToString()),
                    id = new Guid(row["id"].ToString()),
                    paymentmethods = row["paymentmethods"].ToString(),
                    referal_type = row["referal_type"].ToString(),
                    referal_id = (Guid)row["referal_id"],
                    address_id = (Guid)row["address_id"],
                    mobile = row["mobile"].ToString(),
                    website = row["website"].ToString(),
                    Is_Active = row["Is_Active"].ToString(),
                    Owner_Id = (Guid)row["Owner_Id"],

                };


                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = null, Adding = "/Offers/AddOffer/" });
        }


        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            var sucsess = Json(new { code = (200).ToString(), msg = "sucsess" });
            DataTable table = new DataTable();
            OffersModel model = new OffersModel();
            model.id = Guid.NewGuid();
            if(FindCurrentUser(out DataRow owner) && owner["is_admin"].ToString() != "1")
            {
                model.Owner_Id = new Guid(owner["id"].ToString());//في حال كان اليوزر مش آدمين استخدم  استخدم آي دي اليوزر 
            }
            else
            {
                model.Owner_Id = new Guid(Request.Params["owner"]?? owner["id"].ToString());//في حال كان اليوزر آدمين استخدم آي دي الاونر 
            }
            model.referal_type = Request.Params["Offer_type"] ?? "";
            model.referal_id = Request.Params["referal_id"] != null ? new Guid(Request.Params["referal_id"]) : new Guid();
            model.name = Request.Params["name"] ?? "";
            model.Is_Active = Request.Params["Is_Active"] ?? "1";
            model.description = Request.Params["desc"] ?? "";
            model.discount_percentage = Convert.ToDouble((Request.Params["discount_percentage"] ?? "0").ToString());
            model.paymentmethods = Request.Params["paymentmethod"] ?? "";
            model.mobile = Request.Params["phoneno"].ToString() != string.Empty && Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null; 
            model.website = Request.Params["site"] ?? "";
            model.address_id = new Guid(Request.Params["address_id"] ?? new Guid().ToString());
            model.start_date = (Request.Params["meeting-time-st"] ?? "").AsDateTime().ToString();
            model.end_date = (Request.Params["meeting-time-en"] ?? "").AsDateTime().ToString();
            List<string> referal_brands = (Request.Params["BrandsList"] ?? string.Empty).Split(',').ToList();
            List<string> referal_cates = (Request.Params["catesList"] ?? string.Empty).Split(',').ToList();
            List<string> referal_vts = (Request.Params["vtsList"] ?? string.Empty).Split(',').ToList();
            List<string> referal_models = (Request.Params["ModelsList"] ?? string.Empty).Split(',').ToList();
            List<string> referal_products = (Request.Params["ProductsList"] ?? string.Empty).Split(',').ToList();



            string errMessage = string.Empty;

            if (ISOfferValid(model, out msg)) //لازم تنحذف الترو بعد فترة التطوير
            {
                List<string> cols = new List<string> { "description", "referal_id", "referal_type", "start_date", "end_date", "discount_percentage", "created_at", "updated_at", "name", "paymentmethods", "address_id", "mobile", "website", "Is_Active", "Owner_Id" };
                List<Object> vals = new List<object> { model.description, model.referal_id, model.referal_type, model.start_date, model.end_date, model.discount_percentage, DateTime.Now, DateTime.Now, model.name, model.paymentmethods, model.address_id, model.mobile, model.website, model.Is_Active, model.Owner_Id };
                if (vals.Count == cols.Count)
                {
                    if (!InsertRow("Offers", model.id, cols, vals, out errMessage))
                    {
                        msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                        goto Error;
                    }
                    if (referal_brands[0] != String.Empty)
                    {
                        foreach (var item in referal_brands)
                        {
                            List<string> strli = new List<string>()
                            {
                                "brand_id","offer_id","created_at","updated_at"
                            };
                            List<object> objli = new List<object>()
                            {
                                new Guid(item),model.id,DateTime.Now,DateTime.Now
                            };
                            if (!InsertRow("Offers_Brands", Guid.NewGuid(), strli, objli, out errMessage))
                            {
                                msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                                goto Error;
                            }
                        }
                    }
                    if (referal_cates[0] != String.Empty)
                    {
                        foreach (var item in referal_cates)
                        {
                            List<string> strli = new List<string>()
                            {
                                "category_id","offer_id","created_at","updated_at"
                            };
                            List<object> objli = new List<object>()
                            {
                                new Guid(item),model.id,DateTime.Now,DateTime.Now
                            };
                            if (!InsertRow("Offers_Categories", Guid.NewGuid(), strli, objli, out errMessage))
                            {
                                msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                                goto Error;
                            }
                        }

                    }
                    if (referal_vts[0] != String.Empty)
                    {
                        foreach (var item in referal_vts)
                        {
                            List<string> strli = new List<string>()
                            {
                                "vehicle_type_id", "offer_id", "created_at", "updated_at"
                            };
                            List<object> objli = new List<object>()
                            {
                                new Guid(item),model.id,DateTime.Now,DateTime.Now
                            };
                            if (!InsertRow("Offers_Vehicle_Types", Guid.NewGuid(), strli, objli, out errMessage))
                            {
                                msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                                goto Error;
                            }
                        }

                    }
                    if (referal_models[0] != String.Empty)
                    {
                        foreach (var item in referal_models)
                        {
                            List<string> strli = new List<string>()
                            {
                                "model_id","offer_id","created_at","updated_at"
                            };
                            List<object> objli = new List<object>()
                            {
                                new Guid(item),model.id,DateTime.Now,DateTime.Now
                            };
                            if (!InsertRow("Offers_Models", Guid.NewGuid(), strli, objli, out errMessage))
                            {
                                msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                                goto Error;
                            }
                        }
                    }
                    if (referal_products[0] != String.Empty)
                    {
                        foreach (var item in referal_products)
                        {
                            List<string> listr = new List<string>()
                                {
                                   "Product_id"
                                    ,"offer_id"
                                    ,"created_at"
                                    ,"updated_at"
                                };
                            List<object> liobj = new List<object>()
                                {
                                    new Guid(item),
                                    model.id,
                                    DateTime.Now,
                                    DateTime.Now
                                };
                            if (!InsertRow("Offers_Products", Guid.NewGuid(), listr, liobj, out errMessage))
                            {
                                msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                                goto Error;
                            }
                            //InsertRow("Offers_Products", Guid.NewGuid(), listr, liobj, out errMessage);
                        }
                    }


                    return sucsess;
                }
                else
                {
                    msg = "error cols.count != vals.count";
                    goto Error;
                }
            }
            else
            {
                goto Error;
            }

            Error:
                return Json(new { code = (404).ToString(), msg = msg });
        }

        public ActionResult MyOffers()
        {
            if (FindCurrentUser(out DataRow userR))
            {
                var userId = userR["Id"].ToString();
                List<OffersModel> offersModels = getMyOffersList(userId);
                return View(offersModels);
            }
            else
            {
                return RedirectToAction("Index");
            }


        }

        private List<OffersModel> getMyOffersList(string userId)
        {
            
            List<OffersModel> offersModels = new List<OffersModel>();
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@userId", userId));
            String C = "";
            bool flag = true;
            if (!String.IsNullOrWhiteSpace(userId))
            {
                if (flag)
                {
                    C += " where Owner_Id  like @userId ";
                    flag = false;
                }
                else
                {
                    C += " and  Owner_Id  like @userId  ";
                }
                li.Add(new SqlParameter("@userId", userId));
            }

            DataTable table = ReadTable("Offers ", C, li, out msg);
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                    offersModels.Add(
                            new OffersModel()
                            {
                                id = new Guid(row["id"].ToString()),
                                name = row["name"].ToString(),
                                paymentmethods = row["paymentmethods"].ToString(),
                                description = row["description"].ToString(),
                                referal_type=row["referal_type"].ToString(),
                                referal_id =(Guid)row["referal_id"],
                                start_date = row["start_date"].ToString(),
                                end_date = row["end_date"].ToString(),
                                discount_percentage = (double) row["discount_percentage"],                         
                                address_id=(Guid)row["address_id"] ,
                                mobile = row["mobile"].ToString(),
                                website = row["website"].ToString(),
                                Is_Active = row["Is_Active"].ToString(),
                                Owner_Id = (Guid) row["Owner_Id"],
                                 
                            }
                        );
            }
            return offersModels;
        }

        public ActionResult Garage(string Owner) //
        {
            if (Owner == "select") return Json("Plaese select Owner ");

            DataRow user;
            if (FindCurrentUser(out user))
            {
                string msg;
                string sql = @"SELECT [id], [name]
                                  FROM [le-garage].[dbo].[Garages] ";
                //if (user["is_admin"].ToString() == "0")
                //    sql += " WHERE [user_id]= @uid";
                sql += " WHERE [user_id]= @uid";
                List<SqlParameter> li = new List<SqlParameter>();

                if (user["is_admin"].ToString() != "1") //if user isn't Admin
                {
                    li.Add(new SqlParameter("@uid", user["id"].ToString()));
                }
                else //if user is Admin enter Owner
                {
                    li.Add(new SqlParameter("@uid", Owner));
                }

                DataTable Garages = ReadTableByQuery(sql, li, out msg);
                List<GaragesModel> GaragesList = new List<GaragesModel>();
                GaragesList = new List<GaragesModel>();
                foreach (DataRow garage in Garages.Rows)
                {
                    GaragesList.Add(new GaragesModel()
                    {
                        ID = new Guid(garage["Id"].ToString()),
                        Name = garage["name"].ToString(),
                    });
                }

                return PartialView("Sections/GarageOffer", GaragesList);
            }
            return Json("You have to login "); 
        }
        public ActionResult RentOffice(string Owner) 
        {
            if (Owner == "select")return Json("Plaese select Owner ");
 
            DataRow user;
            if (FindCurrentUser(out user))
            {
                string msg;
                string sql = @" SELECT TOP (1000) [id]
                                      ,[name]
                                  FROM [le-garage].[dbo].[Rental_Offices] ";
                //if (user["is_admin"].ToString() == "0")
                //    sql += " WHERE [user_id]= @uid";
                sql += " WHERE [user_id]= @uid";
                List<SqlParameter> li = new List<SqlParameter>();

                if (user["is_admin"].ToString() != "1") //if user isn't Admin
                {
                    li.Add(new SqlParameter("@uid", user["id"].ToString()));
                }
                else //if user is Admin enter Owner
                {
                    li.Add(new SqlParameter("@uid", Owner));
                }

                DataTable rents = ReadTableByQuery(sql, li, out msg);
                List<RentOfficesModel> RentList = new List<RentOfficesModel>();
                RentList = new List<RentOfficesModel>();
                foreach (DataRow rent in rents.Rows)
                {
                    RentList.Add(new RentOfficesModel()
                    {
                        ID = new Guid(rent["Id"].ToString()),
                        Name = rent["name"].ToString(),
                    });
                }

                return PartialView("Sections/RentOfficeOffer", RentList);
            }
            return Json("You have to login ");
        }
        public ActionResult Vehicle(string Owner)
        {
            if (Owner == "select") return Json("Plaese select Owner ");

            DataRow user;
            if (FindCurrentUser(out user))
            {
                string msg;
                string sql = @"SELECT [id]
                              ,[description]
                              ,[price]
                              ,[model_id]
                              ,[store_id]
                              ,[vehicle_type_id]
                              ,[is_new]
                              ,[created_at]
                              ,[updated_at]
                              ,[mobile]
                              ,[quantity]
                              ,[keywords]
                              ,[owner_name]
                              ,[address_id]
                              ,[user_id]
                              ,[year]
                              ,[mieage]
                              ,[gearbox]
                              ,[engine_size]
                              ,[color]
                              ,[title]
                              ,[fuel_type]
                          FROM [le-garage].[dbo].[Vehicles]";
                    sql += " WHERE [user_id]= @uid";
                List<SqlParameter> li = new List<SqlParameter>();

                if (user["is_admin"].ToString() != "1") //if user isn't Admin
                {
                    li.Add(new SqlParameter("@uid", user["id"].ToString()));
                }
                else //if user is Admin enter Owner
                {
                    li.Add(new SqlParameter("@uid", Owner));
                }


                DataTable vehiclesTable = ReadTableByQuery(sql, li, out msg);
                List<VehiclesModel> vehicleList = new List<VehiclesModel>();
                foreach (DataRow row in vehiclesTable.Rows)
                {
                    vehicleList.Add(new VehiclesModel()
                    {
                        ID = new Guid(row["Id"].ToString()),
                        Title = row["Title"].ToString(),
                        Price = row["price"].ToString()
                    });
                }

                return PartialView("Sections/VehicleOffer", vehicleList);
            }
            return Json("You have to login ");
        }
        public ActionResult Part(string Owner)
        {
            if (Owner == "select") return Json("Plaese select Owner ");

            DataRow user;
            if (FindCurrentUser(out user))
            {
                List<SqlParameter> li = new List<SqlParameter>();
                string msg;
                string sql = @"SELECT P.id ID
                              ,title
                              ,I.url AS Img 
                          FROM Products AS P
                            INNER JOIN Images AS I on P.id = I.referral_id    ";

                //if (user["is_admin"].ToString() == "0")
                //{
                //    sql += " WHERE P.user_id= @uid";
                //    li.Add(new SqlParameter("@uid", user["id"].ToString()));
                //}
                sql += " WHERE [user_id]= @uid";

                if (user["is_admin"].ToString() != "1") //if user isn't Admin
                {
                    li.Add(new SqlParameter("@uid", user["id"].ToString()));
                }
                else //if user is Admin enter Owner
                {
                    li.Add(new SqlParameter("@uid", Owner));
                }

                DataTable parts = ReadTableByQuery(sql, li, out msg);
                List<ProductsModel> ProductsList = new List<ProductsModel>();
                foreach (DataRow part in parts.Rows)
                {
                    ProductsList.Add(new ProductsModel()
                    {
                        ID = new Guid(part["ID"].ToString()),
                        Title = part["Title"].ToString(),
                        Image = new ImagesModel() { URL = part["Img"].ToString() }
                    });
                }

                return PartialView("Sections/PartOffer", ProductsList);
            }
            return Json("You have to login ");
        }
        public ActionResult Winche(string Owner)
        {
            if (Owner == "select") return Json("Plaese select Owner ");

            DataRow user;
            if (FindCurrentUser(out user))
            {
                string msg;
                string sql = @"SELECT [id]
                              ,[driver_name]
                              ,[driver_phone]
                              ,[address_id]
                              ,[user_id]
                              ,[whatsapp]
                              ,[mobile]
                              ,[description]
                              ,[created_at]
                              ,[updated_at]
                              ,[keywords]
                              ,[title]
                              ,[vehiclesize]
                              ,[area]
                          FROM [le-garage].[dbo].[Winches]";
                //if (user["is_admin"].ToString() == "0")
                //    sql += " WHERE [user_id]= @uid";
                sql += " WHERE [user_id]= @uid";
                List<SqlParameter> li = new List<SqlParameter>();

                if (user["is_admin"].ToString() != "1") //if user isn't Admin
                {
                    li.Add(new SqlParameter("@uid", user["id"].ToString()));
                }
                else //if user is Admin enter Owner
                {
                    li.Add(new SqlParameter("@uid", Owner));
                }

                //List<SqlParameter> li = new List<SqlParameter>();
                //li.Add(new SqlParameter("@uid", user["id"].ToString()));

                DataTable winches = ReadTableByQuery(sql, li, out msg);
                List<WinchesModel> wincheList = new List<WinchesModel>();
                wincheList = new List<WinchesModel>();
                foreach (DataRow winche in winches.Rows)
                {
                    wincheList.Add(new WinchesModel()
                    {
                        ID = new Guid(winche["Id"].ToString()),
                        Title = winche["Title"].ToString(),
                    });
                }
                return PartialView("Sections/WincheOffer", wincheList);
            }
            return Json("You have to login ");
        }
        public ActionResult get_offers()
        {
            try
            {
                string type = Request.Params["type"] ?? string.Empty;
                string payment = Request.Params["payment"] ?? string.Empty;
                string city = Request.Params["city"] ?? string.Empty;
                string pmax = Request.Params["pmax"] ?? string.Empty;
                string pmin = Request.Params["pmin"] ?? string.Empty;
                DataTable rents = new DataTable();
                List<SqlParameter> li = new List<SqlParameter>();
                var msg = "";
                string sql_query = @"
                                        SELECT TOP (1000) [id]
                                                  ,[description]
                                                  ,[referal_id]
                                                  ,[referal_type]
                                                  ,[start_date]
                                                  ,[end_date]
                                                  ,[discount_percentage]
                                                  ,[name]
                                                  ,[paymentmethods]
                                                  ,[address_id]
                                                  ,[mobile]
                                                  ,[website]
                                                  ,[Is_Active]
                                                  ,[Owner_Id]
                                              FROM [le-garage].[dbo].[Offers] 
                                        ";
                bool flag = true;
                if (type != String.Empty && type != "-1")  
                {
                    if (flag)
                    {
                        sql_query += " where referal_type = @referal_type ";
                        flag = false;
                    }
                    else
                    {
                        sql_query += " and referal_type = @referal_type  ";
                    }
                    li.Add(new SqlParameter("@referal_type", type));
                }

                if (payment != String.Empty && payment != "-1")
                {
                    if (flag)
                    {
                        sql_query += " where paymentmethods = @paymentmethods ";
                        flag = false;
                    }
                    else
                    {
                        sql_query += " and paymentmethods = @paymentmethods  ";
                    }
                    li.Add(new SqlParameter("@paymentmethods", payment));
                }

                List<OffersModel> offers = new List<OffersModel>();

                DataTable table = ReadTable("Offers", out msg);
                if (table != null && table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                        offers.Add(
                                new OffersModel()
                                {
                                    id = new Guid(row["id"].ToString()),
                                    name = row["name"].ToString(),
                                    paymentmethods = row["paymentmethods"].ToString(),
                                    description = row["description"].ToString(),
                                }
                            );
                    ViewBag.ControllerName = "CP_Users";
                    return PartialView("content", offers);
                }

                return Content("<h3 class=\"ml-auto mr-auto\"> Data Not Found</h3>");
            }
            catch
            {
                return Content("<h3 class=\"ml-auto mr-auto\"> Data Not Found</h3>");
            }
        }

        private bool AddNewOffer(OffersModel model, out string msg)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult getGarageOfferOptions(string ID)
        {
            /*if garage not selected*/
            if (ID == "select") return Json(new
            {
                code = "200",
                brands = "",
                cate = "",
                VehicleTypes = "",
                msg = "please select garage"
            });

            Guid Id = new Guid(ID);
            DataTable brands = new DataTable();
            DataTable cate = new DataTable();
            DataTable vt = new DataTable();
            string msg = "", HTML_brands = "", HTML_cate = "", HTML_VT = "";
            List<SqlParameter> parameter = new List<SqlParameter>();
            String vtQuery = @"SELECT  vt.id, type_name FROM Vehicle_Types vt 
                              INNER JOIN Vehicle_Types_Garages vtg on vtg.vehicle_type_id = vt.id 
                              WHERE vtg.garage_id = @GID ";
            String cateQuery = @"SELECT  c.id, name  FROM Categories c 
                                INNER JOIN Garages_Categories gc ON gc.category_id = C.id
                                  WHERE gc.garage_id = @GID ";
            String brandQuery = @"    SELECT B.id , name  FROM Brands B
                              INNER JOIN Garages_Brands gB ON gB.brand_id = B.id
                                WHERE gB.garage_id = @GID  ";
            parameter.Add(new SqlParameter("@GID", Id));
            brands = ReadTableByQuery(brandQuery, parameter, out msg);
            parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@GID", Id));
            cate = ReadTableByQuery(cateQuery, parameter, out msg);
            parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@GID", Id));
            vt = ReadTableByQuery(vtQuery, parameter, out msg);
            foreach (DataRow row in brands.Rows)
            {
                HTML_brands += @"<input type='checkbox' name='BrandsList' value='" + row["id"].ToString() + @"' id='" + row["id"].ToString() + @"' />";
                HTML_brands += @"<label class='list-group-item d-inline-flex w-auto m-1' for='" + row["id"].ToString() + @"'>" + row["name"].ToString() + @"</label>";
            }
            foreach (DataRow row in cate.Rows)
            {
                HTML_cate += @"<input type='checkbox' name='catesList' value='" + row["id"].ToString() + @"' id='" + row["id"].ToString() + @"' />";
                HTML_cate += @"<label class='list-group-item d-inline-flex w-auto m-1' for='" + row["id"].ToString() + @"'>" + row["name"].ToString() + @"</label>";
            }
            foreach (DataRow row in vt.Rows)
            {
                HTML_VT += @"<input type='checkbox' name='vtsList' value='" + row["id"].ToString() + @"' id='" + row["id"].ToString() + @"' />";
                HTML_VT += @"<label class='list-group-item d-inline-flex w-auto m-1' for='" + row["id"].ToString() + @"'>" + row["type_name"].ToString() + @"</label>";
            }
            //Brands =
            return Json(new
            {
                code = "200",
                brands = HTML_brands,
                cate = HTML_cate,
                VehicleTypes = HTML_VT,
                msg = msg
            });
        }
        [HttpPost]
        public ActionResult getOfficeOfferOptions(string ID)
        {
            /*if garage not selected*/
            if (ID == "select") return Json(new
            {
                code = "200",
                brands = "",
                VehicleTypes = "",
                msg = "please select Office"
            });

            Guid Id = new Guid(ID);
            DataTable models = new DataTable();
            DataTable vt = new DataTable();
            string msg = "", HTML_models = "", HTML_VT = "";
            List<SqlParameter> parameter = new List<SqlParameter>();
            String vtQuery = @" SELECT vt.id, type_name
                                  FROM Vehicle_Types vt 
                                  INNER JOIN Vehicle_Types_Rental_Offices vtr on vtr.vehicle_type_id = vt.id 
                                  WHERE vtr.rental_office_id = @RID  ";
            String modelsQuery = @" SELECT M.id ,name  FROM Models M
                                      INNER JOIN Rental_Offices_Models Rm ON rm.model_id = m.id
                                        WHERE Rm.rental_office_id = @RID  ";
            parameter.Add(new SqlParameter("@RID", Id));
            models = ReadTableByQuery(modelsQuery, parameter, out msg);
            parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@RID", Id));
            vt = ReadTableByQuery(vtQuery, parameter, out msg);
            foreach (DataRow row in models.Rows)
            {
                HTML_models += @"<input type='checkbox' name='ModelsList' value='" + row["id"].ToString() + @"' id='" + row["id"].ToString() + @"' />";
                HTML_models += @"<label class='list-group-item d-inline-flex w-auto m-1' for='" + row["id"].ToString() + @"'>" + row["name"].ToString() + @"</label>";
            }

            foreach (DataRow row in vt.Rows)
            {
                HTML_VT += @"<input type='checkbox' name='vtsList' value='" + row["id"].ToString() + @"' id='" + row["id"].ToString() + @"' />";
                HTML_VT += @"<label class='list-group-item d-inline-flex w-auto m-1' for='" + row["id"].ToString() + @"'>" + row["type_name"].ToString() + @"</label>";
            }
            //Brands =
            return Json(new
            {
                code = "200",
                models = HTML_models,
                VehicleTypes = HTML_VT,
                msg = msg
            });
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
            if (offer.referal_id == null || offer.referal_id == new Guid())
            {
                msg = "wrong in offer referal";
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


        //[HttpPost]
        //public JsonResult NewOfferGarage()
        //{
        //    string msg=""; 
        //    //bool flag =true;
        //    var sucsess = Json(new { code = 200.ToString(), msg = "sucsess" });
        //    //var erorr = Json(new { code = 404.ToString(), msg = msg });
        //    DataTable table = new DataTable();
        //    OffersModel model = new OffersModel();
        //    model.id = Guid.NewGuid();
        //    model.referal_type = Request.Params["Offer_type"] ?? "";
        //    model.referal_id = Request.Params["referal_id"] != null ? new Guid(Request.Params["referal_id"]) : new Guid();
        //    //model.referal_id = new Guid(Request.Params["select_winches"] ?? (new Guid()).ToString());
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
        //    List<string> referal_brands = (Request.Params["BrandsList"] ?? string.Empty).Split(',').ToList();
        //    List<string> referal_cates = (Request.Params["catesList"] ?? string.Empty).Split(',').ToList();
        //    List<string> referal_vts = (Request.Params["vtsList"] ?? string.Empty).Split(',').ToList();
        //    string errMessage = string.Empty;

        //    if (true || ISOfferValid(model, out msg))
        //    {
        //        List<string> cols = new List<string> { "description", "referal_id", "referal_type", "start_date", "end_date", "discount_percentage", "created_at", "updated_at", "name", "paymentmethods", "address_id", "mobile", "website", "Is_Active" };
        //        List<Object> vals = new List<object> { model.description, model.referal_id, model.referal_type, model.start_date, model.end_date, model.discount_percentage, DateTime.Now, DateTime.Now, model.name, model.paymentmethods, model.address_id, model.mobile, model.website, model.Is_Active, };
        //        if (vals.Count == cols.Count)
        //        {
        //            if (!InsertRow("Offers", Guid.NewGuid(), cols, vals, out errMessage))
        //            {
        //                msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
        //                goto Error;
        //            }

        //            foreach (var item in referal_brands)
        //            {
        //                if (item != string.Empty)
        //                {
        //                        List<string> strli = new List<string>()
        //                    {
        //                        "brand_id","offer_id","created_at","updated_at"
        //                    };
        //                    List<object> objli = new List<object>()
        //                    {
        //                        new Guid(item),model.id,DateTime.Now,DateTime.Now
        //                    };
        //                    if (!InsertRow("Offers_Brands", Guid.NewGuid(), strli, objli, out errMessage))
        //                    {
        //                        msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
        //                        goto Error;
        //                    }
        //                }

        //            }
        //            foreach (var item in referal_cates)
        //            {
        //                if (item != string.Empty)
        //                {
        //                        List<string> strli = new List<string>()
        //                    {
        //                        "category_id","offer_id","created_at","updated_at"
        //                    };
        //                    List<object> objli = new List<object>()
        //                    {
        //                        new Guid(item),model.id,DateTime.Now,DateTime.Now
        //                    };
        //                    if (!InsertRow("Offers_Categories", Guid.NewGuid(), strli, objli, out errMessage))
        //                    {
        //                        msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
        //                        goto Error;
        //                    }
        //                }

        //            }
        //            foreach (var item in referal_vts)
        //            {
        //                if(item != string.Empty)
        //                {
        //                    List<string> strli = new List<string>()
        //                    {
        //                        "vehicle_type_id", "offer_id", "created_at", "updated_at"
        //                    };
        //                    List<object> objli = new List<object>()
        //                    {
        //                        new Guid(item),model.id,DateTime.Now,DateTime.Now
        //                    };
        //                    if (!InsertRow("Offers_Vehicle_Types", Guid.NewGuid(), strli, objli, out errMessage))
        //                    {
        //                        msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
        //                        goto Error; 
        //                    }
        //                }

        //            }
        //            return sucsess;
        //        }
        //        else
        //        {
        //            msg = "error cols.count != vals.count";
        //            goto Error;
        //        }
        //    }
        //    else
        //    {
        //        goto Error;
        //    }

        //Error:
        //    return Json(new { code = 404.ToString(), msg = msg });
        //}
        //[HttpPost]
        //public JsonResult NewOfferRentOffice()
        //{
        //    string msg = "";
        //    var sucsess = Json(new { code = 200.ToString(), msg = "sucsess" });
        //    DataTable table = new DataTable();
        //    OffersModel model = new OffersModel();
        //    model.id = Guid.NewGuid();
        //    model.referal_type = Request.Params["Offer_type"] ?? "";
        //    model.referal_id = Request.Params["referal_id"] != null ? new Guid(Request.Params["referal_id"]) : new Guid();
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
        //    //List<string> referal_brands = (Request.Params["BrandsList"] ?? string.Empty).Split(',').ToList();
        //    List<string> referal_models = (Request.Params["ModelsList"] ?? string.Empty).Split(',').ToList();
        //    List<string> referal_vts = (Request.Params["vtsList"] ?? string.Empty).Split(',').ToList();
        //    string errMessage = string.Empty;

        //    if (true || ISOfferValid(model, out msg))
        //    {
        //        List<string> cols = new List<string> { "description", "referal_id", "referal_type", "start_date", "end_date", "discount_percentage", "created_at", "updated_at", "name", "paymentmethods", "address_id", "mobile", "website", "Is_Active" };
        //        List<Object> vals = new List<object> { model.description, model.referal_id, model.referal_type, model.start_date, model.end_date, model.discount_percentage, DateTime.Now, DateTime.Now, model.name, model.paymentmethods, model.address_id, model.mobile, model.website, model.Is_Active, };
        //        if (vals.Count == cols.Count)
        //        {
        //            if (!InsertRow("Offers", Guid.NewGuid(), cols, vals, out errMessage))
        //            {
        //                msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
        //                goto Error;
        //            }
        //            if(referal_models != null && referal_models.Count > 0 )
        //            foreach (var item in referal_models)
        //            {
        //                List<string> strli = new List<string>()
        //                {
        //                    "model_id","offer_id","created_at","updated_at"
        //                };
        //                List<object> objli = new List<object>()
        //                {
        //                    new Guid(item),model.id,DateTime.Now,DateTime.Now
        //                };
        //                if (!InsertRow("Offers_Models", Guid.NewGuid(), strli, objli, out errMessage))
        //                {
        //                    msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
        //                    goto Error;
        //                }
        //            }
        //            foreach (var item in referal_vts)
        //            {
        //                List<string> strli = new List<string>()
        //                {
        //                    "vehicle_type_id", "offer_id", "created_at", "updated_at"
        //                };
        //                List<object> objli = new List<object>()
        //                {
        //                    new Guid(item),model.id,DateTime.Now,DateTime.Now
        //                };
        //                if (!InsertRow("Vehicle_Types_Offers", Guid.NewGuid(), strli, objli, out errMessage))
        //                {
        //                    msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
        //                    goto Error;
        //                }
        //            }
        //            return sucsess;
        //        }
        //        else
        //        {
        //            msg = "error cols.count != vals.count";
        //            goto Error;
        //        }
        //    }
        //    else
        //    {
        //        goto Error;
        //    }

        //Error:
        //    return Json(new { code = 404.ToString(), msg = msg });
        //}
        //[HttpPost]
        //public JsonResult NewOfferVehicle()
        //{
        //    string msg;
        //    DataTable table = new DataTable();
        //    OffersModel model = new OffersModel();
        //    model.id = Guid.NewGuid();
        //    model.referal_id = Request.Params["select_vehicle"] != null ? new Guid(Request.Params["select_vehicle"]) : new Guid();
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
        //            if (InsertRow("Offers", Guid.NewGuid(), cols, vals, out errMessage))
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
        //[HttpPost]
        //public JsonResult NewOfferPart()
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
        //            if (InsertRow("Offers", Guid.NewGuid(), cols, vals, out errMessage))
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
        //                    InsertRow("Offers_Products", Guid.NewGuid(), listr, liobj, out errMessage);
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
        //[HttpPost]
        //public JsonResult NewOfferWinche()
        //{
            
        //    string msg;
        //    DataTable table = new DataTable();
        //    OffersModel model = new OffersModel();
        //    model.id = Guid.NewGuid();
        //    //model.referal_id = Request.Params["id"]!= null? new Guid(Request.Params["id"]): new Guid();
        //    model.referal_id = new Guid(Request.Params["select_winches"] ?? new Guid().ToString());
        //    model.referal_type = Request.Params["Offer_type"] ?? "";

        //    model.name = Request.Params["name"] ?? "";
        //    model.Is_Active = Request.Params["Is_Active"] ?? "1";
        //    model.description = Request.Params["desc"] ?? "";
        //    model.discount_percentage = Convert.ToDouble(Request.Params["discount_percentage"].ToString() ?? "0");
        //    //float.Parse(Request.Params["paymentmethod"].ToString() ?? "0", CultureInfo.InvariantCulture.NumberFormat);
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
        //            if (InsertRow("Offers", Guid.NewGuid(), cols, vals, out errMessage))
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
    }
}