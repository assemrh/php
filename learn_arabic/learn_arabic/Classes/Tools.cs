
using Microsoft.AspNetCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;

namespace learn_arabic.Classes
{
    public class Tools
    {
        //public static bool SaveCurrentAttachment(string FolderName, Guid ID, out string FileName) {
        //    FileName = "";
        //    if (HttpContext.Current.Session["Attachment"] != null)
        //    {
        //        try
        //        {
        //            FolderName = HttpContext.Current.Server.MapPath("~/Content/" + FolderName).Trim();
        //            if (!FolderName.EndsWith("\\")) FolderName += "\\";
        //            if (!System.IO.Directory.Exists(FolderName)) System.IO.Directory.CreateDirectory(FolderName);
        //            FileName = ID.ToString() + System.IO.Path.GetExtension(HttpContext.Current.Session["Attachment_File_Name"].ToString().Trim());
        //            byte[] b = (byte[])HttpContext.Current.Session["Attachment"];
        //            System.IO.File.WriteAllBytes(FolderName + FileName, b);
        //            HttpContext.Current.Session["Attachment_File_Name"] = null;
        //            HttpContext.Current.Session["Attachment"] = null;
        //            return true;
        //        }
        //        catch (Exception ex) {
        //            return false;
        //        }
        //    }
        //    else {
        //        return false;
        //    }
        //}

        public async static void SaveError(string err, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null, [CallerFilePath] string callerFile = null)
        {

            string FileName = Directory.GetCurrentDirectory().Replace('\\', '/') + "/Log/";

            
            List<string> text = new List<string>();
            text.Add("// " + DateTime.Now.Date.ToString() + " - " +DateTime.Now.TimeOfDay);
            text.Add("in file " + Directory.GetCurrentDirectory() + callerFile); 
            text.Add(" at line " + lineNumber + " ( at function " + caller + ")");
            text.Add("Error details : ");
            text.Add(err);

            if (!System.IO.Directory.Exists(FileName))
                System.IO.Directory.CreateDirectory(FileName);
                if (!FileName.EndsWith("\\")) FileName += "\\";
                FileName += "ErrorLog.txt";
            if(!System.IO.File.Exists(FileName))
                await System.IO.File.WriteAllLinesAsync(FileName, text);
            else
                await System.IO.File.AppendAllLinesAsync(FileName, text);
        }

        //public static string EncodeBase64(string v) {
        //    byte[] b = System.Text.Encoding.UTF8.GetBytes(v);
        //    string strBase64 = Convert.ToBase64String(b);
        //    return strBase64;
        //}
        //public static string DecodeBase64(string v)
        //{ 
        //    byte[] b = Convert.FromBase64String(v);
        //    string result = System.Text.Encoding.UTF8.GetString(b);
        //    return result;
        //}


        //public static bool FindCurrentUser()
        //{
        //    if (String.IsNullOrWhiteSpace((HttpContext.Current.Session["token"]??"").ToString())) 
        //    {
        //        //Token = HttpContext.Current.Session["token"].ToString();
        //         return false;
        //    }
        //    else
        //    {
        //        //Token = null;
        //        return false;
        //    }
        //}

    }
}