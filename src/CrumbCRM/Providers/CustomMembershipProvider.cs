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
    public class CustomMembershipProvider : MembershipProvider
    {
        [Inject]
        public IMembershipService MembershipService { get; set; }

        #region Properties

        public override string ApplicationName
        {
            get
            {
                return MembershipService.ApplicationName;
            }
            set
            {
                MembershipService.ApplicationName = value;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return MembershipService.MaxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return MembershipService.MinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return MembershipService.MinRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return MembershipService.PasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipService.PasswordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return MembershipService.PasswordStrengthRegularExpression; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return MembershipService.RequiresUniqueEmail; }
        }

        #endregion

        #region Functions

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            return MembershipService.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
        }

        public override bool ValidateUser(string username, string password)
        {
            return MembershipService.ValidateUser(username, password);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return MembershipService.GetUser(username, userIsOnline);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return MembershipService.GetUser(providerUserKey, userIsOnline);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return MembershipService.ChangePassword(username, oldPassword, newPassword);
        }

        public override bool UnlockUser(string userName)
        {
            return MembershipService.UnlockUser(userName);
        }

        public override int GetNumberOfUsersOnline()
        {
            return MembershipService.GetNumberOfUsersOnline();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return MembershipService.DeleteUser(username, deleteAllRelatedData);
        }

        public override string GetUserNameByEmail(string email)
        {
            return MembershipService.GetUserNameByEmail(email);
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return MembershipService.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return MembershipService.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return MembershipService.GetAllUsers(pageIndex, pageSize, out totalRecords);
        }

        #endregion

        #region Not Supported

        //CodeFirstMembershipProvider does not support password retrieval scenarios.
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support password reset scenarios.
        public override bool EnablePasswordReset
        {
            get { return false; }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support question and answer scenarios.
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support UpdateUser because this method is useless.
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
