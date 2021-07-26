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
    public class CP_PartsController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Parts/GetAll/", Add = "/CP_Parts/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Parts/GetAll/", Adding = "/CP_Parts/Adding/" });
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string msg;
            string sql = "";
            DataRow user; Tools.FindCurrentUser(out user);
            string condetion = " ";
            if (Session["Is_Admin"].ToString() != "1")
            {
                condetion = " where U.id = '" + user["id"].ToString() + "' ";
            }

            sql += " select P.id ,P.title,P.is_new AS IsNew,P.price,Models.name AS Models,Vehicle_Types.type_name AS VehicleTypes from Products as P ";
            sql += " inner join Models on Models.id = P.model_id ";
            sql += " inner join Vehicle_Types on Vehicle_Types.id = P.vehicle_type_id ";
            sql += " inner join Users AS U on U.id = P.user_id" + condetion + " ORDER BY P.created_at ASC";
            DataTable products = Database.ReadTableByQuery(sql, null, out msg);
            string HTML_Content = "";
            if (products != null && products.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow product in products.Rows)
                {
                    string IsNew = "";
                    IsNew = product["IsNew"].ToString() == "0" ? IsNew = Resources.CP_Parts.Old : IsNew = Resources.CP_Parts.New;
                    string ID = product["ID"].ToString();
                    HTML_Content += "<tr class=\"part-row \">";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    HTML_Content += "<td> " + product["Title"].ToString() + "</td>";
                    HTML_Content += "<td> " + IsNew + " </td>";
                    HTML_Content += "<td> " + product["Models"].ToString() + " </td>";
                    HTML_Content += "<td> " + product["VehicleTypes"].ToString() + " </td>";
                    HTML_Content += "<td> " + product["Price"].ToString() + " </td>";
                    //Tools: 
                    HTML_Content += "<td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Parts.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Parts/Edit/')\"data-target=\"#Modal\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Parts.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Parts/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Parts.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Parts/Details/')\" data-target=\"#Modal\"></i>&nbsp&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Parts.Images + "\" style=\"color:SlateGrey; cursor:pointer;\" class=\"far fa-images\" data-toggle=\"modal\" onclick=\"Images('" + ID + "','/CP_Parts/Images/')\" data-target=\"#Modal\"></i>";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP.NoParts + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });
        }
        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            ProductsModel new_product = new ProductsModel();
            new_product.Title = Request.Params["Title"] != null ? Request.Params["Title"] : "";
            new_product.Description = Request.Params["Description"] != null ? Request.Params["Description"] : "";
            new_product.IsNew = Convert.ToInt32(Request.Params["is_new"]);
            new_product.Price = Request.Params["price"] != null ? Request.Params["price"] : "";
            new_product.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            new_product.WhatsApp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            new_product.Keywords = Request.Params["keywords"] != null ? Request.Params["keywords"] : "";
            new_product.OwnerName = Request.Params["owner_name"] != null ? Request.Params["owner_name"] : "";
            new_product.Year = Request.Params["Year"] != null ? Request.Params["Year"] : "";
            new_product.Mieage = Request.Params["mieage"] != null ? Request.Params["mieage"] : "";
            new_product.GearBox = Request.Params["gearbox"] != null ? Request.Params["gearbox"] : "";
            new_product.EngineSize = Request.Params["engine_size"] != null ? Request.Params["engine_size"] : "";
            new_product.Color = Request.Params["Color"] != null ? Request.Params["Color"] : "";
            new_product.FuelType = Request.Params["fuel_type"] != null ? Request.Params["fuel_type"] : "";
            new_product.Quantity = Request.Params["quantity"].AsInt().ToString() != null ? Request.Params["quantity"].AsInt().ToString() : "";
            if (Request.Params["model"] != null)
            {
                ModelsModel model = new ModelsModel();
                model.ID = new Guid(Request.Params["model"].ToString());
                new_product.Model = model;
            }
            else
            {
                new_product.Model = null;
            }
            if (Request.Params["brand"] != null)
            {
                BrandsModel brand = new BrandsModel();
                brand.ID = new Guid(Request.Params["brand"].ToString());
                new_product.Brand = brand;
            }
            else
            {
                new_product.Model = null;
            }
            if (Request.Params["vehicletype"] != null)
            {
                VehicleTypesModel vehicleType = new VehicleTypesModel();
                vehicleType.ID = new Guid(Request.Params["vehicletype"].ToString());
                new_product.VehicleType = vehicleType;
            }
            else
            {
                new_product.VehicleType = null;
            }

            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                new_product.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                new_product.User = user;
            }
            else
            {
                new_product.User = null;
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
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                new_product.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                new_product.Address.ProvinceId = new Guid();
            }

            if (ISValid(new_product, true, out msg))
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
                    return Json(new { code = code.ToString(), msg = Resources.CP_Parts.Added });
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
                FileName = Server.MapPath("~/Images/Products/SubImages/" + FileName);
                System.IO.File.WriteAllBytes(FileName, b);
                List<string> cols = new List<string>();
                List<object> vals = new List<object>();
                string[] colsinput = new string[] { "url", "referral_id", "referral_type", "created_at", "is_main" };
                cols.AddRange(colsinput);
                object[] valsinput = new object[] { ImageName, new Guid(ID), "Products", DateTime.Now, 0 };
                vals.AddRange(valsinput);
                Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                Session["Attachment"] = null;
                Session["Attachment_File_Name"] = null;
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Parts.Added });
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
            sql += " SELECT P.id AS PID, I.url AS URL, I.id AS ID from Products AS P ";
            sql += " inner join Images AS I on P.id = I.referral_id";
            sql += " Where P.id = @PID AND I.referral_type = 'Products' AND I.is_main = 0 ORDER BY I.created_at ASC";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@PID", ID));
            string msg = "";
            DataTable Images = Database.ReadTableByQuery(sql, li, out msg);
            string HTML_Content = "";
            if (Images != null)
            {
                foreach (DataRow Image in Images.Rows)
                {
                    HTML_Content += "<div class=\'py-3 border m-1\'>";
                    HTML_Content += "<img class=\"rounded w-50\" src=\"/Images/Products/SubImages/" + Image["URL"] + " \"/>";
                    HTML_Content += "<button type=\"button\" class=\"btn btn-danger ml-5 mt-5\" style=\"margin: 14%;\" onclick =\"DeleteImage('" + Image["PID"] + "','" + Image["ID"] + "','/CP_Parts/DeleteImage/','/CP_Parts/RefreshImages/')\">";
                    HTML_Content += Resources.CP.Delete;
                    HTML_Content += "</button>";
                    HTML_Content += "</div>";
                }
            }
            else
            {
                HTML_Content = Resources.CP_Parts.NoImage;
            }
            return Json(new { code = 200, data = HTML_Content });
        }

        [HttpPost]
        public PartialViewResult Images(string ID)
        {
            string sql = "";
            sql += " SELECT P.id AS PID, I.url AS URL ,I.id AS ID from Products AS P ";
            sql += " inner join Images AS I on P.id = I.referral_id  Where P.id = @PID AND ";
            sql += " I.referral_type = 'Products' AND I.is_main = 0 ORDER BY I.created_at ASC";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@PID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            ProductsModel Product = new ProductsModel();
            Product.ID = new Guid(ID);
            Product.URL = new URLModel
            {
                DeleteImage = "/CP_Parts/DeleteImage/",
                RefreshImages = "/CP_Parts/RefreshImages/",
                AddingImages = "/CP_Parts/AddImage/"
            };
            Product.Images = new List<ImagesModel>();
            foreach (DataRow images in dataTable.Rows)
            {
                ImagesModel image = new ImagesModel();
                image.URL = images["URL"].ToString();
                image.ID = new Guid(images["ID"].ToString());
                Product.Images.Add(image);
            };
            return PartialView(Product);
        }

        [HttpPost]
        public JsonResult DeleteImage(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Images", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Parts.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }

        }
        [HttpPost]
        public PartialViewResult Details(string ID)
        {
            string sql = "";
            sql += " SELECT PR.id , PR.title,PR.price,PR.is_new, PR.quantity,";
            sql += " PR.keywords,PR.owner_name,PR.year,PR.mieage,";
            sql += " PR.gearbox,PR.color,PR.fuel_type,PR.engine_size,";
            sql += " PR.mobile,PR.whatsapp ,PR.description ,";
            sql += " (P.name + ' , ' + AD.details) AS Address,";
            sql += " I.url AS URL,U.full_name AS Users,";
            sql += " PR.keywords,M.name AS Model,";
            sql += " VT.type_name AS VehicleTypes,B.name AS Brand";
            sql += " from Products AS PR";
            sql += " inner join Users AS U on PR.user_id = U.id";
            sql += " inner join Addresses AS AD on AD.id = PR.address_id";
            sql += " inner join Provinces AS P on AD.province_id = p.id";
            sql += " inner join Models AS M ON M.id = PR.model_id";
            sql += " inner join Images AS I on PR.id = I.referral_id";
            sql += " inner join Vehicle_Types AS VT ON PR.vehicle_type_id = VT.id";
            sql += " inner join Brands AS B ON B.id = PR.brand_id";
            sql += " Where PR.id = @PRID AND I.referral_type = 'Products'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@PRID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow Product_ = dataTable.Rows[0];
                ProductsModel Product = new ProductsModel();
                Product.ID = new Guid(Product_["ID"].ToString());
                Product.Title = Product_["Title"].ToString();
                Product.Price = Product_["Price"].ToString();
                Product.IsNew = Product_["is_new"].ToString() == "0" ? 0 : 1;
                Product.WhatsApp = Product_["Whatsapp"].ToString();
                Product.Mobile = Product_["Mobile"].ToString();
                Product.Keywords = Product_["Keywords"].ToString();
                Product.Description = Product_["Description"].ToString();
                Product.Quantity = Product_["quantity"].ToString();
                Product.OwnerName = Product_["owner_name"].ToString();

                Product.Address = new AddressModel();
                Product.Address.AddressName = Product_["Address"].ToString();

                BrandsModel brand = new BrandsModel();
                brand.Name = Product_["Brand"].ToString();
                Product.Brand = brand;

                ModelsModel model = new ModelsModel();
                model.Name = Product_["Model"].ToString();
                Product.Model = model;

                VehicleTypesModel VehicleType = new VehicleTypesModel();
                VehicleType.Name = Product_["VehicleTypes"].ToString();
                Product.VehicleType = VehicleType;

                Product.Image = new ImagesModel()
                {
                    URL = Product_["URL"].ToString()
                };

                UsersModel user = new UsersModel();
                user.UserName = Product_["Users"].ToString();
                Product.User = user;

                sql = "";
                sql += " SELECT C.id, C.name AS Categories from Categories AS C WHERE C.id in ";
                sql += " (SELECT category_id FROM Products_Categories where product_id = @PID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@PID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                Product.Services = new List<ServicesModel>();
                foreach (DataRow item in dataTable.Rows)
                {
                    ServicesModel service = new ServicesModel();
                    service.Name = item["Categories"].ToString();
                    service.ID = new Guid(item["ID"].ToString());
                    Product.Services.Add(service);
                }

                return PartialView(Product);
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public PartialViewResult Edit(string ID)
        {
            string sql = "";
            sql += " select PR.id ,I.url AS URL, PR.title ,I.id  AS IID ,PR.is_new,PR.quantity,PR.engine_size,PR.color,PR.fuel_type,";
            sql += " PR.mobile ,PR.whatsapp ,PR.description,PR.keywords ,PR.price,PR.owner_name,PR.year,PR.mieage,PR.gearbox, ";
            sql += " U.full_name AS Users, B.id AS Brand,VT.id AS VehicleType,M.id AS Model,";
            sql += " AD.id AS AddressID,  AD.details AS AddressDetailes ,";
            sql += " P.id AS CityID,P.country_id AS CountryID";
            sql += " from Products AS PR";
            sql += " inner join Users AS U on PR.user_id = U.id";
            sql += " inner join Addresses AS AD on AD.id = PR.address_id";
            sql += " inner join Provinces AS P on AD.province_id = p.id";
            sql += " inner join Brands AS B ON B.id = PR.brand_id";
            sql += " inner join Vehicle_Types AS VT ON VT.id = PR.vehicle_type_id";
            sql += " inner join Models AS M ON M.id = PR.model_id";
            sql += " inner join Images AS I on PR.id = I.referral_id  ";
            sql += " Where PR.id = @PRID AND I.referral_type = 'Products'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@PRID", ID));
            string msg = "";
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow Product_ = dataTable.Rows[0];
                ProductsModel Product = new ProductsModel();
                Product.ID = new Guid(Product_["ID"].ToString());
                Product.Title = Product_["Title"].ToString();
                Product.Price = Product_["Price"].ToString();
                Product.Color = Product_["Color"].ToString();
                Product.EngineSize = Product_["engine_size"].ToString();
                Product.FuelType = Product_["fuel_type"].ToString();
                Product.OwnerName = Product_["owner_name"].ToString();
                Product.GearBox = Product_["GearBox"].ToString();
                Product.IsNew = Convert.ToInt32(Product_["is_new"].ToString());
                Product.Mieage = Product_["Mieage"].ToString();
                Product.Mobile = Product_["Mobile"].ToString();
                Product.WhatsApp = Product_["Whatsapp"].ToString();
                Product.Year = Product_["Year"].ToString();
                Product.Mobile = Product_["Mobile"].ToString();
                Product.Keywords = Product_["Keywords"].ToString();
                Product.Description = Product_["Description"].ToString();
                Product.Image = new ImagesModel()
                {
                    ID = new Guid(Product_["IID"].ToString()),
                    URL = Product_["URL"].ToString()
                };

                BrandsModel Brand = new BrandsModel();
                Brand.ID = new Guid(Product_["Brand"].ToString());
                Product.Brand = Brand;

                UsersModel user = new UsersModel();
                user.UserName = Product_["Users"].ToString();
                Product.User = user;

                VehicleTypesModel VehicleType = new VehicleTypesModel();
                VehicleType.ID = new Guid(Product_["VehicleType"].ToString());
                Product.VehicleType = VehicleType;

                ModelsModel Model = new ModelsModel();
                Model.ID = new Guid(Product_["Model"].ToString());
                Product.Model = Model;

                Product.Address = new AddressModel()
                {
                    AddressId = new Guid(Product_["AddressID"].ToString()),
                    AddressName = Product_["AddressDetailes"].ToString(),
                    ProvinceId = new Guid(Product_["CityID"].ToString()),
                    CountryId = new Guid(Product_["CountryID"].ToString())
                };

                sql = "";
                sql += " SELECT C.id, C.name AS Categories from Categories AS C WHERE C.id in ";
                sql += " (SELECT category_id FROM Products_Categories where product_id = @PID)";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@PID", ID));
                dataTable = Database.ReadTableByQuery(sql, li, out msg);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    Product.Services = new List<ServicesModel>();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        ServicesModel service = new ServicesModel();
                        service.Name = item["Categories"].ToString();
                        service.ID = new Guid(item["ID"].ToString());
                        Product.Services.Add(service);
                    }
                }

                Product.URL = new URLModel
                {
                    Refresh = "/CP_Parts/GetAll/",
                    Edit = "/CP_Parts/Editing/"
                };
                return PartialView(Product);
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
            ProductsModel Product = new ProductsModel();
            Product.ID = new Guid(Request.Params["ID"].ToString());
            Product.Title = Request.Params["Title"];
            Product.Mobile = Request.Params["phoneno"].ToString() != string.Empty && HelperClass.Phone(Request.Params["phoneno"]) ? "+" + Request.Params["phone_key"] + Request.Params["phoneno"] : null;
            Product.WhatsApp = Request.Params["Whatsapp"].ToString() != string.Empty && HelperClass.Phone(Request.Params["Whatsapp"]) ? "+" + Request.Params["whatsapp_key"] + Request.Params["Whatsapp"] : null;
            Product.Color = Request.Params["Color"];
            Product.Description = Request.Params["Description"];
            Product.EngineSize = Request.Params["EngineSize"];
            Product.FuelType = Request.Params["FuelType"];
            Product.IsNew = Convert.ToInt32(Request.Params["is_new"].ToString());
            Product.Year = Request.Params["Year"];
            Product.Keywords = Request.Params["Keywords"];
            Product.Quantity = Request.Params["quantity"];
            Product.GearBox = Request.Params["GearBox"];
            Product.Price = Request.Params["Price"];
            Product.OwnerName = Request.Params["OwnerName"];
            Product.Mieage = Request.Params["Mieage"];

            if (Request.Params["model"] != null)
            {
                ModelsModel model = new ModelsModel();
                model.ID = new Guid(Request.Params["model"].ToString());
                Product.Model = model;
            }
            else
            {
                Product.Model = null;
            }
            if (Request.Params["brand"] != null)
            {
                BrandsModel brand = new BrandsModel();
                brand.ID = new Guid(Request.Params["brand"].ToString());
                Product.Brand = brand;
            }
            else
            {
                Product.Model = null;
            }
            if (Request.Params["vehicletype"] != null)
            {
                VehicleTypesModel vehicleType = new VehicleTypesModel();
                vehicleType.ID = new Guid(Request.Params["vehicletype"].ToString());
                Product.VehicleType = vehicleType;
            }
            else
            {
                Product.VehicleType = null;
            }
            if (Session["Is_Admin"].ToString() != "1")
            {
                DataRow user;
                Tools.FindCurrentUser(out user);
                Product.User = new UsersModel()
                {
                    ID = new Guid(user["id"].ToString())
                };
            }
            else if (Request.Params["User"] != null && Request.Params["User"].ToString() != "-1")
            {
                UsersModel user = new UsersModel();
                user.ID = new Guid(Request.Params["User"].ToString());
                Product.User = user;
            }
            else
            {
                Product.User = null;
            }
            if (Request.Params["Address"] != null && Request.Params["Address"] != "")
            {
                Product.Address = new AddressModel();
                Product.Address.AddressName = Request.Params["Address"];
            }
            else
            {
                Product.Address.AddressName = "";
            }
            if (Request.Params["City"] != null && Request.Params["City"].ToString() != "-1")
            {
                Product.Address.ProvinceId = new Guid(Request.Params["City"].ToString());
            }
            else
            {
                Product.Address.ProvinceId = new Guid();
            }

            if (ISValid(Product, false, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "user_id", "title", "mobile", "brand_id", "whatsapp", "price", "model_id", "vehicle_type_id", "is_new", "quantity", "owner_name", "year", "mieage", "keywords", "description", "gearbox", "engine_size", "fuel_type", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { Product.User.ID, Product.Title, Product.Mobile, Product.Brand.ID, Product.WhatsApp, Product.Price, Product.Model.ID, Product.VehicleType.ID, Product.IsNew, Product.Quantity, Product.OwnerName, Product.Year, Product.Mieage, Product.Keywords, Product.Description, Product.GearBox, Product.EngineSize, Product.FuelType, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Products", Product.ID, cols, vals, out errMessage))
                {
                    Database.DeleteRow("Addresses", Product.Address.AddressId, out msg);
                    Guid addressId = Guid.NewGuid();
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "province_id", "details", "updated_at" };
                    cols.AddRange(colsinput);
                    object[] valsin = { Product.Address.ProvinceId, Product.Address.AddressName, DateTime.Now };
                    vals.AddRange(valsin);
                    Database.InsertRow("Addresses", addressId, cols, vals, out errMessage);
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "address_id" };
                    cols.AddRange(colsinput);
                    valsin = new object[] { addressId };
                    vals.AddRange(valsin);
                    Database.UpdateRow("Products", Product.ID, cols, vals, out errMessage);
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = new Guid(Request.Params["image_id"].ToString());
                        string ImageURL = Request.Params["image_url"] != null ? "/Images/Products/" + Request.Params["image_url"] : "/Images/dafault.png";
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
                        FileName = Server.MapPath("~/Images/Products/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);

                        cols = new List<string>();
                        vals = new List<object>();

                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "updated_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, Product.ID, "Products", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }
                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Parts.Edited });
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
            if (Database.DeleteRow("Products", new Guid(ID), out msg))
            {
                {
                    List<SqlParameter> li = new List<SqlParameter>();
                    li.Add(new SqlParameter("@PID", new Guid(ID)));
                    Database.ReadTableByQuery("DELETE FROM Products_Categories Where product_id = @PID ", li, out msg);
                }
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Parts.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }
        }


        bool ISValid(ProductsModel Product, bool Is_Add, out string msg)
        {
            bool flag = true;
            if (Product.Title == "")
            {
                msg = Resources.CP_Parts.EnterTitlePlease;
                return false;
            }
            if (Product.User == null)
            {
                msg = Resources.CP_Parts.EnterUserNamePlease;
                return false;
            }
            if (Product.OwnerName == "")
            {
                msg = Resources.CP_Parts.EnterOwnerNamePlease;
                return false;
            }
            if (Product.Address.ProvinceId == new Guid())
            {
                msg = Resources.CP_Parts.EnterCityPlease;
                return false;
            }
            if (Product.Address.AddressName == "")
            {
                msg = Resources.CP_Parts.EnterAddressPlease;
                return false;
            }
            if (Product.Mobile == null)
            {
                msg = Resources.CP_Parts.EnterPhonePlease;
                return false;
            }
            if (Request.Params["services"] == null)
            {
                msg = Resources.CP_Parts.EnterSerivesPlease;
                return false;
            }
            if (Product.VehicleType == null)
            {
                msg = Resources.CP_Parts.EnterVehiclePlease;
                return false;
            }
            if (Product.Model == null)
            {
                msg = Resources.CP_Parts.EnterModelPlease;
                return false;
            }
            if (Product.Brand == null)
            {
                msg = Resources.CP_Parts.EnterBrandPlease;
                return false;
            }
            if (Product.IsNew == -1)
            {
                msg = Resources.CP_Parts.IsNew;
                return false;
            }
            if (Product.Price == "")
            {
                msg = Resources.CP_Parts.EnterPricePlease;
                return false;
            }
            if (Product.Quantity == "0")
            {
                msg = Resources.CP_Parts.EnterQuantityPlease;
                return false;
            }
            if (Product.Year == "-1")
            {
                msg = Resources.CP_Parts.EnterYearPlease;
                return false;
            }
            if (Product.Keywords == "")
            {
                msg = Resources.CP_Parts.EnterKeywordsPlease;
                return false;
            }
            if (Product.Description == "")
            {
                msg = Resources.CP_Parts.EnterDescrptionPlease;
                return false;
            }
            if (Session["Attachment"] == null && Is_Add)
            {
                msg = Resources.CP_Parts.EnterImagePlease;
                return false;
            }
            msg = "";
            return flag;
        }

    }
}