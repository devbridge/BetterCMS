using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Page.GetPageForDelete
{
    public class GetPageForDeleteCommand : CommandBase, ICommand<Guid, DeletePageViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public DeletePageViewModel Execute(Guid request)
        {
            DeletePageViewModel model;

            PageProperties alias = null;
            DeletePageViewModel modelAlias = null;

            model =
                UnitOfWork.Session.QueryOver(() => alias)
                          .Where(p => p.Id == request)
                          .SelectList(select => select
                              .Select(() => alias.Id).WithAlias(() => modelAlias.PageId)
                              .Select(() => alias.Version).WithAlias(() => modelAlias.Version))
                          .TransformUsing(Transformers.AliasToBean<DeletePageViewModel>())
                          .SingleOrDefault<DeletePageViewModel>();

            return model ?? new DeletePageViewModel();
        }
    }
}