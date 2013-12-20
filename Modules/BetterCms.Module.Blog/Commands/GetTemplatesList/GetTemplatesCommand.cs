using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Blog.Commands.GetTemplatesList
{
    public class GetTemplatesCommand : CommandBase, ICommand<bool, IList<BlogTemplateViewModel>>
    {
        /// <summary>
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTemplatesCommand" /> class.
        /// </summary>
        /// <param name="optionService">The option service.</param>
        public GetTemplatesCommand(IOptionService optionService)
        {
            this.optionService = optionService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The list of blog template view models</returns>
        public IList<BlogTemplateViewModel> Execute(bool request)
        {
            BlogTemplateViewModel modelAlias = null;
            Layout layoutAlias = null;

            // Load templates
            var templates = UnitOfWork.Session
                .QueryOver(() => layoutAlias)
                .Where(() => !layoutAlias.IsDeleted)
                .SelectList(select => select
                    .Select(() => layoutAlias.Id).WithAlias(() => modelAlias.TemplateId)
                    .Select(() => layoutAlias.Name).WithAlias(() => modelAlias.Title)
                    .Select(() => layoutAlias.PreviewUrl).WithAlias(() => modelAlias.PreviewUrl))
                .TransformUsing(Transformers.AliasToBean<BlogTemplateViewModel>())
                .List<BlogTemplateViewModel>();

            var compatibleLayouts = Repository.AsQueryable<Layout>()
                      .Where(
                          layout =>
                          layout.LayoutRegions.Count(region => !region.IsDeleted && !region.Region.IsDeleted).Equals(1)
                          || layout.LayoutRegions.Any(region => !region.IsDeleted && !region.Region.IsDeleted && region.Region.RegionIdentifier == BlogModuleConstants.BlogPostMainContentRegionIdentifier))
                      .Select(layout => layout.Id)
                      .ToList();

            foreach (var id in compatibleLayouts)
            {
                templates
                    .Where(t => t.TemplateId == id)
                    .ToList()
                    .ForEach(t => t.IsCompatible = true);
            }

            Repository.AsQueryable<PageProperties>()
                      .Where(page => page.IsMasterPage && !page.IsDeleted)
                      .Select(
                          page =>
                          new BlogTemplateViewModel()
                              {
                                  TemplateId = page.Id,
                                  Title = page.Title,
                                  PreviewUrl =
                                      page.Image != null
                                          ? page.Image.PublicUrl
                                          : page.FeaturedImage != null ? page.FeaturedImage.PublicUrl : page.SecondaryImage != null ? page.SecondaryImage.PublicUrl : null,
                                  IsMasterPage = true,
                                  IsCompatible =
                                      page.PageContents.Count(
                                          pageContnet =>
                                          !pageContnet.IsDeleted && !pageContnet.Content.IsDeleted
                                          && pageContnet.Content.ContentRegions.Any(contentRegion => !contentRegion.IsDeleted && !contentRegion.Region.IsDeleted)).Equals(1)
                                      && page.PageContents.Count(
                                          pageContnet =>
                                          !pageContnet.IsDeleted && !pageContnet.Content.IsDeleted
                                          && pageContnet.Content.ContentRegions.Count(contentRegion => !contentRegion.IsDeleted && !contentRegion.Region.IsDeleted).Equals(1))
                                             .Equals(1)
                              })
                      .ToList()
                      .ForEach(templates.Add);

            var option = optionService.GetDefaultOption();

            // Load default template.
            if (option != null && (option.DefaultLayout != null || option.DefaultMasterPage != null))
            {
                var id = option.DefaultLayout != null ? option.DefaultLayout.Id : option.DefaultMasterPage.Id;

                var defaultTemplate = templates.FirstOrDefault(t => t.TemplateId == id);
                if (defaultTemplate != null)
                {
                    defaultTemplate.IsActive = true;
                }
            }

            return templates;
        }
    }
}