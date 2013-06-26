using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Api.DataContracts;
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
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                return api.DeleteAuthor(new AuthorDeleteRequest { AuthorId = request.Id, Version = request.Version }).Deleted;
            }
        }
    }
}