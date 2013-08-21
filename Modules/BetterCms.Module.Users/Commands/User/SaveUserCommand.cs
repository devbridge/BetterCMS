using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
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
        /// <param name="request">The user item.</param>
        /// <returns>
        /// true if user saved successfully; false otherwise.
        /// </returns>
        public SaveUserResponse Execute(EditUserViewModel request)
        {
            UnitOfWork.BeginTransaction();

            Models.Users user;
            if (!request.Id.HasDefaultValue())
            {
                user = Repository.AsQueryable<Models.Users>().Where(f => f.Id == request.Id).FirstOne();
            }
            else
            {
                user = new Models.Users();
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

//            if (request.RoleId != null)
//            {
//                var userRoles = new UserRoles();
//                userRoles.User = user;
//                userRoles.Role = roleService.GetRole(request.RoleId);
//
//                Repository.Save(userRoles);
//            }

            Repository.Save(user);
            UnitOfWork.Commit();

            return new SaveUserResponse { Id = user.Id, UserName = user.UserName, Version = user.Version };
        }
    }
}