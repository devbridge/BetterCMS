using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.InsertContent
{
    public class InsertContentToPageCommand : CommandBase, ICommand<InsertContentToPageRequest, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(InsertContentToPageRequest request)
        {
            UnitOfWork.BeginTransaction();
            
            var page = Repository.AsProxy<Root.Models.Page>(request.PageId);
            var region = Repository.AsProxy<Region>(request.RegionId);
            var content = Repository.AsProxy<Root.Models.Content>(request.ContentId);
            
            PageContent parentPageContent = null;
            if (request.ParentPageContentId.HasValue)
            {
                parentPageContent = Repository.AsProxy<PageContent>(request.ParentPageContentId.Value);
            }

            var pageContent = new PageContent
                                {
                                    Page = page,
                                    Content = content,
                                    Region = region,
                                    Parent = parentPageContent
                                };

            // Retrieve maximum order number
            var max = Repository
                .AsQueryable<PageContent>()
                .Where(f => f.Page.Id == request.PageId && !f.IsDeleted && f.Parent == parentPageContent)
                .Select(f => (int?)f.Order).Max();
            if (max == null)
            {
                pageContent.Order = 0;
            }
            else
            {
                pageContent.Order = max.Value + 1;
            }

            Repository.Save(pageContent);
            UnitOfWork.Commit();

            // Notify.
            Events.PageEvents.Instance.OnPageContentInserted(pageContent);

            return true;
        }
    }
}