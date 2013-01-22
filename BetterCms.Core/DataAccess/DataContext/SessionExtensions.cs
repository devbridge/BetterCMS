using System;
using System.Linq;

using BetterCms.Core.Models;

using NHibernate;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;

namespace BetterCms.Core.DataAccess.DataContext
{
    public static class SessionExtensions
    {
        public static Boolean IsDirtyEntity(this ISession session, Object entity)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);

            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                INHibernateProxy proxy = entity as INHibernateProxy;
                Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }

            if (oldEntry == null)
            {
                return false;
            }

            string className = oldEntry.EntityName;
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);

            Object[] oldState = oldEntry.LoadedState;
            if (oldState == null)
            {
                return false;
            }

            Object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);
            Int32[] dirtyProps = oldState.Select((o, i) => ValuesAreEqual(oldState[i], currentState[i]) ? -1 : i).Where(x => x >= 0).ToArray();

            return (dirtyProps != null && dirtyProps.Length > 0);
        }

        /// <summary>
        /// Checks, if values the are equal.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns><c>true</c>, if values are equal, else <c>false</c></returns>
        private static bool ValuesAreEqual(object oldValue, object currentValue)
        {
            // If property is not loaded, it has no changes
            if (!NHibernateUtil.IsInitialized(oldValue))
            {
                return true;
            }

            if (oldValue == null)
            {
                return currentValue == null;
            }
            return oldValue.Equals(currentValue);
        }
    }
}