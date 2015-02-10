using System;

using Devbridge.Platform.Core.Models;
using Devbridge.Platform.Core.Security;

namespace Devbridge.Platform.Core.DataAccess.DataContext.EventListeners
{
    public class EventListenerHelper
    {
        private readonly IPrincipalProvider principalProvider;    

        /// <summary>
        /// Initializes a new instance of the <see cref="EventListenerHelper" /> class.
        /// </summary>
        /// <param name="principalProvider">The principal provider.</param>
        public EventListenerHelper(IPrincipalProvider principalProvider)
        {
            this.principalProvider = principalProvider;
        }

        /// <summary>
        /// Called when modifying entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void OnModify(object entity)
        {
            var savingEntity = entity as Entity;
            if (savingEntity != null)
            {
                savingEntity.ModifiedOn = DateTime.Now;
                savingEntity.ModifiedByUser = principalProvider.CurrentPrincipalName;
            }
        }

        /// <summary>
        /// Called when creating entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void OnCreate(object entity)
        {
            var savingEntity = entity as Entity;
            if (savingEntity != null)
            {
                savingEntity.CreatedOn = DateTime.Now;
                savingEntity.CreatedByUser = principalProvider.CurrentPrincipalName;
                savingEntity.ModifiedOn = DateTime.Now;
                savingEntity.ModifiedByUser = principalProvider.CurrentPrincipalName;
            }
        }

        /// <summary>
        /// Called when deleting entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void OnDelete(object entity)
        {
            var deletingEntity = entity as Entity;
            if (deletingEntity != null)
            {
                deletingEntity.IsDeleted = true;
                deletingEntity.DeletedOn = DateTime.Now;
                deletingEntity.DeletedByUser = principalProvider.CurrentPrincipalName;
            }
        }
    }
}
