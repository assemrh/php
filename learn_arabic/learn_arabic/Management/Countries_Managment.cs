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
    public class Countries_Managment
    {
        public async static Task<List<ShowCountryModel>> Get_Countries(string lang, ER_Ref<string> msg)
        {
            List<ShowCountryModel> dtList = new List<ShowCountryModel>();
            string sql = "select c.id , c.code , c.ISO , c.logo , ct.value " +
                "from countries as c inner join country_translations as ct " +
                "on c.id = ct.src_id and ct.language = @lang";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable countries =await Database.ReadTableByQuery(sql, li,  msg);
            if (countries != null && countries.Rows.Count > 0)
            {
                dtList = countries.AsEnumerable()
                       .Select(row => new ShowCountryModel
                       {
                           Code = row["code"].ToString(),
                           ISO = row["ISO"].ToString(),
                           Name = row["value"].ToString(),
                           URL = row["logo"].ToString(),
                           ID= row["id"].ToString()
                       }).ToList();
            }
           

            return dtList;
        }
    }
}
