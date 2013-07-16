using System;
using System.Collections.Generic;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaVideoService
    {
        Tuple<IEnumerable<MediaViewModel>, int> GetItems(MediaManagerViewModel request);
    }
}