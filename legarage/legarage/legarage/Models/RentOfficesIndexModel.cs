using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace legarage.Models
{
    public class RentOfficesIndexModel
    {
        public Guid ID { set; get; }
        public List<RentOfficesModel> RentOffices { get; set; }
        public List<CountriesModel> Countries { get; set; }
        public List<VehicleTypesModel> VehicleTypes { get; set; }
        public List<BrandsModel> Brands { get; set; }
        public List<CitiesModel> Cities { get; set; }
        public List<ModelsModel> Models { get; set; }
        public List<ServicesModel> Categories { get; set; }
    }
}