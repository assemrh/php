using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class AddUserModel:editUserModel
    {
     
        public string Password { get; set; } = "";

    }

    public class editUserModel
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public DateTime DOB { get; set; } = new DateTime();
        public Guid Country { get; set; } = new Guid();
        public string Gender { get; set; } = "";

    }


    public class UserModel
    {
        public string ID { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string  DOB { get; set; } = "";
        public string Country { get; set; } =  "";
        public string Gender { get; set; } = "";
        public string Username { get; set; } = "";

    }

    public class ChangePassword 
    {
        public string Password { get; set; } = "";
        public string New_Password { get; set; } = "";
    }
    public class ChangePasswordModel: ChangePassword
    {
        public Guid ID { get; set; } = new Guid();
    }
}
