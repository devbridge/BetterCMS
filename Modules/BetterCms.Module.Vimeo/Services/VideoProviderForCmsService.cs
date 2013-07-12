using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.Vimeo.Services
{
    public class VideoProviderForCmsService : IMediaVideoService
    {
        public DataListResponse<MediaVideoViewModel> GetItems(MediaManagerViewModel request)
        {
            // TODO: implement
            return new DataListResponse<MediaVideoViewModel>(new List<MediaVideoViewModel>());
        }
    }
}