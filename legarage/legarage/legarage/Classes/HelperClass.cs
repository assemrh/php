using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace legarage.Classes
{
    public class HelperClass
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool NotNull(object value)
        {
            return value != null && value != DBNull.Value;
        }

        public static bool Phone_Number_Sanituzer(ref string value)
        {//TODO
            if (value.Length < 9) return false;
            value = value.Replace(" ", "").Replace("-", "");
            value = "+971" + value.Substring(value.Length - 9, 9);

            return value.Length == 13;
        }
        public static bool Phone(string value)
        {
            return Regex.Match(value, @"\d{10}").Success || Regex.Match(value, @"\d{9}").Success;
        }
        //public static bool Phone(string Phone)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(Phone))
        //            return false;
        //        var r = new Regex(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$");
        //        return r.IsMatch(Phone);

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public static bool Email(string emailaddress )
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
        }

        public static string[,] FuleTypes_ = new string[,]
         {
            {"Dezel","مازوت"},
            {"Gaz" ,"غاز"},
            {"Electric","كهرباء" },
            {"Benzene","بانزين" },
            {"Dezel and Gaz","مازوت و غاز" },
            {"Dezel and Benzene","مازوت و بانزين" },
            {"Gaz and Benzene","غاز و بانزين" },
         };
    }
}