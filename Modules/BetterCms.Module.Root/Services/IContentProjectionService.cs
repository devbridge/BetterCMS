using System.Collections.Generic;

using BetterCms.Core.DataContracts;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.Services
{
    public interface IContentProjectionService
    {
        PageContentProjection CreatePageContentProjection(
            bool canManageContent,
            PageContent pageContent,
            List<PageContent> allPageContents, 
            IChildContent childContent = null,
            System.Guid? previewPageContentId = null,
            System.Guid? languageId = null,
            bool retrieveCorrectVersion = true);
    }
}