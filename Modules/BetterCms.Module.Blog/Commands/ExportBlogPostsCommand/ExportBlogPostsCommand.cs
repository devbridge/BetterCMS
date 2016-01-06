using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Filter;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.ExportBlogPostsCommand
{
    public class ExportBlogPostsCommand : CommandBase, ICommand<BlogsFilter, string>
    {
        private readonly IBlogMLExportService exportService;
        
        private readonly IBlogService blogService;

        public ExportBlogPostsCommand(IBlogMLExportService exportService, IBlogService blogService)
        {
            this.exportService = exportService;
            this.blogService = blogService;
        }

        public string Execute(BlogsFilter request)
        {
            // Get all ONLY PUBLISHED blog posts, filtered out by the request filter
            var query = blogService.GetFilteredBlogPostsQuery(request, true);
            var blogPosts = query.Where(p => p.Status == PageStatus.Published).AddOrder(request).List().Distinct().ToList();

            var xml = exportService.ExportBlogPosts(blogPosts.ToList());

            return xml;
        }
    }
}