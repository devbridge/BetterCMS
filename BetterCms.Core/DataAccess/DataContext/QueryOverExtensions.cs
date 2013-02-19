using System;

using BetterCms.Core.Exceptions.DataTier;

using NHibernate;
using NHibernate.Criterion;

namespace BetterCms.Core.DataAccess.DataContext
{
    /// <summary>
    /// NHibernate extensions container.
    /// </summary>
    public static class QueryOverExtensions
    {
        public static TReturn First<TReturn, TInput>(this IQueryOver<TInput> query)
        {
            var viewModel = query.SingleOrDefault<TReturn>();

            if (viewModel == null)
            {
                throw new EntityNotFoundException(typeof(TReturn), Guid.Empty);
            }

            return viewModel;
        }
    }
}
