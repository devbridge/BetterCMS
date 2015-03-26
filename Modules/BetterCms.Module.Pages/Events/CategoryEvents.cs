using System;

using BetterCms.Core.DataContracts;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class PageEvents
    {
        /// <summary>
        /// Occurs when a redirect is created.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryCreated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryCreated
        {
            add
            {
                RootEvents.Instance.CategoryCreated += value;
            }

            remove
            {
                RootEvents.Instance.CategoryCreated -= value;
            }
        }

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryUpdated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryUpdated
        {
            add
            {
                RootEvents.Instance.CategoryUpdated += value;
            }

            remove
            {
                RootEvents.Instance.CategoryUpdated -= value;
            }
        }

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryDeleted instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryDeleted
        {
            add
            {
                RootEvents.Instance.CategoryDeleted += value;
            }

            remove
            {
                RootEvents.Instance.CategoryDeleted -= value;
            }
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryCreated(...) instead.")]
        public void OnCategoryCreated(ICategory category)
        {
            RootEvents.Instance.OnCategoryCreated(category);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryUpdated(...) instead.")]
        public void OnCategoryUpdated(ICategory category)
        {
            RootEvents.Instance.OnCategoryUpdated(category);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryDeleted(...) instead.")]
        public void OnCategoryDeleted(ICategory category)
        {
            RootEvents.Instance.OnCategoryDeleted(category);
        }
    }
}
