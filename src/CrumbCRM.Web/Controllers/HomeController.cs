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
    public class HomeController : Controller
    {
        private readonly ILeadService _leadService;
        private readonly ITaskService _taskService;
        private readonly IContactService _contactService;
        private readonly IMembershipService _membershipService;
        private readonly IRoleService _roleService;
        private readonly IActivityService _activityService;
        private readonly ISaleService _saleService;
        private readonly ViewDataHelper _initHelper;

        public HomeController(
            ILeadService leadService,
            ITaskService taskService, 
            IContactService contactService,
            IMembershipService membershipService,
            IRoleService roleService,
            IActivityService activityService,
            ISaleService saleService,
            ViewDataHelper initHelper)
        {
            _leadService = leadService;
            _taskService = taskService;
            _contactService = contactService;
            _membershipService = membershipService;
            _roleService = roleService;
            _activityService = activityService;
            _saleService = saleService;
            _initHelper = initHelper;
        }
              

        public ActionResult Index()
        {
            var model = new HomeViewModel();
            var leads = _leadService.GetAll();
            var sales = _saleService.GetAll();
            
            //last months sales
            model.Prev30Leads = leads.Where(x => (x.CreatedDate > (DateTime.Now - TimeSpan.FromDays(60)) && x.CreatedDate < (DateTime.Now - TimeSpan.FromDays(30)))).Count();
            model.Prev30Sales = sales.Where(x => (x.Status.Value != SaleType.Unqualified && x.Status.Value != SaleType.Unqualified && x.CreatedDate > (DateTime.Now - TimeSpan.FromDays(60)) && x.CreatedDate < (DateTime.Now - TimeSpan.FromDays(30)))).Count();
            //this months sales
            model.Last30Leads = leads.Where(x => x.CreatedDate > (DateTime.Now - TimeSpan.FromDays(30))).Count();
            model.Last30Sales = sales.Where(x => (x.Status.Value != SaleType.Unqualified && x.Status.Value != SaleType.Unqualified && x.CreatedDate > (DateTime.Now - TimeSpan.FromDays(30)))).Count();
            
            //other this month
            model.TotalActiveDeals = sales.Where(x => x.Status.Value != SaleType.Unqualified && x.Status.Value != SaleType.Unqualified).Count();

            //total pipeline value
            model.TotalSalesPipeline = sales.Sum(x => x.Value);

            model.Last30TotalEmailed = leads.Where(x => x.Status.HasValue && x.Status.Value == LeadType.Emailed).Count();
            model.Last30TotalNoAnswer = leads.Where(x => x.Status.HasValue && x.Status.Value == LeadType.NoAnswer).Count();
            model.Last30TotalNotInterested = leads.Where(x => x.Status.HasValue && x.Status.Value == LeadType.NotInterested).Count();
            model.Last30TotalCallback = leads.Where(x => x.Status.HasValue && x.Status.Value == LeadType.Callback).Count();
            model.Last30TotalDoNotContact = leads.Where(x => x.Status.HasValue && x.Status.Value == LeadType.DoNotContact).Count();

            model.Last30TotalWon = sales.Where(x => x.Status.Value == SaleType.Won && x.CreatedDate > (DateTime.Now - TimeSpan.FromDays(30))).Count();
            model.Last30TotalLost = sales.Where(x => x.Status.Value == SaleType.Lost && x.CreatedDate > (DateTime.Now - TimeSpan.FromDays(30))).Count();

            model.Tasks = _taskService.GetAll();
            model.Activities = _activityService.GetAll().Take(6).ToList();

            return View("Index", model);
        }

    }
}
