using System;
using System.Collections.Generic;

namespace legarage.Models
{
    public class VehiclesModel
    {
        public Guid ID { set; get; }
        public string OwnerName { set; get; }
        public string Title { get; set; }
        public string Price { set; get; }
        public string Quantity { set; get; }
        public int IsNew { set; get; }
        public string Mobile { set; get; }
        public string Description { set; get; }
        public string Year { get; set; }
        public string Mieage { get; set; }
        public string Gearbox { get; set; }
        public string FuelType { get; set; }
        public string EngineSize { get; set; }
        public string Color { get; set; }
        public string Keywords { get; set; }
        public string WhatsApp { get; set; }
        public VehicleTypesModel VehicleType { get; set; }
        public ModelsModel Model { get; set; }
        public UsersModel User { get; set; }
        public AddressModel Address { get; set; }
        public List<ImagesModel> Images { get; set; }
        public ImagesModel Image { get; set; }
        public URLModel URL { get; set; }
        public float Rate { get; set; }



    }
}