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

            var pageContent = Repository.AsQueryable<PageContent>()
                                    .Where(f => f.Id == request.PageContentId)
                                    .Fetch(f => f.Content)
                                    .FirstOrDefault();

            if (pageContent == null)
            {
                throw new CmsException(string.Format("Content was not found by id={0}.", request.PageContentId));
            }
            
            if (pageContent.Content is HtmlContent)
            {
                Repository.Delete<HtmlContent>(pageContent.Content.Id, request.ContentVersion);
            }

            Repository.Delete<PageContent>(pageContent.Id, request.PageContentVersion);
            UnitOfWork.Commit();

            return true;
        }
    }
}