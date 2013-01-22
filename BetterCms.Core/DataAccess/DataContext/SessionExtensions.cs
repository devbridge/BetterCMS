using System;
using System.Linq;

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

        private static bool ValuesAreEqual(object value1, object value2)
        {
            if (value1 == null)
            {
                return value2 == null;
            }
            return value1.Equals(value2);
        }

        public static Boolean IsDirtyProperty(this ISession session, Object entity, String propertyName)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);
            string className = oldEntry.EntityName;
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);

            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                INHibernateProxy proxy = entity as INHibernateProxy;
                Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }

            Object[] oldState = oldEntry.LoadedState;
            if (oldState == null)
            {
                return false;
            }

            Object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);
            Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
            Int32 index = Array.IndexOf(persister.PropertyNames, propertyName);

            Boolean isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;

            return (isDirty);
        }

        public static Object GetOriginalEntityProperty(this ISession session, Object entity, String propertyName)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);
            string className = oldEntry.EntityName;
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);

            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                INHibernateProxy proxy = entity as INHibernateProxy;
                Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }

            Object[] oldState = oldEntry.LoadedState;
            Object[] currentState = persister.GetPropertyValues(entity, sessionImpl.EntityMode);
            Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
            Int32 index = Array.IndexOf(persister.PropertyNames, propertyName);

            Boolean isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;

            return ((isDirty == true) ? oldState[index] : currentState[index]);
        }
    }
}