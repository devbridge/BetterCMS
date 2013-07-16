using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Root.Commands.Category.DeleteCategory
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
            var category = Repository.Delete<Models.Category>(request.CategoryId, request.Version);
            UnitOfWork.Commit();

            // Notify.
            Events.RootEvents.Instance.OnCategoryDeleted(category);

            return true;
        }
    }
}