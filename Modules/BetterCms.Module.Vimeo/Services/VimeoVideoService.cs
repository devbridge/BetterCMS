using System;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.Vimeo.Services
{
    public class VimeoVideoService : IMediaVideoService
    {
        public DataListResponse<MediaVideoViewModel> GetItems(MediaManagerViewModel request)
        {
            throw new NotImplementedException();
        }
    }
}