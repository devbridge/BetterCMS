using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;
using BetterCms.Module.Pages.Models;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Default service implementation for sitemaps CRUD.
    /// </summary>
    public class SitemapsService : Service, ISitemapsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapsService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        public SitemapsService(IRepository repository, ISitemapService sitemapService)
        {
            this.repository = repository;
            this.sitemapService = sitemapService;
        }

        /// <summary>
        /// Gets the sitemaps list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetSitemapsResponse</c> with tags list.
        /// </returns>
        public GetSitemapsResponse Get(GetSitemapsRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Pages.Models.Sitemap>();

            query = query.ApplySitemapTagsFilter(request.Data);

            var listResponse = query
                .Where(map => !map.IsDeleted)
                .Select(map => new SitemapModel
                {
                    Id = map.Id,
                    Version = map.Version,
                    CreatedBy = map.CreatedByUser,
                    CreatedOn = map.CreatedOn,
                    LastModifiedBy = map.ModifiedByUser,
                    LastModifiedOn = map.ModifiedOn,

                    Title = map.Title
                }).ToDataListResponse(request);

            if (listResponse.Items.Count > 0 && request.Data.IncludeTags)
            {
                LoadTags(listResponse, request.Data.IncludeTags);
            }

            return new GetSitemapsResponse
            {
                Data = listResponse
            };
        }

        /// <summary>
        /// Creates a new sitemap.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostSitemapResponse</c> with a new sitemap id.
        /// </returns>
        public PostSitemapResponse Post(PostSitemapRequest request)
        {
            var result = sitemapService.Put(
                    new PutSitemapRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostSitemapResponse { Data = result.Data };
        }

        /// <summary>
        /// Loads the tags.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="includeTags">if set to <c>true</c> includes tags.</param>
        private void LoadTags(DataListResponse<SitemapModel> response, bool includeTags)
        {
            var pageIds = response.Items.Select(i => i.Id).Distinct().ToArray();
            if (includeTags)
            {
                var tags = repository
                    .AsQueryable<SitemapTag>(pt => pageIds.Contains(pt.Sitemap.Id))
                    .Select(pt => new { PageId = pt.Sitemap.Id, TagName = pt.Tag.Name })
                    .OrderBy(o => o.TagName)
                    .ToFuture()
                    .ToList();

                response.Items.ToList().ForEach(page => { page.Tags = tags.Where(tag => tag.PageId == page.Id).Select(tag => tag.TagName).ToList(); });
            }
        }
    }
}