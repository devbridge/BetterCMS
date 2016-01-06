using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services.Categories.Tree;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Category.DeleteCategoryTree
{
    public class DeleteCategoryTreeCommand : CommandBase, ICommand<CategoryTreeViewModel, bool>
    {
        private readonly ICategoryTreeService CategoryTreeService;

        public DeleteCategoryTreeCommand(ICategoryTreeService categoryTreeService)
        {
            CategoryTreeService = categoryTreeService;
        }

        public bool Execute(CategoryTreeViewModel request)
        {
            var serviceRequest = new DeleteCategoryTreeRequest { Id = request.Id, Version = request.Version, CurrentUser = Context.Principal };
            CategoryTreeService.Delete(serviceRequest);
            return true;
        }
    }
}