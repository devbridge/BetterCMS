using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

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
    [RoutePrefix("bcms-api")]
    public class LanguagesController : ApiController, ILanguagesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The language service.
        /// </summary>
        private readonly ILanguageService languageService;

        public LanguagesController(IRepository repository, ILanguageService languageService)
        {
            this.repository = repository;
            this.languageService = languageService;
        }

        [Route("languages")]
        public GetLanguagesResponse Get([ModelBinder(typeof(JsonModelBinder))]GetLanguagesRequest request)
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

        [Route("languages")]
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