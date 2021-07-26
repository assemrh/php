using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.Countries
{
    public class AddCountryModel:Name
    {
        public string Flag { get; set; }
    }
    public class Country: AddCountryModel
    {
        public string ID { get; set; }
        //public string Ar { get; set; }
        //public string En { get; set; }
        //public string Tr { get; set; }
        //public string Flag { get; set; }
    }

}