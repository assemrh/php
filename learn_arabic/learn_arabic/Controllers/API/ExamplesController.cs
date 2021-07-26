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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamplesController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get_Shape_example(string lang, [FromHeader] string Authorization, Guid shape_id)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }

                if (shape_id == new Guid())
                {
                    msg.Error = "Shape id is Reqiuerd";
                    return NotFound(msg);
                }

                else if (lang == null) lang = "AR";

                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    var data = await Examples_Management.Get_shap_examples(lang, shape_id, msg);

                    if (data == null || data.ExampleShape.Count == 0)
                    {
                        msg.Error = "Data Not found!";
                        return NotFound(msg);
                    }
                    return Ok(data);
                }
                else
                    return Unauthorized();
            }

            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return StatusCode(500, msg);
            }
        }

    }
}
