using CrumbCRM.Enums;
using CrumbCRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrumbCRM.Web.Controllers
{
    [Authorize]
    public class CampaignsController : Controller
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(
            ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult DisplaySideCampaigns()
        {
            var model = _campaignService.GetAll();

            ViewBag.SelectedCampaigns = (List<Campaign>)TempData["SelectedCampaigns"];
            TempData.Keep("SelectedCampaigns");

            return PartialView("Controls/_CampaignsSideList", model);
        }

        public ActionResult SelectCampaign(int id)
        {
            List<Campaign> campaigns = (List<Campaign>)TempData["SelectedCampaigns"];

            if (campaigns == null)
                campaigns = new List<Campaign>();

            campaigns.Add(_campaignService.GetByID(id));

            TempData["SelectedCampaigns"] = campaigns;

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult RemoveCampaign(int id)
        {
            List<Campaign> campaigns = (List<Campaign>)TempData["SelectedCampaigns"];
            if (campaigns == null)
                campaigns = new List<Campaign>();

            var campaign = campaigns.FirstOrDefault(t => t.ID == id);
            campaigns.Remove(campaign);

            if (campaigns.Count > 0)
            {
                TempData["SelectedCampaigns"] = campaigns;
                TempData.Keep("SelectedCampaigns");
            }
            else
            {
                TempData["SelectedCampaigns"] = null;
                TempData.Remove("SelectedCampaigns");
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult ResetSelected()
        {
            TempData.Remove("SelectedCampaigns");
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
