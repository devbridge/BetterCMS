using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.Registration;

namespace BetterCms.Module.Users.Commands.Registration
{
    public class CreateFirstUserCommand : CommandBase, ICommandIn<CreateFirstUserViewModel>
    {
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFirstUserCommand" /> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public CreateFirstUserCommand(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Executes a command to save role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Response with id and version
        /// </returns>
        public void Execute(CreateFirstUserViewModel request)
        {
            if (Repository.AsQueryable<Models.User>().Any())
            {
                throw new ValidationException(() => "First user already exists.", string.Format("Failed to create first user from the model {0}.", request));
            }

            var user = new Models.User();
            var salt = authenticationService.GeneratePasswordSalt();
            var systemRoles = Repository.AsQueryable<Models.Role>().Where(f => f.IsSystematic).ToList();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.Password = authenticationService.CreatePasswordHash(request.Password, salt);
            user.Salt = salt;
            user.UserRoles = systemRoles.Select(role => new UserRole
                                                         {
                                                             User = user,
                                                             Role = role
                                                         })
                                        .ToList();
            
            Repository.Save(user);
            UnitOfWork.Commit();
        }
    }
}