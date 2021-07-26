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
    //[Route("api/[controller]/{id?}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Route("api/[controller]/")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddUserModel user)
        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {

                if (await HelperClass.IsObjectNullOrEmptyString(user.FullName))
                {
                    msg.Error = "Full name is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Phone)&& await HelperClass.IsObjectNullOrEmptyString(user.Email))
                {
                    msg.Error = "Email or mobile is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Country.ToString()) || user.Country ==new Guid())
                {
                    msg.Error = "Country is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Gender))
                {
                    msg.Error = "Gender is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Password))
                {
                    msg.Error = "Password is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.DOB.ToShortDateString()) || user.DOB==new DateTime())
                {
                    msg.Error = "DOB is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Email)==false)
                {
                    DataRow dtr = await Database.FindRow("users", "email", user.Email);
                    if (dtr != null)
                    {
                        msg.Error = "email is unique";
                        return NotFound(msg);
                    }
                }
                else
                {
                    user.Email = string.Empty;
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Phone)==false)
                {
                    DataRow dtr = await Database.FindRow("users", "mobile", user.Phone);
                    if (dtr != null)
                    {
                        msg.Error = "mobile is unique";
                        return NotFound(msg);
                    }
                }
                else
                {
                    user.Phone = string.Empty;
                }
                Ref<TokenModel> token = new Ref<TokenModel>();
                if (await Users_Management.Add(user, msg, token))
                    {
                        return Ok(token);
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

        [Route("api/[controller]/{id}")]
        [HttpGet]
        public async Task<IActionResult> show_profile( [FromRoute] string  id,[FromQuery] string lang)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(id))
                {
                    msg.Error = "ID is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(lang))
                {
                    lang = "AR";
                }
                UserModel user =await Users_Management.showProfile(id, lang, msg);
                if (user != null)
                {
                    Ref<UserModel> result = new Ref<UserModel>();
                    result.Value = user;
                    return Ok(result);
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



        [Route("api/[controller]/change_password")]
        [HttpPost]
        public async Task<IActionResult> change_password([FromHeader] string Authorization, [FromBody] ChangePassword usermodel)
        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(usermodel.Password))
                {
                    msg.Error = "Password is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(usermodel.New_Password ))
                {
                    msg.Error = "New Password is Reqiuerd";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid userID = new Guid(user.Value["id"].ToString());
                    ChangePasswordModel changePassword = new ChangePasswordModel()
                    {
                        New_Password = usermodel.New_Password,
                        Password = usermodel.Password,
                        ID = userID
                    };
                    if (await Users_Management.ChangePassword(changePassword, msg))
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

        [Route("api/[controller]/forget_password")]
        [HttpPost]
        public async Task<IActionResult> forget_password( [FromQuery] string  email)
        {

            ER_Ref<string> msg = new ER_Ref<string>();
            Ref<string> code = new Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(email))
                {
                    msg.Error = "Email is Reqiuerd";
                    return NotFound(msg);
                }
                if (await Users_Management.ForgetPassword(email, msg))
                {
                    return Ok();
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

        [Route("api/[controller]/check_code")]
        [HttpPost]
        public async Task<IActionResult> check_code([FromBody] VerficationCodeModel model)
        {

            ER_Ref<string> msg = new ER_Ref<string>();
            Ref<string> code = new Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(model.Email))
                {
                    msg.Error = "Email is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(model.Code))
                {
                    msg.Error = "Code is Reqiuerd";
                    return NotFound(msg);
                }
                if (await Users_Management.CheckCode(model, msg))
                {
                    return Ok();
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


        [Route("api/[controller]/set_password")]
        [HttpPost]
        public async Task<IActionResult> set_password([FromBody] ResetPasswordModel model)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(model.Email))
                {
                    msg.Error = "Email is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(model.Password))
                {
                    msg.Error = "Password is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(model.Code))
                {
                    msg.Error = "Code is Reqiuerd";
                    return NotFound(msg);
                }
                if (await Users_Management.SetPassword(model, msg))
                {
                    return Ok("Done");
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

        [Route("api/[controller]/resend_code")]
        [HttpPost]
        public async Task<IActionResult> resend_code([FromQuery] string email)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            Ref<string> code = new Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(email))
                {
                    msg.Error = "Email is Reqiuerd";
                    return NotFound(msg);
                }
                if (await Users_Management.ForgetPassword(email, msg))
                {
                    return Ok(code);
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

        [Route("api/[controller]/{id}")]
        [HttpPost]
        public async Task<IActionResult> edit([FromHeader] string Authorization, [FromBody] editUserModel user ,[FromRoute]string id)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(id))
                {
                    msg.Error = "ID is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.FullName))
                {
                    msg.Error = "Full name is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Phone) && await HelperClass.IsObjectNullOrEmptyString(user.Email))
                {
                    msg.Error = "Email or mobile is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Country.ToString()) || user.Country == new Guid())
                {
                    msg.Error = "Country is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Gender))
                {
                    msg.Error = "Gender is required";
                    return NotFound(msg);
                }
                
                if (await HelperClass.IsObjectNullOrEmptyString(user.DOB.ToShortDateString()) || user.DOB == new DateTime())
                {
                    msg.Error = "DOB is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Email) == false)
                {
                    DataRow dtr = await Database.FindRow("users", "email", user.Email);
                    if (dtr != null && dtr["id"].ToString().ToLower() != id.ToLower())
                    {
                        msg.Error = "email is unique";
                        return NotFound(msg);
                    }
                }
                else
                {
                    user.Email = string.Empty;
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Phone) == false)
                {
                    DataRow dtr = await Database.FindRow("users", "mobile", user.Phone);
                    if (dtr != null && dtr["id"].ToString().ToLower() != id.ToLower())
                    {
                        msg.Error = "mobile is unique";
                        return NotFound(msg);
                    }
                }
                else
                {
                    user.Phone = string.Empty;
                }
                Ref<DataRow> user_ = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user_))
                {
                    Ref<string> result = new Ref<string>();
                    result.Value = "edited";
                    if (await Users_Management.edit(new Guid(id), user, msg))
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return StatusCode(500, msg);
                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return StatusCode(500, msg);
            }
        }

        [Route("api/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {

                if (await HelperClass.IsObjectNullOrEmptyString(user.Email))
                {
                    msg.Error = "Email is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(user.Password))
                {
                    msg.Error = "Password is Reqiuerd";
                    return NotFound(msg);
                }
               
                Ref<TokenModel> token = new Ref<TokenModel>();
                if (await Users_Management.Login(user, msg, token))
                {
                    return Ok(token);
                }
                else
                {
                    if (msg.Error is null ||  msg.Error == string.Empty)
                        msg.Error = "Passsword is not correct";
                    return StatusCode(500, msg);
                }


            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return StatusCode(500, msg);
            }
        }

        [Route("api/[controller]/Testing")]
        [HttpPost]
        public async Task<IActionResult> TestImages([FromBody] FormFile item)
        {
            return Ok();
        }
    }
}
