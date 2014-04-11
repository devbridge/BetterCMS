using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Commands.ImportBlogPosts
{
    public class ImportBlogPostsCommand : CommandBase, ICommand<ImportBlogPostsViewModel, ImportBlogPostsResponse>
    {
        private readonly IBlogMLService importService;

        public ImportBlogPostsCommand(IBlogMLService importService)
        {
            this.importService = importService;
        }

        public ImportBlogPostsResponse Execute(ImportBlogPostsViewModel request)
        {
            var blogs = importService.DeserializeXMLStream(request.FileStream);
            importService.ImportBlogs(blogs, Context.Principal, request.UseOriginalUrls, request.CreateRedirects);

            return new ImportBlogPostsResponse();
        }
    }
}