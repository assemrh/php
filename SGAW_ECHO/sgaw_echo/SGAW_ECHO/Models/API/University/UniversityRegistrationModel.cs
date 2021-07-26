using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SGAW_ECHO.Models.API.University
{
    public class UniversityRegistrationModel
    {
        public Guid User_ID { get; set; }
        public Guid University_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Father_Name { get; set; }
        public string Mother_Name { get; set; }
        public string DateOfBirthday { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IsMale { get; set; }
        public string IsMarried { get; set; }
        public string Nationality { get; set; }
        public Attachment Biometric { get; set; }
        public Attachment Passport { get; set; }
        public Attachment Transcript { get; set; }
        public Attachment Certificate { get; set; }

    }
}