using System;

using BetterCms.Api;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Api.Events
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class PagesApiEvents
    {
        /// <summary>
        /// Occurs when a redirect is created.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryCreated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryCreated
        {
            add
            {
                RootApiContext.Events.CategoryCreated += value;
            }

            remove
            {
                RootApiContext.Events.CategoryCreated -= value;
            }
        }

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryUpdated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryUpdated
        {
            add
            {
                RootApiContext.Events.CategoryUpdated += value;
            }

            remove
            {
                RootApiContext.Events.CategoryUpdated -= value;
            }
        }

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryDeleted instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryDeleted
        {
            add
            {
                RootApiContext.Events.CategoryDeleted += value;
            }

            remove
            {
                RootApiContext.Events.CategoryDeleted -= value;
            }
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryCreated(...) instead.")]
        public void OnCategoryCreated(Category category)
        {
            RootApiContext.Events.OnCategoryCreated(category);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryUpdated(...) instead.")]
        public void OnCategoryUpdated(Category category)
        {
            RootApiContext.Events.OnCategoryUpdated(category);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryDeleted(...) instead.")]
        public void OnCategoryDeleted(Category category)
        {
            RootApiContext.Events.OnCategoryDeleted(category);
        }
    }
}
