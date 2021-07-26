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
    public class ExamController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get_Question_Info(string lang, [FromHeader] string Authorization, Guid Question_id)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }

                if (Question_id == new Guid())
                {
                    msg.Error = "Question id is Reqiuerd";
                    return NotFound(msg);
                }

                else if (lang == null) lang = "AR";

                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    var data = await Questions_Management.Get_Question_Info(Question_id,lang, msg);

                    if (data == null )
                    {
                        msg.Error = "enter a qurrect question id!";
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


        [Route("placement")]
        [HttpGet]
        public async Task<IActionResult> Get_Available_Placement_Test([FromHeader] string Authorization)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }

                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid USID = new Guid(user.Value["id"].ToString());
                    Ref<string> Placement_Test = new Ref<string>();
                    if (await Exams_Management.Get_Available_Placement_Test(USID, Placement_Test, msg))
                        return Ok(Placement_Test);
                    return NotFound(msg);
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
