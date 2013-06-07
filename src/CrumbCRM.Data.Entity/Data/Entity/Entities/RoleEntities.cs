using CrumbCRM.Data;
using CrumbCRM.Data.Entity.Entities;
using CrumbCRM.Data.Entity.Model;
using CrumbCRM.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Data.Entity.Entities
{
    public class RoleEntities : EntitiesBase<CrumbCRMEntities>, IRoleRepository
    {
        public string ApplicationName
        {
            get
            {
                return this.GetType().Assembly.GetName().Name.ToString();
            }
            set
            {
                this.ApplicationName = this.GetType().Assembly.GetName().Name.ToString();
            }
        }

        public bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {
                Role Role = null;
                Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (Role != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsUserInRole(string username, string roleName)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.Username == username);
                if (User == null)
                {
                    return false;
                }
                Role Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (Role == null)
                {
                    return false;
                }
                return User.Roles.Contains(Role);
            }
        }

        public string[] GetAllRoles()
        {
            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {
                return Context.Roles.Select(Rl => Rl.RoleName).ToArray();
            }
        }

        public string[] GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return null;
            }
            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {
                Role Role = null;
                Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (Role != null)
                {
                    return Role.Users.Select(Usr => Usr.Username).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        public string[] GetRolesForUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {
                User User = null;
                User = Context.Users.Include("Roles").FirstOrDefault(Usr => Usr.Username == username);
                if (User != null)
                {
                    return User.Roles.Select(Rl => Rl.RoleName).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return null;
            }

            if (string.IsNullOrEmpty(usernameToMatch))
            {
                return null;
            }

            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {

                return (from Rl in Context.Roles from Usr in Rl.Users where Rl.RoleName == roleName && Usr.Username.Contains(usernameToMatch) select Usr.Username).ToArray();
            }
        }

        public void CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                using (CrumbCRMEntities Context = new CrumbCRMEntities())
                {
                    Role Role = null;
                    Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                    if (Role == null)
                    {
                        Role NewRole = new Role
                        {
                            RoleId = Guid.NewGuid(),
                            RoleName = roleName
                        };
                        Context.Roles.Add(NewRole);
                        Context.SaveChanges();
                    }
                }
            }
        }

        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }
            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {
                Role Role = null;
                Role = Context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                if (Role == null)
                {
                    return false;
                }
                if (throwOnPopulatedRole)
                {
                    if (Role.Users.Any())
                    {
                        return false;
                    }
                }
                else
                {
                    Role.Users.Clear();
                }
                Context.Roles.Remove(Role);
                Context.SaveChanges();
                return true;
            }
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {
                List<User> Users = Context.Users.Include("Roles").Where(Usr => usernames.Contains(Usr.Username)).ToList();
                List<Role> Roles = Context.Roles.Where(Rl => roleNames.Contains(Rl.RoleName)).ToList();
                foreach (User user in Users)
                {
                    foreach (Role role in Roles)
                    {
                        if (!user.Roles.Contains(role))
                        {
                            user.Roles.Add(role);
                        }
                    }
                }
                Context.SaveChanges();
            }
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (CrumbCRMEntities Context = new CrumbCRMEntities())
            {
                foreach (String username in usernames)
                {
                    String us = username;
                    User user = Context.Users.FirstOrDefault(U => U.Username == us);
                    if (user != null)
                    {
                        foreach (String roleName in roleNames)
                        {
                            String rl = roleName;
                            Role role = user.Roles.FirstOrDefault(R => R.RoleName == rl);
                            if (role != null)
                            {
                                user.Roles.Remove(role);
                            }
                        }
                    }
                }
                Context.SaveChanges();
            }
        }
    }
}
