using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGAW_ECHO.Models.API.Address;
using SGAW_ECHO.Models.CP;

namespace SGAW_ECHO.Models
{
    public class apiJson<T>
    {
        public int code { get; set; }
        public T data { get; set; }
        public string msg { get; set; }
    }
    public class apiJson
    {
        public int code { get; set; }
        public string msg { get; set; }
    }
    public class UserModel
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }                             
        public string Bio { get; set; }
        public string Token { get; set; }
        public string UserTypeID { get; set; }        
        public string Address { get; set; }
        public string profile { get; set; }
        //public ImageModel Image { get; set; }
    }
    public class AddUserModel
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string Token { get; set; }
        public string UserTypeID { get; set; }
        //public AddAddress Address { get; set; }
        public FileModel Image { get; set; }
    }
    public class LoginModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}