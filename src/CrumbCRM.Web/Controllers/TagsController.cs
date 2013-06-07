using CrumbCRM.Enums;
using CrumbCRM.Filters;
using CrumbCRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrumbCRM.Web.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMembershipService _membershipService;

        public TagsController(
            ITagService tagService,
            IMembershipService membershipService)
        {
            _tagService = tagService;
            _membershipService = membershipService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTags(string q)
        {
            List<dynamic> tags = new List<dynamic>();

            foreach (var item in _tagService.GetAll(new TagFilterOptions() { SearchTerm = q}))
            {
                tags.Add(new { id = item.ID, name = item.Name });
            }

            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(string name)
        {
            Tag tag = _tagService.GetByName(name);
            
            if (tag == null)
            {
                tag = new Tag()
                {
                    Name = name,
                    CreatedDate = DateTime.Now,
                    CreatedByID = _membershipService.GetCurrentMember().UserId,                    
                };

                _tagService.Save(tag);
            }

            return Json(new { success = true });
        }

        public ActionResult DisplaySideTags(AreaType? areaType)
        {
            var userId = _membershipService.GetCurrentMember().UserId;
            List<Tag> model = _tagService.GetByArea(areaType);

            ViewBag.SelectedTags = TempData["SelectedTags"];
            TempData.Keep("SelectedTags");

            return PartialView("Controls/_TagsSideList", model);
        }

        public ActionResult SelectTag(int id)
        {
            List<Tag> tags = (List<Tag>)TempData["SelectedTags"];

            if (tags == null)
                tags = new List<Tag>();

            tags.Add(_tagService.GetByID(id));

            TempData["SelectedTags"] = tags;
            TempData.Keep("SelectedTags");

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult RemoveTag(int id)
        {
            List<Tag> tags = (List<Tag>)TempData["SelectedTags"];

            if (tags == null)
                tags = new List<Tag>();

            var tag = tags.FirstOrDefault(t => t.ID == id);
            tags.Remove(tag);

            if (tags.Count == 0)
                TempData.Remove("SelectedTags");
            else
            {
                TempData["SelectedTags"] = tags;
                TempData.Keep("SelectedTags");
            }


            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult ResetSelected()
        {
            TempData.Remove("SelectedTags");
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
