using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrumbCRM.Services;
using CrumbCRM.Enums;
using System.Web.Security;
using CrumbCRM.Filters;
using CrumbCRM.Web.Filters;
using CrumbCRM.Web.Models;
using CrumbCRM.Web.Helpers;

namespace CrumbCRM.Web.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        #region Services

        private readonly ILeadService _leadService;
        private readonly IContactService _contactService;
        private readonly IMembershipService _membershipService;
        private readonly IRoleService _roleService;
        private readonly INoteService _noteService;
        private readonly IActivityService _activityService;
        private readonly ITaskService _taskService;
        private readonly ViewDataHelper _initHelper;
        private readonly ITagService _tagService;

        #endregion

        public TaskController(
            ILeadService LeadService, 
            IContactService contactService,
            IMembershipService membershipService,
            IRoleService roleService,
            INoteService noteService,
            IActivityService activityService,
            ITaskService taskService,
            ViewDataHelper initHelper,
            ITagService tagService)
        {
            _leadService = LeadService;
            _contactService = contactService;
            _membershipService = membershipService;
            _roleService = roleService;
            _noteService = noteService;
            _activityService = activityService;
            _taskService = taskService;
            _membershipService = membershipService;
            _initHelper = initHelper;
            _tagService = tagService;
        }
               
        public ActionResult Add()
        {
            _initHelper.InitializeMembers(ViewData, null);
            var model = new TaskViewModel();
            ViewData.SelectListEnumViewData<ActionType>("ActionType", true);
 
            return View("Add");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Task task, FormCollection form)
        {
            bool isNew = false;
            if (task.ID == 0)
                isNew = true;

            task.AssignedByID = _membershipService.GetCurrentMember().UserId;
            task.Body = Helpers.HtmlHelper.CleanHtml(form["Body"]);

            //assign the task to a user
            if (!string.IsNullOrEmpty(form["Members"]))
                task.AssignedID = Convert.ToInt32(form["Members"].ToString());

            //assign areatype (either sale or lead)
            if (!string.IsNullOrEmpty(form["AreaType"]))
                task.AreaType = (AreaType)Convert.ToInt32(form["AreaType"]);

            //assign itemid of the above areatype
            if (!string.IsNullOrEmpty(form["ItemID"]))
                task.ItemID = System.Convert.ToInt32(form["ItemID"]);

            //assign its action type; lead, sale, contact...
            if (!string.IsNullOrEmpty(form["AreaType"]))
                ViewData.SelectListEnumViewData<ActionType>("ActionType", true);

            _taskService.Save(task);

            //write activity to the log
            if (isNew)
                _activityService.Create("was added", AreaType.Task, User.Identity.Name, task.ID);

            TempData.Add("StatusMessage", "Task added");
            return RedirectToAction("Index");
        }

        public ActionResult RenderItems(int? page, string order)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var options = new TaskFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower()
            };
            ViewBag.order = order;

            int pageNo = page ?? 1;
            int total = _taskService.Total(options);
            int totalPages = System.Convert.ToInt32(Math.Ceiling((decimal)total / (decimal)10));

            if (totalPages > pageNo)
                ViewBag.Next = (pageNo + 1);

            var model = _taskService.GetAll(options, new PagingSettings() { PageCount = 10, PageIndex = pageNo });

            return PartialView("Controls/_RenderItems", model);
        }

        public ActionResult Index(string order)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var options = new TaskFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower()
            };
            ViewBag.order = order;

            List<Task> model = _taskService.GetAll(options);

            return View(model);
        }

        public ActionResult Delete(int id) 
        {
            var referer = Request.UrlReferrer;

            Task task = _taskService.GetByID(id);
            task.Deleted = DateTime.Now;
            _taskService.Save(task);

            if(referer != null) {
                return Redirect(referer.ToString());
            } else {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)    
        {            
            _initHelper.InitializeMembers(ViewData, null);
            Task editTask = _taskService.GetByID(id);

            ViewData.SelectListEnumViewData<ActionType>("ActionType", true, (int)editTask.ActionType);

            return View("Add", editTask); 
        }

        public ActionResult DisplaySideTasks(AreaType? areaType, int? itemId, bool detail=false)
        {
            var userId = _membershipService.GetCurrentMember().UserId;
            List<Task> model;

            if (String.IsNullOrEmpty(itemId.ToString()))
            {
                //get all tasks for this user
                model = _taskService.GetAll(new TaskFilterOptions() { AssignedID = userId }, new PagingSettings() { PageCount = 5, PageIndex = 1}).ToList();
            }
            else
            {
                //get tasks for a specific item (lead or sale)
                model = _taskService.GetAll(new TaskFilterOptions() { AssignedID = userId, Area = areaType }, new PagingSettings() { PageCount = 5, PageIndex = 1 }).ToList();
            }

            ViewBag.Detail = detail;

            return PartialView("Controls/_TasksSideList", model);
        }
    }
}
