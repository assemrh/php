using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShowCountryModel
    {
        public string ID { get; set; }
        public string  Code { get; set; }
        public string ISO { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        
    }
    public class CountryModel:ShowCountryModel
    {
        public string Arabic_Name { get; set; } = "";
        public string English_Name { get; set; } = "";
        public string Turkish_Name { get; set; } = "";
        public string Russian_Name { get; set; } = "";
    }
}
