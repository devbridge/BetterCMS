using System;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Content.GetPageHtmlContent
{
    public class GetPageHtmlContentCommand : CommandBase, ICommand<Guid, PageContentViewModel>
    {
        public virtual IContentService ContentService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns></returns>        
        public PageContentViewModel Execute(Guid pageContentId)
        {
            var pageContentForEdit = ContentService.GetPageContentForEdit(pageContentId);

            if (pageContentForEdit == null)
            {
                throw new EntityNotFoundException(typeof(PageContent), pageContentId);
            }

            PageContent pageContent = pageContentForEdit.Item1;
            HtmlContent content = (HtmlContent)pageContentForEdit.Item2;

            return new PageContentViewModel 
                                            {
                                                Id = pageContent.Id,
                                                PageId = pageContent.Page.Id,
                                                RegionId = pageContent.Region.Id,
                                                ContentId = pageContent.Content.Id,
                                                ContentName = content.Name,
                                                LiveFrom = content.ActivationDate,
                                                LiveTo = content.ExpirationDate,
                                                PageContent = content.Html,
                                                Version = pageContent.Version,
                                                ContentVersion = pageContent.Content.Version,
                                                CustomCss = content.CustomCss,
                                                CustomJs = content.CustomJs,
                                                EanbledCustomJs = content.UseCustomJs,
                                                EnabledCustomCss = content.UseCustomCss,
                                                EditInSourceMode = content.EditInSourceMode,
                                                CurrentStatus = content.Status,
                                                HasPublishedContent = content.Original != null
                                            };
        }
    }
}