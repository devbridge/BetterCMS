using System;

using Autofac;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;
using BetterCms.Core.DataAccess;
using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Pages.Api.DataContracts;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;

using BetterCms.Module.Root.Models;

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

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<Tag> GetTags(GetTagsRequest request = null)
        {
            try
            {
                if (request == null)
                {
                    request = new GetTagsRequest();
                }
                request.SetDefaultOrder(t => t.Name);

                return Repository.ToDataListResponse(request);
            }
            catch (Exception inner)
            {
                const string message = "Failed to get tags list.";
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of redirect entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of redirect entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<Redirect> GetRedirects(GetRedirectsRequest request = null)
        {
            try
            {
                if (request == null)
                {
                    request = new GetRedirectsRequest();
                }
                request.SetDefaultOrder(s => s.PageUrl);

                return Repository.ToDataListResponse(request);
            }
            catch (Exception inner)
            {
                const string message = "Failed to get redirects list.";
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of category entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of category entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<Category> GetCategories(GetCategoriesRequest request = null)
        {
            try
            {
                if (request == null)
                {
                    request = new GetCategoriesRequest();
                }
                request.SetDefaultOrder(s => s.Name);

                return Repository.ToDataListResponse(request);
            }
            catch (Exception inner)
            {
                const string message = "Failed to get categories list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}