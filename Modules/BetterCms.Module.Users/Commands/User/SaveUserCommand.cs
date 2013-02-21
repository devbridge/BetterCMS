using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels;

namespace BetterCms.Module.Users.Commands.User
{
    /// <summary>
    /// A command to save user item.
    /// </summary>
    public class SaveUserCommand: CommandBase, ICommand<EditUserViewModel, SaveUserResponse>
    {
        private IRoleService roleService;

        private IAuthenticationService authenticationService;

        public SaveUserCommand(IRoleService roleService, IAuthenticationService authenticationService)
        {
            this.roleService = roleService;
            this.authenticationService = authenticationService;
        }
        /// <summary>
        /// Executes a command to save user.
        /// </summary>
        /// <param name="userItem">The user item.</param>
        /// <returns>
        /// true if user saved successfully; false otherwise.
        /// </returns>
        public SaveUserResponse Execute(EditUserViewModel userItem)
        {
            UnitOfWork.BeginTransaction();

            var user = !userItem.Id.HasDefaultValue()
                               ? Repository.AsQueryable<Models.Users>()
                                           .Where(f => f.Id == userItem.Id)
                                           .ToList()
                                           .FirstOrDefault()
                               : new Models.Users();

            if (user == null)
            {
                user = new Models.Users();
            }

            user.UserName = userItem.UserName;
            user.FirstName = userItem.FirstName;
            user.LastName = userItem.LastName;
            user.Email = userItem.Email;
            var salt = authenticationService.GeneratePasswordSalt();
            user.Password = authenticationService.CreatePasswordHash(userItem.Password, salt);
            user.Salt = salt;
            user.Version = userItem.Version;

            if (userItem.Image != null && userItem.Image.ImageId.HasValue)
            {
                user.Image = Repository.AsProxy<MediaImage>(userItem.Image.ImageId.Value);
            }
            else
            {
                user.Image = null;
            }

            if (userItem.RoleId != null)
            {
                var userRoles = new UserRoles();
                userRoles.User = user;
                userRoles.Role = roleService.GetRole(userItem.RoleId);

                Repository.Save(userRoles);
            }

            Repository.Save(user);
            UnitOfWork.Commit();

            return new SaveUserResponse() { Id = user.Id, UserName = user.UserName, Version = user.Version };
        }
    }
}