using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Author;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : CommandBase, ICommand<AuthorViewModel, bool>
    {
        private IAuthorService authorService;

        public DeleteAuthorCommand(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if an author is deleted successfully.</returns>
        public bool Execute(AuthorViewModel request)
        {
            authorService.DeleteAuthor(request.Id, request.Version);

            return true;
        }
    }
}