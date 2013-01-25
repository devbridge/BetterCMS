using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Users.Commands.Role.DeleteRole
{
    /// <summary>
    /// A command to delete given category.
    /// </summary>
    public class DeleteRoleCommand : CommandBase, ICommand<DeleteRoleCommandRequest, bool>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteRoleCommandRequest request)
        {
            Repository.Delete<Models.Role>(request.RoleId, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}
