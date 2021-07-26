using SGAW_ECHO.Models.API.Countries;
using System.Collections.Generic;


namespace SGAW_ECHO.Models.API.Neighborhoods
{
    public class AddNeighborhoodModel:Name
    {
        public string City_ID { get; set; }
    }
    public class NeighborhoodModel: AddNeighborhoodModel
    {
        public string ID { get; set; }
        //public string City_ID { get; set; }
        //public string Ar { get; set; }
        //public string En { get; set; }
        //public string Tr { get; set; }
    }

    public class NeighborhoodDisplayModel : Name
    {
        public string ID { get; set; }
        public Name City { get; set; }
    }
    public class NeighborhoodLargeModel
    {
        public NeighborhoodModel Neighborhood { get; set; }
        //public string cityId { get; set; }
        public string CountryId { get; set; }
        public List<City> Cities { get; set; }
        public List<Country> Countries { get; set; }
    }
}