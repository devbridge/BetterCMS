using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Category;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Commands.Category.SaveCategoryTree
{
    public class SaveCategoryTreeCommand : CommandBase, ICommand<CategoryTreeViewModel, CategoryTreeViewModel>
    {
        private readonly IList<Models.Category> createdCategories = new List<Models.Category>();

        private readonly IList<Models.Category> updatedCategories = new List<Models.Category>();

        private readonly IList<Models.Category> deletedCategories = new List<Models.Category>();

        /// <summary>
        /// Gets or sets the category service.
        /// </summary>
        /// <value>
        /// The category service.
        /// </value>
        public ICategoryService CategoryService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public CategoryTreeViewModel Execute(CategoryTreeViewModel request)
        {
            createdCategories.Clear();
            updatedCategories.Clear();
            deletedCategories.Clear();

            var createNew = request.Id.HasDefaultValue();

            var categories = !createNew ? Repository.AsQueryable<Models.Category>()
                                                        .Where(node => node.CategoryTree.Id == request.Id)
                                                        .ToFuture()
                                                  : new List<Models.Category>();

            var categoryTree = !createNew ? this.Repository.AsQueryable<CategoryTree>().Where(s => s.Id == request.Id).ToFuture().ToList().First() : new CategoryTree();

            UnitOfWork.BeginTransaction();

            categoryTree.Title = request.Title;
            categoryTree.Version = request.Version;
            Repository.Save(categoryTree);

            SaveCategoryTree(categoryTree, request.RootNodes, null, categories.ToList());
            
            UnitOfWork.Commit();

            foreach (var category in createdCategories)
            {
                Events.RootEvents.Instance.OnCategoryCreated(category);
            }

            foreach (var category in updatedCategories)
            {
                Events.RootEvents.Instance.OnCategoryUpdated(category);
            }

            foreach (var category in deletedCategories)
            {
                Events.RootEvents.Instance.OnCategoryDeleted(category);
            }

            return new CategoryTreeViewModel
            {
                Id = categoryTree.Id,
                Title = categoryTree.Title,
                Version = categoryTree.Version
            };
        }

        private void SaveCategoryTree(CategoryTree categoryTree, IEnumerable<CategoryTreeNodeViewModel> categories, Models.Category parentCategory, List<Models.Category> categoryList)
        {
            if (categories == null)
            {
                return;
            }

            foreach (var viewModel in categories)
            {
                var isDeleted = viewModel.IsDeleted || (parentCategory != null && parentCategory.IsDeleted);
                var create = viewModel.Id.HasDefaultValue() && !isDeleted;
                var update = !viewModel.Id.HasDefaultValue() && !isDeleted;
                var delete = !viewModel.Id.HasDefaultValue() && isDeleted;

                bool updatedInDB;
                var category = CategoryService.SaveCategory(out updatedInDB, categoryTree, viewModel.Id, viewModel.Version, viewModel.Title, viewModel.DisplayOrder, viewModel.Macro, viewModel.ParentId, isDeleted, parentCategory, categoryList);

                if (create && updatedInDB)
                {
                    createdCategories.Add(category);
                }
                else if (update && (updatedInDB))
                {
                    updatedCategories.Add(category);
                }
                else if (delete && updatedInDB)
                {
                    deletedCategories.Add(category);
                }

                SaveCategoryTree(categoryTree, viewModel.ChildNodes, category, categoryList);
            }
        }
    }
}