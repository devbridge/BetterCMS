using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Content.SavePageHtmlContent
{
    public class SavePageHtmlContentCommand : CommandBase, ICommand<PageContentViewModel, SavePageHtmlContentResponse>
    {
        private readonly IContentService contentService;

        public SavePageHtmlContentCommand(IContentService contentService)
        {
            this.contentService = contentService;
        }

        public SavePageHtmlContentResponse Execute(PageContentViewModel request)
        {            
            var pageContent = new PageContent();
            pageContent.Page = Repository.AsProxy<Root.Models.Page>(request.PageId);
            pageContent.Region = Repository.AsProxy<Region>(request.RegionId);

            UnitOfWork.BeginTransaction();

            var max = Repository.AsQueryable<PageContent>().Where(f => f.Page.Id == request.PageId && !f.IsDeleted).Select(f => (int?)f.Order).Max();
            if (max == null)
            {
                pageContent.Order = 0;
            }
            else
            {
                pageContent.Order = max.Value + 1;
            }

            pageContent.Content = contentService.SaveContentWithStatusUpdate(
                new HtmlContent
                    {
                        Id = request.ContentId,
                        Name = request.ContentName,
                        ActivationDate = request.LiveFrom,
                        ExpirationDate = request.LiveTo,
                        Html = request.PageContent ?? string.Empty                        
                    },
                request.DesirableStatus);
            
            Repository.Save(pageContent);
            UnitOfWork.Commit();

            return new SavePageHtmlContentResponse {
                                                       PageContentId = pageContent.Id,
                                                       ContentId = pageContent.Content.Id
                                                   };
        }
    }
}