using System;
using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Page.GetPageProperties
{
    /// <summary>
    /// Command for getting view model with page properties
    /// </summary>
    public class GetPagePropertiesCommand : CommandBase, ICommand<Guid, EditPagePropertiesViewModel>
    {
        /// <summary>
        /// The author service
        /// </summary>
        private IAuthorService authorService;

        /// <summary>
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagePropertiesCommand" /> class.
        /// </summary>
        /// <param name="authorService">The author service.</param>
        /// <param name="tagService">The tag service.</param>
        public GetPagePropertiesCommand(IAuthorService authorService, ITagService tagService)
        {
            this.authorService = authorService;
            this.tagService = tagService;
        }

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

                model.Tags = tagService.GetPageTagNames(id);
            }
            else
            {
                model = new EditPagePropertiesViewModel();
            }

            model.Authors = authorService.GetAuthors();


            return model;
        }
    }
}