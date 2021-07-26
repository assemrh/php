﻿using System;
using System.Collections.Generic;

namespace legarage.Models
{
    public class GaragesModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Youtube { get; set; }
        public string Linkedin { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Snapchat { get; set; }
        public string Tiktok { get; set; }
        public string Facebook { get; set; }
        public string Whatsapp { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public List<ImagesModel> Images { get; set; }
        public List<ServicesModel> Services { get; set; }
        public List<BrandsModel> Brands { get; set; }
        public List<VehicleTypesModel> VehicleTypes { get; set; }
        public AddressModel Address { get; set; }
        public UsersModel User { get; set; }
        public ImagesModel Image { get; set; }
        public URLModel URL { get; set; }
        public float Rate { get; set; }
    }
}