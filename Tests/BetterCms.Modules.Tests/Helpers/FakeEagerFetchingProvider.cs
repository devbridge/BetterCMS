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
