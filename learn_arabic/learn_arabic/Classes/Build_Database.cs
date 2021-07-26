
using learn_arabic.Management;
using learn_arabic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace learn_arabic.Classes
{
    public class Build_Database
    {
        public async static void RebuildDatabase()
        {

            ExecQuery(@"CREATE TABLE[users] (
             [id] uniqueidentifier,
              [email] NVARCHAR(50),
              [full_name] NVARCHAR(50),
              [mobile] NVARCHAR(50),
              [username] NVARCHAR(50),
              [dob] DATETIME,
              [gender] NVARCHAR(5),
              [living_country] uniqueidentifier,
              [is_admin] TINYINT,
              [token] NVARCHAR(255),
              [password] NVARCHAR(255),
              [created_at] DATETIME,  
              [updated_at] DATETIME
            )
            ");

            ExecQuery(@"ALTER  TABLE[users]  ADD [otp_code]  NVARCHAR(10) ");
            ExecQuery(@"ALTER  TABLE[users]  ADD [otp_exp_date]  DATETIME ");


            ExecQuery(@"CREATE TABLE[user_archived  ] (
             [id] uniqueidentifier,
              [email] NVARCHAR(50),
              [full_name] NVARCHAR(50),
              [mobile] NVARCHAR(50),
              [username] NVARCHAR(50),
              [dob] DATETIME,
              [gender] NVARCHAR(5),
              [living_country] uniqueidentifier,
              [is_admin] TINYINT,
              [token] NVARCHAR(255),
              [password] NVARCHAR(255),
              [created_at] DATETIME,  
              [updated_at] DATETIME
            )
            ");


            ExecQuery(@"CREATE TABLE[countries](
              [id] uniqueidentifier,
              [code]  NVARCHAR(10),
              [ISO]  NVARCHAR(5),
              [logo] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
            )
            ");

            ExecQuery(@"CREATE TABLE[country_translations](
              [id] uniqueidentifier,
              [src_id] uniqueidentifier,
              [language] VARCHAR(3),
              [value] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
            )
            ");

            ExecQuery(@"CREATE TABLE[companies](
            [id] uniqueidentifier,
            [name] NVARCHAR(50),
            [work_type_id] uniqueidentifier,
            [country_id] uniqueidentifier,
            [city] NVARCHAR(50),
            [address_details] NVARCHAR(MAX),
            [num_of_students] NVARCHAR(5),
            [students_ages_from] NVARCHAR(5),
            [students_ages_to] NVARCHAR(5),
            [num_of_teachers] NVARCHAR(5),
            [manager_name] NVARCHAR(50),
            [establish_date] DATETIME,
            [license_number] NVARCHAR(50),
            [communications_officer_number] NVARCHAR(50),
            [email] NVARCHAR(50),
            [other_info] NVARCHAR(MAX),
            [created_at] DATETIME,  
            [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[work_types](
            [id] uniqueidentifier,
            [name] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[work_type_translations](
              [id] uniqueidentifier,
              [src_id] uniqueidentifier,
              [language] VARCHAR(3),
              [value] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[work_domains](
            [id] uniqueidentifier,
            [name] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[work_domain_translations](
              [id] uniqueidentifier,
              [src_id] uniqueidentifier,
              [language] VARCHAR(3),
              [value] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
        )
        ");


            ExecQuery(@"CREATE TABLE[company_work_domains](
            [id] uniqueidentifier,
            [company_id] uniqueidentifier,
            [work_domain_id] uniqueidentifier,
            [created_at] DATETIME,  
            [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[learning_techniques](
            [id] uniqueidentifier,
            [name] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[learning_technique_translations](
              [id] uniqueidentifier,
              [src_id] uniqueidentifier,
              [language] VARCHAR(3),
              [value] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[company_learning_techniques](
            [id] uniqueidentifier,
            [company_id] uniqueidentifier,
            [learning_technique_id] uniqueidentifier,
            [created_at] DATETIME,  
            [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[tutorial_slides](
          [id] uniqueidentifier,
          [row_order] INT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[tutorial_slide_translations](
          [id] uniqueidentifier,
          [src_id] uniqueidentifier,
          [language] VARCHAR(3),
          [url] NVARCHAR(max),
          [value] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
        )
        ");

            ExecQuery(@"CREATE TABLE[properties](
        [id] uniqueidentifier,
        [index] INT,
        [name] NVARCHAR(50),
        [value] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[exams](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [src_type] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[exam_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[questions](
      [id] uniqueidentifier,
      [exam_id] uniqueidentifier,
      [question_type] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"ALTER  TABLE[questions]  ADD [question_num] INT ");

            ExecQuery(@"CREATE TABLE[question_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
      [voice_url] NVARCHAR(max),
      [question_type] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[choosing_answers](
      [id] uniqueidentifier,
      [question_id] uniqueidentifier,
      [is_correct] TINYINT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[answer_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [src_type] NVARCHAR(50),
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
      [voice_url] NVARCHAR(max),
      [url] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[matching_answers](
      [id] uniqueidentifier,
      [question_id] uniqueidentifier,
      [is_left_side] TINYINT,
      [matching_answer_id] uniqueidentifier,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[letters](
      [id] uniqueidentifier,
      [letter] NVARCHAR(3),
      [parent_id] uniqueidentifier,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"ALTER TABLE dbo.letters
                ALTER COLUMN letter NVARCHAR(10) COLLATE Arabic_100_CI_AI;
    ");

            ExecQuery(@"CREATE TABLE[table_answers](
      [id] uniqueidentifier,
      [question_id] uniqueidentifier,
      [letter_id] uniqueidentifier,
      [is_correct] TINYINT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[table_answer_options](
      [id] uniqueidentifier,
      [table_id] uniqueidentifier,
      [index] INT,
      [is_shown] TINYINT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[categories](
      [id] uniqueidentifier,
      [parent_id] uniqueidentifier,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[category_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[groups](
      [id] uniqueidentifier,
      [category_id] uniqueidentifier,
      [start_time] DATETIME,
      [end_time] DATETIME,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery("ALTER TABLE groups ALTER COLUMN start_time TIME; ");
            ExecQuery("ALTER TABLE groups ALTER COLUMN end_time TIME; ");
            ExecQuery(@"ALTER  TABLE[groups]  ADD [group_num]  INT ");

            ExecQuery(@"CREATE TABLE[group_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[previous_groups](
      [id] uniqueidentifier,
      [group_id] uniqueidentifier,
      [previous_group_id] uniqueidentifier,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[lessons](
      [id] uniqueidentifier,
      [group_id] uniqueidentifier,
      [url] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");



            ExecQuery(@"CREATE TABLE[lesson_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
      [description] NVARCHAR(max),
      [voice_description_url] NVARCHAR(max),
      [video_description_url] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"ALTER  TABLE[lessons]  ADD [lesson_num] INT ");

            ExecQuery(@"CREATE TABLE[previous_lessons](
      [id] uniqueidentifier,
      [lesson_id] uniqueidentifier,
      [previous_lesson_id] uniqueidentifier,
        [created_at] DATETIME,  
        [updated_at] DATETIME
    )
    ");


            ExecQuery(@"CREATE TABLE[lesson_shapes](
      [id] uniqueidentifier,
      [lesson_id] uniqueidentifier,
      [url] NVARCHAR(50),
      [voice_url_spelling_normal] NVARCHAR(50),
      [voice_url_spelling_description] NVARCHAR(50),
      [voice_url_spelling_formatting] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[lesson_shape_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[shape_examples](
      [id] uniqueidentifier,
      [shape_id] uniqueidentifier,
      [url] NVARCHAR(50),
      [voice_url_spelling_normal] NVARCHAR(50),
      [voice_url_spelling_description] NVARCHAR(50),
      [voice_url_spelling_formatting] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[shape_example_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            //        ExecQuery(@"CREATE TABLE[notifications](
            //  [id] uniqueidentifier
            //)
            //");

            ExecQuery(@"CREATE TABLE[suggestions](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [name] NVARCHAR(50),
      [email] NVARCHAR(50),
      [mobile] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");


            ExecQuery(@"ALTER  TABLE[suggestions]  ADD [suggestion]  NVARCHAR(MAX) ");

            ExecQuery(@"CREATE TABLE[helpful_links](
      [id] uniqueidentifier,
      [link] NVARCHAR(50),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[helpful_link_translations](
      [id] uniqueidentifier,
      [src_id] uniqueidentifier,
      [language] VARCHAR(3),
      [value] NVARCHAR(max),
      [description] NVARCHAR(max),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[user_clicks](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [link_id] uniqueidentifier,
      [count] INT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[user_groups](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [group_id] uniqueidentifier,
      [is_finish] TINYINT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[user_lessons](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [lesson_id] uniqueidentifier,
      [is_finish] TINYINT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[user_exams](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [exam_id] uniqueidentifier,
      [mark] DECIMAL(4,2),
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[user_text_answers](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [question_id] uniqueidentifier,
      [mark] INT,
      [answer] NVARCHAR(max),
      [is_corrected] TINYINT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[user_choosing_answers](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [question_id] uniqueidentifier,
      [mark] INT,
      [answer_id] uniqueidentifier,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[user_matching_answers](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [question_id] uniqueidentifier,
      [mark] INT,
      [left_answer_id] uniqueidentifier,
      [right_answer_id] uniqueidentifier,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE[user_table_answers](
      [id] uniqueidentifier,
      [user_id] uniqueidentifier,
      [question_id] uniqueidentifier,
      [mark] INT,
      [table_id] uniqueidentifier,
      [index] INT,
              [created_at] DATETIME,  
              [updated_at] DATETIME
    )
    ");

            ExecQuery(@"CREATE TABLE attachments (ID uniqueidentifier PRIMARY KEY NOT NULL , 
                         URL NVARCHAR(255) , Is_Main TINYINT , Row_Index int,
                        Src_ID uniqueidentifier ,Src_Type NVARCHAR(255) , type NVARCHAR(10),
                        created_at DATETIME,  updated_at DATETIME) ");

            int NumOfAdmins = CheckForNumbers("SELECT Count(*) AS num FROM users WHERE is_admin = 1");

            if (NumOfAdmins != 1)
            {
                if (NumOfAdmins > 1)
                {
                    List<string> cols = new List<string>();
                    List<object> vals = new List<object>();
                    cols.Add("is_admin");
                    vals.Add("1");

                    ER_Ref<string> msg = new ER_Ref<string>();

                  await  Database.DeleteRow("users", cols, vals,msg);
                }
                string password =await Ciphering.GetMD5HashDataAsync("0000");
                ExecQuery(" INSERT INTO users(id, token, username, full_name, is_admin, Password, email) " +
                          " VALUES('D06DACE1-63E9-4CFA-B55A-7178E31D0034', 'GSNX5D6WCHBR6UIHS4ZKCYACBP6IJCXB4XU76XOXTVOSPRWE1O', 'admin', 'admin', '1', '" + password + "','admin@test.com')");

            }



            //LessonModel lesson = new LessonModel()
            //{
            //    Arabic_Lesson = new LessonTranslationModel() { Name = "عربي", Descreption = "عربي" },
            //    English_Lesson = new LessonTranslationModel() { Name = "English", Descreption = "English" },
            //    Turkish_Lesson = new LessonTranslationModel() { Name = "Turkish", Descreption = "Turkish" },
            //    Russian_Lesson = new LessonTranslationModel() { Name = "Russian", Descreption = "Russian" },
            //    Group_ID = new Guid("6EC26301-43CE-4392-8A87-A4E5BFC47AE9"),

            //};
            //ER_Ref<string> msg1 = new ER_Ref<string>();
            //await Lessons_Management.Add(lesson, msg1);


            //ToturialModel tutorial = new ToturialModel()
            //{
            //    Arabic_Name = "toturial1_ar",
            //    English_Name = "toturial1_en",
            //    Turkish_Name = "toturial_tr",
            //    Russian_Name = "toturial1_ru",

            //};
            //ER_Ref<string> msg = new ER_Ref<string>();
            //await Tutorials_Management.Add(tutorial, msg);




            //ER_Ref<string> msg = new ER_Ref<string>();
            //await Tutorials_Management.Delete(new Guid("13CBAB4B-0045-4F68-84FA-4342F493B67E"), msg);







            //LetterModel letter = new LetterModel()
            //{
            //    Letter = "بـ",
            //    Parent_ID=new Guid("D368E3C8-E11F-4EFF-A2DF-5DD56BBBB534")
            //};
            //ER_Ref<string> msg = new ER_Ref<string>();
            //await Answers_Management.AddLetter(letter, msg);

            //letter = new LetterModel()
            //{
            //    Letter = "ـبـ",
            //    Parent_ID = new Guid("D368E3C8-E11F-4EFF-A2DF-5DD56BBBB534")
            //};
            // msg = new ER_Ref<string>();
            //await Answers_Management.AddLetter(letter, msg);


            //letter = new LetterModel()
            //{
            //    Letter = "ـب",
            //    Parent_ID = new Guid("D368E3C8-E11F-4EFF-A2DF-5DD56BBBB534")
            //};
            //msg = new ER_Ref<string>();
            //await Answers_Management.AddLetter(letter, msg);



            //letter = new LetterModel()
            //{
            //    Letter = "بًـ",
            //    Parent_ID = new Guid("D368E3C8-E11F-4EFF-A2DF-5DD56BBBB534")
            //};
            //msg = new ER_Ref<string>();
            //await Answers_Management.AddLetter(letter, msg);


            //GroupModel group = new GroupModel()
            //{
            //    Arabic_Name = "group1_ar",
            //    English_Name = "group1_en",
            //    Turkish_Name = "group1_tr",
            //    Russian_Name = "group1_ru",
            //    Category_ID = new Guid("BEA168C3-9204-4194-BDA2-1C7A25F6C65A"),
            //    Start_Time = new TimeSpan(0, 0, 1),
            //    End_Time = new TimeSpan(0, 2, 20)
            //};
            //ER_Ref<string> msg1 = new ER_Ref<string>();
            //await Groups_Management.Add(group, msg1);


            //group = new GroupModel()
            //{
            //    Arabic_Name = "group11_ar",
            //    English_Name = "group11_en",
            //    Turkish_Name = "group11_tr",
            //    Russian_Name = "group11_ru",
            //    Category_ID = new Guid("BEA168C3-9204-4194-BDA2-1C7A25F6C65A"),
            //    Start_Time = new TimeSpan(0, 2, 20),
            //    End_Time = new TimeSpan(0, 4,23)
            //};
            //msg = new ER_Ref<string>();
            //await Groups_Management.Add(group, msg);

            //CategoriesModel cat = new CategoriesModel()
            //{
            //    name = "",
            //    Arabic_Name = "cat1_ar",
            //    English_Name = "cat1_en",
            //    Turkish_Name = "cat1_tr",
            //    Russian_Name = "cat1_ru"
            //};
            //ER_Ref<string> msg = new ER_Ref<string>();
            //await Categories_Management.Add(cat, msg);


            //cat = new CategoriesModel()
            //{
            //    name = "",
            //    Arabic_Name = "cat2_ar",
            //    English_Name = "cat2_en",
            //    Turkish_Name = "cat2_tr",
            //    Russian_Name = "cat2_ru"
            //};
            //msg = new ER_Ref<string>();
            //await Categories_Management.Add(cat, msg);




            //ExamModel exam = new ExamModel()
            //{
            //    name = "",
            //    Arabic_Name = "exam1_ar",
            //    English_Name = "exam1_en",
            //    Turkish_Name = "exam1_tr",
            //    Russian_Name = "exam1_ru",
            //    type = ExamType.Placement_Test.ToString()
            //};
            //ER_Ref<string> msg = new ER_Ref<string>();
            //await Exams_Management.Add_Placement_Test(exam, msg);


            // exam = new ExamModel()
            //{
            //    name = "",
            //    Arabic_Name = "exam2_ar",
            //    English_Name = "exam2_en",
            //    Turkish_Name = "exam2_tr",
            //    Russian_Name = "exam2_ru",
            //    type = ExamType.Placement_Test.ToString()
            //};
            // msg = new ER_Ref<string>();
            //await Exams_Management.Add_Placement_Test(exam, msg);

            //  CountryModel country = new CountryModel()
            //  {
            //      Name = "",
            //      Arabic_Name = "سوريا",
            //      English_Name = "syria",
            //      Turkish_Name = "surie",
            //      Russian_Name = "syria",
            //      Code = "+936",
            //      ISO = "SAR",
            //      URL = ""
            //  };
            ////  Add_Country(country);
            //  country = new CountryModel()
            //  {
            //      Name = "",
            //      Arabic_Name = "تركيا",
            //      English_Name = "turkey",
            //      Turkish_Name = "turyie",
            //      Russian_Name = "turkey",
            //      Code = "+90",
            //      ISO = "TR",
            //      URL = ""
            //  };
            // // Add_Country(country);
            //  country = new CountryModel()
            //  {
            //      Name = "",
            //      Arabic_Name = "لبنان",
            //      English_Name = "lebanon",
            //      Turkish_Name = "lebnan",
            //      Russian_Name = "lebanon",
            //      Code = "+97",
            //      ISO = "LB",
            //      URL = ""
            //  };
            //  //Add_Country(country);


            //Helpful_LinkModel link = new Helpful_LinkModel()
            //{
            //    Name = "",
            //    Arabic_Name = "link1_AR",
            //    English_Name = "link1_EN",
            //    Turkish_Name = "link1_TR",
            //    Russian_Name = "link1_RU",
            //    Link = "https://www.youtube.com/"
            //};
            //ER_Ref<string> msg_ = new ER_Ref<string>();
            //await Helpful_Links_Management.Add(link, msg_);

            //link = new Helpful_LinkModel()
            //{
            //    Name = "",
            //    Arabic_Name = "link2_AR",
            //    English_Name = "link2_EN",
            //    Turkish_Name = "link3_TR",
            //    Russian_Name = "link4_RU",
            //    Link = "https://web.telegram.org/"
            //};
            //msg_ = new ER_Ref<string>();
            //await Helpful_Links_Management.Add(link, msg_);

            //ExecQuery("CREATE TRIGGER insert_follow ON Friends AFTER INSERT AS " +
            //       "INSERT INTO Friend_Reports  select i.User_ID ,i.Friend_User_ID , "+
            //       " i.Friendship_Date , i.Friendship_Statuse_ID from inserted as i; ");

            //ExecQuery("CREATE TRIGGER update_follow ON Friends AFTER update AS " +
            //       "INSERT INTO Friend_Reports  select i.User_ID ,i.Friend_User_ID , " +
            //       " i.Friendship_Date , i.Friendship_Statuse_ID from inserted as i; ");

            /////////////////////////

        }

        public async static void Add_Constants()
        {
            ConstantModel constatnt = new ConstantModel()
            {
                ID = "0db7145d-d5a7-40c9-9593-aa5e7c43cf57",
                Arabic_Name = "خيري",
                English_Name = "charitable",
                Turkish_Name = "yardımsever",
                Russian_Name = "благотворительный",
                Table_Name = "work_types",
                Translation_Table_Name = "work_type_translations"
            };
              Add_Constant(constatnt);
            
             constatnt = new ConstantModel()
            {
                ID = "5cb83794-5687-44e4-adaf-daf39c130306",
                Arabic_Name = "ربحية",
                English_Name = "Profitability",
                Turkish_Name = "Karlılık",
                Russian_Name = "Рентабельность",
                Table_Name = "work_types",
                Translation_Table_Name = "work_type_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "33de358e-feca-476e-9302-12327f595715",
                Arabic_Name = "رسمي/حكومي",
                English_Name = "Official / governmental",
                Turkish_Name = "Resmi",
                Russian_Name = "Официальный / правительственный",
                Table_Name = "work_domains",
                Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "b4e17a28-8642-4620-add6-7e6abcc7c728",
                Arabic_Name = "غير رسمي",
                English_Name = "unofficial",
                Turkish_Name = "gayri resmi",
                Russian_Name = "неофициальный",
                 Table_Name = "work_domains",
                 Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "d1493947-2b4d-4eaa-9d63-d40333f08f96",
                Arabic_Name = "عمل شرعي",
                English_Name = "Legitimate action",
                Turkish_Name = "Meşru eylem",
                Russian_Name = "Законное действие",
                 Table_Name = "work_domains",
                 Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "f46a6aef-464c-4c87-9fee-2c54388f2745",
                Arabic_Name = "أكاديمي",
                English_Name = "academic",
                Turkish_Name = "akademik",
                Russian_Name = "академический",
                 Table_Name = "work_domains",
                 Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "8c385d6b-1bdc-4757-ab09-24d6640365e7",
                Arabic_Name = "إنساني",
                English_Name = "Humanitarian",
                Turkish_Name = "İnsani yardım",
                Russian_Name = "Гуманитарный",
                 Table_Name = "work_domains",
                 Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "474ce303-5c8b-4936-a796-c8379f576ef2",
                Arabic_Name = "إغاثي",
                English_Name = "Relief",
                Turkish_Name = "Rahatlama",
                Russian_Name = "Облегчение",
                 Table_Name = "work_domains",
                 Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "b919a862-f072-4b70-ae29-aca5a37a6d77",
                Arabic_Name = "تدريب",
                English_Name = "Training",
                Turkish_Name = "Eğitim",
                Russian_Name = "Обучение",
                 Table_Name = "work_domains",
                 Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "7173b2ef-0920-4ebc-8c2e-2609b669addb",
                Arabic_Name = "تنموي",
                English_Name = "Developmental",
                Turkish_Name = "Gelişimsel",
                Russian_Name = "Развивающий",
                 Table_Name = "work_domains",
                 Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "58cce1d8-1ecc-43f6-9533-b0d5021b8266",
                Arabic_Name = "أخرى",
                English_Name = "Other",
                Turkish_Name = "Diğer",
                Russian_Name = "Другой",
                 Table_Name = "work_domains",
                 Translation_Table_Name = "work_domain_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "9605f08b-f2a8-4d56-aaab-09474391bcc4",
                Arabic_Name = "شاشات ذكية",
                English_Name = "Smart screens",
                Turkish_Name = "Akıllı ekranlar",
                Russian_Name = "Умные экраны",
                Table_Name = "learning_techniques",
                Translation_Table_Name = "learning_technique_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "0efa7de1-f8e0-45c9-a172-9939ce489833",
                Arabic_Name = "عاكس ضوئي",
                English_Name = "Light reflector",
                Turkish_Name = "Işık reflektörü",
                Russian_Name = "Светоотражатель",
                 Table_Name = "learning_techniques",
                 Translation_Table_Name = "learning_technique_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "92d7f262-bf5c-48f4-befb-5b158ea12bb6",
                Arabic_Name = "تقنيات أخرى",
                English_Name = "Other techniques",
                Turkish_Name = "Diğer teknikler",
                Russian_Name = "Другие техники",
                 Table_Name = "learning_techniques",
                 Translation_Table_Name = "learning_technique_translations"
             };
            Add_Constant(constatnt);
             constatnt = new ConstantModel()
            {
                ID = "5b6bba20-78b7-424d-a2e9-0ac0b5ddef61",
                Arabic_Name = "بدون تقنيات",
                English_Name = "Without techniques",
                Turkish_Name = "Teknikler olmadan",
                Russian_Name = "Без техники",
                 Table_Name = "learning_techniques",
                 Translation_Table_Name = "learning_technique_translations"
             };
            Add_Constant(constatnt);
        }

        public async static void Remove_Constants()
        {
            ExecQuery("delete from  work_types");
            ExecQuery("delete from  work_type_translations");
            ExecQuery("delete from  work_domains");
            ExecQuery("delete from  work_domain_translations");
            ExecQuery("delete from  learning_techniques");
            ExecQuery("delete from  learning_technique_translations");
        }
        async  static void Add_Country(CountryModel country)
        {
            //add country
            DateTime dateTime = DateTime.Now;
            Guid id = Guid.NewGuid();
            DataRow country_ =await Database.FindRow("countries", "id", id);
            while (country_ != null)
            {
                id = Guid.NewGuid();
                country_ =await Database.FindRow("countries", "id", id);
            }
            ExecQuery("INSERT INTO countries (id, code, ISO, logo,created_at,updated_at) " +
                " VALUES ('" + id + "', '" + country.Code + "','" + country.ISO + "',''," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

            //arabic translation
            Guid T_id = Guid.NewGuid();
            DataRow country_T =await Database.FindRow("country_translations", "id", T_id);
            while (country_T != null)
            {
                T_id = Guid.NewGuid();
                country_T =await Database.FindRow("country_translations", "id", T_id);
            }
            ExecQuery("INSERT INTO country_translations (id, src_id, language, value, created_at, updated_at)" +
                "VALUES ('" + Guid.NewGuid() + "', '" + id + "', 'AR', '" + country.Arabic_Name + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

            //english translation
            T_id = Guid.NewGuid();
            country_T =await Database.FindRow("country_translations", "id", T_id);
            while (country_T != null)
            {
                T_id = Guid.NewGuid();
                country_T =await Database.FindRow("country_translations", "id", T_id);
            }
            ExecQuery("INSERT INTO country_translations (id, src_id, language, value, created_at, updated_at)" +
                "VALUES ('" + Guid.NewGuid() + "', '" + id + "', 'En', '" + country.English_Name + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

            //turkish translation
            T_id = Guid.NewGuid();
            country_T =await Database.FindRow("country_translations", "id", T_id);
            while (country_T != null)
            {
                T_id = Guid.NewGuid();
                country_T =await Database.FindRow("country_translations", "id", T_id);
            }
            ExecQuery("INSERT INTO country_translations (id, src_id, language, value, created_at, updated_at)" +
                "VALUES ('" + Guid.NewGuid() + "', '" + id + "', 'TR', '" + country.Turkish_Name + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

            //russian translation
            T_id = Guid.NewGuid();
            country_T =await Database.FindRow("country_translations", "id", T_id);
            while (country_T != null)
            {
                T_id = Guid.NewGuid();
                country_T =await Database.FindRow("country_translations", "id", T_id);
            }
            ExecQuery("INSERT INTO country_translations (id, src_id, language, value, created_at, updated_at)" +
                "VALUES ('" + Guid.NewGuid() + "', '" + id + "', 'RU', '" + country.Russian_Name + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

        }

        async static void Add_Constant(ConstantModel constant)
        {
            //add country
            DateTime dateTime = DateTime.Now;
            ExecQuery("INSERT INTO "+ constant.Table_Name + " (id,created_at,updated_at) " +
                " VALUES ('" + constant.ID + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

            //arabic translation
            Guid T_id = Guid.NewGuid();
            DataRow constant_T = await Database.FindRow(constant.Translation_Table_Name, "id", T_id);
            while (constant_T != null)
            {
                T_id = Guid.NewGuid();
                constant_T = await Database.FindRow(constant.Translation_Table_Name, "id", T_id);
            }
            ExecQuery("INSERT INTO "+constant.Translation_Table_Name+" (id, src_id, language, value, created_at, updated_at)" +
                "VALUES ('" +T_id + "', '" + constant.ID + "', 'AR', '" + constant.Arabic_Name + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

            //english translation
             T_id = Guid.NewGuid();
             constant_T = await Database.FindRow(constant.Translation_Table_Name, "id", T_id);
            while (constant_T != null)
            {
                T_id = Guid.NewGuid();
                constant_T = await Database.FindRow(constant.Translation_Table_Name, "id", T_id);
            }
            ExecQuery("INSERT INTO " + constant.Translation_Table_Name + " (id, src_id, language, value, created_at, updated_at)" +
                "VALUES ('" + T_id + "', '" + constant.ID + "', 'EN', '" + constant.English_Name + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

            //turkish translation
             T_id = Guid.NewGuid();
             constant_T = await Database.FindRow(constant.Translation_Table_Name, "id", T_id);
            while (constant_T != null)
            {
                T_id = Guid.NewGuid();
                constant_T = await Database.FindRow(constant.Translation_Table_Name, "id", T_id);
            }
            ExecQuery("INSERT INTO " + constant.Translation_Table_Name + " (id, src_id, language, value, created_at, updated_at)" +
                "VALUES ('" + T_id + "', '" + constant.ID + "', 'TR', '" + constant.Turkish_Name + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

            //russian translation
             T_id = Guid.NewGuid();
             constant_T = await Database.FindRow(constant.Translation_Table_Name, "id", T_id);
            while (constant_T != null)
            {
                T_id = Guid.NewGuid();
                constant_T = await Database.FindRow(constant.Translation_Table_Name, "id", T_id);
            }
            ExecQuery("INSERT INTO " + constant.Translation_Table_Name + " (id, src_id, language, value, created_at, updated_at)" +
                "VALUES ('" + T_id + "', '" + constant.ID + "', 'RU', '" + constant.Russian_Name + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");

        }



        public async static void ExecQuery(String str)
        {
            SqlConnection cn = new SqlConnection(Database.ConnectionString);
            SqlCommand cmd = new SqlCommand(str, cn);
            try
            {
                cn.Open();
                try
                {
                  await  cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    string errMessage = ex.Message;
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
            }

        }

        public static int CheckForNumbers(string str)
        {
            SqlConnection cn = new SqlConnection(Database.ConnectionString);
            SqlCommand cmd = new SqlCommand(str, cn);
            int ob;
            try
            {
                cn.Open();

                try
                {
                    ob = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    ob = 0;
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                ob = 0;
            }
            return ob;
            //SqlDataAdapter adp = new SqlDataAdapter(str, Database.ConnectionString);
            //DataTable tbl = new DataTable();
            //adp.Fill(tbl);
            //if (tbl != null && tbl.Rows.Count > 0)
            //{
            //    return Convert.ToInt32(tbl.Rows[0]["Num"]);
            //}
            //else
            //{
            //    return 0;
            //}

        }

    }
}