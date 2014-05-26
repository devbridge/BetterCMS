using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    /// <summary>
    /// Command saves existing or creates new blog post
    /// </summary>
    public class SaveBlogPostCommand : CommandBase, ICommand<BlogPostViewModel, SaveBlogPostCommandResponse>
    {
        private readonly IBlogService blogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveBlogPostCommand" /> class.
        /// </summary>
        /// <param name="blogService">The blog service.</param>
        public SaveBlogPostCommand(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Blog post view model</returns>
        public SaveBlogPostCommandResponse Execute(BlogPostViewModel request)
        {
            string[] error;
            var blogPost = blogService.SaveBlogPost(request, Context.Principal, out error);
            if (blogPost == null)
            {
                Context.Messages.AddError(error);
                return null;
            }

            return new SaveBlogPostCommandResponse
                       {
                           Id = blogPost.Id,
                           Version = blogPost.Version,
                           Title = blogPost.Title,
                           PageUrl = blogPost.PageUrl,
                           ModifiedByUser = blogPost.ModifiedByUser,
                           ModifiedOn = blogPost.ModifiedOn.ToFormattedDateString(),
                           CreatedOn = blogPost.CreatedOn.ToFormattedDateString(),
                           PageStatus = blogPost.Status,
                           DesirableStatus = request.DesirableStatus,
                           PageContentId = blogPost.PageContents[0].Id
                       };
        }
    }
}