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
            
            //last months sales
            model.Prev30Leads = _leadService.Total(new LeadFilterOptions() { StartDate = (DateTime.Now - TimeSpan.FromDays(60)), EndDate = (DateTime.Now - TimeSpan.FromDays(30))});
            model.Prev30Sales = _saleService.Total(new SaleFilterOptions() { IsQualified = true, StartDate = (DateTime.Now - TimeSpan.FromDays(30)), EndDate = (DateTime.Now - TimeSpan.FromDays(30)) });
            
            //this months sales
            model.Last30Leads = _leadService.Total(new LeadFilterOptions() { StartDate = (DateTime.Now - TimeSpan.FromDays(30)) });
            model.Last30Sales = _saleService.Total(new SaleFilterOptions() { IsQualified = true, StartDate = (DateTime.Now - TimeSpan.FromDays(30)) });
            
            //other this month
            model.TotalActiveDeals = _saleService.Total(new SaleFilterOptions() { IsQualified = true });

            //total pipeline value
            model.TotalSalesPipeline = _saleService.Sum(new SaleFilterOptions() { IsQualified = true });

            model.Last30TotalEmailed = _leadService.Total(new LeadFilterOptions() { Type = LeadType.Emailed });
            model.Last30TotalNoAnswer = _leadService.Total(new LeadFilterOptions() { Type = LeadType.NoAnswer });
            model.Last30TotalNotInterested = _leadService.Total(new LeadFilterOptions() { Type = LeadType.NotInterested });
            model.Last30TotalCallback = _leadService.Total(new LeadFilterOptions() { Type = LeadType.Callback });
            model.Last30TotalDoNotContact = _leadService.Total(new LeadFilterOptions() { Type = LeadType.DoNotContact });

            model.Last30TotalWon = _saleService.Total(new SaleFilterOptions() { Status = SaleType.Won, StartDate = (DateTime.Now - TimeSpan.FromDays(30)) });
            model.Last30TotalLost = _saleService.Total(new SaleFilterOptions() { Status = SaleType.Lost, StartDate = (DateTime.Now - TimeSpan.FromDays(30)) });

            model.Tasks = _taskService.GetAll();
            model.Activities = _activityService.GetAll(new PagingSettings() { PageCount = 6, PageIndex = 1 }).ToList();

            return View("Index", model);
        }

    }
}
