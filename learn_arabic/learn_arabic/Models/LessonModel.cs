using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShowLessonModel
    {
        public string ID { get; set; } = "";
        public string Group_Name { get; set; } = "";
        public string Name { get; set; } = "";
        public string Prev_id { get; set; } = "";
        public string next_id { get; set; } = "";
        public string Descreption { get; set; } = "";
        public string Image { get; set; } = "";
        public string Voice { get; set; } = "";
        public string Video { get; set; } = "";
    }
    public class LessonModel : CP_LessonModel
    {
        public LessonTranslationModel Arabic_Lesson { get; set; } = new LessonTranslationModel();
        public LessonTranslationModel English_Lesson { get; set; } = new LessonTranslationModel();
        public LessonTranslationModel Turkish_Lesson { get; set; } = new LessonTranslationModel();
        public LessonTranslationModel Russian_Lesson { get; set; } = new LessonTranslationModel();
        public Attachment Image { get; set; } = new Attachment();
        public Guid Group_ID { get; set; } = new Guid();
        public Guid Prev_Lesson_ID { get; set; } = new Guid();

    }

    public class CP_LessonModel
    {
        public string ID { get; set; } = "";
    }

    public class LessonTranslationModel
    {
        public string Name { get; set; } = "";
        public string Descreption { get; set; } = "";
        public Attachment Voice { get; set; } = new Attachment();
        public Attachment Video { get; set; } = new Attachment();
    }

    public class ShowLessonTranslationModel
    {
        public string Name { get; set; } = "";
        public string Descreption { get; set; } = "";
        public string  Voice { get; set; } = "";
        public string Video { get; set; } = "";
    }

    public class View_Lesson_Model
    {
        public Guid ID { get; set; } = new Guid();
        public Guid Group_ID { get; set; } = new Guid();
        public ShowLessonTranslationModel Arabic_Lesson { get; set; } = new ShowLessonTranslationModel();
        public ShowLessonTranslationModel English_Lesson { get; set; } = new ShowLessonTranslationModel();
        public ShowLessonTranslationModel Turkish_Lesson { get; set; } = new ShowLessonTranslationModel();
        public ShowLessonTranslationModel Russian_Lesson { get; set; } = new ShowLessonTranslationModel();
        public string  Image { get; set; } = "";
    }

    public class GroupLessonInfo
    {
        public string ID { get; set; } = "";
        public string Title { get; set; } = "";
        public string Image { get; set; } = "";
    }
}
