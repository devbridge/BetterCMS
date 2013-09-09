using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.Models;

using BetterCms.Module.Root.Mvc;

using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Commands.User.SaveUserProfile
{
    public class SaveUserProfileCommand : CommandBase, ICommand<EditUserProfileViewModel, bool>
    {
        /// <summary>
        /// The authentication service
        /// </summary>
        private IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveUserProfileCommand" /> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public SaveUserProfileCommand(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Saved user username</returns>
        public bool Execute(EditUserProfileViewModel request)
        {
            UnitOfWork.BeginTransaction();

            var user = Repository
                .AsQueryable<Models.User>()
                .Where(u => u.UserName == Context.Principal.Identity.Name)
                .FetchMany(u => u.UserRoles)
                .ThenFetch(ur => ur.Role)
                .ToList()
                .FirstOne();
            var beforeUpdate = user.CopyDataTo(new Models.User());

            ValidateUser(user, request);

            user.Version = request.Version;
            user.UserName = request.UserName;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var salt = authenticationService.GeneratePasswordSalt();
                user.Password = authenticationService.CreatePasswordHash(request.Password, salt);
                user.Salt = salt;
            }

            if (request.Image != null && request.Image.ImageId.HasValue)
            {
                user.Image = Repository.AsProxy<MediaImage>(request.Image.ImageId.Value);
            }
            else
            {
                user.Image = null;
            }

            Repository.Save(user);
            UnitOfWork.Commit();

            // Notify user was changed and user profile was changed.
            Events.UserEvents.Instance.OnUserUpdated(user);
            Events.UserEvents.Instance.OnUserProfileUpdated(beforeUpdate, user);

            return true;
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="model">The model.</param>
        private void ValidateUser(Models.User user, EditUserProfileViewModel model)
        {
            // Validate security hash
            var securityHash = authenticationService.CreatePasswordHash(Context.Principal.Identity.Name, user.Salt);
            if (string.IsNullOrWhiteSpace(model.SecurityHash) || model.SecurityHash != securityHash)
            {
                var message = string.Format(UsersGlobalization.SaveProfile_SecurityHashasAreNotEqual_Message, model.UserName);
                var logMessage = string.Format("Failed to update user profile. Possible injecion - security hashes are not equal. User Name: {0}, Context Username: {1}", model.UserName, Context.Principal.Identity.Name);

                throw new ValidationException(() => message, logMessage);
            }

            // Validate user name
            var existIngId = Repository
                .AsQueryable<Models.User>(c => c.UserName == model.UserName.Trim() && c.UserName != Context.Principal.Identity.Name)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (!existIngId.HasDefaultValue())
            {
                var message = string.Format(UsersGlobalization.SaveProfile_UserNameExists_Message, model.UserName);
                var logMessage = string.Format("Failed to update user profile. User Name already exists. User Name: {0}, User Email: {1}, Context Username: {2}", model.UserName, model.Email, Context.Principal.Identity.Name);

                throw new ValidationException(() => message, logMessage);
            }

            // Validate user email
            existIngId = Repository
                .AsQueryable<Models.User>(c => c.Email == model.Email.Trim() && c.UserName != Context.Principal.Identity.Name)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (!existIngId.HasDefaultValue())
            {
                var message = string.Format(UsersGlobalization.SaveProfile_UserEmailExists_Message, model.Email);
                var logMessage = string.Format("Failed to update user profile. User Email already exists. User Name: {0}, User Email: {1}, Context Username: {2}", model.UserName, model.Email, Context.Principal.Identity.Name);

                throw new ValidationException(() => message, logMessage);
            }
        }
    }
}