using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShowExamModel
    {
        public Guid ID { get; set; } = new Guid();
        public string name { get; set; } = "";
        public string type { get; set; } = "";
        public Guid Src_ID { get; set; } = new Guid();
    }

    public class ExamModel:ShowExamModel
    {
        public string Arabic_Name { get; set; } = "";
        public string English_Name { get; set; } = "";
        public string Turkish_Name { get; set; } = "";
        public string Russian_Name { get; set; } = "";
    }
    public enum ExamType
    {
        Placement_Test,
        Group,Lesson
    }
}
