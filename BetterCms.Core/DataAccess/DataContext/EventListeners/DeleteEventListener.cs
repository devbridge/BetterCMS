using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
using BetterCms.Core.Security;

using Iesi.Collections;

using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;

namespace BetterCms.Core.DataAccess.DataContext.EventListeners
{
    /// <summary>
    /// nHibernate Delete Event Listener
    /// </summary>
    public class DeleteEventListener : DefaultDeleteEventListener
    {
        /// <summary>
        /// Event listener helper
        /// </summary>
        private readonly EventListenerHelper eventListenerHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteEventListener" /> class.
        /// </summary>
        /// <param name="eventListenerHelper">The event listener helper.</param>
        public DeleteEventListener(EventListenerHelper eventListenerHelper)
        {
            this.eventListenerHelper = eventListenerHelper;
        }

        /// <summary>
        /// Perform the entity deletion.  Well, as with most operations, does not
        /// really perform it; just schedules an action/execution with the
        /// <see cref="T:NHibernate.Engine.ActionQueue" /> for execution during flush.
        /// </summary>
        /// <param name="session">The originating session</param>
        /// <param name="entity">The entity to delete</param>
        /// <param name="entityEntry">The entity's entry in the <see cref="T:NHibernate.ISession" /></param>
        /// <param name="isCascadeDeleteEnabled">Is delete cascading enabled?</param>
        /// <param name="persister">The entity persister.</param>
        /// <param name="transientEntities">A cache of already deleted entities.</param>
        protected override void DeleteEntity(IEventSource session, object entity, EntityEntry entityEntry, 
            bool isCascadeDeleteEnabled, IEntityPersister persister, ISet transientEntities)
        {
            if (entity is IEntity)
            {                
                Events.CoreEvents.Instance.OnEntityDelete((IEntity)entity);
            }

            if (entity is Entity && !(entity is IDeleteableEntity))
            {
                eventListenerHelper.OnDelete(entity);

                CascadeBeforeDelete(session, persister, entity, entityEntry, transientEntities);
                CascadeAfterDelete(session, persister, entity, transientEntities);
            }
            else
            {
                base.DeleteEntity(session, entity, entityEntry, isCascadeDeleteEnabled, persister, transientEntities);
            }
        }
    }
}
