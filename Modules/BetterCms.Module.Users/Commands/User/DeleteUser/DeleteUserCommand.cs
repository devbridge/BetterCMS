using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Users.Commands.User.DeleteUser
{
    /// <summary>
    /// A command to delete given user.
    /// </summary>
    public class DeleteUserCommand : CommandBase, ICommand<DeleteUserCommandRequest, bool>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteUserCommandRequest request)
        {
            Repository.Delete<Models.User>(request.UserId, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}