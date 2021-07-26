using SGAW_ECHO.Classes;
using SGAW_ECHO.Models.API.Post;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SGAW_ECHO.Controllers
{
    public class PostController : Controller
    {
        public enum PostType
        {
            University, Event, Uni_Group, Public
        }
        //SrcId : if public it will be null 
        // else it will be ( event_ID,post_ID,story_ID )

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetPosts()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            GetPostModel posts = new GetPostModel();
            try
            {
                posts = JsonConvert.DeserializeObject<GetPostModel>(json);
                if(posts.UserID ==null || posts.UserID.Trim()== string.Empty)
                    return Json(new { @msg = "enter user id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.TypeID == null || posts.TypeID.Trim() == string.Empty)
                    return Json(new { @msg = "enter type id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.SrcType == null || posts.SrcType.Trim() == string.Empty)
                    posts.SrcType = PostType.Public.ToString();


                string UserPosts = " select p.ID,p.Post_Text,p.Date_Of_Create,(select count(*) from Post_History where Post_ID = p.ID) as history, " +
               " p.Duration_By_Sec , p.Post_Privecy_ID,p.Lang_ID,(select count(*) from Comments where Src_ID = p.ID and Src_Type = 'Post') as comments_count, " +
               " (select count(*) from Likes where Src_ID = p.ID and Src_Type = 'Post') as liks_count, " +
               " (select count(*) from Images where Src_ID = p.ID and Src_Type = 'Post') as Images_count, " +
               " (select count(*) from Videos where Src_ID = p.ID and Src_Type = 'Post') as Videos_count, " +
               " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 1) as Image_URL1, " +
               " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 2) as Image_URL2, " +
               " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 3) as Image_URL3, " +
               " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 1) as Video_URL1, " +
               " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 2) as Video_URL2, " +
               " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 3) as Video_URL3 ," +
                " (select count(*) from Likes where Src_ID=p.ID AND Src_Type='Post' and User_ID=@userID) as ISLike " +
               " from Post as P where p.Src_Type like @srcType and p.Post_Type_ID = @typeID and p.User_ID = @userID";
                if(posts.SrcType!=PostType.Public.ToString() && posts.SrcID != null) UserPosts+=" and p.Src_ID = @srcID ";

                UserPosts += "  ORDER BY p.Date_Of_Create DESC ";

                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@srcType",posts.SrcType.ToString()));
                list.Add(new SqlParameter("@typeID", new Guid(posts.TypeID)));
                list.Add(new SqlParameter("@userID", new Guid(posts.UserID)));
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) list.Add(new SqlParameter("@srcID", new Guid(posts.SrcID)));
                string msg = "";

                DataTable Posts = Database.ReadTableByQuery(UserPosts, list, out msg);

                if (Posts != null && Posts.Rows.Count > 0)
                {
                    List<ShowPostModel> dtList = Posts.AsEnumerable()
                        .Select(row => new ShowPostModel
                        {
                            ID = row["ID"].ToString(),
                            Post_Text = row["Post_Text"].ToString(),
                            Date_Of_Create = row["Date_Of_Create"].ToString(),
                            Edit_History = row["history"].ToString(),
                            Duration_By_Sec = row["Duration_By_Sec"].ToString(),
                            Post_Privecy_ID = row["Post_Privecy_ID"].ToString(),
                            Lang_ID = row["Lang_ID"].ToString(),
                            Comments_Count = row["comments_count"].ToString(),
                            Likes_Count = row["liks_count"].ToString(),
                            Images_Count = row["Images_count"].ToString(),
                            Videos_Count = row["Videos_count"].ToString(),
                            ISLike = row["ISLike"].ToString(),
                            Media1 = row["Image_URL1"] == DBNull.Value && row["Video_URL1"] == DBNull.Value ? null : new Media()
                            {
                                Type = row["Image_URL1"] == DBNull.Value ? "Vedio" : "Image",
                                Url = row["Image_URL1"] == DBNull.Value ? row["Video_URL1"].ToString() : row["Image_URL1"].ToString()
                            },
                            Media2 = row["Image_URL2"] == DBNull.Value && row["Video_URL2"] == DBNull.Value ? null : new Media()
                            {
                                Type = row["Image_URL2"] == DBNull.Value ? "Vedio" : "Image",
                                Url = row["Image_URL2"] == DBNull.Value ? row["Video_URL2"].ToString() : row["Image_URL2"].ToString()
                            },
                            Media3 = row["Image_URL3"] == DBNull.Value && row["Video_URL3"] == DBNull.Value ? null : new Media()
                            {
                                Type = row["Image_URL3"] == DBNull.Value ? "Vedio" : "Image",
                                Url = row["Image_URL3"] == DBNull.Value ? row["Video_URL3"].ToString() : row["Image_URL3"].ToString()
                            }
                        }).ToList();

                    return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }
        //        {
        //    "UserID": "52AD07DB-2278-4F03-BF06-8A9F2B8F7818",
        //   "SrcType": "Public",
        //   "TypeID": "767cb275-6571-4ae7-9e29-4379339cf255"
        //}

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetAvailablePosts()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            GetPostModel posts = new GetPostModel();
            try
            {
                posts = JsonConvert.DeserializeObject<GetPostModel>(json);

                posts = JsonConvert.DeserializeObject<GetPostModel>(json);
                if (posts.UserID == null || posts.UserID.Trim() == string.Empty)
                    posts.UserID = string.Empty;

                if (posts.TypeID == null || posts.TypeID.Trim() == string.Empty)
                    return Json(new { @msg = "enter type id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.SrcType == null || posts.SrcType.Trim() == string.Empty)
                    posts.SrcType = PostType.Public.ToString();
                string user_condetion = posts.UserID == string.Empty ? "" : " or p.User_ID = @userID " +
                " or (p.Post_Privecy_ID = 'e28f6b11-c2c5-4e7d-8174-a6b4a1561afa' and p.User_ID in (select User_ID from Friends where Friend_User_ID = @userID))";

                string follow_And_Like_Condetion = posts.UserID == string.Empty ? "" : " ,(select COUNT(*) from Friends where User_ID = @userID  and Friend_User_ID = p.User_ID) as IsFollow, " +
                " (select count(*) from Likes where Src_ID=p.ID AND Src_Type='Post' and User_ID=@userID) as ISLike ";
                string UserPosts = " select p.ID,p.Post_Text,p.Date_Of_Create,(select count(*) from Post_History where Post_ID = p.ID) as history, "+
                " p.Duration_By_Sec , p.Post_Privecy_ID,p.Lang_ID,u.ID as user_ID,u.Username,(select url from images where Src_ID = u.id and Src_Type = "+
                " 'Users' and Is_Main = 1  and Row_Index = 1 ) as userImage, (select count(*) from Comments where Src_ID = p.ID and Src_Type = 'Post') as "+
                " comments_count,(select count(*) from Likes where Src_ID = p.ID and Src_Type = 'Post') as liks_count,(select count(*) from Images where "+
                " Src_ID = p.ID and Src_Type = 'Post') as Images_count, (select count(*) from Videos where Src_ID = p.ID and Src_Type = 'Post') "+
                " as Videos_count, "+
                " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 1) as Image_URL1, " +
               " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 2) as Image_URL2, " +
               " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 3) as Image_URL3, " +
               " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 1) as Video_URL1, " +
               " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 2) as Video_URL2, " +
               " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 3) as Video_URL3  " +
               follow_And_Like_Condetion+
                " from Post as P inner join Users as U on p.User_ID = u.ID " +
                " where p.Src_Type = @srcType and p.Post_Type_ID = @typeID and( "+
                " p.Post_Privecy_ID = '7043ad6d-5994-4f6b-a02f-071228047610' "+ user_condetion + " )";
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) UserPosts += " and p.Src_ID = @srcID ";

                UserPosts += "  ORDER BY p.Date_Of_Create DESC ";

                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@srcType", posts.SrcType.ToString()));
                list.Add(new SqlParameter("@typeID", new Guid(posts.TypeID)));
                if(posts.UserID != string.Empty)
                list.Add(new SqlParameter("@userID", new Guid(posts.UserID)));
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) list.Add(new SqlParameter("@srcID", new Guid(posts.SrcID)));
                string msg = "";

                DataTable Posts = Database.ReadTableByQuery(UserPosts, list, out msg);

                if (Posts != null && Posts.Rows.Count > 0)
                {
                    List<ShowAvailablePostModel> dtList = Posts.AsEnumerable()
                    .Select(row => new ShowAvailablePostModel
                    {
                        ID = row["ID"].ToString(),
                        Post_Text = row["Post_Text"].ToString(),
                        Date_Of_Create = row["Date_Of_Create"].ToString(),
                        Edit_History = row["history"].ToString(),
                        Duration_By_Sec = row["Duration_By_Sec"].ToString(),
                        Post_Privecy_ID = row["Post_Privecy_ID"].ToString(),
                        Lang_ID = row["Lang_ID"].ToString(),
                        Comments_Count = row["comments_count"].ToString(),
                        Likes_Count = row["liks_count"].ToString(),
                        Images_Count = row["Images_count"].ToString(),
                        Videos_Count = row["Videos_count"].ToString(),
                        Media1 = row["Image_URL1"] == DBNull.Value && row["Video_URL1"] == DBNull.Value ? null : new Media()
                        {
                            Type = row["Image_URL1"] == DBNull.Value ? "Vedio" : "Image",
                            Url = row["Image_URL1"] == DBNull.Value ? row["Video_URL1"].ToString() : row["Image_URL1"].ToString()
                        },
                        Media2 = row["Image_URL2"] == DBNull.Value && row["Video_URL2"] == DBNull.Value ? null : new Media()
                        {
                            Type = row["Image_URL2"] == DBNull.Value ? "Vedio" : "Image",
                            Url = row["Image_URL2"] == DBNull.Value ? row["Video_URL2"].ToString() : row["Image_URL2"].ToString()
                        },
                        Media3 = row["Image_URL3"] == DBNull.Value && row["Video_URL3"] == DBNull.Value ? null : new Media()
                        {
                            Type = row["Image_URL3"] == DBNull.Value ? "Vedio" : "Image",
                            Url = row["Image_URL3"] == DBNull.Value ? row["Video_URL3"].ToString() : row["Image_URL3"].ToString()
                        },
                        User_ID= row["user_ID"].ToString(),
                        User_Name = row["Username"].ToString(),
                        User_URL = row["userImage"].ToString(),
                        ISFollow= posts.UserID == string.Empty?"0": row["IsFollow"].ToString(),
                        ISLike = posts.UserID == string.Empty ? "0" : row["ISLike"].ToString()
                    }).ToList();

                    return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }
        //        {
        //    "UserID": "52AD07DB-2278-4F03-BF06-8A9F2B8F7818",
        //   "SrcType": "Public",
        //   "TypeID": "767cb275-6571-4ae7-9e29-4379339cf255"
        //}


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetFriendsPosts()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            GetPostModel posts = new GetPostModel();
            try
            {
                posts = JsonConvert.DeserializeObject<GetPostModel>(json);
                posts = JsonConvert.DeserializeObject<GetPostModel>(json);
                if (posts.UserID == null || posts.UserID.Trim() == string.Empty)
                    return Json(new { @msg = "enter user id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.TypeID == null || posts.TypeID.Trim() == string.Empty)
                    return Json(new { @msg = "enter type id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.SrcType == null || posts.SrcType.Trim() == string.Empty)
                    posts.SrcType = PostType.Public.ToString();

                string UserPosts = " select p.ID,p.Post_Text,p.Date_Of_Create,(select count(*) from Post_History where Post_ID = p.ID) as history, " +
                     " p.Duration_By_Sec , p.Post_Privecy_ID,p.Lang_ID,u.ID as user_ID,u.Username,(select url from images where Src_ID = u.id and Src_Type = " +
                     " 'Users' and Is_Main = 1  and Row_Index = 1 ) as userImage, (select count(*) from Comments where Src_ID = p.ID and Src_Type = 'Post') as " +
                     " comments_count,(select count(*) from Likes where Src_ID = p.ID and Src_Type = 'Post') as liks_count,(select count(*) from Images where " +
                     " Src_ID = p.ID and Src_Type = 'Post') as Images_count, (select count(*) from Videos where Src_ID = p.ID and Src_Type = 'Post') " +
                     " as Videos_count, " +
                     " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 1) as Image_URL1, " +
                    " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 2) as Image_URL2, " +
                    " (select URL from Images where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 3) as Image_URL3, " +
                    " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 1) as Video_URL1, " +
                    " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 2) as Video_URL2, " +
                    " (select URL from Videos where Src_ID = p.ID and Src_Type = 'Post' and Row_Index = 3) as Video_URL3 ," +
                     " (select count(*) from Likes where Src_ID=p.ID AND Src_Type='Post' and User_ID=@userID) as ISLike " +
                     " from Post as P inner join Users as U on p.User_ID = u.ID " +
                " where p.Src_Type = @srcType and p.Post_Type_ID = @typeID and p.User_ID in (select User_ID from Friends where Friend_User_ID = @userID) ";
                
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) UserPosts += " and p.Src_ID = @srcID ";
                UserPosts += "  ORDER BY p.Date_Of_Create DESC ";
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@srcType", posts.SrcType.ToString()));
                list.Add(new SqlParameter("@typeID", new Guid(posts.TypeID)));
                list.Add(new SqlParameter("@userID", new Guid(posts.UserID)));
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) list.Add(new SqlParameter("@srcID", new Guid(posts.SrcID)));
                string msg = "";

                DataTable Posts = Database.ReadTableByQuery(UserPosts, list, out msg);

                if (Posts != null && Posts.Rows.Count > 0)
                {
                    List<ShowAvailablePostModel> dtList = Posts.AsEnumerable()
                    .Select(row => new ShowAvailablePostModel
                    {
                        ID = row["ID"].ToString(),
                        Post_Text = row["Post_Text"].ToString(),
                        Date_Of_Create = row["Date_Of_Create"].ToString(),
                        Edit_History = row["history"].ToString(),
                        Duration_By_Sec = row["Duration_By_Sec"].ToString(),
                        Post_Privecy_ID = row["Post_Privecy_ID"].ToString(),
                        Lang_ID = row["Lang_ID"].ToString(),
                        Comments_Count = row["comments_count"].ToString(),
                        Likes_Count = row["liks_count"].ToString(),
                        Images_Count = row["Images_count"].ToString(),
                        Videos_Count = row["Videos_count"].ToString(),
                        Media1 = row["Image_URL1"] == DBNull.Value && row["Video_URL1"] == DBNull.Value ? null : new Media()
                        {
                            Type = row["Image_URL1"] == DBNull.Value ? "Vedio" : "Image",
                            Url = row["Image_URL1"] == DBNull.Value ? row["Video_URL1"].ToString() : row["Image_URL1"].ToString()
                        },
                        Media2 = row["Image_URL2"] == DBNull.Value && row["Video_URL2"] == DBNull.Value ? null : new Media()
                        {
                            Type = row["Image_URL2"] == DBNull.Value ? "Vedio" : "Image",
                            Url = row["Image_URL2"] == DBNull.Value ? row["Video_URL2"].ToString() : row["Image_URL2"].ToString()
                        },
                        Media3 = row["Image_URL3"] == DBNull.Value && row["Video_URL3"] == DBNull.Value ? null : new Media()
                        {
                            Type = row["Image_URL3"] == DBNull.Value ? "Vedio" : "Image",
                            Url = row["Image_URL3"] == DBNull.Value ? row["Video_URL3"].ToString() : row["Image_URL3"].ToString()
                        },
                        User_ID = row["user_ID"].ToString(),
                        User_Name = row["Username"].ToString(),
                        User_URL = row["userImage"].ToString(),
                        ISFollow = "1",
                        ISLike = row["ISLike"].ToString()
                    }).ToList();

                    return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }
       
        [HttpPost]
        public JsonResult Add()
        {
            AddPostModel PopstInfo = new AddPostModel();
            try
            {
                //  post.PopstInfo = JsonConvert.DeserializeObject<AddPostModel>(json);
                if (Request.Form != null && Request.Form.Count > 0)
                {
                    if (Request.Form["Type_ID"] != null) PopstInfo.Type_ID = new Guid(Request.Form["Type_ID"]);
                    else return Json(new { @code = 404, msg = "enter type id" });
                    if (Request.Form["User_ID"] != null) PopstInfo.User_ID = new Guid(Request.Form["User_ID"]);
                    else return Json(new { @code = 404, msg = "enter user id" });
                    if (Request.Form["Text"] != null) PopstInfo.Text = Request.Form["Text"];
                    else return Json(new { @code = 404, msg = "enter text" });
                    if (Request.Form["Privecy_ID"] != null) PopstInfo.Privecy_ID = new Guid(Request.Form["Privecy_ID"]);
                    else return Json(new { @code = 404, msg = "enter Privecy id" });
                    if (Request.Form["Lang_ID"] != null) PopstInfo.Lang_ID = new Guid(Request.Form["Lang_ID"]);
                    else return Json(new { @code = 404, msg = "enter language id" });
                    if (Request.Form["Src_ID"] != null) PopstInfo.Src_ID = new Guid(Request.Form["Src_ID"]);
                    else PopstInfo.Src_ID = new Guid();
                    if (Request.Form["Src_Type"] != null) PopstInfo.Src_Type = Request.Form["Src_Type"];
                    else PopstInfo.Src_Type = "Public";
                    if (Request.Form["Duration_By_Sec"] != null) PopstInfo.Duration_By_Sec = Request.Form["Duration_By_Sec"];
                    else PopstInfo.Duration_By_Sec = "0";



                    //// add post info 
                    List<string> cols = new List<string>();
                    List<Object> vals = new List<object>();


                    string[] colsinput = { "Post_Type_ID", "User_ID", "Post_Text", "Post_Privecy_ID", "Lang_ID", "Date_Of_Create", "Src_Type", "Duration_By_Sec" };
                    cols.AddRange(colsinput);

                    object[] valsinput = { PopstInfo.Type_ID, PopstInfo.User_ID, PopstInfo.Text, PopstInfo.Privecy_ID, PopstInfo.Lang_ID, PopstInfo.Created_Date, PopstInfo.Src_Type, PopstInfo.Duration_By_Sec };
                    vals.AddRange(valsinput);

                    if (PopstInfo.Src_ID != new Guid())
                    {
                        cols.Add("Src_ID");
                        vals.Add(PopstInfo.Src_ID);
                    }


                    string errMessage = string.Empty;

                    Guid post_ID = Guid.NewGuid();
                    DataRow temp = Database.GetRow("Post", post_ID);
                    while (temp != null)
                    {
                        post_ID = Guid.NewGuid();
                        temp = Database.GetRow("Post", post_ID);
                    }

                    if (Database.InsertRow("Post", post_ID, cols, vals, out errMessage))
                    {
                        if (Request.Files != null && Request.Files.Count > 0)
                        {
                            int row_index = 1;
                            for (int i = 0; i < Request.Files.Count; i++)
                            {
                                var file = Request.Files[i];
                                if (file.ContentType == null) continue;
                                string type = file.ContentType.Split('/')[0];
                                MemoryStream target = new MemoryStream();
                                file.InputStream.CopyTo(target);
                                byte[] data = target.ToArray();
                                if (type == "image")
                                {
                                    string FileName = file.FileName;
                                    Guid image_id = Guid.NewGuid();
                                    temp = Database.GetRow("Images", image_id);
                                    while (temp != null)
                                    {
                                        image_id = Guid.NewGuid();
                                        temp = Database.GetRow("Images", image_id);
                                    }
                                    // uploade image to server
                                    FileName = image_id.ToString() + System.IO.Path.GetExtension(FileName);
                                    string dir = Server.MapPath("~/Images/Posts/");
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

                                    valsinput = new object[] { FileName, 0, row_index++, post_ID, "Post", DateTime.Now, DateTime.Now };
                                    vals.AddRange(valsinput);

                                    errMessage = string.Empty;


                                    //add image to data base
                                    Database.InsertRow("Images", image_id, cols, vals, out errMessage);

                                }
                                else if (type == "video")
                                {
                                    string FileName = file.FileName;
                                    Guid videoID = Guid.NewGuid();
                                    temp = Database.GetRow("Videos", videoID);
                                    while (temp != null)
                                    {
                                        videoID = Guid.NewGuid();
                                        temp = Database.GetRow("Videos", videoID);
                                    }
                                    // uploade image to server
                                    FileName = videoID.ToString() + System.IO.Path.GetExtension(FileName);
                                    string dir = Server.MapPath("~/Videos/Posts/");
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

                                    valsinput = new object[] { FileName, 0, row_index++, post_ID, "Post", DateTime.Now, DateTime.Now };
                                    vals.AddRange(valsinput);

                                    errMessage = string.Empty;


                                    //add image to data base
                                    Database.InsertRow("Videos", videoID, cols, vals, out errMessage);

                                }
                            }
                        }
                        return Json(new { @data = post_ID, @code = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int code = 404;
                        string msg = "added post  failed" + "<br/>" + errMessage.Replace(Environment.NewLine, "<br/>");
                        return Json(new { @code = code.ToString(), msg = msg });
                    }

                }
                else
                {
                    int code = 404;
                    string msg = "enter post information";
                    return Json(new { @code = code.ToString(), msg = msg });
                }


            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetPostImages()
        {

            if (Request.QueryString["Post_ID"] == null)
            {
                return Json(new { @msg = "Enter Post ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            
            Guid post_ID =new Guid(Request.QueryString["Post_ID"].ToString());

            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@PID", post_ID));
            string msg = "";
            DataTable images = Database.ReadTableByQuery("select URL from Images where Src_Type= 'Post' and Src_ID  = @PID", li, out msg);
            if (images != null && images.Rows.Count >0)
            {
                List<string> Urls = images.AsEnumerable()
                   .Select(row => row["URL"].ToString()).ToList();
                return Json(new { @data = Urls, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "no images", @code = 404 }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetPostVideos()
        {

            if (Request.QueryString["Post_ID"] == null)
            {
                return Json(new { @msg = "Enter Post ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }

            Guid post_ID = new Guid(Request.QueryString["Post_ID"].ToString());

            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@PID", post_ID));
            string msg = "";
            DataTable Videos = Database.ReadTableByQuery("select URL from Videos where Src_Type= 'Post' and Src_ID  = @PID", li, out msg);
            if (Videos != null && Videos.Rows.Count > 0)
            {
                List<string> Urls = Videos.AsEnumerable()
                  .Select(row => row["URL"].ToString()).ToList();
                return Json(new { @data = Urls, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "no Videos", @code = 404 }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [ValidateInput(false)]
        public JsonResult GetPostInfo()
        {

            if (Request.QueryString["Post_ID"] == null)
            {
                return Json(new { @msg = "Enter Post ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }

            Guid post_ID = new Guid(Request.QueryString["Post_ID"].ToString());

            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@PID", post_ID));
            string msg = "";
            string sql = " select p.ID,p.Post_Text,p.Date_Of_Create, " +
                " (select count(*) from Post_History where Post_ID = p.ID) as history, " +
                " p.Duration_By_Sec , p.Post_Privecy_ID,p.Lang_ID,u.ID as user_ID,u.Username, " +
                " (select url from images where Src_ID = u.id and Src_Type ='Users' and Is_Main = 1  and Row_Index = 1 ) as userImage " +
                "  from post as p inner join Users as U on p.User_ID = U.id  where p.ID = @PID ";
            PostInfoModel post = new PostInfoModel();
            DataTable posts = Database.ReadTableByQuery(sql, li, out msg);
            if (posts != null && posts.Rows.Count > 0)
            {
                post.ID = post_ID.ToString();
                post.User_ID = posts.Rows[0]["user_ID"].ToString();
                post.User_Name = posts.Rows[0]["Username"].ToString();
                post.User_URL = posts.Rows[0]["userImage"].ToString();
                post.Post_Text = posts.Rows[0]["Post_Text"].ToString();
                post.Date_Of_Create = posts.Rows[0]["Date_Of_Create"].ToString();
                post.Edit_History = posts.Rows[0]["history"].ToString();
                post.Duration_By_Sec = posts.Rows[0]["Duration_By_Sec"].ToString();
                post.Post_Privecy_ID = posts.Rows[0]["Post_Privecy_ID"].ToString();
                post.Lang_ID = posts.Rows[0]["Lang_ID"].ToString();
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@PID", post_ID));
                DataTable Images = Database.ReadTableByQuery("select URL from images where Src_Type= 'Post' and Src_ID  = @PID", li, out msg);
                if (Images != null && Images.Rows.Count > 0)
                {
                    post.Images = Images.AsEnumerable()
                    .Select(row => row["URL"].ToString()).ToList();
                }
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@PID", post_ID));
                DataTable Videos = Database.ReadTableByQuery("select URL from Videos where Src_Type= 'Post' and Src_ID  = @PID", li, out msg);
                if (Videos != null && Videos.Rows.Count > 0)
                {
                    post.Videos = Videos.AsEnumerable()
                    .Select(row => row["URL"].ToString()).ToList();
                }
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@PID", post_ID));
                sql = " select C.ID as Comment_ID , C.Comment as Comment_Text , U.ID as user_ID, C.Date_Of_Create , " +
                     " U.Username,(select url from images where Src_ID = U.id and Src_Type = 'Users' and Is_Main = 1 "+
                     " and Row_Index = 1 ) as userImage from Comments as C inner join Users as U on C.User_ID = U.ID "+
                     " where C.Src_Type = 'Post' and c.Src_ID = @PID  order by Date_Of_Create";
                DataTable Comments = Database.ReadTableByQuery(sql, li, out msg);
                if (Comments != null && Comments.Rows.Count > 0)
                {
                    post.Comments = Comments.AsEnumerable()
                    .Select(row => new Comment() 
                    {
                        ID = row["Comment_ID"].ToString(),
                        Text = row["Comment_Text"].ToString(),
                        User_ID = row["user_ID"].ToString(),
                        User_Name = row["Username"].ToString(),
                        User_URL = row["userImage"].ToString(),
                        Date= row["Date_Of_Create"].ToString()
                    }).ToList();
                }
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@PID", post_ID));
                sql = " select L.ID as Like_ID , U.ID as user_ID,U.Username, " +
                     " (select url from images where Src_ID = U.id and Src_Type = 'Users' and Is_Main = 1 " +
                     " and Row_Index = 1 ) as userImage from Likes as L inner join Users as U on L.User_ID = U.ID " +
                     " where L.Src_Type = 'Post' and L.Src_ID = @PID";
                DataTable Likes = Database.ReadTableByQuery(sql, li, out msg);
                if (Likes != null && Likes.Rows.Count > 0)
                {
                    post.Likes = Likes.AsEnumerable()
                    .Select(row => new Like()
                    {
                        ID = row["Like_ID"].ToString(),
                        User_ID = row["user_ID"].ToString(),
                        User_Name = row["Username"].ToString(),
                        User_URL = row["userImage"].ToString()
                    }).ToList();
                }
                return Json(new { @data = post, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data not found", @code = 404 }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        [ValidateInput(false)]
        public JsonResult DeletePost()
        {

            if (Request.QueryString["Post_ID"] == null)
            {
                return Json(new { @msg = "Enter Post ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }

            Guid post_ID = new Guid(Request.QueryString["Post_ID"].ToString());
            string msg = "";
            if(Database.DeleteRow("post",post_ID,out msg))
            {
                List<string> cols = new List<string>();
                cols.Add("Src_ID");
                cols.Add("Src_Type");

                List<object> vals = new List<object>();
                vals.Add(post_ID);
                vals.Add("post");
                Database.DeleteRow("images",cols,vals,out msg);
               
                
                msg = "";
                cols = new List<string>();
                cols.Add("Src_ID");
                cols.Add("Src_Type");

                vals = new List<object>();
                vals.Add(post_ID);
                vals.Add("post");
                Database.DeleteRow("Videos", cols, vals, out msg);

                msg = "";
                cols = new List<string>();
                cols.Add("Src_ID");
                cols.Add("Src_Type");

                vals = new List<object>();
                vals.Add(post_ID);
                vals.Add("post");
                Database.DeleteRow("Comments", cols, vals, out msg);


                msg = "";
                cols = new List<string>();
                cols.Add("Src_ID");
                cols.Add("Src_Type");

                vals = new List<object>();
                vals.Add(post_ID);
                vals.Add("post");
                Database.DeleteRow("Likes", cols, vals, out msg);
                return Json(new { @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { @msg = "Data not found", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }
            

        

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetPostsMedia()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            GetPostModel posts = new GetPostModel();
            try
            {
                posts = JsonConvert.DeserializeObject<GetPostModel>(json);
                if (posts.UserID == null || posts.UserID.Trim() == string.Empty)
                    return Json(new { @msg = "enter user id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.TypeID == null || posts.TypeID.Trim() == string.Empty)
                    return Json(new { @msg = "enter type id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.SrcType == null || posts.SrcType.Trim() == string.Empty)
                    posts.SrcType = PostType.Public.ToString();


                string UserMedia = " select ID,URL,'Image' as type , Src_ID ,Date_Of_Create from Images " +
                     " where Src_Type = 'post' and Src_ID in " +
                    " (select ID from post as P where p.Src_Type like @srcType and p.Post_Type_ID = @typeID and p.User_ID = @userID )";
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) UserMedia += " and p.Src_ID = @srcID ";
                UserMedia += " union select ID,URL,'Video' as type , Src_ID , Date_Of_Create from Videos " +
                    " where Src_Type = 'post' and Src_ID in " +
                    " (select ID from post as P where p.Src_Type like @srcType and p.Post_Type_ID = @typeID and p.User_ID = @userID )";
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) UserMedia += " and p.Src_ID = @srcID ";
                UserMedia += "  ORDER BY Date_Of_Create DESC ";
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@srcType", posts.SrcType.ToString()));
                list.Add(new SqlParameter("@typeID", new Guid(posts.TypeID)));
                list.Add(new SqlParameter("@userID", new Guid(posts.UserID)));
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) list.Add(new SqlParameter("@srcID", new Guid(posts.SrcID)));
                string msg = "";

                DataTable Media = Database.ReadTableByQuery(UserMedia, list, out msg);

                if (Media != null && Media.Rows.Count > 0)
                {
                    List<Post_Media> dtList = Media.AsEnumerable()
                        .Select(row => new Post_Media
                        {
                            Media_ID = row["ID"].ToString(),
                            Media_Type = row["type"].ToString(),
                            Media_URL = row["URL"].ToString(),
                            Post_ID = row["Src_ID"].ToString(),

                        }).ToList();

                    return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetAvailablePostsMedia()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            GetPostModel posts = new GetPostModel();
            try
            {
                posts = JsonConvert.DeserializeObject<GetPostModel>(json);
                if (posts.UserID == null || posts.UserID.Trim() == string.Empty)
                    posts.UserID = string.Empty;

                if (posts.TypeID == null || posts.TypeID.Trim() == string.Empty)
                    return Json(new { @msg = "enter type id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.SrcType == null || posts.SrcType.Trim() == string.Empty)
                    posts.SrcType = PostType.Public.ToString();
                string user_condetion = posts.UserID == string.Empty ? "" : " or p.User_ID = @userID " +
                " or (p.Post_Privecy_ID = 'e28f6b11-c2c5-4e7d-8174-a6b4a1561afa' and p.User_ID in (select User_ID from Friends where Friend_User_ID = @userID))";

                string UserMedia = " select ID,URL,'Image' as type , Src_ID ,Date_Of_Create from Images " +
                     " where Src_Type = 'post' and Src_ID in " +
                    " (select ID from post as P where p.Src_Type like @srcType and p.Post_Type_ID = @typeID  "+
                    " and ( p.Post_Privecy_ID = '7043ad6d-5994-4f6b-a02f-071228047610' "+ user_condetion + "))";
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) UserMedia += " and p.Src_ID = @srcID ";
                UserMedia += " union select ID,URL,'Video' as type , Src_ID , Date_Of_Create from Videos " +
                    " where Src_Type = 'post' and Src_ID in " +
                    " (select ID from post as P where p.Src_Type like @srcType and p.Post_Type_ID = @typeID " +
                   " and ( p.Post_Privecy_ID = '7043ad6d-5994-4f6b-a02f-071228047610' " + user_condetion + "))";
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) UserMedia += " and p.Src_ID = @srcID ";
                UserMedia += "  ORDER BY Date_Of_Create DESC ";
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@srcType", posts.SrcType.ToString()));
                list.Add(new SqlParameter("@typeID", new Guid(posts.TypeID)));
                if(posts.UserID != string.Empty)
                list.Add(new SqlParameter("@userID", new Guid(posts.UserID)));
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) list.Add(new SqlParameter("@srcID", new Guid(posts.SrcID)));
                string msg = "";

                DataTable Media = Database.ReadTableByQuery(UserMedia, list, out msg);

                if (Media != null && Media.Rows.Count > 0)
                {
                    List<Post_Media> dtList = Media.AsEnumerable()
                        .Select(row => new Post_Media
                        {
                            Media_ID = row["ID"].ToString(),
                            Media_Type = row["type"].ToString(),
                            Media_URL = row["URL"].ToString(),
                            Post_ID = row["Src_ID"].ToString(),

                        }).ToList();

                    return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetFriendsPostsMedia()
        {
            Stream req = Request.InputStream;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            GetPostModel posts = new GetPostModel();
            try
            {
                posts = JsonConvert.DeserializeObject<GetPostModel>(json);
                if (posts.UserID == null || posts.UserID.Trim() == string.Empty)
                    return Json(new { @msg = "enter user id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.TypeID == null || posts.TypeID.Trim() == string.Empty)
                    return Json(new { @msg = "enter type id", @code = 404 }, JsonRequestBehavior.AllowGet);

                if (posts.SrcType == null || posts.SrcType.Trim() == string.Empty)
                    posts.SrcType = PostType.Public.ToString();


                string UserMedia = " select ID,URL,'Image' as type , Src_ID ,Date_Of_Create from Images " +
                     " where Src_Type = 'post' and Src_ID in " +
                    " (select ID from post as P where p.Src_Type like @srcType and p.Post_Type_ID = @typeID and p.User_ID = @userID " +
                    " and ( p.User_ID in (select User_ID from Friends where Friend_User_ID = @userID))) " ;
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) UserMedia += " and p.Src_ID = @srcID ";
                UserMedia += " union select ID,URL,'Video' as type , Src_ID , Date_Of_Create from Videos " +
                    " where Src_Type = 'post' and Src_ID in " +
                    " (select ID from post as P where p.Src_Type like @srcType and p.Post_Type_ID = @typeID and p.User_ID = @userID " +
                    " and ( p.User_ID in (select User_ID from Friends where Friend_User_ID = @userID))) ";
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) UserMedia += " and p.Src_ID = @srcID ";
                UserMedia += "  ORDER BY Date_Of_Create DESC ";
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@srcType", posts.SrcType.ToString()));
                list.Add(new SqlParameter("@typeID", new Guid(posts.TypeID)));
                list.Add(new SqlParameter("@userID", new Guid(posts.UserID)));
                if (posts.SrcType != PostType.Public.ToString() && posts.SrcID != null) list.Add(new SqlParameter("@srcID", new Guid(posts.SrcID)));
                string msg = "";

                DataTable Media = Database.ReadTableByQuery(UserMedia, list, out msg);

                if (Media != null && Media.Rows.Count > 0)
                {
                    List<Post_Media> dtList = Media.AsEnumerable()
                        .Select(row => new Post_Media
                        {
                            Media_ID = row["ID"].ToString(),
                            Media_Type = row["type"].ToString(),
                            Media_URL = row["URL"].ToString(),
                            Post_ID = row["Src_ID"].ToString(),

                        }).ToList();

                    return Json(new { @data = dtList, @code = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { @msg = "Data Not Found!", @code = 404 }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddLike()
        {

            if (Request.QueryString["Post_ID"] == null)
            {
                return Json(new { @msg = "Enter Post ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (Request.QueryString["User_ID"] == null)
            {
                return Json(new { @msg = "Enter User ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string LikeType = "";
            if (Request.QueryString["Like_Type"] == null)
            {
                LikeType = "Post";
            }
            else
                LikeType = Request.QueryString["Like_Type"].ToString();

           Guid post_ID = new Guid(Request.QueryString["Post_ID"].ToString());
            Guid user_ID = new Guid(Request.QueryString["User_ID"].ToString());
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@USID", user_ID));
            li.Add(new SqlParameter("@PID", post_ID));
            li.Add(new SqlParameter("@type", LikeType));

            object temp2 = Database.ReadValueByQuery("select count(*) from Likes where User_ID = @USID and Src_ID = @PID and Src_Type = @type ", li);
            try
            {
               if(Convert.ToInt32(temp2) > 0)
                    return Json(new { @msg = "Like already added", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            catch ( Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "User_ID", "Like_Type_ID", "Src_ID", "Src_Type", "Date_Of_Create" };
            cols.AddRange(colsinput);

            object[] valsinput = { user_ID, "29a79640-e930-4a19-a675-e397a1a135e8", post_ID, LikeType, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);

            Guid like_ID = Guid.NewGuid();
            DataRow temp = Database.GetRow("Likes", like_ID);
            while (temp != null)
            {
                like_ID = Guid.NewGuid();
                temp = Database.GetRow("Likes", like_ID);
            }
            string msg = "";

            if (Database.InsertRow("Likes", like_ID, cols, vals, out msg))
            {
                return Json(new { @data = "added", @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteLike()
        {

            if (Request.QueryString["Post_ID"] == null)
            {
                return Json(new { @msg = "Enter Post ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (Request.QueryString["User_ID"] == null)
            {
                return Json(new { @msg = "Enter User ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string LikeType = "";
            if (Request.QueryString["Like_Type"] == null)
            {
                LikeType = "Post";
            }
            else
                LikeType = Request.QueryString["Like_Type"].ToString();

            Guid post_ID = new Guid(Request.QueryString["Post_ID"].ToString());
            Guid user_ID = new Guid(Request.QueryString["User_ID"].ToString());
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@USID", user_ID));
            li.Add(new SqlParameter("@PID", post_ID));
            li.Add(new SqlParameter("@type", LikeType));

            object temp2 = Database.ReadValueByQuery("select count(*) from Likes where User_ID = @USID and Src_ID = @PID and Src_Type = @type ", li);
            try
            {
                if (Convert.ToInt32(temp2) == 0)
                    return Json(new { @msg = "Like already deleted or not added ", @code = 404 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { @msg = ex.Message, @code = 404 }, JsonRequestBehavior.AllowGet);
            }

            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "User_ID", "Like_Type_ID", "Src_ID", "Src_Type" };
            cols.AddRange(colsinput);

            object[] valsinput = { user_ID, "29a79640-e930-4a19-a675-e397a1a135e8", post_ID, LikeType };
            vals.AddRange(valsinput);

            Guid like_ID = Guid.NewGuid();
            DataRow temp = Database.GetRow("Likes", like_ID);
            while (temp != null)
            {
                like_ID = Guid.NewGuid();
                temp = Database.GetRow("Likes", like_ID);
            }
            string msg = "";

            if (Database.DeleteRow("Likes", cols, vals, out msg))
            {
                return Json(new { @data = "Deleted", @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddComment()
        {

            if (Request.QueryString["Post_ID"] == null)
            {
                return Json(new { @msg = "Enter Post ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (Request.QueryString["User_ID"] == null)
            {
                return Json(new { @msg = "Enter User ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (Request.QueryString["Text"] == null)
            {
                return Json(new { @msg = "Enter Comment Text !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            string Comment_Type = "";
            if (Request.QueryString["Comment_Type"] == null)
            {
                Comment_Type = "Post";
            }
            else
                Comment_Type = Request.QueryString["Comment_Type"].ToString();


            Guid post_ID = new Guid(Request.QueryString["Post_ID"].ToString());
            Guid user_ID = new Guid(Request.QueryString["User_ID"].ToString());
            string Text = Request.QueryString["Text"].ToString();
            

            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();

            string[] colsinput = { "Comment", "Src_ID", "Src_Type", "Date_Of_Create", "User_ID" };
            cols.AddRange(colsinput);

            object[] valsinput = { Text, post_ID,Comment_Type, DateTime.Now.ToShortDateString(),  user_ID};
            vals.AddRange(valsinput);

            Guid Comment_ID = Guid.NewGuid();
            DataRow temp = Database.GetRow("Comments", Comment_ID);
            while (temp != null)
            {
                Comment_ID = Guid.NewGuid();
                temp = Database.GetRow("Comments", Comment_ID);
            }
            string msg = "";

            if (Database.InsertRow("Comments", Comment_ID, cols, vals, out msg))
            {
                return Json(new { @data = Comment_ID, @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateComment()
        {

            if (Request.QueryString["Comment_ID"] == null)
            {
                return Json(new { @msg = "Enter Comment ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            if (Request.QueryString["Text"] == null)
            {
                return Json(new { @msg = "Enter Comment Text !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
           


            Guid Comment_ID = new Guid(Request.QueryString["Comment_ID"].ToString());
            string Text = Request.QueryString["Text"].ToString();
           

            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();
            

            string[] colsinput = { "Comment", "Date_Of_Update" };
            cols.AddRange(colsinput);

            object[] valsinput = { Text, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);

            string msg = "";

            if (Database.UpdateRow("Comments", Comment_ID, cols, vals, out msg))
            {
                return Json(new { @data = "Updated", @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteComment()
        {

            if (Request.QueryString["Comment_ID"] == null)
            {
                return Json(new { @msg = "Enter Comment ID !", @code = 404 },
                    JsonRequestBehavior.AllowGet);
            }
            Guid Comment_ID = new Guid(Request.QueryString["Comment_ID"].ToString());

            string msg = "";

            if (Database.DeleteRow("Comments", Comment_ID,  out msg))
            {
                return Json(new { @data = "Deleted", @code = 200 }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { @msg = msg, @code = 404 }, JsonRequestBehavior.AllowGet);
        }
    }
}
