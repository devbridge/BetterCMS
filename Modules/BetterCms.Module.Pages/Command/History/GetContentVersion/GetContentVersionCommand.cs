using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.GetContentVersion
{
    /// <summary>
    /// Command for getting page content version
    /// </summary>
    public class GetContentVersionCommand : CommandBase, ICommand<Guid, PageContentProjection>
    {
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
        public PageContentProjection Execute(Guid request)
        {
            var content = Repository
                .AsQueryable<PageContent>(c => c.Id == request)
                .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                .FetchMany(f => f.Options)
                .FirstOrDefault();

            var contentProjection = PageContentProjectionFactory.Create(content);
            return contentProjection;
        }
    }
}