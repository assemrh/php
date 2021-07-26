using System;

namespace legarage.Models
{
    public class CitiesModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }
        public URLModel URL { get; set; }


    }
}