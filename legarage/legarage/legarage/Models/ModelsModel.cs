using System;

namespace legarage.Models
{
    public class ModelsModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string BrandId { get; set; }
        public string VehicleTypeId { get; set; }
        public URLModel URL { get; set; }

    }

    public class ModelsModel_view
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string VehicleType { get; set; }
    }
}