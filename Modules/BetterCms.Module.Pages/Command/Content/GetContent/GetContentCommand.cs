using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.GetContent
{
    public class GetContentCommand : CommandBase, ICommand<Guid, PageContentViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns></returns>        
        public PageContentViewModel Execute(Guid pageContentId)
        {
            return
                Repository.AsQueryable<PageContent>()
                          .Where(f => f.Id == pageContentId)
                          .Select(
                              f =>
                              new PageContentViewModel
                                  {
                                      Id = f.Id,
                                      PageId = f.Page.Id,
                                      RegionId = f.Region.Id,
                                      ContentName = f.Content.Name,
                                      LiveFrom = ((HtmlContent)f.Content).ActivationDate,
                                      LiveTo = ((HtmlContent)f.Content).ExpirationDate,
                                      PageContent = ((HtmlContent)f.Content).Html,
                                      Version = f.Version,
                                  })
                          .FirstOrDefault();
        }
    }
}