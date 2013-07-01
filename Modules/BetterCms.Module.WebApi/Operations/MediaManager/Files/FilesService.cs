using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    public class FilesService : Service, IFilesService
    {
        private readonly IRepository repository;

        public FilesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetFilesResponse Get(GetFilesRequest request)
        {
            request.SetDefaultOrder("Title");

            var listResponse = repository
                .AsQueryable<Module.MediaManager.Models.Media>()
                .Select(media => new MediaModel
                                   {
                                       Id = media.Id,
                                       Version = media.Version,
                                       CreatedBy = media.CreatedByUser,
                                       CreatedOn = media.CreatedOn,
                                       LastModifiedBy = media.ModifiedByUser,
                                       LastModifiedOn = media.ModifiedOn,
                                       Title = media.Title
                                   })
                .ToDataListResponse(request);

            return new GetFilesResponse
                       {
                           Data = listResponse
                       };
        }
    }
}