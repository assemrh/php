using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace legarage.Classes
{
    public class Build_Database
    {
        public static void RebuildDatabase()
        {
            string errMessage_ = string.Empty;
            string sql = "select name,SUBSTRING(name,5,CHARINDEX('_', name, 5) - (CHARINDEX('__', name) + 2)) as tablename  from sys.objects where type_desc like '%CHECK_CONSTRAINT' and name like 'CK%'";
            DataTable cheak_constraint = Database.ReadTableByQuery(sql, null, out errMessage_);
            if (cheak_constraint != null && cheak_constraint.Rows.Count > 0)
            {
                foreach (DataRow item in cheak_constraint.Rows)
                {
                    ExecQuery(" alter table " + item["tablename"].ToString() + " drop constraint " + item["name"].ToString());
                }
            }
            //ExecQuery("CREATE TABLE Users (ID uniqueidentifier PRIMARY KEY NOT NULL, Token NVARCHAR(50), Username NVARCHAR(100), Full_Name NVARCHAR(100)," +
            //    "Email NVARCHAR(50), User_Type TINYINT, Mobile NVARCHAR(50), Role_ID uniqueidentifier, Country_ID uniqueidentifier, City_ID uniqueidentifier," +
            //    "Is_Company TINYINT, Company_ID uniqueidentifier, Master_ID uniqueidentifier, Address NVARCHAR(MAX)," +
            //    "Password NVARCHAR(100), Date_Of_Create DATETIME, Is_Customer TINYINT, Date_Of_Update DATETIME)"); 
            /*1*/ /*ExecQuery("CREATE TABLE Users (ID uniqueidentifier PRIMARY KEY NOT NULL, Full_Name NVARCHAR(255) NULL, Email NVARCHAR(255) NULL, Phone NVARCHAR(255) NULL, Password NVARCHAR(255) NULL, Token NVARCHAR(255) NULL, Address_ID uniqueidentifier NULL, Is_Admin TINYINT NULL, [website] nvarchar(255), [youtube] nvarchar(255), [linkedin] nvarchar(255), [instagram] nvarchar(255), [twitter] nvarchar(255), [snapchat] nvarchar(255), [tiktok] nvarchar(255), [facebook] nvarchar(255), [whatsapp] nvarchar(255), [fax] nvarchar(255),  [description] text, , Created_At DATETIME NULL, Updated_At DATETIME NULL)");*/

            /*1*/
            ExecQuery("CREATE TABLE [Users] ( " +
                " [id] uniqueidentifier PRIMARY KEY NOT NULL, " +
                " [full_name] nvarchar(255), " +
                " [email] nvarchar(255), " +
                " [phone] nvarchar(255), " +
                " [password] nvarchar(255), " +
                " [token] nvarchar(255), " +
                " [address_id] uniqueidentifier, " +
                " [is_admin] tinyint, " +
                " [website] nvarchar(255), " +
                " [youtube] nvarchar(255), " +
                " [linkedin] nvarchar(255), " +
                " [instagram] nvarchar(255), " +
                " [twitter] nvarchar(255), " +
                " [snapchat] nvarchar(255), " +
                " [tiktok] nvarchar(255), " +
                " [facebook] nvarchar(255), " +
                " [whatsapp] nvarchar(255), " +
                " [fax] nvarchar(255), " +
                " [description] text, " +
                " [created_at] datetime, " +
                " [updated_at] datetime)");


            ExecQuery("ALTER TABLE Users ADD username  nvarchar(255)");
            ExecQuery("ALTER TABLE Users ALTER COLUMN password nvarchar(50); ");

            /*1.5*/
            ExecQuery("ALTER TABLE Users ADD nickname  nvarchar(255)");

            /*2*/
            //ExecQuery("CREATE TABLE [dbo].[Countries]([id][uniqueidentifier] PRIMARY KEY NOT NULL, [name][nvarchar](255) NULL, [code][nvarchar](255) NULL,[created_at][datetime] NULL, [updated_at][datetime] NULL)");
            ExecQuery("CREATE TABLE [Countries] ( " +
                " [Id] [uniqueidentifier] PRIMARY KEY NOT NULL, " +
                " [Name] [nvarchar](255) NULL , " +
                " [ar_name] [nvarchar](255) NULL , " +
                " [ISO] [nvarchar](2) NULL , " +
                " [ISO3] [nvarchar](3) NULL , " +
                " [Phone_key] [int] NULL, " +
                " [is_market] [tinyint] NULL , " +
                " [is_factory] [tinyint] NULL , " +
                " [created_at] [datetime] NULL , " +
                " [updated_at] [datetime] NULL );");
            /*3 */

            /*4*/
            ExecQuery("CREATE TABLE [Provinces] ( " +
                     " [id] uniqueidentifier PRIMARY KEY, " +
                     " [name] nvarchar(255), " +
                     " [country_id] uniqueidentifier, [created_at] datetime, [updated_at] datetime)");

            /*5*/
            ExecQuery("CREATE TABLE [Addresses] ( " +
                     " [id] uniqueidentifier PRIMARY KEY, " +
                     " [name] nvarchar(255), " +
                     " [country_id] uniqueidentifier, " +
                     " [province_id] uniqueidentifier, " +
                     " [created_at] datetime, " +
                     " [updated_at] datetime)");

            /*6*/
            ExecQuery("CREATE TABLE [Categories] ( " +
                     " [id] uniqueidentifier PRIMARY KEY, " +
                     " [name] nvarchar(255), " +
                     " [parent_category_id] uniqueidentifier, " +
                     " [created_at] datetime, " +
                     " [updated_at] datetime)");

            /*6.5*/
            //ExecQuery("CREATE TABLE [Services] ( " +
            //              " [id] uniqueidentifier PRIMARY KEY, " +
            //              " [name] nvarchar(255), " +
            //              " [created_at] datetime, " +
            //              " [updated_at] datetime)");

            /*7*/
            ExecQuery("CREATE TABLE [Products] ( " +
                            " [id] uniqueidentifier PRIMARY KEY, " +
                            " [name] nvarchar(255), " +
                            " [category_id] uniqueidentifier, " +
                            " [description] text, " +
                            " [price] decimal, " +
                            " [status] nvarchar(255) NOT NULL CHECK([status] IN('out_of_stock', 'in_stock', 'best_seller', 'running_low')), " +
                            " [store_id] uniqueidentifier, " +
                            " [model_id] uniqueidentifier, " +
                            " [vehicle_type_id] uniqueidentifier, " +
                            " [is_new] tinyint, " +
                            " [created_at] datetime, " +
                            " [updated_at] datetime)");

            ExecQuery("ALTER TABLE Products ADD brands text");
            ExecQuery("ALTER TABLE Products ADD vehicletypes nvarchar(255)");
            ExecQuery("ALTER TABLE Products ADD paymentmethods nvarchar(255)");
            ExecQuery("ALTER TABLE Products ADD mobile nvarchar(255)");
            ExecQuery("ALTER TABLE Products ADD website nvarchar(255)");
            ExecQuery("ALTER TABLE Products ADD condition nvarchar(255)");
            ExecQuery("ALTER TABLE Products ADD categories nvarchar(255)");
            ExecQuery("ALTER TABLE Products ADD quantity int");
            ExecQuery("ALTER TABLE Products ADD keywords nvarchar(MAX)");

            /*8*/
            ExecQuery("CREATE TABLE [Vehicles] ( " +
                             " [id] uniqueidentifier PRIMARY KEY, " +
                             " [name] nvarchar(255), " +
                             " [description] text, " +
                             " [price] decimal, " +
                             " [status] nvarchar(255) NOT NULL CHECK([status] IN('out_of_stock', 'in_stock', 'best_seller', 'running_low')), " +
                             " [model_id] uniqueidentifier, " +
                             " [store_id] uniqueidentifier, " +
                             " [vehicle_type_id] uniqueidentifier, " +
                             " [is_new] tinyint, " +
                             " [created_at] datetime, " +
                             " [updated_at] datetime)");

            ExecQuery("ALTER TABLE Vehicles ADD brand nvarchar(255)");
            ExecQuery("ALTER TABLE Vehicles ADD paymentmethod nvarchar(255)");
            ExecQuery("ALTER TABLE Vehicles ADD mobile nvarchar(255)");
            ExecQuery("ALTER TABLE Vehicles ADD website nvarchar(255)");
            ExecQuery("ALTER TABLE Vehicles ADD vehicle_price nvarchar(255)");
            ExecQuery("ALTER TABLE Vehicles ADD condition nvarchar(255)");
            ExecQuery("ALTER TABLE Vehicles ADD quantity int");
            ExecQuery("ALTER TABLE Vehicles ADD keywords nvarchar(MAX)");

            /*9*/
            ExecQuery("CREATE TABLE [Garages] ( " +
                            " [id] uniqueidentifier PRIMARY KEY, " +
                            " [name] nvarchar(255), " +
                            " [address_id] uniqueidentifier, " +
                            " [user_id] uniqueidentifier, " +
                            " [whatsapp] nvarchar(255), " +
                            " [facebook] nvarchar(255), " +
                            " [website] nvarchar(255), " +
                            " [youtube] nvarchar(255), " +
                            " [linkedin] nvarchar(255), " +
                            " [instagram] nvarchar(255), " +
                            " [twitter] nvarchar(255), " +
                            " [snapchat] nvarchar(255), " +
                            " [tiktok] nvarchar(255), " +
                            " [mobile] nvarchar(255), " +
                            " [fax] nvarchar(255), " +
                            " [description] text, " +
                            " [created_at] datetime, " +
                            " [updated_at] datetime)");

            ExecQuery("ALTER TABLE Garages ADD services nvarchar(255)");
            ExecQuery("ALTER TABLE Garages ADD vehicletypes nvarchar(255)");
            ExecQuery("ALTER TABLE Garages ADD brands text");
            ExecQuery("ALTER TABLE Garages ADD pricelist text");
            ExecQuery("ALTER TABLE Garages ADD keywords nvarchar(MAX)");

            /*10*/
            ExecQuery("CREATE TABLE [Rental_Offices] ( " +
                              " [id] uniqueidentifier PRIMARY KEY, " +
                              " [name] nvarchar(255), " +
                              " [address_id] uniqueidentifier, " +
                              " [user_id] uniqueidentifier, " +
                              " [whatsapp] nvarchar(255), " +
                              " [facebook] nvarchar(255), " +
                              " [website] nvarchar(255), " +
                              " [youtube] nvarchar(255), " +
                              " [linkedin] nvarchar(255), " +
                              " [instagram] nvarchar(255), " +
                              " [twitter] nvarchar(255), " +
                              " [snapchat] nvarchar(255), " +
                              " [tiktok] nvarchar(255), " +
                              " [mobile] nvarchar(255), " +
                              " [fax] nvarchar(255), " +
                              " [description] text, " +
                              " [created_at] datetime, " +
                              " [updated_at] datetime)");


            ExecQuery("ALTER TABLE Rental_Offices ADD models text");
            ExecQuery("ALTER TABLE Rental_Offices ADD vehicletypes nvarchar(255)");
            ExecQuery("ALTER TABLE Rental_Offices ADD brands text");
            ExecQuery("ALTER TABLE Rental_Offices ADD pricelist text");


            /*11*/
            ExecQuery("CREATE TABLE [Stores] ( " +
                                  " [id] uniqueidentifier PRIMARY KEY, " +
                                  " [name] nvarchar(255), " +
                                  " [address_id] uniqueidentifier, " +
                                  " [user_id] uniqueidentifier, " +
                                  " [whatsapp] nvarchar(255), " +
                                  " [facebook] nvarchar(255), " +
                                  " [website] nvarchar(255), " +
                                  " [youtube] nvarchar(255), " +
                                  " [linkedin] nvarchar(255), " +
                                  " [instagram] nvarchar(255), " +
                                  " [twitter] nvarchar(255), " +
                                  " [snapchat] nvarchar(255), " +
                                  " [tiktok] nvarchar(255), " +
                                  " [mobile] nvarchar(255), " +
                                  " [fax] nvarchar(255), " +
                                  " [description] text, " +
                                  " [created_at] datetime, " +
                                  " [updated_at] datetime)");

            /*12*/
            ExecQuery("CREATE TABLE [Brands] (" +
            " [id] uniqueidentifier PRIMARY KEY, " +
            " [name] nvarchar(255), " +
            " [country_id] uniqueidentifier, " +
            " [created_at] datetime, " +
            " [updated_at] datetime)");

            /*13*/
            ExecQuery("CREATE TABLE [Models] ( " +
              " [id] uniqueidentifier PRIMARY KEY, " +
              " [name] nvarchar(255), " +
              " [brand_id] uniqueidentifier, " +
              " [parent_model_id] uniqueidentifier, " +
              " [vehicle_type_id] uniqueidentifier, " +
              " [created_at] datetime, " +
              " [updated_at] datetime)");

            /*14*/
            ExecQuery("CREATE TABLE [Winches] ( " +
                  " [id] uniqueidentifier PRIMARY KEY, " +
                  " [driver_name] nvarchar(255), " +
                  " [driver_phone] nvarchar(255), " +
                  " [address_id] uniqueidentifier, " +
                  " [user_id] uniqueidentifier, " +
                  " [whatsapp] nvarchar(255), " +
                  " [facebook] nvarchar(255), " +
                  " [website] nvarchar(255), " +
                  " [youtube] nvarchar(255), " +
                  " [linkedin] nvarchar(255), " +
                  " [instagram] nvarchar(255), " +
                  " [twitter] nvarchar(255), " +
                  " [snapchat] nvarchar(255), " +
                  " [tiktok] nvarchar(255), " +
                  " [mobile] nvarchar(255), " +
                  " [fax] nvarchar(255), " +
                  " [description] text, " +
                  " [created_at] datetime, " +
                  " [updated_at] datetime)");

            ExecQuery("ALTER TABLE Winches ADD service_price decimal(18,2)");
            ExecQuery("ALTER TABLE Winches ADD area nvarchar(255)");
            ExecQuery("ALTER TABLE Winches ADD vehicletypes nvarchar(255)");
            ExecQuery("ALTER TABLE Winches ADD paymentmethods nvarchar(255)");
            ExecQuery("ALTER TABLE Winches ADD vehiclesizes nvarchar(255)");
            ExecQuery("ALTER TABLE Winches ADD price nvarchar(255)");
            ExecQuery("ALTER TABLE Winches ADD keywords nvarchar(MAX)");

            /*15*/
            ExecQuery("CREATE TABLE [Offers] ( " +
                                    " [id] uniqueidentifier PRIMARY KEY, " +
                                    " [description] text, " +
                                    " [details] text, " +
                                    " [referal_id] uniqueidentifier, " +
                                    " [referal_type] nvarchar(255), " +
                                    " [start_date] datetime, " +
                                    " [end_date] datetime, " +
                                    " [discount_percentage] float, " +
                                    " [created_at] datetime, " +
                                    " [updated_at] datetime)");


            ExecQuery("ALTER TABLE Offers ADD name nvarchar(255)");
            ExecQuery("ALTER TABLE Offers ADD old_price decimal(18,2)");
            ExecQuery("ALTER TABLE Offers ADD paymentmethods nvarchar(255)");
            ExecQuery("ALTER TABLE Offers ADD address_id uniqueidentifier");
            ExecQuery("ALTER TABLE Offers ADD mobile nvarchar(255)");
            ExecQuery("ALTER TABLE Offers ADD website nvarchar(255)");

            /*16*/
            ExecQuery("CREATE TABLE [Comments] ( " +
                                  " [id] uniqueidentifier PRIMARY KEY, " +
                                  " [comment] nvarchar(255), " +
                                  " [user_id] uniqueidentifier, " +
                                  " [rating] nvarchar(255) NOT NULL CHECK([rating] IN('1', '2', '3', '4', '5')), " +
                                  " [created_at] datetime, " +
                                  " [updated_at] datetime)");

            /*17*/
            ExecQuery("CREATE TABLE [Rating] ( " +
              " [id] uniqueidentifier PRIMARY KEY, " +
              " [value] nvarchar(255), " +
              " [created_at] datetime, " +
              " [updated_at] datetime)");

            /*18*/
            ExecQuery("CREATE TABLE [User_Rating] ( " +
             " [id] uniqueidentifier PRIMARY KEY, " +
             " [user_id] uniqueidentifier, " +
             " [rating_id] uniqueidentifier, " +
             " [created_at] datetime, " +
             " [updated_at] datetime)");
            ExecQuery("DROP TABLE User_Rating;");
            ExecQuery("ALTER TABLE Rating ADD src_id uniqueidentifier; ");
            ExecQuery("ALTER TABLE Rating ADD user_id uniqueidentifier; ");
            ExecQuery("ALTER TABLE Rating ADD keywords nvarchar(MAX); ");


            /*19*/
            ExecQuery("CREATE TABLE [Properties] ( " +
                                 " [id] uniqueidentifier PRIMARY KEY, " +
                                 " [value] nvarchar(255), " +
                                 " [for_vehicles] tinyint, " +
                                 " [for_products] tinyint, " +
                                 " [show_in_main_for_products] tinyint, " +
                                 " [show_in_main_for_vehicles] tinyint, " +
                                 " [created_at] datetime, " +
                                 " [updated_at] datetime)");

            /*20*/
            ExecQuery("CREATE TABLE [Images] ( " +
          " [id] uniqueidentifier PRIMARY KEY, " +
          " [url] nvarchar(255), " +
          " [referral_id] uniqueidentifier, " +
          " [referral_type] nvarchar(255), " +
          " [created_at] datetime, " +
          " [updated_at] datetime)");

            /*21*/
            ExecQuery("CREATE TABLE [Slider] ( " +
         " [id] uniqueidentifier PRIMARY KEY, " +
         " [referral_id] uniqueidentifier, " +
         " [referral_type] nvarchar(255), " +
         " [image_id] uniqueidentifier, " +
         " [link] nvarchar(255), " +
         " [created_at] datetime, " +
         " [updated_at] datetime)");

            /*22*/
            ExecQuery("CREATE TABLE [Vehicle_Types] ( " +
         " [id] uniqueidentifier PRIMARY KEY, " +
         " [type_name] nvarchar(255), " +
         " [created_at] datetime, " +
         " [updated_at] datetime)");

            /*23*/
            ExecQuery("CREATE TABLE [Products_Properties] ( " +
             " [id] uniqueidentifier PRIMARY KEY, " +
             " [product_id] uniqueidentifier, " +
             " [property_id] uniqueidentifier, " +
             " [value] nvarchar(255), " +
             " [created_at] datetime, " +
             " [updated_at] datetime)");

            //Products_Categories
            /*23.5*/
            ExecQuery("CREATE TABLE [Products_Categories] ( " +
              " [id] uniqueidentifier PRIMARY KEY, " +
              " [product_id] uniqueidentifier, " +
              " [category_id] uniqueidentifier, " +
              " [created_at] datetime, " +
              " [updated_at] datetime)");


            /*24*/
            ExecQuery("CREATE TABLE [Garages_Categories] ( " +
                  " [id] uniqueidentifier PRIMARY KEY, " +
                  " [garage_id] uniqueidentifier, " +
                  " [category_id] uniqueidentifier, " +
                  " [created_at] datetime, " +
                  " [updated_at] datetime)");


            /*24.5*/
            //ExecQuery("CREATE TABLE [Garages_Services] ( " +
            //         " [id] uniqueidentifier PRIMARY KEY, " +
            //         " [garage_id] uniqueidentifier, " +
            //         " [service_id] uniqueidentifier, " +
            //         " [created_at] datetime, " +
            //         " [updated_at] datetime)");


            /*25*/
            ExecQuery("CREATE TABLE [Garages_Models] ( " +
                             " [id] uniqueidentifier PRIMARY KEY, " +
                             " [garage_id] uniqueidentifier, " +
                             " [model_id] uniqueidentifier, " +
                             " [created_at] datetime, " +
                             " [updated_at] datetime)");

            /*26*/
            ExecQuery("CREATE TABLE [Rental_Offices_Models] ( " +
                        " [id] uniqueidentifier PRIMARY KEY, " +
                        " [rental_office_id] uniqueidentifier, " +
                        " [model_id] uniqueidentifier, " +
                        " [created_at] datetime, " +
                        " [updated_at] datetime)");

            /* *27 */
            ExecQuery("CREATE TABLE [Vehicle_Types_Garages] ( " +
                 " [id] uniqueidentifier PRIMARY KEY, " +
                 " [vehicle_type_id] uniqueidentifier, " +
                 " [garage_id] uniqueidentifier, " +
                 " [created_at] datetime, " +
                 " [updated_at] datetime)");

            /* *28 */
            ExecQuery("CREATE TABLE [Vehicle_Types_Rental_Offices] ( " +
                " [id] uniqueidentifier PRIMARY KEY, " +
                " [vehicle_type_id] uniqueidentifier, " +
                " [rental_office_id] uniqueidentifier, " +
                " [created_at] datetime, " +
                " [updated_at] datetime)");

            /* *29 */
            ExecQuery("CREATE TABLE [Vehicles_Properties] ( " +
                " [id] uniqueidentifier PRIMARY KEY, " +
                " [vehicle_id] uniqueidentifier, " +
                " [property_id] uniqueidentifier, " +
                " [value] nvarchar(255), " +
                " [created_at] datetime, " +
                " [updated_at] datetime)");

            /* *30 */
            ExecQuery("CREATE TABLE [Users_Offers] ( " +
                  " [id] uniqueidentifier PRIMARY KEY, " +
                  " [user_id] uniqueidentifier, " +
                  " [offer_id] uniqueidentifier, " +
                  " [created_at] datetime, " +
                  " [updated_at] datetime)");


            //add to Garages table, do services and garages_services and products_categories and offers and change all description to text and fax and mobile and check ids and others

            //////////////views

            ExecQuery("Create View GetAllUsers as SELECT  dbo.Users.id, dbo.Users.full_name," +
                " dbo.Users.email, dbo.Users.phone, dbo.Users.password, dbo.Addresses.name, " +
                " dbo.Provinces.name AS province, dbo.Countries.name AS country FROM  dbo.Addresses INNER JOIN" +
                 " dbo.Users ON dbo.Addresses.id = dbo.Users.address_id INNER JOIN  dbo.Countries " +
                 " ON dbo.Addresses.country_id = dbo.Countries.id INNER JOIN  dbo.Provinces ON " +
                 " dbo.Addresses.province_id = dbo.Provinces.id  where  dbo.Users.is_admin = 0 ");
            //ExecQuery("ALTER TABLE Countries ADD is_market tinyint; ");
            //ExecQuery("ALTER TABLE Countries DROP COLUMN is_marketing ;");



            ExecQuery("ALTER View [dbo].[GetAllUsers] as SELECT  dbo.Users.id, dbo.Users.full_name, dbo.Users.email , dbo.Users.user_name, dbo.Users.phone, dbo.Users.nickname, dbo.Addresses.name as address_desc,  dbo.Provinces.name AS province, dbo.Countries.name AS country FROM  dbo.Addresses INNER JOIN dbo.Users ON dbo.Addresses.id = dbo.Users.address_id INNER JOIN  dbo.Countries  ON dbo.Addresses.country_id = dbo.Countries.id INNER JOIN  dbo.Provinces ON  dbo.Addresses.province_id = dbo.Provinces.id  where  dbo.Users.is_admin = 0 ");
            ExecQuery("ALTER TABLE Users DROP COLUMN nickname ; ");
            ExecQuery("ALTER TABLE Addresses DROP COLUMN name ;");
            ExecQuery("ALTER TABLE Addresses DROP COLUMN country_id ;");
            ExecQuery("ALTER TABLE Addresses ADD details NVARCHAR(50);");

            ExecQuery("ALTER TABLE Garages DROP COLUMN services;");
            ExecQuery("ALTER TABLE Garages DROP COLUMN pricelist;");
            ExecQuery("ALTER TABLE Garages DROP COLUMN brands;");
            ExecQuery("ALTER TABLE Garages DROP COLUMN vehicletypes;");
            ExecQuery("ALTER TABLE Vehicles DROP COLUMN brand;");
            //ExecQuery("ALTER TABLE Vehicles DROP constraint CK__Vehicles__status__6EF57B66;");
            ExecQuery("ALTER TABLE Vehicles DROP COLUMN status;");

            ExecQuery("DROP TABLE Garages_Models;");
            ExecQuery("CREATE TABLE [Garages_Brands] ( " +
                 " [id] uniqueidentifier PRIMARY KEY, " +
                 " [garage_id] uniqueidentifier, " +
                 " [brand_id] uniqueidentifier, " +
                 " [created_at] datetime, " +
                 " [updated_at] datetime)");
            ExecQuery("ALTER TABLE Rental_Offices DROP COLUMN brands;");
            ExecQuery("ALTER TABLE Rental_Offices DROP COLUMN pricelist;");
            ExecQuery("ALTER TABLE Vehicles DROP COLUMN condition;");
            ExecQuery("ALTER TABLE Vehicles DROP COLUMN vehicle_price;");
            ExecQuery("ALTER TABLE Vehicles DROP COLUMN website;");
            ExecQuery("ALTER TABLE Vehicles DROP COLUMN paymentmethod;");
            ExecQuery("ALTER TABLE Vehicles DROP COLUMN name;");
            ExecQuery("ALTER TABLE Vehicles ADD owner_name text;");
            ExecQuery("ALTER TABLE Vehicles ADD address_id uniqueidentifier;");
            ExecQuery("ALTER TABLE Vehicles ADD user_id uniqueidentifier;");
            ExecQuery("ALTER TABLE Vehicles DROP COLUMN vehicletype;");
            ExecQuery("ALTER TABLE Products DROP COLUMN categories;");
            ExecQuery("ALTER TABLE Products DROP COLUMN condition;");
            ExecQuery("ALTER TABLE Products DROP COLUMN website;");
            ExecQuery("ALTER TABLE Products DROP COLUMN paymentmethods;");
            ExecQuery("ALTER TABLE Products DROP COLUMN vehicletypes;");
            ExecQuery("ALTER TABLE Products DROP COLUMN brands;");
            ExecQuery("ALTER TABLE Products DROP COLUMN status;");
            ExecQuery("ALTER TABLE Products DROP COLUMN name;");
            ExecQuery("ALTER TABLE Products ADD user_id uniqueidentifier;");
            ExecQuery("ALTER TABLE Products ADD owner_name text;");
            ExecQuery("ALTER TABLE Products ADD address_id uniqueidentifier;");
            ExecQuery("ALTER TABLE Products ADD title text;");
            ExecQuery("ALTER TABLE Vehicles ADD year text;");
            ExecQuery("ALTER TABLE Vehicles ADD mieage text;");
            ExecQuery("ALTER TABLE Vehicles ADD gearbox text;");
            ExecQuery("ALTER TABLE Vehicles ADD fuel_type text;");
            ExecQuery("ALTER TABLE Vehicles drop COLUMN fuel_type;");
            ExecQuery("ALTER TABLE Vehicles ADD engine_size text;");
            ExecQuery("ALTER TABLE Vehicles ADD color text;");
            ExecQuery("ALTER TABLE Vehicles ADD title text;");
            ExecQuery("ALTER TABLE Vehicles ADD fuel_type tinyint;");
            ExecQuery("ALTER TABLE Images ADD is_main tinyint;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN facebook;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN website;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN youtube;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN linkedin;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN instagram;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN twitter;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN snapchat;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN tiktok;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN fax;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN service_price;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN paymentmethods;");
            ExecQuery("ALTER TABLE Winches ADD title text;");

            ExecQuery("ALTER TABLE Products ADD year text;");
            ExecQuery("ALTER TABLE Products ADD mieage text;");
            ExecQuery("ALTER TABLE Products ADD gearbox text;");
            ExecQuery("ALTER TABLE Products ADD engine_size text;");
            ExecQuery("ALTER TABLE Products ADD color text;");
            ExecQuery("ALTER TABLE Products ADD fuel_type tinyint;");
            ExecQuery("ALTER TABLE Products ADD whatsapp nvarchar(255);");

            ExecQuery("ALTER TABLE Products ADD year text;");
            ExecQuery("ALTER TABLE Products ADD mieage text;");
            ExecQuery("ALTER TABLE Products ADD gearbox text;");
            ExecQuery("ALTER TABLE Products ADD engine_size text;");
            ExecQuery("ALTER TABLE Products ADD color text;");
            ExecQuery("ALTER TABLE Products ADD fuel_type text;");
            ExecQuery("ALTER TABLE Products ADD whatsapp text;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN vehicletypes;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN vehiclesizes;");
            ExecQuery("ALTER TABLE Winches ADD vehiclesize text;");
            ExecQuery("ALTER TABLE Products ADD brand_id uniqueidentifier;");
            ExecQuery("ALTER TABLE Products DROP COLUMN category_id;");

            ExecQuery("ALTER TABLE Winches DROP COLUMN area;");
            ExecQuery("ALTER TABLE Winches DROP COLUMN price;");
            ExecQuery("ALTER TABLE Winches ADD area text;");

            ExecQuery("ALTER TABLE Slider DROP COLUMN image_id;");
            ExecQuery("ALTER TABLE Slider ADD description text;");
            ExecQuery("ALTER TABLE Slider ADD roworder int;");
            ExecQuery("ALTER TABLE Slider ADD title text;");
            ExecQuery("ALTER TABLE Users ADD user_type tinyint;");

            // ExecQuery("DROP TABLE Countries;"); // if there any data will be droped

            /*08-03-2021*   ALTER Offers  */
            ExecQuery(@"ALTER TABLE Offers DROP COLUMN  details  ;");
            ExecQuery(@"ALTER TABLE Offers DROP COLUMN  old_price  ;");
            ExecQuery(@"ALTER TABLE Offers ADD  Is_Active tinyint DEFAULT 1 ;");
            /*08-03-2021*   CREATE Products_Offers  */
            ExecQuery(@"CREATE TABLE Offers_Products  (
                            [Id] uniqueidentifier PRIMARY KEY NOT NULL,
                            [Product_id] uniqueidentifier ,
                            [offer_id] uniqueidentifier,    
                            [created_at] datetime,
                            [updated_at] datetime 
                            );");
            /*08-03-2021*   CREATE Categories_Offers  */
            ExecQuery(@"CREATE TABLE Offers_Categories  (
                            [Id] uniqueidentifier PRIMARY KEY NOT NULL,
                            [category_id] uniqueidentifier ,
                            [offer_id] uniqueidentifier ,    
                            [created_at] datetime,
                            [updated_at] datetime
                            );");
            /*08-03-2021*   CREATE Offers_Models  */
            ExecQuery(@"CREATE TABLE Offers_Models  (
                            [Id] uniqueidentifier PRIMARY KEY NOT NULL,
                            [model_id] uniqueidentifier,
                            [offer_id] uniqueidentifier,    
                            [created_at] datetime,
                            [updated_at] datetime
                        );");
            /*08-03-2021*   CREATE Offers_Brands  */
            ExecQuery(@"CREATE TABLE Offers_Brands  (
                            [Id] uniqueidentifier PRIMARY KEY NOT NULL,
                            [brand_id] uniqueidentifier,
                            [offer_id] uniqueidentifier,    
                            [created_at] datetime,
                            [updated_at] datetime
                        );");
            /*08-03-2021*   CREATE Vehicle_Types_Offers  */
            ExecQuery(@"CREATE TABLE Vehicle_Types_Offers  (
                            [Id] uniqueidentifier PRIMARY KEY NOT NULL,
                            [vehicle_type_id] uniqueidentifier,
                            [offer_id] uniqueidentifier,    
                            [created_at] datetime,
                            [updated_at] datetime
                        );");
            ExecQuery(@"DROP TABLE Vehicle_Types_Offers ");
            ExecQuery(@"CREATE TABLE Offers_Vehicle_Types  (
                            [Id] uniqueidentifier PRIMARY KEY NOT NULL,
                            [vehicle_type_id] uniqueidentifier,
                            [offer_id] uniqueidentifier,    
                            [created_at] datetime,
                            [updated_at] datetime
                        );");

            ExecQuery("ALTER TABLE Offers ADD Owner_Id uniqueidentifier;");

            ExecQuery("ALTER TABLE Rating ADD src_type nvarchar(255);");
            ExecQuery("ALTER TABLE Rating ALTER COLUMN value float;");

            //ExecQuery("ALTER TABLE Rating ADD src_type nvarchar(255);");




            int NumOfAdmins = CheckForNumbers("SELECT Count(*) AS Num FROM Users WHERE is_admin = 1");

            if (NumOfAdmins != 1)
            {
                List<string> cols = new List<string>();
                List<object> vals = new List<object>();
                cols.Add("is_admin");
                vals.Add("1");

                string errMessage = "";

                Database.DeleteRow("Users", cols, vals, out errMessage);
                string password = Ciphering.GetMD5HashData("0000");
                ExecQuery(" INSERT INTO Users(ID, Token, Username, Full_Name, is_admin, Password, Email) " +
                          " VALUES('D06DACE1-63E9-4CFA-B55A-7178E31D0034', 'GSNX5D6WCHBR6UIHS4ZKCYACBP6IJCXB4XU76XOXTVOSPRWE1O', 'admin', 'admin', '1', '" + password + "','admin@legarage.com')");
            }
        }

        public static void EnterCuntries()
        {
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Afghanistan' , 'أفغانستان' , 'AF' , 'AFG' , 0 , 0 , 93 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Albania' , 'ألبانيا' , 'AL' , 'ALB' , 0 , 0 , 355 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Algeria' , 'الجزائر' , 'DZ' , 'DZA' , 0 , 0 , 213 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'American Samoa' , 'ساموا الأمريكية' , 'AS' , 'ASM' , 0 , 0 , 1684 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Andorra' , 'أندورا' , 'AD' , 'AND' , 0 , 0 , 376 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Angola' , 'أنغولا' , 'AO' , 'AGO' , 0 , 0 ,  244 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Anguilla' , 'أنغيلا' , 'AI' , 'AIA' , 0 , 0 , 1264 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Antigua and Barbuda' , 'أنتيغوا وبربودا' , 'AG' , 'ATG' , 0 , 0 , 1268 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Argentina' , 'الأرجنتين' , 'AR' , 'ARG' , 0 , 0 , 54 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Armenia' , 'أرمينيا' , 'AM' , 'ARM' , 0 , 0 , 374 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Aruba' , 'أروبا' , 'AW' , 'ABW' , 0 , 0 , 297 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Australia' , 'أستراليا' , 'AU' , 'AUS' , 0 , 0 , 61 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Austria' , 'النمسا' , 'AT' , 'AUT' , 0 , 0 , 43 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Azerbaijan' , 'أذربيجان' , 'AZ' , 'AZE' , 0 , 0 , 994 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Bahamas' , 'جزر البهاما' , 'BS' , 'BHS' , 0 , 0 , 1242 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Bahrain' , 'البحرين' , 'BH' , 'BHR' , 0 , 0 , 973 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Bangladesh' , 'بنغلاديش' , 'BD' , 'BGD' , 0 , 0 , 880 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Barbados' , 'بربادوس' , 'BB' , 'BRB' , 0 , 0 , 1246 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Belarus' , 'بيلاروسيا' , 'BY' , 'BLR' , 0 , 0 , 375 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Belgium' , 'بلجيكا' , 'BE' , 'BEL' , 0 , 0 , 32 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Belize' , 'بليز' , 'BZ' , 'BLZ' , 0 , 0 , 501 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Benin' , 'بنين' , 'BJ' , 'BEN' , 0 , 0 , 229 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Bermuda' , 'برمودا' , 'BM' , 'BMU' , 0 , 0 , 1441 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Bhutan' , 'بوتان' , 'BT' , 'BTN' , 0 , 0 , 975 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Bolivia' , 'بوليفيا' , 'BO' , 'BOL' , 0 , 0 , 591 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Bosnia and Herzegovina' , 'البوسنة والهرسك' , 'BA' , 'BIH' , 0 , 0 , 387 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Botswana' , 'بوتسوانا' , 'BW' , 'BWA' , 0 , 0 , 267 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Brazil' , 'البرازيل' , 'BR' , 'BRA' , 0 , 0 , 55 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'British Virgin Islands' , 'جزر فيرجن البريطانية' , 'VG' , 'VGB' , 0 , 0 , 1284 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Brunei Darussalam' , 'بروناي دار السلام' , 'BN' , 'BRN' , 0 , 0 , 673 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Bulgaria' , 'بلغاريا' , 'BG' , 'BGR' , 0 , 0 , 359 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Burkina Faso' , 'بوركينا فاسو' , 'BF' , 'BFA' , 0 , 0 , 226 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Burundi' , 'بوروندي' , 'BI' , 'BDI' , 0 , 0 , 257 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Cambodia' , 'كمبوديا' , 'KH' , 'KHM' , 0 , 0 , 855 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Cameroon' , 'الكاميرون' , 'CM' , 'CMR' , 0 , 0 , 237 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Canada' , 'كندا' , 'CA' , 'CAN' , 0 , 0 , 1 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Cape Verde' , 'الرأس الأخضر' , 'CV' , 'CPV' , 0 , 0 , 238 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Cayman Islands' , 'جزر كايمان' , 'KY' , 'CYM' , 0 , 0 , 1345 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Central African Republic' , 'جمهورية افريقيا الوسطى' , 'CF' , 'CAF' , 0 , 0 , 236 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Chad' , 'تشاد' , 'TD' , 'TCD' , 0 , 0 , 235 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Chile' , 'تشيلي' , 'CL' , 'CHL' , 0 , 0 , 56 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'China' , 'الصين' , 'CN' , 'CHN' , 0 , 0 , 86 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Colombia' , 'كولومبيا' , 'CO' , 'COL' , 0 , 0 , 57 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Comoros' , 'جزر القمر' , 'KM' , 'COM' , 0 , 0 , 269 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Congo' , 'الكونغو' , 'CG' , 'COG' , 0 , 0 , 243 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Cook Islands' , 'جزر كوك' , 'CK' , 'COK' , 0 , 0 , 682 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Costa Rica' , 'كوستا ريكا' , 'CR' , 'CRI' , 0 , 0 , 506 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Cote d'Ivoire' , 'كوت ديفوار' , 'CI' , 'CIV' , 0 , 0 , 225 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Croatia' , 'كرواتيا' , 'HR' , 'HRV' , 0 , 0 , 385 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Cuba' , 'كوبا' , 'CU' , 'CUB' , 0 , 0 , 53 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Cyprus' , 'قبرص' , 'CY' , 'CYP' , 0 , 0 , 357 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Czech Republic' , 'الجمهورية التشيكية' , 'CZ' , 'CZE' , 0 , 0 , 420 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Denmark' , 'الدنمارك' , 'DK' , 'DNK' , 0 , 0 , 45 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Djibouti' , 'جيبوتي' , 'DJ' , 'DJI' , 0 , 0 , 253 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Dominica' , 'دومينيكا' , 'DM' , 'DMA' , 0 , 0 , 1767 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Dominican Republic' , 'جمهورية الدومنيكان' , 'DO' , 'DOM' , 0 , 0 , 1809 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Ecuador' , 'الاكوادور' , 'EC' , 'ECU' , 0 , 0 , 593 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Egypt' , 'مصر' , 'EG' , 'EGY' , 0 , 0 , 20 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'El Salvador' , 'السلفادور' , 'SV' , 'SLV' , 0 , 0 , 503 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Equatorial Guinea' , 'غينيا الإستوائية' , 'GQ' , 'GNQ' , 0 , 0 , 240 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Eritrea' , 'إريتريا' , 'ER' , 'ERI' , 0 , 0 , 291 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Estonia' , 'إستونيا' , 'EE' , 'EST' , 0 , 0 , 372 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Ethiopia' , 'أثيوبيا' , 'ET' , 'ETH' , 0 , 0 , 251 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Faeroe Islands' , 'جزر فايرو' , 'FO' , 'FRO' , 0 , 0 , 298 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Falkland Islands' , 'جزر فوكلاند' , 'FK' , 'FLK' , 0 , 0 , 500 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Fiji' , 'فيجي' , 'FJ' , 'FJI' , 0 , 0 , 679 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Finland' , 'فنلندا' , 'FI' , 'FIN' , 0 , 0 , 358 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'France' , 'فرنسا' , 'FR' , 'FRA' , 0 , 0 , 33 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'French Guiana' , 'غيانا الفرنسية' , 'GF' , 'GUF' , 0 , 0 , 594 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'French Polynesia' , 'بولينيزيا الفرنسية' , 'PF' , 'PYF' , 0 , 0 , 689 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Gabon' , 'الجابون' , 'GA' , 'GAB' , 0 , 0 , 241 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Gambia' , 'غامبيا' , 'GM' , 'GMB' , 0 , 0 , 220 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Georgia' , 'جورجيا' , 'GE' , 'GEO' , 0 , 0 , 995 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Germany' , 'ألمانيا' , 'DE' , 'DEU' , 0 , 0 , 49 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Ghana' , 'غانا' , 'GH' , 'GHA' , 0 , 0 , 233 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Gibraltar' , 'جبل طارق' , 'GI' , 'GIB' , 0 , 0 , 350 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Greece' , 'اليونان' , 'GR' , 'GRC' , 0 , 0 , 30 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Greenland' , 'الأرض الخضراء' , 'GL' , 'GRL' , 0 , 0 , 299 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Grenada' , 'غرينادا' , 'GD' , 'GRD' , 0 , 0 , 1473 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Guadeloupe' , 'جوادلوب' , 'GP' , 'GLP' , 0 , 0 , 590 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Guam' , 'غوام' , 'GU' , 'GUM' , 0 , 0 , 1671 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Guatemala' , 'غواتيمالا' , 'GT' , 'GTM' , 0 , 0 , 502 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Guinea' , 'غينيا' , 'GN' , 'GIN' , 0 , 0 , 224 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Guinea-Bissau' , 'غينيا بيساو' , 'GW' , 'GNB' , 0 , 0 , 245 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Guyana' , 'غيانا' , 'GY' , 'GUY' , 0 , 0 , 592 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Haiti' , 'هايتي' , 'HT' , 'HTI' , 0 , 0 , 509 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Honduras' , 'هندوراس' , 'HN' , 'HND' , 0 , 0 , 504 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Hong Kong' , 'هونج كونج' , 'HK' , 'HKG' , 0 , 0 , 852 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Hungary' , 'هنغاريا' , 'HU' , 'HUN' , 0 , 0 , 36 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Iceland' , 'أيسلندا' , 'IS' , 'ISL' , 0 , 0 , 354 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'India' , 'الهند' , 'IN' , 'IND' , 0 , 0 , 91 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Indonesia' , 'إندونيسيا' , 'ID' , 'IDN' , 0 , 0 , 62 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Iran' , 'إيران' , 'IR' , 'IRN' , 0 , 0 , 964 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Iraq' , 'العراق' , 'IQ' , 'IRQ' , 0 , 0 , 98 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Ireland' , 'أيرلندا' , 'IE' , 'IRL' , 0 , 0 , 353 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Israel' , 'إسرائيل' , 'IL' , 'ISR' , 0 , 0 , 972 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Italy' , 'إيطاليا' , 'IT' , 'ITA' , 0 , 0 , 39 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Jamaica' , 'جامايكا' , 'JM' , 'JAM' , 0 , 0 , 1876 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Japan' , 'اليابان' , 'JP' , 'JPN' , 0 , 0 , 81 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Jordan' , 'الأردن' , 'JO' , 'JOR' , 0 , 0 , 962 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Kazakhstan' , 'كازاخستان' , 'KZ' , 'KAZ' , 0 , 0 , 7 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Kenya' , 'كينيا' , 'KE' , 'KEN' , 0 , 0 , 254 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Kiribati' , 'كيريباتي' , 'KI' , 'KIR' , 0 , 0 , 686 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Kuwait' , 'الكويت' , 'KW' , 'KWT' , 0 , 0 , 965 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Kyrgyzstan' , 'قيرغيزستان' , 'KG' , 'KGZ' , 0 , 0 , 996 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Laos' , 'لاوس' , 'LA' , 'LAO' , 0 , 0 , 856 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Latvia' , 'لاتفيا' , 'LV' , 'LVA' , 0 , 0 , 371 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Lebanon' , 'لبنان' , 'LB' , 'LBN' , 0 , 0 , 961 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Lesotho' , 'ليسوتو' , 'LS' , 'LSO' , 0 , 0 , 266 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Liberia' , 'ليبيريا' , 'LR' , 'LBR' , 0 , 0 , 231 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Libya' , 'ليبيا' , 'LY' , 'LBY' , 0 , 0 , 218 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Liechtenstein' , 'ليختنشتاين' , 'LI' , 'LIE' , 0 , 0 , 423 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Lithuania' , 'ليتوانيا' , 'LT' , 'LTU' , 0 , 0 , 370 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Luxembourg' , 'لوكسمبورغ' , 'LU' , 'LUX' , 0 , 0 , 352 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Macao' , 'ماكاو' , 'MO' , 'MAC' , 0 , 0 , 853 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Macedonia' , 'مقدونيا' , 'MK' , 'MKD' , 0 , 0 , null , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Madagascar' , 'مدغشقر' , 'MG' , 'MDG' , 0 , 0 , 261 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Malawi' , 'ملاوي' , 'MW' , 'MWI' , 0 , 0 , 265 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Malaysia' , 'ماليزيا' , 'MY' , 'MYS' , 0 , 0 , 60 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Maldives' , 'جزر المالديف' , 'MV' , 'MDV' , 0 , 0 , 960 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Mali' , 'مالي' , 'ML' , 'MLI' , 0 , 0 , 223 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Malta' , 'مالطا' , 'MT' , 'MLT' , 0 , 0 , 356 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Marshall Islands' , 'جزر مارشال' , 'MH' , 'MHL' , 0 , 0 , 692 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Martinique' , 'مارتينيك' , 'MQ' , 'MTQ' , 0 , 0 , 596 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Mauritania' , 'موريتانيا' , 'MR' , 'MRT' , 0 , 0 , 222 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Mauritius' , 'موريشيوس' , 'MU' , 'MUS' , 0 , 0 , 230 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Mayotte' , 'مايوت' , 'YT' , 'MYT' , 0 , 0 , 262 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Mexico' , 'المكسيك' , 'MX' , 'MEX' , 0 , 0 , 52 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Micronesia' , 'ميكرونيزيا' , 'FM' , 'FSM' , 0 , 0 , 691 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Moldova' , 'مولدوفا' , 'MD' , 'MDA' , 0 , 0 , 373 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Monaco' , 'موناكو' , 'MC' , 'MCO' , 0 , 0 , 377 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Mongolia' , 'منغوليا' , 'MN' , 'MNG' , 0 , 0 , 976 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Montenegro' , 'الجبل الأسود' , 'ME' , 'MNE' , 0 , 0 , 382 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Montserrat' , 'مونتسيرات' , 'MS' , 'MSR' , 0 , 0 , 1664 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Morocco' , 'المغرب' , 'MA' , 'MAR' , 0 , 0 , 212 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Mozambique' , 'موزمبيق' , 'MZ' , 'MOZ' , 0 , 0 , 258 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Myanmar' , 'ميانمار' , 'MM' , 'MMR' , 0 , 0 , 95 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Namibia' , 'ناميبيا' , 'NA' , 'NAM' , 0 , 0 , 264 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Nauru' , 'ناورو' , 'NR' , 'NRU' , 0 , 0 , 674 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Nepal' , 'نيبال' , 'NP' , 'NPL' , 0 , 0 , 977 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Netherlands' , 'هولندا' , 'NL' , 'NLD' , 0 , 0 , 31 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Netherlands Antilles' , 'جزر الأنتيل الهولندية' , 'AN' , 'ANT' , 0 , 0 , null , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'New Caledonia' , 'كاليدونيا الجديدة' , 'NC' , 'NCL' , 0 , 0 , 687 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'New Zealand' , 'نيوزيلاندا' , 'NZ' , 'NZL' , 0 , 0 , 64 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Nicaragua' , 'نيكاراغوا' , 'NI' , 'NIC' , 0 , 0 , 505 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Niger' , 'النيجر' , 'NE' , 'NER' , 0 , 0 , 227 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Nigeria' , 'نيجيريا' , 'NG' , 'NGA' , 0 , 0 , 234 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Niue' , 'نيوي' , 'NU' , 'NIU' , 0 , 0 , 683 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Norfolk Island' , 'جزيرة نورفولك' , 'NF' , 'NFK' , 0 , 0 , 6723 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'North Korea' , 'كوريا الشمالية' , 'KP' , 'PRK' , 0 , 0 , 850 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Northern Mariana Islands' , 'جزر مريانا الشمالية' , 'MP' , 'MNP' , 0 , 0 , 1670 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Norway' , 'النرويج' , 'NO' , 'NOR' , 0 , 0 , 47 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Oman' , 'سلطنة عمان' , 'OM' , 'OMN' , 0 , 0 , 968 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Pakistan' , 'باكستان' , 'PK' , 'PAK' , 0 , 0 , 92 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Palau' , 'بالاو' , 'PW' , 'PLW' , 0 , 0 , 680 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Palestine' , 'فلسطين' , 'PS' , 'PSE' , 0 , 0 , 970 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Panama' , 'بنما' , 'PA' , 'PAN' , 0 , 0 , 507 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Papua New Guinea' , 'بابوا غينيا الجديدة' , 'PG' , 'PNG' , 0 , 0 , 675 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Paraguay' , 'باراغواي' , 'PY' , 'PRY' , 0 , 0 , 595 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Peru' , 'بيرو' , 'PE' , 'PER' , 0 , 0 , 51 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Philippines' , 'فيلبيني' , 'PH' , 'PHL' , 0 , 0 , 63 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Poland' , 'بولندا' , 'PL' , 'POL' , 0 , 0 , 48 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Portugal' , 'البرتغال' , 'PT' , 'PRT' , 0 , 0 , 351 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Puerto Rico' , 'بورتوريكو' , 'PR' , 'PRI' , 0 , 0 , 1787 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Qatar' , 'دولة قطر' , 'QA' , 'QAT' , 0 , 0 , 974 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Reunion' , 'جمع شمل' , 'RE' , 'REU' , 0 , 0 , 262 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Romania' , 'رومانيا' , 'RO' , 'ROU' , 0 , 0 , 40 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Russian Federation' , 'الاتحاد الروسي' , 'RU' , 'RUS' , 0 , 0 , 7 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Rwanda' , 'رواندا' , 'RW' , 'RWA' , 0 , 0 , 250 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Saint Helena' , 'سانت هيلانة' , 'SH' , 'SHN' , 0 , 0 , null , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Saint Kitts and Nevis' , 'سانت كيتس ونيفيس' , 'KN' , 'KNA' , 0 , 0 , 1869 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Saint Lucia' , 'القديسة لوسيا' , 'LC' , 'LCA' , 0 , 0 , 1758 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Saint Pierre and Miquelon' , 'سانت بيير وميكلون' , 'PM' , 'SPM' , 0 , 0 , 508 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Saint Vincent and the Grenadines' , 'سانت فنسنت وجزر غرينادين' , 'VC' , 'VCT' , 0 , 0 , 1784 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Saint-Barthelemy' , 'سانت بارتيليمي' , 'BL' , 'BLM' , 0 , 0 , 590 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Saint-Martin' , 'القديس مارتن' , 'MF' , 'MAF' , 0 , 0 , 590 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Samoa' , 'ساموا' , 'WS' , 'WSM' , 0 , 0 , 685 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'San Marino' , 'سان مارينو' , 'SM' , 'SMR' , 0 , 0 , 239 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Sao Tome and Principe' , 'ساو تومي وبرينسيبي' , 'ST' , 'STP' , 0 , 0 , 966 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Saudi Arabia' , 'المملكة العربية السعودية' , 'SA' , 'SAU' , 0 , 0 , 221 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Senegal' , 'السنغال' , 'SN' , 'SEN' , 0 , 0 , 381 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Serbia' , 'صربيا' , 'RS' , 'SRB' , 0 , 0 , 248 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Seychelles' , 'سيشيل' , 'SC' , 'SYC' , 0 , 0 , 232 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Sierra Leone' , 'سيرا ليون' , 'SL' , 'SLE' , 0 , 0 , 1721 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Singapore' , 'سنغافورة' , 'SG' , 'SGP' , 0 , 0 , 65 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Slovakia' , 'سلوفاكيا' , 'SK' , 'SVK' , 0 , 0 , 421 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Slovenia' , 'سلوفينيا' , 'SI' , 'SVN' , 0 , 0 , 386 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Solomon Islands' , 'جزر سليمان' , 'SB' , 'SLB' , 0 , 0 , 677 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Somalia' , 'الصومال' , 'SO' , 'SOM' , 0 , 0 , 252 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'South Africa' , 'جنوب أفريقيا' , 'ZA' , 'ZAF' , 0 , 0 , 27 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'South Korea' , 'كوريا الجنوبية' , 'KR' , 'KOR' , 0 , 0 , 82 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'South Sudan' , 'جنوب السودان' , 'SS' , 'SSD' , 0 , 0 , 211 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Spain' , 'إسبانيا' , 'ES' , 'ESP' , 0 , 0 , 34 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Sri Lanka' , 'سيريلانكا' , 'LK' , 'LKA' , 0 , 0 , 94 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Sudan' , 'السودان' , 'SD' , 'SDN' , 0 , 0 , 249 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Suriname' , 'سورينام' , 'SR' , 'SUR' , 0 , 0 , 597 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Svalbard and Jan Mayen Islands' , 'جزر سفالبارد وجان ماين' , 'SJ' , 'SJM' , 0 , 0 , null , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Swaziland' , 'سوازيلاند' , 'SZ' , 'SWZ' , 0 , 0 , 268 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Sweden' , 'السويد' , 'SE' , 'SWE' , 0 , 0 , 46 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Switzerland' , 'سويسرا' , 'CH' , 'CHE' , 0 , 0 , 41 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Syrian Arab Republic' , 'الجمهورية العربية السورية' , 'SY' , 'SYR' , 0 , 0 , 963 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Taiwan' , 'تايوان' , 'null' , 'null' , 0 , 0 , 886 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Tajikistan' , 'طاجيكستان' , 'TJ' , 'TJK' , 0 , 0 , 992 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Tanzania' , 'تنزانيا' , 'TZ' , 'TZA' , 0 , 0 , 255 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Thailand' , 'تايلاند' , 'TH' , 'THA' , 0 , 0 , 66 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Togo' , 'توجو' , 'TG' , 'TGO' , 0 , 0 , 228 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Tokelau' , 'توكيلاو' , 'TK' , 'TKL' , 0 , 0 , 690 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Tonga' , 'تونغا' , 'TO' , 'TON' , 0 , 0 , 676 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Trinidad and Tobago' , 'ترينداد وتوباغو' , 'TT' , 'TTO' , 0 , 0 , 1868 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Tunisia' , 'تونس' , 'TN' , 'TUN' , 0 , 0 , 216 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Turkey' , 'تركيا' , 'TR' , 'TUR' , 0 , 0 , 90 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Turkmenistan' , 'تركمانستان' , 'TM' , 'TKM' , 0 , 0 , 993 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Turks and Caicos Islands' , 'جزر تركس وكايكوس' , 'TC' , 'TCA' , 0 , 0 , 1649 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Tuvalu' , 'توفالو' , 'TV' , 'TUV' , 0 , 0 , 688 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'U.S. Virgin Islands' , 'جزر فيرجن الأمريكية' , 'VI' , 'VIR' , 0 , 0 , 1340 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Uganda' , 'أوغندا' , 'UG' , 'UGA' , 0 , 0 , 256 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Ukraine' , 'أوكرانيا' , 'UA' , 'UKR' , 0 , 0 , 380 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'United Arab Emirates' , 'الإمارات العربية المتحدة' , 'AE' , 'ARE' , 0 , 0 , 971 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'United Kingdom' , 'المملكة المتحدة' , 'GB' , 'GBR' , 0 , 0 , 44 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'United States' , 'الولايات المتحدة' , 'US' , 'USA' , 0 , 0 , 1 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Uruguay' , 'أوروغواي' , 'UY' , 'URY' , 0 , 0 , 598 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Uzbekistan' , 'أوزبكستان' , 'UZ' , 'UZB' , 0 , 0 , 998 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Vanuatu' , 'فانواتو' , 'VU' , 'VUT' , 0 , 0 , 678 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Venezuela' , 'فنزويلا' , 'VE' , 'VEN' , 0 , 0 , 58 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Viet Nam' , 'فييت نام' , 'VN' , 'VNM' , 0 , 0 , 84 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Wallis and Futuna Islands' , 'جزر واليس وفوتونا' , 'WF' , 'WLF' , 0 , 0 , 681 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Yemen' , 'اليمن' , 'YE' , 'YEM' , 0 , 0 , 967 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Zambia' , 'زامبيا' , 'ZM' , 'ZMB' , 0 , 0 , 260 , '" + DateTime.Now + "', null);");
            ExecQuery("INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('" + Guid.NewGuid() + "', 'Zimbabwe' , 'زمبابوي' , 'ZW' , 'ZWE' , 0 , 0 , 263 , '" + DateTime.Now + "', null);");

        }

        public static void DropAllDataFromDatabases()
        {
            ExecQuery("declare @command varchar(15)  set @command = 'drop table ?'  exec sp_msforeachtable @command");
            RebuildDatabase();
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
                catch (Exception)
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
            //"INSERT INTO Countries(ID, Name, ar_name, ISO, ISO3, is_market, is_factory,  Phone_key, created_at, updated_at) VALUES ('4a862ebc-5525-402d-aec2-af9d62c2b0e5', 'Afghanistan' , 'أفغانستان' , 'AF' , 'AFG' , 0 , 0 , 93 , '21/02/2021 09:01:54 م', null);"
        }
        public static void ExecQuery(String str)
        {
            SqlConnection cn = new SqlConnection(Database.ConnectionString);
            SqlCommand cmd = new SqlCommand(str, cn);
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

    }
}