using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.Post
{
    public class PostModel
    {
        public ShowPostModel PopstInfo { get; set; }
        public List<string> Images { get; set; }
        public List<string> Videos { get; set; }
        public List<string> Comments { get; set; }
        public List<string> Likes { get; set; }
    }

    public class GetComments
    {

    }

    public class ShowPostModel
    {
        public string ID { get; set; }
        public string Post_Text { get; set; }
        public string Date_Of_Create { get; set; }
        public string Duration_By_Sec { get; set; }
        public string Post_Privecy_ID { get; set; }
        public string Lang_ID { get; set; }
        public string Edit_History { get; set; }
        public string Comments_Count { get; set; }
        public string Likes_Count { get; set; }
        public string ISLike { get; set; }
        public string Images_Count { get; set; }
        public string Videos_Count { get; set; }
        public Media Media1 { get; set; }
        public Media Media2 { get; set; }
        public Media Media3 { get; set; }
    }

    public class Media
    {
        public string Url { get; set; }
        public string Type { get; set; }
    }

    public class Comment
    {
        public string ID { get; set; }
        public string Text { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_URL { get; set; }
        public string Date { get; set; }
    }

    public class Like
    {
        public string ID { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_URL { get; set; }
    }

    public class Post_Media
    {
        public string Media_ID { get; set; }
        public string Post_ID { get; set; }
        public string Media_Type { get; set; }
        public string Media_URL { get; set; }
    }
    enum MediaType
    {
        Image,Vedio
    }
    public class ShowAvailablePostModel
    {
        public string ID { get; set; }

        public string User_ID { get; set; }

        public string User_Name { get; set; }

        public string User_URL { get; set; }
        public string Post_Text { get; set; }
        public string Date_Of_Create { get; set; }
        public string Duration_By_Sec { get; set; }
        public string Post_Privecy_ID { get; set; }
        public string Lang_ID { get; set; }
        public string Edit_History { get; set; }
        public string ISFollow { get; set; }
        public string ISLike { get; set; }
        public string Comments_Count { get; set; }
        public string Likes_Count { get; set; }
        public string Images_Count { get; set; }
        public string Videos_Count { get; set; }
        public Media Media1 { get; set; }
        public Media Media2 { get; set; }
        public Media Media3 { get; set; }
    }

    public class PostInfoModel
    {
        public string ID { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_URL { get; set; }
        public string Post_Text { get; set; }
        public string Date_Of_Create { get; set; }
        public string Duration_By_Sec { get; set; }
        public string Post_Privecy_ID { get; set; }
        public string Lang_ID { get; set; }
        public string Edit_History { get; set; }
        public List<string> Images { get; set; }
        public List<string> Videos { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
    }

    public class GetPostModel
    {
        public string UserID { get; set; }
        public string SrcID { get; set; }
        public string SrcType { get; set; }
        public string PrivecyID { get; set; }
        public string TypeID { get; set; }
    }

    

    public class AddPostModel
    {
        public Guid Type_ID { get; set; }
        public string Text { get; set; }
        public Guid Privecy_ID { get; set; }
        public DateTime Created_Date = DateTime.Now;
        public Guid User_ID { get; set; }
        public Guid Lang_ID { get; set; }
        public Guid Src_ID { get; set; }
        public string Src_Type { get; set; }
        public string Duration_By_Sec { get; internal set; }
    }
}