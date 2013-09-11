using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.DeletePageContent
{
    /// <summary>
    /// Command for page content deleting.
    /// </summary>
    public class DeletePageContentCommand : CommandBase, ICommand<DeletePageContentCommandRequest, bool>
    {
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
                                    .FirstOne();

            if (pageContent.Content is HtmlContent)
            {
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