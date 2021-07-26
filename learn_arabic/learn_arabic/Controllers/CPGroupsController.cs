using learn_arabic.Classes;
using learn_arabic.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using m = learn_arabic.Management.Groups_Management;
using mc = learn_arabic.Management.Categories_Management;


namespace learn_arabic.Controllers
{
    //[Route("admin/Groups/{Action=Index}/")]
    public class GroupsController : _BaseController
    {
        ER_Ref<string> msg_Ref = new ER_Ref<string>();

        [HttpGet]
        public async Task<IActionResult> Index(int p = 1)
        {
            page = p;
            ViewBag.Controller = "/admin/Groups";
            ViewBag.TableTitle = "Groups table";
            ViewBag.Img = "/img/svg/tags.svg";
            ViewBag.Title = "Admin Panel - Groups";
            ViewBag.Message = Resources.Message.NoGroup;
            ViewBag.CurrentPage = page;

            Task<PaginationList<ShowGroupModel>> usersTask;
            PaginationList<ShowGroupModel> Groups;

            await (usersTask = m.Get_Groups(lang, 20, page, msg_Ref));
            Groups = usersTask.Result;
            if (usersTask.IsCompleted)
            {
                ViewBag.ColTitles = new List<string>()
                {
                    Resources.General.Name,
                    Resources.General.Prev_Name,
                    Resources.General.Category_Name,
                    Resources.General.End_Time,
                    Resources.General.Start_Time,
                };
                ViewBag.colKeys = new List<string>()
                {
                    "Name",
                    "Prev_Name",
                    "Category_Name",
                    "Start_Time",
                    "End_Time",

                };
                //var x = ((int)Math.Floor(Groups.ItemsCount / 20.0)) + 1;
                //HttpContext.Session.SetInt32("TotalPage", x);
                //ViewBag.TotalPage = x;
                List<RowT> keyValues = new List<RowT>() { };
                if (Groups != null && Groups.Count > 0)
                {
                    foreach (var item in Groups)
                    {
                        keyValues.Add(
                            new RowT() {
                                {"Id",item.ID },
                                { "Name", item.Name },
                                { "Prev_Name", item.Prev_Name },
                                { "Category_Name", item.Category_Name },
                                { "Start_Time", item.Start_Time.ToString() },
                                { "End_Time", item.End_Time.ToString() },
                            });
                    }
                }



                return View("IndexTable", keyValues);
            }
            //TODO Remove  msg.Error before deploy the project
            ViewBag.Error = "unKnowen erroe :" + msg_Ref.Error;
            return View("IndexTable");
        }


        [HttpGet]
        public async Task<IActionResult> Add(string ReturnUrl)
        {
            ViewBag.TableTitle = "Add New Group";
            ViewBag.Action = "Add";
            if (string.IsNullOrWhiteSpace(ReturnUrl))
                ReturnUrl = "/admin/Groups/";
            ViewBag.Title = "Admin Panel - Add New Group";

            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.GroupsList = await m.Get_Groups(lang,new Guid(), msg_Ref);
            ViewBag.CategoriesList = await mc.Get_Categories(lang, msg_Ref);
            return View("forms/Group");
        }
        [HttpPost]
        public async Task<IActionResult> Add(GroupModel Group)
        {

            if (!string.IsNullOrWhiteSpace(Group.Arabic_Name))
            {
                if (string.IsNullOrWhiteSpace(Group.English_Name))
                    Group.English_Name = Group.Arabic_Name;
                if (string.IsNullOrWhiteSpace(Group.Turkish_Name))
                    Group.Turkish_Name = Group.Arabic_Name;
                if (string.IsNullOrWhiteSpace(Group.Russian_Name))
                    Group.Russian_Name = Group.Arabic_Name;
                var task = await m.Add(Group, msg_Ref);
                if (task)
                {
                    ViewBag.Success = Resources.Message.GroupAdded;
                    return StatusCode(201, new { @msg = Resources.Message.GroupAdded, });
                }
            }
            return Json(new { @msg = Resources.Message.EmptyVailds, @title = Resources.General.Failed });
        }

        [HttpGet]
        public async Task<IActionResult> Edite(string Id, string ReturnUrl)
        {
            ViewBag.TableTitle = "Edite Group";
            ViewBag.Action = "Edite";
            ViewBag.ReturnUrl = ReturnUrl;
            if (string.IsNullOrWhiteSpace(Id))
                return RedirectToAction("Index");
            ViewBag.Title = "Admin Panel - Edit Group";

            ViewBag.CategoriesList = await mc.Get_Categories(lang, msg_Ref);
            Task<GroupModel> usersTask;
            GroupModel Group = new GroupModel();

            Guid guid;

            if (Guid.TryParse(Id, out guid))
            {
                await (usersTask = m.Get_Group(guid, msg_Ref));
                Group = usersTask.Result;
                return View("forms/Group", Group);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edite(GroupModel model, string ReturnUrl)
        {
            ViewBag.TableTitle = "Edite Group";
            ViewBag.Action = "Edite";
            if (string.IsNullOrWhiteSpace(model.ID))
                return BadRequest("invalid Id");
            if (string.IsNullOrWhiteSpace(ReturnUrl))
                ReturnUrl = "/admin/Groups/";
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.CategoriesList = await mc.Get_Categories(lang, msg_Ref);
            Task<bool> Task;

            Guid guid;

            if (Guid.TryParse(model.ID, out guid))
            {
                await (Task = m.Edit(guid, model, msg_Ref));
                if (Task.Result)
                    return StatusCode(201, new { @msg = Resources.Message.GroupEdited, @title = Resources.General.Edited });

                return BadRequest(msg_Ref.Error);
            }
            return BadRequest("invalid Id");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var test = HttpContext;
            var test2 = Request;
            Guid guid;
            if (Guid.TryParse(Id, out guid))
            {
                var task = await m.Delete(guid, msg_Ref);
                if (task)
                {
                    return Ok(new { @msg = Resources.Message.GroupDeleted, @title = Resources.General.Deleted });
                }
                return BadRequest(msg_Ref.Error);
            }
            return BadRequest("invalid Id");
        }

        public async Task<IActionResult> Details(String Id, string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (string.IsNullOrWhiteSpace(Id))
                return Redirect(ReturnUrl);

            Task<GroupModel> usersTask;
            GroupModel Group = new GroupModel();

            Guid guid;

            if (Guid.TryParse(Id, out guid))
            {
                await (usersTask = m.Get_Group(guid, msg_Ref));
                Group = usersTask.Result;
                return View("Group_Details", Group);
            }
            return RedirectToAction("Index");

        }


    }
}
