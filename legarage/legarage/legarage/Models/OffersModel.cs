using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace legarage.Models
{
    public class OffersModel
    {
        public Guid id{ get; set; }
        public string name{ get; set; }
        public Guid referal_id{ get; set; }
        public string referal_type{ get; set; }
        public string Is_Active{ get; set; }  
        public string old_price{ get; set; }
        public double discount_percentage { get; set; }
        public string start_date{ get; set; }
        public string end_date{ get; set; }
        public string website{ get; set; }
        public string mobile{ get; set; }
        public string paymentmethods{ get; set; }
        public string description{ get; set; }
        public Guid address_id{ get; set; }
        public Guid country_id{ get; set; }
        public Guid province_id{ get; set; }
        public string address_name{ get; set; }
        public string Refresh { get; set; }
        public string Adding { get; set; }
        public string Edit { get; set; }
        public Guid Owner_Id { get; set; }
        public List<VehicleTypesModel> VehicleTypes { get; set; }
        public List<BrandsModel> Brands { get; set; }
        public List<ServicesModel> Categories { get; set; }
        public List<ModelsModel> Models { get; set; }
        public List<ProductsModel> Products { get; set; }
        public URLModel URL { get; set; }
        public ImagesModel Image { get; set; }


    }
}