using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShowGroupModel
    {
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public string Prev_Name { get; set; } = "";
        public string Category_Name { get; set; } = "";
        public TimeSpan Start_Time { get; set; } = new TimeSpan();
        public TimeSpan End_Time { get; set; } = new TimeSpan();
    }
    public class GroupModel:ShowGroupModel
    {
        public string Arabic_Name { get; set; } = "";
        public string English_Name { get; set; } = "";
        public string Turkish_Name { get; set; } = "";
        public string Russian_Name { get; set; } = "";
        public Guid Category_ID { get; set; } = new Guid();
        public Guid Prev_Group_ID { get; set; } = new Guid();
    }

    public class GroupInfoModel
    {
        public string Name { get; set; } = "";
        public TimeSpan Start_Time { get; set; } = new TimeSpan();
        public TimeSpan End_Time { get; set; } = new TimeSpan();
        public List<GroupLessonInfo> Lessons { get; set; } = new List<GroupLessonInfo>();
    }
}

