// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterByTagExtensions.cs" company="Devbridge Group LLC">
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