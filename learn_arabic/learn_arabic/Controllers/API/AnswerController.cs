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
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        [Route("api/[controller]/Choosing_Answer")]
        [HttpPost]
        public async Task<IActionResult> Add([FromHeader] string Authorization,[FromBody] Choosing_Answer answer)

        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Answer_ID.ToString()) || answer.Answer_ID == new Guid())
                {
                    msg.Error = "Answer ID is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Question_ID.ToString()) || answer.Question_ID == new Guid())
                {
                    msg.Error = "Question ID is required";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid userID = new Guid(user.Value["id"].ToString());
                    UserChoosingAnswerModel _answer=new UserChoosingAnswerModel()
                    {
                        Answer_ID=answer.Answer_ID,
                        Question_ID=answer.Question_ID,
                        User_ID= userID
                    };
                    if (await Answers_Management.UserChoosingAnswer(_answer, msg))
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

        [Route("api/[controller]/ex/Choosing_Answer")]
        [HttpPost]
        public async Task<IActionResult> Add_ex([FromHeader] string Authorization, [FromBody] Choosing_Answer answer)

        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Answer_ID.ToString()) || answer.Answer_ID == new Guid())
                {
                    msg.Error = "Answer ID is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Question_ID.ToString()) || answer.Question_ID == new Guid())
                {
                    msg.Error = "Question ID is required";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid userID = new Guid(user.Value["id"].ToString());
                    UserChoosingAnswerModel _answer = new UserChoosingAnswerModel()
                    {
                        Answer_ID = answer.Answer_ID,
                        Question_ID = answer.Question_ID,
                        User_ID = userID
                    };
                    Ref<int> mark = new Ref<int>();
                    if (await Answers_Management.UserChoosingAnswer(_answer,mark, msg))
                    {
                        return Ok(mark);
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

        [Route("api/[controller]/Text_Answer")]
        [HttpPost]
        public async Task<IActionResult> Add([FromHeader] string Authorization, [FromBody] Text_Answer answer)

        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Answer))
                {
                    msg.Error = "Answer  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Question_ID.ToString()) || answer.Question_ID == new Guid())
                {
                    msg.Error = "Question ID is required";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid userID = new Guid(user.Value["id"].ToString());
                    UserTextAnswerModel _answer = new UserTextAnswerModel()
                    {
                        Answer = answer.Answer,
                        Question_ID = answer.Question_ID,
                        User_ID = userID
                    };
                    if (await Answers_Management.UserTextAnswer(_answer, msg))
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

        [Route("api/[controller]/Matching_Answer")]
        [HttpPost]
        public async Task<IActionResult> Add([FromHeader] string Authorization, [FromBody] Matching_Answer answer)

        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.LeftAnswer_ID.ToString()) || answer.LeftAnswer_ID == new Guid())
                {
                    msg.Error = "Left Answer ID  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.RightAnswer_ID.ToString()) || answer.RightAnswer_ID == new Guid())
                {
                    msg.Error = "Right Answer ID  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Question_ID.ToString()) || answer.Question_ID == new Guid())
                {
                    msg.Error = "Question ID is required";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid userID = new Guid(user.Value["id"].ToString());
                    UserMatchingAnswerModel _answer = new UserMatchingAnswerModel()
                    {
                        User_ID = userID,
                        LeftAnswer_ID=answer.LeftAnswer_ID,
                        RightAnswer_ID=answer.RightAnswer_ID,
                        Question_ID = answer.Question_ID
                    };
                    if (await Answers_Management.UserMatchingAnswer(_answer, msg))
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

        [Route("api/[controller]/ex/Matching_Answer")]
        [HttpPost]
        public async Task<IActionResult> Add_ex([FromHeader] string Authorization, [FromBody] Matching_Answer answer)
        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.LeftAnswer_ID.ToString()) || answer.LeftAnswer_ID == new Guid())
                {
                    msg.Error = "Left Answer ID  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.RightAnswer_ID.ToString()) || answer.RightAnswer_ID == new Guid())
                {
                    msg.Error = "Right Answer ID  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Question_ID.ToString()) || answer.Question_ID == new Guid())
                {
                    msg.Error = "Question ID is required";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid userID = new Guid(user.Value["id"].ToString());
                    UserMatchingAnswerModel _answer = new UserMatchingAnswerModel()
                    {
                        User_ID = userID,
                        LeftAnswer_ID = answer.LeftAnswer_ID,
                        RightAnswer_ID = answer.RightAnswer_ID,
                        Question_ID = answer.Question_ID
                    };
                    Ref<int> mark = new Ref<int>();
                    if (await Answers_Management.UserMatchingAnswer(_answer,mark, msg))
                    {
                        return Ok(mark);
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

        [Route("api/[controller]/Table_Answer")]
        [HttpPost]
        public async Task<IActionResult> Add([FromHeader] string Authorization, [FromBody] Table_Answer answer)

        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Answer_ID.ToString()) || answer.Answer_ID == new Guid())
                {
                    msg.Error = " Answer ID  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Index))
                {
                    msg.Error = "Index  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Question_ID.ToString()) || answer.Question_ID == new Guid())
                {
                    msg.Error = "Question ID is required";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid userID = new Guid(user.Value["id"].ToString());
                    UserTableAnswerModel _answer = new UserTableAnswerModel()
                    {
                        User_ID = userID,
                        Answer_ID = answer.Answer_ID,
                        Index = answer.Index,
                        Question_ID = answer.Question_ID
                    };
                    if (await Answers_Management.UserTableAnswer(_answer, msg))
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
        
        
        [Route("api/[controller]/ex/Table_Answer")]
        [HttpPost]
        public async Task<IActionResult> Add_ex([FromHeader] string Authorization, [FromBody] Table_Answer answer)

        {

            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {
                if (await HelperClass.IsObjectNullOrEmptyString(Authorization))
                {
                    return Unauthorized();
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Answer_ID.ToString()) || answer.Answer_ID == new Guid())
                {
                    msg.Error = " Answer ID  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Index))
                {
                    msg.Error = "Index  is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(answer.Question_ID.ToString()) || answer.Question_ID == new Guid())
                {
                    msg.Error = "Question ID is required";
                    return NotFound(msg);
                }
                Ref<DataRow> user = new Ref<DataRow>();
                if (await Users_Management.GetCurrentUser(Authorization, user))
                {
                    Guid userID = new Guid(user.Value["id"].ToString());
                    UserTableAnswerModel _answer = new UserTableAnswerModel()
                    {
                        User_ID = userID,
                        Answer_ID = answer.Answer_ID,
                        Index = answer.Index,
                        Question_ID = answer.Question_ID
                    };
                    Ref<int> mark = new Ref<int>();
                    if (await Answers_Management.UserTableAnswer(_answer,mark, msg))
                    {
                        return Ok(mark);
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
