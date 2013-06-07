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
    public class LeadController : Controller
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

        public LeadController(
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

        public ActionResult Add()
        {
            _viewdataHelper.InitializeMembers(ViewData, null);
            _viewdataHelper.InitializeCompanies(ViewData, null);
            _viewdataHelper.InitializeCampaigns(ViewData, null);

            ViewData.SelectListEnumViewData<PriorityType>("PriorityType", true);


            return View("Add");
        }

        [HttpPost]
        public ActionResult Add(Lead lead, FormCollection form)
        {
            bool isNew = false;

            //write activity to the log
            if (lead.ID == 0)
            {
                isNew = true;
                lead.Status = LeadType.New;
            }

            if (!string.IsNullOrEmpty(form["Companies"]))
            {
                lead.CompanyID = System.Convert.ToInt32(form["Companies"]);
                // make sure we clear the company name reference if association already exists
                lead.CompanyName = string.Empty;
            }

            if (!string.IsNullOrEmpty(form["PriorityType"]))
            {
                lead.Priority = (PriorityType)Enum.Parse(typeof(CrumbCRM.PriorityType), form["PriorityType"], true);
            }

            if (!string.IsNullOrEmpty(form["Campaigns"]))
                lead.CampaignID = System.Convert.ToInt32(form["Campaigns"]);
            else if(!string.IsNullOrEmpty(form["CampaignName"]))
            {
                var campaign = new Campaign()
                {
                    CreatedByID = _membershipService.GetCurrentMember().UserId,
                    CreatedDate = DateTime.Now,
                    Name = form["CampaignName"],
                    Description = ""
                };

                lead.CampaignID = _campaignService.Save(campaign);
            }

            lead.OwnerID = _membershipService.GetCurrentMember().UserId;

            _leadService.Save(lead);            

            if (!string.IsNullOrEmpty(form["TagSelector"]))
            {
                var current = _tagService.GetByArea(AreaType.Lead);
                string[] tags = form["TagSelector"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                tags.ToList().ForEach(t => {
                    var tag = _tagService.GetByName(t);
                    if(tag == null)
                    {
                        tag = new Tag()
                        {
                            Name = t.ToLower(),
                            CreatedDate = DateTime.Now,
                            CreatedByID = _membershipService.GetCurrentMember().UserId
                        };
                        _tagService.Save(tag);
                    }
                    if(!current.Contains(tag))
                        _leadService.AssignTag(lead, tag);
                });
            }

            if (isNew)
                _activityService.Create("was added", AreaType.Lead, User.Identity.Name, lead.ID);

            TempData.Add("StatusMessage", "Lead added");

            return RedirectToAction("Index");
        }

        public ActionResult Convert(int id, LeadType type)
        {
            Lead lead = _leadService.GetByID(id);
            
            Contact company = new Contact();

            if (!string.IsNullOrEmpty(lead.CompanyName))
            {
                //add a company contact   
                company.Type = ContactType.Company;
                company.CompanyName = lead.CompanyName;
                company.Email = lead.Email;
                company.Mobile = lead.Mobile;
                company.Work = lead.Work;
                company.Address = lead.Address;
                company.City = lead.City;
                company.County = lead.County;
                company.Postcode = lead.Postcode;
                company.OwnerID = lead.OwnerID;

                _contactService.Save(company);
            }

            Contact person = new Contact();
            if (!string.IsNullOrEmpty(lead.FirstName) && !string.IsNullOrEmpty(lead.LastName))
            {
                //add a person contact       
                person.Type = ContactType.Person;                
                person.CompanyID = lead.CompanyID.HasValue ? lead.CompanyID.Value : company.ID;
                person.FirstName = lead.FirstName;
                person.LastName = lead.LastName;
                person.JobTitle = lead.JobTitle;
                person.Email = lead.Email;
                person.Mobile = lead.Mobile;
                person.Work = lead.Work;
                person.Address = lead.Address;
                person.City = lead.City;
                person.County = lead.County;
                person.Postcode = lead.Postcode;
                person.OwnerID = lead.OwnerID;

                _contactService.Save(person);
            }

            Sale sale = new Sale()
            {
                Name = lead.FirstName + " " + lead.LastName,
                JobTitle = company.CompanyName,
                CreatedDate = DateTime.Now,
                CompanyID = lead.CompanyID.HasValue ? lead.CompanyID.Value : company.ID,
                OwnerID = lead.OwnerID,
                PersonID = person.ID,
                Status = SaleType.Prospecting,
                CampaignID = lead.CampaignID
            };
           
            // set converted date
            lead.Converted = DateTime.Now;            
            _leadService.Save(lead);

            _saleService.Save(sale);
            _activityService.Create("was converted", AreaType.Sale, User.Identity.Name, sale.ID);

            // re-assign existing tags
            lead.Tags.ToList().ForEach(t => _saleService.AssignTag(sale, t.Tag));

            TempData.Add("StatusMessage", "Lead converted");

            return RedirectToAction("Index", "Sale");
        }

        public ActionResult AddNote(Note LeadNote, FormCollection Form)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Lead Lead = _leadService.GetByID(id);
            _leadService.Delete(Lead);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Lead editLead = _leadService.GetByID(id);

            _viewdataHelper.InitializeMembers(ViewData, editLead.OwnerID);
            _viewdataHelper.InitializeCompanies(ViewData, editLead.CompanyID);
            _viewdataHelper.InitializeCampaigns(ViewData, editLead.CampaignID);

            List<dynamic> tags = new List<dynamic>();
            editLead.Tags.ToList().ForEach(t => tags.Add(new { id = t.TagID, name = t.Tag.Name }));

            ViewBag.CurrentTags = JsonConvert.SerializeObject(tags);
            ViewData.SelectListEnumViewData<PriorityType>("PriorityType", true);

            return View("Add", editLead);
        }

        public ActionResult Update(int id, LeadType type)
        {
            var lead = _leadService.GetByID(id);
            lead.Status = type;

            //write activity to the log
            _activityService.Create("status was updated", AreaType.Lead, User.Identity.Name, lead.ID);
            _leadService.Save(lead);

            return RedirectToAction("Index");
        }

        public ActionResult RenderItems(string id, int? page, string order)
        {
            var campaigns = (List<Campaign>)TempData["SelectedCampaigns"];
            TempData.Keep("SelectedCampaigns");

            var tags = (List<Tag>)TempData["SelectedTags"];
            TempData.Keep("SelectedCampaigns");

            var options = new LeadFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower(),
                Campaigns = campaigns != null ? campaigns.ToList<object>() : null,
                Tags = tags != null ? tags.ToList<object>() : null
            };
            ViewBag.order = order;

            int pageNo = page ?? 1;
            int total = _leadService.Total(options);
            int totalPages = System.Convert.ToInt32(Math.Ceiling((decimal)total / (decimal)10)); 

            //default lead type
            if (string.IsNullOrEmpty(id))
                id = "New";

            if (totalPages > pageNo)
                ViewBag.Next = (pageNo + 1);

            var leadType = (LeadType)Enum.Parse(typeof(CrumbCRM.LeadType), id, true);
            var model = _leadService.GetAll(options, new PagingSettings() { PageCount = 10, PageIndex = pageNo }).Where(x => (string.IsNullOrEmpty(id) || (x.Status.HasValue && x.Status.Value == leadType))).ToList();

            return PartialView("Controls/_RenderItems", model);
        }

        public ActionResult Index(string id, int? page, string order)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", new { id = "New" });

            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var campaigns = (List<Campaign>)TempData["SelectedCampaigns"];
            TempData.Keep("SelectedCampaigns");

            var tags = (List<Tag>)TempData["SelectedTags"];
            TempData.Keep("SelectedCampaigns");

            var options = new LeadFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower(),
                Campaigns = campaigns != null ? campaigns.ToList<object>() : null,
                Tags = tags != null ? tags.ToList<object>() : null
            };
            ViewBag.order = order;

            var model = new LeadViewModel();
            model.LeadType = (LeadType)Enum.Parse(typeof(CrumbCRM.LeadType), id, true);
            model.TotalNew = _leadService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == LeadType.New).Count();
            model.TotalEmailed = _leadService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == LeadType.Emailed).Count();
            model.TotalNoAnswer = _leadService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == LeadType.NoAnswer).Count();
            model.TotalNotInterested = _leadService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == LeadType.NotInterested).Count();
            model.TotalCallback = _leadService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == LeadType.Callback).Count();
            model.TotalDoNotContact = _leadService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == LeadType.DoNotContact).Count();

            ViewData.SelectListEnumViewData<NoteActionType>("ActionType", true);

            int pageNo = page ?? 1;
            int total = _leadService.Total(options);
            int totalPages = System.Convert.ToInt32(Math.Ceiling((decimal)total / (decimal)10));            

            if (totalPages > pageNo)
                ViewBag.Next = (pageNo + 1);

            return View("Index", model);
        }

        public ActionResult View(int id)
        {
            if (TempData.Keys.Contains("Message"))
                ViewBag.Message = TempData["Message"];

            var model = new LeadItemViewModel();
            model.Lead = _leadService.GetByID(id);
            model.Notes = _noteService.GetByType(id, NoteType.Lead);

            ViewData.SelectListEnumViewData<NoteActionType>("ActionType", true);

            return View(model);
        }

        public ActionResult RemoveTag(int id, int tagId)
        {
            var lead = _leadService.GetByID(id);
            lead.Tags.Remove(lead.Tags.FirstOrDefault(t => t.TagID == tagId));
            _leadService.Save(lead);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

    }
}
