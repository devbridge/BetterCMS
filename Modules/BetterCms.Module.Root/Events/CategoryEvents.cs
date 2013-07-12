using BetterCms.Module.Root.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class RootEvents
    {
        /// <summary>
        /// Occurs when a redirect is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryCreated;

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryUpdated;

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryDeleted;

        public void OnCategoryCreated(Category category)
        {
            if (CategoryCreated != null)
            {
                CategoryCreated(new SingleItemEventArgs<Category>(category));
            }
        }

        public void OnCategoryUpdated(Category category)
        {
            if (CategoryUpdated != null)
            {
                CategoryUpdated(new SingleItemEventArgs<Category>(category));
            }
        }

        public void OnCategoryDeleted(Category category)
        {
            if (CategoryDeleted != null)
            {
                CategoryDeleted(new SingleItemEventArgs<Category>(category));
            }        
        }
    }
}
