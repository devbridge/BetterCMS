using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Page.GetPageForCloning
{   
    public class GetPageForCloningCommand : CommandBase, ICommand<Guid, ClonePageViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ClonePageViewModel Execute(Guid request)
        {
            ClonePageViewModel model;

            Root.Models.Page alias = null;
            ClonePageViewModel modelAlias = null;

            model = UnitOfWork.Session.QueryOver(() => alias)
                    .Where(p => p.Id == request)
                    .SelectList(select => select
                        .Select(() => alias.Id).WithAlias(() => modelAlias.PageId)
                        .Select(() => alias.Version).WithAlias(() => modelAlias.Version))
                    .TransformUsing(Transformers.AliasToBean<ClonePageViewModel>())
                    .SingleOrDefault<ClonePageViewModel>();

            return model ?? new ClonePageViewModel();
        }
    }
}