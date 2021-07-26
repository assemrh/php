using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShowHelpful_LinkModel
    {
        public string ID { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Image_Url { get; set; }
    }

    public class Helpful_LinkModel : ShowHelpful_LinkModel
    {
        public string Arabic_Name { get; set; } = "";
        public string English_Name { get; set; } = "";
        public string Turkish_Name { get; set; } = "";
        public string Russian_Name { get; set; } = "";
        public Attachment Image { get; set; } = new Attachment();
    }
}
