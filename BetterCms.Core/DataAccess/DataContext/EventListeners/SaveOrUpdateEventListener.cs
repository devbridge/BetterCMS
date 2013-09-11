using BetterCms.Core.DataContracts;

using NHibernate.Event;
using NHibernate.Event.Default;

namespace BetterCms.Core.DataAccess.DataContext.EventListeners
{
    /// <summary>
    /// nHibernate Save Or Update Event Listener
    /// </summary>
    public class SaveOrUpdateEventListener : DefaultSaveOrUpdateEventListener
    {
        /// <summary>
        /// Event listener helper
        /// </summary>
        private readonly EventListenerHelper eventListenerHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveOrUpdateEventListener" /> class.
        /// </summary>
        /// <param name="eventListenerHelper">The event listener helper.</param>
        public SaveOrUpdateEventListener(EventListenerHelper eventListenerHelper)
        {
            this.eventListenerHelper = eventListenerHelper;
        }

        /// <summary>
        /// Performs the save or update.
        /// </summary>
        /// <param name="evt">The save or update event.</param>
        /// <returns>
        /// The id used to save the entity; may be null depending on the
        /// type of id generator used and the requiresImmediateIdAccess value
        /// </returns>
        protected override object PerformSaveOrUpdate(SaveOrUpdateEvent evt)
        {
            if (evt.Entity is IEntity)
            {
                Events.CoreEvents.Instance.OnEntitySaving((IEntity)evt.Entity);
            } 

            if (evt.Session.IsDirtyEntity(evt.Entity))
            {
                eventListenerHelper.OnModify(evt.Entity);
            }

            return base.PerformSaveOrUpdate(evt);
        }

        /// <summary>
        /// Prepares the save call by checking the session caches for a pre-existing
        /// entity and performing any lifecycle callbacks.
        /// </summary>
        /// <param name="entity">The entity to be saved.</param>
        /// <param name="id">The id by which to save the entity.</param>
        /// <param name="persister">The entity's persister instance.</param>
        /// <param name="useIdentityColumn">Is an identity column being used?</param>
        /// <param name="anything">Generally cascade-specific information.</param>
        /// <param name="source">The session from which the event originated.</param>
        /// <param name="requiresImmediateIdAccess">does the event context require
        /// access to the identifier immediately after execution of this method (if
        /// not, post-insert style id generators may be postponed if we are outside
        /// a transaction).</param>
        /// <returns>
        /// The id used to save the entity; may be null depending on the
        /// type of id generator used and the requiresImmediateIdAccess value
        /// </returns>
        protected override object PerformSave(object entity, object id, NHibernate.Persister.Entity.IEntityPersister persister, bool useIdentityColumn, object anything, IEventSource source, bool requiresImmediateIdAccess)
        {
            if (entity is IEntity)
            {
                Events.CoreEvents.Instance.OnEntitySaving((IEntity)entity);
            }

            eventListenerHelper.OnCreate(entity);

            return base.PerformSave(entity, id, persister, useIdentityColumn, anything, source, requiresImmediateIdAccess);
        }

        /// <summary>
        /// Performs the update.
        /// </summary>
        /// <param name="evt">The event.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="persister">The entity's persister.</param>
        protected override void PerformUpdate(SaveOrUpdateEvent evt, object entity, NHibernate.Persister.Entity.IEntityPersister persister)
        {
            if (entity is IEntity)
            {
                Events.CoreEvents.Instance.OnEntitySaving((IEntity)entity);
            }

            if (evt.Session.IsDirtyEntity(entity))
            {
                eventListenerHelper.OnModify(entity);
            }
            
            base.PerformUpdate(evt, entity, persister);
        }
    }
}