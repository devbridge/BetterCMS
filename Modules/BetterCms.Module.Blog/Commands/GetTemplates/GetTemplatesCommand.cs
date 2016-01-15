// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetTemplatesCommand.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Transform;

namespace BetterCms.Module.Blog.Commands.GetTemplates
{
    public class GetTemplatesCommand : CommandBase, ICommand<bool, IList<BlogTemplateViewModel>>
    {
        /// <summary>
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTemplatesCommand" /> class.
        /// </summary>
        /// <param name="optionService">The option service.</param>
        /// <param name="configuration">The configuration.</param>
        public GetTemplatesCommand(IOptionService optionService, ICmsConfiguration configuration)
        {
            this.optionService = optionService;
            this.configuration = configuration;
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

            // Get current default template
            var option = optionService.GetDefaultOption();
            System.Guid? selectedTemplateId = null;
            if (option != null && (option.DefaultLayout != null || option.DefaultMasterPage != null))
            {
                selectedTemplateId = option.DefaultLayout != null ? option.DefaultLayout.Id : option.DefaultMasterPage.Id;
            }

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

            var mainContentIdentifier = BlogModuleConstants.BlogPostMainContentRegionIdentifier.ToLowerInvariant();
            var compatibleLayouts = Repository.AsQueryable<Layout>()
                      .Where(
                          layout =>
                          layout.LayoutRegions.Count(region => !region.IsDeleted && !region.Region.IsDeleted).Equals(1)
                          || layout.LayoutRegions.Any(region => !region.IsDeleted && !region.Region.IsDeleted && region.Region.RegionIdentifier.ToLowerInvariant() == mainContentIdentifier))
                      .Select(layout => layout.Id)
                      .ToList();

            foreach (var id in compatibleLayouts)
            {
                templates
                    .Where(t => t.TemplateId == id)
                    .ToList()
                    .ForEach(t => t.IsCompatible = true);
            }

            var masterPagesQuery = Repository
                .AsQueryable<PageProperties>()
                .Where(page => page.IsMasterPage && !page.IsDeleted);

            if (configuration.Security.AccessControlEnabled)
            {
                var deniedPages = AccessControlService.GetDeniedObjects<PageProperties>();
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    if (id == selectedTemplateId)
                    {
                        continue;
                    }
                    masterPagesQuery = masterPagesQuery.Where(f => f.Id != id);
                }
            }

            masterPagesQuery
                .Select(
                    page =>
                    new BlogTemplateViewModel
                    {
                        TemplateId = page.Id,
                        Title = page.Title,
                        PreviewUrl =
                            page.Image != null
                                ? page.Image.PublicUrl
                                : page.FeaturedImage != null ? page.FeaturedImage.PublicUrl : page.SecondaryImage != null ? page.SecondaryImage.PublicUrl : null,
                        IsMasterPage = true,
                        IsCompatible =
                            page.PageContents.Count(pageContent =>
                                !pageContent.IsDeleted && !pageContent.Content.IsDeleted
                                    && pageContent.Content.ContentRegions.Count(contentRegion => !contentRegion.IsDeleted && !contentRegion.Region.IsDeleted
                                        && contentRegion.Region.RegionIdentifier.ToLowerInvariant() == mainContentIdentifier).Equals(1)
                            ).Equals(1)

                            || page.PageContents.Count(pageContent =>
                                !pageContent.IsDeleted && !pageContent.Content.IsDeleted
                                    && pageContent.Content.ContentRegions.Count(contentRegion => !contentRegion.IsDeleted && !contentRegion.Region.IsDeleted).Equals(1)
                            ).Equals(1)
                    })
                .ToList()
                .ForEach(templates.Add);

            // Select default template.
            if (selectedTemplateId.HasValue)
            {
                var defaultTemplate = templates.FirstOrDefault(t => t.TemplateId == selectedTemplateId);
                if (defaultTemplate != null)
                {
                    defaultTemplate.IsActive = true;
                }
            }

            return templates;
        }
    }
}