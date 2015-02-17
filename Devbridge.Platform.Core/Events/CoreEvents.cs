using Devbridge.Platform.Core.DataContracts;

using NHibernate;

// ReSharper disable CheckNamespace
namespace Devbridge.Platform.Events
// ReSharper restore CheckNamespace
{
    public class CoreEvents : EventsBase<CoreEvents>
    {
        public event DefaultEventHandler<EntitySavingEventArgs> EntitySaving;

        public event DefaultEventHandler<SingleItemEventArgs<IEntity>> EntityDeleting;

        /// <summary>
        /// Called before an entity is saved.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="nHibernateSession">The n hibernate session.</param>
        public void OnEntitySaving(IEntity entity, ISession nHibernateSession)
        {
            if (EntitySaving != null)
            {
                EntitySaving(new EntitySavingEventArgs(entity, nHibernateSession));
            }
        }

        /// <summary>
        /// Called before an entity is deleted.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void OnEntityDelete(IEntity entity)
        {
            if (EntityDeleting != null)
            {
                EntityDeleting(new SingleItemEventArgs<IEntity>(entity));
            }
        }  
    }
}
