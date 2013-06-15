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
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IContactService _contactService;
        private readonly ILeadService _leadService;
        private readonly IQuoteService _quoteService;
        private readonly IMembershipService _membershipService;

        public NoteController(
            INoteService NoteService, 
            IContactService contactService, 
            ILeadService leadService, 
            IQuoteService quoteService,
            IMembershipService membershipService)
        {
            _noteService = NoteService;
            _contactService = contactService;
            _leadService = leadService;
            _quoteService = quoteService;
            _membershipService = membershipService;
        }

        public ActionResult Delete(int id)
        {
            Note note = _noteService.GetByID(id);
            _noteService.Delete(note);

            TempData.Add("Message", "Note deleted");

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(NoteViewModel model, FormCollection form)
        {
            model.Note.ItemID = model.ItemID;
            model.Note.Type = model.NoteType;
            model.Note.AuthorID = _membershipService.GetCurrentMember().UserId;
            model.Note.Action = (NoteActionType)Convert.ToInt32(form["ActionType"]);
            _noteService.Save(model.Note);

            TempData.Add("Message", "Note added");

            int itemID = model.Note.ItemID;
            int type = (int)model.Note.Type;

            switch (model.NoteType)
            {
                case NoteType.Lead:
                    return RedirectToAction("View", "Lead", new { id = model.ItemID });
                case NoteType.Contact:
                    return RedirectToAction("View", "Contact", new { id = model.ItemID });
                case NoteType.Sale:
                    return RedirectToAction("View", "Sale", new { id = model.ItemID });
                case NoteType.Quote:
                    return RedirectToAction("View", "Quote", new { id = model.ItemID });
                default:
                    return Redirect(Request.UrlReferrer.AbsoluteUri);
            }
        }

        public JsonResult AddNote(Note note, FormCollection form)
        {
            note.AuthorID = _membershipService.GetCurrentMember().UserId;
            _noteService.Save(note);

            return Json(new { success = true });
        }
     
        public ActionResult Add(int id, string type)
        {
            var model = new NoteViewModel();
            model.ItemID = id;
            model.NoteType = (NoteType)Enum.Parse(typeof(CrumbCRM.NoteType), type, true);
            model.ParentItem = _leadService.GetByID(model.ItemID);

            switch (model.NoteType)
            {
                case NoteType.Contact:
                    model.ParentItem = _contactService.GetByID(id);
                    break;
                case NoteType.Quote:
                    model.ParentItem = _quoteService.GetByID(id);
                    break;
                default:
                    model.ParentItem = _leadService.GetByID(id);
                    break;
            }

            ViewData.SelectListEnumViewData<NoteActionType>("ActionType", true);

            return View("Add", model);
        }


        public ActionResult Edit(int id)    
        {
            var model = new NoteViewModel();
            model.Note = _noteService.GetByID(id);
            var noteType = model.Note.Type;
            model.ItemID = model.Note.ItemID;
            model.NoteType = model.Note.Type; 
            switch (noteType)
            {
                case NoteType.Contact:
                    model.ParentItem = _contactService.GetByID(model.Note.ItemID);
                    break;
                case NoteType.Quote:
                    model.ParentItem = _quoteService.GetByID(model.Note.ItemID);
                    break;
                default:
                    model.ParentItem = _leadService.GetByID(model.Note.ItemID);
                    break;
            }

            ViewData.SelectListEnumViewData<NoteActionType>("ActionType", true, (int?)model.Note.Action);

            return View("Add", model); 
        }

        public ActionResult View(int id, string type)
        {
            string message = (string)TempData["StatusMessage"];
            ViewBag.message = message;

            var model = new NoteViewModel();
            var noteType = (NoteType)Enum.Parse(typeof(CrumbCRM.NoteType), type, true);
            model.Notes = _noteService.GetByType(id, noteType);

            switch (noteType)
            {
                case NoteType.Contact:
                    model.ParentItem = _contactService.GetByID(id);
                    break;
                case NoteType.Quote:
                    model.ParentItem = _quoteService.GetByID(id);
                    break;
                default:
                    model.ParentItem = _leadService.GetByID(id);
                    break;
            }
            
            model.NoteType = (NoteType)Enum.Parse(typeof(CrumbCRM.NoteType), type, true);
                       
            return View("Index", model);
        }

        public ActionResult DisplayNotes(int id, NoteType type)
        {
            return View("Controls/_NotesList", _noteService.GetByType(id, type));
        }

    }
}
