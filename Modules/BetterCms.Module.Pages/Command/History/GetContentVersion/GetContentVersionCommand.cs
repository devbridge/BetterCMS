using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Api.Interfaces.Models;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;

using NHibernate.Linq;

using ContentEntity = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Pages.Command.History.GetContentVersion
{
    /// <summary>
    /// Command for getting page content version
    /// </summary>
    public class GetContentVersionCommand : CommandBase, ICommand<Guid, RenderPageViewModel>
    {
        private const string regionIdentifier = "VersionContent";
        private const string regionId = "41195FE2-DB5D-412E-A648-ED02B279C8F3";

        /// <summary>
        /// Gets or sets the page content projection factory.
        /// </summary>
        /// <value>
        /// The page content projection factory.
        /// </value>
        public PageContentProjectionFactory PageContentProjectionFactory { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public RenderPageViewModel Execute(Guid request)
        {
            // Creating fake region
            var regionGuid = new Guid(regionId);
            var region = new Region { Id = regionGuid, RegionIdentifier = regionIdentifier};
            var regionViewModel = new PageRegionViewModel { RegionId = regionGuid, RegionIdentifier = regionIdentifier };

            // Creating fake page content and loading it's childs
            var pageContent = new PageContent
                                  {
                                      Options = new List<PageContentOption>(),
                                      Region = region
                                  };

            pageContent.Content = Repository
                .AsQueryable<ContentEntity>(c => c.Id == request)
                .FetchMany(f => f.ContentOptions)
                .FirstOrDefault();

            List<IOption> options = new List<IOption>();
            options.AddRange(pageContent.Options);

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