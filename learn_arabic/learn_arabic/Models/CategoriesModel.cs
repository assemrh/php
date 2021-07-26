using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    
        public class ShowCategoriesModel
        {
            public string ID { get; set; } = "";
            public string Name { get; set; } = "";
        }

        public class CategoriesModel : ShowCategoriesModel
        {
            public string Arabic_Name { get; set; } = "";
            public string English_Name { get; set; } = "";
            public string Turkish_Name { get; set; } = "";
            public string Russian_Name { get; set; } = "";
            public string ParentID { get; set; } = "";

        }
    }

