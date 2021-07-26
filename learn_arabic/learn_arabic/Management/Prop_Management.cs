using learn_arabic.Classes;
using learn_arabic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Management
{
    public class Prop_Management
    {
        /// <title>
        /// /Properties indexs
        /// </title>
        /// <values>
        ///  1 => company video
        /// </values>
        public async static Task<bool> Add(Attachment video, ER_Ref<string> msg)
        {
            //// TODO: add vedio
            if (video != null && video.Base64 != null)
            {
                var bytes = Convert.FromBase64String(video.Base64);
              return  await Storage.Add_Company_Video("/video/", video.File_Name, "properties/video", "Video",  bytes, msg);
            }
            msg.Error = "enter valied video!";
            return false;
        }


       
    }
}
