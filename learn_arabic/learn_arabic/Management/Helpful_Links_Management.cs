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
    public class Helpful_Links_Management
    {

        public async static Task<bool> Add(Helpful_LinkModel link, ER_Ref<string> msg)
        {
            //// add Helpful Link
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "link", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("helpful_links", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("helpful_links", ID);
            }
            object[] valsinput = { link.Link, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if(await Database.InsertRow("helpful_links", ID, cols, vals, msg))
            {
                //TODO: Add Image
                //// add Arabic translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("helpful_link_translations", "AR", link.Arabic_Name, ID, msg);


                //// add English translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("helpful_link_translations", "EN", link.English_Name, ID, msg);

                //// add Turkish translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("helpful_link_translations", "TR", link.Turkish_Name, ID, msg);

                //// add Russian translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("helpful_link_translations", "RU", link.Russian_Name, ID, msg);
                return true;
            }
            return false;
        }

        public async static Task<bool> Edit(Guid ID, Helpful_LinkModel link, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "link", "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("helpful_links", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }
            object[] valsinput = { link.Link, DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("helpful_links", ID, cols, vals, msg))
            {
                ///TODO: should change the image
                //// edit Arabic translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("helpful_link_translations", "AR", link.Arabic_Name, ID, msg);

                //// edit English translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("helpful_link_translations", "EN", link.English_Name, ID, msg);

                //// edit Turkish translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("helpful_link_translations", "TR", link.Turkish_Name, ID, msg);

                //// edit Russian translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("helpful_link_translations", "RU", link.Russian_Name, ID, msg);

                return true;
            }
            return false;
        }


        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {
            ///TODO: should be a trigger here to remove all dependecies with helpful_links
            DataRow temp = await Database.GetRow("helpful_links", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("helpful_links", ID, msg);

        }

        public async static Task<ShowHelpful_LinkModel> Get_Helpful_link(Guid ID, ER_Ref<string> msg)
        {
            ShowHelpful_LinkModel Example = new ShowHelpful_LinkModel();
            string sql = @"select c.id , c.link , arabic.value, english.value, turkish.value, russian.value, att.URL
                            from helpful_links as c 
                            inner join helpful_link_translations as arabic on c.id = arabic.src_id and arabic.language = 'AR'
                            inner join helpful_link_translations as english on c.id = english.src_id and english.language = 'EN'
                            inner join helpful_link_translations as turkish on c.id = turkish.src_id and turkish.language = 'TR'
                            inner join helpful_link_translations as russian on c.id = russian.src_id and russian.language = 'RU'
                            left join attachments as att on c.id=att.Src_ID and Src_Type='Link'
                            where c.id = @id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            DataTable links = await Database.ReadTableByQuery(sql, li, msg);
            if (links != null && links.Rows.Count > 0)
            {
                DataRow row = links.Rows[0];
                Example = new ShowHelpful_LinkModel()
                {
                    Link = row["link"].ToString(),
                    ID = row["id"].ToString(),
                    Name = row["value"].ToString(),
                    Image_Url = row["URL"].ToString()
                };
            }
            return Example;
        }

        public async static Task<List<ShowHelpful_LinkModel>> Get_Helpful_links(string lang, ER_Ref<string> msg)
        {
            List<ShowHelpful_LinkModel> dtList = new List<ShowHelpful_LinkModel>();
            string sql = @"select c.id , c.link , ct.value  ,att.URL
                            from helpful_links as c inner join helpful_link_translations as ct 
                            on c.id = ct.src_id and ct.language = @lang 
                            left join attachments as att on c.id=att.Src_ID and Src_Type='Link' and att.type = 'Imagge'";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable links = await Database.ReadTableByQuery(sql, li, msg);
            if (links != null && links.Rows.Count > 0)
            {
                dtList = links.AsEnumerable()
                       .Select(row => new ShowHelpful_LinkModel
                       {
                           Link = row["link"].ToString(),
                           ID = row["id"].ToString(),
                           Name = row["value"].ToString(),
                           Image_Url= row["URL"].ToString()

                       }).ToList();
            }

            return dtList;
        }
    }
}
