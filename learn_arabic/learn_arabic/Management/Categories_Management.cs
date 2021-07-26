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
    public class Categories_Management
    {
        public async static Task<bool> Add(CategoriesModel category, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = {  "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("categories", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("categories", ID);
            }
            object[] valsinput = {  DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("categories", ID, cols, vals, msg))
            {

                //// Add Arabic translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("category_translations", "AR", category.Arabic_Name, ID, msg);


                //// add English translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("category_translations", "EN", category.English_Name, ID, msg);

                //// add Turkish translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("category_translations", "TR", category.Turkish_Name, ID, msg);


                //// add Russian translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("category_translations", "RU", category.Russian_Name, ID, msg);

                return true;
            }
            return false;
        }



        public async static Task<bool> Edit(Guid ID ,CategoriesModel category, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("categories", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }
            object[] valsinput = { DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("categories", ID, cols, vals, msg))
            {
                
                //// edit Arabic translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("category_translations", "AR", category.Arabic_Name, ID, msg);

                //// edit English translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("category_translations", "EN", category.English_Name, ID, msg);

                //// edit Turkish translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("category_translations", "TR", category.Turkish_Name, ID, msg);
                //// edit Russian translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("category_translations", "RU", category.Russian_Name, ID, msg);
                
                return true;
            }
            return false;
        }

        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {
          
            DataRow temp = await Database.GetRow("categories", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("categories", ID, msg);
            
        }
        public async static Task<PaginationList<ShowCategoriesModel>> Get_Categories(string lang, ER_Ref<string> msg)
        {
            List<ShowCategoriesModel> dtList = new List<ShowCategoriesModel>();
            string sql = "select c.id , ct.value " +
                "from categories as c inner join category_translations as ct " +
                "on c.id = ct.src_id and ct.language = @lang";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable categories = await Database.ReadTableByQuery(sql, li, msg);
            if (categories != null && categories.Rows.Count > 0)
            {
                dtList = categories.AsEnumerable()
                       .Select(row => new ShowCategoriesModel
                       {
                           ID = row["id"].ToString(),
                           Name = row["value"].ToString(),

                       }).ToList();
                PaginationList<ShowCategoriesModel> result = new PaginationList<ShowCategoriesModel>(dtList);
                return result;
            }
            return null;

        }
        public async static Task<PaginationList<ShowCategoriesModel>> Get_Categories(string lang,int per_page_number, int page_number, ER_Ref<string> msg)
        {
            List<ShowCategoriesModel> dtList = new List<ShowCategoriesModel>();
            string sql = "select c.id , ct.value " +
                "from categories as c inner join category_translations as ct " +
                "on c.id = ct.src_id and ct.language = @lang";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            Ref<int> count = new Ref<int>();
            DataTable categories = await Database.ConverSQLQueryPage(sql, li,"id",page_number,per_page_number, msg,count);
            if (categories != null && categories.Rows.Count > 0)
            {
                dtList = categories.AsEnumerable()
                       .Select(row => new ShowCategoriesModel
                       {
                           ID = row["id"].ToString(),
                           Name = row["value"].ToString(),

                       }).ToList();
                PaginationList<ShowCategoriesModel> result = new PaginationList<ShowCategoriesModel>(dtList);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@lang", lang));
                result.ItemsCount = count.Value;
                return result;
            }
            return null;
           
        }

        public async static Task<CategoriesModel> Get_Category(Guid ID, ER_Ref<string> msg)
        {
            CategoriesModel category = new CategoriesModel();
            string sql = @"select c.id , ar.value as arabic, en.value as english,
                        tr.value as turkish ,ru.value as russian from categories as c
                        inner join category_translations as ar 
                        on c.id = ar.src_id and ar.language = 'AR'
                        inner join category_translations as en 
                        on c.id = en.src_id and en.language = 'EN'
                        inner join category_translations as tr 
                        on c.id = tr.src_id and tr.language = 'TR'
                        inner join category_translations as ru 
                        on c.id = ru.src_id and ru.language = 'RU'
                        where c.id=@id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            DataTable categories = await Database.ReadTableByQuery(sql, li, msg);
            if (categories != null && categories.Rows.Count > 0)
            {
                DataRow row = categories.Rows[0];
                category =  new CategoriesModel
                {
                    ID = row["id"].ToString(),
                    Arabic_Name = row["arabic"].ToString(),
                    English_Name = row["english"].ToString(),
                    Turkish_Name = row["turkish"].ToString(),
                    Russian_Name = row["russian"].ToString(),

                };
            }
            return category;
        }
    }
}
