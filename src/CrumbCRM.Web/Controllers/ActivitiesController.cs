using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrumbCRM.Web.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
