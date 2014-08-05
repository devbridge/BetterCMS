using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Content.InsertContent
{
    public class InsertContentToPageCommand : CommandBase, ICommand<InsertContentToPageRequest, bool>
    {
        private readonly IContentService contentService;

        public InsertContentToPageCommand(IContentService contentService)
        {
            this.contentService = contentService;
        }

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
            pageContent.Order = contentService.GetPageContentNextOrderNumber(request.PageId, request.ParentPageContentId);


            Repository.Save(pageContent);
            UnitOfWork.Commit();

            // Notify.
            Events.PageEvents.Instance.OnPageContentInserted(pageContent);

            return true;
        }
    }
}