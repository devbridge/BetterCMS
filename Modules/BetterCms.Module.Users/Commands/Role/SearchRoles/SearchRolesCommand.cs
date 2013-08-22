using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Users.Commands.Role.SearchRoles
{
    /// <summary>
    /// A command to get roles list by filter.
    /// </summary>
    public class SearchRolesCommand : CommandBase, ICommand<string, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>
        /// A list of roles.
        /// </returns>
        public List<LookupKeyValue> Execute(string roleName)
        {
            return Repository.AsQueryable<Models.Role>()
                    .Where(role => (role.DisplayName == null && role.Name.Contains(roleName)) 
                        || role.DisplayName.Contains(roleName))
                    .OrderBy(role => role.Name)
                    .Select(role => new LookupKeyValue { Key = role.Id.ToString(), Value = role.DisplayName ?? role.Name })
                    .ToList();
        }
    }
}