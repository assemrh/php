using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.User
{
    public class SignUpModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}