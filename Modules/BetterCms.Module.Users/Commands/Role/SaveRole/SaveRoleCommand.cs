using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.Role;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.Role.SaveRole
{
    /// <summary>
    /// A command to save role item.
    /// </summary>
    public class SaveRoleCommand : CommandBase, ICommand<RoleViewModel, SaveRoleCommandResponse>
    {
        private readonly IRoleService roleService;

        public SaveRoleCommand(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        /// <summary>
        /// Executes a command to save role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Response with id and version
        /// </returns>
        public SaveRoleCommandResponse Execute(RoleViewModel request)
        {
            Models.Role role;
            if (request.Id.HasDefaultValue())
            {
                role = roleService.CreateRole(request.Name, request.Description);
                UnitOfWork.Commit();
                Events.UserEvents.Instance.OnRoleCreated(role);
            }
            else
            {
                role = roleService.UpdateRole(request.Id, request.Version, request.Name, request.Description);
                UnitOfWork.Commit();
                Events.UserEvents.Instance.OnRoleUpdated(role);
            }

            return new SaveRoleCommandResponse { Id = role.Id, Name = role.Name, Version = role.Version };
        }
    }
}