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
    public class Answers_Management
    {
        public async static Task<bool> AddChoosingAnswer(ChoosingAnswerModel answer, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "question_id", "is_correct", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("choosing_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("choosing_answers", ID);
            }
            object[] valsinput = { answer.Question_ID, answer.Is_Correct, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("choosing_answers", ID, cols, vals, msg))
            {


                //// Add Arabic translation
                // TODO : Add voice 
                // TODO : Add image 
                msg.Error = string.Empty;
                await Database.InsertTranslation("answer_translations", "AR", answer.Arabic_Answer.Name, ID, msg);

                //// Add English translation
                // TODO : Add voice 
                // TODO : Add image 
                msg.Error = string.Empty;
                await Database.InsertTranslation("answer_translations", "EN", answer.English_Answer.Name, ID, msg);


                //// Add Turkish translation
                 // TODO : Add voice 
                // TODO : Add image 
                msg.Error = string.Empty;
                await Database.InsertTranslation("answer_translations", "TR", answer.Turkish_Answer.Name, ID, msg);


                //// Add Russian translation
                 // TODO : Add voice 
                // TODO : Add image 
                msg.Error = string.Empty;
                await Database.InsertTranslation("answer_translations", "RU", answer.Russian_Answer.Name, ID, msg);

                return true;
            }
            return false;
        }

        public async static Task<bool> AddMatchingAnswer(MatchingAnswerModel answer, ER_Ref<string> msg)
        {
            //// add left side
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "question_id", "is_left_side", "created_at" };
            cols.AddRange(colsinput);
            Guid LID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("matching_answers", LID);
            while (temp != null)
            {
                LID = Guid.NewGuid();
                temp = await Database.GetRow("matching_answers", LID);
            }
            object[] valsinput = { answer.Question_ID, 1, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("matching_answers", LID, cols, vals, msg))
            {
                //// add right side
                cols = new List<string>();
                vals = new List<object>();


                colsinput = new string[] { "question_id", "is_left_side", "matching_answer_id", "created_at" };
                cols.AddRange(colsinput);
                Guid RID = Guid.NewGuid();
                msg.Error = string.Empty;

                temp = await Database.GetRow("matching_answers", RID);
                while (temp != null)
                {
                    RID = Guid.NewGuid();
                    temp = await Database.GetRow("matching_answers", RID);
                }
                valsinput = new object[] { answer.Question_ID, 0, LID, DateTime.Now.ToShortDateString() };
                vals.AddRange(valsinput);
                if (await Database.InsertRow("matching_answers", RID, cols, vals, msg))
                {
                    cols = new List<string>();
                    vals = new List<object>();


                    colsinput = new string[] { "matching_answer_id" };

                    msg.Error = string.Empty;

                    valsinput = new object[] { RID };
                    vals.AddRange(valsinput);
                    if (await Database.UpdateRow("matching_answers", LID, cols, vals, msg))
                    {
                        //// add Left Arabic translation
                        // TODO : Add voice 
                        // TODO : Add image 
                        msg.Error = string.Empty;
                        await Database.InsertTranslation("answer_translations", "AR", answer.Left_Side.Arabic_Answer.Name, LID, msg);


                        //// add Right Arabic translation
                         // TODO : Add voice 
                        // TODO : Add image
                        msg.Error = string.Empty;
                        await Database.InsertTranslation("answer_translations", "AR", answer.Right_Side.Arabic_Answer.Name, RID, msg);



                        //// add Left English translation
                          // TODO : Add voice 
                        // TODO : Add image
                        msg.Error = string.Empty;
                        await Database.InsertTranslation("answer_translations", "EN", answer.Left_Side.English_Answer.Name, LID, msg);


                        //// add Right English translation
                        // TODO : Add voice 
                        // TODO : Add image
                        msg.Error = string.Empty;
                        await Database.InsertTranslation("answer_translations", "EN", answer.Right_Side.English_Answer.Name, RID, msg);


                        //// add Left Turkish translation
                        // TODO : Add voice 
                        // TODO : Add image
                        msg.Error = string.Empty;
                        await Database.InsertTranslation("answer_translations", "TR", answer.Left_Side.Turkish_Answer.Name, LID, msg);

                        //// add Right Turkish translation
                        // TODO : Add voice 
                        // TODO : Add image
                        msg.Error = string.Empty;
                        await Database.InsertTranslation("answer_translations", "TR", answer.Right_Side.Turkish_Answer.Name, RID, msg);


                        //// add Left Russian translation
                        // TODO : Add voice 
                        // TODO : Add image
                        msg.Error = string.Empty;
                        await Database.InsertTranslation("answer_translations", "RU", answer.Left_Side.Russian_Answer.Name, LID, msg);



                        //// add Right Russian translation
                        // TODO : Add voice 
                        // TODO : Add image
                        msg.Error = string.Empty;
                        await Database.InsertTranslation("answer_translations", "RU", answer.Right_Side.Russian_Answer.Name, RID, msg);

                        return true;
                    }
                }
            }
            return false;
        }



        public async static Task<bool> AddLetter(LetterModel letter, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();

            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("letters", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("letters", ID);
            }
            string[] colsinput;
            object[] valsinput;
            if (letter.Parent_ID == new Guid())
            {
                colsinput = new string[] { "letter", "created_at" };
                valsinput = new object[] { letter.Letter, DateTime.Now.ToShortDateString() };
            }
            else
            {
                colsinput = new string[] { "letter", "parent_id", "created_at" };
                valsinput = new object[] { letter.Letter, letter.Parent_ID, DateTime.Now.ToShortDateString() };
            }
            cols.AddRange(colsinput);
            vals.AddRange(valsinput);

            return await Database.InsertRow("letters", ID, cols, vals, msg);

        }



        public async static Task<List<ShowLetterModel>> Get_Letters(string Parent_ID, ER_Ref<string> msg)
        {
            List<ShowLetterModel> dtList = new List<ShowLetterModel>();
            string sql = "";
            List<SqlParameter> li = new List<SqlParameter>();
            if(Parent_ID==null|| Parent_ID == "")
            {
                sql = @" select id , letter from letters
                        where parent_id is null ";
            }
            else
            {
                sql = @" select id , letter from letters
                        where parent_id = @Parent_ID ";
                li.Add(new SqlParameter("@Parent_ID", Parent_ID));

            }
            DataTable letters = await Database.ReadTableByQuery(sql, li, msg);
            if (letters != null && letters.Rows.Count > 0)
            {
                dtList = letters.AsEnumerable()
                       .Select(row => new ShowLetterModel
                       {
                           Letter = row["letter"].ToString(),
                           ID = row["id"].ToString(),
                          
                       }).ToList();
            }
            return dtList;
        }



        public async static Task<bool> AddTableAnswer(TableAnswerModel answer, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "question_id", "letter_id", "is_correct", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("table_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("table_answers", ID);
            }
            object[] valsinput = { answer.Question_ID,answer.Letter_ID, answer.Is_Correct, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("table_answers", ID, cols, vals, msg))
            {
                foreach (var item in answer.Options)
                {
                    cols = new List<string>();
                    vals = new List<object>();


                    colsinput = new string[] { "table_id", "index", "is_shown", "created_at" };
                    cols.AddRange(colsinput);
                    Guid TID = Guid.NewGuid();
                    msg.Error = string.Empty;

                    temp = await Database.GetRow("table_answer_options", TID);
                    while (temp != null)
                    {
                        TID = Guid.NewGuid();
                        temp = await Database.GetRow("table_answer_options", TID);
                    }
                    valsinput = new object[] { ID, item.Index, item.Is_Shown, DateTime.Now.ToShortDateString() };
                    vals.AddRange(valsinput);
                    await Database.InsertRow("table_answer_options", TID, cols, vals, msg);
                }
                return true;
            }
            return false;
        }



        public async static Task<bool> UserChoosingAnswer(UserChoosingAnswerModel answer, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();

            int mark = 0;
            var _mark = await Database.ReadValue("choosing_answers", "is_correct", answer.Answer_ID);
            mark = _mark == null ? 0 : Convert.ToInt32(_mark)*100;

            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("user_choosing_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("user_choosing_answers", ID);
            } 

            string[] colsinput = { "user_id", "question_id", "answer_id","mark", "created_at" };
            cols.AddRange(colsinput);
            
            object[] valsinput = { answer.User_ID,answer.Question_ID, answer.Answer_ID,mark, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            return await Database.InsertRow("user_choosing_answers", ID, cols, vals, msg);
        }
        public async static Task<bool> UserChoosingAnswer(UserChoosingAnswerModel answer,Ref<int> returned_mark, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();

            int mark = 0;
            var _mark = await Database.ReadValue("choosing_answers", "is_correct", answer.Answer_ID);
            mark = _mark == null ? 0 : Convert.ToInt32(_mark) * 100;

            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("user_choosing_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("user_choosing_answers", ID);
            }

            string[] colsinput = { "user_id", "question_id", "answer_id", "mark", "created_at" };
            cols.AddRange(colsinput);

            object[] valsinput = { answer.User_ID, answer.Question_ID, answer.Answer_ID, mark, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            returned_mark.Value = (int)mark % 10;
            return await Database.InsertRow("user_choosing_answers", ID, cols, vals, msg);
        }

        public async static Task<bool> UserTextAnswer(UserTextAnswerModel answer, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("user_text_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("user_text_answers", ID);
            }

            string[] colsinput = { "user_id", "question_id", "answer", "created_at" };
            cols.AddRange(colsinput);

            object[] valsinput = { answer.User_ID, answer.Question_ID, answer, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if( await Database.InsertRow("user_text_answers", ID, cols, vals, msg))
            {
                //TODO Add notification 
                return true;
            }
            return false;
        }
        public async static Task<bool> User_Answer_Correction(UserAnswerCorrectionModel answer, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            Guid ID = answer.Answer_ID;

            string[] colsinput = { "mark", "is_corrected", "updated_at" };
            cols.AddRange(colsinput);

            object[] valsinput = {(int)Math.Ceiling( answer.Mark*10), answer.Is_Correct, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            return await Database.UpdateRow("user_text_answers", ID, cols, vals, msg);
        }


        public async static Task<bool> UserMatchingAnswer(UserMatchingAnswerModel answer, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();

            int mark = 0;
            string sql = @"select matching_answer_id from matching_answers where id=@left and is_left_side=1";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@left", answer.LeftAnswer_ID));
            var _mark = await Database.ReadValueByQuery(sql , li);
            if (_mark == null || _mark.ToString() != answer.RightAnswer_ID.ToString())
                mark = 0;
            else mark = 100;

            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("user_matching_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("user_matching_answers", ID);
            }

            string[] colsinput = { "user_id", "question_id", "left_answer_id", "right_answer_id", "mark", "created_at" };
            cols.AddRange(colsinput);

            object[] valsinput = { answer.User_ID, answer.Question_ID,answer.LeftAnswer_ID,answer.RightAnswer_ID, mark, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            return await Database.InsertRow("user_matching_answers", ID, cols, vals, msg);
        }

        public async static Task<bool> UserMatchingAnswer(UserMatchingAnswerModel answer, Ref<int> returned_mark, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();

            int mark = 0;
            string sql = @"select matching_answer_id from matching_answers where id=@left and is_left_side=1";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@left", answer.LeftAnswer_ID));
            var _mark = await Database.ReadValueByQuery(sql, li);
            if (_mark == null || _mark.ToString() != answer.RightAnswer_ID.ToString())
                mark = 0;
            else mark = 100;

            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("user_matching_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("user_matching_answers", ID);
            }

            string[] colsinput = { "user_id", "question_id", "left_answer_id", "right_answer_id", "mark", "created_at" };
            cols.AddRange(colsinput);

            object[] valsinput = { answer.User_ID, answer.Question_ID, answer.LeftAnswer_ID, answer.RightAnswer_ID, mark, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            returned_mark.Value = (int)mark % 10;
            return await Database.InsertRow("user_matching_answers", ID, cols, vals, msg);
        }



        public async static Task<bool> UserTableAnswer(UserTableAnswerModel answer, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();

            int mark = 0;
            string sql = @"select Count(*) from table_answer_options  
  where                     table_id= @table_id and [index] = @_index and is_shown=0";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@table_id", answer.Answer_ID));
            li.Add(new SqlParameter("@_index", answer.Index));
            var _mark = await Database.ReadValueByQuery(sql, li);
            if (_mark == null || Convert.ToInt32(_mark) != 1)
                mark = 0;
            else mark = 100;

            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("user_table_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("user_table_answers", ID);
            }

            string[] colsinput = { "user_id", "question_id", "table_id", "index", "mark", "created_at" };
            cols.AddRange(colsinput);

            object[] valsinput = { answer.User_ID, answer.Question_ID, answer.Answer_ID, answer.Index, mark, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            return await Database.InsertRow("user_table_answers", ID, cols, vals, msg);
        }
        public async static Task<bool> UserTableAnswer(UserTableAnswerModel answer, Ref<int> returned_mark, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();

            int mark = 0;
            string sql = @"select Count(*) from table_answer_options  
  where                     table_id= @table_id and [index] = @_index and is_shown=0";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@table_id", answer.Answer_ID));
            li.Add(new SqlParameter("@_index", answer.Index));
            var _mark = await Database.ReadValueByQuery(sql, li);
            if (_mark == null || Convert.ToInt32(_mark) != 1)
                mark = 0;
            else mark = 100;

            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("user_table_answers", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("user_table_answers", ID);
            }

            string[] colsinput = { "user_id", "question_id", "table_id", "index", "mark", "created_at" };
            cols.AddRange(colsinput);

            object[] valsinput = { answer.User_ID, answer.Question_ID, answer.Answer_ID, answer.Index, mark, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            returned_mark.Value = (int)mark % 10;
            return await Database.InsertRow("user_table_answers", ID, cols, vals, msg);
        }
    }
}
