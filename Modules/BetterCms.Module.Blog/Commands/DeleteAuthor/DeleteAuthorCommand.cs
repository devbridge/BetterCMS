using BetterCms.Api;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : CommandBase, ICommand<AuthorViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if an author is deleted successfully.</returns>
        public bool Execute(AuthorViewModel request)
        {
            var author = Repository.Delete<Author>(request.Id, request.Version);
            UnitOfWork.Commit();

            BlogsApiContext.Events.OnAuthorDeleted(author);

            return true;
        }
    }
}