using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Content.Resources;

namespace BetterCms.Module.Users.Commands.Role.DeleteRole
{
    /// <summary>
    /// A command to delete given role.
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
            var role = Repository.First<Models.Role>(request.RoleId);
            role.Version = request.Version;

            if (role.IsSystematic)
            {
                var logMessage = string.Format("Cannot delete systematic role: {0} {1}", role.Name, role.DisplayName);
                var message = string.Format(UsersGlobalization.DeleteRole_Cannot_Delete_Systematic_Role, role.DisplayName ?? role.Name);

                throw new ValidationException(() => message, logMessage);
            }

            Repository.Delete(role);
            UnitOfWork.Commit();

            // Notify.
            Events.UserEvents.Instance.OnRoleDeleted(role);

            return true;
        }
    }
}
