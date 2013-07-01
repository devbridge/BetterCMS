using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    public class ImagesService : Service, IImagesService
    {
        private readonly IRepository repository;

        public ImagesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetImagesResponse Get(GetImagesRequest request)
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

            return new GetImagesResponse
                       {
                           Data = listResponse
                       };
        }
    }
}