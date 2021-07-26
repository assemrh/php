using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.University
{
    public class Student_LogIn:Old_Student_LogIn
    {
        public string Email { get; set; }

    }
    public class Old_Student_LogIn
    {
        public string UserID { get; set; }
        public string UniversityID { get; set; }

    }
}