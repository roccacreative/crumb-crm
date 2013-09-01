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
using Newtonsoft.Json;


namespace CrumbCRM.Web.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ILeadService _leadService;
        private readonly IContactService _contactService;
        private readonly IMembershipService _membershipService;
        private readonly IRoleService _roleService;
        private readonly INoteService _noteService;
        private readonly IActivityService _activityService;
        private readonly ISaleService _saleService;
        private readonly ITaskService _taskService;
        private readonly ViewDataHelper _viewdataHelper;
        private readonly ICampaignService _campaignService;
        private readonly ITagService _tagService;

        public SearchController(
            ILeadService LeadService,
            IContactService contactService,
            IMembershipService membershipService,
            IRoleService roleService,
            INoteService noteService,
            IActivityService activityService,
            ISaleService saleService,
            ITaskService taskService,
            ViewDataHelper viewdataHelper,
            ICampaignService campaignService,
            ITagService tagService)
        {
            _leadService = LeadService;
            _contactService = contactService;
            _membershipService = membershipService;
            _roleService = roleService;
            _noteService = noteService;
            _activityService = activityService;
            _saleService = saleService;
            _taskService = taskService;
            _viewdataHelper = viewdataHelper;
            _campaignService = campaignService;
            _tagService = tagService;
        }

        public ActionResult Index(string id)
        {
            string query = id;
            var model = new SearchViewModel();

            //gather lead results
            model.Leads = SearchLeads(query);
            model.Leads.AddRange(SearchTagsLeads(query));
            model.Leads.AddRange(SearchCampaignsLeads(query));

            model.Leads = model.Leads.Distinct().ToList();
            
            //gather sale results
            model.Sales = SearchSales(query);
            model.Sales.AddRange(SearchTagsSales(query));
            model.Sales.AddRange(SearchCampaignsSales(query));

            model.Sales = model.Sales.Distinct().ToList();

            return View("Index", model);
        }

        /* search for leads/sales */
        private List<Lead> SearchLeads(string query)
        {
            var leads = _leadService.GetAll(new LeadFilterOptions() { SearchTerm = query }).ToList();
            return leads;
        }
        private List<Sale> SearchSales(string query)
        {
            var sales = _saleService.GetAll(new SaleFilterOptions() { SearchTerm = query }).ToList();
            return sales;
        }

        /* search for leads/sales by tags */

        private List<Lead> SearchTagsLeads(string query)
        {
            List<Tag> tags = _tagService.GetAll(new TagFilterOptions() { SearchTerm = query }).ToList();
            List<Lead> leads = _leadService.GetAll(new LeadFilterOptions() { Tags = tags.ToList<object>() });
            return leads;
        }

        private List<Sale> SearchTagsSales(string query)
        {
            List<Tag> tags = _tagService.GetAll(new TagFilterOptions() { SearchTerm = query }).ToList();
            List<Sale> sales = _saleService.GetAll(new SaleFilterOptions() { Tags = tags.ToList<object>() });
            return sales;
        }

        private List<Lead> SearchCampaignsLeads(string query)
        {
            List<Campaign> campaigns = _campaignService.GetAll(new CampaignFilterOptions() { SearchTerm = query }).ToList();
            List<Lead> leads = _leadService.GetAll(new LeadFilterOptions() { Campaigns = campaigns.ToList<object>() });
         
            return leads;
        }
        private List<Sale> SearchCampaignsSales(string query)
        {
            List<Campaign> campaigns = _campaignService.GetAll(new CampaignFilterOptions() { SearchTerm = query }).ToList();
            List<Sale> sales = _saleService.GetAll(new SaleFilterOptions() { Campaigns = campaigns.ToList<object>() });
            
            return sales;
        }



    }
}
