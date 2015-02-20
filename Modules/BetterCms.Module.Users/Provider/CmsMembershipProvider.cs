using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.Services;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Dependencies;

namespace BetterCms.Module.Users.Provider
{
    public class CmsMembershipProvider : System.Web.Security.MembershipProvider
    {
        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// The name of the application using the custom membership provider.
        /// </summary>
        /// <returns>The name of the application using the custom membership provider.</returns>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the name of the member ship.
        /// </summary>
        /// <value>
        /// The name of the member ship.
        /// </value>
        private readonly string membershipName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsMembershipProvider" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="membershipName">Name of the membership.</param>
        internal CmsMembershipProvider(IUserService userService, IAuthenticationService authenticationService, IUnitOfWork unitOfWork, string membershipName)
        {
            this.authenticationService = authenticationService;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
            this.membershipName = membershipName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsMembershipProvider" /> class.
        /// </summary>
// ReSharper disable UnusedMember.Global
        public CmsMembershipProvider()
// ReSharper restore UnusedMember.Global
        {
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        public override bool ValidateUser(string username, string password)
        {
            if (authenticationService == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return ValidateUser(container.Resolve<IAuthenticationService>(), username, password);
                }
            }

            return ValidateUser(authenticationService, username, password);
        }

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (authenticationService == null || unitOfWork == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return ChangePassword(container.Resolve<IAuthenticationService>(), container.Resolve<IUnitOfWork>(), username, oldPassword, newPassword);
                }
            }

            return ChangePassword(authenticationService, unitOfWork, username, oldPassword, newPassword);
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to search for.</param>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
        public override string GetUserNameByEmail(string email)
        {
            if (userService == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return GetUserNameByEmail(container.Resolve<IUserService>(), email);
                }
            }

            return GetUserNameByEmail(userService, email);
        }

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (userService == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return GetAllUsers(container.Resolve<IUserService>(), pageIndex, pageSize, out totalRecords);
                }
            }

            return GetAllUsers(userService, pageIndex, pageSize, out totalRecords);
        }


        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            if (userService == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return FindUsersByName(container.Resolve<IUserService>(), usernameToMatch, pageIndex, pageSize, out totalRecords);
                }
            }

            return FindUsersByName(userService, usernameToMatch, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            if (userService == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return FindUsersByEmail(container.Resolve<IUserService>(), emailToMatch, pageIndex, pageSize, out totalRecords);
                }
            }

            return FindUsersByEmail(userService, emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>
        /// The password for the specified user name.
        /// </returns>
        public override string GetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (userService == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return GetUser(container.Resolve<IUserService>(), username);
                }
            }

            return GetUser(userService, username);
        }

        /// <summary>
        /// Processes a request to update the password question and answer for a membership user.
        /// </summary>
        /// <param name="username">The user to change the password question and answer for.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
        /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
        /// <returns>
        /// true if the password question and answer are updated successfully; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="username">The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus" /> enumeration value indicating whether the user was created successfully.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the information for the newly created user.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override System.Web.Security.MembershipUser CreateUser(
            string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out System.Web.Security.MembershipCreateStatus status)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        /// <returns>
        /// true if the user was successfully deleted; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <returns>true if the membership provider supports password reset; otherwise, false. The default is true.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool EnablePasswordReset
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <returns>true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int GetNumberOfUsersOnline()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <returns>The number of invalid password or password-answer attempts allowed before the membership user is locked out.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>The minimum number of special characters that must be present in a valid password.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <returns>The minimum length required for a password. </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <returns>The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int PasswordAttemptWindow
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.Security.MembershipPasswordFormat" /> values indicating the format for storing passwords in the data store.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>A regular expression used to evaluate a password.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <returns>true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <returns>true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>
        /// The new password for the specified user.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string ResetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="userName">The membership user whose lock status you want to clear.</param>
        /// <returns>
        /// true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool UnlockUser(string userName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">A <see cref="T:System.Web.Security.MembershipUser" /> object that represents the user to update and the updated information for the user.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new System.NotImplementedException();
        }

        private static bool ValidateUser(IAuthenticationService authenticationService, string username, string password)
        {
            return authenticationService.ValidateUser(username, password);
        }

        private static bool ChangePassword(IAuthenticationService authenticationService, IUnitOfWork unitOfWork, string username, string oldPassword, string newPassword)
        {
            unitOfWork.BeginTransaction();
            var success = authenticationService.ChangePassword(username, oldPassword, newPassword);
            unitOfWork.Commit();

            return success;
        }

        private System.Web.Security.MembershipUserCollection FindUsersByEmail(
            IUserService service, string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var users = service.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);

            return ToMembershipUserCollection(users);
        }

        private System.Web.Security.MembershipUserCollection FindUsersByName(
            IUserService service, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var users = service.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);

            return ToMembershipUserCollection(users);
        }

        private System.Web.Security.MembershipUser GetUser(
            IUserService service, string username)
        {
            var user = service.GetUser(username);

            return ToMembershipUser(user);
        }

        private System.Web.Security.MembershipUserCollection GetAllUsers(IUserService service, int pageIndex, int pageSize, out int totalRecords)
        {
            var users = service.GetAllUsers(pageIndex, pageSize, out totalRecords);

            return ToMembershipUserCollection(users);
        }

        private static string GetUserNameByEmail(IUserService userService, string email)
        {
            return userService.GetUserNameByEmail(email);
        }

        private System.Web.Security.MembershipUserCollection ToMembershipUserCollection(IEnumerable<Models.User> users)
        {
            var mUsers = users.Select(user =>
                new System.Web.Security.MembershipUser(
                    membershipName ?? Name,
                    user.UserName,
                    user.Id,
                    user.Email,
                    null,
                    null,
                    true,
                    false,
                    user.CreatedOn,
                    System.DateTime.MinValue,
                    System.DateTime.MinValue,
                    System.DateTime.MinValue,
                    System.DateTime.MinValue)).ToList();

            var collection = new System.Web.Security.MembershipUserCollection();
            mUsers.ForEach(collection.Add);

            return collection;
        }

        private System.Web.Security.MembershipUser ToMembershipUser(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new System.Web.Security.MembershipUser(
                membershipName ?? Name,
                user.UserName,
                user.Id,
                user.Email,
                null,
                null,
                true,
                false,
                user.CreatedOn,
                System.DateTime.MinValue,
                System.DateTime.MinValue,
                System.DateTime.MinValue,
                System.DateTime.MinValue);
        }
    }
}