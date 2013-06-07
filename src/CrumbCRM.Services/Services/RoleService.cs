using CrumbCRM.Data;
using CrumbCRM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Services.Services
{
    public class RoleService : ServiceBase<IRoleRepository>, IRoleService
    {
        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            Repository.AddUsersToRoles(usernames, roleNames);
        }

        public string ApplicationName
        {
            get
            {
                return Repository.ApplicationName;
            }
            set
            {
                Repository.ApplicationName = value;
            }
        }

        public void CreateRole(string roleName)
        {
            Repository.CreateRole(roleName);
        }

        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return Repository.DeleteRole(roleName, throwOnPopulatedRole);
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return Repository.FindUsersInRole(roleName, usernameToMatch);
        }

        public string[] GetAllRoles()
        {
            return Repository.GetAllRoles();
        }

        public string[] GetRolesForUser(string username)
        {
            return Repository.GetRolesForUser(username);
        }

        public string[] GetUsersInRole(string roleName)
        {
            return Repository.GetUsersInRole(roleName);
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return Repository.IsUserInRole(username, roleName);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            Repository.RemoveUsersFromRoles(usernames, roleNames);
        }

        public bool RoleExists(string roleName)
        {
            return Repository.RoleExists(roleName);
        }
    }
}
