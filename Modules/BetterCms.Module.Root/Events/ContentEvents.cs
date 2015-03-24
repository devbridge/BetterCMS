using BetterCms.Module.Root.Models;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable content events container.
    /// </summary>
    public partial class RootEvents
    {
        /// <summary>
        /// Occurs when a content is restored.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Content>> ContentRestored;

        public void OnContentRestored(Content category)
        {
            if (ContentRestored != null)
            {
                ContentRestored(new SingleItemEventArgs<Content>(category));
            }
        }
    }
}
