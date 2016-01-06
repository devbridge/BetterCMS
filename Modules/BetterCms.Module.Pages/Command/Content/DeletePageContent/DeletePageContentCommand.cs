using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

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
            var deletingPageContent = Repository
                .AsQueryable<PageContent>()
                .Where(f => f.Id == request.PageContentId)
                .Fetch(f => f.Content)
                .Fetch(f => f.Page)
                .ThenFetchMany(f => f.PageContents)
                .ThenFetch(f => f.Content)
                .ToList()
                .FirstOne();

            if (request.PageContentVersion != deletingPageContent.Version)
            {
                throw new ConcurrentDataException(deletingPageContent);
            }

            // Check content's / draft content's version
            var contentToCheck = contentService.GetDraftOrPublishedContent(deletingPageContent.Content);
            if (request.ContentVersion != contentToCheck.Version)
            {
                throw new ConcurrentDataException(contentToCheck);
            }

            var htmlContainer = deletingPageContent.Content as IDynamicContentContainer;
            if (htmlContainer != null)
            {
                // Check if user has confirmed the deletion of content
                if (!request.IsUserConfirmed)
                {
                    var hasAnyChildren = contentService.CheckIfContentHasDeletingChildren(deletingPageContent.Page.Id, deletingPageContent.Content.Id);
                    if (hasAnyChildren)
                    {
                        var message = PagesGlobalization.DeletePageContent_ContentHasChildrenContents_DeleteConfirmationMessage;
                        var logMessage = string.Format("User is trying to delete content which has children contents. Confirmation is required. PageContentId: {0}, ContentId: {1}, PageId: {2}",
                               request.PageContentId, deletingPageContent.Page.Id, deletingPageContent.Content.Id);
                        throw new ConfirmationRequestException(() => message, logMessage);
                    }
                }
            }

            var pageContentsToDelete = new List<PageContent> { deletingPageContent };
            pageContentsToDelete = RetrieveAllChildrenToDelete(deletingPageContent.Page.PageContents.ToList(), pageContentsToDelete, new[] { deletingPageContent.Id });

            UnitOfWork.BeginTransaction();

            var htmlContentsToDelete = new List<HtmlContent>();
            pageContentsToDelete.ForEach(pageContent =>
                {
                    // If content is HTML content, delete HTML content
                    var htmlContent = pageContent.Content as HtmlContent;
                    if (htmlContent != null)
                    {
                        var draft = pageContent.Content.History != null ? pageContent.Content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft) : null;
                        if (draft != null)
                        {
                            Repository.Delete(draft);
                        }

                        Repository.Delete(pageContent.Content);
                        htmlContentsToDelete.Insert(0, htmlContent);
                    }

                    if (pageContent.Options != null)
                    {
                        foreach (var option in pageContent.Options)
                        {
                            Repository.Delete(option);
                        }
                    }

                    Repository.Delete(pageContent);
                });

            UnitOfWork.Commit();

            // Notify
            pageContentsToDelete.Reverse();
            pageContentsToDelete.ForEach(pageContent => Events.PageEvents.Instance.OnPageContentDeleted(pageContent));
            htmlContentsToDelete.ForEach(htmlContent => Events.PageEvents.Instance.OnHtmlContentDeleted(htmlContent));

            return true;
        }

        /// <summary>
        /// Retrieves all children to delete.
        /// </summary>
        /// <param name="allPageContents">All page contents.</param>
        /// <param name="pageContentsToDelete">The page contents to delete.</param>
        /// <returns>The list with page contents to delete</returns>
        private List<PageContent> RetrieveAllChildrenToDelete(List<PageContent> allPageContents, List<PageContent> pageContentsToDelete, Guid[] parentIds)
        {
            var children = allPageContents.Where(pc => pc.Parent != null && parentIds.Contains(pc.Parent.Id)).ToList();

            if (children.Any())
            {
                pageContentsToDelete.AddRange(children);
                parentIds = children.Select(c => c.Id).ToArray();
                pageContentsToDelete = RetrieveAllChildrenToDelete(allPageContents, pageContentsToDelete, parentIds);
            }

            return pageContentsToDelete;
        }
    }
}