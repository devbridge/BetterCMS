using BetterCms.Module.Root.Models;

using BetterModules.Core.Events;

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
        public event DefaultEventHandler<SingleItemEventArgs<CategoryTree>> CategoryTreeCreated;

        /// <summary>
        /// Occurs when a category tree is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<CategoryTree>> CategoryTreeUpdated;

        /// <summary>
        /// Occurs when a category tree is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<CategoryTree>> CategoryTreeDeleted;

        /// <summary>
        /// Occurs when a category node is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryCreated;

        /// <summary>
        /// Occurs when a category node is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryUpdated;

        /// <summary>
        /// Occurs when a category node is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Category>> CategoryDeleted;

        public void OnCategoryTreeCreated(CategoryTree categoryTree)
        {
            if (CategoryTreeCreated != null)
            {
                CategoryTreeCreated(new SingleItemEventArgs<CategoryTree>(categoryTree));
            }
        }

        public void OnCategoryTreeUpdated(CategoryTree categoryTree)
        {
            if (CategoryTreeUpdated != null)
            {
                CategoryTreeUpdated(new SingleItemEventArgs<CategoryTree>(categoryTree));
            }
        }

        public void OnCategoryTreeDeleted(CategoryTree categoryTree)
        {
            if (CategoryTreeDeleted != null)
            {
                CategoryTreeDeleted(new SingleItemEventArgs<CategoryTree>(categoryTree));
            }        
        }
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
