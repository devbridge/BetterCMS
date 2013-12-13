using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

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
            var culturesFuture = cultureService.GetCultures();

            var mainPageCultureId = pageService.GetMainCulturePageId(request);
            var mainPageProxy = Repository.AsProxy<Root.Models.Page>(mainPageCultureId);

            response.Translations = Repository
                .AsQueryable<Root.Models.Page>()
                .Where(p => p.MainCulturePage == mainPageProxy || p.Id == mainPageCultureId)
                .Select(p => new PageTranslationViewModel
                    {
                        Id = p.Id,
                        MainCulturePageId = p.MainCulturePage.Id,
                        Title = p.Title,
                        PageUrl = p.PageUrl,
                        CultureId = p.Culture.Id
                    })
                .ToFuture().ToList();
            response.Cultures = culturesFuture.ToList();

            return response;
        }
    }
}