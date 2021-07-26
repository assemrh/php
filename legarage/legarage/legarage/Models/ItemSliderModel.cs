using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace legarage.Models
{
    public class ItemSliderModel
    {
        public Guid Id { get; set; }
        public string Directory { get; set; }
        public List<ImagesModel> ImagesLsit { get; set; }
    }
}