using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Services
{
    public interface IContentService
    {
        Models.Content SaveContentWithStatusUpdate(Models.Content updatedContent, ContentStatus requestedStatus);
    }
}
