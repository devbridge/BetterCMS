using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;

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
 
        public void OnTagCreated(params Tag[] tags)
        {
            if (TagCreated != null && tags != null)
            {
                foreach (var tag in tags)
                {
                    TagCreated(new SingleItemEventArgs<Tag>(tag));
                }
            }
        }

        public void OnTagCreated(IEnumerable<Tag> tags)
        {
            if (tags != null)
            {
                OnTagCreated(tags.ToArray());
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
