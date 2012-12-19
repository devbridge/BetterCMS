using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.SortPageContent
{
    public class SortPageContentCommand : CommandBase, ICommand<PageContentSortViewModel, SortPageContentResponse>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SortPageContentResponse Execute(PageContentSortViewModel request)
        {
            var response = new SortPageContentResponse();

            UnitOfWork.BeginTransaction();

            #region Updated page content Order if needed.
            var pageContents = Repository.AsQueryable<PageContent>().Where(f => f.Page.Id == request.PageId && f.Region.Id == request.RegionId).ToList();
            var index = 0;
            foreach (var content in request.PageContents)
            {
                var pageContent = pageContents.FirstOrDefault(f => f.Id == content.Id);
                if (pageContent == null)
                {
                    throw new CmsException(string.Format("Content was not found by id={0}.", content.Id));
                }
                if (pageContent.Order != index)
                {
                    if (pageContent.Version != content.Version)
                    {
                        throw new CmsException(string.Format("Page content with id={0} was modified.", content.Id));
                    }
                    pageContent.Order = index;
                    Repository.Save(pageContent);
                    response.UpdatedPageContents.Add(new ContentViewModel { Id = pageContent.Id });
                }
                index++;
            }
            #endregion

            #region Get page content updated versions.
            // NOTE: Maybe, it is logical to place this code after UnitOfWork.Commit(), but in this way, NHibernate generates less traffic to DB.
            pageContents = Repository.AsQueryable<PageContent>().Where(f => f.Page.Id == request.PageId && f.Region.Id == request.RegionId).ToList();
            foreach (var updatedPageContent in response.UpdatedPageContents)
            {
                var pageContent = pageContents.FirstOrDefault(f => f.Id == updatedPageContent.Id);
                if (pageContent == null)
                {
                    throw new CmsException(string.Format("Content was not found by id={0}.", updatedPageContent.Id));
                }
                updatedPageContent.Version = pageContent.Version;
            }
            #endregion

            UnitOfWork.Commit();

            return response;
        }
    }
}