using System.Linq;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Api.Helpers
{
    public static class FilterByTagExtensions
    {
        public static IQueryable<TModel> ApplyPageTagsFilter<TModel>(this IQueryable<TModel> query, IFilterByTags tagsFilter) 
            where TModel : PageProperties
        {
            if (tagsFilter != null && tagsFilter.FilterByTags != null)
            {
                var tags = tagsFilter.FilterByTags.Where(tag => !string.IsNullOrWhiteSpace(tag)).Distinct().ToArray();

                if (tags.Length > 0)
                {
                    if (tagsFilter.FilterByTagsConnector == FilterConnector.And)
                    {
                        query = query.Where(page => page.PageTags.Count(pageTag => tags.Contains(pageTag.Tag.Name) && !pageTag.IsDeleted && !pageTag.Tag.IsDeleted) == tags.Length);
                    }
                    else
                    {
                        query = query.Where(page => page.PageTags.Any(pageTag => tags.Contains(pageTag.Tag.Name) && !pageTag.IsDeleted && !pageTag.Tag.IsDeleted));
                    }
                }
            }

            return query;
        }
        
        public static IQueryable<TModel> ApplyMediaTagsFilter<TModel>(this IQueryable<TModel> query, IFilterByTags tagsFilter) 
            where TModel : Media
        {
            if (tagsFilter != null && tagsFilter.FilterByTags != null)
            {
                var tags = tagsFilter.FilterByTags.Where(tag => !string.IsNullOrWhiteSpace(tag)).Distinct().ToArray();

                if (tags.Length > 0)
                {
                    if (tagsFilter.FilterByTagsConnector == FilterConnector.And)
                    {
                        query = query.Where(page => page.MediaTags.Count(pageTag => tags.Contains(pageTag.Tag.Name) && !pageTag.IsDeleted && !pageTag.Tag.IsDeleted) == tags.Length);
                    }
                    else
                    {
                        query = query.Where(page => page.MediaTags.Any(pageTag => tags.Contains(pageTag.Tag.Name) && !pageTag.IsDeleted && !pageTag.Tag.IsDeleted));
                    }
                }
            }

            return query;
        }

        public static IQueryable<TModel> ApplySitemapTagsFilter<TModel>(this IQueryable<TModel> query, IFilterByTags tagsFilter)
            where TModel : Sitemap
        {
            if (tagsFilter != null && tagsFilter.FilterByTags != null)
            {
                var tags = tagsFilter.FilterByTags.Where(tag => !string.IsNullOrWhiteSpace(tag)).Distinct().ToArray();

                if (tags.Length > 0)
                {
                    if (tagsFilter.FilterByTagsConnector == FilterConnector.And)
                    {
                        query = query.Where(page => page.SitemapTags.Count(pageTag => tags.Contains(pageTag.Tag.Name) && !pageTag.IsDeleted && !pageTag.Tag.IsDeleted) == tags.Length);
                    }
                    else
                    {
                        query = query.Where(page => page.SitemapTags.Any(pageTag => tags.Contains(pageTag.Tag.Name) && !pageTag.IsDeleted && !pageTag.Tag.IsDeleted));
                    }
                }
            }

            return query;
        }
    }
}