using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    /// <summary>
    /// Command saves existing or creates new blog post
    /// </summary>
    public class SaveBlogPostCommand : CommandBase, ICommand<SaveBlogPostCommandRequest, SaveBlogPostCommandResponse>
    {
        private readonly IBlogService blogService;

        private readonly IWidgetService widgetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveBlogPostCommand" /> class.
        /// </summary>
        /// <param name="blogService">The blog service.</param>
        /// <param name="widgetService">The widget service.</param>
        public SaveBlogPostCommand(IBlogService blogService, IWidgetService widgetService)
        {
            this.blogService = blogService;
            this.widgetService = widgetService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Blog post view model</returns>
        public SaveBlogPostCommandResponse Execute(SaveBlogPostCommandRequest request)
        {
            if (request.Content.ContentTextMode == ContentTextMode.Markdown)
            {
                request.Content.OriginalText = request.Content.Content;
                request.Content.Content = null;
            }

            string[] error;
            var blogPost = blogService.SaveBlogPost(request.Content, request.ChildContentOptionValues, Context.Principal, out error, false);
            if (blogPost == null)
            {
                Context.Messages.AddError(error);
                return null;
            }

            var response = new SaveBlogPostCommandResponse
                       {
                           Id = blogPost.Id,
                           Version = blogPost.Version,
                           Title = blogPost.Title,
                           PageUrl = blogPost.PageUrl,
                           ModifiedByUser = blogPost.ModifiedByUser,
                           ModifiedOn = blogPost.ModifiedOn.ToFormattedDateString(),
                           CreatedOn = blogPost.CreatedOn.ToFormattedDateString(),
                           PageStatus = blogPost.Status,
                           DesirableStatus = request.Content.DesirableStatus,
                           PageContentId = blogPost.PageContents[0].Id,
                           ContentId = blogPost.PageContents[0].Content.Id,
                           ContentVersion = blogPost.PageContents[0].Content.Version
                       };

            if (request.Content.IncludeChildRegions)
            {
                var content = blogPost.PageContents[0].Content;
                var contentData = (content.History != null
                    ? content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft) ?? content
                    : content);

                response.Regions = widgetService.GetWidgetChildRegionViewModels(contentData);
            }

            return response;
        }
    }
}