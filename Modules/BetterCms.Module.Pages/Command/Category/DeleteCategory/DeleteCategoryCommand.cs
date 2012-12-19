using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Commands.DeleteCategory
{
    /// <summary>
    /// A command to delete given category.
    /// </summary>
    public class DeleteCategoryCommand : CommandBase, ICommand<DeleteCategoryCommandRequest, bool>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteCategoryCommandRequest request)
        {
            Repository.Delete<Category>(request.CategoryId, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}