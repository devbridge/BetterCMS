using System;
using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Navigation.Services;
using BetterCms.Module.Navigation.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Navigation.Command.Sitemap.SaveSitemapNode
{

    public class SaveSitemapNodeCommand : CommandBase, ICommand<SitemapNodeViewModel, SitemapNodeViewModel>
    {
        /// <summary>
        /// Gets or sets the sitemap service.
        /// </summary>
        /// <value>
        /// The sitemap service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Execution result.</returns>
        public SitemapNodeViewModel Execute(SitemapNodeViewModel request)
        {
            //var rootNodes = SitemapService.GetRootNodes(request);

            // TODO: implement.
            // Search for root sitemap nodes.

            return new SitemapNodeViewModel();
        }
    }
}