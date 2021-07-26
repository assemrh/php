using learn_arabic.Classes;
using learn_arabic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using m = learn_arabic.Management.Categories_Management;

namespace learn_arabic.Controllers
{
    //[Route("admin/Categories/{Action=Index}/")]
    public class CategoriesController : _BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Index(int p =1)
        {
            page = p;
            ViewBag.Data = new
            {
                Controller = "/admin/Categories",
                TableTitle = "Categories table",

            };
            ViewBag.Controller = "/admin/Categories";
            ViewBag.TableTitle = "Categories table";
            ViewBag.Img = "/img/svg/tags.svg";
            ViewBag.Title = "Admin Panel - Categories";
            ViewBag.Message = Resources.Message.NoCategory;
            ViewBag.CurrentPage = page;

            ER_Ref<string> msg = new ER_Ref<string>();
            Task<PaginationList<ShowCategoriesModel>> usersTask;
            PaginationList<ShowCategoriesModel> Categories;
            
            await (usersTask = m.Get_Categories(lang,20,page, msg));
            Categories = usersTask.Result;
            if (usersTask.IsCompleted && Categories?.ItemsCount != null)
            {
                ViewBag.ColTitles = new List<string>()
                {
                    Resources.General.Name,
                };
                ViewBag.colKeys = new List<string>()
                {
                    "Name"
                };
                var x  = ((int)Math.Floor(Categories.ItemsCount/20.0)) + 1;
                //HttpContext.Session.SetInt32("TotalPage", x);
                ViewBag.TotalPage = x;
                ViewBag.TotalItems = Categories.ItemsCount;
                List<RowT> keyValues = new List<RowT>() { };
                if(Categories !=null && Categories.Count > 0)
                {
                    foreach (var item in Categories)
                    {
                        keyValues.Add(
                            new RowT() {
                                {"Id",item.ID },
                                { "Name", item.Name },
                            });
                    }
                }



                return View("IndexTable", keyValues);
            }
            //TODO Remove  msg.Error before deploy the project
            ViewBag.Error = "unKnowen erroe :" + msg.Error;
            return View("IndexTable");
        }


        [HttpGet]
        public IActionResult Add(string ReturnUrl)
        {
            ViewBag.TableTitle = "Add New Category";
            ViewBag.Action = "Add";
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.Title = "Admin Panel - Add New Category";

            return View("forms/Category");
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoriesModel category)
        {

            ER_Ref<string> e = new ER_Ref<string>();
            if (!string.IsNullOrWhiteSpace(category.Arabic_Name))
            {
                if (string.IsNullOrWhiteSpace(category.English_Name))
                    category.English_Name = category.Arabic_Name;
                if (string.IsNullOrWhiteSpace(category.Turkish_Name))
                    category.Turkish_Name = category.Arabic_Name;
                if (string.IsNullOrWhiteSpace(category.Russian_Name))
                    category.Russian_Name = category.Arabic_Name;
                var task = await m.Add(category, e);
                if(task)
                {
                    ViewBag.Success = Resources.Message.CategoryAdded;
                    //return Ok(new { @msg = Resources.Message.CategoryAdded,  });
                    return StatusCode(201, new { @msg = Resources.Message.CategoryAdded, });
                }
            }
            return Json(new { @msg = Resources.Message.EmptyVailds, @title=Resources.General.Failed });
        }

        [HttpGet]
        public async Task<IActionResult> Edite(string Id, string ReturnUrl)
        {
            ViewBag.TableTitle = "Edite Category";
            ViewBag.Action = "Edite";
            if (string.IsNullOrWhiteSpace(ReturnUrl))
                ReturnUrl = "/admin/Categories/";
            if (string.IsNullOrWhiteSpace(Id))
                return Redirect(ReturnUrl);

            ViewBag.ReturnUrl = ReturnUrl;
            ER_Ref<string> msg = new ER_Ref<string>();
            Task<CategoriesModel> usersTask;
            CategoriesModel Category = new CategoriesModel();
            ViewBag.Title = "Admin Panel - Edit Category";
            Guid guid;

            if (Guid.TryParse(Id, out guid))
            {
                await(usersTask = m.Get_Category(guid, msg));
                Category = usersTask.Result;
                return View("forms/Category", Category);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edite(CategoriesModel category)
        {
            ViewBag.TableTitle = "Edite Category";
            ViewBag.Action = "Edite";
            if (string.IsNullOrWhiteSpace(category.ID))
                return BadRequest("invalid Id");

            ER_Ref<string> msg = new ER_Ref<string>();
            Task<bool> Task;

            Guid guid;

            if (Guid.TryParse(category.ID, out guid))
            {
                await (Task = m.Edit(guid, category, msg));
                if(Task.Result)
                    return StatusCode(201, new { @msg = Resources.Message.CategoryEdited, @title = Resources.General.Edited });
                
                    return BadRequest(msg.Error);
            }
            return BadRequest("invalid Id");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var test = HttpContext;
            var test2 = Request;
            ER_Ref<string> msg = new ER_Ref<string>();
            Guid guid;
            //var Id = "";
            if (Guid.TryParse(Id, out guid))
            {
                var task = await m.Delete(guid, msg);
                if (task)
                {
                    return Ok( new { @msg = Resources.Message.CategoryDeleted, @title = Resources.General.Deleted });
                }
                return BadRequest(msg.Error);
            }
            return BadRequest("invalid Id");
        }

        public async Task<IActionResult> Details(String Id, string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (string.IsNullOrWhiteSpace(Id))
                return Redirect(ReturnUrl);
            ViewBag.Title = "Admin Panel - Details of Categories";
            ER_Ref<string> msg = new ER_Ref<string>();
            Task<CategoriesModel> usersTask;
            CategoriesModel Category= new CategoriesModel();
            
            Guid guid;

            if (Guid.TryParse(Id, out guid))
            {
                await (usersTask = m.Get_Category(guid, msg));
                Category = usersTask.Result;
                return View("Category_Details", Category);
            }
            return RedirectToAction("Index");

        }


    }
}
