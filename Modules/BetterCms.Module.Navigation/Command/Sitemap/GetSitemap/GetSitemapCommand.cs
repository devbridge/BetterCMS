using System;
using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Navigation.Services;
using BetterCms.Module.Navigation.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Navigation.Command.Sitemap.GetSitemap
{
    /// <summary>
    /// Command to get media image data.
    /// </summary>
    public class GetSitemapCommand : CommandBase, ICommand<string, SearchableSitemapViewModel>
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
        /// <returns>Sitemap root nodes.</returns>
        public SearchableSitemapViewModel Execute(string request)
        {
            //var rootNodes = SitemapService.GetRootNodes(request);

            // TODO: implement.
            // Search for root sitemap nodes.

            return new SearchableSitemapViewModel
                {
                    SearchQuery = request,
                    RootNodes =
                        new List<SitemapNodeViewModel>()
                            {
                                new SitemapNodeViewModel()
                                    {
                                        Id = Guid.NewGuid(),
                                        Version = 0,
                                        Title = "Error 505",
                                        Url = "/505",
                                        DisplayOrder = 0,
                                        ChildNodes =
                                            new List<SitemapNodeViewModel>()
                                                {
                                                    new SitemapNodeViewModel() { Id = Guid.NewGuid(), Version = 0, Title = "2", Url = "/2", DisplayOrder = 2 },
                                                    new SitemapNodeViewModel() { Id = Guid.NewGuid(), Version = 0, Title = "1", Url = "/1", DisplayOrder = 1 }
                                                }
                                    },
                                new SitemapNodeViewModel()
                                    {
                                        Id = Guid.NewGuid(),
                                        Version = 0,
                                        Title = "all",
                                        Url = "/all",
                                        DisplayOrder = 0,
                                        ChildNodes =
                                            new List<SitemapNodeViewModel>()
                                                {
                                                    new SitemapNodeViewModel() { Id = Guid.NewGuid(), Version = 0, Title = "3", Url = "/2", DisplayOrder = 2 },
                                                    new SitemapNodeViewModel() { Id = Guid.NewGuid(), Version = 0, Title = "4", Url = "/1", DisplayOrder = 1 },
                                                    new SitemapNodeViewModel() { Id = Guid.NewGuid(), Version = 0, Title = "2", Url = "/2", DisplayOrder = 2 },
                                                    new SitemapNodeViewModel() { Id = Guid.NewGuid(), Version = 0, Title = "1", Url = "/1", DisplayOrder = 1 }
                                                }
                                    },
                                new SitemapNodeViewModel() { Id = Guid.NewGuid(), Version = 0, Title = "Not found 404", Url = "/404", DisplayOrder = 0 },
                                new SitemapNodeViewModel() { Id = Guid.NewGuid(), Version = 0, Title = "x", Url = "/x/x/x/x/x/x/xx/xxxxxxxxx/", DisplayOrder = 0 }
                            }
                };
        }
    }
}