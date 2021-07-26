using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Classes
{
    public class Storage
    {
        public static string rootPath;

        public static void CleanMyTmpFolder(DataRow UserAccount)
        {
            string folderName = rootPath + "Tmp\\Files\\" + UserAccount["Id"].ToString() + "\\";
            if (!System.IO.Directory.Exists(folderName)) System.IO.Directory.CreateDirectory(folderName);
            string[] files = System.IO.Directory.GetFiles(folderName);
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
        }

        public async static Task<bool> SaveAttachment(string path, string filename, string attachableName,string type, Guid src_id, byte[] filebytes,ER_Ref<string> msg,int ismain,int rowindex)
        {
            path = path.Trim();
            path = rootPath + path;
           // if (path == string.Empty) path = rootPath;
            //if (path.EndsWith("\\")) path = path + "\\";
           // path += "Storage\\";
            if (attachableName.Trim() != "") path += attachableName + "/";
            if (src_id != Guid.Empty) path += src_id.ToString() + "/";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            try
            {
                System.IO.File.WriteAllBytes(path + filename, filebytes);

                if (attachableName.Trim() != "")
                {
                    List<string> cols = new string[] { "URL", "Is_Main", "Row_Index", "Src_ID", "Src_Type", "type" , "created_at" }.ToList();
                    List<object> vals = new object[] { filename, ismain, rowindex,src_id, attachableName, type,DateTime.Now.ToShortDateString()}.ToList();
                    Guid DID = Guid.NewGuid();
                   DataRow temp = await Database.GetRow("attachments", DID);
                    while (temp != null)
                    {
                        DID = Guid.NewGuid();
                        temp = await Database.GetRow("attachments", DID);
                    }
                    await Database.InsertRow("attachments",DID, cols, vals, msg);
                }

                return true;
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return false;
            }
        }
        public async static Task<bool> RemoveAttachment(Guid Id, string path, string filename, string attachableName, Guid attachableId,ER_Ref<string> msg)
        {
            msg.Error = "";
            path = path.Trim();
            if (path == string.Empty) path = rootPath;
            if (path.EndsWith("\\")) path = path + "\\";
            path += "Storage\\";
            if (attachableName.Trim() != "") path += attachableName + "\\";
            if (attachableId != Guid.Empty) path += attachableId.ToString() + "\\";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            try
            {
                System.IO.File.Delete(path + filename);

                if (Id != Guid.Empty)
                {
                  await  Database.DeleteRow("Attachments", Id, msg);
                }

                return true;
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return false;
            }
        }
        //public static byte[] GetAttachment(Guid Id, out string errMessage)
        //{
        //    errMessage = "";
        //    DataRow rAttachment = Database.FindRow("Attachments", "Id", Id, out errMessage);
        //    if (rAttachment == null) return null;

        //    string path = rootPath;
        //    path += "Storage\\" + rAttachment["Attachable_Name"].ToString() + "\\" + rAttachment["Attachable_Id"].ToString() + "\\";
        //    if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
        //    try
        //    {
        //        byte[] b = System.IO.File.ReadAllBytes(path + rAttachment["File_Name"].ToString());

        //        return b;
        //    }
        //    catch (Exception ex)
        //    {
        //        errMessage = ex.Message;
        //        return null;
        //    }
        //}

        public async static Task<bool> Add_Company_Video(string path, string filename, string attachableName, string type,  byte[] filebytes, ER_Ref<string> msg)
        {
            path = path.Trim();
            path = rootPath + path;
            if (attachableName.Trim() != "") path += attachableName + "/";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            try
            {
                System.IO.File.WriteAllBytes(path + filename, filebytes);
                return attachableName.Trim() != "" && await Database.WriteProp(1, filename, msg);
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return false;
            }
        }

    }
}
