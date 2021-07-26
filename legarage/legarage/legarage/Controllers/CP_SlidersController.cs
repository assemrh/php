using legarage.Classes;
using legarage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class CP_SlidersController : BaseController
    {
        public ActionResult Index()
        {
            return View(new URLModel { Refresh = "/CP_Sliders/GetAll/", Add = "/CP_Sliders/Add/" });
        }

        [HttpPost]
        public PartialViewResult Add()
        {
            return PartialView(new URLModel { Refresh = "/CP_Sliders/GetAll/", Adding = "/CP_Sliders/Adding/" });
        }

        [HttpPost]
        public JsonResult GetData(string table_name)
        {
            string msg="";
            string Html_Content = "";
            string sql = " select  id , ";
            if (table_name == "Garages" || table_name == "Rental_Offices")
                sql += " name as title ";
            else
                sql += " title ";
            sql += " from " + table_name;
            DataTable data = Database.ReadTableByQuery(sql, null, out msg);
            if(data != null && data.Rows.Count > 0)
            {
                foreach (DataRow item in data.Rows)
                {
                    Html_Content += "<option value=\"" + item["id"].ToString() + "\"> " + item["title"].ToString() + " </option>";
                }
            }
            else
            {
                Html_Content += "<option value=\"0\"> " + "No Data" + " </option>";
            }
            if(msg!="")
                return Json(new { code = 404, msg = msg });
            else
                return Json(new { code = 200, data = Html_Content }); 
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            string msg;
            DataTable Sliders = Database.ReadTable("Slider", "ORDER BY created_at ASC", null, out msg);
            string HTML_Content = "";
            if (Sliders != null && Sliders.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow Slider in Sliders.Rows)
                {
                    string ID = Slider["ID"].ToString();
                    HTML_Content += "<tr>";
                    HTML_Content += "<th scope=\"row\" >" + (++count) + " </th>";
                    if (Slider["referral_type"] == null || Slider["referral_type"].ToString() == string.Empty)
                    {
                        HTML_Content += "<td> " + Slider["title"].ToString() + "</td>";
                    }
                    else
                    {
                        string referral_type = Slider["referral_type"].ToString();
                        string id = Slider["referral_id"].ToString();
                        string masg = "";
                        string sql = "";
                        sql += " select ";
                        if (referral_type == "Garages" || referral_type == "Rental_Offices")
                            sql += " name as title ";
                        else
                            sql += " title ";
                        sql += " from " + referral_type;
                        sql += " where id = @id";
                        List<SqlParameter> li = new List<SqlParameter>();
                        li.Add(new SqlParameter("@id", id));
                        DataTable data = Database.ReadTableByQuery(sql, li, out masg);
                        if(data != null && data.Rows.Count == 1 )
                        HTML_Content += "<td> " + data.Rows[0]["title"].ToString() + "</td>";
                        else
                            HTML_Content += "<td> </td>";
                    }
                    HTML_Content += "<td> " + Slider["roworder"].ToString() + "</td>";
                    //Tools: 
                    HTML_Content += "<td>";
                    HTML_Content += "<i title = \"" + Resources.CP_Sliders.Edit + "\" style = \"color:darkcyan; cursor:pointer;\" class=\"fas fa-file-alt\"data-toggle=\"modal\" onclick=\"Edit('" + ID + "','/CP_Sliders/Edit/')\"data-target=\"#Modal\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Sliders.Delete + "\" style=\"color:red; cursor:pointer;\" class=\"fas fa-trash\" onclick=\"Delete('" + ID + "','/CP_Sliders/Delete/');\"></i>&nbsp";
                    HTML_Content += "<i title = \"" + Resources.CP_Sliders.Details + "\" style=\"color:lawngreen; cursor:pointer;\" class=\"fas fa-table\" data-toggle=\"modal\" onclick=\"Details('" + ID + "','/CP_Sliders/Details/')\" data-target=\"#Modal\"></i>&nbsp&nbsp";
                    HTML_Content += "</td>";
                    HTML_Content += "</tr>";
                }
            }
            else
            {
                HTML_Content = "<h3>" + Resources.CP_Sliders.NoSliders + "</h3>";
            }
            return Json(new { code = 200, data = HTML_Content });

        }

        [HttpPost]
        public JsonResult Adding()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            SlidersModel NewSlider = new SlidersModel();
            if (Request.Params["site_elements_selector"] == "out")
            {
                NewSlider.ReferralType = Request.Params["site_elements_selector"];
                NewSlider.Title = Request.Params["Title"] != null ? Request.Params["Title"] : "";
                NewSlider.RowOrder = Convert.ToInt32(Request.Params["Order"]);
                NewSlider.Description = Request.Params["desc"] != null ? Request.Params["desc"] : "";
                NewSlider.Link = Request.Params["Link"] != null ? Request.Params["Link"] : "";
                if (ISValid(NewSlider, true, out msg))
                {
                    Guid ID = Guid.NewGuid();
                    List<string> cols = new List<string>();
                    List<Object> vals = new List<object>();
                    string[] colsinput = { "link", "roworder", "title", "description", "created_at" };
                    cols.AddRange(colsinput);
                    object[] valsinput = { NewSlider.Link, NewSlider.RowOrder, NewSlider.Title, NewSlider.Description, DateTime.Now };
                    vals.AddRange(valsinput);
                    string errMessage = string.Empty;
                    if (Database.InsertRow("Slider", ID, cols, vals, out errMessage))
                    {
                        Guid ImageID = Guid.NewGuid();
                        string ImageName = "";
                        byte[] b = (byte[])Session["Attachment"];
                        string FileName = (string)Session["Attachment_File_Name"];
                        FileName = ImageID.ToString() + System.IO.Path.GetExtension(FileName);
                        ImageName = FileName;
                        FileName = Server.MapPath("~/Images/Sliders/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);
                        cols = new List<string>();
                        vals = new List<object>();
                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "created_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, ID, "Sliders", DateTime.Now };
                        vals.AddRange(valsinput);
                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);
                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                        code = 200;
                        msg = Resources.CP_Sliders.Added;
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
            else
            {
                NewSlider.RowOrder = Convert.ToInt32(Request.Params["Order"]);
                NewSlider.ReferralType = Request.Params["site_elements_selector"];
                NewSlider.Description = Request.Params["desc"] != null ? Request.Params["desc"] : "";
                NewSlider.ReferralID = new Guid(Request.Params["secound_ddl"]);
                if (ISValid(NewSlider, false, out msg))
                {
                    Guid ID = Guid.NewGuid();
                    List<string> cols = new List<string>();
                    List<Object> vals = new List<object>();
                    string[] colsinput = { "referral_id", "roworder", "description", "created_at", "referral_type" };
                    cols.AddRange(colsinput);
                    object[] valsinput = { NewSlider.ReferralID, NewSlider.RowOrder,  NewSlider.Description, DateTime.Now,NewSlider.ReferralType };
                    vals.AddRange(valsinput);
                    string errMessage = string.Empty;

                    if (Database.InsertRow("Slider", ID, cols, vals, out errMessage))
                    {
                        code = 200;
                        msg = Resources.CP_Sliders.Added;
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
            
        }

        [HttpPost]
        public PartialViewResult Edit(string ID)
        {
            string msg = "";
            string sql = "";
            sql += " select S.id as ID , S.title , I.url AS URL, I.id  AS ImageID ,S.description,S.link,S.roworder,S.referral_id,S.referral_type";
            sql += " from Slider AS S";
            sql += " left join Images AS I on I.referral_id = S.id";
            sql += " Where S.id = @SID";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@SID", new Guid(ID)));
            SlidersModel slider = new SlidersModel();
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow slider_ = dataTable.Rows[0];
                if (slider_["referral_type"] == null || slider_["referral_type"].ToString() == string.Empty)
                {
                    slider.Title = slider_["Title"].ToString();
                    slider.Link = slider_["Link"].ToString();
                    slider.Image = new ImagesModel()
                    {
                        URL = slider_["URL"].ToString(),
                        ID = new Guid(slider_["ImageID"].ToString())
                    };

                }
                else
                {
                    slider.ReferralType = slider_["referral_type"].ToString();
                    string referral_type = slider_["referral_type"].ToString();
                    slider.ReferralID = new Guid( slider_["referral_id"].ToString());
                    string id = slider_["referral_id"].ToString();
                    string masg = "";
                    string sql1 = "";
                    sql1 += " select ";
                    if (referral_type == "Garages" || referral_type == "Rental_Offices")
                        sql1 += "S.id, S.name as title ,I.url AS URL, I.id  AS ImageID ";
                    else
                        sql1 += " S.title ";
                    sql1 += " from " + referral_type + " AS S inner join Images AS I ON I.referral_id = S.id ";

                    sql1 += " where S.id = @id";
                    List<SqlParameter> li1 = new List<SqlParameter>();
                    li1.Add(new SqlParameter("@id", id));
                    DataTable data = Database.ReadTableByQuery(sql1, li1, out masg);
                    slider.Title = data.Rows[0]["title"].ToString();
                    slider.Image = new ImagesModel()
                    {
                        URL = data.Rows[0]["URL"].ToString(),
                        ID = new Guid(data.Rows[0]["ImageID"].ToString())
                    };

                }
                slider.RowOrder = Convert.ToInt32(slider_["roworder"]);
                slider.ID = new Guid(slider_["ID"].ToString());
                slider.Description = slider_["Description"].ToString();
                slider.URL = new URLModel()
                {
                    Refresh = "/CP_Sliders/GetAll/",
                    Edit = "/CP_Sliders/Editing/"
                };
            }
            return PartialView(slider);
        }

        [HttpPost]
        public JsonResult Editing()
        {
            string msg = "";
            int code = 0;
            Session["error"] = null;
            SlidersModel Edit = new SlidersModel();
            Edit.ID = new Guid(Request.Params["ID"]);
            Edit.Title = Request.Params["Title"] != null ? Request.Params["Title"] : "";
            Edit.Link = Request.Params["Link"] != null ? Request.Params["Link"] : "";
            Edit.Description = Request.Params["Description"] != null ? Request.Params["Description"] : "";
            Edit.RowOrder = Convert.ToInt32(Request.Params["Order"]);
            if (ISValid(Edit,false, out msg))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();
                string[] colsinput = { "title", "link", "roworder", "description", "updated_at" };
                cols.AddRange(colsinput);
                object[] valsinput = { Edit.Title, Edit.Link,Edit.RowOrder, Edit.Description, DateTime.Now };
                vals.AddRange(valsinput);
                string errMessage = string.Empty;
                if (Database.UpdateRow("Slider", Edit.ID, cols, vals, out errMessage))
                {
                    if (Session["Attachment"] != null)
                    {
                        Guid ImageID = new Guid(Request.Params["image_id"].ToString());
                        string ImageURL = Request.Params["image_url"] != null ? "/Images/Sliders/" + Request.Params["image_url"] : "/Images/dafault.png";
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
                        FileName = Server.MapPath("~/Images/Sliders/" + FileName);
                        System.IO.File.WriteAllBytes(FileName, b);

                        cols = new List<string>();
                        vals = new List<object>();

                        colsinput = new string[] { "is_main", "url", "referral_id", "referral_type", "updated_at" };
                        cols.AddRange(colsinput);
                        valsinput = new object[] { 1, ImageName, Edit.ID, "Sliders", DateTime.Now };
                        vals.AddRange(valsinput);

                        Database.InsertRow("Images", ImageID, cols, vals, out errMessage);

                        Session["Attachment"] = null;
                        Session["Attachment_File_Name"] = null;
                    }

                    code = 200;
                    return Json(new { code = code.ToString(), msg = Resources.CP_Sliders.Edited });
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
            string msg = "";
            string sql = "";
            sql += " select S.id as ID, S.title, S.description,S.link,S.roworder,I.url AS URL ";
            sql += " from Slider AS S ";
            sql += " inner join Images AS I on I.referral_id = S.id ";
            sql += " where S.id = @SID ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@SID", new Guid(ID)));
            DataTable dataTable = Database.ReadTableByQuery(sql, li, out msg);
            DataRow Slider_ = dataTable.Rows[0];
            SlidersModel slider = new SlidersModel();
            slider.ID = new Guid(Slider_["ID"].ToString());
            slider.Title = Slider_["title"].ToString();
            slider.Description = Slider_["description"].ToString();
            slider.Link = Slider_["link"].ToString();
            slider.RowOrder = Convert.ToInt32(Slider_["roworder"]);
            ImagesModel Image = new ImagesModel();
            Image.URL = Slider_["URL"].ToString();
            slider.Image = Image;
            return PartialView(slider);
        }

        [HttpPost]
        public JsonResult Delete(string ID)
        {
            string msg = "";
            int code = 0;
            if (Database.DeleteRow("Slider", new Guid(ID), out msg))
            {
                code = 200;
                return Json(new { code = code.ToString(), msg = Resources.CP_Sliders.Deleted });
            }
            else
            {
                code = 404;
                msg = "faill" + "<br/>" + msg.Replace(Environment.NewLine, "<br/>");
                return Json(new { code = code.ToString(), msg = msg });
            }

        }

        bool ISValid(SlidersModel slider, bool Is_Add, out string msg)
        {
            bool flag = true;
            //if (slider.Title == "")
            //{
            //    msg = Resources.CP_Sliders.EnterTitlePlease;
            //    return false;
            //}
            //if (slider.RowOrder < 1 || slider.RowOrder > 7  )
            //{
            //    msg = Resources.CP_Sliders.EnterCorrectOrder;
            //    return false;
            //}
            //if (slider.Link == "")
            //{
            //    msg = Resources.CP_Sliders.EnterLinkPlease;
            //    return false;
            //}
            //if (slider.Description == "")
            //{
            //    msg = Resources.CP_Sliders.EnterDescrptionPlease;
            //    return false;
            //}
            if (Session["Attachment"] == null && Is_Add)
            {
                msg = Resources.CP_Sliders.EnterImagePlease;
                return false;
            }
            msg = "";
            return flag;
        }

    }
}