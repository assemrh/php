using legarage.Classes;
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
    public class CP_OffersController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Offers/GetAllOffers/", Add = "/CP_Offers/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Offers/GetAllOffers/", Adding = "/Offers/Adding/" });
        }


        [HttpPost]
        public ActionResult Edit(string ID)
        {
            OffersModel offer = getModel(ID);
            if (offer != null)
            {
                offer.URL = new URLModel
                {
                    Refresh = "/CP_Offers/GetAll/",
                    Edit = "/CP_Offers/Editing/"
                };
                return PartialView(offer);
                //return PartialView(new URLModel { Refresh = "/CP_Offers/GetAllOffers/", Adding = "/CP_Offers/Edit/" });
            }
            else
            {
                return Json("no data ");
            }
        }

        public JsonResult Editing()
        {
            string msg = "";
            var sucsess = Json(new { code = 200, msg = "sucsess" });
            DataTable table = new DataTable();
            OffersModel model = new OffersModel();
            model.id = Guid.NewGuid();
            if (legarage.Classes.Tools.FindCurrentUser(out DataRow owner) && owner["is_admin"].ToString() != "1")
            {
                model.Owner_Id = new Guid(owner["id"].ToString());//في حال كان اليوزر مو آدمين استخدم  استخدم آي دي اليوزر 
            }
            else
            {
                model.Owner_Id = new Guid(Request.Params["owner"] ?? owner["id"].ToString());//في حال كان اليوزر آدمين استخدم آي دي الاونر في حال الاونر نال استخدم آي دي اليوزر يلي هو الآدمين
            }
            model.referal_type = Request.Params["Offer_type"] ?? "";
            model.referal_id = Request.Params["referal_id"] != null ? new Guid(Request.Params["referal_id"]) : new Guid();
            model.name = Request.Params["name"] ?? "";
            model.Is_Active = Request.Params["Is_Active"] ?? "1";
            model.description = Request.Params["desc"] ?? "";
            model.discount_percentage = Convert.ToDouble((Request.Params["discount_percentage"] ?? "0").ToString());
            model.paymentmethods = Request.Params["paymentmethod"] ?? "";
            model.mobile = Request.Params["phonenum"] ?? "";
            model.website = Request.Params["site"] ?? "";
            model.address_id = new Guid(Request.Params["address_id"] ?? new Guid().ToString());
            model.start_date = (Request.Params["meeting-time-st"] ?? "").AsDateTime().ToString();
            model.end_date = (Request.Params["meeting-time-en"] ?? "").AsDateTime().ToString();
            List<string> referal_brands = (Request.Params["BrandsList"] ?? string.Empty).Split(',').ToList();
            List<string> referal_cates = (Request.Params["catesList"] ?? string.Empty).Split(',').ToList();
            List<string> referal_vts = (Request.Params["vtsList"] ?? string.Empty).Split(',').ToList();
            List<string> referal_models = (Request.Params["ModelsList"] ?? string.Empty).Split(',').ToList();


            string errMessage = string.Empty;

            if (ISOfferValid(model, out msg)) 
            {
                List<string> cols = new List<string> { "description", "referal_id", "referal_type", "start_date", "end_date", "discount_percentage", "created_at", "updated_at", "name", "paymentmethods", "address_id", "mobile", "website", "Is_Active", "Owner_Id" };
                List<Object> vals = new List<object> { model.description, model.referal_id, model.referal_type, model.start_date, model.end_date, model.discount_percentage, DateTime.Now, DateTime.Now, model.name, model.paymentmethods, model.address_id, model.mobile, model.website, model.Is_Active, model.Owner_Id };
                if (vals.Count == cols.Count)
                {
                    if (!Database.InsertRow("Offers", model.id, cols, vals, out errMessage))
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
                            if (!Database.InsertRow("Offers_Brands", Guid.NewGuid(), strli, objli, out errMessage))
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
                            if (!Database.InsertRow("Offers_Categories", Guid.NewGuid(), strli, objli, out errMessage))
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
                            if (!Database.InsertRow("Offers_Vehicle_Types", Guid.NewGuid(), strli, objli, out errMessage))
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
                            if (!Database.InsertRow("Offers_Models", Guid.NewGuid(), strli, objli, out errMessage))
                            {
                                msg = "<br/>" + "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                                goto Error;
                            }
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
            return Json(new { code = 404, msg = msg });
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Offers", new Guid(ID), out msg))
            {
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@OID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Offers_Brands Where offer_id like @OID ", li, out msg);
                }
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@OID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Offers_Categories Where offer_id like @OID ", li, out msg);
                }
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@OID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Offers_Models Where offer_id like @OID ", li, out msg);
                }
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@OID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Offers_Products Where offer_id like @OID ", li, out msg);
                }
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@OID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Offers_Vehicle_Types Where offer_id like @OID ", li, out msg);
                }
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Garages.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        [HttpPost]
        public ActionResult Details(string ID)
        {
            OffersModel offers = getModel(ID);
            if (offers != null)
            {
                return PartialView(offers);
            }
            else
            {
                return Json("no data ");
            }
        }

        [HttpPost]
        public JsonResult GetAllOffers()
        {
            string msg;
            DataTable offers = Database.ReadTable("Offers", "ORDER BY created_at ASC", null, out msg);
            string HTML_Content = "";
            if (offers != null && offers.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow offer in offers.Rows)
                {
                    HTML_Content += @"<tr class='offer-row'>
                                       <th scope='row' >" + (++count) + @" </th>
                                        <td> " + offer["name"].ToString() + @"</td>
                                        <td> " + offer["referal_type"].ToString() + @"</td>
                                        <td> " + offer["Owner_Id"].ToString() + @"</td>
                                        <td> " + offer["discount_percentage"].ToString() + @" </td>
                                        <td> " + offer["start_date"].ToString() + @" </td>
                                        <td> " + offer["end_date"].ToString() + @" </td>
                                         <td>";
                                    //    "<i title = '" + Resources.CP.Edit + "' style = 'color:darkcyan; cursor:pointer;' class='fas fa-file-alt'data-toggle= 'modal' data-target = '#OfferModal' ></i>&nbsp"+
                                    //    "<i title = '" + Resources.CP.Delete + "' style ='color:red; cursor:pointer;' class='fas fa-trash' onclick='Delete('" + offer["Id"].ToString() + "','/CP_Offers/Delete/');'></i>&nbsp" +
                                    //    "<i title = '" + Resources.CP.Details +  "' onclick=\"Details('" + offer["Id"].ToString() + "', '/CP_Offers/Details/');\" " +@" style ='color:lawngreen; cursor:pointer;' class='fas fa-table' data-toggle='modal' data-target='#OfferModal'></i>
                                    //     </td>
                                    //</tr> ";
                    HTML_Content += "<i title = \"" + Resources.CP_Garages.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + offer["Id"] + "','/CP_Offers/Edit/')\"data-target=\"#Modal\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Garages.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + offer["Id"] + "','/CP_Offers/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Garages.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + offer["Id"] + "','/CP_Offers/Details/')\" data-target=\"#Modal\"></i>&nbsp&nbsp";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";

                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP.NoOffers + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult AddOffer()
        {
            DataTable table = new DataTable();
            OffersModel model = new OffersModel();
            model.id = new Guid();
            model.referal_id = new Guid(Request.Params["id"]);
            model.referal_type = "";
            model.start_date = "";
            model.end_date = "";
            model.Is_Active = "1";
            model.Adding = "";
            model.description = "";
            model.discount_percentage = 0;
            model.name = "";
            model.paymentmethods = "";
            model.mobile = "";
            string msg = "";
            int code = 0;

            Session["error"] = null;
            OffersModel new_offer = new OffersModel();
            new_offer.name = Request.Params["name"] != null ? Request.Params["name"] : "";
            //add address id and user id
            //new_garage.website = Request.Params["email"] != null ? Request.Params["email"] : "";
            new_offer.mobile = Request.Params["phonenum"];
            new_offer.description = Request.Params["desc"] != null ? Request.Params["desc"] : "";
            //new_offer.details = Request.Params["details"] != null ? Request.Params["details"] : "";
            new_offer.website = Request.Params["site"];
            new_offer.address_id = new Guid();
            new_offer.referal_type = Request.Params["by"] != null ? Request.Params["by"] : "";
            new_offer.referal_id = new Guid();

            //new_winche.service_price = Request.Params["price"] != null ? Request.Params["price"] : "";
            //new_offer.old_price = Request.Params["oldprice"] != null ? Request.Params["oldprice"] : "";
//            new_offer.offer_price = Request.Params["price"] != null ? Request.Params["price"] : "";

            new_offer.start_date = Request.Params["meeting-time-st"].AsDateTime().ToString() != null ? Request.Params["meeting-time-st"].AsDateTime().ToString() : "";
            new_offer.end_date = Request.Params["meeting-time-en"].AsDateTime().ToString() != null ? Request.Params["meeting-time-en"].AsDateTime().ToString() : "";

            new_offer.paymentmethods = Request.Params["paymentmethod"] != null ? Request.Params["paymentmethod"] : "";

            /*new_offer.whatsapp = Request.Params["whatsapp"] != null ? Request.Params["whatsapp"] : "";
            new_offer.facebook = Request.Params["facebook"] != null ? Request.Params["facebook"] : "";
            new_offer.tiktok = Request.Params["tiktok"] != null ? Request.Params["tiktok"] : "";
            new_offer.snapchat = Request.Params["snapchat"] != null ? Request.Params["snapchat"] : "";
            new_offer.twitter = Request.Params["twitter"] != null ? Request.Params["twitter"] : "";
            new_offer.instagram = Request.Params["instagram"] != null ? Request.Params["instagram"] : "";
            new_offer.linkedin = Request.Params["linkedin"] != null ? Request.Params["linkedin"] : "";
            new_offer.youtube = Request.Params["youtube"] != null ? Request.Params["youtube"] : "";*/
            //TODO: must add address and image before addind the user later
            if (ISOfferValid(new_offer, out msg))
            {
                //string Token = HelperClass.RandomString(50);
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "name", "mobile", "address_id", "referal_id", "referal_type", "start_date", "end_date", "website", "old_price", "discount_percentage", "paymentmethods", "details", "description", "created_at", "updated_at" };
                cols.AddRange(colsinput);

                object[] valsinput = { new_offer.name, new_offer.mobile, new_offer.address_id, new_offer.referal_id, new_offer.referal_type, new_offer.start_date, new_offer.end_date,
                    new_offer.website, new_offer.paymentmethods, new_offer.description, DateTime.Now, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.InsertRow("Offers", Guid.NewGuid(), cols, vals, out errMessage))
                {
                    code = 200;
                    msg = "sucsess";
                    return Json(new { code = code.ToString(), msg = msg });
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
            if (offer.mobile == "")
            {
                msg = "wrong in offer phone";
                return false;
            }
            if(offer.referal_id== new Guid())
            {
                msg = "يجب تحديد منتج";
                return false;
            }
            //TODO:
            //if (Mobile == "" || !HelperClass.Phone_Number_Sanituzer(ref Mobile))
            //{
            //    code = 404;
            //    msg = Resources.Registration.valid_phone;
            //    return Json(new { @code = code.ToString(), msg = msg });
            //}
            msg = "";
            return flag;
        }

        OffersModel getModel(string ID)
        {
            string msg = "";
            string query = @"SELECT TOP (1000) O.id AS id
                                  ,O.description
                                  ,referal_id
                                  ,referal_type
                                  ,start_date
                                  ,end_date
                                  ,discount_percentage
                                  ,O.created_at
                                  ,O.updated_at
                                  ,name
                                  ,paymentmethods
                                  ,O.address_id
                                  ,mobile
                                  ,O.website
                                  ,Is_Active
                                  ,O.Owner_Id
	                              ,U.full_name AS Owner_Name
                                FROM Offers O
	                            LEFT JOIN Users U ON U.id = O.Owner_Id 

                                Where O.id like @OID ";

            List<SqlParameter> parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@OID", ID));
            OffersModel model = new OffersModel();

            DataTable dataTable = Database.ReadTableByQuery(query, parameter, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                model.id = new Guid(row["id"].ToString());
                model.Is_Active = row["Is_Active"].ToString();
                model.mobile = row["mobile"].ToString();
                model.name = row["name"].ToString();
                model.paymentmethods = row["paymentmethods"].ToString();
                model.referal_type = row["referal_type"].ToString();
                model.referal_id = new Guid (row["referal_id"].ToString());
                model.discount_percentage = Convert.ToDouble(row["discount_percentage"].ToString());
                model.description = row["description"].ToString();
                model.Owner_Id = new Guid(row["Owner_Id"].ToString());
                model.start_date = Convert.ToDateTime(row["start_date"]).ToString("yyyy-MM-ddThh:mm");
                model.end_date = Convert.ToDateTime(row["end_date"]).ToString("yyyy-MM-ddThh:mm");
                model.website = row["website"].ToString();
            }
             query = "SELECT id, type_name FROM Vehicle_Types where id in (select vehicle_type_id from [Offers_Vehicle_Types] where offer_id like @OID )";
            parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@OID", ID));
            DataTable VTable = Database.ReadTableByQuery(query, parameter, out msg);
            model.VehicleTypes = new List<VehicleTypesModel>();
            if (VTable != null && VTable.Rows.Count > 0)
            {
                foreach(DataRow row in VTable.Rows)
                {
                    model.VehicleTypes.Add(new VehicleTypesModel() { ID = new Guid(row["id"].ToString()), Name = row["type_name"].ToString()});
                }
            }

            query = "SELECT id, name FROM Brands  where id in (select brand_id from Offers_Brands where offer_id like  @OID)";
            parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@OID", ID));
            DataTable BTable = Database.ReadTableByQuery(query, parameter, out msg);
            model.Brands = new List<BrandsModel>();
            if (BTable != null && BTable.Rows.Count > 0)
            {
                foreach (DataRow row in BTable.Rows)
                {
                    model.Brands.Add(new BrandsModel() { ID = new Guid(row["id"].ToString()), Name = row["name"].ToString() });
                }

            }

            query = "SELECT id, [name] FROM [Categories] where id in (SELECT [category_id]  FROM [Offers_Categories] WHERE offer_id like @OID) ";
            parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@OID", ID));
            DataTable CTable = Database.ReadTableByQuery(query, parameter, out msg);
             model.Categories = new List<ServicesModel>();
            if (CTable != null && CTable.Rows.Count > 0)
            {
                foreach (DataRow row in CTable.Rows)
                {
                    model.Categories.Add(new ServicesModel() { ID = new Guid(row["id"].ToString()), Name = row["name"].ToString() });
                }
            }

            query = "SELECT id, [name] FROM [Models] where id in (SELECT [model_id] FROM [Offers_Models] WHERE offer_id like @OID) ";
            parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@OID", ID));
            dataTable = Database.ReadTableByQuery(query, parameter, out msg);
            model.Models = new List<ModelsModel>();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    model.Models.Add(new ModelsModel() { ID = new Guid(row["id"].ToString()), Name = row["name"].ToString() });
                }
            }
            return model;
        }

    }
}