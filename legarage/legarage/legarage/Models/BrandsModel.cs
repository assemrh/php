using System;

namespace legarage.Models
{
    public class BrandsModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public ImagesModel Image { get; set; }
        public URLModel URL { get; set; }
    }
}