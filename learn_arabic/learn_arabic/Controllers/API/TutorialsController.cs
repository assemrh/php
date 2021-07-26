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
    public class TutorialsController : ControllerBase
    {

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<IActionResult> Get_tutorial_slides(string lang)
        {
            if (lang == null) lang = "AR";
            ER_Ref<string> msg = new ER_Ref<string>();
            var data = await Tutorials_Management.Get_Tutorials(lang, msg);
            if (msg.Error == "")
            {
                return NotFound(msg);
            }
            return Ok(data);
        }
    }
}
