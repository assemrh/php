using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace learn_arabic.Controllers
{
    public class ControlPanelController : _BaseController
    {
        public ControlPanelController()
        {
            var x = HttpContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadFile(IFormFile file, string name)
        {
            var file1 = Request;
            if (file.ContentType != null)
            {
                //string type = file.ContentType.Split('/')[1];
                //var _fileName = file.FileName;
                //var _file = file.Name;
                MemoryStream target = new MemoryStream();
                file.CopyTo(target);
                byte[] data = target.ToArray();
                string strFile = Convert.ToBase64String(data);
                string newName = name +"_"+ DateTime.Now.ToString("yyyy_MM_dd")+"_" + file.FileName;
                //TODO marge Base64 with Name in same session
                HttpContext.Session.SetString(name, strFile);
                HttpContext.Session.SetString(name+ "Name", newName);
                //HttpContext.Session.Set("byteFile", data);
            }
            return Ok();
        }
    }
}
