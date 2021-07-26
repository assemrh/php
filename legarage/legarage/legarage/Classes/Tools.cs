using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace legarage.Classes
{
    public class Tools
    {
        public static bool SaveCurrentAttachment(string FolderName, Guid ID, out string FileName)
        {
            FileName = "";
            if (HttpContext.Current.Session["Attachment"] != null)
            {
                try
                {
                    FolderName = HttpContext.Current.Server.MapPath("~/Content/" + FolderName).Trim();
                    if (!FolderName.EndsWith("\\")) FolderName += "\\";
                    if (!System.IO.Directory.Exists(FolderName)) System.IO.Directory.CreateDirectory(FolderName);
                    FileName = ID.ToString() + System.IO.Path.GetExtension(HttpContext.Current.Session["Attachment_File_Name"].ToString().Trim());
                    byte[] b = (byte[])HttpContext.Current.Session["Attachment"];
                    System.IO.File.WriteAllBytes(FolderName + FileName, b);
                    HttpContext.Current.Session["Attachment_File_Name"] = null;
                    HttpContext.Current.Session["Attachment"] = null;
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void SaveError(string err)
        {
            string FileName = HttpContext.Current.Server.MapPath("~/Log/");
            if (!System.IO.Directory.Exists(FileName))
            {
                System.IO.Directory.CreateDirectory(FileName);
            }
            if (!FileName.EndsWith("\\")) FileName += "\\";
            FileName += "ErrorLog.txt";
            System.IO.File.WriteAllText(FileName, err);
        }
        public static string EncodeBase64(string v)
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(v);
            string strBase64 = Convert.ToBase64String(b);
            return strBase64;
        }
        public static string DecodeBase64(string v)
        {
            byte[] b = Convert.FromBase64String(v);
            string result = System.Text.Encoding.UTF8.GetString(b);
            return result;
        }
        public static bool FindCurrentUser(out DataRow rUser)
        {
            if (HttpContext.Current.Session["token"] != null && HttpContext.Current.Session["token"].ToString().Trim() != string.Empty) //String.IsNullOrWhiteSpace(HttpContext.Current.Session["token"].ToString())
            {
                rUser = Database.FindRow("Users", "token", HttpContext.Current.Session["token"].ToString());
                if (rUser != null) { return true; }
                else { return false; }
            }
            else
            {
                rUser = null;
                return false;
            }
        }
        //public static string FindCurrentMarket()
        //{
        //    if (HttpContext.Current.Session["market"] != null && HttpContext.Current.Session["token"].ToString().Trim() != string.Empty) //String.IsNullOrWhiteSpace(HttpContext.Current.Session["token"].ToString())
        //    {
        //        rUser = Database.FindRow("Users", "token", HttpContext.Current.Session["token"].ToString());
        //        if (rUser != null) { return true; }
        //        else { return false; }
        //    }
        //    else
        //    {
        //        rUser = null;
        //        return false;
        //    }
        //}
        public static List<I> ConvertToList<I>(DataTable datatable) where I : class
        {
            List<I> lstRecord = new List<I>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                lstRecord = datatable.AsEnumerable().ToList().ConvertAll<I>(row => GetObject<I>(row, columnsNames));
                return lstRecord;
            }
            catch
            {
                return lstRecord;
            }

        }

        private static I GetObject<I>(DataRow row, List<string> columnsName) where I : class
        {
            I obj = (I)Activator.CreateInstance(typeof(I));
            try
            {
                PropertyInfo[] Properties = typeof(I).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    string columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        object dbValue = row[columnname];
                        if (dbValue != DBNull.Value)
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                objProperty.SetValue(obj, Convert.ChangeType(dbValue, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                objProperty.SetValue(obj, Convert.ChangeType(dbValue, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                return obj;
            }
        }

        public static void ErrorLog(string log = "")
        {
            string dir = HttpContext.Current.Server.MapPath("~/Log/");
            string fn = "ErrorLog"+ DateTime.Now.ToString("yy-MM-dd-hh-mm-ss") +".txt";
            string path = HttpContext.Current.Request.Url.Host;
            System.IO.File.WriteAllText(dir + fn, $"\n // {DateTime.Now} \n{log} \n{dir} \n {path}");
            //StreamWriter sw = new StreamWriter(,true);
            //var CurrentDirectory = Directory.GetCurrentDirectory();
            //sw.WriteLine();
            //sw.Close();
        }
    }
}