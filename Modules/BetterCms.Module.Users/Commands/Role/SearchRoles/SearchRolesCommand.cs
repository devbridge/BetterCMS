using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.Role.SearchRoles
{
    /// <summary>
    /// A command to get roles list by filter.
    /// </summary>
    public class SearchRolesCommand : CommandBase, ICommand<SuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of roles.
        /// </returns>
        public List<LookupKeyValue> Execute(SuggestionViewModel model)
        {
            var query = Repository.AsQueryable<Models.Role>()
                .Where(role => role.Name.Contains(model.Query) || role.Description.Contains(model.Query));

            if (model.ExistingItemsArray.Length > 0)
            {
                query = query.Where(role => !model.ExistingItems.Contains(role.Name) && !model.ExistingItems.Contains(role.Id.ToString().ToUpper()));
            }

            return query.OrderBy(role => role.Name)
                .Select(role => new LookupKeyValue { Key = role.Id.ToString(), Value = role.Name })
                .ToList();
        }
    }
}