using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Services;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.Role.DeleteRole
{
    /// <summary>
    /// A command to delete given role.
    /// </summary>
    public class DeleteRoleCommand : CommandBase, ICommand<DeleteRoleCommandRequest, bool>
    {
        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRoleCommand" /> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        public DeleteRoleCommand(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteRoleCommandRequest request)
        {
            UnitOfWork.BeginTransaction();
            var role = roleService.DeleteRole(request.RoleId, request.Version, false);
            UnitOfWork.Commit();

            // Notify.
            Events.UserEvents.Instance.OnRoleDeleted(role);

            return true;
        }
    }
}
