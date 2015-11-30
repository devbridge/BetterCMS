// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagEvents.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
