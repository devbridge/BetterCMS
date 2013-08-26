using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Commands.User.SaveUser
{
    /// <summary>
    /// A command to save user item.
    /// </summary>
    public class SaveUserCommand : CommandBase, ICommand<EditUserViewModel, SaveUserCommandResponse>
    {
        private IAuthenticationService authenticationService;

        public SaveUserCommand(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        /// <summary>
        /// Executes a command to save user.
        /// </summary>
        /// <param name="request">The user item.</param>
        /// <returns>
        /// true if user saved successfully; false otherwise.
        /// </returns>
        public SaveUserCommandResponse Execute(EditUserViewModel request)
        {
            ValidateUser(request);

            UnitOfWork.BeginTransaction();

            Models.User user;
            if (!request.Id.HasDefaultValue())
            {
                user = Repository
                    .AsQueryable<Models.User>()
                    .Where(u => u.Id == request.Id)
                    .FetchMany(u => u.UserRoles)
                    .ThenFetch(ur => ur.Role)
                    .ToList()
                    .FirstOne();
            }
            else
            {
                user = new Models.User();
            }

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

            SaveUserRoles(user, request);

            Repository.Save(user);
            UnitOfWork.Commit();

            // Notify.
            if (request.Id.HasDefaultValue())
            {
                Events.UserEvents.Instance.OnUserCreated(user);
            }
            else
            {
                Events.UserEvents.Instance.OnUserUpdated(user);
            }

            return new SaveUserCommandResponse
                       {
                           Id = user.Id, 
                           UserName = user.UserName, 
                           Version = user.Version,
                           Email = user.Email,
                           FullName = user.FirstName + " " + user.LastName
                       };
        }

        private void SaveUserRoles(Models.User user, EditUserViewModel request)
        {
            var dbRoles = user.UserRoles ?? new List<UserRole>();
            var requestRoles = request.Roles ?? new List<string>();

            var test = dbRoles.Where(
                dbRole => !requestRoles.Any(requestRole => (dbRole.Role.DisplayName == null && requestRole == dbRole.Role.Name) || requestRole == dbRole.Role.DisplayName))
                   .ToList();

            // Delete removed roles
            dbRoles
                .Where(dbRole => !requestRoles.Any(requestRole => (dbRole.Role.DisplayName == null && requestRole == dbRole.Role.Name)
                    || requestRole == dbRole.Role.DisplayName))
                .ToList()
                .ForEach(del => Repository.Delete(del));

            // Insert new roles
            var rolesToInsert = requestRoles
                .Where(requestRole => !dbRoles.Any(dbRole => (dbRole.Role.DisplayName == null && requestRole == dbRole.Role.Name)
                    || requestRole == dbRole.Role.DisplayName))
                .ToList();

            if (rolesToInsert.Count > 0)
            {
                var roles = Repository
                    .AsQueryable<Models.Role>(role => (role.DisplayName == null && rolesToInsert.Contains(role.Name))
                        || rolesToInsert.Contains(role.DisplayName))
                    .ToList();

                rolesToInsert
                    .ForEach(roleName => 
                        Repository.Save(new UserRole
                            {
                                User = user,
                                Role = roles.Where(role => (role.DisplayName == null && role.Name == roleName)
                                    || role.DisplayName == roleName).FirstOne()
                            }));
            }
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        private void ValidateUser(EditUserViewModel request)
        {
            var existIngId = Repository
                .AsQueryable<Models.User>(c => c.UserName == request.UserName.Trim() && c.Id != request.Id)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (!existIngId.HasDefaultValue())
            {
                var message = string.Format(UsersGlobalization.SaveUse_UserNameExists_Message, request.UserName);
                var logMessage = string.Format("User Name already exists. User Name: {0}, User Email: {1}, Id: {2}", request.UserName, request.Email, request.Id);

                throw new ValidationException(() => message, logMessage);
            }
            
            existIngId = Repository
                .AsQueryable<Models.User>(c => c.Email == request.Email.Trim() && c.Id != request.Id)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (!existIngId.HasDefaultValue())
            {
                var message = string.Format(UsersGlobalization.SaveUse_UserEmailExists_Message, request.Email);
                var logMessage = string.Format("User Email already exists. User Name: {0}, User Email: {1}, Id: {2}", request.UserName, request.Email, request.Id);

                throw new ValidationException(() => message, logMessage);
            }
        }
    }
}