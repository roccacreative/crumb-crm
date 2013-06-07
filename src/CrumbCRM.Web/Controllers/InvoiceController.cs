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
    public class InvoiceController : Controller
    {
        private readonly ILeadService _leadService;
        private readonly IContactService _contactService;
        private readonly IMembershipService _membershipService;
        private readonly IRoleService _roleService;
        private readonly INoteService _noteService;
        private readonly IActivityService _activityService;
        private readonly IInvoiceService _invoiceService;
        private readonly ISaleService _saleService;
        private readonly ViewDataHelper _initHelper;

        public InvoiceController(
            ILeadService LeadService,
            IContactService contactService,
            IMembershipService membershipService,
            IRoleService roleService,
            INoteService noteService,
            IActivityService activityService,
            IInvoiceService invoiceService,
            ISaleService saleService,
            ViewDataHelper initHelper)
        {
            _leadService = LeadService;
            _contactService = contactService;
            _membershipService = membershipService;
            _roleService = roleService;
            _noteService = noteService;
            _activityService = activityService;
            _saleService = saleService;
            _invoiceService = invoiceService;
            _initHelper = initHelper;
        }



        public ActionResult RenderItems(int? page, string order)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var options = new InvoiceFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower()
            };
            ViewBag.order = order;

            int pageNo = page ?? 1;
            int total = _invoiceService.Total(options);
            int totalPages = System.Convert.ToInt32(Math.Ceiling((decimal)total / (decimal)10));

            if (totalPages > pageNo)
                ViewBag.Next = (pageNo + 1);

            var model = _invoiceService.GetAll(options, new PagingSettings() { PageCount = 10, PageIndex = pageNo });

            return PartialView("Controls/_RenderItems", model);
        }

        public ActionResult Index(string order)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var options = new InvoiceFilterOptions()
            {
                Order = string.IsNullOrEmpty(order) ? string.Empty : order.ToLower()
            };
            ViewBag.order = order;

            List<Invoice> model = _invoiceService.GetAll(options);
            return View(model);
        }


        public ActionResult Add() {
            return View("Add");
        }

        [HttpPost]
        public ActionResult Add(Invoice invoice, FormCollection form)
        {

            //write activity to the log
            if (invoice.ID == 0)
                _activityService.Create("was generated", AreaType.Invoice, User.Identity.Name, invoice.ID);

            _invoiceService.Save(invoice);

            TempData.Add("StatusMessage", "Invoice added");
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Invoice invoice = _invoiceService.GetByID(id);
            _invoiceService.Delete(invoice);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Invoice editInvoice = _invoiceService.GetByID(id);
            return View("Add", editInvoice);
        }
        public ActionResult Print(int id)
        {
            Invoice invoice = _invoiceService.GetByID(id);
            invoice.Items = _invoiceService.GetItemsByInvoiceID(invoice.ID);

            byte[] pdf = invoice.Print();
            return File(pdf, "application/pdf", invoice.InvoiceNumber + ".pdf");
        }
    }

}
