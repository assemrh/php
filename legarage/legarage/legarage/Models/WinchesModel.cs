using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace legarage.Models
{
    public class WinchesModel
    {
        public Guid ID { set; get; }
        public string Title { get; set; }
        public string DriverName { set; get; }
        public string DriverPhone { set; get; }
        public string Area { set; get; }
        public string VehicleSize { set; get; }
        public string Mobile { set; get; }
        public string Whatsapp { set; get; }
        public string Description { set; get; }
        public string Keywords { get; set; }
        public AddressModel Address { set; get; }
        public UsersModel User { set; get; }

        public ImagesModel Image { get; set; }
        public List<ImagesModel> Images { get; set; }
        public URLModel URL { get; set; }

        public string mesaj { get; set; }
        public float Rate { get; set; }


    }
}