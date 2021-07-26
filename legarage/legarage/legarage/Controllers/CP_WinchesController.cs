using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_WinchesController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Winches/GetAll/", Add = "/CP_Winches/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Winches/GetAll/", Adding = "/CP_Winches/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            DataRow user; Tools.FindCurrentUser(out user);
            string condetion = " ";
            if (Session["Is_Admin"].ToString() != "1")
            {
                condetion = " where user_id = '" + user["id"].ToString() + "' ";
            }

            string msg;
            DataTable winches = Database.ReadTable("Winches", condetion + " ORDER BY created_at ASC", null, out msg);
            string HTML_Content = "";
            if (winches != null && winches.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow winche in winches.Rows)
                {
                    string ID = winche["ID"].ToString();
                    HTML_Content += "<tr class=\"winches-row\">";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + winche["title"].ToString() + "</td>";
                    HTML_Content += "<td> " + winche["driver_name"].ToString() + "</td>";
                    HTML_Content += "<td class='telefon'> " + winche["driver_phone"].ToString() + " </td>";
                    HTML_Content += "<td class='telefon'> " + winche["mobile"].ToString() + " </td>";
                    HTML_Content += "<td> " + winche["vehiclesize"].ToString() + " </td>";
                    HTML_Content += "<td class='telefon'> " + winche["whatsapp"].ToString() + " </td>";
                    //Tools: 
                    HTML_Content += "<td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Winches.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Winches/Edit/')\"data-target=\"#Modal\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Winches.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Winches/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Winches.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Winches/Details/')\" data-target=\"#Modal\"></i>&nbsp&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Winches.Images + "\" style=\"color:SlateGrey; cursor:pointer;\" class=\"far fa-images\" data-toggle=\"modal\" onclick=\"Images('" + ID + "','/CP_Winches/Images/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_Winches.NoWinches + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            WinchesModel new_winche = new WinchesModel();
            new_winche.Title = Request.Params["Title"];
            new_winche.DriverName = Request.Params["DriverName"];
            new_winche.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            new_winche.Whatsapp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            new_winche.DriverPhone = Request.Params["DriverPhone"].ToString() != string.Empty && HelperClass.Phone(Request.Params["DriverPhone"]) ? "+" + Request.Params["DriverPhone_key"] + Request.Params["DriverPhone"] : null;
            new_winche.VehicleSize = Request.Params["vehiclesize"];
            new_winche.Area = Request.Params["area"];
            new_winche.Keywords = Request.Params["keywords"];
            new_winche.Description = Request.Params["description"];
            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                new_winche.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                new_winche.User = user;
            }
            else
            {
                new_winche.User = null;
            }
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

            if (ISValid(new_winche, true, out msg))
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
                    return Json(new { code = code.ToString(), msg = Resources.CP_Winches.Added });

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

        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            string sql = "";
            sql += " SELECT W.id,W.driver_name,W.driver_phone,W.vehiclesize,W.area,W.mobile,W.whatsapp ,W.description ,W.keywords,";
            sql += " I.url AS URL, W.title, U.full_name AS OwnerName,";
            sql += " (P.name + ' , ' + AD.details) AS Address";
            sql += " from Winches AS W";
            sql += " inner join Users AS U on W.user_id = U.id";
            sql += " inner join Addresses AS AD on AD.id = W.address_id";
            sql += " inner join Provinces AS P on AD.province_id = p.id";
            sql += " inner join Images AS I on W.id = I.referral_id ";
            sql += " Where W.id = @WID AND I.referral_type = 'Winches'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@WID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow Winche_ = dataTable.Rows[0];
                WinchesModel Winche = new WinchesModel();
                Winche.ID = new Guid(Winche_["ID"].ToString());
                Winche.Title = Winche_["Title"].ToString();
                Winche.Area = Winche_["Area"].ToString();
                Winche.DriverName = Winche_["driver_name"].ToString();
                Winche.DriverPhone = Winche_["driver_phone"].ToString();
                Winche.Keywords = Winche_["Keywords"].ToString();
                Winche.Mobile = Winche_["Mobile"].ToString();
                Winche.VehicleSize = Winche_["vehiclesize"].ToString();
                Winche.Whatsapp = Winche_["Whatsapp"].ToString();
                Winche.Description = Winche_["Description"].ToString();
                Winche.Address = new AddressModel();
                Winche.Address.AddressName = Winche_["Address"].ToString();
                Winche.Image = new ImagesModel()
                {
                    URL = Winche_["URL"].ToString()
                };
                UsersModel user = new UsersModel();
                user.UserName = Winche_["OwnerName"].ToString();
                Winche.User = user;

                return PartialView(Winche);
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public JsonResult RefreshImages(string ID)
        {
            string sql = "";
            sql += " SELECT W.id AS WID, I.url AS URL,I.id AS ID from Winches AS W";
            sql += " inner join Images AS I on W.id = I.referral_id ";
            sql += " Where W.id = @WID ";
            sql += " AND I.referral_type = 'Winches' AND I.is_main = 0";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@WID", ID));
            string msg = "";
            DataTable Images = Database.ReadTableByQuery(sql, li, out msg);
            string HTML_Content = "";
            if (Images != null)
            {
                foreach (DataRow Image in Images.Rows)
                {
                    HTML_Content += "<div class=\'py-3 border m-1\'>";
                    HTML_Content += "<img class=\"rounded w-50\" src=\"/Images/Winches/SubImages/" + Image["URL"] + " \"/>";
                    HTML_Content += "<button type=\"button\" class=\"btn btn-danger ml-5 mt-5\" style=\"margin: 14%;\" onclick =\"DeleteImage('" + Image["WID"] + "','" + Image["ID"] + "','/CP_Winches/DeleteImage/','/CP_Winches/RefreshImages/')\">";
                    HTML_Content += Resources.CP_Winches.DeleteImage;
                    HTML_Content += "</button>";
                    HTML_Content += "</div>";
                }
            }
            else
            {
                HTML_Content = Resources.CP_Winches.NoImage;
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult AddImage(string ID)
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            ImagesModel image = new ImagesModel();
            string errMessage = string.Empty;
            if (Session["Attachment"] != null)
            {
                Guid ImageID = Guid.NewGuid();
                string ImageName = "";
                byte[] b = (byte[])Session["Attachment"];
                string FileName = (string)Session["Attachment_File_Name"];
                FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                ImageName = FileName;
                FileName = Server.MapPath("~/Images/Winches/SubImages/" + FileName);
                System.IO.File.WriteAllBytes(FileName, b);
                List<string> cols = new List<string>();
                List<object> vals = new List<object>();
                string[] colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                cols.AddRange(colsinput);
                object[] valsinput = new object[] { ImageName, new Guid(ID), "Winches", DateTime.Now, 0 };
                vals.AddRange(valsinput);
                Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                Session["Attachment"] = null;
                Session["Attachment_File_Name"] = null;
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Winches.AddedImage });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        [HttpPost]
        public PartialViewResult Images(string ID)
        {
            string sql = "";
            sql += " SELECT W.id AS WID, I.url AS URL,I.id AS ID from Winches AS W";
            sql += " inner join Images AS I on W.id = I.referral_id ";
            sql += " Where W.id = @WID ";
            sql += " AND I.referral_type = 'Winches' AND I.is_main = 0";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@WID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            WinchesModel Winche = new WinchesModel();
            Winche.ID = new Guid(ID);

            Winche.URL = new URLModel
            {
                RefreshImages = "/CP_Winches/RefreshImages/",
                DeleteImage = "/CP_Winches/DeleteImage/",
                AddingImages = "/CP_Winches/AddImage/"
            };
            Winche.Images = new List<ImagesModel>();
            foreach (DataRow images in dataTable.Rows)
            {
                ImagesModel image = new ImagesModel();
                image.URL = images["URL"].ToString();
                image.ID = new Guid(images["ID"].ToString());
                Winche.Images.Add(image);
            };

            return PartialView(Winche);
        }

        [HttpPost]
        public JsonResult DeleteImage(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Images", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Winches.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }

        }

        [HttpPost]
        public PartialViewResult Edit(string ID)
        {
            string sql = "";
            sql += " select W.id ,I.url AS URL, W.title ,I.id  AS IID , U.full_name AS OwnerName,";
            sql += " AD.id AS AddressID, W.area,W.driver_name,W.driver_phone,W.vehiclesize,";
            sql += " AD.details AS AddressDetailes , P.id AS CityID,P.country_id AS CountryID, ";
            sql += " W.mobile ,W.whatsapp ,W.description,W.keywords";
            sql += " from Winches AS W";
            sql += " inner join Users AS U on W.user_id = U.id";
            sql += " inner join Addresses AS AD on AD.id = W.address_id";
            sql += " inner join Provinces AS P on AD.province_id = p.id";
            sql += " inner join Images AS I on W.id = I.referral_id ";
            sql += " Where W.id = @WID AND I.referral_type = 'Winches'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@WID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow Winche_ = dataTable.Rows[0];
                WinchesModel Winche = new WinchesModel();
                Winche.ID = new Guid(Winche_["ID"].ToString());
                Winche.Title = Winche_["Title"].ToString();
                Winche.Area = Winche_["Area"].ToString();
                Winche.DriverName = Winche_["driver_name"].ToString();
                Winche.DriverPhone = Winche_["driver_phone"].ToString();
                Winche.VehicleSize = Winche_["vehiclesize"].ToString();
                Winche.Whatsapp = Winche_["Whatsapp"].ToString();
                Winche.Mobile = Winche_["Mobile"].ToString();
                Winche.Keywords = Winche_["Keywords"].ToString();
                Winche.Description = Winche_["Description"].ToString();
                Winche.Image = new ImagesModel()
                {
                    ID = new Guid(Winche_["IID"].ToString()),
                    URL = Winche_["URL"].ToString()
                };
                UsersModel user = new UsersModel();
                user.UserName = Winche_["OwnerName"].ToString();
                Winche.User = user;

                Winche.Address = new AddressModel()
                {
                    AddressId = new Guid(Winche_["AddressID"].ToString()),
                    AddressName = Winche_["AddressDetailes"].ToString(),
                    ProvinceId = new Guid(Winche_["CityID"].ToString()),
                    CountryId = new Guid(Winche_["CountryID"].ToString())
                };

                Winche.URL = new URLModel
                {
                    Refresh = "/CP_Winches/GetAll/",
                    Edit = "/CP_Winches/Editing/"
                };
                return PartialView(Winche);
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            WinchesModel Winche = new WinchesModel();
            Winche.ID = new Guid(Request.Params["ID"].ToString());
            Winche.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            Winche.Whatsapp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            Winche.DriverPhone = Request.Params["DriverPhone"].ToString() != string.Empty && HelperClass.Phone(Request.Params["DriverPhone"]) ? "+" + Request.Params["DriverPhone_key"] + Request.Params["DriverPhone"] : null;
            Winche.Title = Request.Params["Title"];
            Winche.Description = Request.Params["Description"];
            Winche.DriverName = Request.Params["DriverName"];
            Winche.VehicleSize = Request.Params["VehicleSize"];
            Winche.Keywords = Request.Params["Keywords"];
            Winche.Area = Request.Params["Area"];

            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                Winche.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                Winche.User = user;
            }
            else
            {
                Winche.User = null;
            }
            Winche.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                Winche.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                Winche.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                Winche.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                Winche.Address.ProvinceId = new Guid();
            }

            if (ISValid(Winche, false, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "user_id", "title", "mobile", "area", "whatsapp", "driver_name", "driver_phone", "vehiclesize", "keywords", "description", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { Winche.User.ID, Winche.Title, Winche.Mobile, Winche.Area, Winche.Whatsapp, Winche.DriverName, Winche.DriverPhone, Winche.VehicleSize, Winche.Keywords, Winche.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Winches", Winche.ID, cols, vals, out errMessage))
                {
                    Database.DeleteRow("Addresses", Winche.Address.AddressId, out msg);
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "updated_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { Winche.Address.ProvinceId, Winche.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Winches", Winche.ID, cols, vals, out errMessage);
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = new Guid(Request.Params["IID"].ToString());
                        string ImageURL = Request.Params["image_url"] != null ? "/Images/Winches/" + Request.Params["image_url"] : "/Images/dafault.png";
                        System.IO.File.Delete(Server.MapPath("~" + ImageURL));
                        //Delete fromFiles
                        Database.DeleteRow("Images", ImageID, out msg);
                        //Delete Image from DB
                        ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Winches/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);

                        cols = new List<string>();
                        vals = new List<object>();

                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "updated_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, Winche.ID, "Winches", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Winches.Edited });
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



        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Winches", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Winches.DeletedImage});
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }


        bool ISValid(WinchesModel Winche, bool Is_Add, out string msg)
        {
            bool flag = true;
            if (Winche.Title == "")
            {
                msg = Resources.CP_Winches.EnterTitleP;
                return false;
            }
            if (Winche.User == null)
            {
                msg = Resources.CP_Winches.EnterUserP;
                return false;
            }
            if (Winche.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP_Winches.EnterCityP;
                return false;
            }
            if (Winche.Address.AddressName == "")
            {
                msg = Resources.CP_Winches.EnterAddressP;
                return false;
            }
            if (Winche.Mobile == null)
            {
                msg = Resources.CP_Winches.EnterPhoneP;
                return false;
            }
            if (Winche.Whatsapp == null)
            {
                msg = Resources.CP_Winches.EnterWhatsappP;
                return false;
            }
            if (Winche.DriverName == "")
            {
                msg = Resources.CP_Winches.EnterDriverNameP;
                return false;
            }
            if (Winche.DriverPhone == "")
            {
                msg = Resources.CP_Winches.EnterDriverPhoneP;
                return false;
            }
            if (Winche.VehicleSize == "0")
            {
                msg = Resources.CP_Winches.EnterVehicleSizesP;
                return false;
            }
            if (Winche.Area == "")
            {
                msg = Resources.CP_Winches.EnterAddressP;
                return false;
            }
            if (Winche.Keywords == "")
            {
                msg = Resources.CP_Winches.EnterKeywordsP;
                return false;
            }
            if (Winche.Description == "")
            {
                msg = Resources.CP_Winches.EnterDescrptionP;
                return false;
            }
            if (Session["Attachment"] == null && Is_Add)
            {
                msg = Resources.CP_Winches.AddImageP;
                return false;
            }
            msg = "";
            return flag;
        }

    }
}