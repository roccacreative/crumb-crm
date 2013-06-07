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
using System.Threading;


namespace CrumbCRM.Web.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly ILeadService _leadService;
        private readonly IContactService _contactService;
        private readonly IMembershipService _membershipService;
        private readonly IRoleService _roleService;
        private readonly INoteService _noteService;
        private readonly IActivityService _activityService;
        private readonly ISaleService _saleService;
        private readonly ViewDataHelper _initHelper;
        private readonly ITagService _tagService;

        public SaleController(
            ILeadService LeadService,
            IContactService contactService,
            IMembershipService membershipService,
            IRoleService roleService,
            INoteService noteService,
            IActivityService activityService,
            ISaleService saleService,
            ViewDataHelper initHelper,
            ITagService tagService)
        {
            _leadService = LeadService;
            _contactService = contactService;
            _membershipService = membershipService;
            _roleService = roleService;
            _noteService = noteService;
            _activityService = activityService;
            _saleService = saleService;
            _initHelper = initHelper;
            _tagService = tagService;
        }

        public ActionResult RenderItems(string id, int? page, string order)
        {
            var campaigns = (List<Campaign>)TempData["SelectedCampaigns"];
            TempData.Keep("SelectedCampaigns");

            var tags = (List<Tag>)TempData["SelectedTags"];
            TempData.Keep("SelectedCampaigns");

            var options = new SaleFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower(),
                Campaigns = campaigns != null ? campaigns.ToList<object>() : null,
                Tags = tags != null ? tags.ToList<object>() : null
            };
            ViewBag.order = order;

            int pageNo = page ?? 1;
            int total = _saleService.Total(options);
            int totalPages = System.Convert.ToInt32(Math.Ceiling((decimal)total / (decimal)10));

            //default lead type
            if (string.IsNullOrEmpty(id))
                id = "New";

            if (totalPages > pageNo)
                ViewBag.Next = (pageNo + 1);

            var saleType = (SaleType)Enum.Parse(typeof(CrumbCRM.SaleType), id, true);
            var model = _saleService.GetAll(options, new PagingSettings() { PageCount = 10, PageIndex = pageNo }).Where(x => (x.Status.HasValue && x.Status.Value == saleType)).ToList();

            return PartialView("Controls/_RenderItems", model);
        }

        public ActionResult Index(string id, int? page, string order)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", new { id = "Prospecting" });

            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var campaigns = (List<Campaign>)TempData["SelectedCampaigns"];
            TempData.Keep("SelectedCampaigns");

            var tags = (List<Tag>)TempData["SelectedTags"];
            TempData.Keep("SelectedCampaigns");

            var options = new SaleFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower(),
                Campaigns = campaigns != null ? campaigns.ToList<object>() : null,
                Tags = tags != null ? tags.ToList<object>() : null
            };
            ViewBag.order = order;

            var model = new SaleViewModel();
            model.SaleType = (SaleType)Enum.Parse(typeof(CrumbCRM.SaleType), id, true);
            model.Sales = _saleService.GetAll(options).Where(x => (x.Status.HasValue && x.Status.Value == model.SaleType)).ToList();
            model.TotalProspecting = _saleService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == SaleType.Prospecting).Count();
            model.TotalQualified = _saleService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == SaleType.Qualified).Count();
            model.TotalUnqualified = _saleService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == SaleType.Unqualified).Count();
            model.TotalQuote = _saleService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == SaleType.Quote).Count();
            model.TotalClosure = _saleService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == SaleType.Closure).Count();
            model.TotalWon = _saleService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == SaleType.Won).Count();
            model.TotalLost = _saleService.GetAll(options).Where(x => x.Status.HasValue && x.Status.Value == SaleType.Lost).Count();

            ViewData.SelectListEnumViewData<NoteActionType>("ActionType", true);


            int pageNo = page ?? 1;
            int total = _saleService.Total(options);
            int totalPages = System.Convert.ToInt32(Math.Ceiling((decimal)total / (decimal)10));

            if (totalPages > pageNo)
                ViewBag.Next = (pageNo + 1);

            return View("Index", model);
        }

        public ActionResult Add()
        {
            _initHelper.InitializeMembers(ViewData, null);
            _initHelper.InitializeCompanies(ViewData, null);
            InitializePeople();

            ViewData.SelectListEnumViewData<PriorityType>("PriorityType", true);

            return View("Add");
        }

        [HttpPost]
        public ActionResult Add(Sale sale, FormCollection form, bool? convert)
        {
            bool isNew = false;

            //write activity to the log
            if (sale.ID == 0)
            {
                isNew = true;
                sale.Status = SaleType.Prospecting;
            }

            if(!string.IsNullOrEmpty(form["Companies"]))
                sale.CompanyID = Convert.ToInt32(form["Companies"]);

            if(!string.IsNullOrEmpty(form["People"]))
                sale.PersonID = Convert.ToInt32(form["People"]);

            if(!string.IsNullOrEmpty(form["Members"]))
                sale.OwnerID = new Guid(form["Members"]);

            if (!string.IsNullOrEmpty(form["PriorityType"]))
                sale.Priority = (PriorityType)Enum.Parse(typeof(CrumbCRM.PriorityType), form["PriorityType"], true);
           

            _saleService.Save(sale);

            if (!string.IsNullOrEmpty(form["Tags"]))
            {
                var current = _tagService.GetByArea(AreaType.Sale);
                string[] tags = form["Tags"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                tags.ToList().ForEach(t =>
                {
                    var tag = _tagService.GetByName(t);
                    if (tag == null)
                    {
                        tag = new Tag()
                        {
                            Name = t,
                            CreatedDate = DateTime.Now,
                            CreatedByID = _membershipService.GetCurrentMember().UserId
                        };
                        _tagService.Save(tag);
                    }
                    if(!current.Contains(tag))
                        _saleService.AssignTag(sale, tag);
                });
            }

            if(isNew)
                _activityService.Create("was added", AreaType.Sale, User.Identity.Name, sale.ID);

            TempData.Add("StatusMessage", "Sale added");

            return RedirectToAction("Index");
        }

        public ActionResult Update(int id, SaleType type)
        {
            var sale = _saleService.GetByID(id);
            sale.Status = type;

            _activityService.Create("status was updated " + type.ToString().AddSpacesToSentence(), AreaType.Sale, User.Identity.Name, sale.ID);

            _saleService.Save(sale);

            TempData.Add("StatusMessage", "Sales converted");
            return RedirectToAction("Index");
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
            Sale editSale = _saleService.GetByID(id);

            _initHelper.InitializeMembers(ViewData, null);
            _initHelper.InitializeCompanies(ViewData, editSale.CompanyID);
            InitializePeople(editSale.PersonID);

            List<dynamic> tags = new List<dynamic>();
            editSale.Tags.ToList().ForEach(t => tags.Add(new { id = t.TagID, name = t.Tag.Name }));

            ViewBag.CurrentTags = JsonConvert.SerializeObject(tags);
            ViewData.SelectListEnumViewData<PriorityType>("PriorityType", true);

            return View("Add", editSale);
        }

        public ActionResult View(int id)
        {
            if (TempData.Keys.Contains("Message"))
            {
                ViewBag.Message = TempData["Message"];
                TempData.Remove("Message");
            }

            var model = new SaleItemViewModel();
            model.Sale = _saleService.GetByID(id);
            model.Notes = _noteService.GetByType(id, NoteType.Sale);

            return View(model);
        }

        private void InitializeCompanies(int? id = null)
        {
            var companies = _contactService.GetAll(new ContactFilterOptions() { Type = ContactType.Company });
            ViewData.Add("Companies", new SelectList(companies, "ID", "CompanyName", id));
        }

        private void InitializePeople(int? id = null)
        {
            var people = _contactService.GetAll(new ContactFilterOptions() { Type = ContactType.Person });
            ViewData.Add("People", new SelectList(people, "ID", "FullName", id));
        }
    }
}
