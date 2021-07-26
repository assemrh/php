using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace learn_arabic.Classes
{
    public class HelperClass
    {
        private static Random random = new Random();

        public async static Task<string> RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool NotNull(object value)
        {
            return value != null && value != DBNull.Value;
        }

        public static string NotNull_S(object value)
        {
            return value != null && value != DBNull.Value ? value.ToString(): "";
        }

        public static double NotNull_D(object value)
        {
            double res = 0;
            try
            {
                res= Convert.ToDouble(value);
            }
            catch
            {
                res = 0;
            }
            return res;
        }

        public static DateTime NotNull_Date(object value)
        {
            DateTime res = new DateTime();
            try
            {
                res = Convert.ToDateTime(value);
            }
            catch
            {
                res = new DateTime();
            }
            return res;
        }
        public static bool Phone_Number_Sanituzer(ref string value)
        {
            //return Regex.Match(value, @"^(\+[0-9]{9})$").Success;
            //if (value.Length < 9) return false;
            //value = value.Replace(" ", "").Replace("-", "");
            //value = "+966" + value.Substring(value.Length - 9, 9);
            //return value.Length == 13;
            return true;
        }

        public static bool IsToday(DateTime date)
        {
            DateTime now = DateTime.Now;
            if (date.Year < now.Year)
                return false;
            else if (date.Month < now.Month)
                return false;
            else if (date.Day < now.Day)
                return false;
            return true;
        }
        public static double Calc_Price(object price,object discount)
        {
            try
            {
                double price_ = Convert.ToDouble(price);
                double discount_ = Convert.ToDouble(discount);
                return price_ - (price_ * discount_ / 100);
            }
            catch
            {
                try
                {
                    double price_ = Convert.ToDouble(price);
                    return price_;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static double Convert_Currency (double amount ,double rate)
        {
            //double rate = Convert.ToDouble(Database.ReadValue("Currencies", "Rate", Currency_ID));
            return amount * rate;
        }

        public static bool IsValidGuid(dynamic Id)
        {
            if (Id == null) return false;
            if (!Guid.TryParse(Id.ToString(), out Guid guid) ) return false;
            if (guid == Guid.Empty) return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">object contains string value</param>
        /// <returns>
        ///     true if object is null or contain Empty string or WhiteSpace ,
        ///      true if the value parameter is null or System.String.Empty, or if value consists
        ///       exclusively of white-space characters.
        ///      false any thing else
        /// </returns>
        public async static Task<bool> IsObjectNullOrEmptyString(object obj)
        {
            if (obj == null) return true;
            obj = Convert.ToString(obj);
            if (obj.GetType() == typeof(string)) 
            {
               try
               {
                   if (string.IsNullOrEmpty(obj.ToString())) return true;
                   if (string.IsNullOrWhiteSpace(obj.ToString())) return true;
                   return false;
               }
               catch
               {
                   return true;
               }
            }
            else
            return true;
        }
        
        public static string JsStringEncode(string str)
        {
            return System.Web.HttpUtility.JavaScriptStringEncode(str);
        } 
    }
}