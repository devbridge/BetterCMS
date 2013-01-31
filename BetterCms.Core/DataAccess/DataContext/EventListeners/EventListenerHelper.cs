using System;

using BetterCms.Core.Models;
using BetterCms.Core.Services;

namespace BetterCms.Core.DataAccess.DataContext.EventListeners
{
    public class EventListenerHelper
    {
        private readonly ISecurityService securityService;    

        /// <summary>
        /// Initializes a new instance of the <see cref="EventListenerHelper" /> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        public EventListenerHelper(ISecurityService securityService)
        {
            this.securityService = securityService;
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
                savingEntity.ModifiedByUser = securityService.CurrentPrincipalName;
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
                savingEntity.CreatedByUser = securityService.CurrentPrincipalName;
                savingEntity.ModifiedOn = DateTime.Now;
                savingEntity.ModifiedByUser = securityService.CurrentPrincipalName;
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
                deletingEntity.DeletedByUser = securityService.CurrentPrincipalName;
            }
        }
    }
}
