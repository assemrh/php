using learn_arabic.Classes;
using learn_arabic.Management;
using learn_arabic.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Controllers.API
{
    //[Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public SettingsController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [Route("api/Settings/video/add")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Attachment video, [FromHeader] string Authorization)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(video.File_Name))
                {
                    msg.Error = "file name is required!!";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user) && user.Value["is_admin"].ToString() == "1")
                {
                    Storage.rootPath = _environment.WebRootPath;
                    if (!await Prop_Management.Add(video, msg))
                    {
                        return NotFound(msg);
                    }

                    return Ok("Added");
                }
                else
                {
                    if (await HelperClass.IsObjectNullOrEmptyString(msg.Error)) msg.Error = "you must to be admin";
                    return NotFound(msg);
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return StatusCode(500, msg);
            }
        }


        [Route("api/Settings/video/get")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int index, [FromHeader] string Authorization)
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
                    string url = await Database.ReadProp(1);
                    if (url == "")
                    {
                        return NotFound(msg);
                    }
                    Ref<string> v = new Ref<string>();
                    v.Value = url;
                    return Ok(v);
                }
                else
                {
                    if (msg.Error == "") msg.Error = "you must to be admin";
                    return NotFound(msg);
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return StatusCode(500, msg);
            }
        }
    }
}
