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
        /// <param name="accessSecuredObject">The access secured object.</param>
        /// <param name="nHibernateSession">The n hibernate session.</param>
        public void OnEntitySaving(IEntity accessSecuredObject, ISession nHibernateSession)
        {
            if (EntitySaving != null)
            {
                EntitySaving(new EntitySavingEventArgs(accessSecuredObject, nHibernateSession));
            }
        }

        /// <summary>
        /// Called before an entity is deleted.
        /// </summary>
        /// <param name="accessSecuredObject">The access secured object.</param>
        public void OnEntityDelete(IEntity accessSecuredObject)
        {
            if (EntityDeleting != null)
            {
                EntityDeleting(new SingleItemEventArgs<IEntity>(accessSecuredObject));
            }
        }  
    }
}
