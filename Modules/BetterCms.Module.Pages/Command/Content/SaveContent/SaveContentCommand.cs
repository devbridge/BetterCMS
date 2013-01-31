using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.SaveContent
{
    public class SaveContentCommand : CommandBase, ICommand<PageContentViewModel, Guid>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Guid Execute(PageContentViewModel request)
        {
            var isNewContent = request.Id.HasDefaultValue();
            var pageContent = isNewContent ?  new PageContent() : Repository.First<PageContent>(request.Id);

            pageContent.Page = Repository.AsProxy<PageProperties>(request.PageId);
            pageContent.Region = Repository.AsProxy<Region>(request.RegionId);

            if (isNewContent)
            {
                var max = Repository.AsQueryable<PageContent>().Where(f => f.Page.Id == request.PageId).Select(f => (int?)f.Order).Max();
                pageContent.Order = max ?? 1;
                pageContent.Status = request.DesirableStatus;
            }

            pageContent.Content = new HtmlContent
                {
                    Name = request.ContentName,                    
                    ActivationDate = request.LiveFrom, 
                    ExpirationDate = request.LiveTo, 
                    Html = request.PageContent ?? string.Empty, 
                    Version = request.Version,
                    UseCustomCss = request.EnabledCustomCss,
                    UseCustomJs = request.EanbledCustomJs,
                    CustomCss = request.CustomCss,
                    CustomJs = request.CustomJs
                };

            Repository.Save(pageContent);
            UnitOfWork.Commit();

            return pageContent.Id;
        }
    }
}