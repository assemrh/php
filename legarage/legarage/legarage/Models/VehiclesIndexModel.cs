using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace legarage.Models
{
    public class VehiclesIndexModel
    {
        public Guid Id { get; set; }
        public List<VehiclesModel> Vehicles { get; set; }
        public List<CountriesModel> Countries { get; set; }
        public List<VehicleTypesModel> VehicleTypes { get; set; }
        public List<BrandsModel> Brands { get; set; }
        public List<CitiesModel> Cities { get; set; }
        public List<ModelsModel> Models { get; set; }
    }
}