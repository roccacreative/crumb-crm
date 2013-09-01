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

namespace CrumbCRM.Web.Helpers
{
    public class ViewDataHelper
    {
        private readonly IMembershipService _membershipService;
        private readonly IContactService _contactService;
        private readonly ICampaignService _campaignService;

        public ViewDataHelper(
            IMembershipService membershipService,
            IContactService contactService,
            ICampaignService campaignService)
        {
            _membershipService = membershipService;
            _contactService = contactService;
            _campaignService = campaignService;
        }

        public void InitializeMembers(ViewDataDictionary viewData, int? userId)
        {
            var members = _membershipService.GetUsersInRole("Administrator");
            viewData.Add("Members", new SelectList(members, "UserId", "Username", userId.HasValue ? userId.Value : members.FirstOrDefault(u => u.Username == HttpContext.Current.User.Identity.Name).UserId));
        }
        public void InitializeCompanies(ViewDataDictionary viewData, int? companyId)
        {
            var companies = _contactService.GetAll(new ContactFilterOptions() { Type = ContactType.Company });
            viewData.Add("Companies", new SelectList(companies, "ID", "CompanyName", companyId));
        }

        public void InitializeCampaigns(ViewDataDictionary viewData, int? campaignId)
        {
            var campaigns = _campaignService.GetAll();
            viewData.Add("Campaigns", new SelectList(campaigns, "ID", "Name", campaignId));
        }
    }
}