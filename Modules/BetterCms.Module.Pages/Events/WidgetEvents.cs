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
        /// Occurs when a widget is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Widget>> WidgetCreated;

        /// <summary>
        /// Occurs when a widget is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Widget>> WidgetUpdated;

        /// <summary>
        /// Occurs when a widget is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Widget>> WidgetDeleted;

        public void OnWidgetCreated(Widget widget)
        {
            if (WidgetCreated != null)
            {
                WidgetCreated(new SingleItemEventArgs<Widget>(widget));
            }
        }

        public void OnWidgetUpdated(Widget widget)
        {
            if (WidgetUpdated != null)
            {
                WidgetUpdated(new SingleItemEventArgs<Widget>(widget));
            }
        }

        public void OnWidgetDeleted(Widget widget)
        {
            if (WidgetDeleted != null)
            {
                WidgetDeleted(new SingleItemEventArgs<Widget>(widget));
            }        
        }
    }
}
