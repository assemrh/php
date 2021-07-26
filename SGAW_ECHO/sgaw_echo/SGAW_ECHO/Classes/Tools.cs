using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SGAW_ECHO.Classes
{
    public class Tools
    {
        public static bool SaveCurrentAttachment(string FolderName, Guid ID, out string FileName) {
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
                catch (Exception ex) {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        public static void SaveError(string err) {
            string FileName = HttpContext.Current.Server.MapPath("~/Log/");
            if (!System.IO.Directory.Exists(FileName)){
                System.IO.Directory.CreateDirectory(FileName);
            }
            if (!FileName.EndsWith("\\")) FileName += "\\";
            FileName += "ErrorLog.txt";
            System.IO.File.WriteAllText(FileName, err);
        }

        public static string EncodeBase64(string v) {
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


        public static bool FindCurrentUser()
        {
            if (String.IsNullOrWhiteSpace((HttpContext.Current.Session["token"]??"").ToString())) 
            {
                //Token = HttpContext.Current.Session["token"].ToString();
                 return false;
            }
            else
            {
                //Token = null;
                return false;
            }
        }

    }
}