using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.User
{
    public class ChangePassword
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}