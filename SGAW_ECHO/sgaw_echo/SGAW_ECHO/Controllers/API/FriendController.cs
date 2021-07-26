using SGAW_ECHO.Classes;
using SGAW_ECHO.Models.API.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGAW_ECHO.Controllers
{
    public class FriendController : Controller
    {

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Follow_Request()
        {

            if (Request.QueryString["Sender_ID"] == null)
            {
                return Json(new { @msg = "Enter Sender ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (Request.QueryString["Receiver_ID"] == null)
            {
                return Json(new { @msg = "Enter Receiver ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            Guid Sender_ID = new Guid(Request.QueryString["Sender_ID"].ToString());
            Guid Receiver_ID = new Guid(Request.QueryString["Receiver_ID"].ToString());

            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "User_ID", "Friend_User_ID", "Friendship_Date", "Friendship_Statuse_ID", "Date_Of_Create" };
            cols.AddRange(colsinput);

            object[] valsinput = { Sender_ID, Receiver_ID, DateTime.Now.ToShortDateString(), "34cfda02-0146-4a49-a086-4adb6213ac2d", DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);

            Guid follow_ID = Guid.NewGuid();
            DataRow temp = Database.GetRow("Friends", follow_ID);
            while (temp != null)
            {
                follow_ID = Guid.NewGuid();
                temp = Database.GetRow("Friends", follow_ID);
            }
            string msg = "";
            if (Database.InsertRow("Friends", follow_ID, cols, vals, out msg))
            {

                return Json(new { @data = "request sended", @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Accept_Request()
        {

            if (Request.QueryString["Sender_ID"] == null)
            {
                return Json(new { @msg = "Enter Sender ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (Request.QueryString["Receiver_ID"] == null)
            {
                return Json(new { @msg = "Enter Receiver ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            Guid Sender_ID = new Guid(Request.QueryString["Sender_ID"].ToString());
            Guid Receiver_ID = new Guid(Request.QueryString["Receiver_ID"].ToString());

            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "User_ID", "Friend_User_ID", "Date_Of_Update", "Friendship_Statuse_ID" };
            cols.AddRange(colsinput);

            object[] valsinput = { Sender_ID, Receiver_ID, DateTime.Now.ToShortDateString(), "012f955b-8121-48d8-9efb-c59167c4d23d" };
            vals.AddRange(valsinput);


            string msg = "";
            string query = " select ID from Friends where User_ID = @UID and Friend_User_ID =@FID ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@UID", Sender_ID));
            li.Add(new SqlParameter("@FID", Receiver_ID));
            try
            {
                Guid Follow_ID = new Guid(Database.ReadValueByQuery(query, li).ToString());
                if (Database.UpdateRow("Friends", Follow_ID, cols, vals, out msg))
                {
                    return Json(new { @data = "Request Accepted", @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Ignore_Request()
        {

            if (Request.QueryString["Sender_ID"] == null)
            {
                return Json(new { @msg = "Enter Sender ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (Request.QueryString["Receiver_ID"] == null)
            {
                return Json(new { @msg = "Enter Receiver ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            Guid Sender_ID = new Guid(Request.QueryString["Sender_ID"].ToString());
            Guid Receiver_ID = new Guid(Request.QueryString["Receiver_ID"].ToString());

            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "User_ID", "Friend_User_ID", "Date_Of_Update", "Friendship_Statuse_ID" };
            cols.AddRange(colsinput);

            object[] valsinput = { Sender_ID, Receiver_ID, DateTime.Now.ToShortDateString(), "8d0a5072-ac2d-42e2-a56b-08645c407751" };
            vals.AddRange(valsinput);


            string msg = "";
            string query = " select ID from Friends where User_ID = @UID and Friend_User_ID =@FID ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@UID", Sender_ID));
            li.Add(new SqlParameter("@FID", Receiver_ID));
            try
            {
                Guid Follow_ID = new Guid(Database.ReadValueByQuery(query, li).ToString());
                if (Database.UpdateRow("Friends", Follow_ID, cols, vals, out msg))
                {
                    return Json(new { @data = "Request Ignored", @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public JsonResult Get_Follow_Requests()
        {

            if (Request.QueryString["User_ID"] == null)
            {
                return Json(new { @msg = "Enter User ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }

            Guid User_ID = new Guid(Request.QueryString["User_ID"].ToString());

            string query = "select f.User_ID , f.Friendship_Date , u.Full_name , i.URL from Friends as f  " +
                " inner join Users as u on f.User_ID = u.ID left join Images as i" +
                " on u.ID = i.Src_ID where f.Friend_User_ID = @FID and i.Src_Type = 'User' and i.Row_Index = 1";

            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@FID", User_ID));
            string msg = "";
            DataTable follows = Database.ReadTableByQuery(query, li, out msg);
            if (follows != null && follows.Rows.Count > 0)
            {
                List<Follow_RequestsModel> dtList = follows.AsEnumerable()
                       .Select(row => new Follow_RequestsModel
                       {
                           Receiver_ID = row["User_ID"].ToString(),
                           Follow_Date = row["Friendship_Date"].ToString(),
                           FullName = row["Full_name"].ToString(),
                           Image_Url = row["URL"].ToString()
                       }).ToList();
                return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "No follow requests !", @code = 404 },
                                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}