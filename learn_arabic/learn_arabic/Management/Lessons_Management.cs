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
    public class Lessons_Management
    {
        public async static Task<bool> Add(LessonModel lesson, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "group_id", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("lessons", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("lessons", ID);
            }
            object[] valsinput = { lesson.Group_ID, DateTime.Now };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("lessons", ID, cols, vals, msg))
            {
                int num = 0;
                if(lesson.Prev_Lesson_ID != new Guid())
                {
                    cols = new List<string>();
                    vals = new List<object>();


                    colsinput = new string[] { "lesson_id", "previous_lesson_id", "created_at" };
                    cols.AddRange(colsinput);
                    Guid Prev_ID = Guid.NewGuid();
                    msg.Error = string.Empty;
                    try
                    {
                        DataRow row = await Database.GetRow("lessons", Prev_ID);
                        num = Convert.ToInt32(row["lesson_num"].ToString());
                        num++;
                    }
                    catch (Exception ex)
                    {
                        num = 0;
                    }

                    temp = await Database.GetRow("previous_lessons", Prev_ID);
                    while (temp != null)
                    {
                        Prev_ID = Guid.NewGuid();
                        temp = await Database.GetRow("previous_lessons", Prev_ID);
                    }
                    valsinput = new object[] { ID,  lesson.Prev_Lesson_ID, DateTime.Now };
                    vals.AddRange(valsinput);
                    if (!await Database.InsertRow("previous_lessons", Prev_ID, cols, vals, msg))
                        return false;
                    
                }
                if (!await Database.UpdateRow("lessons", ID, new List<string>() { "lesson_num" }, new List<object>() { num }, msg))
                    return false;

                if (lesson.Image != null && lesson.Image.Base64 != null)
                {
                    Attachment img = lesson.Image;
                    var bytes = Convert.FromBase64String(img.Base64);
                    await Storage.SaveAttachment("/img/", img.File_Name, "lessons", "Image", ID, bytes, msg, img.IsMain, img.RowIndex);
                }

                //// add Arabic translation
                msg.Error = string.Empty;
                Ref<Guid> TID = new Ref<Guid>();
                await Database.InsertTranslation("lesson_translations", "AR", lesson.Arabic_Lesson.Name, lesson.Arabic_Lesson.Descreption, ID,TID, msg);

                if (lesson.Arabic_Lesson.Voice != null && lesson.Arabic_Lesson.Voice.Base64 != null)
                {
                    Attachment voice = lesson.Arabic_Lesson.Voice;
                    var bytes = Convert.FromBase64String(voice.Base64);
                    await Storage.SaveAttachment("/voice/", voice.File_Name, "lessons", "Voice", TID, bytes, msg, voice.IsMain, voice.RowIndex);
                }
                if (lesson.Arabic_Lesson.Video != null && lesson.Arabic_Lesson.Video.Base64 != null)
                {
                    Attachment video = lesson.Arabic_Lesson.Video;
                    var bytes = Convert.FromBase64String(video.Base64);
                    await Storage.SaveAttachment("/video/", video.File_Name, "lessons", "Video", TID, bytes, msg, video.IsMain, video.RowIndex);
                }



                //// add English translation
                msg.Error = string.Empty;
                TID = new Ref<Guid>();
                await Database.InsertTranslation("lesson_translations", "EN", lesson.English_Lesson.Name, lesson.English_Lesson.Descreption, ID,TID, msg);


                if (lesson.English_Lesson.Voice != null && lesson.English_Lesson.Voice.Base64 != null)
                {
                    Attachment voice = lesson.English_Lesson.Voice;
                    var bytes = Convert.FromBase64String(voice.Base64);
                    await Storage.SaveAttachment("/voice/", voice.File_Name, "lessons", "Voice", TID, bytes, msg, voice.IsMain, voice.RowIndex);
                }
                if (lesson.English_Lesson.Video != null && lesson.English_Lesson.Video.Base64 != null)
                {
                    Attachment video = lesson.English_Lesson.Video;
                    var bytes = Convert.FromBase64String(video.Base64);
                    await Storage.SaveAttachment("/video/", video.File_Name, "lessons", "Video", TID, bytes, msg, video.IsMain, video.RowIndex);
                }



                //// add Turkish translation
                msg.Error = string.Empty;
                TID = new Ref<Guid>();
                await Database.InsertTranslation("lesson_translations", "TR", lesson.Turkish_Lesson.Name, lesson.Turkish_Lesson.Descreption, ID, TID, msg);
                if (lesson.Turkish_Lesson.Voice != null && lesson.Turkish_Lesson.Voice.Base64 != null)
                {
                    Attachment voice = lesson.Turkish_Lesson.Voice;
                    var bytes = Convert.FromBase64String(voice.Base64);
                    await Storage.SaveAttachment("/voice/", voice.File_Name, "lessons", "Voice", TID, bytes, msg, voice.IsMain, voice.RowIndex);
                }
                if (lesson.Turkish_Lesson.Video != null && lesson.Turkish_Lesson.Video.Base64 != null)
                {
                    Attachment video = lesson.Turkish_Lesson.Video;
                    var bytes = Convert.FromBase64String(video.Base64);
                    await Storage.SaveAttachment("/video/", video.File_Name, "lessons", "Video", TID, bytes, msg, video.IsMain, video.RowIndex);
                }



                //// add Russian translation
                msg.Error = string.Empty;
                TID = new Ref<Guid>();
                await Database.InsertTranslation("lesson_translations", "RU", lesson.Russian_Lesson.Name, lesson.Russian_Lesson.Descreption, ID, TID, msg);
                if (lesson.Russian_Lesson.Voice != null && lesson.Russian_Lesson.Voice.Base64 != null)
                {
                    Attachment voice = lesson.Russian_Lesson.Voice;
                    var bytes = Convert.FromBase64String(voice.Base64);
                    await Storage.SaveAttachment("/voice/", voice.File_Name, "lessons", "Voice", TID, bytes, msg, voice.IsMain, voice.RowIndex);
                }
                if (lesson.Russian_Lesson.Video != null && lesson.Russian_Lesson.Video.Base64 != null)
                {
                    Attachment video = lesson.Russian_Lesson.Video;
                    var bytes = Convert.FromBase64String(video.Base64);
                    await Storage.SaveAttachment("/video/", video.File_Name, "lessons", "Video", TID, bytes, msg, video.IsMain, video.RowIndex);
                }

                return true;
            }
            return false;
        }

        public async static Task<bool> Edit(Guid ID, LessonModel lesson, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "group_id", "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("lessons", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }
            object[] valsinput = { DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("lessons", ID, cols, vals, msg))
            {
                cols = new List<string>() { "lesson_id" };
                vals = new List<object>() { ID };
                msg.Error = "";
                if (!await Database.DeleteRow("previous_lessons",cols,vals,msg))
                    return false;
                int num = 0;
                if (lesson.Prev_Lesson_ID != new Guid())
                {
                    cols = new List<string>();
                    vals = new List<object>();


                    colsinput = new string[] { "lesson_id", "previous_lesson_id", "created_at" };
                    cols.AddRange(colsinput);
                    Guid Prev_ID = Guid.NewGuid();
                    msg.Error = string.Empty;
                    try
                    {
                        DataRow row = await Database.GetRow("lessons", Prev_ID);
                        num = Convert.ToInt32(row["lesson_num"].ToString());
                        num++;
                    }
                    catch (Exception ex)
                    {
                        num = 0;
                    }

                    temp = await Database.GetRow("previous_lessons", Prev_ID);
                    while (temp != null)
                    {
                        Prev_ID = Guid.NewGuid();
                        temp = await Database.GetRow("previous_lessons", Prev_ID);
                    }
                    valsinput = new object[] { ID, lesson.Prev_Lesson_ID, DateTime.Now };
                    vals.AddRange(valsinput);
                    if (!await Database.InsertRow("previous_lessons", Prev_ID, cols, vals, msg))
                        return false;

                }
                if (!await Database.UpdateRow("lessons", ID, new List<string>() { "lesson_num" }, new List<object>() { num }, msg))
                    return false;
                /// TODO: Edit Image

                //// edit Arabic translation
                msg.Error = string.Empty;
                Ref<Guid> TID = new Ref<Guid>();
                await Database.UpdateTranslation("lesson_translations", "AR", lesson.Arabic_Lesson.Name, lesson.Arabic_Lesson.Descreption, ID,TID, msg);
                //TODO: Edite VOICE
                //TODO: Edite VIDEO

                //// edit English translation
                msg.Error = string.Empty;
                TID = new Ref<Guid>();
                await Database.UpdateTranslation("lesson_translations", "EN", lesson.English_Lesson.Name, lesson.English_Lesson.Descreption, ID, TID, msg);
                //TODO: Edite VOICE
                //TODO: Edite VIDEO

                //// edit Turkish translation
                msg.Error = string.Empty;
                TID = new Ref<Guid>();
                await Database.UpdateTranslation("lesson_translations", "TR", lesson.Turkish_Lesson.Name, lesson.Turkish_Lesson.Descreption, ID, TID, msg);
                //TODO: Edite VOICE
                //TODO: Edite VIDEO

                //// edit Russian translation
                msg.Error = string.Empty;
                TID = new Ref<Guid>();
                await Database.UpdateTranslation("lesson_translations", "RU", lesson.Russian_Lesson.Name, lesson.Russian_Lesson.Descreption, ID, TID, msg);
                //TODO: Edite VOICE
                //TODO: Edite VIDEO

                return true;
            }
            return false;
        }

        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {

            DataRow temp = await Database.GetRow("lessons", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("lessons", ID, msg);

        }
        public async static Task<PaginationList<ShowLessonModel>> Get_Lessons(string lang, int per_page_number, int page_number, ER_Ref<string> msg)
        {
            List<ShowLessonModel> dtList = new List<ShowLessonModel>();
            string sql = @" select l.id ,lt.value as name,lt.description , g.value as group_name,
                      i.URL as Image,v.URL as Voice,vid.URL as Video , 
                        pl.previous_lesson_id as previous_id , nl.lesson_id as next_id
                      from lessons l
                      left join previous_lessons pl on l.id = pl.lesson_id
                      left join previous_lessons nl on l.id = pl.previous_lesson_id
                      inner join lesson_translations lt on l.id = lt.src_id and lt.language=@lang
                      inner join group_translations g on l.group_id = g.src_id and g.language=@lang
                      left join attachments i on l.id = i.Src_ID and i.Src_Type='Lessons' and i.type='Image'
                      left join attachments v on lt.id = v.Src_ID and v.Src_Type='Lessons' and v.type='Voice'
                      left join attachments vid on lt.id = vid.Src_ID and vid.Src_Type='Lessons' and vid.type='Video'
                       ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            Ref<int> count = new Ref<int>() { Value = 0 };
            DataTable categories = await Database.ConverSQLQueryPage(sql, li, "id", page_number, per_page_number, msg, count);
            if (categories != null && categories.Rows.Count > 0)
            {
                dtList = categories.AsEnumerable()
                       .Select(row => new ShowLessonModel
                       {
                           ID = row["id"].ToString(),
                           Group_Name = row["group_name"].ToString(),
                           Image = row["Image"].ToString(),
                           Video = row["Video"].ToString(),
                           Descreption = row["description"].ToString(),
                           Name = row["name"].ToString(),
                           Voice = row["Voice"].ToString(),
                           Prev_id = row["previous_id"].ToString(),
                           next_id = row["next_id"].ToString(),
                       }).ToList();
                PaginationList<ShowLessonModel> result = new PaginationList<ShowLessonModel>(dtList);
                li = new List<SqlParameter>();
                li.Add(new SqlParameter("@lang", lang));
                result.ItemsCount = count.Value;
                return result;
            }
            return null;

        }

        public async static Task<List<ShowLessonModel>> Get_Lessons(string lang, ER_Ref<string> msg)
        {
            List<ShowLessonModel> dtList = new List<ShowLessonModel>();
            string sql = @"select l.id ,lt.value as name,lt.description , g.value as group_name,
                      i.URL as Image,v.URL as Voice,vid.URL as Video , 
                        pl.previous_lesson_id as previous_id , nl.lesson_id as next_id
                      from lessons l
                      left join previous_lessons pl on l.id = pl.lesson_id
                      left join previous_lessons nl on l.id = pl.previous_lesson_id
                      inner join lesson_translations lt on l.id = lt.src_id and lt.language=@lang
                      inner join group_translations g on l.group_id = g.src_id and g.language=@lang
                      left join attachments i on l.id = i.Src_ID and i.Src_Type='Image'
                      left join attachments v on lt.id = v.Src_ID and v.Src_Type='Voice'
                      left join attachments vid on lt.id = vid.Src_ID and vid.Src_Type='Video' 
                       order by l.lesson_num";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable lessons = await Database.ReadTableByQuery(sql, li, msg);
            if (lessons != null && lessons.Rows.Count > 0)
            {
                dtList = lessons.AsEnumerable()
                       .Select(row => new ShowLessonModel
                       {
                           ID = row["id"].ToString(),
                           Group_Name = row["group_name"].ToString(),
                           Image=row["Image"].ToString(),
                           Video = row["Video"].ToString(),
                           Descreption = row["description"].ToString(),
                           Name = row["name"].ToString(),
                           Voice = row["Voice"].ToString(),
                           Prev_id=row["previous_id"].ToString(),
                           next_id = row["next_id"].ToString(),
                       }).ToList();
            }
            return dtList;
        }


        public async static Task<List<ShowLessonModel>> Get_Lessons_by_groupe_id(string lang, Guid groupe_id, ER_Ref<string> msg)
        {
            List<ShowLessonModel> dtList = new List<ShowLessonModel>();
            string sql = @"select l.id ,lt.value as name,lt.description , g.value as group_name,
                            i.URL as Image,v.URL as Voice,vid.URL as Video , 
                            pl.previous_lesson_id as previous_id , nl.lesson_id as next_id
                            from lessons l
                            left join previous_lessons pl on l.id = pl.lesson_id
                            left join previous_lessons nl on l.id = pl.previous_lesson_id
                            inner join lesson_translations lt on l.id = lt.src_id and lt.language=@lang
                            inner join group_translations g on l.group_id = g.src_id and g.language=@lang
                            left join attachments i on l.id = i.Src_ID and i.Src_Type='Image'
                            left join attachments v on lt.id = v.Src_ID and v.Src_Type='Voice'
                            left join attachments vid on lt.id = vid.Src_ID and vid.Src_Type='Video' 
                            where l.group_id = @gid order by l.lesson_num";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            li.Add(new SqlParameter("@gid", groupe_id));
            DataTable lessons = await Database.ReadTableByQuery(sql, li, msg);
            if (lessons != null && lessons.Rows.Count > 0)
            {
                dtList = lessons.AsEnumerable()
                       .Select(row => new ShowLessonModel
                       {
                           ID = row["id"].ToString(),
                           Group_Name = row["group_name"].ToString(),
                           Image = row["Image"].ToString(),
                           Video = row["Video"].ToString(),
                           Descreption = row["description"].ToString(),
                           Name = row["name"].ToString(),
                           Voice = row["Voice"].ToString(),
                           Prev_id = row["previous_id"].ToString(),
                           next_id = row["next_id"].ToString(),
                       }).ToList();
            }
            return dtList;
        }
        public async static Task<LessonModel> Get_LessonModel(Guid ID, ER_Ref<string> msg)
        {
            LessonModel lesson = new LessonModel();
            string sql = @"  select  l.id   ,   i.URL as Image,l.group_id,
                                pl.previous_lesson_id as previous_id 
                            ,  a_lt.value as a_name  ,  a_lt.description as a_desc
		                    ,  e_lt.value as e_name  ,  e_lt.description as e_desc
		                    ,  t_lt.value as t_name  ,  t_lt.description as t_desc
		                    ,  r_lt.value as r_name  ,  r_lt.description as r_desc
                            ,  av.URL as a_Voice,a_vid.URL as a_Video
		                    ,  ev.URL as e_Voice,e_vid.URL as e_Video
		                    ,  tv.URL as t_Voice,t_vid.URL as t_Video
		                    ,  rv.URL as r_Voice,r_vid.URL as r_Video
                              from lessons l
                              left join previous_lessons pl on l.id = pl.lesson_id
                              inner join lesson_translations a_lt on l.id = a_lt.src_id and a_lt.language='AR'
                              inner join lesson_translations e_lt on l.id = e_lt.src_id and e_lt.language='EN'
                              inner join lesson_translations t_lt on l.id = t_lt.src_id and t_lt.language='TR'
                              inner join lesson_translations r_lt on l.id = r_lt.src_id and r_lt.language='RU'
                              inner join attachments i on l.id = i.Src_ID and i.Src_Type='Image'
                              inner join attachments av on a_lt.id = av.Src_ID and av.Src_Type='Voice'
                              inner join attachments ev on e_lt.id = ev.Src_ID and ev.Src_Type='Voice'
                              inner join attachments tv on t_lt.id = tv.Src_ID and tv.Src_Type='Voice'
                              inner join attachments rv on r_lt.id = rv.Src_ID and rv.Src_Type='Voice'
                              inner join attachments a_vid on a_lt.id = a_vid.Src_ID and a_vid.Src_Type='Video'
                              inner join attachments e_vid on e_lt.id = e_vid.Src_ID and e_vid.Src_Type='Video'
                              inner join attachments t_vid on t_lt.id = t_vid.Src_ID and t_vid.Src_Type='Video'
                              inner join attachments r_vid on r_lt.id = r_vid.Src_ID and r_vid.Src_Type='Video'
                              where l.id=@id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            DataTable lessons = await Database.ReadTableByQuery(sql, li, msg);
            if (lessons != null && lessons.Rows.Count > 0)
            {
                DataRow row = lessons.Rows[0];
                lesson = new LessonModel()
                {
                    //ID = new Guid(row["id"].ToString()),
                    Group_ID = new Guid(row["group_id"].ToString()),
                    Image = new Attachment() { File_Name =  row["Image"].ToString() },
                    Arabic_Lesson = new LessonTranslationModel()
                    {
                        Name = row["a_name"].ToString(),
                        Descreption = row["a_desc"].ToString(),
                        Voice = new Attachment() { File_Name = row["a_Voice"].ToString() },
                        Video = new Attachment() { File_Name = row["a_Video"].ToString() },
                    },
                    English_Lesson = new LessonTranslationModel()
                    {
                        Name = row["e_name"].ToString(),
                        Descreption = row["e_desc"].ToString(),
                        Voice = new Attachment() { File_Name = row["e_Voice"].ToString() },
                        Video = new Attachment() { File_Name = row["e_Video"].ToString() },
                    },
                    Turkish_Lesson = new LessonTranslationModel()
                    {
                        Name = row["t_name"].ToString(),
                        Descreption = row["t_desc"].ToString(),
                        Voice = new Attachment() { File_Name = row["t_Voice"].ToString() },
                        Video = new Attachment() { File_Name = row["t_Video"].ToString() },
                    },
                    Russian_Lesson = new LessonTranslationModel()
                    {
                        Name = row["r_name"].ToString(),
                        Descreption = row["r_desc"].ToString(),
                        Voice = new Attachment() { File_Name = row["r_Voice"].ToString() },
                        Video = new Attachment() { File_Name = row["r_Video"].ToString() },
                    }
                };
            }
            return lesson;
        }
        public async static Task<View_Lesson_Model> Get_Lesson(Guid ID, ER_Ref<string> msg)
        {
            View_Lesson_Model lesson = new View_Lesson_Model();
            string sql = @"  select  l.id   ,   i.URL as Image,l.group_id,
            pl.previous_lesson_id as previous_id 
        ,  a_lt.value as a_name  ,  a_lt.description as a_desc
		,  e_lt.value as e_name  ,  e_lt.description as e_desc
		,  t_lt.value as t_name  ,  t_lt.description as t_desc
		,  r_lt.value as r_name  ,  r_lt.description as r_desc
        ,av.URL as a_Voice,a_vid.URL as a_Video
		,ev.URL as e_Voice,e_vid.URL as e_Video
		,tv.URL as t_Voice,t_vid.URL as t_Video
		,rv.URL as r_Voice,r_vid.URL as r_Video
          from lessons l
          left join previous_lessons pl on l.id = pl.lesson_id
          inner join lesson_translations a_lt on l.id = a_lt.src_id and a_lt.language='AR'
          inner join lesson_translations e_lt on l.id = e_lt.src_id and e_lt.language='EN'
          inner join lesson_translations t_lt on l.id = t_lt.src_id and t_lt.language='TR'
          inner join lesson_translations r_lt on l.id = r_lt.src_id and r_lt.language='RU'
          inner join attachments i on l.id = i.Src_ID and i.Src_Type='Image'
          inner join attachments av on a_lt.id = av.Src_ID and av.Src_Type='Voice'
          inner join attachments ev on e_lt.id = ev.Src_ID and ev.Src_Type='Voice'
          inner join attachments tv on t_lt.id = tv.Src_ID and tv.Src_Type='Voice'
          inner join attachments rv on r_lt.id = rv.Src_ID and rv.Src_Type='Voice'
          inner join attachments a_vid on a_lt.id = a_vid.Src_ID and a_vid.Src_Type='Video'
          inner join attachments e_vid on e_lt.id = e_vid.Src_ID and e_vid.Src_Type='Video'
          inner join attachments t_vid on t_lt.id = t_vid.Src_ID and t_vid.Src_Type='Video'
          inner join attachments r_vid on r_lt.id = r_vid.Src_ID and r_vid.Src_Type='Video'
          where l.id=@id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            DataTable lessons = await Database.ReadTableByQuery(sql, li, msg);
            if (lessons != null && lessons.Rows.Count > 0)
            {
                DataRow row = lessons.Rows[0];
                lesson = new View_Lesson_Model()
                {
                    ID = new Guid(row["id"].ToString()),
                    Group_ID=new Guid(row["group_id"].ToString()),
                    Image= row["Image"].ToString(),
                    Arabic_Lesson =new ShowLessonTranslationModel()
                    {
                        Name= row["a_name"].ToString(),
                        Descreption  = row["a_desc"].ToString(),
                        Voice = row["a_Voice"].ToString(),
                        Video = row["a_Video"].ToString(),
                    },
                    English_Lesson = new ShowLessonTranslationModel()
                    {
                        Name = row["e_name"].ToString(),
                        Descreption = row["e_desc"].ToString(),
                        Voice = row["e_Voice"].ToString(),
                        Video = row["e_Video"].ToString(),
                    },
                    Turkish_Lesson = new ShowLessonTranslationModel()
                    {
                        Name = row["t_name"].ToString(),
                        Descreption = row["t_desc"].ToString(),
                        Voice = row["t_Voice"].ToString(),
                        Video = row["t_Video"].ToString(),
                    },
                    Russian_Lesson = new ShowLessonTranslationModel()
                    {
                        Name = row["r_name"].ToString(),
                        Descreption = row["r_desc"].ToString(),
                        Voice = row["r_Voice"].ToString(),
                        Video = row["r_Video"].ToString(),
                    }
                };
            }
            return lesson;
        }


        public async static Task<bool> Check_IF_Exists(Guid user_id, Guid lesson_id, ER_Ref<string> msg)
        {
            string sql = @"select(select COUNT(*) from user_lessons where user_id = @user_id and lesson_id = lesson_id)";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@user_id", user_id));
            li.Add(new SqlParameter("@lesson_id", lesson_id));
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


        public async static Task<bool> Get_Prev_lesson_(Guid lesson_id, string lang, ER_Ref<string> msg, Ref<ShowConstantModel> prev)
        {
            string sql = @"select pl.previous_lesson_id as id ,lt.value as name
                            from  previous_lessons pl
                            inner join lesson_translations lt 
                            on pl.previous_lesson_id =lt.src_id
                            and lt.language=@lang
                            where lesson_id = @lesson_id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lesson_id", lesson_id));
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


        public async static Task<bool> Check_prev_is_fineshed(Guid user_id, Guid prev_lesson_id, ER_Ref<string> msg)
        {
            string sql = @"select(select COUNT(*) from user_lessons where user_id = @user_id and lesson_id = @prev_lesson_id and is_finish = 1)";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@user_id", user_id));
            li.Add(new SqlParameter("@prev_lesson_id", prev_lesson_id));
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


        public async static Task<bool> Add_user_lesson(Guid user_id, Guid lesson_id, string lang, ER_Ref<string> msg)
        {
            if (!await Check_IF_Exists(user_id, lesson_id, msg))
            {
                Ref<ShowConstantModel> prev = new Ref<ShowConstantModel>();
                if (await Get_Prev_lesson_(lesson_id, lang, msg, prev))
                {
                    Guid prev_id = new Guid(prev.Value.ID);
                    if (!await Check_prev_is_fineshed(user_id, prev_id, msg))
                    {
                        msg.Error = "error :" + prev.Value.Value;
                        return false;
                    }

                }
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();


                string[] colsinput = { "user_id", "lesson_id", "is_finish", "created_at" };
                cols.AddRange(colsinput);
                Guid ID = Guid.NewGuid();
                msg.Error = string.Empty;

                DataRow temp = await Database.GetRow("user_lessons", ID);
                while (temp != null)
                {
                    ID = Guid.NewGuid();
                    temp = await Database.GetRow("user_lessons", ID);
                }
                object[] valsinput = { user_id, lesson_id, 0, DateTime.Now };
                vals.AddRange(valsinput);
                return await Database.InsertRow("user_lessons", ID, cols, vals, msg);
            }
            msg.Error = "The user already added";
            return false;
        }


        public async static Task<LessonModel_> Get_Lesson_shapes(Guid ID, string lang, Guid lesson_id, ER_Ref<string> msg)
        {
            LessonModel_ shape = null;
            string sql = @"select l.id ,lt.value as name,lt.description , g.value as group_name,
                            i.URL as Image,v.URL as Voice,vid.URL as Video , 
                            pl.previous_lesson_id as previous_id , nl.lesson_id as next_id
                            from lessons l
                            left join previous_lessons pl on l.id = pl.lesson_id
                            left join previous_lessons nl on l.id = pl.previous_lesson_id
                            inner join lesson_translations lt on l.id = lt.src_id and lt.language=@lang
                            inner join group_translations g on l.group_id = g.src_id and g.language=@lang
                            left join attachments i on l.id = i.Src_ID and i.Src_Type='Image'
                            left join attachments v on lt.id = v.Src_ID and v.Src_Type='Voice'
                            left join attachments vid on lt.id = vid.Src_ID and vid.Src_Type='Video' 
                            where l.id = @id order by l.lesson_num";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            li.Add(new SqlParameter("@lang", lang));
            DataTable shapes = await Database.ReadTableByQuery(sql, li, msg);
            if (shapes != null && shapes.Rows.Count > 0)
            {
                DataRow row = shapes.Rows[0];
                shape = new LessonModel_()
                {
                    ID = row["id"].ToString(),
                    Descreption = row["description"].ToString(),
                    Name = row["name"].ToString(),
                    Image = row["Image"].ToString(),
                    Video = row["Video"].ToString(),
                    Voice = row["Voice"].ToString(),
                    Prev_id = row["previous_id"].ToString(),
                    next_id = row["next_id"].ToString()
                };

                string sql_ = @"select s.id, lt.value , att.URL as Image, st.value as Name,
                        v1.URL as Voice_spelling_normal,
                        v2.URL as Voice_spelling_description,
                        v3.URL as Voice_spelling_formatting
                        from lesson_shapes as s
                        inner join lesson_translations as lt
                        on s.lesson_id = lt.src_id and lt.language=@lang
                        inner join attachments as att
                        on s.id = att.Src_ID and att.Src_Type='Image'
                        inner join lesson_shape_translations as st
                        on s.id=st.src_id and st.language@lang
                        inner join attachments as v1
                        on s.id=v1.src_id and v1.Src_Type='Voice' and v1.Row_Index =1
                        inner join attachments as v2
                        on s.id=v2.src_id and v2.Src_Type='Voice' and v2.Row_Index =2
                        inner join attachments as v3
                        on s.id=v3.src_id and v3.Src_Type='Voice' and v3.Row_Index =3
                        where l.id = @id";
                List<SqlParameter> li_ = new List<SqlParameter>();
                li.Add(new SqlParameter("@id", ID));
                li.Add(new SqlParameter("@lang", lang));
                DataTable shapes_ = await Database.ReadTableByQuery(sql_, li_, msg);
                if (shapes_ != null && shapes_.Rows.Count > 0)
                {
                    shape.Shapes = shapes_.AsEnumerable().Select(row => new ShowShapeModel
                    {
                        ID = row["id"].ToString(),
                        Image_URL = row["Image"].ToString(),
                        Name = row["Name"].ToString(),
                        Voice_spelling_description_URL = row["Voice_spelling_description"].ToString(),
                        Voice_spelling_formatting_URL = row["Voice_spelling_formatting"].ToString(),
                        Voice_spelling_normal_URL = row["Voice_spelling_normal"].ToString(),

                    }).ToList();
                }

                string sql_qustions_id = @" select id from questions where exam_id in(select id from exams where src_id = @lesson_id and src_type = 'lesson')
                                    order by question_num";
                List<SqlParameter> li_id = new List<SqlParameter>();
                li.Add(new SqlParameter("@lesson_id", lesson_id));
                DataTable qustions_id = await Database.ReadTableByQuery(sql_qustions_id, li_id, msg);
                if (qustions_id != null && qustions_id.Rows.Count > 0)
                {
                    shape.Questions = qustions_id.AsEnumerable().Select(row => row["id"].ToString()).ToList();
                }
            
            }
            return shape;
        }

    }
}
