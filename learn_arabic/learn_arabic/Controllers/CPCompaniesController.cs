using learn_arabic.Classes;
using learn_arabic.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using m = learn_arabic.Management.Companies_Managment;


namespace learn_arabic.Controllers
{
    //[Route("admin/Company/{Action=Index}")]
    public class CompaniesController : _BaseController
    {
        public async Task<IActionResult> Index(int p=1)
        {
            ViewBag.CurrentPage = page = p;
            ViewBag.Controller = "/admin/Company";
            ViewBag.TableTitle = "Company table";
            ViewBag.Img = "/img/svg/building-solid.svg";
            ViewBag.Title = "Admin Panel - Company";

            ViewBag.Message = Resources.Message.NoCompany;
            ER_Ref<string> msg = new ER_Ref<string>();
            Task<PaginationList<ShowCompanyModel>> comTypesTask;
            PaginationList<ShowCompanyModel> list;

            await(comTypesTask = m.Get_Companies(lang, 20, page, msg));
            if (comTypesTask.IsCompleted)
            {
                list = comTypesTask.Result;
                ViewBag.ColTitles = new List<string>()
                {
                    Resources.General.Address,
                    Resources.General.Email,
                    Resources.General.Name,
                    Resources.General.WorkType,
                };
                ViewBag.colKeys = new List<string>()
                {
                    "Address",
                    "Email",
                    "Name",
                    "WorkType",
                };

                List<RowT> keyValues = new List<RowT>() { };
                if(list !=null && list.Count>0)
                foreach (var item in list)
                {
                    keyValues.Add(
                        new RowT() {
                            {"Id", item.ID },
                            { "Address", item.Address },
                            { "Email", item.Email },
                            { "Name", item.Name },
                            { "WorkType", item.WorkType },
                        });
                }

                return View("IndexTable", keyValues);
            }
            //TODO Remove  msg.Error before deploy the project
            ViewBag.Error = "unKnowen erroe :" + msg.Error;
            return View("IndexTable");
        }
        public async Task<IActionResult> Types()
        {
            ER_Ref<string> msg = new ER_Ref<string>();
            Task<List<ShowConstantModel>> comTypesTask;
            List<ShowConstantModel> list;

            await(comTypesTask = m.Get_Work_types(lang, msg));
            list = comTypesTask.Result;
            ViewBag.ColTitles = new List<string>()
            {
                Resources.General.CompanyType,
            };
            ViewBag.colKeys = new List<string>()
            {
                "Value",
            };

            List<RowT> keyValues = new List<RowT>() { };
            if(list !=null && list.Count>0)
            foreach (var item in list)
            {
                keyValues.Add(
                    new RowT() {
                        {"Id", item.ID },
                        { "Value", item.Value },
                    });
            }


            ViewBag.Title = "Admin Panel - Company";
            ViewBag.TableTitle = "Company table";
            ViewBag.Img = "/img/svg/users.svg";
            ViewBag.Message = Resources.Message.NoCompany;

            return View("IndexTable", keyValues);
        }


        [HttpGet]
        public IActionResult Add(string ReturnUrl)
        {
            ViewBag.TableTitle = "Add New Company";
            ViewBag.Action = "Add";
            ViewBag.ReturnUrl = ReturnUrl;
            return View("forms/Company");
        }
        [HttpPost]
        public async Task<IActionResult> Add(CompanyModel Company)
        {

            ER_Ref<string> e = new ER_Ref<string>();


                var task = await m.Add(Company, e);
                if (task)
                {
                    ViewBag.Success = Resources.Message.CompanyAdded;
                    //return Ok(new { @msg = Resources.Message.CompanyAdded,  });
                    return StatusCode(201, new { @msg = Resources.Message.CompanyAdded, });
                }
            
            return Json(new { @msg = Resources.Message.EmptyVailds, @title = Resources.General.Failed });
        }

        //[HttpGet]
        //public async Task<IActionResult> Edite(string Id, string ReturnUrl)
        //{
        //    ViewBag.TableTitle = "Edite Company";
        //    ViewBag.Action = "Edite";
        //    ViewBag.ReturnUrl = ReturnUrl;
        //    if (string.IsNullOrWhiteSpace(Id))
        //        return RedirectToAction("Index");

        //    ER_Ref<string> msg = new ER_Ref<string>();
        //    Task<CompanyDetailsModel> usersTask;
        //    CompanyDetailsModel Company = new CompanyDetailsModel();

        //    Guid guid;

        //    if (Guid.TryParse(Id, out guid))
        //    {
        //        await (usersTask = Companies_Managment.Get_Company(guid,lang, msg));
        //        Company = usersTask.Result;
        //        return View("forms/Company", Company);
        //    }
        //    return RedirectToAction("Index");
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edite(ShowCompanyModel Company)
        //{
        //    ViewBag.TableTitle = "Edite Company";
        //    ViewBag.Action = "Edite";
        //    if (string.IsNullOrWhiteSpace(Company.ID))
        //        return BadRequest("invalid Id");

        //    ER_Ref<string> msg = new ER_Ref<string>();
        //    Task<bool> Task;
        //    ShowCompanyModel Company = new ShowCompanyModel();

        //    Guid guid;

        //    if (Guid.TryParse(Company.ID, out guid))
        //    {
        //        await (Task = Companies_Managment.Edit(guid, Company, msg));
        //        if (Task.Result)
        //            return StatusCode(201, new { @msg = Resources.Message.CompanyEdited, @title = Resources.General.Edited });

        //        return BadRequest(msg.Error);
        //    }
        //    return BadRequest("invalid Id");
        //}

        //[HttpDelete]
        //public async Task<IActionResult> Delete(string Id)
        //{
        //    var test = HttpContext;
        //    var test2 = Request;
        //    ER_Ref<string> msg = new ER_Ref<string>();
        //    Guid guid;
        //    //var Id = "";
        //    if (Guid.TryParse(Id, out guid))
        //    {
        //        var task = await Companies_Managment.Delete(guid, msg);
        //        if (task)
        //        {
        //            return Ok(new { @msg = Resources.Message.CompanyDeleted, @title = Resources.General.Deleted });
        //        }
        //        return BadRequest(msg.Error);
        //    }
        //    return BadRequest("invalid Id");
        //}

        public async Task<IActionResult> Details(String Id, string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (string.IsNullOrWhiteSpace(Id))
                return Redirect(ReturnUrl);

            ER_Ref<string> msg = new ER_Ref<string>();
            Task<CompanyDetailsModel> usersTask;
            CompanyDetailsModel Company = new CompanyDetailsModel();

            Guid guid;

            if (Guid.TryParse(Id, out guid))
            {
                await (usersTask = m.Get_Company(guid, lang, msg));
                Company = usersTask.Result;
                return View(Company);
            }
            return RedirectToAction("Index");

        }
    }
}
