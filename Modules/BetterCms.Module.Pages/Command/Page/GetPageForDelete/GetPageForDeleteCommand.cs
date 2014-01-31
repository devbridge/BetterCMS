using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Page.GetPageForDelete
{
    /// <summary>
    /// Command for page delete confirmation.
    /// </summary>
    public class GetPageForDeleteCommand : CommandBase, ICommand<Guid, DeletePageViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Delete confirmation view model.</returns>
        public DeletePageViewModel Execute(Guid request)
        {
            var inSitemapFuture = Repository.AsQueryable<SitemapNode>().Where(node => node.Page.Id == request && !node.IsDeleted && !node.Sitemap.IsDeleted).Select(node => node.Id).ToFuture();
            var page = Repository.First<PageProperties>(request);
            string message = null;

            if (page.IsMasterPage && Repository.AsQueryable<MasterPage>(mp => mp.Master == page).Any())
            {
                message = PagesGlobalization.DeletePageCommand_MasterPageHasChildren_Message;
            }

            var urlHash = page.PageUrl.UrlHash();
            return new DeletePageViewModel
                {
                    PageId = page.Id,
                    Version = page.Version,
                    IsInSitemap = inSitemapFuture.Any() || Repository.AsQueryable<SitemapNode>().Any(node => node.UrlHash == urlHash && !node.IsDeleted && !node.Sitemap.IsDeleted),
                    ValidationMessage = message
                };
        }
    }
}