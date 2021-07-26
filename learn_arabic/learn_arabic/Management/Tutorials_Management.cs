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
    public class Tutorials_Management
    {

        public async static Task<bool> Add(ToturialModel tutorial, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = {"row_order", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("tutorial_slides", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("tutorial_slides", ID);
            }
            object[] valsinput = {tutorial.Row_Order , DateTime.Now};
            vals.AddRange(valsinput);
            if (await Database.InsertRow("tutorial_slides", ID, cols, vals, msg))
            {

                //// Add Arabic translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("tutorial_slide_translations", "AR", tutorial.Arabic_Name, ID, msg);

                //// Add English translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("tutorial_slide_translations", "EN", tutorial.Arabic_Name, ID, msg);


                //// Add Turkish translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("tutorial_slide_translations", "TR", tutorial.Arabic_Name, ID, msg);


                //// Add Russian translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("tutorial_slide_translations", "RU", tutorial.Arabic_Name, ID, msg);

                return true;
            }
            return false;
        }


        public async static Task<bool> Edit(Guid ID, ToturialModel tutorial, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = {"row_order", "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("tutorial_slides", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }
            object[] valsinput = {tutorial.Row_Order, DateTime.Now};
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("tutorial_slides", ID, cols, vals, msg))
            {

                //// edit Arabic translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("tutorial_slide_translations", "AR", tutorial.Arabic_Name, ID, msg);

                //// edit English translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("tutorial_slide_translations", "EN", tutorial.English_Name, ID, msg);

                //// edit Turkish translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("tutorial_slide_translations", "TR", tutorial.Turkish_Name, ID, msg);
                //// edit Russian translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("tutorial_slide_translations", "RU", tutorial.Russian_Name, ID, msg);

                return true;
            }
            return false;
        }

        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {

            DataRow temp = await Database.GetRow("tutorial_slides", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("tutorial_slides", ID, msg);

        }


        public async static Task<List<ShowTutorialModel>> Get_Tutorials(string lang, ER_Ref<string> msg)
        {
            List<ShowTutorialModel> dtList = new List<ShowTutorialModel>();
            string sql = "select t.id , tst.value, t.row_order " +
                "from tutorial_slides as t inner join tutorial_slide_translations as tst " +
                "on t.id = tst.src_id and tst.language = @lang";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable tutorials = await Database.ReadTableByQuery(sql, li, msg);
            if (tutorials != null && tutorials.Rows.Count > 0)
            {
                dtList = tutorials.AsEnumerable()
                       .Select(row => new ShowTutorialModel
                       {
                           ID = row["id"].ToString(),
                           Name = row["value"].ToString(),
                           Row_Order = row["row_order"].ToString()

                       }).ToList();
            }


            return dtList;
        }


        public async static Task<ToturialModel> Get_Tutorial(Guid ID, ER_Ref<string> msg)
        {
            ToturialModel tutorial = new ToturialModel();
            string sql = @"select t.id, t.row_order , ar.value as arabic, en.value as english,
                        tr.value as turkish ,ru.value as russian from tutorial_slides as t
                        inner join tutorial_slide_translations as ar 
                        on t.id = ar.src_id and ar.language = 'AR'
                        inner join tutorial_slide_translations as en 
                        on t.id = en.src_id and en.language = 'EN'
                        inner join tutorial_slide_translations as tr
                        on t.id = tr.src_id and tr.language = 'TR'
                        inner join tutorial_slide_translations as ru 
                        on t.id = ru.src_id and ru.language = 'RU'
                        where t.id=@id order_by row_order";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            DataTable tutorials = await Database.ReadTableByQuery(sql, li, msg);
            if (tutorials != null && tutorials.Rows.Count > 0)
            {
                DataRow row = tutorials.Rows[0];
                tutorial = new ToturialModel
                {
                    ID = row["id"].ToString(),
                    Arabic_Name = row["arabic"].ToString(),
                    English_Name = row["english"].ToString(),
                    Turkish_Name = row["turkish"].ToString(),
                    Russian_Name = row["russian"].ToString(),
                    Row_Order = row["row_order"].ToString()
                };
            }
            return tutorial;
        }

    }
}
