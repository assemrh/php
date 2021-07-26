using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ConstantModel
    {
        public string ID { get; set; } = "";
        public string Table_Name { get; set; } = "";
        public string Translation_Table_Name { get; set; } = "";
        public string Arabic_Name { get; set; } = "";
        public string English_Name { get; set; } = "";
        public string Turkish_Name { get; set; } = "";
        public string Russian_Name { get; set; } = "";
    }

    public class ShowConstantModel
    {
        public string ID { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
