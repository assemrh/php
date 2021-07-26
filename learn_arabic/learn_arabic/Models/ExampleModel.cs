using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class AddExampleModel : ExampleModel
    {
        public Attachment Image { get; set; } = new Attachment();
        public Attachment Voice_spelling_normal { get; set; } = new Attachment();
        public Attachment Voice_spelling_description { get; set; } = new Attachment();
        public Attachment Voice_spelling_formatting { get; set; } = new Attachment();
    }
    public class ExampleModel : ShowExampleModel
    {
        public string Arabic_Name { get; set; } = "";
        public string English_Name { get; set; } = "";
        public string Turkish_Name { get; set; } = "";
        public string Russian_Name { get; set; } = "";
        public Guid Shape_ID { get; set; } = new Guid();
    }
    public class ShowExampleModel
    {
        public string ID { get; set; } = "";
        public string Shape_Name { get; set; } = "";
        public string Image_URL { get; set; } = "";
        public string Name { get; set; } = "";
        public string Voice_spelling_normal_URL { get; set; } = "";
        public string Voice_spelling_description_URL { get; set; } = "";
        public string Voice_spelling_formatting_URL { get; set; } = "";
        //public List<string> Prev
    }
}
