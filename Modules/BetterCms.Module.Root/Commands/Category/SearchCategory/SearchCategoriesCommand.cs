using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.Web.Mvc.Commands;

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
            // TODO: #1210
            // return Categories(model);
            var query = Repository.AsQueryable<Models.Category>()
                .Where(category => category.Name.Contains(model.Query));

            if (model.ExistingItemsArray.Length > 0)
            {
                query = query.Where(category => !model.ExistingItems.Contains(category.Id.ToString().ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.CategoryTreeForKey))
            {
                query = query.Where(c => !c.CategoryTree.IsDeleted && c.CategoryTree.AvailableFor.Any(e => e.CategorizableItem.Name == model.CategoryTreeForKey));
            }

            return query.OrderBy(category => category.Name)
                .Select(category => new LookupKeyValue { Key = category.Id.ToString(), Value = category.Name })
                .ToList();
        }


        private List<LookupKeyValue> Categories(CategorySuggestionViewModel model)
        {
            var query = Repository.AsQueryable<Models.Category>().Where(c => !c.CategoryTree.IsDeleted && !c.IsDeleted);
            if (!string.IsNullOrEmpty(model.CategoryTreeForKey))
            {
                query = query.Where(c => c.CategoryTree.AvailableFor.Any(e => e.CategorizableItem.Name == model.CategoryTreeForKey));
            }
            var allCategories = query.ToList();
            var treeLikePlainList = new List<LookupKeyValue>();
            ConstructTreeLike(
                treeLikePlainList,
                string.Empty,
                allCategories.Where(c => c.ParentCategory == null).OrderBy(c => c.DisplayOrder).ToList(),
                allCategories,
                model.Query);
            return treeLikePlainList;
        }

        private void ConstructTreeLike(List<LookupKeyValue> list, string prefix, List<Models.Category> categoriesToAdd, List<Models.Category> allCategories, string query)
        {
            foreach (var category in categoriesToAdd)
            {
                if (category.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()))
                {
                    list.Add(new LookupKeyValue { Key = category.Id.ToString(), Value = string.Format("{0}{1}", prefix, category.Name) });
                }
                ConstructTreeLike(
                    list,
                    string.Format("{0}{1} => ", prefix, category.Name),
                    allCategories.Where(c => c.ParentCategory != null && c.ParentCategory.Id == category.Id).OrderBy(c => c.DisplayOrder).ToList(),
                    allCategories,
                    query);
            }
        }
    }
}