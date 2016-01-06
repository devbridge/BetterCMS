using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Tag.SearchTags
{
    /// <summary>
    /// A command to get tag list by filter.
    /// </summary>
    public class SearchTagsCommand : CommandBase, ICommand<SuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of tags.
        /// </returns>
        public List<LookupKeyValue> Execute(SuggestionViewModel model)
        {
            var query = Repository.AsQueryable<Models.Tag>()
                .Where(tag => tag.Name.Contains(model.Query));

            if (model.ExistingItemsArray.Length > 0)
            {
                query = query.Where(tag => !model.ExistingItems.Contains(tag.Name) && !model.ExistingItems.Contains(tag.Id.ToString().ToUpper()));
            }

            return query.OrderBy(tag => tag.Name)
                .Select(tag => new LookupKeyValue { Key = tag.Id.ToString(), Value = tag.Name })
                .ToList();
        }
    }
}