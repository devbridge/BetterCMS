using BetterCms.Api;
using BetterCms.Core.Mvc.Commands;
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
            var category = Repository.Delete<Category>(request.CategoryId, request.Version);
            UnitOfWork.Commit();

            // Notify.
            PagesApiContext.Events.OnCategoryDeleted(category);

            return true;
        }
    }
}