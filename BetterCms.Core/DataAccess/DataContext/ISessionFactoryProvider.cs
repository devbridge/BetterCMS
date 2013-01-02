using NHibernate;

namespace BetterCms.Core.DataAccess.DataContext
{
    public interface ISessionFactoryProvider
    {       
        /// <summary>
        /// Opens a session.
        /// </summary>
        /// <param name="trackEntitiesConcurrency">if set to <c>true</c> tracks entities concurrency.</param>
        /// <returns>An opened session object.</returns>       
        ISession OpenSession(bool trackEntitiesConcurrency = true);
    }
}
