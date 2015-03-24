using BetterCms.Module.Pages.Models;

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
        public event DefaultEventHandler<SingleItemEventArgs<Redirect>> RedirectCreated;

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Redirect>> RedirectUpdated;

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Redirect>> RedirectDeleted; 
 
        public void OnRedirectCreated(Redirect redirect)
        {
            if (RedirectCreated != null)
            {
                RedirectCreated(new SingleItemEventArgs<Redirect>(redirect));
            }
        }

        public void OnRedirectUpdated(Redirect redirect)
        {
            if (RedirectUpdated != null)
            {
                RedirectUpdated(new SingleItemEventArgs<Redirect>(redirect));
            }
        }

        public void OnRedirectDeleted(Redirect redirect)
        {
            if (RedirectDeleted != null)
            {
                RedirectDeleted(new SingleItemEventArgs<Redirect>(redirect));
            }        
        }
    }
}
