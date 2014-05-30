using System;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

using UrlHelper = BetterCms.Module.Pages.Helpers.UrlHelper;

namespace BetterCms.Module.Pages.Command.Page.GetUrlSuggestion
{
    public class GetUrlSuggestionCommand : CommandBase, ICommand<GetUrlSuggestionCommandRequest, string>
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly IPageService pageService;

        public GetUrlSuggestionCommand(IPageService pageService)
        {
            this.pageService = pageService;
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
                    Logger.Error("Custom page url generation failed.", ex);
                }
            }

            if (string.IsNullOrWhiteSpace(newUrl) && request.TitleChanged)
            {
                newUrl = pageService.CreatePagePermalink(request.Title, HttpUtility.UrlDecode(request.ParentPageUrl));
            }

            return newUrl;
        }
    }
}