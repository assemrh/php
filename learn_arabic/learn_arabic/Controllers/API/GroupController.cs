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
    public class GroupController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromHeader] string Authorization,[FromQuery]string lang,[FromQuery] Guid cat_id)
        {
            if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
            {
                return Unauthorized();
            }
            Ref<DataRow> user = new Ref<DataRow>();
            if (await Users_Management.GetCurrentUser(Authorization, user)==false)
                return Unauthorized();
            ER_Ref<string> msg = new ER_Ref<string>();
            if (await HelperClass.IsObjectNullOrEmptyString(cat_id) || cat_id==new Guid())
            {
                msg.Error = "uncorrect category id";
                return NotFound(msg);
            }
            if (lang == null) lang = "AR";
            
            var data = await Groups_Management.Get_Groups(lang,cat_id, msg);
            if (msg.Error == "")
            {
                return NotFound(msg);
            }
            return Ok(data);
        }

        [Route("info")]
        [HttpGet]
        public async Task<IActionResult> Get_group_info([FromHeader] string Authorization, [FromQuery] string lang, [FromQuery] Guid GID)
        {
            if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
            {
                return Unauthorized();
            }
            Ref<DataRow> user = new Ref<DataRow>();
            if (await Users_Management.GetCurrentUser(Authorization, user) == false)
                return Unauthorized();
            ER_Ref<string> msg = new ER_Ref<string>();
            if (await HelperClass.IsObjectNullOrEmptyString(GID) || GID == new Guid())
            {
                msg.Error = "uncorrect group id";
                return NotFound(msg);
            }
            if (lang == null) lang = "AR";

            var data = await Groups_Management.Get_Group_Info(GID ,lang, msg);
            if (data == null )
            {
                return NotFound(msg);
            }
            return Ok(data);
        }

        //[Route("/api/[controller]/info")]
        //public IActionResult test()
        //{
        //    return RedirectToAction("Get_group_info");
        //}

        [HttpPost]
        public async Task<IActionResult> Post_user_group(Guid user_id, [FromHeader]string Authorization, Guid group_id, string lang)
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

                else if (group_id == new Guid())
                {
                    msg.Error = "Group id is required";
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

                    if (await Groups_Management.Add_user_group(user_id, group_id, lang, msg))
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
    }
}
