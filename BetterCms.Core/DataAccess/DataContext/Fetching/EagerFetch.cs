using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Autofac;

using BetterCms.Core.Dependencies;

namespace BetterCms.Core.DataAccess.DataContext.Fetching
{
    public static class EagerFetch
    {
        private static IFetchingProvider FetchingProvider
        {
            get
            {
                return ContextScopeProvider.ApplicationContainer.Resolve<IFetchingProvider>();
            }
        }

        public static IFetchRequest<TOriginating, TRelated> Fetch<TOriginating, TRelated>(
            this IQueryable<TOriginating> query, Expression<Func<TOriginating, TRelated>> relatedObjectSelector)
        {
            return FetchingProvider.Fetch(query, relatedObjectSelector);
        }

        public static IFetchRequest<TOriginating, TRelated> FetchMany<TOriginating, TRelated>(
            this IQueryable<TOriginating> query,
            Expression<Func<TOriginating, IEnumerable<TRelated>>> relatedObjectSelector)
        {
            return FetchingProvider.FetchMany(query, relatedObjectSelector);
        }

        public static IFetchRequest<TQueried, TRelated> ThenFetch<TQueried, TFetch, TRelated>(
            this IFetchRequest<TQueried, TFetch> query, Expression<Func<TFetch, TRelated>> relatedObjectSelector)
        {
            return FetchingProvider.ThenFetch(query, relatedObjectSelector);
        }

        public static IFetchRequest<TQueried, TRelated> ThenFetchMany<TQueried, TFetch, TRelated>(
            this IFetchRequest<TQueried, TFetch> query,
            Expression<Func<TFetch, IEnumerable<TRelated>>> relatedObjectSelector)
        {
            return FetchingProvider.ThenFetchMany(query, relatedObjectSelector);
        }
    }
}
