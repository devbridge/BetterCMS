using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.SaveContent
{
    public class SaveContentCommand : CommandBase, ICommand<PageContentViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Execute(PageContentViewModel request)
        {
            var content = !request.Id.HasDefaultValue() ? Repository.First<PageContent>(request.Id) : new PageContent();

            content.Page = Repository.AsProxy<PageProperties>(request.PageId);
            content.Region = Repository.AsProxy<Region>(request.RegionId);

            content.Content = new HtmlContent
                {
                    Name = request.ContentName,
                    
                    ActivationDate = request.LiveFrom, 
                    ExpirationDate = request.LiveTo, 
                    Html = request.PageContent ?? string.Empty, 
                    Version = request.Version
                };

            Repository.Save(content);
            UnitOfWork.Commit();

            return true;
        }
    }
}