using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels.Role;

namespace BetterCms.Module.Users.Commands.Role.SaveRole
{
    /// <summary>
    /// A command to save role item.
    /// </summary>
    public class SaveRoleCommand : CommandBase, ICommand<RoleViewModel, SaveRoleCommandResponse>
    {
        /// <summary>
        /// Executes a command to save role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Response with id and version
        /// </returns>
        public SaveRoleCommandResponse Execute(RoleViewModel request)
        {
            // Check if such role doesn't exist
            ValidateRoleName(request);

            Models.Role role;
            if (!request.Id.HasDefaultValue())
            {
                role = Repository.AsQueryable<Models.Role>().Where(f => f.Id == request.Id).FirstOne();

                if (role.IsSystematic)
                {
                    var logMessage = string.Format("Cannot save systematic role: {0} {1}", role.Name, role.DisplayName);
                    var message = string.Format(UsersGlobalization.SaveRole_Cannot_Save_Systematic_Role, role.DisplayName ?? role.Name);

                    throw new ValidationException(() => message, logMessage);
                }
            }
            else
            {
                role = new Models.Role();
            }

            role.Version = request.Version;
            role.Name = request.Name;

            Repository.Save(role);
            UnitOfWork.Commit();

            return new SaveRoleCommandResponse { Id = role.Id, Name = role.Name, Version = role.Version };
        }

        /// <summary>
        /// Validates the name of the role.
        /// </summary>
        /// <param name="request">The request.</param>
        private void ValidateRoleName(RoleViewModel request)
        {
            var existIngId = Repository
                .AsQueryable<Models.Role>(c => c.Name == request.Name.Trim() && c.Id != request.Id)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (!existIngId.HasDefaultValue())
            {
                var message = string.Format(UsersGlobalization.SaveRole_RoleExists_Message, request.Name);
                var logMessage = string.Format("Role already exists. Role name: {0}, Id: {1}", request.Name, request.Id);

                throw new ValidationException(() => message, logMessage);
            }
        }
    }
}