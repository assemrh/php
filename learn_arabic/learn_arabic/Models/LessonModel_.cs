using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class LessonModel_ : ShowLessonModel
    {
        public List<ShowShapeModel> Shapes { get; set; }
        public List<string> Questions { get; set; }
    }
}
