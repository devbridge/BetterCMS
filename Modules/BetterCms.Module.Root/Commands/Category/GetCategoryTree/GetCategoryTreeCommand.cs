using System;
using System.Linq;

using BetterCms.Module.Root.Helpers;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Commands.Category.GetCategoryTree
{
    public class GetCategoryTreeCommand : CommandBase, ICommand<Guid, CategoryTreeViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        public CategoryTreeViewModel Execute(Guid categoryTreeId)
        {
            if (categoryTreeId.HasDefaultValue())
            {
                var categorizableItems = Repository.AsQueryable<CategorizableItem>().ToList();
                return new CategoryTreeViewModel()
                {
                    ShowMacros = CmsConfiguration.EnableMacros,
                    CategorizableItems = categorizableItems
                        .Select(i => new CategorizableItemViewModel { Id = i.Id, Name = i.Name, IsSelected = true })
                        .OrderBy(i => i.Name)
                        .ToList()
                };
            }

            var categorizableItemsFuture = Repository.AsQueryable<CategorizableItem>().ToFuture();
            var selectedCategorizableItemsFuture = Repository.AsQueryable<CategoryTreeCategorizableItem>()
                .Where(i => i.CategoryTree.Id == categoryTreeId)
                .ToFuture();

            IQueryable<CategoryTree> sitemapQuery = Repository.AsQueryable<CategoryTree>()
                .Where(map => map.Id == categoryTreeId)
                .FetchMany(map => map.Categories);

            var categoryTree = sitemapQuery.Distinct().ToFuture().ToList().First();

            var selectedItems = selectedCategorizableItemsFuture.ToList();
            var model = new CategoryTreeViewModel
            {
                Id = categoryTree.Id,
                Version = categoryTree.Version,
                Title = categoryTree.Title,
                Macro = categoryTree.Macro,
                RootNodes =
                    CategoriesHelper.GetCategoryTreeNodesInHierarchy(
                        CmsConfiguration.EnableMultilanguage,
                        categoryTree.Categories.Distinct().Where(f => f.ParentCategory == null).ToList(),
                        categoryTree.Categories.Distinct().ToList(),
                        null),
                ShowMacros = CmsConfiguration.EnableMacros,
                CategorizableItems = categorizableItemsFuture.ToList()
                    .Select(i => new CategorizableItemViewModel { Id = i.Id, Name = i.Name, IsSelected = selectedItems.Any(s => s.CategorizableItem.Id == i.Id) })
                    .OrderBy(i => i.Name)
                    .ToList()
            };

            return model;
        }

    }
}