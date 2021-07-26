using learn_arabic.Classes;
using learn_arabic.Management;
using learn_arabic.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Controllers.API
{

    [ApiController]
    public class CompanyController : ControllerBase
    {
        public static IWebHostEnvironment _environment;

        public CompanyController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [Route("api/[controller]/work_type")]
        [HttpGet]
        public async Task<IActionResult> Get_work_types(string lang)
        {
            if (lang == null) lang = "AR";
            ER_Ref<string> msg = new ER_Ref<string>();
            var data = await Companies_Managment.Get_Work_types(lang, msg);
            if (msg.Error == "")
            {
                return NotFound(msg);
            }
            return Ok(data);
        }

        [Route("api/[controller]/work_domain")]
        [HttpGet]
        public async Task<IActionResult> Get_work_domains(string lang)
        {
            if (lang == null) lang = "AR";
            ER_Ref<string> msg = new ER_Ref<string>();
            var data = await Companies_Managment.Get_Work_domains(lang, msg);
            if (msg.Error == "")
            {
                return NotFound(msg);
            }
            return Ok(data);
        }

        [Route("api/[controller]/learning_technique")]
        [HttpGet]
        public async Task<IActionResult> Get_learning_technique(string lang)
        {
            if (lang == null) lang = "AR";
            ER_Ref<string> msg = new ER_Ref<string>();
            var data = await Companies_Managment.Get_learning_techniques(lang, msg);
            if (msg.Error == "")
            {
                return NotFound(msg);
            }
            return Ok(data);
        }





        [Route("api/[controller]")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CompanyModel company)
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            try
            {

                if (await HelperClass.IsObjectNullOrEmptyString(company.Company_Name))
                {
                    msg.Error = "Company Name is Reqiuerd";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.Work_Type_ID) || company.Work_Type_ID == new Guid().ToString())
                {
                    msg.Error = "work type is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.Country_ID.ToString()) || company.Country_ID == new Guid().ToString())
                {
                    msg.Error = "Country is required";
                    return NotFound(msg);
                }
                if (company.Work_Domains == null || company.Work_Domains.Count == 0)
                {
                    msg.Error = "work domains is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.City))
                {
                    msg.Error = "city is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.Address_Details))
                {
                    msg.Error = "Address Details is required";
                    return NotFound(msg);
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.Num_Of_Students))
                {
                    company.Num_Of_Students = "0";
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.Num_Of_Teachers))
                {
                    company.Num_Of_Teachers = "0";
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.Students_Ages_From))
                {
                    company.Students_Ages_From = "0";
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.Students_Ages_To))
                {
                    company.Students_Ages_To = "0";
                }
                if (await HelperClass.IsObjectNullOrEmptyString(company.Email))
                {
                    msg.Error = "Email is required";
                    return NotFound(msg);
                }

                Ref<TokenModel> token = new Ref<TokenModel>();
                Storage.rootPath = _environment.WebRootPath;
               
                if (await Companies_Managment.Add(company, msg))
                {
                    return Ok("Added");
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





        [Route("api/[controller]/TOBASE64")]
        [HttpPost]
        public async Task<IActionResult> Post11([FromForm] IFormFile img)
        {
            if (img.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    img.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                    return Ok(s);
                }
            }

            return NoContent();
        }
    }
}
