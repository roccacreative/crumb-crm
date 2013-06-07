using CrumbCRM.Security;
using CrumbCRM.Services;
using CrumbCRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace CrumbCRM.Services.Services
{
    public class MembershipService : ServiceBase<IMembershipRepository>, IMembershipService
    {
        #region Properties

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

        public int MaxInvalidPasswordAttempts
        {
            get { return Repository.MaxInvalidPasswordAttempts; }
        }

        public int MinRequiredNonAlphanumericCharacters
        {
            get { return Repository.MinRequiredNonAlphanumericCharacters; }
        }

        public int MinRequiredPasswordLength
        {
            get { return Repository.MinRequiredPasswordLength; }
        }

        public int PasswordAttemptWindow
        {
            get { return Repository.PasswordAttemptWindow; }
        }

        public MembershipPasswordFormat PasswordFormat
        {
            get { return Repository.PasswordFormat; }
        }

        public string PasswordStrengthRegularExpression
        {
            get { return Repository.PasswordStrengthRegularExpression; }
        }

        public bool RequiresUniqueEmail
        {
            get { return Repository.RequiresUniqueEmail; }
        }

        #endregion

        public MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            return Repository.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
        }

        public bool ValidateUser(string username, string password, bool? autoLogin = false)
        {
            return Repository.ValidateUser(username, password, autoLogin);
        }

        public MembershipUser GetUser(string username, bool userIsOnline)
        {
            return Repository.GetUser(username, userIsOnline);
        }

        public MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return Repository.GetUser(providerUserKey, userIsOnline);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return Repository.ChangePassword(username, oldPassword, newPassword);
        }

        public bool UnlockUser(string userName)
        {
            return Repository.UnlockUser(userName);
        }

        public int GetNumberOfUsersOnline()
        {
            return Repository.GetNumberOfUsersOnline();
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return Repository.DeleteUser(username, deleteAllRelatedData);
        }

        public string GetUserNameByEmail(string email)
        {
            return Repository.GetUserNameByEmail(email);
        }

        public MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return Repository.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        public MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return Repository.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
        }

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return Repository.GetAllUsers(pageIndex, pageSize, out totalRecords);
        }

        public Guid CreateMembershipRole(string name, string description)
        {
            return Repository.CreateMembershipRole(name, description);
        }

        public List<User> GetUsersInRole(string role)
        {
            return Repository.GetUsersInRole(role);
        }

        public List<User> GetAllUsers()
        {
            return Repository.GetAllUsers();
        }

        public User GetByUsername(string username)
        {
            return Repository.GetByUsername(username);
        }

        public User GetCurrentMember()
        {
            return GetByUsername(HttpContext.Current.User.Identity.Name);
        }
    }
}
