using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.WebPages;

namespace legarage.Controllers
{
    public class CP_VehiclesController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Vehicles/GetAll/", Add = "/CP_Vehicles/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Vehicles/GetAll/", Adding = "/CP_Vehicles/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            DataRow user; Tools.FindCurrentUser(out user);
            string condetion = " ";
            if (Session["Is_Admin"].ToString() != "1")
            {
                condetion = " where U.id = '" + user["id"].ToString() + "' ";
            }
            string sql = "";
            sql += " select V.id AS ID, V.title,V.is_new,V.color,V.engine_size,V.fuel_type,V.gearbox,V.mieage,V.year,V.price,";
            sql += " VT.type_name AS vehicle_type, M.name AS model from Vehicles AS V";
            sql += " inner join Vehicle_Types AS VT on V.vehicle_type_id = VT.id";
            sql += " inner join Models AS M on M.id = V.model_id ";
            sql += " inner join Users AS U on U.id = V.user_id" + condetion + " ORDER BY V.created_at ASC";
            string msg;
            DataTable vehicles = Database.ReadTableByQuery(sql, null, out msg);
            string HTML_Content = "";
            if (vehicles != null && vehicles.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow vehicle in vehicles.Rows)
                {
                    string IsNew = "";
                    IsNew = vehicle["is_new"].ToString() == "0" ? Resources.CP_Vehicles.Old : Resources.CP_Vehicles.New;
                    string GB = "";
                    GB = vehicle["gearbox"].ToString() == "0" ? Resources.CP_Vehicles.Automatic : Resources.CP_Vehicles.Manual;
                    string FT = "";
                    FT = vehicle["fuel_type"].ToString() == "0" ? Resources.CP_Vehicles.Petrol : Resources.CP_Vehicles.Diesel;
                    string Mieage = "";
                    if (vehicle["mieage"].ToString() == "0")
                    {
                        Mieage = Resources.CP.New;
                    }
                    else
                    {
                        if (vehicle["mieage"].ToString() == "1")
                        {
                            Mieage = Resources.CP.Less50;
                        }
                        else
                        {
                            if (vehicle["mieage"].ToString() == "2")
                            {
                                Mieage = Resources.CP_Vehicles.Less100;
                            }
                            else
                            {
                                if (vehicle["mieage"].ToString() == "3")
                                {
                                    Mieage = Resources.CP_Vehicles.Less150;
                                }
                                else
                                {
                                    Mieage = Resources.CP_Vehicles.More150;
                                }
                            }
                        }
                    }
                    string ID = vehicle["ID"].ToString();
                    HTML_Content += "<tr class=\"vehicle-row \">";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + vehicle["title"].ToString() + " </td>";
                    HTML_Content += "<td> " + IsNew + "</td>";
                    HTML_Content += "<td> " + vehicle["color"].ToString() + " </td>";
                    HTML_Content += "<td> " + vehicle["engine_size"].ToString() + " </td>";
                    HTML_Content += "<td> " + FT + "</td>";
                    HTML_Content += "<td> " + GB + "</td>";
                    HTML_Content += "<td> " + Mieage + "</td>";
                    HTML_Content += "<td> " + vehicle["year"].ToString() + "</td>";
                    HTML_Content += "<td> " + vehicle["model"].ToString() + " </td>";
                    HTML_Content += "<td> " + vehicle["vehicle_type"].ToString() + " </td>";
                    HTML_Content += "<td> " + vehicle["price"].ToString() + " </td>";
                    //Tools: 
                    HTML_Content += " <td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Vehicles.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\" onclick=\"Edit('" + ID + "','/CP_Vehicles/Edit/')\"data-toggle= \"modal\" data-target = \"#Modal\" ></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Vehicles.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Vehicles/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Vehicles.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\"onclick=\"Details('" + ID + "','/CP_Vehicles/Details/')\" data-toggle=\"modal\" data-target=\"#Modal\"></i>&nbsp&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Vehicles.Images + "\" style=\"color:SlateGrey; cursor:pointer;\" class=\"far fa-images\" data-toggle=\"modal\" onclick=\"Images('" + ID + "','/CP_Vehicles/Images/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_Vehicles.NoVehicle + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            VehiclesModel new_vehicle = new VehiclesModel();
            new_vehicle.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            new_vehicle.WhatsApp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            new_vehicle.Description = Request.Params["description"];
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

            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                new_vehicle.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                new_vehicle.User = user;
            }
            else
            {
                new_vehicle.User = null;
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
            if (ISValid(new_vehicle, true, out msg))
            {
                Guid id = Guid.NewGuid();
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = {  "user_id", "model_id", "vehicle_type_id", "owner_name", "keywords", "mobile", "is_new", "quantity", "price", "description", "year", "mieage", "gearbox", "fuel_type", "engine_size", "color", "title", "created_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { new_vehicle.User.ID, new_vehicle.Model.ID, new_vehicle.VehicleType.ID, new_vehicle.OwnerName, new_vehicle.Keywords, new_vehicle.Mobile, new_vehicle.IsNew, new_vehicle.Quantity, new_vehicle.Price, new_vehicle.Description, new_vehicle.Year, new_vehicle.Mieage, new_vehicle.Gearbox, new_vehicle.FuelType, new_vehicle.EngineSize, new_vehicle.Color, new_vehicle.Title, DateTime.Now, };
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
                    return Json(new { code = code.ToString(), msg = Resources.CP_Vehicles.Added });
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
        public PartialViewResult Edit(string ID)
        {
            string sql = "";
            sql += " SELECT V.id ,V.title,V.owner_name AS OwnerName ,I.url AS URL,I.id AS IID, V.title , U.full_name AS UserName,V.whatsapp, ";
            sql += " AD.id AS AddressID, V.price, V.is_new,V.quantity,V.owner_name,V.year,V.mieage,V.gearbox,V.fuel_type,V.engine_size,V.color, ";
            sql += " AD.details AS AddressDetailes , P.id AS CityID,P.country_id AS CountryID, ";
            sql += " V.mobile ,VT.id AS VehicleType,V.description,V.keywords ,M.id AS Model ";
            sql += " from Vehicles AS V";
            sql += " INNER JOIN Vehicle_Types AS VT ON V.vehicle_type_id = VT.id ";
            sql += " INNER JOIN Models AS M ON V.model_id = M.id ";
            sql += " INNER JOIN Users AS U ON V.user_id = U.id ";
            sql += " INNER JOIN Addresses AS AD ON AD.id = V.address_id ";
            sql += " INNER JOIN Provinces AS P ON AD.province_id = p.id ";
            sql += " INNER JOIN Images AS I ON V.id = I.referral_id ";
            sql += " WHERE V.id = @VID AND I.referral_type = 'Vehicles'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@VID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            DataRow vehicle = dataTable.Rows[0];
            VehiclesModel vehicles = new VehiclesModel();
            vehicles.ID = new Guid(vehicle["ID"].ToString());
            vehicles.Title = vehicle["Title"].ToString();
            vehicles.Color = vehicle["Color"].ToString();
            vehicles.EngineSize = vehicle["engine_size"].ToString();
            vehicles.FuelType = vehicle["fuel_type"].ToString();
            vehicles.Gearbox = vehicle["Gearbox"].ToString();
            vehicles.IsNew = Convert.ToInt32(vehicle["is_new"]);
            vehicles.WhatsApp = vehicle["whatsapp"].ToString();
            vehicles.Mieage = vehicle["Mieage"].ToString();
            vehicles.OwnerName = vehicle["OwnerName"].ToString();
            vehicles.Price = vehicle["Price"].ToString();
            vehicles.Quantity = vehicle["Quantity"].ToString();
            vehicles.Year = vehicle["Year"].ToString();
            vehicles.Mobile = vehicle["Mobile"].ToString();
            vehicles.Keywords = vehicle["Keywords"].ToString();
            vehicles.Description = vehicle["Description"].ToString();

            ModelsModel model = new ModelsModel();
            model.ID = new Guid(vehicle["Model"].ToString());
            vehicles.Model = model;

            VehicleTypesModel VehicleType = new VehicleTypesModel();
            VehicleType.ID = new Guid(vehicle["VehicleType"].ToString());
            vehicles.VehicleType = VehicleType;

            vehicles.Image = new ImagesModel()
            {
                URL = vehicle["URL"].ToString(),
                ID = new Guid(vehicle["IID"].ToString())
            };

            UsersModel user = new UsersModel();
            user.UserName = vehicle["UserName"].ToString();
            vehicles.User = user;

            vehicles.Address = new AddressModel()
            {
                AddressId = new Guid(vehicle["AddressID"].ToString()),
                AddressName = vehicle["AddressDetailes"].ToString(),
                ProvinceId = new Guid(vehicle["CityID"].ToString()),
                CountryId = new Guid(vehicle["CountryID"].ToString())
            };
            vehicles.URL = new URLModel()
            {
                Refresh = "/CP_Vehicles/GetAll/",
                Edit = "/CP_Vehicles/Editing/"
            };
            return PartialView(vehicles);
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            VehiclesModel EditVehicle = new VehiclesModel();
            EditVehicle.ID = new Guid(Request.Params["ID"].ToString());
            EditVehicle.Title = Request.Params["Title"];
            EditVehicle.Color = Request.Params["Color"];
            EditVehicle.Description = Request.Params["Description"];
            EditVehicle.EngineSize = Request.Params["engine_size"];
            EditVehicle.FuelType = Request.Params["FuelType"];
            EditVehicle.Gearbox = Request.Params["Gearbox"];
            EditVehicle.Mieage = Request.Params["Mieage"];
            EditVehicle.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            EditVehicle.WhatsApp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            EditVehicle.OwnerName = Request.Params["OwnerName"];
            EditVehicle.Price = Request.Params["Price"];
            EditVehicle.Quantity = Request.Params["Quantity"];
            EditVehicle.Keywords = Request.Params["Keywords"];
            EditVehicle.Year = Request.Params["Year"];
            EditVehicle.IsNew = Convert.ToInt32(Request.Params["is_new"]);
            EditVehicle.Mobile = Request.Params["phoneno"].ToString() != string.Empty ? Request.Params["phone_key"] + " " + Request.Params["phoneno"] : null;

            if (Request.Params["model"] != null)
            {
                ModelsModel model = new ModelsModel();
                model.ID = new Guid(Request.Params["model"].ToString());
                EditVehicle.Model = model;
            }
            else
            {
                EditVehicle.Model = null;
            }
            if (Request.Params["vehicletype"] != null)
            {
                VehicleTypesModel vehicleType = new VehicleTypesModel();
                vehicleType.ID = new Guid(Request.Params["vehicletype"].ToString());
                EditVehicle.VehicleType = vehicleType;
            }
            else
            {
                EditVehicle.VehicleType = null;
            }

            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                EditVehicle.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                EditVehicle.User = user;
            }
            else
            {
                EditVehicle.User = null;
            }
            EditVehicle.Address = new AddressModel();
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                EditVehicle.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                EditVehicle.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                EditVehicle.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                EditVehicle.Address.ProvinceId = new Guid();
            }
            if (ISValid(EditVehicle, false, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "user_id", "title", "mobile", "color", "engine_size", "fuel_type", "gearbox", "is_new", "mieage", "model_id", "owner_name", "price", "quantity", "vehicle_type_id", "year", "keywords", "description", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { EditVehicle.User.ID, EditVehicle.Title, EditVehicle.Mobile, EditVehicle.Color, EditVehicle.EngineSize, EditVehicle.FuelType, EditVehicle.Gearbox, EditVehicle.IsNew, EditVehicle.Mieage, EditVehicle.Model.ID, EditVehicle.OwnerName, EditVehicle.Price, EditVehicle.Quantity, EditVehicle.VehicleType.ID, EditVehicle.Year, EditVehicle.Keywords, EditVehicle.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Vehicles", EditVehicle.ID, cols, vals, out errMessage))
                {
                    Database.DeleteRow("Addresses", EditVehicle.Address.AddressId, out msg);
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "updated_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { EditVehicle.Address.ProvinceId, EditVehicle.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Vehicles", EditVehicle.ID, cols, vals, out errMessage);
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = new Guid(Request.Params["image_id"].ToString());
                        string ImageURL = Request.Params["image_url"] != null ? "/Images/Vehicles/" + Request.Params["image_url"] : "/Images/dafault.png";
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
                        FileName = Server.MapPath("~/Images/Vehicles/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "updated_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, EditVehicle.ID, "Vehicles", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Vehicles.Edited });
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
            sql += " select V.id, I.url AS URL,V.whatsapp, V.title, U.full_name AS UserName, V.color, V.engine_size AS EngineSize,";
            sql += " (P.name + ' , ' + AD.details) AS Address, V.price,V.is_new,V.quantity,V.mieage,V.year,V.owner_name AS OwnerName,";
            sql += " V.gearbox,V.fuel_type,V.mobile, V.description, V.keywords, M.name AS Model,VT.type_name AS VehicleType";
            sql += " from Vehicles AS V";
            sql += " inner join Users AS U on V.user_id = U.id";
            sql += " inner join Addresses AS AD on AD.id = V.address_id";
            sql += " inner join Provinces AS P on AD.province_id = p.id";
            sql += " inner join Vehicle_Types AS VT on V.vehicle_type_id = VT.id";
            sql += " inner join Models AS M on V.model_id = M.id";
            sql += " inner join Images AS I on V.id = I.referral_id";
            sql += " Where V.id = @VID";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@VID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow vehicle = dataTable.Rows[0];
                VehiclesModel vehicles = new VehiclesModel();
                string GB = "";
                if (vehicle["gearbox"].ToString() == "0")
                {
                    GB = Resources.CP_Vehicles.Automatic;
                }
                else
                {
                    GB = Resources.CP_Vehicles.Manual;
                };
                vehicles.Gearbox = GB;

                string FT = "";
                if (vehicle["fuel_type"].ToString() == "0")
                {
                    FT = Resources.CP_Vehicles.Petrol;
                }
                else
                {
                    FT = Resources.CP_Vehicles.Diesel;
                };
                vehicles.FuelType = FT;


                vehicles.ID = new Guid(vehicle["ID"].ToString());
                vehicles.Title = vehicle["Title"].ToString();
                vehicles.Mobile = vehicle["Mobile"].ToString();
                vehicles.Keywords = vehicle["Keywords"].ToString();
                vehicles.Description = vehicle["Description"].ToString();
                vehicles.Color = vehicle["Color"].ToString();
                vehicles.EngineSize = vehicle["EngineSize"].ToString();
                vehicles.Mieage = vehicle["Mieage"].ToString();
                vehicles.Year = vehicle["Year"].ToString();
                vehicles.Price = vehicle["Price"].ToString();
                vehicles.OwnerName = vehicle["OwnerName"].ToString();
                vehicles.Quantity = vehicle["Quantity"].ToString();
                vehicles.WhatsApp = vehicle["Whatsapp"].ToString();
                vehicles.Address = new AddressModel();
                vehicles.Address.AddressName = vehicle["Address"].ToString();

                VehicleTypesModel VT = new VehicleTypesModel();
                VT.Name = vehicle["VehicleType"].ToString();
                vehicles.VehicleType = VT;

                ModelsModel M = new ModelsModel();
                M.Name = vehicle["Model"].ToString();
                vehicles.Model = M;

                vehicles.IsNew = vehicle["is_new"].ToString() == "0" ? 0 : 1;

                vehicles.Image = new ImagesModel()
                {
                    URL = vehicle["URL"].ToString()
                };

                UsersModel user = new UsersModel();
                user.UserName = vehicle["UserName"].ToString();
                vehicles.User = user;

                return PartialView(vehicles);
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public PartialViewResult Images(string ID)
        {
            string sql = "";
            sql += " select V.id AS VID,I.url AS URL ,I.id AS ID from Vehicles AS V";
            sql += " inner join Images AS I on V.id = I.referral_id Where V.id = @VID";
            sql += " AND I.referral_type = 'Vehicles' AND I.is_main = 0 ORDER BY I.created_at ASC";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@VID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            VehiclesModel vehicles = new VehiclesModel();
            vehicles.ID = new Guid(ID);
            vehicles.URL = new URLModel
            {
                DeleteImage = "/CP_Vehicles/DeleteImage/",
                RefreshImages = "/CP_Vehicles/RefreshImages/",
                AddingImages = "/CP_Vehicles/AddImage/"
            };
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                vehicles.Images = new List<ImagesModel>();
                foreach (DataRow images in dataTable.Rows)
                {
                    ImagesModel image = new ImagesModel();
                    image.URL = images["URL"].ToString();
                    image.ID = new Guid(images["ID"].ToString());
                    vehicles.Images.Add(image);
                };

                return PartialView(vehicles);
            }
            else
            {
                return PartialView();
            }
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
                FileName = Server.MapPath("~/Images/Vehicles/SubImages/" + FileName);
                System.IO.File.WriteAllBytes(FileName, b);
                List<string> cols = new List<string>();
                List<object> vals = new List<object>();
                string[] colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                cols.AddRange(colsinput);
                object[] valsinput = new object[] { ImageName, new Guid(ID), "Vehicles", DateTime.Now, 0 };
                vals.AddRange(valsinput);
                Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                Session["Attachment"] = null;
                Session["Attachment_File_Name"] = null;
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Vehicles.Added });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });

            }
        }

        [HttpPost]
        public JsonResult RefreshImages(string ID)
        {
            string sql = "";
            sql += " select V.id AS VID,I.url AS URL,I.id AS ID from Vehicles AS V";
            sql += " inner join Images AS I on V.id = I.referral_id Where V.id = @VID";
            sql += " AND I.referral_type = 'Vehicles' AND I.is_main = 0 ORDER BY I.created_at ASC";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@VID", ID));
            string msg = "";
            DataTable Images = Database.ReadTableByQuery(sql, li, out msg);
            string HTML_Content = "";
            if (Images != null)
            {
                foreach (DataRow Image in Images.Rows)
                {
                    HTML_Content += "<div class=\'py-3 border m-1\'>";
                    HTML_Content += "<img class=\"rounded w-50\" src=\"/Images/Vehicles/SubImages/" + Image["URL"] + " \"/>";
                    HTML_Content += "<button type=\"button\" class=\"btn btn-danger ml-5 mt-5\" style=\"margin: 14%;\" onclick =\"DeleteImage('" + Image["VID"] + "', '" + Image["ID"] + "','/CP_Vehicles/DeleteImage/','/CP_Vehicles/RefreshImages/')\" >";
                    HTML_Content += Resources.CP_Vehicles.DeleteImage;
                    HTML_Content += "</button>";
                    HTML_Content += "</div>";
                }
            }
            else
            {
                HTML_Content = Resources.CP_Vehicles.NoImage;
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public JsonResult DeleteImage(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Images", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Vehicles.Deleted });

            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });

            }

        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Vehicles", new Guid(ID), out msg))
            {
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@VID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Vehicles_Properties Where vehicle_id= @VID ", li, out msg);
                }
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Vehicles.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }

        bool ISValid(VehiclesModel Vehicle, bool Is_Add, out string msg)
        {
            bool flag = true;
            if (Vehicle.Title == "")
            {
                msg = Resources.CP_Vehicles.EnterTitleP;
                return false;
            }
            if (Vehicle.OwnerName == "")
            {
                msg = Resources.CP_Vehicles.EnterOwnerNameP;
                return false;
            }
            if (Vehicle.User == null)
            {
                msg = Resources.CP_Vehicles.EnterUserP;
                return false;
            }
            if (Vehicle.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP_Vehicles.EnterCityP;
                return false;
            }
            if (Vehicle.Address.AddressName == "")
            {
                msg = Resources.CP_Vehicles.EnterAddressP;
                return false;
            }
            if (Vehicle.Mobile == null)
            {
                msg = Resources.CP_Vehicles.EnterPhoneP;
                return false;
            }
            //if (Vehicle.WhatsApp == null)
            //{
            //    msg = Resources.CP.EnterWhatsapp;
            //    return false;
            //}
            if (Vehicle.VehicleType == null)
            {
                msg = Resources.CP_Vehicles.EnterVehicleP;
                return false;
            }
            if (Vehicle.Model == null)
            {
                msg = Resources.CP_Vehicles.EnterModelP;
                return false;
            }
            if (Vehicle.IsNew == -1)
            {
                msg = Resources.CP_Vehicles.IsNew;
                return false;
            }
            if (Vehicle.Price == "")
            {
                msg = Resources.CP_Vehicles.EnterPriceP;
                return false;
            }
            if (Vehicle.Quantity == "")
            {
                msg = Resources.CP_Vehicles.EnterQuantityP;
                return false;
            }
            if (Vehicle.Year == "-1")
            {
                msg = Resources.CP_Vehicles.EnterYearP;
                return false;
            }
            if (Vehicle.Mieage == "-1")
            {
                msg = Resources.CP_Vehicles.EnterMieageP;
                return false;
            }
            if (Vehicle.Gearbox == "-1")
            {
                msg = Resources.CP_Vehicles.EnterGearBoxP;
                return false;
            }
            if (Vehicle.FuelType == "-1")
            {
                msg = Resources.CP_Vehicles.EnterFuelTypeP;
                return false;
            }
            if (Vehicle.EngineSize == "")
            {
                msg = Resources.CP_Vehicles.EnterEngineSizeP;
                return false;
            }
            if (Vehicle.Color == "")
            {
                msg = Resources.CP_Vehicles.EnterColorP;
                return false;
            }
            if (Vehicle.Keywords == "")
            {
                msg = Resources.CP_Vehicles.EnterKeywordsP;
                return false;
            }
            if (Vehicle.Description == "")
            {
                msg = Resources.CP_Vehicles.EnterDescrptionP;
                return false;
            }
            if (Session["Attachment"] == null && Is_Add)
            {
                msg = Resources.CP_Vehicles.AddImageP;
                return false;
            }

            msg = "";
            return flag;
        }
    }
}