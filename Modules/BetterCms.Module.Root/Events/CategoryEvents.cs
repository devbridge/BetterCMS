using BetterCms.Core.DataContracts;

using BetterModules.Events;

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
        /// Occurs when a category tree is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategoryTree>> CategoryTreeCreated;

        /// <summary>
        /// Occurs when a category tree is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategoryTree>> CategoryTreeUpdated;

        /// <summary>
        /// Occurs when a category tree is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategoryTree>> CategoryTreeDeleted;

        /// <summary>
        /// Occurs when a category node is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryCreated;

        /// <summary>
        /// Occurs when a category node is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryUpdated;

        /// <summary>
        /// Occurs when a category node is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryDeleted;

        public void OnCategoryTreeCreated(ICategoryTree categoryTree)
        {
            if (CategoryTreeCreated != null)
            {
                CategoryTreeCreated(new SingleItemEventArgs<ICategoryTree>(categoryTree));
            }
        }

        public void OnCategoryTreeUpdated(ICategoryTree categoryTree)
        {
            if (CategoryTreeUpdated != null)
            {
                CategoryTreeUpdated(new SingleItemEventArgs<ICategoryTree>(categoryTree));
            }
        }

        public void OnCategoryTreeDeleted(ICategoryTree categoryTree)
        {
            if (CategoryTreeDeleted != null)
            {
                CategoryTreeDeleted(new SingleItemEventArgs<ICategoryTree>(categoryTree));
            }        
        }
        public void OnCategoryCreated(ICategory category)
        {
            if (CategoryCreated != null)
            {
                CategoryCreated(new SingleItemEventArgs<ICategory>(category));
            }
        }

        public void OnCategoryUpdated(ICategory category)
        {
            if (CategoryUpdated != null)
            {
                CategoryUpdated(new SingleItemEventArgs<ICategory>(category));
            }
        }

        public void OnCategoryDeleted(ICategory category)
        {
            if (CategoryDeleted != null)
            {
                CategoryDeleted(new SingleItemEventArgs<ICategory>(category));
            }        
        }
    }
}
