using System;
using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Page.GetPageProperties
{
    /// <summary>
    /// Command for getting view model with page properties
    /// </summary>
    public class GetPagePropertiesCommand : CommandBase, ICommand<Guid, EditPagePropertiesViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="id">The page id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public EditPagePropertiesViewModel Execute(Guid id)
        {
            EditPagePropertiesViewModel model;

            PageProperties alias = null;
            EditPagePropertiesViewModel modelAlias = null;

            model = UnitOfWork.Session.QueryOver(() => alias)
                    .Where(p => p.Id == id)
                    .SelectList(select => select
                        .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                        .Select(() => alias.Title).WithAlias(() => modelAlias.PageName)
                        .Select(() => alias.PageUrl).WithAlias(() => modelAlias.PagePermalink)
                        .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                        .Select(() => alias.CustomCss).WithAlias(() => modelAlias.PageCSS)
                        .Select(() => alias.UseNoFollow).WithAlias(() => modelAlias.UseNoFollow)
                        .Select(() => alias.UseNoIndex).WithAlias(() => modelAlias.UseNoIndex)
                        .Select(() => alias.IsPublic).WithAlias(() => modelAlias.IsVisibleToEveryone)
                        .Select(() => alias.Layout.Id).WithAlias(() => modelAlias.TemplateId)
                        .Select(() => alias.Author.Id).WithAlias(() => modelAlias.AuthorId))
                    .TransformUsing(Transformers.AliasToBean<EditPagePropertiesViewModel>())
                    .SingleOrDefault<EditPagePropertiesViewModel>();

            if (model != null)
            {
                model.FeaturedPageImageUrl = "http://www.agileturas.lt/assets/img/kaunas_sponsors/devbridge_sponsor_logo.png"; // TODO: url ???
                model.FileName = "TODO: FileName"; // TODO
                model.FileSize = "TODO: FileSize"; // TODO
                model.RedirectFromOldUrl = true; // TODO: 

                model.Tags = GetTags(UnitOfWork.Session, id);
                model.Categories = GetCategories(UnitOfWork.Session, id);
            }
            else
            {
                model = new EditPagePropertiesViewModel();
            }

            model.Authors = GetAuthorsList(UnitOfWork.Session);


            return model;
        }

        private IList<string>  GetTags(ISession session, Guid id)
        {
            Root.Models.Tag tagAlias = null;

            return session
                .QueryOver<PageTag>()
                .Where(w => w.Page.Id == id && !w.IsDeleted)
                .JoinAlias(f => f.Tag, () => tagAlias)
                .SelectList(select => select.Select(() => tagAlias.Name))
                .List<string>();
        }

        private IList<string> GetCategories(ISession session, Guid id)
        {
            Category categoryAlias = null;

            return session
                .QueryOver<PageCategory>()
                .Where(w => w.Page.Id == id && !w.IsDeleted)
                .JoinAlias(f => f.Category, () => categoryAlias)
                .SelectList(select => select.Select(() => categoryAlias.Name))
                .List<string>();
        }

        private IList<LookupKeyValue> GetAuthorsList(ISession session)
        {
            Author alias = null;
            LookupKeyValue lookupAlias = null;

            return session
                .QueryOver(() => alias)
                .SelectList(select => select
                    .Select(NHibernate.Criterion.Projections.Cast(NHibernateUtil.String, NHibernate.Criterion.Projections.Property<Author>(c => c.Id))).WithAlias(() => lookupAlias.Key)
                    .Select(() => alias.DisplayName).WithAlias(() => lookupAlias.Value))
                .OrderBy(o => o.DisplayName).Asc()
                .TransformUsing(Transformers.AliasToBean<LookupKeyValue>())
                .List<LookupKeyValue>();
        }
    }
}