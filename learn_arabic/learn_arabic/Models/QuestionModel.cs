using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShowQuestionModel
    {
        public string Type { get; set; } = "";
        public Guid Exam_ID { get; set; } = new Guid();
        public QuestionTranslation Show { get; set; } = new QuestionTranslation();
        public int Question_Number { get; set; }
    }

    public class QuestionTranslation
    {
        public string Name { get; set; } = "";
        public Attachment Voice { get; set; } = new Attachment();

    }
    public class QuestionModel:ShowQuestionModel
    {
        public QuestionTranslation Arabic_Question { get; set; } = new QuestionTranslation();
        public QuestionTranslation English_Question { get; set; } = new QuestionTranslation();
        public QuestionTranslation Turkish_Question { get; set; } = new QuestionTranslation();
        public QuestionTranslation Russian_Question { get; set; } = new QuestionTranslation();
    }

    public class ViewQuestionModel
    {
        public string  Lesson_Title { get; set; }
        public string Lesson_Image_Url { get; set; }
        public string Question_Text { get; set; }
        public string Question_Voice { get; set; }
        public string Question_Type { get; set; }
        public List<object> Answers { get; set; } 
    }



}
