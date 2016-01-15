// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryEvents.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;

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
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryCreated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryCreated
        {
            add
            {
                RootEvents.Instance.CategoryCreated += value;
            }

            remove
            {
                RootEvents.Instance.CategoryCreated -= value;
            }
        }

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryUpdated instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryUpdated
        {
            add
            {
                RootEvents.Instance.CategoryUpdated += value;
            }

            remove
            {
                RootEvents.Instance.CategoryUpdated -= value;
            }
        }

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        [Obsolete("This event is obsolete; use method RootApiContext.Events.CategoryDeleted instead.")]
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryDeleted
        {
            add
            {
                RootEvents.Instance.CategoryDeleted += value;
            }

            remove
            {
                RootEvents.Instance.CategoryDeleted -= value;
            }
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryCreated(...) instead.")]
        public void OnCategoryCreated(ICategory category)
        {
            RootEvents.Instance.OnCategoryCreated(category);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryUpdated(...) instead.")]
        public void OnCategoryUpdated(ICategory category)
        {
            RootEvents.Instance.OnCategoryUpdated(category);
        }

        [Obsolete("This method is obsolete; use method RootApiContext.Events.OnCategoryDeleted(...) instead.")]
        public void OnCategoryDeleted(ICategory category)
        {
            RootEvents.Instance.OnCategoryDeleted(category);
        }
    }
}
