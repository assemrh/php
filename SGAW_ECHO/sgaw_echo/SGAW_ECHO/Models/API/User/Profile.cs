using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.User
{  

    public class ShowProfile
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Profile_Url { get; set; }
        public string Cover_Url { get; set; }
        public string ISFollow  { get; set; }
        public int Post_Count { get; set; }
        public int Followers_Count { get; set; }
        public int Followings_Count { get; set; }

    }

    public class EditProfile
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Accoint_Info
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string URL { get; set; }
        public int Bio { get; set; }
        public int Email { get; set; }
        public int Password { get; set; }
    }
}