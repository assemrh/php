using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace learn_arabic.Classes
{
    class CountryProcreses
    {

        public static  string XmlFilePath { get; set; } = Storage.rootPath + @"\xml\XmlData\";

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
        /// <param name="XmlFilePath">the path of proje</param>
        /// 
        /// <example>
        ///     destination:
        ///     CountryProcreses.Initialization 
        /// </example>
        /// 
        /// <returns></returns>
        public static async void Initialization(String[] langs, string XmlFilePath)
        {
            _xmlFilePath = XmlFilePath;
            DropAllTables();
            AddToDAtabase();
            foreach(string lang in langs)
                AddNewCountryTranslations(lang);
        }
        public static async void Initialization(String[] langs )
        {
            DropAllTables();
            AddToDAtabase();
            foreach (string lang in langs)
                AddNewCountryTranslations(lang);
        }

        private static async void AddToDAtabase()
        {
            List<CountryModel> countries =await GetCountryListFromXmlFile();
            foreach (var country in countries)
            {
                ER_Ref<string> msg = new ER_Ref<string>();
                List<string> cols = new List<string>() { "code", "ISO", "logo", "created_at" };
                List<object> vals = new List<object>() { country.Code, country.ISO, country.Flag, DateTime.Now };
                Guid guid = new Guid(country.Id);
               await Database.InsertRow("countries", guid, cols, vals, msg);
            }
        }
        /// <summary>
        /// Drop all data from Country and translation tables to database
        /// </summary>
        private static async void DropAllTables()
        {
            ExecQuery(@"DELETE FROM [dbo].[country_translations] WHERE 1=1");
            ExecQuery(@"DELETE FROM [dbo].[countries] WHERE 1=1");
        }
        private static async Task<List<CountryModel>> GetCountryListFromXmlFile()
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

        private static async void AddNewCountryTranslations(string lang)
        {
            List<Country_translations> translations =await GetTranslationsListFromXmlFile(lang);
            foreach (var translation in translations)
            {
                ER_Ref<string> msg = new ER_Ref<string>();
                List<string> cols = new List<string>() { "src_id", "language", "value", "created_at" };
                List<object> vals = new List<object>() { translation.src_id, translation.language, translation.value, DateTime.Now };
              await  Database.InsertRow("country_translations", Guid.NewGuid(), cols, vals, msg);
            }
        }

        private static async Task<List<Country_translations>> GetTranslationsListFromXmlFile(string lang)
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

        public static async void ExecQuery(String str)
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
