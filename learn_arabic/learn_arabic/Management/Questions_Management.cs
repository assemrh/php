using learn_arabic.Classes;
using learn_arabic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Management
{
    public class Questions_Management
    {
        public async static Task<bool> Add(QuestionModel question, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput ={ "exam_id", "question_type","question_num", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("questions", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("questions", ID);
            }
            object[] valsinput = { question.Exam_ID, question.Type,question.Question_Number, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("questions", ID, cols, vals, msg))
            {
                //// add Arabic translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("question_translations", "AR", question.Arabic_Question.Name, ID, msg);


                // TODO : Add voice 


                //// add English translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("question_translations", "EN", question.English_Question.Name, ID, msg);



                // TODO : Add voice 


                //// add Turkish translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("question_translations", "TR", question.Turkish_Question.Name, ID, msg);
                // TODO : Add voice 



                //// add Russian translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("question_translations", "AR", question.Russian_Question.Name, ID, msg);
                // TODO : Add voice 



                return true;
            }
            return false;
        }

        public async static Task<bool> edit(Guid ID ,QuestionModel question, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "question_num", "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;

           
            object[] valsinput = {question.Question_Number, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("questions", ID, cols, vals, msg))
            {
                //// edit Arabic translation
                msg.Error = "";
                await Database.UpdateTranslation("question_translations", "AR", question.Arabic_Question.Name,ID, msg);
                /// edit voice

                //// edit English translation
                msg.Error = "";
                await Database.UpdateTranslation("question_translations", "EN", question.English_Question.Name, ID, msg);
                /// edit voice
                

                //// edit Turkish translation
                msg.Error = "";
                await Database.UpdateTranslation("question_translations", "TR", question.Turkish_Question.Name, ID, msg);
                /// edit voice


                //// edit Russian translation
                msg.Error = "";
                await Database.UpdateTranslation("question_translations", "RU", question.Russian_Question.Name, ID, msg);
                /// edit voice


                return true;
            }
            return false;
        }

        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {
            ///TODO: should be a trigger here to remove all dependecies with questions
            DataRow temp = await Database.GetRow("questions", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("questions", ID, msg);

        }

        public async static Task<ViewQuestionModel> Get_Question_Info(Guid ID,string lang, ER_Ref<string> msg)

        {
            ViewQuestionModel question = null;
            string sql = @"select lt.value as lesson_title , att.URL as lesson_image 
                            ,q.question_type ,qt.value as question_title 
                            ,att2.URL as question_voice 
                            from questions as q 
                            inner join lesson_translations as lt 
                            on lt.src_id in  
                            (select src_id from exams where id=q.exam_id) 
                            and lt.language=@lang 
                            left join attachments as att 
                            on att.Src_ID in  
                            (select src_id from exams where id=q.exam_id) 
                            and att.Src_Type='Image' 
                            inner join question_translations as qt 
                            on q.id=qt.src_id and qt.language=@lang 
                            left join attachments as att2 
                            on qt.id  = att2.Src_ID and att2.type ='Voice'  
                            where q.id = @id ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            li.Add(new SqlParameter("@lang", lang));
            DataTable questions = await Database.ReadTableByQuery(sql, li, msg);
            if (questions != null && questions.Rows.Count > 0)
            {
                DataRow row = questions.Rows[0];
                question = new ViewQuestionModel()
                {
                    Lesson_Title = row["lesson_title"].ToString(),
                    Lesson_Image_Url = row["lesson_image"].ToString(),
                    Question_Text = row["question_title"].ToString(),
                    Question_Type = row["question_type"].ToString(),
                    Question_Voice = row["question_voice"].ToString(),

                };
                question.Answers = new List<object>();
                switch (question.Question_Type)
                {
                    case "Text":
                        break;
                    case "Choosing":
                      question.Answers.AddRange(await Get_Choosing_Answers(ID, lang));
                        break;
                    case "Matching":
                        question.Answers.AddRange(await Get_Matching_Answers(ID, lang));
                        break;
                    case "Table":
                        question.Answers.AddRange(await Get_Table_Answers(ID));
                        break;
                    default:
                        msg.Error = "enter a correct type";
                        question = null;
                        break;
                }
              
            } return question;
        }

        async static Task<List<View_Choosing_Answer>> Get_Choosing_Answers(Guid Question_ID, string lang)
        {
            List<View_Choosing_Answer> answers = null;
            string sql = @"select a.id ,t.value as title, 
                        att.URL as img,att2.URL as voice
                        from choosing_answers as a 
                        inner join answer_translations  as t on a.id=t.src_id 
                        and t.src_type='Choosing' and t.language=@lang 
                        left join attachments as att 
                        on t.id=att.Src_ID and att.Src_Type='Image'
                        left join attachments as att2 
                        on t.id=att2.Src_ID and att2.Src_Type='Voice'
                        where a.question_id=@id ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", Question_ID));
            li.Add(new SqlParameter("@lang", lang));
            ER_Ref<string> msg = new ER_Ref<string>();
            DataTable _answers = await Database.ReadTableByQuery(sql, li, msg);
            if (_answers != null && _answers.Rows.Count > 0)
            {
                answers = _answers.AsEnumerable().Select(row =>  new View_Choosing_Answer()
                {
                    ID = row["lesson_title"].ToString(),
                    Text = row["lesson_image"].ToString(),
                    Image_Url = row["question_title"].ToString(),
                    Voice_Url = row["question_type"].ToString(),

                }).ToList();  
            }
            return  answers;
        }

        async static Task<List<View_Matching_Answer>> Get_Matching_Answers(Guid Question_ID, string lang)
        {
            List<View_Matching_Answer> answers = null;
            string sql = @"select a.id ,t.value as title,a.is_left_side,
                        att.URL as img,att2.URL as voice
                        from matching_answers as a 
                        inner join answer_translations  as t on a.id=t.src_id 
                        and t.src_type='Matching' and t.language=@lang 
                        left join attachments as att 
                        on t.id=att.Src_ID and att.Src_Type='Image'
                        left join attachments as att2 
                        on t.id=att2.Src_ID and att2.Src_Type='Voice'
                        where a.question_id=@id ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", Question_ID));
            li.Add(new SqlParameter("@lang", lang));
            ER_Ref<string> msg = new ER_Ref<string>();
            DataTable _answers = await Database.ReadTableByQuery(sql, li, msg);
            if (_answers != null && _answers.Rows.Count > 0)
            {
                answers = _answers.AsEnumerable().Select(row => new View_Matching_Answer()
                {
                    ID = row["lesson_title"].ToString(),
                    Text = row["lesson_image"].ToString(),
                    Image_Url = row["question_title"].ToString(),
                    Voice_Url = row["question_type"].ToString(),
                    IsLeft = row["is_left_side"].ToString(),
                }).ToList();
            }
            return answers;
        }

        async static Task<List<View_Table_Answer>> Get_Table_Answers(Guid Question_ID)
        {
            List<View_Table_Answer> answers = null;
            string sql = @"select a.id ,l.letter,a.is_correct
                        from table_answers as a 
                        inner join letters  as l on a.letter_id=l.id
                        where a.question_id=@id ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", Question_ID));
            ER_Ref<string> msg = new ER_Ref<string>();
            DataTable _answers = await Database.ReadTableByQuery(sql, li, msg);
           
            if (_answers != null && _answers.Rows.Count > 0)
            {
                sql = "select [index] as ind,table_id,is_shown  from table_answer_options";
                DataTable options = await Database.ReadTableByQuery(sql, li, msg);
                if (options != null && options.Rows.Count > 0)
                {
                    answers = _answers.AsEnumerable().Select(row => new View_Table_Answer()
                    {
                        ID = row["id"].ToString(),
                        Text = row["letter"].ToString(),
                        IsCorrect = row["is_correct"].ToString(),
                        Options = options.AsEnumerable().Where(ans => ans["table_id"].ToString() == row[""].ToString()).
                        Select(ans => new TableAnswerOptionModel
                        {
                            Index = ans["ind"].ToString(),
                            Is_Shown = ans["is_shown"].ToString()
                        }).ToList()
                    }).ToList();
                }
                else
                    msg.Error = "no options";
            }
            return answers;
        }

        
    }
}
