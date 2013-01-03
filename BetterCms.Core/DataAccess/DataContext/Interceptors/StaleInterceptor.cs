using System;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Models;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Core.DataAccess.DataContext.Interceptors
{
    /// <summary>
    /// Checks for stale object state
    /// </summary>
    public class StaleInterceptor : EmptyInterceptor
    {
        /// <summary>
        /// Instance of the NHibernate ISession 
        /// </summary>
        private ISession session;

        /// <summary>
        /// Sets the session.
        /// </summary>
        /// <param name="session">The session.</param>
        public override void SetSession(ISession session)
        {
            this.session = session;
            base.SetSession(session);
        }     

        /// <summary>
        /// Called on flush dirty entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="currentState">State of the current.</param>
        /// <param name="previousState">State of the previous.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <param name="types">The types.</param>
        /// <returns>True if object is dirty and should be flashed.</returns>
        /// <exception cref="NHibernate.StaleObjectStateException">The stale object state exception.</exception>
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, NHibernate.Type.IType[] types)
        {            
            ISessionImplementor sessionImplementation = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImplementation.PersistenceContext;
            EntityEntry entityEntry = persistenceContext.GetEntry(entity);
            IEntityPersister persister = entityEntry.Persister;
            
            if (persister.IsVersioned && entityEntry.ExistsInDatabase)
            {
                EntityMode mode = session.GetSessionImplementation().EntityMode;

                object version = persister.GetVersion(entity, mode);
                object currentVersion = persister.GetCurrentVersion(id, sessionImplementation);

                if (!persister.VersionType.IsEqual(currentVersion, version))
                {
                    throw new ConcurrentDataException((Entity)entity);
                }
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }
    }
}