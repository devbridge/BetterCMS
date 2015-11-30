// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeEagerFetchingProvider.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterModules.Core.DataAccess.DataContext.Fetching;

using NHibernate.Linq;

namespace BetterCms.Test.Module.Helpers
{
    public class FakeEagerFetchingProvider : IFetchingProvider
    {
        private static IFetchingProvider defaultFetchProvider = new DefaultFetchingProvider();

        public IFetchRequest<TOriginating, TRelated> Fetch<TOriginating, TRelated>(IQueryable<TOriginating> query, Expression<Func<TOriginating, TRelated>> relatedObjectSelector)
        {
            if (query is NhQueryable<TOriginating>)
            {
                return defaultFetchProvider.Fetch(query, relatedObjectSelector);
            }
            return new FetchRequest<TOriginating, TRelated>(query);
        }

        public IFetchRequest<TOriginating, TRelated> FetchMany<TOriginating, TRelated>(IQueryable<TOriginating> query, Expression<Func<TOriginating, IEnumerable<TRelated>>> relatedObjectSelector)
        {
            if (query is NhQueryable<TOriginating>)
            {
                return defaultFetchProvider.FetchMany(query, relatedObjectSelector);
            }
            return new FetchRequest<TOriginating, TRelated>(query);
        }

        public IFetchRequest<TQueried, TRelated> ThenFetch<TQueried, TFetch, TRelated>(IFetchRequest<TQueried, TFetch> query, Expression<Func<TFetch, TRelated>> relatedObjectSelector)
        {
            if (query is FetchRequest<TQueried, TFetch>)
            {
                var impl = query as FetchRequest<TQueried, TFetch>;
                return new FetchRequest<TQueried, TRelated>(impl.query);
            }
            return defaultFetchProvider.ThenFetch(query, relatedObjectSelector);
        }

        public IFetchRequest<TQueried, TRelated> ThenFetchMany<TQueried, TFetch, TRelated>(IFetchRequest<TQueried, TFetch> query, Expression<Func<TFetch, IEnumerable<TRelated>>> relatedObjectSelector)
        {
            if (query is FetchRequest<TQueried, TFetch>)
            {
                var impl = query as FetchRequest<TQueried, TFetch>;
                return new FetchRequest<TQueried, TRelated>(impl.query);
            }
            return defaultFetchProvider.ThenFetchMany(query, relatedObjectSelector);
        }

        public class FetchRequest<TQueried, TFetch> : IFetchRequest<TQueried, TFetch>
        {
            public readonly IQueryable<TQueried> query;

            public IEnumerator<TQueried> GetEnumerator()
            {
                return query.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return query.GetEnumerator();
            }

            public Type ElementType
            {
                get { return query.ElementType; }
            }

            public Expression Expression
            {
                get { return query.Expression; }
            }

            public IQueryProvider Provider
            {
                get { return query.Provider; }
            }

            public FetchRequest(IQueryable<TQueried> query)
            {
                this.query = query;
            }
        }
    }
}
