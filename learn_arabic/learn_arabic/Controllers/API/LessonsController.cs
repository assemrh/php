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
    public class LessonsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(string lang, Guid groupe_id)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (groupe_id == new Guid())
                {
                    msg.Error = "Groupe id is Reqiuerd";
                    return NotFound(msg);
                }
                else if (lang == null) lang = "AR";

                var data = await Lessons_Management.Get_Lessons_by_groupe_id(lang, groupe_id, msg);

                if (data == null || data.Count == 0)
                {
                    msg.Error = "Data Not found!";
                    return NotFound(msg);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return StatusCode(500, msg);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Post_user_lesson(Guid user_id, [FromHeader] string Authorization, Guid lesson_id, string lang)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {

                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }

                if (user_id == new Guid())
                {
                    msg.Error = "User id is Reqiuerd";
                    return NotFound(msg);
                }

                else if (lesson_id == new Guid())
                {
                    msg.Error = "Lesson id is required";
                    return NotFound(msg);
                }

                else if (await HelperClass.IsObjectNullOrEmptyString(lang))
                {
                    msg.Error = "Lang is required";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {

                    if (await Lessons_Management.Add_user_lesson(user_id, lesson_id, lang, msg))
                    {
                        return Ok("Successfully Added");
                    }

                    else
                    {
                        return StatusCode(400, msg);
                    }
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



        [HttpGet]
        public async Task<IActionResult> Get_lesson_shape(Guid ID ,string lang, [FromHeader] string Authorization, Guid lesson_id)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }

                if (ID == new Guid())
                {
                    msg.Error = "ID is Reqiuerd";
                    return NotFound(msg);
                }

                else if (lang == null) lang = "AR";

                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    var data = await Lessons_Management.Get_Lesson_shapes(ID, lang, lesson_id, msg);

                    if (data == null || data.Shapes.Count == 0)
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
