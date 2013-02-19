using System;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

using NHibernate.Transform;

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
            PageProperties alias = null;
            DeletePageViewModel modelAlias = null;

            return UnitOfWork.Session.QueryOver(() => alias)
                          .Where(p => p.Id == request && !p.IsDeleted)
                          .SelectList(select => select
                              .Select(() => alias.Id).WithAlias(() => modelAlias.PageId)
                              .Select(() => alias.Version).WithAlias(() => modelAlias.Version))
                          .TransformUsing(Transformers.AliasToBean<DeletePageViewModel>())
                          .First<DeletePageViewModel, PageProperties>();
        }
    }
}