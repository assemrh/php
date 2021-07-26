using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace legarage.Models
{
    public class ProductsModel
    {
        public Guid ID { set; get; }
        public string Title { set; get; }
        public string Price { set; get; }
        public string Quantity { set; get; }
        public int IsNew { set; get; }
        public string Mobile { set; get; }
        public string Description { set; get; }
        public string Keywords { get; set; }
        public string OwnerName { get; set; }
        public string Year { get; set; }
        public string Mieage { get; set; }
        public string GearBox { get; set; }
        public string EngineSize { get; set; }
        public string Color { get; set; }
        public string FuelType { get; set; }
        public string WhatsApp { get; set; }
        public ModelsModel Model { set; get; }
        public BrandsModel Brand { get; set; }
        public VehicleTypesModel VehicleType { set; get; }
        public AddressModel Address { set; get; }
        public ImagesModel Image { get; set; }
        public UsersModel User { get; set; }
        public List<ServicesModel> Services { set; get; }
        public List<ImagesModel> Images { get; set; }
        public URLModel URL { get; set; }
        public float Rate { get; set; }

    }
}