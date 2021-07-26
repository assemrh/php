using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Models.API.Address
{
    public class AddressModel: AddAddress
    {
        public Guid ID { get; set; }
        //public Guid Neighborhood_ID { get; set; }
        //public string Lat { get; set; }
        //public string Long { get; set; }
        //public string Descreption { get; set; }
    }
    public class AddAddress
    {
        public Guid Neighborhood_ID { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Descreption { get; set; }
    }
}