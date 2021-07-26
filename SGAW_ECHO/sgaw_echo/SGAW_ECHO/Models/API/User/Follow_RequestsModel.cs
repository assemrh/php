using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.User
{
    public class Follow_RequestsModel
    {
        public string Receiver_ID { get; set; }
        public string Follow_Date { get; set; }
        public string FullName { get; set; }
        public string Image_Url { get; set; }
    }
}