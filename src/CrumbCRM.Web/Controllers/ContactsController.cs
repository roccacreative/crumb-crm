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
    public class ContactsController : Controller
    {
        #region Services
        
        private readonly INoteService _noteService;
        private readonly IContactService _contactService;
        private readonly ILeadService _leadService;
        private readonly IQuoteService _quoteService;
        private readonly IMembershipService _membershipService;
        private readonly IActivityService _activityService;
        private readonly ViewDataHelper _initHelper;

        #endregion

        public ContactsController(
            INoteService noteService, 
            IContactService contactService, 
            ILeadService leadService, 
            IQuoteService quoteService,
            IMembershipService membershipService,
            IActivityService activityService,
            ViewDataHelper initHelper)
        {
            _noteService = noteService;
            _contactService = contactService;
            _leadService = leadService;
            _quoteService = quoteService;
            _membershipService = membershipService;
            _activityService = activityService;
            _initHelper = initHelper;
        }

        
        public ActionResult Add()
        {
            _initHelper.InitializeMembers(ViewData, null);
            _initHelper.InitializeCompanies(ViewData, null);

            return View("Add");
        }

        public ActionResult AddCompany()
        {
            _initHelper.InitializeMembers(ViewData, null);     
            return View("AddCompany");
        }

        [HttpPost]
        public ActionResult Add(Contact contact, FormCollection form)
        {
            bool isNew = false;

            //write activity to the log
            if (contact.ID == 0)
                isNew = true;                

            contact.OwnerID = new Guid(form["Members"].ToString());            

            if (contact.Type == ContactType.Person && !string.IsNullOrEmpty(contact.CompanyName))
            {
                Contact company = new Contact();

                //add a company contact       
                company.Type = ContactType.Company;
                company.CompanyName = contact.CompanyName;
                company.JobTitle = contact.JobTitle;
                company.Email = contact.Email;
                company.Mobile = contact.Mobile;
                company.Work = contact.Work;
                company.Address = contact.Address;
                company.City = contact.City;
                company.County = contact.County;
                company.Postcode = contact.Postcode;
                company.OwnerID = new Guid(form["Members"].ToString());

                _contactService.Save(company);

                contact.CompanyName = string.Empty;
                contact.CompanyID = company.ID;
            }   
         
            _contactService.Save(contact);

            if (isNew)
                _activityService.Create("was added", AreaType.Contact, User.Identity.Name, contact.ID);

            TempData.Add("StatusMessage", "Contact added");
            return RedirectToAction("Index");
        }

        public ActionResult RenderItems(int? page, string order)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var options = new ContactFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower()
            };
            ViewBag.order = order;

            int pageNo = page ?? 1;
            int total = _contactService.Total(options);
            int totalPages = System.Convert.ToInt32(Math.Ceiling((decimal)total / (decimal)10));

            if (totalPages > pageNo)
                ViewBag.Next = (pageNo + 1);

            var model = _contactService.GetAll(options, new PagingSettings() { PageCount = 10, PageIndex = pageNo });

            return PartialView("Controls/_RenderItems", model);
        }

        public ActionResult Index(string order)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var options = new ContactFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower()
            };
            ViewBag.order = order;

            List<Contact> model = _contactService.GetAll(options);
            return View(model);
        }

        public ActionResult Delete(int id) 
        {
            Contact contact = _contactService.GetByID(id);
            _contactService.Delete(contact);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)    
        {
            Contact editContact = _contactService.GetByID(id);

            _initHelper.InitializeMembers(ViewData, editContact.OwnerID);
            _initHelper.InitializeCompanies(ViewData, editContact.CompanyID);

            if(editContact.Type == ContactType.Person)
                return View("Add", editContact); 
            else
                return View("AddCompany", editContact); 
        }

    }
}
