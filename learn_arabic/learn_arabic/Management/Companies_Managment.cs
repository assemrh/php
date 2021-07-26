using learn_arabic.Classes;
using learn_arabic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Management
{
    public class Companies_Managment
    {
        public async static Task<List<ShowConstantModel>> Get_Work_types(string lang, ER_Ref<string> msg)
        {
            List<ShowConstantModel> dtList = new List<ShowConstantModel>();
            string sql = "select w.id , wt.value from work_types as w inner join work_type_translations wt " +
                "on w.id= wt.src_id and  wt.language = @lang";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable Work_types = await Database.ReadTableByQuery(sql, li, msg);
            if (Work_types != null && Work_types.Rows.Count > 0)
            {
                dtList = Work_types.AsEnumerable()
                       .Select(row => new ShowConstantModel
                       {
                           ID = row["id"].ToString(),
                           Value = row["value"].ToString()
                       }).ToList();
            }
            return dtList;
        }

        public async static Task<List<ShowConstantModel>> Get_Work_domains(string lang, ER_Ref<string> msg)
        {
            List<ShowConstantModel> dtList = new List<ShowConstantModel>();
            string sql = "select w.id , wt.value from work_domains as w inner join work_domain_translations wt " +
                "on w.id= wt.src_id and  wt.language = @lang";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable Work_types = await Database.ReadTableByQuery(sql, li, msg);
            if (Work_types != null && Work_types.Rows.Count > 0)
            {
                dtList = Work_types.AsEnumerable()
                       .Select(row => new ShowConstantModel
                       {
                           ID = row["id"].ToString(),
                           Value = row["value"].ToString()
                       }).ToList();
            }
            return dtList;
        }

        public async static Task<List<ShowConstantModel>> Get_learning_techniques(string lang, ER_Ref<string> msg)
        {
            List<ShowConstantModel> dtList = new List<ShowConstantModel>();
            string sql = "select w.id , wt.value from learning_techniques as w inner join learning_technique_translations wt " +
                "on w.id= wt.src_id and  wt.language = @lang";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable Work_types = await Database.ReadTableByQuery(sql, li, msg);
            if (Work_types != null && Work_types.Rows.Count > 0)
            {
                dtList = Work_types.AsEnumerable()
                       .Select(row => new ShowConstantModel
                       {
                           ID = row["id"].ToString(),
                           Value = row["value"].ToString()
                       }).ToList();
            }
            return dtList;
        }

        public async static Task<List<string>> Get_Work_domains(Guid ID,string lang, ER_Ref<string> msg)
        {
            List<string> dtList = new List<string>();
            string sql = @"select w.id , wt.value from work_domains as w 
                        inner join work_domain_translations wt 
                         on w.id= wt.src_id and  wt.language = @lang 
                        where w.id in (select work_domain_id from company_work_domains 
                        where company_id = @id)";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            li.Add(new SqlParameter("@id", ID));
            DataTable Work_types = await Database.ReadTableByQuery(sql, li, msg);
            if (Work_types != null && Work_types.Rows.Count > 0)
            {
                dtList = Work_types.AsEnumerable()
                       .Select(row => row["value"].ToString()).ToList();
            }
            return dtList;
        }

        public async static Task<List<string>> Get_learning_techniques(Guid ID, string lang, ER_Ref<string> msg)
        {
            List<string> dtList = new List<string>();
            string sql = @"select w.id , wt.value from learning_techniques as w 
                            inner join learning_technique_translations wt 
                            on w.id= wt.src_id and  wt.language = @lang 
                     where w.id in (select learning_technique_id from company_learning_techniques 
                                        where company_id = @id)";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            li.Add(new SqlParameter("@id", ID));
            DataTable Work_types = await Database.ReadTableByQuery(sql, li, msg);
            if (Work_types != null && Work_types.Rows.Count > 0)
            {
                dtList = Work_types.AsEnumerable()
                       .Select(row => row["value"].ToString()).ToList(); 
            }
            return dtList;
        }
        public async static Task<bool> Add(CompanyModel company, ER_Ref<string> msg)
        {
            //// add company info 
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "name", "work_type_id", "country_id", "city", "address_details", "num_of_students", "students_ages_from", "students_ages_to", "num_of_teachers", "manager_name", "license_number", "communications_officer_number", "email", "other_info", "created_at" };
            
            cols.AddRange(colsinput);
           // if (company.Establish_Date != new DateTime()) cols.Add("establish_date");
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("companies", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("companies", ID);
            }
            object[] valsinput = { company.Company_Name,new Guid(company.Work_Type_ID),new Guid(company.Country_ID),company.City,company.Address_Details,company.Num_Of_Students,company.Students_Ages_From,company.Students_Ages_To,company.Num_Of_Teachers,company.Manager_Name, company.License_Number,company.Communications_Officer_Number,company.Email,company.Other_Info,DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
           // if (company.Establish_Date != new DateTime()) vals.Add(company.Establish_Date);
            if ( await Database.InsertRow("companies", ID, cols, vals, msg))
            {
                foreach (string  item in company.Work_Domains)
                {
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "company_id", "work_domain_id", "created_at" };
                    valsinput = new object[] { ID, item,DateTime.Now.ToShortDateString() };
                    Guid DID = Guid.NewGuid();
                    temp = await Database.GetRow("company_work_domains", DID);
                    while (temp != null)
                    {
                        DID = Guid.NewGuid();
                        temp = await Database.GetRow("company_work_domains", DID);
                    }
                    await Database.InsertRow("company_work_domains", DID, cols, vals, msg);
                }
                foreach (string item in company.Learning_Techniques)
                {
                    cols = new List<string>();
                    vals = new List<object>();
                    colsinput = new string[] { "company_id", "learning_technique_id", "created_at" };
                    valsinput = new object[] { ID, item, DateTime.Now.ToShortDateString() };
                    Guid DID = Guid.NewGuid();
                    temp = await Database.GetRow("company_learning_techniques", DID);
                    while (temp != null)
                    {
                        DID = Guid.NewGuid();
                        temp = await Database.GetRow("company_learning_techniques", DID);
                    }
                    await Database.InsertRow("company_learning_techniques", DID, cols, vals, msg);
                }
                if (company.License_Image != null && company.License_Image.Base64 != null)
                {
                    Attachment img = company.License_Image;
                    var bytes = Convert.FromBase64String(img.Base64);
                    await Storage.SaveAttachment("/img/", img.File_Name, "companies","Image",ID,bytes,msg,img.IsMain,img.RowIndex);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public async static Task<PaginationList<ShowCompanyModel>> Get_Companies(string lang, int per_page_number, int page_number, ER_Ref<string> msg)
        {
            List<ShowCompanyModel> dtList = new List<ShowCompanyModel>();
            string sql = @"select c.id,c.name,wt.value ,c.email, 
                        ct.value + ' - '+c.city+' - '+ c.address_details as address 
                        from companies as c 
                        inner join work_type_translations as wt 
                        on c.work_type_id=wt.src_id and wt.language=@lang 
                        inner join country_translations as ct 
                        on c.country_id=ct.src_id and ct.language=@lang";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            Ref<int> count = new Ref<int>();
            DataTable companies = await Database.ConverSQLQueryPage(sql, li,"id",page_number,per_page_number, msg,count);
            if (companies != null && companies.Rows.Count > 0)
            {
                dtList = companies.AsEnumerable()
                       .Select(row => new ShowCompanyModel
                       {
                           ID = row["id"].ToString(),
                           Name= row["name"].ToString(),
                           WorkType = row["value"].ToString(),
                           Email= row["email"].ToString(),
                           Address= row["address"].ToString()
                       }).ToList();
                PaginationList<ShowCompanyModel> result = new PaginationList<ShowCompanyModel>(dtList);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@lang", lang));
                result.ItemsCount = count.Value;
                return result;
            }
            return null;
        }
        public async static Task<CompanyDetailsModel> Get_Company(Guid ID, string lang, ER_Ref<string> msg)
        {
            CompanyDetailsModel company = new CompanyDetailsModel();
            string sql = @"select * ,wt.value as work_type ,att.URL as Image
                        ,ct.value + ' - '+c.city+' - '+ c.address_details
                         as address   from companies as c 
                        inner join work_type_translations as wt 
                        on c.work_type_id=wt.src_id and wt.language=@lang 
                        inner join country_translations as ct 
                        on c.country_id=ct.src_id and ct.language=@lang 
                        left join attachments as att 
                        on c.id=att.src_id and att.type='Image' 
                        where c.id=@id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            li.Add(new SqlParameter("@id", ID));
            DataTable companies = await Database.ReadTableByQuery(sql, li, msg);
            if (companies != null && companies.Rows.Count > 0)
            {
                DataRow row = companies.Rows[0];
                company = new CompanyDetailsModel()
                {
                    Address = row["address"].ToString(),
                    Email = row["email"].ToString(),
                    License_Image = row["Image"].ToString(),
                    Communications_Officer_Number = row["communications_officer_number"].ToString(),
                    Company_Name = row["name"].ToString(),
                    Work_Type = row["work_type"].ToString(),
                    Establish_Date= row["establish_date"].ToString(),
                    License_Number= row["license_number"].ToString(),
                    Manager_Name = row["manager_name"].ToString(),
                    Num_Of_Students= row["num_of_students"].ToString(),
                    Num_Of_Teachers = row["num_of_teachers"].ToString(),
                    Other_Info = row["other_info"].ToString(),
                    Students_Ages_From  = row["students_ages_from"].ToString(),
                    Students_Ages_To= row["students_ages_to"].ToString(),
                };
                company.Work_Domains =await Get_Work_domains(ID, lang, msg);
                company.Learning_Techniques = await Get_learning_techniques(ID, lang, msg);
            }
            return company;
        }
     
    }
}
