using learn_arabic.Classes;
using learn_arabic.Management;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader] string Authorization,[FromQuery] string lang)
        {
            if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
            {
                return Unauthorized();
            }
            Ref<DataRow> user = new Ref<DataRow>();
            if (await Users_Management.GetCurrentUser(Authorization, user) == false)
                return Unauthorized();
            if (lang == null) lang = "AR";
            ER_Ref<string> msg = new ER_Ref<string>();
            var data = await Categories_Management.Get_Categories(lang, msg);
            if (data !=null )
            {
                if (data.Count > 0)
                    return Ok(data);
                else
                    msg.Error = "No Category";
            }
            return NotFound(msg);
        }
    }
}
