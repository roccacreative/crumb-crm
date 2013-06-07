using CrumbCRM.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace CrumbCRM.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        [Inject]
        public IRoleService RoleService { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            RoleService.AddUsersToRoles(usernames, roleNames);
        }

        public override string ApplicationName
        {
            get
            {
                return RoleService.ApplicationName;
            }
            set
            {
                RoleService.ApplicationName = value;
            }
        }

        public override void CreateRole(string roleName)
        {
            RoleService.CreateRole(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return RoleService.DeleteRole(roleName, throwOnPopulatedRole);
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return RoleService.FindUsersInRole(roleName, usernameToMatch);
        }

        public override string[] GetAllRoles()
        {
            return RoleService.GetAllRoles();
        }

        public override string[] GetRolesForUser(string username)
        {
            return RoleService.GetRolesForUser(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return RoleService.GetUsersInRole(roleName);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return RoleService.IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            RoleService.RemoveUsersFromRoles(usernames, roleNames);
        }

        public override bool RoleExists(string roleName)
        {
            return RoleService.RoleExists(roleName);
        }
    }
}
