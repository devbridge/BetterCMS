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
