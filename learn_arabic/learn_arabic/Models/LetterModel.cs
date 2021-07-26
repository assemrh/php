using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class LetterModel
    {
        public Guid Parent_ID { get; set; } = new Guid();
        public string  Letter { get; set; } = "";
    }

    public class ShowLetterModel
    {
        public string ID { get; set; } = "";
        public string Letter { get; set; } = "";
    }
}
