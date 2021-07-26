using learn_arabic.Classes;
using learn_arabic.Management;
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
    public class Helpful_linkController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(string lang)
        {
            if (lang == null) lang = "AR";
            ER_Ref<string> msg = new ER_Ref<string>();
            var data = await Helpful_Links_Management.Get_Helpful_links(lang, msg);
            if (msg.Error == "")
            {
                return NotFound(msg);
            }
            return Ok(data);
        }
    }
}
