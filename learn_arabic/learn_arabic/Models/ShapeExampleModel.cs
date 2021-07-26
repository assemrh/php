using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShapeExampleModel
    {

        public string ShapeName { get; set; } = "";
        public string ShapImage { get; set; } = "";
        public List<ShowExampleModel> ExampleShape { get; set; }
    }
}
