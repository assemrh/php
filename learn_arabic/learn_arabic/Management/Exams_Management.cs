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
    public class Exams_Management
    {


        public async static Task<bool> Add(ExamModel exam, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = exam.Src_ID == new Guid() ?
               new string[] { "src_type", "created_at" } :
               new string[] { "src_type", "src_id", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("exams", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("exams", ID);
            }
            object[] valsinput =
                exam.Src_ID == new Guid() ?
                new object[] { exam.type, DateTime.Now.ToShortDateString() } :
                new object[] { exam.type, exam.Src_ID, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("exams", ID, cols, vals, msg))
            {

                //// add Arabic translation

                msg.Error = string.Empty;
                await Database.InsertTranslation("exam_translations", "AR", exam.Arabic_Name, ID, msg);


                //// add English translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("exam_translations", "EN", exam.English_Name, ID, msg);


                //// add Turkish translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("exam_translations", "TR", exam.Turkish_Name, ID, msg);


                //// add Russian translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("exam_translations", "RU", exam.Russian_Name, ID, msg);

                return true;
            }
            return false;
        }
        public async static Task<bool> edit(Guid ID, ExamModel exam, ER_Ref<string> msg)
        {
            //// edit exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;


            object[] valsinput = { DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("exams", ID, cols, vals, msg))
            {
                //// edit Arabic translation
                msg.Error = "";
                await Database.UpdateTranslation("exam_translations", "AR", exam.Arabic_Name, ID, msg);
                /// edit voice

                //// edit English translation
                msg.Error = "";
                await Database.UpdateTranslation("exam_translations", "EN", exam.English_Name, ID, msg);
                /// edit voice


                //// edit Turkish translation
                msg.Error = "";
                await Database.UpdateTranslation("exam_translations", "TR", exam.Turkish_Name, ID, msg);
                /// edit voice


                //// edit Russian translation
                msg.Error = "";
                await Database.UpdateTranslation("exam_translations", "RU", exam.Russian_Name, ID, msg);
                /// edit voice


                return true;
            }
            return false;
        }
        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {
            ///TODO: should be a trigger here to remove all dependecies with exams
            DataRow temp = await Database.GetRow("exams", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("exams", ID, msg);

        }
        public async static Task<bool> Get_Available_Placement_Test(Guid USID, Ref<string> Placement_Test, ER_Ref<string> msg)
        {
            string sql = @" select id from exams where created_at in 
                            ( select MAX(created_at) from exams )";

            DataTable data = await Database.ReadTableByQuery(sql, null, msg);
            if (data != null && data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                if (row["id"] == null)
                {
                    msg.Error = "no any placement test yet";
                    return false;
                }
                Placement_Test.Value = row["id"].ToString();
                sql = @"select COUNT(*) from user_exams  where user_id = @usid and exam_id = @exid";
                List<SqlParameter> li = new List<SqlParameter>();
                li.Add(new SqlParameter("@usid", USID));
                li.Add(new SqlParameter("@exid", new Guid(Placement_Test.Value)));
                data = await Database.ReadTableByQuery(sql, li, msg);
                int count = 0;
                if (data != null && data.Rows.Count > 0)
                {

                    try
                    {
                        count = Convert.ToInt32(data.Rows[0][0]);
                    }
                    catch (Exception ex)
                    {

                        msg.Error = ex.Message;
                        count = -1;

                    }
                }
                if (count == -1)
                    return false;
                if (count > 0)
                {
                    msg.Error = "No Available Placement Test!";
                    return false;
                }
                return true;

            }
            return false;

        }

    }
}