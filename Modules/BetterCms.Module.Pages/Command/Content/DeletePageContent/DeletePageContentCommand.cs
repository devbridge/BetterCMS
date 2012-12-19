using System.Linq;
using NHibernate.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Exceptions;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Command.Content.DeletePageContent
{
    public class DeletePageContentCommand : CommandBase, ICommand<DeletePageContentCommandRequest, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(DeletePageContentCommandRequest request)
        {
            UnitOfWork.BeginTransaction();

            var content = Repository.AsQueryable<PageContent>()
                                    .Where(f => f.Id == request.pageContentId)
                                    .Fetch(f => f.Content)
                                    .FirstOrDefault();

            if (content == null)
            {
                throw new CmsException(string.Format("Content was not found by id={0}.", request.pageContentId));
            }
            if (content.Content is HtmlContent)
            {
                Repository.Delete<HtmlContent>(content.Content.Id, request.ContentVersion);
            }
            Repository.Delete<PageContent>(content.Id, request.PageContentVersion);

            UnitOfWork.Commit();
            return true;
        }
    }
}