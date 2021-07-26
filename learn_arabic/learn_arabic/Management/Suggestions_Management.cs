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
    public class Suggestions_Management
    {
        public async static Task<bool> Add(VistorSuggestionModel suggestion, ER_Ref<string> msg)
        {
            //// add user info 
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "email", "name", "mobile", "suggestion", "created_at" };
            cols.AddRange(colsinput); 
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("suggestions", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("suggestions", ID);
            } 
            object[] valsinput = { suggestion.Email, suggestion.Name, suggestion.Mobile, suggestion.Suggestion, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            return await Database.InsertRow("suggestions", ID, cols, vals, msg);
        }

        public async static Task<bool> Add(string user_id,VistorSuggestionModel suggestion, ER_Ref<string> msg)
        {
            //// add Suggestion
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "user_id", "email", "name", "mobile", "suggestion", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("suggestions", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("suggestions", ID);
            }
            object[] valsinput = { new Guid(user_id),suggestion.Email, suggestion.Name, suggestion.Mobile, suggestion.Suggestion, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            return await Database.InsertRow("suggestions", ID, cols, vals, msg);
        }

        public async static Task<List<ShowSuggestionModel>> Get_saggestions(ER_Ref<string> msg)
        {
            List<ShowSuggestionModel> saggestionsList = new List<ShowSuggestionModel>();
            string sql = @"select sug.user_id, sug.name, sug.email, sug.mobile, sug.suggestion,
                            sug.created_at, sug.updated_at from suggestions as sug";

            List<SqlParameter> li = new List<SqlParameter>();
            DataTable saggestion = await Database.ReadTableByQuery(sql, null, msg);
            if (saggestion != null && saggestion.Rows.Count > 0)
            {
                saggestionsList = saggestion.AsEnumerable()
                       .Select(row => new ShowSuggestionModel
                       {
                           Name = row["name"].ToString(),
                           Email = row["email"].ToString(),
                           Mobile = row["mobile"].ToString(),
                           Suggestion = row["suggestion"].ToString(),
                           Type = row["user_id"] == null || row["user_id"].ToString() == new Guid().ToString() ? "visitor" : "user" 

                       }).ToList();
            }
            return saggestionsList;
        }
    }
}
