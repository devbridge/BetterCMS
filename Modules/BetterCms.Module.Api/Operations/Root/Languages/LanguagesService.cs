using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Languages.Language;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Languages
{
    /// <summary>
    /// Language service for languages list handling.
    /// </summary>
    public class LanguagesService : Service, ILanguagesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The language service.
        /// </summary>
        private readonly ILanguageService languageService;

        public LanguagesService(IRepository repository, ILanguageService languageService)
        {
            this.repository = repository;
            this.languageService = languageService;
        }

        public GetLanguagesResponse Get(GetLanguagesRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.Language>()
                .Select(language => new LanguageModel
                    {
                        Id = language.Id,
                        Version = language.Version,
                        CreatedBy = language.CreatedByUser,
                        CreatedOn = language.CreatedOn,
                        LastModifiedBy = language.ModifiedByUser,
                        LastModifiedOn = language.ModifiedOn,

                        Name = language.Name,
                        Code = language.Code
                    })
                .ToDataListResponse(request);

            return new GetLanguagesResponse
                       {
                           Data = listResponse
                       };
        }

        public PostLanguageResponse Post(PostLanguageRequest request)
        {
            var result =
                languageService.Put(
                    new PutLanguageRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostLanguageResponse { Data = result.Data };
        }
    }
}