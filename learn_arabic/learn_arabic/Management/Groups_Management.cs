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
    public class Groups_Management

    {
        public async static Task<bool> Add(GroupModel group, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "category_id", "start_time", "end_time", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("groups", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("groups", ID);
            }
            object[] valsinput = { group.Category_ID, group.Start_Time, group.End_Time, DateTime.Now };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("groups", ID, cols, vals, msg))
            {
                int num = 0;
                if (group.Prev_Group_ID != new Guid())
                {
                    cols = new List<string>();
                    vals = new List<object>();


                    colsinput = new string[] { "group_id", "previous_group_id", "created_at" };
                    cols.AddRange(colsinput);
                    Guid Prev_ID = Guid.NewGuid();
                    msg.Error = string.Empty;

                    try
                    {
                        DataRow row = await Database.GetRow("groups", Prev_ID);
                        num = Convert.ToInt32(row["group_num"].ToString());
                        num++;
                    }
                    catch (Exception ex)
                    {
                        num = 0;
                    }

                    temp = await Database.GetRow("previous_groups", Prev_ID);
                    while (temp != null)
                    {
                        Prev_ID = Guid.NewGuid();
                        temp = await Database.GetRow("previous_groups", Prev_ID);
                    }
                    valsinput = new object[] { ID, group.Prev_Group_ID, DateTime.Now };
                    vals.AddRange(valsinput);
                    if (!await Database.InsertRow("previous_groups", Prev_ID, cols, vals, msg))
                        return false;

                }
                if (!await Database.UpdateRow("groups", ID, new List<string>() { "group_num" }, new List<object>() { num }, msg))
                    return false;

                //// add Arabic translation
                cols = new List<string>();
                vals = new List<object>();


                //// add Arabic translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("group_translations", "AR", group.Arabic_Name, ID, msg);

                //// add English translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("group_translations", "EN", group.English_Name, ID, msg);


                //// add Turkish translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("group_translations", "TR", group.Turkish_Name, ID, msg);

                //// add Russian translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("group_translations", "RU", group.Russian_Name, ID, msg);
                return true;
            }
            return false;
        }

        public async static Task<bool> Edit(Guid ID, GroupModel group, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "category_id", "start_time", "end_time", "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("groups", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }
            object[] valsinput = { DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("groups", ID, cols, vals, msg))
            {
                cols = new List<string>() { "group_id" };
                vals = new List<object>() { ID };
                msg.Error = "";
                if (!await Database.DeleteRow("previous_groups", cols, vals, msg))
                    return false;
                int num = 0;
                if (group.Prev_Group_ID != new Guid())
                {
                    cols = new List<string>();
                    vals = new List<object>();


                    colsinput = new string[] { "group_id", "previous_group_id", "created_at" };
                    cols.AddRange(colsinput);
                    Guid Prev_ID = Guid.NewGuid();
                    msg.Error = string.Empty;

                    try
                    {
                        DataRow row = await Database.GetRow("groups", Prev_ID);
                        num = Convert.ToInt32(row["group_num"].ToString());
                        num++;
                    }
                    catch (Exception ex)
                    {
                        num = 0;
                    }

                    temp = await Database.GetRow("previous_groups", Prev_ID);
                    while (temp != null)
                    {
                        Prev_ID = Guid.NewGuid();
                        temp = await Database.GetRow("previous_groups", Prev_ID);
                    }
                    valsinput = new object[] { ID, group.Prev_Group_ID, DateTime.Now };
                    vals.AddRange(valsinput);
                    if (!await Database.InsertRow("previous_groups", Prev_ID, cols, vals, msg))
                        return false;

                }
                if (!await Database.UpdateRow("groups", ID, new List<string>() { "group_num" }, new List<object>() { num }, msg))
                    return false;

                //// edit Arabic translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("group_translations", "AR", group.Arabic_Name, ID, msg);

                //// edit English translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("group_translations", "EN", group.English_Name, ID, msg);

                //// edit Turkish translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("group_translations", "TR", group.Turkish_Name, ID, msg);

                //// edit Russian translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("group_translations", "RU", group.Russian_Name, ID, msg);

                return true;
            }
            return false;
        }

        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {

            DataRow temp = await Database.GetRow("groups", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("groups", ID, msg);

        }
        public async static Task<PaginationList<ShowGroupModel>> Get_Groups(string lang, int per_page_number, int page_number, ER_Ref<string> msg)
        {
            List<ShowGroupModel> dtList = new List<ShowGroupModel>();
            string sql = @"select g.id,gt.value as name,ct.value as category, 
                        g.start_time,g.end_time,
                        pg.previous_group_id as previous_id , ng.group_id as next_id 
                        , gt2.value as previous_name                        
                        from groups as g 
                        left join previous_groups pg on g.id = pg.group_id
                        left join previous_groups ng on g.id = ng.previous_group_id
                        inner join group_translations gt 
                        on g.id=gt.src_id and gt.language= @lang 
                        left join group_translations gt2 
                        on pg.previous_group_id =gt2.src_id and gt2.language= @lang
                        inner join category_translations as ct 
                        on g.category_id=ct.src_id and ct.language= @lang 
                                          ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            Ref<int> count = new Ref<int>();
            DataTable categories = await Database.ConverSQLQueryPage(sql, li, "id", page_number, per_page_number, msg, count);
            if (categories != null && categories.Rows.Count > 0)
            {
                dtList = categories.AsEnumerable()
                       .Select(row => new ShowGroupModel
                       {
                           ID = row["id"].ToString(),
                           Name = row["name"].ToString(),
                           Category_Name = row["category"].ToString(),
                           Start_Time = TimeSpan.Parse(row["start_time"].ToString()),
                           End_Time = TimeSpan.Parse(row["end_time"].ToString()),
                           Prev_Name= row["previous_name"].ToString(),

                       }).ToList();
                PaginationList<ShowGroupModel> result = new PaginationList<ShowGroupModel>(dtList);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@lang", lang));
                result.ItemsCount = count.Value;
                return result;
            }
            return null;

        }

        public async static Task<List<ShowConstantModel>> Get_Groups(string lang,Guid cat_id, ER_Ref<string> msg)
        {
            List<ShowConstantModel> dtList = new List<ShowConstantModel >();
            string sql = @"select g.id,gt.value as name,ct.value as category, 
                        g.start_time,g.end_time,
                        pg.previous_group_id as previous_id , ng.group_id as next_id 
                        , gt2.value as previous_name
                        from groups as g 
                        left join previous_groups pg on g.id = pg.group_id
                        left join previous_groups ng on g.id = ng.previous_group_id
                        inner join group_translations gt 
                        on g.id=gt.src_id and gt.language= @lang 
                        left join group_translations gt2 
                        on pg.previous_group_id =gt2.src_id and gt2.language= @lang
                        inner join category_translations as ct 
                        on g.category_id=ct.src_id and ct.language= @lang 
                        where g.category_id like @cat_id
                        order by g.group_num                  ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            if(cat_id == new Guid())
                li.Add(new SqlParameter("@cat_id", '%'));
            else
                li.Add(new SqlParameter("@cat_id", cat_id));
            DataTable groups = await Database.ReadTableByQuery(sql, li, msg);
            if (groups != null && groups.Rows.Count > 0)
            {
                dtList = groups.AsEnumerable()
                       .Select(row => new ShowConstantModel
                       {
                           ID = row["id"].ToString(),
                           Value = row["name"].ToString(),
                          
                       }).ToList();
            }
            return dtList;
        }
        public async static Task<GroupModel> Get_Group(Guid ID, ER_Ref<string> msg)
        {
            GroupModel group = new GroupModel();
            string sql = @"select g.id , g.category_id,g.start_time,g.end_time , 
                        ar.value as arabic, en.value as english,
                        tr.value as turkish ,ru.value as russian ,
                        pg.previous_group_id as previous_id from groups as g
                        left join previous_groups pg on g.id = pg.group_id
                        inner join group_translations as ar 
                        on g.id = ar.src_id and ar.language = 'AR'
                        inner join group_translations as en 
                        on g.id = en.src_id and en.language = 'EN'
                        inner join group_translations as tr 
                        on g.id = tr.src_id and tr.language = 'TR'
                        inner join group_translations as ru 
                        on g.id = ru.src_id and ru.language = 'RU'
                        where g.id=@id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            DataTable groups = await Database.ReadTableByQuery(sql, li, msg);
            if (groups != null && groups.Rows.Count > 0)
            {
                DataRow row = groups.Rows[0];
                group = new GroupModel()
                {
                    ID = row["id"].ToString(),
                    Arabic_Name = row["arabic"].ToString(),
                    English_Name = row["english"].ToString(),
                    Turkish_Name = row["turkish"].ToString(),
                    Russian_Name = row["russian"].ToString(),
                    Category_ID = new Guid(row["category_id"].ToString()),
                    Start_Time = TimeSpan.Parse(row["start_time"].ToString()),
                    End_Time = TimeSpan.Parse(row["end_time"].ToString())
                };
            }
            return group;
        }

        public async static Task<GroupInfoModel> Get_Group_Info(Guid ID,string lang, ER_Ref<string> msg)
        {
            GroupInfoModel group = null;
            string sql = @"select gr.value as title,g.start_time,g.end_time from groups as g 
              inner join group_translations as gr on g.id = gr.src_id and gr.language = @lang 
                where g.id=@id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            li.Add(new SqlParameter("@lang", lang));
            DataTable groups = await Database.ReadTableByQuery(sql, li, msg);
            if (groups != null && groups.Rows.Count > 0)
            {
                DataRow row = groups.Rows[0];
                group = new GroupInfoModel()
                {
                    Name = row["title"].ToString(),
                    Start_Time = TimeSpan.Parse(row["start_time"].ToString()),
                    End_Time = TimeSpan.Parse(row["end_time"].ToString())
                };
                sql = @" select l.id ,lt.value as title,att.URL from lessons as l 
                     inner join lesson_translations as lt on l.id = lt.src_id and lt.language =@lang 
                     left join attachments as att on l.id = att.src_id and att.Src_Type = 'Image' 
                     where l.group_id=@gid ";
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@gid", ID));
                li.Add(new SqlParameter("@lang", lang));
                DataTable lessons = await Database.ReadTableByQuery(sql, li, msg);
                if (lessons != null && lessons.Rows.Count > 0)
                {
                    group.Lessons = lessons.AsEnumerable().Select(row => new GroupLessonInfo
                    {
                        ID = row["id"].ToString(),
                        Title = row["title"].ToString(),
                        Image = row["URL"].ToString(),
                    }).ToList();
                }
            }
            return group;
        }

        public async static Task<bool> Check_IF_Exists(Guid user_id, Guid group_id, ER_Ref<string> msg)
        {
            string sql = @"select(select COUNT(*) from user_groups where user_id = @user_id and group_id = @group_id)";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@user_id", user_id));
            li.Add(new SqlParameter("@group_id", group_id));
            DataTable result = await Database.ReadTableByQuery(sql, li, msg);
            if (result != null && result.Rows.Count > 0)
            {
                int num = 0;
                int.TryParse(result.Rows[0][0].ToString(), out num);
                return num > 0;
            }
            else
            {
                return false;

            }
        }


        public async static Task<bool> Get_Prev_groupe_(Guid group_id,string lang, ER_Ref<string> msg, Ref<ShowConstantModel> prev)
        { 
            string sql = @"select pg.previous_group_id as id ,gt.value as name
                        from  previous_groups pg
                        inner join group_translations gt 
                        on pg.previous_group_id =gt.src_id
                        and gt.language=@lang
                        where group_id = @group_id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@group_id", group_id));
            li.Add(new SqlParameter("@lang", lang));
            DataTable groups = await Database.ReadTableByQuery(sql, li, msg);
            if (groups != null && groups.Rows.Count > 0)
            {
                DataRow dr = groups.Rows[0];
                prev.Value.ID = dr["id"].ToString();
                prev.Value.Value = dr["name"].ToString();
                return true;
            }
            else
            {
                return false;
            }

        }


        public async static Task<bool> Check_prev_is_fineshed(Guid user_id, Guid prev_group_id, ER_Ref<string> msg)
        {
            string sql = @"select(select COUNT(*) from user_groups where user_id = @user_id and group_id = @prev_group_id and is_finish = 1)";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@user_id", user_id));
            li.Add(new SqlParameter("@prev_group_id", prev_group_id));
            DataTable result = await Database.ReadTableByQuery(sql, li, msg);
            if (result != null && result.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async static Task<bool> Add_user_group(Guid user_id, Guid group_id, string lang, ER_Ref<string> msg)
        {
            if (!await Check_IF_Exists(user_id, group_id, msg))
            {
                Ref<ShowConstantModel> prev = new Ref<ShowConstantModel>();
                if (await Get_Prev_groupe_(group_id,lang,msg,prev))
                {
                    Guid prev_id = new Guid(prev.Value.ID);
                    if (!await Check_prev_is_fineshed(user_id, prev_id, msg))
                    {
                        msg.Error = "error :"+ prev.Value.Value;
                        return false;
                    }
                   
                }
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();


                string[] colsinput = { "user_id", "group_id", "is_finish", "created_at" };
                cols.AddRange(colsinput);
                Guid ID = Guid.NewGuid();
                msg.Error = string.Empty;

                DataRow temp = await Database.GetRow("user_groups", ID);
                while (temp != null)
                {
                    ID = Guid.NewGuid();
                    temp = await Database.GetRow("user_groups", ID);
                }
                object[] valsinput = {user_id, group_id, 0 , DateTime.Now };
                vals.AddRange(valsinput);
                return await Database.InsertRow("user_groups", ID, cols, vals, msg);
            }
            msg.Error = "The user already added";
            return false;
        }

    }
}
