using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaService
    {
        void DeleteMedia(Media media);

        void ArchiveSubMedias(Media media, List<Media> archivedMedias);

        void UnarchiveSubMedias(Media media, List<Media> unarchivedMedias);
    }
}