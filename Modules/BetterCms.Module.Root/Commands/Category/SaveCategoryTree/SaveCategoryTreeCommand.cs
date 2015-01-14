using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Category;
using System.Collections.Generic;
using System.Linq;

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

            Models.CategoryTree categoryTree;

            var categories = Repository.AsQueryable<Models.Category>().ToList();

            UnitOfWork.BeginTransaction();

            SaveCategoryTree(request.RootCategories, null, categories);
            
            UnitOfWork.Commit();

            //TODO: should we add events for categories ?
            //foreach (var category in createdCategories)
            //{
            //    Events.SitemapEvents.Instance.OnSitemapNodeCreated(node);
            //}

            //foreach (var node in updatedNodes)
            //{
            //    Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
            //}

            //foreach (var node in deletedNodes)
            //{
            //    Events.SitemapEvents.Instance.OnSitemapNodeDeleted(node);
            //}
            //:~

            return new CategoryTreeViewModel
            {
                Id = request.Id,
                Title = request.Title,
                Version = request.Version
            };
        }

        private void SaveCategoryTree(IEnumerable<CategoryItemViewModel> categories, Models.Category parentCategory, List<Models.Category> categoryList)
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
                var category = CategoryService.SaveCategory(out updatedInDB, viewModel.Id, viewModel.Version, viewModel.Name, viewModel.DisplayOrder, viewModel.Macro, viewModel.ParentCategoryId, isDeleted, parentCategory, categoryList);

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
                
                SaveCategoryTree(viewModel.ChildCategories, category, categoryList);
            }
        }
    }
}