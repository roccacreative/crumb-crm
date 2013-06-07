using CrumbCRM.Filters;
using CrumbCRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrumbCRM.Web.Helpers;
using System.Text.RegularExpressions;

namespace CrumbCRM.Web.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ITaskService _taskService;

        public CalendarController(
            ITaskService taskService)
        {
            _taskService = taskService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UpdateDeadlineEvent(int id, string date)
        {
            var task = _taskService.GetByID(id);
            //parse date from fullcalendar
            Match date_clean = Regex.Match(date, @"^(\w{3}\s\w{3}\s\d{2}\s\d{4}\s\d{2}:\d{2}:\d{2})");

            task.DueDate = Convert.ToDateTime(date_clean.ToString());
            _taskService.Save(task);

            return Json(new { success = true });
        }

        public JsonResult GetDeadlineEvents(double start, double end)
        {
            List<Task> tasks = _taskService.GetAll(new TaskFilterOptions() { StartDate = FromUnix(start), EndDate = FromUnix(end) });
            List<dynamic> events = new List<dynamic>();

            foreach (var item in tasks)
            {
                string bg = "";
                switch (item.ActionType)
                {
                    case ActionType.Callback:
                        bg = "#7ecc76";
                        break;
                    case ActionType.Meeting:
                        bg = "#4283c3";
                        break;
                    default:
                        break;
                }

                events.Add(new
                {
                    id = item.ID,
                    title = CrumbCRM.Web.Helpers.HtmlHelper.StripHtml(item.Body),
                    start = String.Format("{0:F}", item.DueDate),
                    //end = String.Format("{0:F}", DateTime.Now),
                    allDay = true,
                    backgroundColor = bg,
                    borderColor = bg
                });   
            }

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public DateTime FromUnix(double date)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(date).ToLocalTime();
            return dtDateTime;
        }
    }
}
