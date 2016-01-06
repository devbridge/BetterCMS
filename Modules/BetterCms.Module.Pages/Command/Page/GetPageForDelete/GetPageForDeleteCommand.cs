using System;
using System.Linq;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

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
            var page = Repository
                .AsQueryable<PageProperties>(p => p.Id == request)
                .Fetch(p => p.PagesView)
                .FirstOne();
            string message = null;

            if (page.IsMasterPage && Repository.AsQueryable<MasterPage>(mp => mp.Master == page).Any())
            {
                message = PagesGlobalization.DeletePageCommand_MasterPageHasChildren_Message;
            }

            var model = new DeletePageViewModel
                {
                    PageId = page.Id,
                    Version = page.Version,
                    IsInSitemap = page.PagesView.IsInSitemap,
                    ValidationMessage = message
                };
            model.UpdateSitemap = model.IsInSitemap;
            return model;
        }
    }
}