using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services.Categories;
using BetterCms.Module.Root.Services.Categories.Tree;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Category.SaveCategoryTree
{
    public class SaveCategoryTreeCommand : CommandBase, ICommand<CategoryTreeViewModel, CategoryTreeViewModel>
    {
        private readonly ICategoryTreeService CategoryTreeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveCategoryTreeCommand"/> class.
        /// </summary>
        /// <param name="categoryTreeService">The category tree service.</param>
        public SaveCategoryTreeCommand(ICategoryTreeService categoryTreeService)
        {
            CategoryTreeService = categoryTreeService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public CategoryTreeViewModel Execute(CategoryTreeViewModel request)
        {
            var serviceRequest = new SaveCategoryTreeRequest();

            serviceRequest.Id = request.Id;
            serviceRequest.Title = request.Title;
            serviceRequest.Version = request.Version;
            serviceRequest.Macro = request.Macro;
            serviceRequest.UseForCategorizableItems = request.CategorizableItems.Where(i => i.IsSelected).Select(i => i.Id).ToList();

            IList<CategoryNodeModel> rootNodes = new List<CategoryNodeModel>();
            if (request.RootNodes != null)
            {
                foreach (var node in request.RootNodes)
                {
                    rootNodes.Add(RemapChildren(node));
                }
                serviceRequest.RootNodes = rootNodes;
            }

            var categoryTree = CategoryTreeService.Save(serviceRequest);

            return new CategoryTreeViewModel
            {
                Id = categoryTree.Id,
                Title = categoryTree.Title,
                Version = categoryTree.Version
            };
        }

        private CategoryNodeModel RemapChildren(CategoryTreeNodeViewModel category)
        {
            var categoryNode = new CategoryNodeModel();
            IList<CategoryNodeModel> childrenCategories = new List<CategoryNodeModel>();
            categoryNode.DisplayOrder = category.DisplayOrder;
            categoryNode.Id = category.Id;
            categoryNode.Macro = category.Macro;
//            categoryNode.ParentId = category.ParentId;
            categoryNode.Title = category.Title;
            categoryNode.Version = category.Version;

            if (category.ChildNodes != null)
            {
                foreach (var node in category.ChildNodes)
                {
                    childrenCategories.Add(RemapChildren(node));
                }
                categoryNode.ChildNodes = childrenCategories;
            }

            return categoryNode;
        }
    }
}