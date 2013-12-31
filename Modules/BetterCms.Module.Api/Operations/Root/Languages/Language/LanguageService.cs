using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    public class LanguageService : Service, ILanguageService
    {
        private readonly IRepository repository;

        public LanguageService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetLanguageResponse Get(GetLanguageRequest request)
        {
            var query = repository.AsQueryable<Module.Root.Models.Language>();

            if (request.LanguageId.HasValue)
            {
                query = query.Where(language => language.Id == request.LanguageId);
            }
            else
            {
                query = query.Where(language => language.Code == request.LanguageCode);
            }

            var model = query
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
                .FirstOne();

            return new GetLanguageResponse
                       {
                           Data = model
                       };
        }
    }
}