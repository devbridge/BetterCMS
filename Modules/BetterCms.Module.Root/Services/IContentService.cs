using System;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services
{
    public interface IContentService
    {
        Tuple<PageContent, Models.Content> GetPageContentForEdit(Guid pageContentId);

        Models.Content GetContentForEdit(Guid contentId);

        Models.Content SaveContentWithStatusUpdate(Models.Content updatedContent, ContentStatus requestedStatus);

        void RestoreContentFromArchive(Models.Content restoreFrom);

        int GetPageContentNextOrderNumber(Guid pageId);

        void PublishDraftContent(Guid pageId);

        bool CheckIfContentHasDeletingChildren(Guid pageId, Guid contentId, string html = null);
    }
}
