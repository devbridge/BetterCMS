using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Root.ViewModels.Option;

using NHibernate.Linq;

using ContentEntity = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Pages.Command.History.GetContentVersion
{
    /// <summary>
    /// Command for getting page content version.
    /// </summary>
    public class GetContentVersionCommand : CommandBase, ICommand<Guid, RenderPageViewModel>
    {
        /// <summary>
        /// The region identifier.
        /// </summary>
        private const string regionIdentifier = "VersionContent";

        /// <summary>
        /// The region id.
        /// </summary>
        private const string regionId = "41195FE2-DB5D-412E-A648-ED02B279C8F3";

        /// <summary>
        /// Gets or sets the page content projection factory.
        /// </summary>
        /// <value>
        /// The page content projection factory.
        /// </value>
        public PageContentProjectionFactory PageContentProjectionFactory { get; set; }

        /// <summary>
        /// Gets or sets the option service.
        /// </summary>
        /// <value>
        /// The option service.
        /// </value>
        public IOptionService OptionService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Render page view model.</returns>
        public RenderPageViewModel Execute(Guid request)
        {
            // Creating fake region.
            var regionGuid = new Guid(regionId);
            var region = new Region { Id = regionGuid, RegionIdentifier = regionIdentifier };
            var regionViewModel = new PageRegionViewModel { RegionId = regionGuid, RegionIdentifier = regionIdentifier };

            // Creating fake page content and loading it's children.
            var pageContent = new PageContent
                                  {
                                      Options = new List<PageContentOption>(),
                                      Region = region
                                  };

            pageContent.Content = Repository
                .AsQueryable<ContentEntity>(c => c.Id == request)
                .FetchMany(f => f.ContentOptions)
                .FirstOrDefault();

            if (pageContent.Content != null)
            {
                var contentType = pageContent.Content.GetType();
                if (contentType == typeof(HtmlContentWidget) || contentType == typeof(ServerControlWidget))
                {
                    DemandAccess(RootModuleConstants.UserRoles.Administration);
                }
                else
                {
                    DemandAccess(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent);
                }
            }

            var options = OptionService.GetMergedOptionValues(pageContent.Options, pageContent.Content.ContentOptions);

            var contentProjection = PageContentProjectionFactory.Create(pageContent, pageContent.Content, options);

            return new RenderPageViewModel
                       {
                           Contents = new List<PageContentProjection> { contentProjection },
                           Stylesheets = new List<IStylesheetAccessor> { contentProjection },
                           JavaScripts = new List<IJavaScriptAccessor> { contentProjection },
                           Regions = new List<PageRegionViewModel> { regionViewModel }
                       };
        }
    }
}