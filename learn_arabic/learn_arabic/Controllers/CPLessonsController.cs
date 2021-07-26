using learn_arabic.Classes;
using learn_arabic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using m = learn_arabic.Management.Lessons_Management;
using mb = learn_arabic.Management.Groups_Management;


namespace learn_arabic.Controllers
{
    public class LessonsController : _BaseController
    {
        ER_Ref<string> msg = new ER_Ref<string>();

        [HttpGet]
        public async Task<IActionResult> Index(int p = 1)
        {
            page = p;
            ViewBag.Controller = "/admin/Lessons";
            ViewBag.TableTitle = "Lessons table";
            ViewBag.Img = "/img/svg/tags.svg";
            ViewBag.Title = "Admin Panel - Lessons";
            ViewBag.Message = Resources.Message.NoLesson;
            ViewBag.CurrentPage = page;

            Task<PaginationList<ShowLessonModel>> usersTask;
            PaginationList<ShowLessonModel> Lessons;

            await (usersTask = m.Get_Lessons(lang, 20, page, msg));
            Lessons = usersTask.Result;
            if (usersTask.IsCompleted)
            {
                ViewBag.ColTitles = new List<string>()
                {
                    Resources.General.Name,
                    Resources.General.Group_Name,
                    Resources.General.Image,
                    Resources.General.Video,
                    Resources.General.Descreption,
                    Resources.General.Voice,
                    Resources.General.Prev_id,
                    Resources.General.Next_id,
                };
                ViewBag.colKeys = new List<string>()
                {
                    "Name",
                    "Group_Name",
                    "Image",
                    "Video",
                    "Descreption",
                    "Voice",
                    "Prev_id",
                    "Next_id",
                };
                List<RowT> keyValues = new List<RowT>() { };
                if (Lessons != null && Lessons.Count > 0)
                {
                    int n = Lessons?.ItemsCount ==0 ?  1: Lessons.ItemsCount;
                    var totalPage = ((int)Math.Floor(n / 20.0)) + 1;
                    ViewBag.TotalItems = Lessons.ItemsCount;
                    HttpContext.Session.SetInt32("TotalPage", totalPage);
                    ViewBag.TotalPage = totalPage;
                    foreach (var item in Lessons)
                    {
                        keyValues.Add(
                            new RowT() {
                                {"Id",item.ID },
                                { "Group_Name", item.Group_Name },
                                { "Image", item.Image },
                                { "Video", item.Video },
                                { "Descreption", item.Descreption },
                                { "Name", item.Name },
                                { "Voice", item.Voice },
                                { "Prev_id", item.Prev_id },
                                { "Next_id", item.next_id },
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
        public async Task<IActionResult> Add(string ReturnUrl)
        {
            ViewBag.TableTitle = "Add New Lesson";
            ViewBag.Action = "Add";
            ViewBag.ReturnUrl = ReturnUrl;
            //ViewBag.LessonsList = await m.Get_Lessons(lang, msg);
            ViewBag.GroupsList = await mb.Get_Groups(lang,new Guid(), msg);
            //var GroupsList = ViewBag.CategoriesList as List<ShowGroupModel> ?? new List<ShowGroupModel>();
            return View("forms/Lesson");
        }

        [HttpPost]
        public async Task<IActionResult> Add(LessonModel Lesson)
        {
            //var file = Request.Form.Files;

            ER_Ref<string> e = new ER_Ref<string>();
            if (!string.IsNullOrWhiteSpace(Lesson.Arabic_Lesson.Name))
            {
                if (string.IsNullOrWhiteSpace(Lesson.English_Lesson.Name))
                    Lesson.English_Lesson.Name = Lesson.Arabic_Lesson.Name;
                if (string.IsNullOrWhiteSpace(Lesson.Turkish_Lesson.Name))
                    Lesson.Turkish_Lesson.Name = Lesson.Arabic_Lesson.Name;
                if (string.IsNullOrWhiteSpace(Lesson.Russian_Lesson.Name))
                    Lesson.Russian_Lesson.Name = Lesson.Arabic_Lesson.Name;
                Lesson.Arabic_Lesson.Voice.Base64 = HttpContext.Session.GetString("Ar_Voice");
                Lesson.Arabic_Lesson.Video.Base64 = HttpContext.Session.GetString("Ar_Video");
                Lesson.English_Lesson.Voice.Base64 = HttpContext.Session.GetString("En_Voice");
                Lesson.English_Lesson.Voice.Base64 = HttpContext.Session.GetString("En_Voice");
                Lesson.Turkish_Lesson.Voice.Base64 = HttpContext.Session.GetString("Tr_Voice");
                Lesson.Turkish_Lesson.Voice.Base64 = HttpContext.Session.GetString("Tr_Voice");
                Lesson.Russian_Lesson.Voice.Base64 = HttpContext.Session.GetString("Ru_Voice");
                Lesson.Russian_Lesson.Voice.Base64 = HttpContext.Session.GetString("Ru_Voice");
                Lesson.Image.Base64 = HttpContext.Session.GetString("Image");
                Lesson.Arabic_Lesson.Voice.File_Name = HttpContext.Session.GetString("Ar_VoiceName");
                Lesson.Arabic_Lesson.Video.File_Name = HttpContext.Session.GetString("Ar_VideoName");
                Lesson.English_Lesson.Voice.File_Name = HttpContext.Session.GetString("En_VoiceName");
                Lesson.English_Lesson.Voice.File_Name = HttpContext.Session.GetString("En_VoiceName");
                Lesson.Turkish_Lesson.Voice.File_Name = HttpContext.Session.GetString("Tr_VoiceName");
                Lesson.Turkish_Lesson.Voice.File_Name = HttpContext.Session.GetString("Tr_VoiceName");
                Lesson.Russian_Lesson.Voice.File_Name = HttpContext.Session.GetString("Ru_VoiceName");
                Lesson.Russian_Lesson.Voice.File_Name = HttpContext.Session.GetString("Ru_VoiceName");
                Lesson.Image.File_Name = HttpContext.Session.GetString("ImageName");
                var task = await m.Add(Lesson, e);
                if (task)
                {
                    ViewBag.Success = Resources.Message.LessonAdded;
                    return StatusCode(201, new { @msg = Resources.Message.LessonAdded, });
                }
            }
            return Json(new { @msg = Resources.Message.EmptyVailds, @title = Resources.General.Failed });
        }

        [HttpGet]
        public async Task<IActionResult> Edite(string Id, string ReturnUrl)
        {
            ViewBag.TableTitle = "Edite Lesson";
            ViewBag.Action = "Edite";
            if (string.IsNullOrWhiteSpace(ReturnUrl))
                ReturnUrl = "/admin/Lessons/";
            if (string.IsNullOrWhiteSpace(Id))
                return Redirect(ReturnUrl);

            ViewBag.ReturnUrl = ReturnUrl;
            ER_Ref<string> msg = new ER_Ref<string>();
            Task<LessonModel> usersTask;
            LessonModel Lesson = new LessonModel();

            Guid guid;

            if (Guid.TryParse(Id, out guid))
            {
                await (usersTask = m.Get_LessonModel(guid, msg));
                Lesson = usersTask.Result;
                return View("forms/Lesson", Lesson);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edite(LessonModel Lesson)
        {
            ViewBag.TableTitle = "Edite Lesson";
            ViewBag.Action = "Edite";
            //if (string.IsNullOrWhiteSpace(Lesson.ID))
            //    return BadRequest("invalid Id");

            ER_Ref<string> msg = new ER_Ref<string>();
            Task<bool> Task;

            Guid guid;

            //if (Guid.TryParse(Lesson.ID, out guid))
            //{
            //    await (Task = m.Edit(guid, Lesson, msg));
            //    if (Task.Result)
            //        return StatusCode(201, new { @msg = Resources.Message.LessonEdited, @title = Resources.General.Edited });

            //    return BadRequest(msg.Error);
            //}
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
                    return Ok(new { @msg = Resources.Message.LessonDeleted, @title = Resources.General.Deleted });
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

            ER_Ref<string> msg = new ER_Ref<string>();
            Task<View_Lesson_Model> usersTask;
            View_Lesson_Model Lesson = new View_Lesson_Model();

            Guid guid;

            if (Guid.TryParse(Id, out guid))
            {
                await (usersTask = m.Get_Lesson(guid, msg));
                Lesson = usersTask.Result;
                return View(Lesson);
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> GetOption(Guid id)
        {
            string _content = $"<option value=\"{new Guid()}\">Select one from GetOption</option>";
            var LessonsList = await m.Get_Lessons_by_groupe_id(lang, id, msg);
            foreach (var item in LessonsList)
            {
                _content += $"<option value=\"{item.ID}\">{item.Name}</option>";
            }
            return Content(_content);
        }
    }
}
