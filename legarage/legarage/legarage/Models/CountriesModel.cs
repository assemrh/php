using System;

namespace legarage.Models
{
    public class CountriesModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsMarket { get; set; } 
        public int IsFactory { get; set; } 
        public URLModel URL { get; set; }


    }
}