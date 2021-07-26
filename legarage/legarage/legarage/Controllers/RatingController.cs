using legarage.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace legarage.Controllers
{
    public class RatingController : Controller
    {
        [HttpGet]
        public ActionResult Rate()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult AddRating()
        {
            try
            {
                if (!Tools.FindCurrentUser(out DataRow userRow)) 
                    return Json(new { @msg = Response.StatusDescription = "Unauthorized!", @code = Response.StatusCode = 401 },
                        JsonRequestBehavior.AllowGet);
                else if (Request.Params["src_id"] == null)
                        return Json(new { @msg = Response.StatusDescription = "Enter Source ID!", @code = Response.StatusCode = 404 },
                            JsonRequestBehavior.AllowGet);
                    else if (Request.Params["src_type"] == null)
                        return Json(new { @msg = Response.StatusDescription = "Enter Source Type!", @code = Response.StatusCode = 404 },
                            JsonRequestBehavior.AllowGet);
                    else if (Request.Params["value"] == null)
                        return Json(new { @msg = Response.StatusDescription = "Enter Value!", @code = Response.StatusCode = 404 },
                            JsonRequestBehavior.AllowGet);


                Guid user_id = new Guid(userRow["id"].ToString());
                Guid src_id = new Guid(Request.Params["src_id"].ToString());
                string src_type = (Request.Params["src_type"].ToString());
                string value = (Request.Params["value"].ToString());


                List<SqlParameter> li = new List<SqlParameter>();
                li.Add(new SqlParameter("@UID", user_id));
                li.Add(new SqlParameter("@src_id", src_id));
                li.Add(new SqlParameter("@src_type", src_type));
                li.Add(new SqlParameter("@value", value));
                string msg = "";
           
                //if (Database.check_rating("rating", "user_id", "src_id", user_id.ToString(), src_id.ToString()) == true)
                if (Database.checkRating(user_id.ToString(), src_id.ToString()))
                    {
                    List<string> colums = new List<string>() { "value", "created_at", "updated_at", "src_id", "user_id", "src_type" };
                    List<object> values = new List<object>() { value, DateTime.Now, DateTime.Now, src_id, user_id, src_type };
                    if (Database.InsertRow("rating", Guid.NewGuid(), colums, values, out msg))
                    {

                        return Json(new { @msg = Response.StatusDescription = "you are successfully rated", @code = Response.StatusCode = 201 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { @msg = Response.StatusDescription = "Bad request!", @code = Response.StatusCode = 400 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { @msg = Response.StatusDescription = "Not allowed to rate twice!", @code = Response.StatusCode = 405 }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { @msg = Response.StatusDescription = e.Message, @code = Response.StatusCode = 500 }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult get_rate_by_id()
        {
            try
            {
                if (Request.QueryString["src_id"] == null)
                {
                    return Json(new { @msg = Response.StatusDescription = "Enter Source ID !", @code = Response.StatusCode = 404 },
                        JsonRequestBehavior.AllowGet);
                }

                Guid src_id = new Guid(Request.QueryString["src_id"].ToString());

                List<SqlParameter> li = new List<SqlParameter>();
                li.Add(new SqlParameter("@src_id", src_id));
                string msg = "";

                if (Database.FindRow("rating", "Src_ID", src_id) != null)
                {
                    DataTable rating = Database.ReadTableByQuery("select avg(value) as Result from Rating where Src_ID  = @src_id", li, out msg);
                    if (rating != null && rating.Rows.Count > 0)
                    {
                        List<string> Result = rating.AsEnumerable()
                           .Select(row => row["Result"].ToString()).ToList();
                        return Json(new { @data = Result, @code = Response.StatusCode = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { @msg = Response.StatusDescription = "No Data" , @code = Response.StatusCode = 404 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { @msg = Response.StatusDescription = "The source id dose not exists", @code = Response.StatusCode = 404 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { @msg = Response.StatusDescription = e.Message , @code = Response.StatusCode = 500 }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}