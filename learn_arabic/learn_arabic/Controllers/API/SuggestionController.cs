using learn_arabic.Classes;
using learn_arabic.Management;
using learn_arabic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Controllers.API
{
    [ApiController]
    public class SuggestionController : ControllerBase
    {
        [Route("api/[controller]/visitor")]
        [HttpPost]
        public async Task<IActionResult> Vistor_Add([FromBody] VistorSuggestionModel suggestion)
        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {

                if (await HelperClass.IsObjectNullOrEmptyString(suggestion.Name))
                {
                    msg.Error = "Name is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(suggestion.Mobile) && await HelperClass.IsObjectNullOrEmptyString(suggestion.Email))
                {
                    msg.Error = "Mobile or email is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(suggestion.Suggestion))
                {
                    msg.Error = "Suggestion is Reqiuerd";
                    return NotFound(msg);
                }
                if (await Suggestions_Management.Add(suggestion, msg))
                {
                    return Ok("added");
                }
                else
                {
                    return StatusCode(500, msg);
                }


            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return StatusCode(500, msg);
            }
        }

        [Route("api/[controller]/user")]
        [HttpPost]
        public async Task<IActionResult> user_Add([FromHeader] string Authorization, [FromBody] UserSuggestionModel suggestion)
        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {

                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(suggestion.Suggestion))
                {
                    msg.Error = "Suggestion is Reqiuerd";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if(await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Ref<string> userID = new Ref<string>();
                    VistorSuggestionModel suggestion_ =await  Users_Management.GetUserInfoForSuggestion(user, userID);
                    suggestion_.Suggestion = suggestion.Suggestion;
                    if (await Suggestions_Management.Add(userID.Value, suggestion_, msg))
                    {
                        return Ok("added");
                    }
                    else
                    {
                        return StatusCode(500, msg);
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
