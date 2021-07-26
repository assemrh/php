using SGAW_ECHO.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models
{
    //public class CityModel
    //{
    //    public Guid Country_ID { get; set; }
    //    public string Ar { get; set; }
    //    public string En { get; set; }
    //    public string Tr { get; set; }
    //}
    //public class CityJson
    //{
    //    public int code { get; set; }
    //    public string data { get; set; }
    //    public string msg { get; set; }
    //}
    public class City:Name
    {
        public string ID { get; set; }
    }
}