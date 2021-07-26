using SGAW_ECHO.Models.API;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SGAW_ECHO.Classes
{
    public class Build_Database
    {
        public static void RebuildDatabase()
        {

            ExecQuery("CREATE TABLE Users (ID uniqueidentifier PRIMARY KEY NOT NULL ,Username NVARCHAR(100), Full_name NVARCHAR(100) , " +
                      " Email NVARCHAR(50) ,Password NVARCHAR(100) ,Mobile NVARCHAR(50),Address_ID uniqueidentifier ," +
                       " Token NVARCHAR(50),User_Type_ID uniqueidentifier,External_Login_Token NVARCHAR(50) , External_Login_Type NVARCHAR(50)," +
                       "Date_Of_Create DATETIME,  Date_Of_Update DATETIME )");

            ExecQuery("CREATE TABLE User_Types (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                       ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Friends(ID uniqueidentifier PRIMARY KEY NOT NULL , User_ID uniqueidentifier," +
                      "Friend_User_ID uniqueidentifier,Friendship_Date DATETIME,Date_Of_Create DATETIME,  Date_Of_Update DATETIME," +
                      "Friendship_Statuse_ID uniqueidentifier )");

            ExecQuery("CREATE TABLE Friend_Reports(ID INT  PRIMARY KEY  NOT NULL  IDENTITY(1,1) , User_ID uniqueidentifier," +
                      "Friend_User_ID uniqueidentifier,Report_Date DATETIME,Friendship_Statuse_ID uniqueidentifier )");

            ExecQuery("CREATE TABLE Friendship_Statuses (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                   ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");


            ExecQuery("CREATE TABLE Post (ID uniqueidentifier PRIMARY KEY NOT NULL, Post_Type_ID uniqueidentifier,Post_Text NVARCHAR(MAX) , " +
            " Date_Of_Create DATETIME ,Duration_By_Sec int ,Post_Privecy_ID uniqueidentifier ,User_ID uniqueidentifier,Lang_ID uniqueidentifier)");


            ExecQuery("CREATE TABLE Post_History (ID uniqueidentifier PRIMARY KEY NOT NULL,Post_ID uniqueidentifier, " +
                      "Edit_Text  NVARCHAR(MAX) , Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Privacies (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                   ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Post_Types (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                   ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");


            ExecQuery("CREATE TABLE Comments (ID uniqueidentifier PRIMARY KEY NOT NULL, Comment NVARCHAR(255)," +
                      " Src_ID uniqueidentifier ,Src_Type NVARCHAR(255), Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");


            ExecQuery("CREATE TABLE Likes (ID uniqueidentifier PRIMARY KEY NOT NULL , User_ID uniqueidentifier , Like_Type_ID uniqueidentifier," +
                      " Src_ID uniqueidentifier ,Src_Type NVARCHAR(255), Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Like_Types (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                   ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");


            ExecQuery("CREATE TABLE Addresses (ID uniqueidentifier PRIMARY KEY NOT NULL ,Address_Details NVARCHAR(MAX), " +
                "Neighborhood_ID uniqueidentifier,latitude decimal(9,2) ,longitude decimal(9,2), Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Neighborhoods (ID uniqueidentifier PRIMARY KEY NOT NULL ,Neighborhood_Name NVARCHAR(255), " +
                "City_ID uniqueidentifier , Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Cities (ID uniqueidentifier PRIMARY KEY NOT NULL ,City_Name NVARCHAR(255), " +
                "Country_ID uniqueidentifier , Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Countries (ID uniqueidentifier PRIMARY KEY NOT NULL ,Country_Name NVARCHAR(255), " +
                " Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Images (ID uniqueidentifier PRIMARY KEY NOT NULL , URL  NVARCHAR(255) , Is_Main TINYINT , Row_Index int," +
                     " Src_ID uniqueidentifier ,Src_Type NVARCHAR(255) ,Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Videos (ID uniqueidentifier PRIMARY KEY NOT NULL , URL  NVARCHAR(255) , Is_Main TINYINT , Row_Index int," +
                     " Src_ID uniqueidentifier ,Src_Type NVARCHAR(255), Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Universities (ID uniqueidentifier PRIMARY KEY NOT NULL , Name  NVARCHAR(255) ," +
                     " Address_ID uniqueidentifier, Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE University_Groups (ID uniqueidentifier PRIMARY KEY NOT NULL , Name  NVARCHAR(255) ," +
                     " University_ID uniqueidentifier ,Group_Type_ID uniqueidentifier, Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE University_Group_Types (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                  ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Chats (ID uniqueidentifier PRIMARY KEY NOT NULL , Name  NVARCHAR(255) ," +
                     " Chat_Type_ID uniqueidentifier , University_Group_ID uniqueidentifier, Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Chat_Types (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                  ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Messages (ID uniqueidentifier PRIMARY KEY NOT NULL , Text  NVARCHAR(MAX) , URL  NVARCHAR(255) ," +
                     " User_ID uniqueidentifier , Chat_ID uniqueidentifier ,Message_Type  NVARCHAR(50), Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Chat_Members (ID uniqueidentifier PRIMARY KEY NOT NULL ,User_ID uniqueidentifier , " +
                     " Chat_ID uniqueidentifier , Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Student_Universities (ID uniqueidentifier PRIMARY KEY NOT NULL ,User_ID uniqueidentifier , " +
                     " University_ID uniqueidentifier , User_Type_ID uniqueidentifier , Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE University_Register_Statuses (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                  ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Student_University_Registrations (ID uniqueidentifier PRIMARY KEY NOT NULL ,Register_Statues_ID uniqueidentifier , " +
                    " Student_University_ID uniqueidentifier , Date_Of_Create DATETIME,Description NVARCHAR(MAX) ) ");

            ExecQuery("CREATE TABLE Events (ID uniqueidentifier PRIMARY KEY NOT NULL ,Name NVARCHAR(255) ,Members_Count int , Price decimal(9,2) ," +
                    " Currency_ID uniqueidentifier ,Privacy_id uniqueidentifier,Description NVARCHAR(MAX), Lang_ID uniqueidentifier ) ");

            ExecQuery("CREATE TABLE Event_Days (ID uniqueidentifier PRIMARY KEY NOT NULL ,Event_ID uniqueidentifier ,Day DATETIME, " +
                " Start_Time time, End_Time time ,Address_ID uniqueidentifier ,Date_Of_Create DATETIME )");

            ExecQuery("CREATE TABLE Event_Members (ID uniqueidentifier PRIMARY KEY NOT NULL ,Event_ID uniqueidentifier ,User_ID uniqueidentifier,  " +
                " Is_Admin TINYINT, Is_Owner TINYINT  ,Date_Of_Create DATETIME )");

            ExecQuery("CREATE TABLE Currencies (ID uniqueidentifier PRIMARY KEY NOT NULL ,Name NVARCHAR(255) ,Symbol_Name NVARCHAR(10),  " +
                " Exchange decimal(9,2)  ,Date_Of_Create DATETIME )");

            ExecQuery("CREATE TABLE Payments (ID uniqueidentifier PRIMARY KEY NOT NULL , Payment decimal(9,2),   " +
               " Event_Member_ID  uniqueidentifier ,Date_Of_Create DATETIME )");

            ExecQuery("CREATE TABLE Report_Reasons (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                  ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Report_Statuses (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                  ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE Reports (ID uniqueidentifier PRIMARY KEY NOT NULL ,Report_Reason_ID uniqueidentifier ,Report_Status_ID uniqueidentifier," +
                    " Src_ID uniqueidentifier ,Src_Type NVARCHAR(255), Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Notifications (ID uniqueidentifier PRIMARY KEY NOT NULL ,User_ID uniqueidentifier , " +
                " Is_Seen TINYINT , Seen_Date  DATETIME , Header NVARCHAR(MAX) , Body NVARCHAR(MAX)," +
                   "  Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Languages (ID uniqueidentifier PRIMARY KEY NOT NULL, Key_ NVARCHAR(50)" +
                                 ", Date_Of_Create DATETIME,  Date_Of_Update DATETIME)");

            ExecQuery("CREATE TABLE User_Languages (ID uniqueidentifier PRIMARY KEY NOT NULL ,User_ID uniqueidentifier ," +
                    " Lang_id uniqueidentifier , Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery("CREATE TABLE Translations (ID uniqueidentifier PRIMARY KEY NOT NULL ,Src_ID uniqueidentifier , " +
                "Src_Type NVARCHAR(255), Ar_Value NVARCHAR(255) , En_Value NVARCHAR(255) , Tr_Value NVARCHAR(255), " +
                " Ar_Descreption NVARCHAR(MAX) , En_Descreption NVARCHAR(MAX) , TR_Descreption NVARCHAR(MAX) ,Date_Of_Create DATETIME,  Date_Of_Update DATETIME) ");

            ExecQuery(" ALTER TABLE Users ADD Bio NVARCHAR(MAX)");

            ExecQuery(" ALTER TABLE Post ADD Src_ID uniqueidentifier ,Src_Type NVARCHAR(255)");

            ExecQuery(" ALTER TABLE Comments ADD User_ID uniqueidentifier");

            ExecQuery(" ALTER TABLE Universities ADD Domain NVARCHAR(40)");

            ExecQuery(" ALTER TABLE Student_Universities ADD Std_Email NVARCHAR(50)");

            ExecQuery("sp_rename 'Countries.Country_Name', 'Key_', 'COLUMN' ");

            ExecQuery(" ALTER TABLE Addresses  MODIFY COLUMN latitude NVARCHAR(50); ");
            ExecQuery(" ALTER TABLE Addresses  MODIFY COLUMN longitude NVARCHAR(50); "); 

            ExecQuery("CREATE TRIGGER insert_follow ON Friends AFTER INSERT AS " +
                   "INSERT INTO Friend_Reports  select i.User_ID ,i.Friend_User_ID , "+
                   " i.Friendship_Date , i.Friendship_Statuse_ID from inserted as i; ");

            ExecQuery("CREATE TRIGGER update_follow ON Friends AFTER update AS " +
                   "INSERT INTO Friend_Reports  select i.User_ID ,i.Friend_User_ID , " +
                   " i.Friendship_Date , i.Friendship_Statuse_ID from inserted as i; ");

            /////////////////////////

            /// check for user type ==> user
            TableType type = new TableType
            {
                TableName = "User_Types",
                ID = "02dd85fe-6f1b-41ee-bee6-df82161467ce",
                Key = "user",
                Ar_Value = "مستخدم",
                En_Value = "User",
                Tr_Value = "Kullanıcı",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);


            /// check for user type ==> super admin
            type = new TableType
            {
                TableName = "User_Types",
                ID = "D07DACE1-64E9-5CFA-B55A-7178E31D0034",
                Key = "spuer_admin",
                Ar_Value = "مدير مميز",
                En_Value = "Super Admin",
                Tr_Value = "süper yönetici",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);

            /// check for user ==> admin
            DataRow users = Database.FindRow("Users", "ID", "D06DACE1-63E9-4CFA-B55A-7178E31D0034");
            if (users == null)
            {
                DateTime dateTime = DateTime.Now;
                ExecQuery("INSERT INTO Users (ID, Username, Full_name, Email, Password, Token, User_Type_ID, Date_Of_Create, Date_Of_Update)" +
                    "VALUES ('D06DACE1-63E9-4CFA-B55A-7178E31D0034', 'admin', 'Admin admin', 'admin@admin.com', '741253021220717864511724120418410161155', 'GSNX5D6WCHBR6UIHS4ZKCYACBP6IJCXB4XU76XOXTVOSPRWE1O', 'D07DACE1-64E9-5CFA-B55A-7178E31D0034'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");
            }

            /// check for post type ==> post
            type = new TableType
            {
                TableName = "Post_Types",
                ID = "767cb275-6571-4ae7-9e29-4379339cf255",
                Key = "post",
                Ar_Value = "منشور",
                En_Value = "Post",
                Tr_Value = "Yayınlanan",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);

            /// check for post type ==> Q&A
            type = new TableType
            {
                TableName = "Post_Types",
                ID = "fb05b1be-e79e-4d80-817d-55dcfe96cdef",
                Key = "Q&A",
                Ar_Value = "سؤال وجواب",
                En_Value = "Q&A",
                Tr_Value = "Soru-Cevap",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);

            /// check for post type ==> story
            type = new TableType
            {
                TableName = "Post_Types",
                ID = "4b927029-e181-4699-8767-05c36b18d824",
                Key = "story",
                Ar_Value = "حالة",
                En_Value = "Story",
                Tr_Value = "statü",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);

            /// check for post type ==> announcement
            type = new TableType
            {
                TableName = "Post_Types",
                ID = "83d0becd-831a-4e84-8d15-cdfc68330e0a",
                Key = "announcement",
                Ar_Value = "إعلان",
                En_Value = "Announcement",
                Tr_Value = "Duyuru",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);

            /// check for privacy type ==> public
            type = new TableType
            {
                TableName = "Privacies",
                ID = "7043ad6d-5994-4f6b-a02f-071228047610",
                Key = "public",
                Ar_Value = "عامة",
                En_Value = "Public",
                Tr_Value = "Genel",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);

            /// check for privacy type ==> Friends
            type = new TableType
            {
                TableName = "Privacies",
                ID = "e28f6b11-c2c5-4e7d-8174-a6b4a1561afa",
                Key = "Friends",
                Ar_Value = "أصدقاء",
                En_Value = "Friends",
                Tr_Value = "Arkadaşlar",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);


            /// check for Languages ==> Arabic
            type = new TableType
            {
                TableName = "Languages",
                ID = "979e1f0e-038c-4e43-93ac-bb8109162004",
                Key = "arabic",
                Ar_Value = "عربى",
                En_Value = "Arabic",
                Tr_Value = "Arapça",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);



            /// check for Languages ==> English
            type = new TableType
            {
                TableName = "Languages",
                ID = "3bdf13cc-a0d6-45c6-b668-ec731f4123d3",
                Key = "english",
                Ar_Value = "الإنجليزية",
                En_Value = "English",
                Tr_Value = "ingilizce",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);



            /// check for Languages ==> Turkish
            type = new TableType
            {
                TableName = "Languages",
                ID = "7be8281f-0de2-4932-8307-e8be78a58cec",
                Key = "turkish",
                Ar_Value = " التركية",
                En_Value = "Turkish",
                Tr_Value = "Türkçe",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);

            /// check for Friendship Statuses ==> accepted
            type = new TableType
            {
                TableName = "Friendship_Statuses",
                ID = "012f955b-8121-48d8-9efb-c59167c4d23d",
                Key = "accepted",
                Ar_Value = " تم القبول",
                En_Value = "Accepted",
                Tr_Value = "Türkçe",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);


            /// check for Friendship Statuses ==> pending
            type = new TableType
            {
                TableName = "Friendship_Statuses",
                ID = "34cfda02-0146-4a49-a086-4adb6213ac2d",
                Key = "pending",
                Ar_Value = " في الانتظار",
                En_Value = "Pending",
                Tr_Value = "Türkçe",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);



            /// check for Friendship Statuses ==> rejected
            type = new TableType
            {
                TableName = "Friendship_Statuses",
                ID = "8d0a5072-ac2d-42e2-a56b-08645c407751",
                Key = "rejected",
                Ar_Value = " تم الرفض",
                En_Value = "Rejected",
                Tr_Value = "Türkçe",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);




            /// check for Friendship Statuses ==> blocked
            type = new TableType
            {
                TableName = "Friendship_Statuses",
                ID = "637e8cd7-ec6f-45b2-a04d-b8d6a54b946b",
                Key = "blocked",
                Ar_Value = " حظر",
                En_Value = "Blocked",
                Tr_Value = "Türkçe",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);


            /// check for Countries ==> Turkey 
            type = new TableType
            {
                TableName = "Countries",
                ID = "721a35f2-8dd1-49d3-a25a-b78648f6c126",
                Key = "Turkey",
                Ar_Value = "تركيا",
                En_Value = "Turkey ",
                Tr_Value = "Türkiye",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);
            /// Add Image 
            DataRow image = Database.FindRow("Images", "ID", "1185f02e-d81a-454f-b366-3c52f54fab86");
            if (image == null)
            {
                DateTime dateTime = DateTime.Now;
                ExecQuery("INSERT INTO Images (ID, URL, Src_ID, Src_Type, Date_Of_Create, Date_Of_Update)" +
                    "VALUES ('1185f02e-d81a-454f-b366-3c52f54fab86', '1185f02e-d81a-454f-b366-3c52f54fab86.jpg', '721a35f2-8dd1-49d3-a25a-b78648f6c126', 'Countries'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");
            }


            /// check for Countries ==> Cyprus 
            type = new TableType
            {
                TableName = "Countries",
                ID = "54d862ab-8ba8-4c40-9009-a4318ed65017",
                Key = "Cyprus ",
                Ar_Value = "قبرص",
                En_Value = "Cyprus ",
                Tr_Value = "Kıbrıs",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);
            /// Add Image 
            image = Database.FindRow("Images", "ID", "4a0886c9-7503-4d71-9c46-86d6453822f8");
            if (image == null)
            {
                DateTime dateTime = DateTime.Now;
                ExecQuery("INSERT INTO Images (ID, URL, Src_ID, Src_Type, Date_Of_Create, Date_Of_Update)" +
                    "VALUES ('4a0886c9-7503-4d71-9c46-86d6453822f8', '4a0886c9-7503-4d71-9c46-86d6453822f8.png', '54d862ab-8ba8-4c40-9009-a4318ed65017', 'Countries'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");
            }


            /// check for like Type ==> like
            type = new TableType
            {
                TableName = "Like_Types",
                ID = "29a79640-e930-4a19-a675-e397a1a135e8",
                Key = "like",
                Ar_Value = " إعجاب",
                En_Value = "Like",
                Tr_Value = "Beğen",
                Ar_Descreption = null,
                En_Descreption = null,
                Tr_Descreption = null
            };
            Check_Add_Type(type);

            


            //int NumOfSuper = CheckForNumbers("SELECT Count(*) AS Num FROM Super_Category");

            //if (NumOfSuper == 0)
            //{
            //    ExecQuery(" Drop Table Super_Category");

            //    ExecQuery("CREATE TABLE Super_Category (ID uniqueidentifier PRIMARY KEY NOT NULL, Ar_Name NVARCHAR(255), En_Name NVARCHAR(255), Tr_Name NVARCHAR(255), num int NOT NULL IDENTITY, CategoryOrder TINYINT, ShowInMain TINYINT, Date_Of_Create DATETIME, Date_Of_Update DATETIME)");

            //    ExecQuery(" INSERT INTO Super_Category (ID ,Ar_Name ,En_Name ,Tr_Name ,CategoryOrder ,ShowInMain ,Date_Of_Create ,Date_Of_Update) " +
            //              " VALUES ('43F4CED7-BDA9-4782-B21B-0562841F6951' , 'مقاسات كبيرة' , 'Big sizes' , 'Büyük bedenler' , 1 , 1 , '2020-09-24 17:25:06.340', " +
            //              " '2020-09-24 17:25:06.340')");

            //    ExecQuery(" INSERT INTO Super_Category (ID ,Ar_Name ,En_Name ,Tr_Name ,CategoryOrder ,ShowInMain ,Date_Of_Create ,Date_Of_Update) " +
            //              " VALUES ('43F4CED7-BDA9-4782-B21B-0562841F6952' , 'ملابس محجبات' , 'clothes for veiled women' , 'örtülü kadınlar için giysiler' , 2 , 1 , '2020-09-24 17:25:06.340', " +
            //              " '2020-09-24 17:25:06.340')");

            //    ExecQuery(" INSERT INTO Super_Category (ID ,Ar_Name ,En_Name ,Tr_Name ,CategoryOrder ,ShowInMain ,Date_Of_Create ,Date_Of_Update) " +
            //              " VALUES ('43F4CED7-BDA9-4782-B21B-0562841F6953' , 'ملابس غير رسمية' , 'Casual wear' , 'Rahat kıyafet' , 3 , 1 , '2020-09-24 17:25:06.340', " +
            //              " '2020-09-24 17:25:06.340')");

            //    ExecQuery(" INSERT INTO Super_Category (ID ,Ar_Name ,En_Name ,Tr_Name ,CategoryOrder ,ShowInMain ,Date_Of_Create ,Date_Of_Update) " +
            //              " VALUES ('43F4CED7-BDA9-4782-B21B-0562841F6954' , 'ملابس سهرة' , 'Evening Wear' , 'Akşam Giyim' , 4 , 1 , '2020-09-24 17:25:06.340', " +
            //              " '2020-09-24 17:25:06.340')");

            //    ExecQuery(" INSERT INTO Super_Category (ID ,Ar_Name ,En_Name ,Tr_Name ,CategoryOrder ,ShowInMain ,Date_Of_Create ,Date_Of_Update) " +
            //              " VALUES ('43F4CED7-BDA9-4782-B21B-0562841F6955' , 'أخرى ...' , 'Other ...' , 'Diğer ...' , 5 , 0 , '2020-09-24 17:25:06.340', " +
            //              " '2020-09-24 17:25:06.340')");
            //}


            //int NumOfAdmins = CheckForNumbers("SELECT Count(*) AS Num FROM Users WHERE Is_SuperAdmin = 1");

            //if (NumOfAdmins != 1)
            //{
            //    List<string> cols = new List<string>();
            //    List<object> vals = new List<object>();
            //    cols.Add("Is_SuperAdmin");
            //    vals.Add("1");

            //    string errMessage = "";

            //    Database.DeleteRow("Users", cols, vals, out errMessage);
            //    string password = Ciphering.GetMD5HashData("0000");

            //    ExecQuery(" INSERT INTO Users(ID, Token, Username, Full_Name, Is_SuperAdmin, Password , Email, User_Type) " +
            //              " VALUES('C7C78EF1-E6FD-4799-841F-63570D8090FC', 'ST7OB080QU486JFG6LZX021US8D96R1WHRMDT76QFS509ZEV91', 'Admin', 'Admin', '1', '" + password + "' , 'admin@admin.com' , '0')");
            //}

            //int NumOfSupervisors = CheckForNumbers("SELECT Count(*) AS Num FROM Users WHERE User_Type = 5");

            //if (NumOfSupervisors != 1)
            //{
            //    List<string> cols = new List<string>();
            //    List<object> vals = new List<object>();
            //    cols.Add("User_Type");
            //    vals.Add("5");

            //    string errMessage = "";

            //    Database.DeleteRow("Users", cols, vals, out errMessage);
            //    string password = Ciphering.GetMD5HashData("kizmoda");

            //    ExecQuery(" INSERT INTO Users(ID, Token, Username, Full_Name, Is_SuperAdmin, Password , Email, User_Type) " +
            //              " VALUES('D07DACE1-64E9-5CFA-B55A-7178E31D0034', 'GSNX8D6WCHBR6UIHS8ZKCYACBP6IJCXB4XU76XOXTVOSPRWE18', 'Supervisor', 'Supervisor', '0', '"+password+"' , 'info@kizmoda.com' , '5')");
            //}


        }

        static void Check_Add_Type(TableType type)
        {
            DataRow TypeRow = Database.FindRow(type.TableName, "ID", type.ID);
            if (TypeRow == null)
            {
                DateTime dateTime = DateTime.Now;

                ExecQuery("INSERT INTO " + type.TableName + " (ID, Key_, Date_Of_Create, Date_Of_Update) " +
                    " VALUES ('" + type.ID + "', '" + type.Key + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");


                Guid Translations_ID = Guid.NewGuid();
                DataRow temp = Database.GetRow("Translations", Translations_ID);
                while (temp != null)
                {
                    Translations_ID = Guid.NewGuid();
                    temp = Database.GetRow("Translations", Translations_ID);
                }

                ExecQuery("INSERT INTO Translations (ID, Src_ID, Src_Type, Ar_Value, En_Value, Tr_Value,Ar_Descreption,En_Descreption,TR_Descreption, Date_Of_Create, Date_Of_Update)" +
                    "VALUES ('" + Translations_ID + "', '" + type.ID + "', '" + type.TableName + "', '" + type.Ar_Value + "', '" + type.En_Value + "', '" + type.Tr_Value + "','" + type.Ar_Descreption + "','" + type.En_Descreption + "','" + type.Tr_Descreption + "'," + dateTime.ToShortDateString() + "," + dateTime.ToShortDateString() + ")");
            }
        }
        //public static void CheckCurrency()
        //{
        //    DataRow dtr = Database.GetRow("Currencies", new Guid("D07DACE1-64E9-5CFA-B55A-7178E31D0000"));

        //    if (dtr == null)
        //    {
        //        ExecQuery(" INSERT INTO Currencies(ID, Ar_Name, En_Name, Tr_Name, Rate, Symbol) " +
        //                  " VALUES('D07DACE1-64E9-5CFA-B55A-7178E31D0000', 'الليرة التركية', 'Turkish Lira', 'Türk Lirası', '1', '₺')");
        //    }
        //    string msg;
        //    var cur = Database.ReadProp(10);
        //    if (cur == null || cur == DBNull.Value.ToString() || cur == "")
        //        Database.WriteProp(10, "D07DACE1-64E9-5CFA-B55A-7178E31D0000", out msg);

        //    cur = Database.ReadProp(11);
        //    if (cur == null || cur == DBNull.Value.ToString() || cur == "")
        //        Database.WriteProp(11, "D07DACE1-64E9-5CFA-B55A-7178E31D0000", out msg);

        //    cur = Database.ReadProp(12);
        //    if (cur == null || cur == DBNull.Value.ToString() || cur == "")
        //        Database.WriteProp(12, "D07DACE1-64E9-5CFA-B55A-7178E31D0000", out msg);
        //}

        public static void ExecQuery(String str)
        {
            SqlConnection cn = new SqlConnection(Database.ConnectionString);
            SqlCommand cmd = new SqlCommand(str, cn);
            try
            {
                cn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
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