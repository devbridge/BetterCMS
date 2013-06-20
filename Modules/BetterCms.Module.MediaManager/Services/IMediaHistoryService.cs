using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaHistoryService
    {
        IList<Media> GetMediaHistory(Guid mediaId, SearchableGridOptions gridOptions);
    }
}
