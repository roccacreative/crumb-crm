using CrumbCRM.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace CrumbCRM.Data
{
    public interface IMembershipRepository : IDisposable
    {
        string ApplicationName { get; set; }
        int MaxInvalidPasswordAttempts { get; }
        int MinRequiredNonAlphanumericCharacters { get; }
        int MinRequiredPasswordLength { get; }
        int PasswordAttemptWindow { get; }
        MembershipPasswordFormat PasswordFormat { get; }
        string PasswordStrengthRegularExpression { get; }
        bool RequiresUniqueEmail { get; }

        bool ChangePassword(string username, string oldPassword, string newPassword);
        MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status);
        bool DeleteUser(string username, bool deleteAllRelatedData);
        MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);
        MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);
        MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords);
        int GetNumberOfUsersOnline();
        MembershipUser GetUser(object providerUserKey, bool userIsOnline);
        MembershipUser GetUser(string username, bool userIsOnline);
        string GetUserNameByEmail(string email);
        bool UnlockUser(string userName);
        bool ValidateUser(string username, string password, bool? autoLogin = false);

        Guid CreateMembershipRole(string name, string description);

        List<User> GetUsersInRole(string role);

        List<User> GetAllUsers();

        User GetByUsername(string username);
    }
}
