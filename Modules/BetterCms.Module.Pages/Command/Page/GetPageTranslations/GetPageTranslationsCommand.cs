using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Page.GetPageTranslations
{
    public class GetPageTranslationsCommand : CommandBase, ICommand<Guid, GetPageTranslationsCommandResponse>
    {
        private readonly ICultureService cultureService;

        private readonly IPageService pageService;

        public GetPageTranslationsCommand(ICultureService cultureService, IPageService pageService)
        {
            this.cultureService = cultureService;
            this.pageService = pageService;
        }

        public GetPageTranslationsCommandResponse Execute(Guid request)
        {
            var response = new GetPageTranslationsCommandResponse();
            var mainPageCultureId = pageService.GetMainCulturePageId(request);

            var culturesFuture = cultureService.GetCultures();

            response.Translations = pageService.GetPageTranslations(mainPageCultureId).ToList();
            response.Cultures = culturesFuture.ToList();

            return response;
        }
    }
}