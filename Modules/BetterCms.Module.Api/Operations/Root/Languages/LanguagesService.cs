using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Languages
{
    public class LanguagesService : Service, ILanguagesService
    {
        private readonly IRepository repository;

        public LanguagesService(IRepository repository)
        {
            this.repository = repository;
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
    }
}