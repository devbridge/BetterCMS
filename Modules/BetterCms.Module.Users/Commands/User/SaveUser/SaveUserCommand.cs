using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels;

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
            //TODO: var salt = authenticationService.GeneratePasswordSalt();
            //TODO: user.Password = authenticationService.CreatePasswordHash(request.Password, salt);
            //TODO: user.Salt = salt;

            user.Password = "TEST";
            user.Salt = "TEST";

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

            return new SaveUserCommandResponse { Id = user.Id, UserName = user.UserName, Version = user.Version };
        }

        private void SaveUserRoles(Models.User user, EditUserViewModel request)
        {
            var dbRoles = user.UserRoles ?? new List<UserRole>();
            var requestRoles = request.Roles ?? new List<string>();

            // Delete removed roles
            dbRoles
                .Where(dbRole => requestRoles.All(requestRole => requestRole != dbRole.Role.Name))
                .ToList()
                .ForEach(del => Repository.Delete(del));

            // Insert new roles
            var rolesToInsert = requestRoles
                .Where(requestRole => dbRoles.All(dbRole => requestRole != dbRole.Role.Name))
                .ToList();

            if (rolesToInsert.Count > 0)
            {
                var roles = Repository
                    .AsQueryable<Models.Role>(role => rolesToInsert.Contains(role.Name))
                    .ToList();

                rolesToInsert
                    .ForEach(roleName => 
                        Repository.Save(new UserRole
                            {
                                User = user,
                                Role = roles.Where(role => role.Name == roleName).FirstOne()
                            }));
            }
        }
    }
}