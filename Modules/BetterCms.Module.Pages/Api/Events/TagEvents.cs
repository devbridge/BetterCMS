using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Occurs when a redirect is created.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.TagCreated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<Tag>> TagCreated
        {
            add
            {
                RootApiContext.Events.TagCreated += value;
            }

            remove
            {
                RootApiContext.Events.TagCreated -= value;
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
                RootApiContext.Events.TagUpdated += value;
            }

            remove
            {
                RootApiContext.Events.TagUpdated -= value;
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
                RootApiContext.Events.TagDeleted += value;
            }

            remove
            {
                RootApiContext.Events.TagDeleted -= value;
            }
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnTagCreated(...) instead.")]
        public void OnTagCreated(params Tag[] tags)
        {
            RootApiContext.Events.OnTagCreated(tags);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnTagCreated(...) instead.")]
        public void OnTagCreated(IEnumerable<Tag> tags)
        {
            if (tags != null)
            {
                RootApiContext.Events.OnTagCreated(tags.ToArray());
            }
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnTagUpdated(...) instead.")]
        public void OnTagUpdated(Tag tag)
        {
            RootApiContext.Events.OnTagUpdated(tag);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnTagDeleted(...) instead.")]
        public void OnTagDeleted(Tag tag)
        {
            RootApiContext.Events.OnTagDeleted(tag);
        }
    }
}
