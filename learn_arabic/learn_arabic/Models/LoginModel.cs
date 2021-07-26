using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class LoginModel
    {
        public string  Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class VerficationCodeModel
    {
        public string Code { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class ResetPasswordModel: VerficationCodeModel
    {
        public string Password { get; set; } = "";
    }
}
