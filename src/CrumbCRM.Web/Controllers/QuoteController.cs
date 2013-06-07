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
    public class QuoteController : Controller
    {
        private readonly ILeadService _leadService;
        private readonly IContactService _contactService;
        private readonly IMembershipService _membershipService;
        private readonly IRoleService _roleService;
        private readonly INoteService _noteService;
        private readonly IActivityService _activityService;
        private readonly IQuoteService _quoteService;
        private readonly IInvoiceService _invoiceService;

        public QuoteController(
            ILeadService LeadService, 
            IContactService contactService,
            IMembershipService membershipService,
            IRoleService roleService,
            INoteService noteService,
            IActivityService activityService,
            IQuoteService quoteService,
            IInvoiceService invoiceService)
        {
            _leadService = LeadService;
            _contactService = contactService;
            _membershipService = membershipService;
            _roleService = roleService;
            _noteService = noteService;
            _activityService = activityService;
            _quoteService = quoteService;
            _invoiceService = invoiceService;
        }
               

        public ActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public ActionResult Add(Quote quote, FormCollection form)
        {
            //write activity to the log
            if (quote.ID == 0)
                _activityService.Create("was added", AreaType.Quote, User.Identity.Name, quote.ID);

            _quoteService.Save(quote);

            TempData.Add("StatusMessage", "Quote added");

            return RedirectToAction("Index");
        }


        public ActionResult RenderItems(int? page, string order)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;
            var options = new QuoteFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower()
            };

            ViewBag.order = order;

            int pageNo = page ?? 1;
            int total = _quoteService.Total(options);
            int totalPages = System.Convert.ToInt32(Math.Ceiling((decimal)total / (decimal)10));

            if (totalPages > pageNo)
                ViewBag.Next = (pageNo + 1);

            var model = _quoteService.GetAll(options, new PagingSettings() { PageCount = 10, PageIndex = pageNo });

            return PartialView("Controls/_RenderItems", model);
        }

        public ActionResult Index(string order)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;
            var options = new QuoteFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower()
            };

            ViewBag.order = order;

            List<Quote> model = _quoteService.GetAll(options);
            return View(model);
        }

        public ActionResult Delete(int id, FormCollection form) 
        {
            Quote quote = _quoteService.GetByID(id);
            _quoteService.Delete(quote);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)    
        {
            Quote editQuote = _quoteService.GetByID(id);
            return View("Add", editQuote); 
        }

        public ActionResult View(int id)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var model = new QuoteItemViewModel();
            model.Quote = _quoteService.GetByID(id);
            model.Notes = _noteService.GetByType(id, NoteType.Lead);
            model.QuoteItems = _quoteService.GetItemsByQuoteID(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult AddItem(QuoteItem quoteItem, FormCollection form)
        {
            var quoteID = form["QuoteID"];
            quoteItem.QuoteID = Convert.ToInt32(quoteID);

            _quoteService.SaveItem(quoteItem);
            TempData.Add("StatusMessage", "Quote Item added");
            return RedirectToAction("View", new { id = quoteItem.QuoteID });
        }

        public JsonResult GenerateInvoice(FormCollection form)
        {
            Invoice invoice = new Invoice();
            //convert quote into an invoice
            invoice.Title = form["Title"];
            invoice.Description = form["Description"];
            invoice.InvoiceNumber = Convert.ToInt32(form["InvoiceNumber"]);
            invoice.PurchaseOrder = form["PurchaseOrder"];
            invoice.ExcludingVAT = Convert.ToBoolean(form["ExcludeVAT"]);

            invoice.SubValue = _quoteService.GetItemsByQuoteID(Convert.ToInt32(form["QuoteID"])).Sum(q => q.Value);

            if (invoice.ExcludingVAT == false)
            {
                //add on vat 
                invoice.TotalValue = invoice.SubValue + invoice.SubValue * (decimal)0.20;
            }
            else
            {
                invoice.TotalValue = invoice.SubValue;
            }

            //save the new invoice
            _invoiceService.Save(invoice);

            //save invoice items
            List<QuoteItem> quoteItems = _quoteService.GetItemsByQuoteID(Convert.ToInt32(form["QuoteID"]));

            foreach (var item in quoteItems)
            {
                //popular invoice items with quote item data
                InvoiceItem invoiceItem = new InvoiceItem();
                invoiceItem.InvoiceID = invoice.ID;
                invoiceItem.Title = item.Title;
                invoiceItem.Value = item.Value;
                _invoiceService.SaveItem(invoiceItem);

                //remove quote items
                _quoteService.DeleteItem(item);
            }
                
            //remove quote
            Quote quote = _quoteService.GetByID(Convert.ToInt32(form["QuoteID"]));
            _quoteService.Delete(quote);

            return Json(new { success = true });
        }
     

        public ActionResult EditItem(int id)
        {
            QuoteItem editQuoteItem = _quoteService.GetItemByID(id);
            return View("AddItem", editQuoteItem);
        }

        public ActionResult DisplayQuoteItems(int id)
        {
            return View("Controls/_QuoteItemsList", _quoteService.GetItemsByQuoteID(id));
        }

        public ActionResult DeleteItem(int id)
        {
            QuoteItem quoteItem = _quoteService.GetItemByID(id);
            _quoteService.DeleteItem(quoteItem);
            return RedirectToAction("View", new { id = quoteItem.QuoteID });
        }

    }
}
