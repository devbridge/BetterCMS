using System;

using BetterCms.Api.Interfaces.Models.Enums;

namespace BetterCms.Module.Root.Services
{
    public interface IContentService
    {
        Tuple<Models.PageContent, Models.Content> GetPageContentForEdit(Guid pageContentId);

        Models.Content GetContentForEdit(Guid contentId);

        Models.Content SaveContentWithStatusUpdate(Models.Content updatedContent, ContentStatus requestedStatus);

        void RestoreContentFromArchive(Models.Content restoreFrom);
    }
}
