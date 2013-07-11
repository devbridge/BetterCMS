using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaVideoService
    {
        DataListResponse<MediaVideoViewModel> GetItems(MediaManagerViewModel request);
    }
}