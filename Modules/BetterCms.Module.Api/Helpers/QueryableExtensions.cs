// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryableExtensions.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations;

using NHibernate.Linq;

namespace BetterCms.Module.Api.Helpers
{
    public static class QueryableExtensions
    {
        public static DataListResponse<TModel> ToDataListResponse<TModel, TRequestModel>(this IQueryable<TModel> query, RequestBase<TRequestModel> request)
            where TRequestModel : DataOptions, new()
        {
            return query.ToDataListResponse(request.Data);
        }

        public static DataListResponse<TModel> ToDataListResponse<TModel>(this IQueryable<TModel> query, DataOptions options)
        {
            var creator = new DataOptionsQueryCreator<TModel>(options);

            query = query.ApplyFilter(options, creator);

            if (options.HasPaging())
            {
                var totalCount = query.ToRowCountFutureValue();

                query = query.ApplyOrder(options, creator);
                query = query.ApplyPaging(options);
                var items = query.ToFuture();

                return new DataListResponse<TModel>(items.ToList(), totalCount.Value);
            }
            else
            {
                query = query.ApplyOrder(options, creator);
                var items = query.ToList();

                return new DataListResponse<TModel>(items, items.Count);
            }
        }

        public static IQueryable<TModel> ApplyFilter<TModel>(this IQueryable<TModel> query, DataOptions options, DataOptionsQueryCreator<TModel> creator = null)
        {
            if (options != null
                && options.Filter != null
                && ((options.Filter.Where != null && options.Filter.Where.Count > 0)
                    || (options.Filter.Inner != null && options.Filter.Inner.Count > 0)))
            {
                if (creator == null)
                {
                    creator = new DataOptionsQueryCreator<TModel>(options);
                }

                query = query.Where(creator.GetFilterQuery(), creator.GetFilterParameters());
            }

            return query;
        }

        public static IQueryable<TModel> ApplyOrder<TModel>(this IQueryable<TModel> query, DataOptions options, DataOptionsQueryCreator<TModel> creator = null)
        {
            if (options != null && options.Order != null && options.Order.By != null && options.Order.By.Count > 0)
            {
                if (creator == null)
                {
                    creator = new DataOptionsQueryCreator<TModel>(options);
                }

                query = query.OrderBy(creator.GetOrderQuery());
            }

            return query;
        }

        public static IQueryable<TModel> ApplyPaging<TModel>(this IQueryable<TModel> query, DataOptions options)
        {
            if (options.HasPaging())
            {
                if (options.Skip > 0)
                {
                    query = query.Skip(options.Skip.Value).Take(options.Take.Value).Cast<TModel>();
                }
                else
                {
                    query = query.Take(options.Take.Value).Cast<TModel>();
                }
            }

            return query;
        }
    }
}