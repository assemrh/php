using SGAW_ECHO.Models.API.Countries;
using System.Collections.Generic;


namespace SGAW_ECHO.Models.API.Cities
{
    public class AddCityModel:Name
    {
        public string Country_ID { get; set; }
        //public string Ar { get; set; }
        //public string En { get; set; }
        //public string Tr { get; set; }
    }
    public class CityModel: AddCityModel
    {
        public string ID { get; set; }
    }
    public class CityDisplayModel : Name
    {
        public string ID { get; set; }
        public Name Country { get; set; }
    }
    public class CityLargeModel 
    {
        public CityModel City { get; set; }
        public List<Country> Countries { get; set; }
    }
}