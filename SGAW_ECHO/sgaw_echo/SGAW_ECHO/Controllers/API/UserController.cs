using Newtonsoft.Json;
using SGAW_ECHO.Classes;
using SGAW_ECHO.Models;
using SGAW_ECHO.Models.API.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using static SGAW_ECHO.Classes.HelperClass;


namespace SGAW_ECHO.Controllers
{
    public class UserController : Controller
    {
        // GET/post: test
        [HttpPost]
        public JsonResult Test()
        {
            return Json(new { @code = 200, msg = "test" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SignUp()
        {

            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            SignUpModel user = new SignUpModel();
            try
            {
                user = JsonConvert.DeserializeObject<SignUpModel>(json);
                int code;
                string msg;
                user.Password = Ciphering.GetMD5HashData(user.Password);
                DataRow dtr = Database.FindRow("Users", "Username", user.UserName);
                if (dtr != null)
                {
                    code = 404;
                    msg = "user name is already exist";
                    return Json(new { @code = code.ToString(), msg = msg });
                }

                dtr = Database.FindRow("Users", "Email", user.Email);
                if (dtr != null)
                {
                    code = 404;
                    msg = "email  is already exist";
                    return Json(new { @code = code.ToString(), msg = msg });
                }

                string Token = RandomString(50);

                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();


                string[] colsinput = { "Username", "Token", "Full_name", "Email", "Password", "User_Type_ID", "Date_Of_Create", "Date_Of_Update" };
                cols.AddRange(colsinput);

                object[] valsinput = { user.UserName, Token, user.FullName, user.Email, user.Password, "02dd85fe-6f1b-41ee-bee6-df82161467ce", DateTime.Now, DateTime.Now };
                vals.AddRange(valsinput);

                string errMessage = string.Empty;

                Guid User_ID = Guid.NewGuid();
                DataRow temp = Database.GetRow("Users", User_ID);
                while (temp != null)
                {
                    User_ID = Guid.NewGuid();
                    temp = Database.GetRow("Users", User_ID);
                }

                if (Database.InsertRow("Users", User_ID, cols, vals, out errMessage))
                {
                    Token token = new Token() { User_ID = User_ID.ToString(), User_Token = Token };
                    return Json(new { @data = token, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 404;
                    msg = "regestration failed" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                    return Json(new { @code = code.ToString(), msg = msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }
                /*{
            "UserName": "Test_Man2",
            "FullName": "Test Man",
            "Email": "tm2@test.com",
            "Password": "123456789"
        }*/



        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Login(LoginModel user)
        {

            if (user.Email == null)
            {
                return Json(new { @msg = "Enter email !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (user.Password == null)
            {
                return Json(new { @msg = "Enter password!", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            //string email = Request.QueryString["email"];
            //string password = Request.QueryString["password"];

            DataRow dtr = Database.FindRow("Users", "Email", user.Email);
            if (dtr != null)
            {
                if (dtr["Password"].ToString() == Ciphering.GetMD5HashData(user.Password))
                {
                    string token = dtr["token"].ToString();
                    string userId = dtr["ID"].ToString();
                    Token data = new Token() { User_ID = userId, User_Token = token };

                    return Json(new { @data = data, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "passowrd incorrect!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { @msg = "Invalied Email !", @code = 404 }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ChangePassword()
        {
            string userID = Request.QueryString["UserID"];
            string password = Request.QueryString["Password"];
            string newPassword = Request.QueryString["NewPassword"];

            if (userID == null)
            {
                return Json(new { @msg = "Enter ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            if (password != null)
            {
                DataRow userRow = Database.FindRow("Users", "ID", userID);
                if (userRow["Password"].ToString() == Ciphering.GetMD5HashData(password))
                {
                    List<string> cols = new List<string>();
                    List<object> vals = new List<object>();
                    cols.Add("Password");
                    vals.Add(Ciphering.GetMD5HashData(newPassword));
                    string msg = "";
                    Guid ID = new Guid(userID);
                    if (Database.UpdateRow("Users", ID, cols, vals, out msg))
                    {
                        return Json(new { @code = 200, @msg = "Password has changed!" });
                    }
                    else
                    {
                        return Json(new { @code = 404, @msg = "Failed!" });
                    }
                }   
                else
                {
                    return Json(new { @code = 404, @msg = "Password didn't match!" });
                }
            }
            else
            {
                return Json(new { @code = 404, @msg = "Enter Old And New Password!" });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditProfile()
        {
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();
            EditProfile profile = new EditProfile();

            try
            {
                int code;
                string msg;
                profile = JsonConvert.DeserializeObject<EditProfile>(json);

                if (profile.UserID == null)
                {
                    return Json(new { @msg = "Enter ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }

                DataRow userRow = Database.FindRow("Users", "ID", profile.UserID);
                
                if (userRow["Password"].ToString() == Ciphering.GetMD5HashData(profile.Password))
                {
                    if (userRow["Username"].ToString() != profile.UserName)
                    {
                        userRow = Database.FindRow("Users", "Username", profile.UserName);
                        if (userRow != null)
                        {
                            code = 404;
                            msg = "Username is already exists!";
                            return Json(new { @code = code.ToString(), @msg = msg });
                        }
                    }
                    
                    if (userRow["Email"].ToString() != profile.Email)
                    {
                        userRow = Database.FindRow("Users", "Email", profile.Email);
                        if (userRow != null)
                        {
                            code = 404;
                            msg = "Email is already exists!";
                            return Json(new { @code = code.ToString(), @msg = msg });
                        }
                    }                    

                    List<string> cols = new List<string>();
                    List<object> vals = new List<object>();

                    string[] colsName = { "Username", "Full_name", "Email", "Bio", "Password" };
                    string password = Ciphering.GetMD5HashData(profile.Password);
                    object[] valsInput = { profile.UserName, profile.FullName, profile.Email, profile.Bio, password };

                    cols.AddRange(colsName);
                    vals.AddRange(valsInput);

                    string errMsg = string.Empty;
                    Guid userId = new Guid(profile.UserID);
                    if (Database.UpdateRow("Users", userId, cols, vals, out errMsg))
                    {
                        code = 200;
                        msg = "Profile updated!";
                        return Json(new { @code = code.ToString(), @msg = msg }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        code = 404;
                        msg = "Update failed!";
                        return Json(new { @code = code.ToString(), @msg = msg });
                    }
                }
                else
                {
                    code = 404;
                    msg = "Password didn't match!";
                    return Json(new { @code = code.ToString(), msg = msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @code = 404, @msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ChangeProfilePic()
        {
            Guid USID;
            if (Request.Form != null && Request.Form.Count > 0)
            {
                if (Request.Form["UserID"] != null) USID = new Guid(Request.Form["UserID"]);
                else return Json(new { @code = 404, msg = "enter user id" });
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file.ContentType == null)
                    {
                        return Json(new { @msg = "Enter a valied Image", @code = 404 }, JsonRequestBehavior.AllowGet);
                    }
                    string type = file.ContentType.Split('/')[0];
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    if (type == "image")
                    {
                        /// check if ther is an old image
                        string sql_ = "select url from images where Src_ID = @UID and Src_Type = 'Users' and  Is_Main = 1  and Row_Index=1 ";
                        List<SqlParameter> li = new List<SqlParameter>();
                        li.Add(new SqlParameter("@UID", USID));
                        string msg = "";
                        DataTable dataTable = Database.ReadTableByQuery(sql_, li, out msg);
                        if(dataTable != null && dataTable.Rows.Count>0)
                        {
                            DataRow oldImg = dataTable.Rows[0];
                            List<string> cols1 = new List<string>();
                            List<Object> vals1 = new List<object>();


                            string[] colsinput1 = { "Src_ID", "Src_Type", "Is_Main", "Row_Index" };
                            cols1.AddRange(colsinput1);

                            object[] valsinput1 = { USID, "Users", 1, 1  };
                            vals1.AddRange(valsinput1);

                            string errMessage1 = string.Empty;
                            Database.DeleteRow("images", cols1, vals1, out errMessage1);
                            string oldUrl = oldImg["url"].ToString();
                            oldUrl = Server.MapPath("~/Images/Users/Profile/" + oldUrl);
                            if (System.IO.File.Exists(oldUrl))
                            {
                                System.IO.File.Delete(oldUrl);
                            }

                        }
                        
                        string FileName = file.FileName;
                        Guid image_id = Guid.NewGuid();
                        DataRow temp = Database.GetRow("Images", image_id);
                        while (temp != null)
                        {
                            image_id = Guid.NewGuid();
                            temp = Database.GetRow("Images", image_id);
                        }
                        // uploade image to server
                        FileName = image_id.ToString() + System.IO.Path.GetExtension(FileName);
                        string dir = Server.MapPath("~/Images/Users/Profile/");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        // FileName = dir + FileName;
                        System.IO.File.WriteAllBytes(dir + FileName, data);
                        List<string> cols = new List<string>();
                        List<object> vals = new List<object>();


                        string[] colsinput = new string[] { "URL", "Is_Main", "Row_Index", "Src_ID", "Src_Type", "Date_Of_Create", "Date_Of_Update" };
                        cols.AddRange(colsinput);

                        object[] valsinput = new object[] { FileName, 1, 1, USID, "Users", DateTime.Now, DateTime.Now };
                        vals.AddRange(valsinput);

                        string errMessage = string.Empty;


                        //add image to data base
                        if (Database.InsertRow("Images", image_id, cols, vals, out errMessage))
                            return Json(new { @data = "added", @code = 200 }, JsonRequestBehavior.AllowGet);
                        else
                        {
                            return Json(new { @msg = "not Added <br>" + errMessage, @code = 404 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { @msg = "Enter a valied Image", @code = 404 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                return Json(new { @msg = "Enter an Image", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { @code = 404, msg = "enter user id" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ChangeCoverPic()
        {
            Guid USID;
            if (Request.Form != null && Request.Form.Count > 0)
            {
                if (Request.Form["UserID"] != null) USID = new Guid(Request.Form["UserID"]);
                else return Json(new { @code = 404, msg = "enter user id" });
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file.ContentType == null)
                    {
                        return Json(new { @msg = "Enter a valied Image", @code = 404 }, JsonRequestBehavior.AllowGet);
                    }
                    string type = file.ContentType.Split('/')[0];
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    if (type == "image")
                    {
                        /// check if ther is an old image
                        string sql_ = "select url from images where Src_ID = @UID and Src_Type = 'Users' and  Is_Main = 1  and Row_Index=2 ";
                        List<SqlParameter> li = new List<SqlParameter>();
                        li.Add(new SqlParameter("@UID", USID));
                        string msg = "";
                        DataTable dataTable = Database.ReadTableByQuery(sql_, li, out msg);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            DataRow oldImg = dataTable.Rows[0];
                            List<string> cols1 = new List<string>();
                            List<Object> vals1 = new List<object>();


                            string[] colsinput1 = { "Src_ID", "Src_Type", "Is_Main", "Row_Index" };
                            cols1.AddRange(colsinput1);

                            object[] valsinput1 = { USID, "Users", 1, 2 };
                            vals1.AddRange(valsinput1);

                            string errMessage1 = string.Empty;
                            Database.DeleteRow("images", cols1, vals1, out errMessage1);
                            string oldUrl = oldImg["url"].ToString();
                            oldUrl = Server.MapPath("~/Images/Users/Cover/" + oldUrl);
                            if (System.IO.File.Exists(oldUrl))
                            {
                                System.IO.File.Delete(oldUrl);
                            }

                        }
                        string FileName = file.FileName;
                        Guid image_id = Guid.NewGuid();
                        DataRow temp = Database.GetRow("Images", image_id);
                        while (temp != null)
                        {
                            image_id = Guid.NewGuid();
                            temp = Database.GetRow("Images", image_id);
                        }
                        // uploade image to server
                        FileName = image_id.ToString() + System.IO.Path.GetExtension(FileName);
                        string dir = Server.MapPath("~/Images/Users/Cover/");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        // FileName = dir + FileName;
                        System.IO.File.WriteAllBytes(dir + FileName, data);
                        List<string> cols = new List<string>();
                        List<object> vals = new List<object>();


                        string[] colsinput = new string[] { "URL", "Is_Main", "Row_Index", "Src_ID", "Src_Type", "Date_Of_Create", "Date_Of_Update" };
                        cols.AddRange(colsinput);

                        object[] valsinput = new object[] { FileName, 1, 2, USID, "Users", DateTime.Now, DateTime.Now };
                        vals.AddRange(valsinput);

                        string errMessage = string.Empty;


                        //add image to data base
                        if (Database.InsertRow("Images", image_id, cols, vals, out errMessage))
                            return Json(new { @data = "added", @code = 200 }, JsonRequestBehavior.AllowGet);
                        else
                        {
                            return Json(new { @msg = "not Added <br>" + errMessage, @code = 404 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { @msg = "Enter a valied Image", @code = 404 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { @msg = "Enter an Image", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { @code = 404, msg = "enter user id" });
        }


        [HttpGet]
        [ValidateInput(false)]
        public JsonResult ShowProfile()
        {
            if (Request.QueryString["UserID"] == null)
            {
                return Json(new { @msg = "Enter ID!", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string Current_UserID = "";
            if (Request.QueryString["Current_UserID"] == null)
                Current_UserID = Request.QueryString["UserID"];
            else
                Current_UserID = Request.QueryString["Current_UserID"];

            string UserID = Request.QueryString["UserID"];
            string sql = "select U.ID, U.Full_name, U.Username , u.Bio,u.Email , ";
            sql += "(select COUNT(*) from Friends where User_ID = @Current_UserID  and Friend_User_ID = @USID) as IsFollow,";
            sql += "(select URL from Images where Src_ID = u.ID and Src_Type = 'Users' ";
            sql += " and Is_Main = 1 and Row_Index = 1) AS Profile,(select URL from Images ";
            sql += " where Src_ID = u.ID and Src_Type = 'Users' and Is_Main = 1 and Row_Index = 2) AS Cover, ";
            sql += " (select COUNT(*) from post where User_ID = u.ID and Post_Type_ID = ";
            sql += "'767cb275-6571-4ae7-9e29-4379339cf255' and Src_Type = 'Public') as Post_Cnts,";
            sql += " (select COUNT(*) from Friends where User_ID = u.id and Friendship_Statuse_ID = ";
            sql += "'012f955b-8121-48d8-9efb-c59167c4d23d'  ) as Followers_Count,(select COUNT(*) from Friends";
            sql += " where Friend_User_ID = u.id and Friendship_Statuse_ID = '012f955b-8121-48d8-9efb-c59167c4d23d' ";
            sql+=" ) as Followings_Count from Users as U where u.id = @USID";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@USID", UserID));
            li.Add(new SqlParameter("@Current_UserID", Current_UserID));
            string msg = "";
            DataTable users = Database.ReadTableByQuery(sql, li, out msg);
            if(users!= null && users.Rows.Count > 0)
            {
                DataRow rUser = users.Rows[0];
                ShowProfile user = new ShowProfile()
                {
                    UserName = rUser["Username"].ToString(),
                    FullName = rUser["Full_name"].ToString(),
                    Bio = rUser["Bio"].ToString(),
                    Email = rUser["Email"].ToString(),
                    Profile_Url = rUser["Profile"].ToString(),
                    Cover_Url = rUser["Cover"].ToString(),
                    ISFollow = rUser["IsFollow"].ToString(),
                    Post_Count = Convert.ToInt32(rUser["Post_Cnts"].ToString()),
                    Followers_Count = Convert.ToInt32(rUser["Followers_Count"].ToString()),
                    Followings_Count = Convert.ToInt32(rUser["Followings_Count"].ToString())
                };
                return Json(new { @data = user, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
               
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetAccountInformation()
        {
            if (Request.QueryString["UserID"] == null)
            {
                return Json(new { @msg = "Enter ID!", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }

            string UserID = Request.QueryString["UserID"];

            DataRow rUser = Database.FindRow("Users", "ID", UserID);
            if (rUser != null)
            {
                string userName = rUser["Username"].ToString();
                string fullName = rUser["Full_name"].ToString();
                string bio = rUser["Bio"].ToString();
                string email = rUser["Email"].ToString();
                ShowProfile user = new ShowProfile()
                {
                    UserName = userName,
                    FullName = fullName,
                    Bio = bio,
                    Email = email,
                };
                return Json(new { @data = user, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllUsers()
        {
            string sql = "";
            string msg;
            sql += "select U.id, full_name,email,mobile,username,bio, (select  N.Neighborhood_Name + ' , ' + AD.Address_Details from Neighborhoods as n " +
                    " inner join Addresses as ad on n.ID = ad.Neighborhood_ID) AS address from Users AS U" +
                    " where u.User_Type_ID <> 'D07DACE1-64E9-5CFA-B55A-7178E31D0034'";
            DataTable users = Database.ReadTableByQuery(sql, null, out msg);            
            if (users != null && users.Rows.Count > 0)
            {                
                List<UserModel> uList = new List<UserModel>();
                foreach (DataRow user in users.Rows)
                {
                    uList = users.AsEnumerable().Select(row => new UserModel
                    {
                        FullName = row["full_name"].ToString(),
                        Email = row["email"].ToString(),
                        Phone = row["mobile"].ToString(),
                        Address = row["address"].ToString(),
                        UserName = row["username"].ToString(),
                        Bio = row["bio"].ToString(),
                        ID = new Guid(row["id"].ToString())
                    }).ToList();                    
                }
                return Json(new { @code = 200, @data = uList }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
            }         
        }


        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult AddUser()
        {
            string msg = string.Empty;
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();
            AddUserModel User = new AddUserModel();
            User = JsonConvert.DeserializeObject<AddUserModel>(json);
            try
            {
                User = JsonConvert.DeserializeObject<AddUserModel>(json);

                    if (IsObjectNullOrEmpty(User.UserName))
                    return Json(new { @code = 404, msg = "enter user name" });
                    if (IsObjectNullOrEmpty(User.FullName))
                     return Json(new { @code = 404, msg = "enter full name" });
                    if (IsObjectNullOrEmpty(User.Email))
                     return Json(new { @code = 404, msg = "enter email" });
                    if (IsObjectNullOrEmpty(User.Password))
                     return Json(new { @code = 404, msg = "enter password" });
                    if (IsObjectNullOrEmpty(User.Bio))
                     User.Bio = "";

                    DataRow dtr = Database.FindRow("Users", "Username", User.UserName);
                    if (dtr != null)
                    {
                       int code = 404;
                         msg = "user name is already exist";
                        return Json(new { @code = code.ToString(), msg = msg });
                    }

                    dtr = Database.FindRow("Users", "Email", User.Email);
                    if (dtr != null)
                    {
                        int code = 404;
                         msg = "email  is already exist";
                        return Json(new { @code = code.ToString(), msg = msg });
                    }

                    //// add user info 
                    List<string> cols = new List<string>();
                    List<Object> vals = new List<object>();


                    string[] colsinput = { "Username", "Full_name", "Email", "Password", "bio", "User_Type_ID", "Token", "Date_Of_Create" };
                    cols.AddRange(colsinput);
                    string Token = RandomString(50);

                    object[] valsinput = { User.UserName,User.FullName,User.Email,User.Password,User.Bio, "02dd85fe-6f1b-41ee-bee6-df82161467ce",Token,DateTime.Now.ToShortDateString() };
                    vals.AddRange(valsinput);

                    User.ID =Guid.NewGuid();
                    string errMessage = string.Empty;

                    DataRow temp = Database.GetRow("Users", User.ID);
                    while (temp != null)
                    {
                        User.ID = Guid.NewGuid();
                        temp = Database.GetRow("Users", User.ID);
                    }

                    if (Database.InsertRow("Users", User.ID, cols, vals, out errMessage))
                    {
                        byte[] data = Convert.FromBase64String(User.Image.Data);
                        string FileName = User.Image.FileName;
                        Guid image_id = Guid.NewGuid();
                        temp = Database.GetRow("Images", image_id);
                        while (temp != null)
                        {
                            image_id = Guid.NewGuid();
                            temp = Database.GetRow("Images", image_id);
                        }
                        // uploade image to server
                        FileName = image_id.ToString() + System.IO.Path.GetExtension(FileName);
                        string dir = Server.MapPath("~/Images/Users/Profile/");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        // FileName = dir + FileName;
                        System.IO.File.WriteAllBytes(dir + FileName, data);
                        cols = new List<string>();
                        vals = new List<object>();


                        colsinput = new string[] { "URL", "Is_Main", "Row_Index", "Src_ID", "Src_Type", "Date_Of_Create", "Date_Of_Update" };
                        cols.AddRange(colsinput);

                        valsinput = new object[] { FileName, 1, 1, User.ID, "Users", DateTime.Now, DateTime.Now };
                        vals.AddRange(valsinput);

                        errMessage = string.Empty;


                        //add image to data base
                        Database.InsertRow("Images", image_id, cols, vals, out errMessage);
                        return Json(new { @data = User.ID, @code = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int code = 404;
                         msg = "added User  failed" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                        return Json(new { @code = code.ToString(), msg = msg });
                    }


            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditUser()
        {
            string msg = string.Empty;
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();
            UserModel user = new UserModel();
            user = JsonConvert.DeserializeObject<UserModel>(json);
            try
            {
                int code;
                string errMessage;
                if (user.ID == null)
                    return Json(new { @msg = "Enter ID!", @code = 404 }, JsonRequestBehavior.AllowGet);
                if (IsObjectNullOrEmpty(user.UserName)) 
                    return Json(new { @code = 404, msg = "enter user name" });
                if (IsObjectNullOrEmpty(user.FullName))
                    return Json(new { @code = 404, msg = "enter full name" });
                if (IsObjectNullOrEmpty(user.Email))
                    return Json(new { @code = 404, msg = "enter email" });

                DataRow userRow = Database.FindRow("Users", "ID", user.ID);
                if(userRow==null)
                    return Json(new { @code = 404, msg = "ID is not correct" });

                if(user.UserName != userRow["Username"].ToString())
                {
                    if (Database.FindRow("Users", "Username", user.UserName) != null)
                    {
                        code = 404;
                        errMessage = "user name is already exist";
                        return Json(new { @code = code.ToString(), msg = errMessage });
                    }
                }

                if (user.Email != userRow["Email"].ToString())
                {
                    if (Database.FindRow("Users", "Email", user.Email) != null)
                    {
                        code = 404;
                        errMessage = "Email is already exist";
                        return Json(new { @code = code.ToString(), msg = errMessage });
                    }
                }

              

                List<string> cols = new List<string>() { "Username", "Full_name", "Email" , "Bio", "Date_Of_Update" };
                List<object> vals = new List<object>() { user.UserName, user.FullName, user.Email, user.Bio, DateTime.Now.ToShortDateString() };

                if (Database.UpdateRow("Users", user.ID, cols, vals, out errMessage))
                {
                    code = 200;
                    msg = "Profile updated!";
                    return Json(new { @code = code.ToString(), @msg = msg }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    code = 404;
                    msg = "Update failed!";
                    return Json(new { @code = code.ToString(), @msg = msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { @code = 404, @msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public JsonResult DeleteUser()
        {
            if (Request.QueryString["ID"] == null)
            {
                return Json(new { @msg = "Enter User ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                Guid UserID = new Guid(Request.QueryString["ID"].ToString());
                string msg = "";
                if (Database.DeleteRow("Users", UserID, out msg))
                {
                    List<string> cols = new List<string>();
                    cols.Add("Src_ID");
                    cols.Add("Src_Type");

                    List<object> vals = new List<object>();
                    vals.Add(UserID);
                    vals.Add("Users");
                    Database.DeleteRow("images", cols, vals, out msg);

                    msg = "";
                    cols = new List<string>();
                    cols.Add("Src_ID");
                    cols.Add("Src_Type");

                    vals = new List<object>();
                    vals.Add(UserID);
                    vals.Add("Users");
                    Database.DeleteRow("Users", cols, vals, out msg);

                    return Json(new { @msg = "Deleted", @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "Not deleted:" + msg, @code = 404 }, JsonRequestBehavior.AllowGet);
                }
            }


        }
        bool ISUserValid(AddUserModel user, out string msg)
        {
            bool flag = true;
            if (user.UserName == "")
            {

                msg = "Enter Username";
                return false;

            }
            if (user.FullName == "")
            {

                msg = "Enter Full Name";
                return false;
            }
            if (user.Email == "")
            {

                msg = "Enter Email";
                return false;
            }
            if (user.Password == "")
            {
                msg = "Enter Password";
                return false;
            }
            if (user.Phone == "")
            {
                msg = "Enter Phone";
                return false;
            }
            //if (user.Address.Neighborhood_ID == null || user.Address.Neighborhood_ID == new Guid())
            //{
            //    msg = "Enter Neighborhood";
            //    return false;
            //}
            //if (user.Address.Descreption == "")
            //{
            //    msg = "Enter Address";
            //    return false;
            //}
            msg = "";
            return flag;
        }

        //for cpanel 
        [HttpGet]
        public JsonResult GetUserInformation()
        {
            if (Request.QueryString["UserID"] == null)
            {
                return Json(new { @msg = "Enter ID!", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }

            string UserID = Request.QueryString["UserID"];

            string sql = "select U.ID, U.Full_name, U.Username , u.Bio,u.Email , ";
            sql += "(select URL from Images where Src_ID = u.ID and Src_Type = 'Users' ";
            sql += " and Is_Main = 1 and Row_Index = 1) AS Profile ";
            sql+=" from Users as U where u.id = @USID";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@USID", UserID));
            string msg = "";
            DataTable users = Database.ReadTableByQuery(sql, li, out msg);
            if(users != null && users.Rows.Count > 0)
            {
                DataRow rUser = users.Rows[0];
                if (rUser != null)
                {
                    string userName = rUser["Username"].ToString();
                    string fullName = rUser["Full_name"].ToString();
                    string bio = rUser["Bio"].ToString();
                    string email = rUser["Email"].ToString();
                    UserModel user = new UserModel()
                    {
                        ID = new Guid(UserID),
                        UserName = userName,
                        FullName = fullName,
                        Bio = bio,
                        Email = email,
                        profile = rUser["Profile"].ToString()
                };
                    return Json(new { @data = user, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
            }


        }

    }
}