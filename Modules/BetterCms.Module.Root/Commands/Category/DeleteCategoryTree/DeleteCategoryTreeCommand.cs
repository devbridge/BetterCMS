using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Category;

namespace BetterCms.Module.Root.Commands.Category.DeleteCategoryTree
{
    public class DeleteCategoryTreeCommand : CommandBase, ICommand<CategoryTreeViewModel, bool>
    {
        public ICategoryService categoryService { get; set; }

        public bool Execute(CategoryTreeViewModel request)
        {
            categoryService.DeleteCategoryTree(request.Id, request.Version, Context.Principal);
            return true;
        }
    }
}