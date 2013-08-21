using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
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
            Models.Role role;

            if (!request.Id.HasDefaultValue())
            {
                role = Repository.AsQueryable<Models.Role>().Where(f => f.Id == request.Id).FirstOne();
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
    }
}