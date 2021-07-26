using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using learn_arabic.Controllers.API;
using learn_arabic.Classes;

namespace learn_arabic.Controllers
{
    public class CPCarouselController : Controller
    {
        public IActionResult Index()
        {
            ReadCarouselsData();
            return View();
        }
        public IActionResult ReadCarousels()
        {
            ReadCarouselsData();
            return View("Table");
        }
        private void ReadCarouselsData()
        {
            string errMessage = "";
            //JArray data = ApiWebsite.ReadCarousels(out errMessage);
            //ViewBag.jData = data;
            List<string> colNames = new List<string>() { "local_name", "latin_name", "image" };
            List<string> colTitles = new List<string>() { Resources.General.LocalName, Resources.General.LatinName, Resources.General.Image } ;
            ViewBag.Cols = colNames;
            ViewBag.ColTitles = colTitles;
            ViewBag.EditAction = "/CPCarousel/CarouselEdit/ID_PARAM";
            ViewBag.SaveAction = "/CPCarousel/CarouselSave/ID_PARAM";
            ViewBag.ReadAction = "/CPCarousel/ReadCarousels";
            ViewBag.DeleteAction = "/CPCarousel/CarouselDelete/ID_PARAM";
        }

        [HttpGet]
        [Route("/CPCarousel/CarouselEdit/{Id?}")]
        public IActionResult CarouselEdit()
        {
            string Id = Request.RouteValues["Id"]?.ToString() ?? "";

            string errMessage = "";
            JObject obj = null;
            if(Id.Trim() != "")
            {
                //obj = ApiWebsite.GetCarousel(Id, out errMessage);
            }

            //ViewBag.Data = obj;
            //Storage.CleanMyTmpFolder(); 

            return View();
        }
        [HttpPost]
        [Route("/CPCarousel/CarouselSave/{Id?}")]
        public JsonResult CarouselSave()
        {
            string Id = Request.Form["id"].ToString();
            string localName = Request.Form["localname"].ToString();
            string latinName = Request.Form["latinname"].ToString();
            string localTopic = Request.Form["localtopic"].ToString();
            string latinTopic = Request.Form["latintopic"].ToString();

            string image = "";
            string image_file_name = "";

            //string[] files = Storage.GetMyTmpFiles();
            //byte[] b = null;
            //if (files != null && files.Length > 0)
            //{
            //    b = System.IO.File.ReadAllBytes(files[0]);
            //    image = Convert.ToBase64String(b);
            //    image_file_name = System.IO.Path.GetFileName(files[0]).Replace(" ", "_");
            //}

            string errMessage = "";
            int status = 0;

            JObject p = new JObject();
            p["local_name"] = localName;
            p["latin_name"] = latinName;
            p["local_topic"] = localTopic;
            p["latin_topic"] = latinTopic;
            p["image"] = image;
            p["image_file_name"] = image_file_name;

            //if (ApiWebsite.SaveCarousel(Id, p, out errMessage))
            //{
            //    Storage.CleanMyTmpFolder();
            //    status = 200;
            //}
            //else
            //{
            //    status = 400;
            //}

            return Json(new { @status=status, @data ="", @msg = errMessage });
        }
        [HttpGet]
        [Route("/CPCarousel/CarouselDelete/{Id}")]
        public JsonResult CarouselDelete()
        {
            string errMessage = "";
            string Id = Request.RouteValues["id"].ToString();
            int status =  0;
            //if(ApiWebsite.DeleteCarousel(Id, out errMessage))
            //{
            //    status = 200;
            //}
            //else
            //{
            //    status = 400;
            //}

            return Json(new { @status = status, @data = "", @msg = errMessage });
        }

    }
}
