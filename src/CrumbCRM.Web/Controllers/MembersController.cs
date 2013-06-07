using CrumbCRM.Services;
using CrumbCRM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CrumbCRM.Web.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly IRoleService _roleService;

        public MembersController(
            IMembershipService membershipService,
            IRoleService roleService)
        {
            _membershipService = membershipService;
            _roleService = roleService;
        }

        public ActionResult Index()
        {
            var model = new MembersViewModel();
            model.Users = _membershipService.GetAllUsers();

            return View(model);
        }

        public ActionResult Delete(string id)
        {
            _membershipService.DeleteUser(id, true);
            return RedirectToAction("Index");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CreateMemberModel model)
        {
            MembershipCreateStatus status;
            _membershipService.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out status);
            _roleService.AddUsersToRoles(new[] { model.UserName }, new[] { "Administrator" });

            return RedirectToAction("Index");
        }
    }
}
