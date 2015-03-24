using System;
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
    public partial class PageEvents
    {
        /// <summary>
        /// Occurs when a redirect is created.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.TagCreated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<Tag>> TagCreated
        {
            add
            {
                RootEvents.Instance.TagCreated += value;
            }

            remove
            {
                RootEvents.Instance.TagCreated -= value;
            }
        }

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.TagUpdated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<Tag>> TagUpdated
        {
            add
            {
                RootEvents.Instance.TagUpdated += value;
            }

            remove
            {
                RootEvents.Instance.TagUpdated -= value;
            }
        }

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.TagDeleted instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<Tag>> TagDeleted
        {
            add
            {
                RootEvents.Instance.TagDeleted += value;
            }

            remove
            {
                RootEvents.Instance.TagDeleted -= value;
            }
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnTagCreated(...) instead.")]
        public void OnTagCreated(params Tag[] tags)
        {
            RootEvents.Instance.OnTagCreated(tags);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnTagCreated(...) instead.")]
        public void OnTagCreated(IEnumerable<Tag> tags)
        {
            if (tags != null)
            {
                RootEvents.Instance.OnTagCreated(tags.ToArray());
            }
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnTagUpdated(...) instead.")]
        public void OnTagUpdated(Tag tag)
        {
            RootEvents.Instance.OnTagUpdated(tag);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnTagDeleted(...) instead.")]
        public void OnTagDeleted(Tag tag)
        {
            RootEvents.Instance.OnTagDeleted(tag);
        }
    }
}
