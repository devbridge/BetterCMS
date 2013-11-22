using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.DeletePageContent
{
    /// <summary>
    /// Command for page content deleting.
    /// </summary>
    public class DeletePageContentCommand : CommandBase, ICommand<DeletePageContentCommandRequest, bool>
    {
        /// <summary>
        /// The content service
        /// </summary>
        private readonly IContentService contentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePageContentCommand" /> class.
        /// </summary>
        /// <param name="contentService">The content service.</param>
        public DeletePageContentCommand(IContentService contentService)
        {
            this.contentService = contentService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>True if deleted successfully and False otherwise.</returns>
        public bool Execute(DeletePageContentCommandRequest request)
        {
            UnitOfWork.BeginTransaction();

            var pageContent = Repository.AsQueryable<PageContent>()
                                    .Where(f => f.Id == request.PageContentId)
                                    .Fetch(f => f.Content)
                                    .Fetch(f => f.Page)
                                    .FirstOne();

            if (pageContent.Content is HtmlContent)
            {
                // Check if user has confirmed the deletion of content
                if (!request.IsUserConfirmed && pageContent.Page.IsMasterPage)
                {
                    var hasAnyChildren = contentService.CheckIfContentHasDeletingChildren(pageContent.Page.Id, pageContent.Content.Id);
                    if (hasAnyChildren)
                    {
                        var message = PagesGlobalization.DeletePageContent_ContentHasChildrenContents_DeleteConfirmationMessage;
                        var logMessage = string.Format("User is trying to delete content which has children contents. Confirmation is required. PageContentId: {0}, ContentId: {1}, PageId: {2}",
                               request.PageContentId, pageContent.Page.Id, pageContent.Content.Id);
                        throw new ConfirmationRequestException(() => message, logMessage);
                    }
                }

                var draft = pageContent.Content.History != null ? pageContent.Content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft) : null;
                if (draft != null)
                {
                    Repository.Delete<HtmlContent>(draft.Id, request.ContentVersion);
                    Repository.Delete<HtmlContent>(pageContent.Content.Id, pageContent.Content.Version);
                }
                else
                {
                    Repository.Delete<HtmlContent>(pageContent.Content.Id, request.ContentVersion);
                }
            }

            if (pageContent.Options != null)
            {
                foreach (var option in pageContent.Options)
                {
                    Repository.Delete(option);
                }
            }

            Repository.Delete<PageContent>(pageContent.Id, request.PageContentVersion);
            UnitOfWork.Commit();

            return true;
        }
    }
}