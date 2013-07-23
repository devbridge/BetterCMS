using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Services;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public partial class PagesApiContext : DataApiContext
    {
        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// The history service.
        /// </summary>
        private readonly IHistoryService historyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagesApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="historyService">The history service.</param>
        public PagesApiContext(ILifetimeScope lifetimeScope, IRepository repository = null, ISitemapService sitemapService = null, IHistoryService historyService = null)
            : base(lifetimeScope, repository)
        {
            if (historyService == null)
            {
                this.historyService = Resolve<IHistoryService>();
            }
            else
            {
                this.historyService = historyService;
            }

            if (sitemapService == null)
            {
                this.sitemapService = Resolve<ISitemapService>();
            }
            else
            {
                this.sitemapService = sitemapService;
            }
        }
    }
}