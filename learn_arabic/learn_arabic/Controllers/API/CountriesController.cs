using learn_arabic.Classes;
using learn_arabic.Management;
using learn_arabic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        
       
        [HttpGet]
        public async Task<IActionResult> Get(string lang)
        {
            if (lang == null) lang = "AR";
            ER_Ref<string> msg = new ER_Ref<string>();
            var data =await Countries_Managment.Get_Countries(lang,msg);
            if(msg.Error == "") 
            {
                return NotFound(msg);
            }
            return Ok(data);
        }

        public IActionResult test()
        {
            return Content("test");
        }
    }
}
