using NHibernate;
using NHibernate.Criterion;

namespace Devbridge.Platform.Core.DataAccess.DataContext
{
    /// <summary>
    /// NHibernate extensions container.
    /// </summary>
    public static class RestrictionsExtensions
    {
        /// <summary>
        /// Determines whether given property values is null or white space.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Criterion with logic to determine whether given property values is null or white space.</returns>
        public static ICriterion IsNullOrWhiteSpace(IProjection property)
        {
            return Restrictions.Or(Restrictions.IsNull(property), Restrictions.Eq(Projections.SqlFunction("trim", NHibernateUtil.String, property), string.Empty));
        }
    }
}
