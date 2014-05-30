using System;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

namespace BetterCms.Module.Blog.Commands.GetUrlSuggestion
{
    public class GetUrlSuggestionCommand : CommandBase, ICommand<GetUrlSuggestionCommandRequest, string>
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly IBlogService blogService;

        public GetUrlSuggestionCommand(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        public virtual string Execute(GetUrlSuggestionCommandRequest request)
        {
            string newUrl = null;
            if (UrlHelper.GeneratePageUrl != null)
            {
                try
                {
                    newUrl =
                        UrlHelper.GeneratePageUrl(
                            new PageUrlGenerationRequest
                                {
                                    Title = request.Title,
                                    ParentPageId = request.ParentPageId,
                                    LanguageId = request.LanguageId,
                                    CategoryId = request.CategoryId
                                });
                }
                catch (Exception ex)
                {
                    Logger.Error("Custom blog post url generation failed.", ex);
                }
            }

            if (string.IsNullOrWhiteSpace(newUrl) && request.TitleChanged)
            {
                newUrl = blogService.CreateBlogPermalink(request.Title);
            }

            return newUrl;
        }
    }
}