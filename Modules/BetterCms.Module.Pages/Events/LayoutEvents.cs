using BetterCms.Module.Root.Models;

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
        /// Occurs when a layout is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Layout>> LayoutCreated;

        /// <summary>
        /// Occurs when a layout is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Layout>> LayoutUpdated;

        /// <summary>
        /// Occurs when a layout is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Layout>> LayoutDeleted;

        public void OnLayoutCreated(Layout layout)
        {
            if (LayoutCreated != null)
            {
                LayoutCreated(new SingleItemEventArgs<Layout>(layout));
            }
        }

        public void OnLayoutUpdated(Layout layout)
        {
            if (LayoutUpdated != null)
            {
                LayoutUpdated(new SingleItemEventArgs<Layout>(layout));
            }
        }

        public void OnLayoutDeleted(Layout layout)
        {
            if (LayoutDeleted != null)
            {
                LayoutDeleted(new SingleItemEventArgs<Layout>(layout));
            }
        }
    }
}
