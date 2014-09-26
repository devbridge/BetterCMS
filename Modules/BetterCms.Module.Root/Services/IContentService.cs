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

        Models.Content RestoreContentFromArchive(Models.Content restoreFrom);

        int GetPageContentNextOrderNumber(Guid pageId, Guid? parentPageContentId);

        void PublishDraftContent(Guid pageId);

        bool CheckIfContentHasDeletingChildren(Guid? pageId, Guid contentId, string html = null);

        void CheckIfContentHasDeletingChildrenWithException(Guid? pageId, Guid contentId, string html = null);

        void UpdateDynamicContainer(Models.Content content);

        TEntity GetDraftOrPublishedContent<TEntity>(TEntity content) where TEntity : Models.Content;
    }
}
