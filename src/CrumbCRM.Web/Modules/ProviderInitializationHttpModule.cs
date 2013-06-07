using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CrumbCRM.Web.Modules
{
    public class ProviderInitializationHttpModule : IHttpModule
    {
        public ProviderInitializationHttpModule(MembershipProvider membershipProvider, RoleProvider roleProvider)
        {
            MembershipCreateStatus createStatus;

            Roles.CreateRole("Administrator");

            string username = ConfigurationManager.AppSettings["default:username"];
            string password = ConfigurationManager.AppSettings["default:password"];
            string email = ConfigurationManager.AppSettings["default:email"];

            Membership.CreateUser(username, password, email, null, null, true, null, out createStatus);
            Roles.AddUserToRole("admin", "Administrator");
        }

        public void Init(HttpApplication context)
        {
        }

        public void Dispose()
        {
        }
    }
}