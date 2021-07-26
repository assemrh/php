using SGAW_ECHO.Models.API.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.University
{
    public class UniversityModel
    {
        public string ID { get; set; }
        public AddressModel Address { get; set; }
        public string URL { get; set; }
        public string Ar { get; set; }
        public string Ar_Descreption { get; set; }
        public string En { get; set; }
        public string En_Descreption { get; set; }
        public string Tr { get; set; }
        public string Tr_Descreption { get; set; }
    }
    public class ShowUniversityModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
       
        public string URL { get; set; }
        
    }

    public class ShowUniversityLargModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Ar_Descreption { get; set; }
        public string En_Descreption { get; set; }
        public string Tr_Descreption { get; set; }
        public string URL { get; set; }
        public string Domain { get; set; }
        public string Ar_Address { get; set; }
        public string En_Address { get; set; }
        public string Tr_Address { get; set; }
    }
}