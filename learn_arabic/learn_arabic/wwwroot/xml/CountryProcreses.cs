using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xml.Classes.CountryProcreses.all
{
    class CountryProcreses
    {

        public static string XmlFilePath { get; set; } = @"C:\Repos\xml\xml\XmlData\";

        /// <summary>
        /// مسار المجلد الرئيسي للمشروع
        ///  المسار المؤدي الى المجلد الحاوي لملفات اكس ام ال
        /// </summary>
         static string _xmlFilePath   // property
        {
            get => XmlFilePath;   // get method
            set
            {
                if (value.EndsWith('\\'))
                    XmlFilePath = value;
                else
                {
                    XmlFilePath = value + '\\';
                }
            }
        }

        /// <summary>
        /// 
        /// 
        /// Fill data into country and translation tables to the database
        ///     : CountryProcreses.Initialization (new string[] { "Ar", "Tr", "En", "Ru" });
        /// 
        /// </summary>
        /// <param name="langs">Array of languages string : new string[] { "Ar", "Tr", "En", "Ru" }</param>
        /// 
        /// <example>
        ///     destination:
        ///     CountryProcreses.Initialization 
        /// </example>
        /// 
        /// <returns></returns>
        public static void Initialization(String[] langs)
        {
            DropAllTables();
            AddToDAtabase();
            foreach(string lang in langs)
                AddNewCountryTranslations(lang);
        }

        private static void AddToDAtabase()
        {
            List<CountryModel> countries = GetCountryListFromXmlFile();
            foreach (var country in countries)
            {
                List<string> cols = new List<string>() { "code", "ISO", "logo", "created_at" };
                List<object> vals = new List<object>() { country.Code, country.ISO, country.Flag, DateTime.Now };
                Guid guid = new Guid(country.Id);
                Database.InsertRow("countries", guid, cols, vals, out string str);
            }
        }
        /// <summary>
        /// Drop all data from Country and translation tables to database
        /// </summary>
        private static void DropAllTables()
        {
            ExecQuery(@"DELETE FROM [dbo].[country_translations] WHERE 1=1");
            ExecQuery(@"DELETE FROM [dbo].[countries] WHERE 1=1");
        }
        private static List<CountryModel> GetCountryListFromXmlFile()
        {
            string inputUri = XmlFilePath + "CoutriesList.xml";
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;


            XmlReader Countries = XmlReader.Create(inputUri, settings);
            List<CountryModel> countries = new List<CountryModel>();
            while (Countries.Read())
            {
                if ((Countries.NodeType == XmlNodeType.Element) && (Countries.Name == "Country"))
                {
                    if (Countries.HasAttributes)
                    {
                        CountryModel country = new CountryModel();
                        countries.Add(new CountryModel()
                        {
                            Id = Countries.GetAttribute("Id"),
                            Name = Countries.GetAttribute("Name"),
                            Code = Countries.GetAttribute("Code"),
                            Flag = Countries.GetAttribute("Flag"),
                            ISO = Countries.GetAttribute("ISO")
                        });
                    }
                }
            }
            return countries;
        }

        private static void AddNewCountryTranslations(string lang)
        {
            List<Country_translations> translations = GetTranslationsListFromXmlFile(lang);
            foreach (var translation in translations)
            {
                List<string> cols = new List<string>() { "src_id", "language", "value", "created_at" };
                List<object> vals = new List<object>() { translation.src_id, translation.language, translation.value, DateTime.Now };
                Database.InsertRow("country_translations", Guid.NewGuid(), cols, vals, out string str);
            }
        }

        private static List<Country_translations> GetTranslationsListFromXmlFile(string lang)
        {
            string inputUri = XmlFilePath + "CoutriesList." + lang + ".xml";
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;


            XmlReader Countries = XmlReader.Create(inputUri, settings);
            List<Country_translations> translations = new List<Country_translations>();
            while (Countries.Read())
            {
                if ((Countries.NodeType == XmlNodeType.Element) && (Countries.Name == "country_translation"))
                {
                    if (Countries.HasAttributes)
                    {
                        //Console.WriteLine(Countries.GetAttribute("Name") + ": " + Countries.GetAttribute("Id"));
                        translations.Add(new Country_translations()
                        {
                            src_id = Countries.GetAttribute("src_id"),
                            language = Countries.GetAttribute("language"),
                            value = Countries.GetAttribute("value")
                        });
                    }
                }
            }
            return translations;
        }
        private class CountryModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string ISO { get; set; }
            public string Flag { get; set; }
            public string Code { get; set; }
        }
        private class Country_translations
        {
            public Guid ID { get; set; }
            public string src_id { get; set; }
            public string language { get; set; }
            public string value { get; set; }
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
