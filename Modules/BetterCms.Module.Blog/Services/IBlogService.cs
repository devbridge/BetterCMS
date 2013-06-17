using System.Linq;

using BetterCms.Module.Blog.Api.DataFilters;
using BetterCms.Module.Blog.Api.DataModels;

namespace BetterCms.Module.Blog.Services
{
    public interface IBlogService
    {
        string CreateBlogPermalink(string title);

        IQueryable<BlogPostModel> GetBlogPostsAsQueryable(GetBlogPostRequest filter);
    }
}