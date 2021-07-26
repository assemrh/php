using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShowChoosingAnswerModel
    {
        public bool Is_Correct { get; set; } = false;
        public Guid Question_ID { get; set; } = new Guid();
        public ChoosingAnswerTranslation Show { get; set; } = new ChoosingAnswerTranslation();
    }
    public class ChoosingAnswerTranslation
    {
        public string Name { get; set; } = "";
        public Attachment Voice { get; set; } = new Attachment();
        public Attachment Image { get; set; } = new Attachment();

    }
    public class ChoosingAnswerModel : ShowChoosingAnswerModel
    {
        public ChoosingAnswerTranslation Arabic_Answer { get; set; } = new ChoosingAnswerTranslation();
        public ChoosingAnswerTranslation English_Answer { get; set; } = new ChoosingAnswerTranslation();
        public ChoosingAnswerTranslation Turkish_Answer { get; set; } = new ChoosingAnswerTranslation();
        public ChoosingAnswerTranslation Russian_Answer { get; set; } = new ChoosingAnswerTranslation();
    }
    public class ShowMatchingAnswerModel
    {
        public Guid Question_ID { get; set; } = new Guid();
        public ChoosingAnswerTranslation Left { get; set; } = new ChoosingAnswerTranslation();
        public ChoosingAnswerTranslation Right { get; set; } = new ChoosingAnswerTranslation();
    }
    public class MatchingAnswerTranslation
    {
        public string Name { get; set; } = "";
        public Attachment Voice { get; set; } = new Attachment();
        public Attachment Image { get; set; } = new Attachment();

    }
    public class SideMatchingAnswerModel : ShowMatchingAnswerModel
    {
        public MatchingAnswerTranslation Arabic_Answer { get; set; } = new MatchingAnswerTranslation();
        public MatchingAnswerTranslation English_Answer { get; set; } = new MatchingAnswerTranslation();
        public MatchingAnswerTranslation Turkish_Answer { get; set; } = new MatchingAnswerTranslation();
        public MatchingAnswerTranslation Russian_Answer { get; set; } = new MatchingAnswerTranslation();
    }

    public class MatchingAnswerModel : ShowMatchingAnswerModel
    {
        public SideMatchingAnswerModel Left_Side { get; set; } = new SideMatchingAnswerModel();
        public SideMatchingAnswerModel Right_Side { get; set; } = new SideMatchingAnswerModel();
       
    }

    public class TableAnswerModel
    {
        public bool Is_Correct { get; set; } = false;
        public Guid Question_ID { get; set; } = new Guid();
        public Guid Letter_ID { get; set; } = new Guid();
        public List<TableAnswerOptionModel> Options { get; set; }
    }
    public class TableAnswerOptionModel
    {
        public string Index { get; set; }
        public string Is_Shown { get; set; }
    }

    public class UserChoosingAnswerModel : Choosing_Answer
    {
        public Guid User_ID { get; set; } = new Guid();
        
    }
    public class Choosing_Answer
    {
        public Guid Answer_ID { get; set; } = new Guid();
        public Guid Question_ID { get; set; } = new Guid();
    }
    public class UserTextAnswerModel: Text_Answer
    {
        public Guid User_ID { get; set; } = new Guid();
    }
    public class Text_Answer
    {
        public Guid Question_ID { get; set; } = new Guid();
        public string Answer { get; set; } = "";
    }
    public class UserAnswerCorrectionModel
    {
        public Guid User_ID { get; set; } = new Guid();
        public Guid Answer_ID { get; set; } = new Guid();
        public bool Is_Correct { get; set; } = false;
        public double Mark { get; set; } = 0;
    }

    public class UserMatchingAnswerModel: Matching_Answer
    {
        public Guid User_ID { get; set; } = new Guid();
    }
    public class Matching_Answer
    {
        public Guid Question_ID { get; set; } = new Guid();
        public Guid LeftAnswer_ID { get; set; } = new Guid();
        public Guid RightAnswer_ID { get; set; } = new Guid();
    }
    public class UserTableAnswerModel: Table_Answer
    {
        public Guid User_ID { get; set; } = new Guid();
    }
    public class Table_Answer
    {
        public Guid Answer_ID { get; set; } = new Guid();
        public Guid Question_ID { get; set; } = new Guid();
        public int Index { get; set; } = -1;
    }

    public class View_Answer
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }
    public class View_Choosing_Answer:View_Answer
    {
        public string Image_Url { get; set; }
        public string Voice_Url { get; set; }
    }
    public class View_Matching_Answer: View_Choosing_Answer
    {
        public string IsLeft { get; set; }
    }
    public class View_Table_Answer: View_Answer
    {
        public string IsCorrect { get; set; }
        public List<TableAnswerOptionModel> Options { get; set; }
    }

}
