using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.ApiExtensions;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Pages.Services;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations
{
    public class PageTranslationsController : ApiController, IPageTranslationsService
    {
        private readonly IRepository repository;
        
        private readonly IUrlService urlService;

        public PageTranslationsController(IRepository repository, IUrlService urlService)
        {
            this.repository = repository;
            this.urlService = urlService;
        }

        [Route("bcms-api/pages/{PageId}/translations")]
        [Route("bcms-api/pages/translations/by-url/{PageUrl}")]
        [ValidationAtttibute]
        public GetPageTranslationsResponse Get([ModelBinder(typeof(JsonModelBinder))] GetPageTranslationsRequest request)
        {
            var languageGroupIdentifier = GetPageLanguageGroupIdentifier(request);

            // Page has no translations
            if (!languageGroupIdentifier.HasValue)
            {
                return new GetPageTranslationsResponse { Data = new DataListResponse<PageTranslationModel>() };
            }

            request.Data.SetDefaultOrder("Title");

            // Load translations
            var query = repository
                .AsQueryable<Module.Pages.Models.PageProperties>()
                .Where(p => p.LanguageGroupIdentifier == languageGroupIdentifier.Value);

            var dataListResult = query.Select(p => new PageTranslationModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    PageUrl = p.PageUrl,
                    LanguageId = p.Language != null ? p.Language.Id : (System.Guid?)null,
                    LanguageCode = p.Language != null ? p.Language.Code : null,
                    IsPublished = p.Status == PageStatus.Published,
                    PublishedOn = p.PublishedOn
                }).ToDataListResponse(request);

            return new GetPageTranslationsResponse { Data = dataListResult };
        }

        private System.Guid? GetPageLanguageGroupIdentifier(GetPageTranslationsRequest request)
        {
            // Get page language group identifier
            var query = repository.AsQueryable<Module.Pages.Models.PageProperties>();
            if (request.PageId.HasValue)
            {
                query = query.Where(p => p.Id == request.PageId.Value);
            }
            else
            {
                var url = urlService.FixUrl(request.PageUrl);
                query = query.Where(p => p.PageUrl == url);
            }
            
            return query.Select(p => p.LanguageGroupIdentifier).FirstOrDefault();
        }
    }
}