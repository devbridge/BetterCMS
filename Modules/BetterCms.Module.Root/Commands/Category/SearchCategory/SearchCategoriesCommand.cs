using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;
using BetterCms.Module.Root.ViewModels.Category;

namespace BetterCms.Module.Root.Commands.Category.SearchCategory
{
    /// <summary>
    /// A command to get tag list by filter.
    /// </summary>
    public class SearchCategoriesCommand : CommandBase, ICommand<CategorySuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of tags.
        /// </returns>
        public List<LookupKeyValue> Execute(CategorySuggestionViewModel model)
        {
            var query = Repository.AsQueryable<Models.Category>()
                .Where(category => category.Name.Contains(model.Query));

            if (model.ExistingItemsArray.Length > 0)
            {
                query = query.Where(category => !model.ExistingItems.Contains(category.Name) && !model.ExistingItems.Contains(category.Id.ToString().ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.CategoryTreeForKey))
            {
                query = query.Where(c => !c.CategoryTree.IsDeleted && c.CategoryTree.AvailableFor.Any(e => e.CategorizableItem.Name == model.CategoryTreeForKey));
            }

            return query.OrderBy(category => category.Name)
                .Select(category => new LookupKeyValue { Key = category.Id.ToString(), Value = category.Name })
                .ToList();
        }
    }
}