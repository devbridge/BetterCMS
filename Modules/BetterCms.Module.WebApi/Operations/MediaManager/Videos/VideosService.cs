using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos
{
    public class VideosService : Service, IVideosService
    {
        private readonly IRepository repository;

        public VideosService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetVideosResponse Get(GetVideosRequest request)
        {
            request.SetDefaultOrder("Title");

            var listResponse = repository
                .AsQueryable<Module.MediaManager.Models.Media>()
                .Select(media => new Videos.MediaModel
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

            return new GetVideosResponse
                       {
                           Data = listResponse
                       };
        }
    }
}