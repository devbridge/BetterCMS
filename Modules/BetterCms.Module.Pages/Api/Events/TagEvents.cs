using System;

using BetterCms.Api;
using BetterCms.Module.Pages.Models;
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
        public event DefaultEventHandler<SingleItemEventArgs<Tag>> TagCreated;

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Tag>> TagUpdated;

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Tag>> TagDeleted; 
 
        public void OnTagCreated(Tag tag)
        {
            if (TagCreated != null)
            {
                TagCreated(new SingleItemEventArgs<Tag>(tag));
            }
        }

        public void OnTagUpdated(Tag tag)
        {
            if (TagUpdated != null)
            {
                TagUpdated(new SingleItemEventArgs<Tag>(tag));
            }
        }

        public void OnTagDeleted(Tag tag)
        {
            if (TagDeleted != null)
            {
                TagDeleted(new SingleItemEventArgs<Tag>(tag));
            }        
        }
    }
}
