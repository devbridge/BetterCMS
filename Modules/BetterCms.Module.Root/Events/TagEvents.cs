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
