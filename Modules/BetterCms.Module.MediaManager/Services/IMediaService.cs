using System;
using System.Collections.Generic;
using System.Security.Principal;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaService
    {
        bool DeleteMedia(Guid id, int version, bool checkSecurity, IPrincipal currentPrincipal = null);

        void ArchiveSubMedias(Media media, List<Media> archivedMedias);

        void UnarchiveSubMedias(Media media, List<Media> unarchivedMedias);
    }
}