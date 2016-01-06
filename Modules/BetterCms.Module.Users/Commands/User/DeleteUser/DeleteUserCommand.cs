using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Services;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.User.DeleteUser
{
    /// <summary>
    /// A command to delete given user.
    /// </summary>
    public class DeleteUserCommand : CommandBase, ICommand<DeleteUserCommandRequest, bool>
    {
        private readonly IUserService userService;

        public DeleteUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteUserCommandRequest request)
        {
            userService.DeleteUser(request.UserId, request.Version);

            return true;
        }
    }
}